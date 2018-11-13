using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;

using MGI.Core.Compliance.Contract;
using MGI.Core.Compliance.Data;

using MGI.Core.Partner.Contract;
//using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Biz.Compliance.Impl
{
	public class LimitServiceImpl : ILimitService
	{

		#region Dependencies

		private IComplianceTransactionService _txnSvc;
		public IComplianceTransactionService ComplianceTransactionService { set { _txnSvc = value; } }

		private ICustomerSessionService _ptnrCustSessSvc;
		public ICustomerSessionService PartnerCustomerSessionService { set { _ptnrCustSessSvc = value; } }

		private IComplianceProgramService _coreComplianceProgramService;
		public IComplianceProgramService CoreComplianceProgramService { set { _coreComplianceProgramService = value; } }

		public MGI.Common.Util.NLoggerCommon NLogger { private get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }

		#endregion

		#region Interface Implementation

		public decimal CalculateTransactionMaximumLimit(long customerSessionId, string complianceProgramName,
			TransactionTypes transactionType, MGIContext mgiContext)
		{
			return CalculateTransactionLimits(customerSessionId, complianceProgramName, transactionType, mgiContext);
		}

		public decimal GetProductMinimum(string complianceProgramName, TransactionTypes transactionTypes, MGIContext mgiContext)
		{
			//AL-1014 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Compliance Program:"+complianceProgramName);
			details.Add("Transaction Type:"+Convert.ToString(transactionTypes));
			MongoDBLogger.ListInfo<string>(mgiContext.CustomerSessionId, details, "GetProductMinimum", AlloyLayerName.BIZ,
				ModuleName.Transaction, "Begin GetProductMinimum - MGI.Biz.Compliance.Impl.LimitServiceImpl",
				mgiContext);

			Limit limit = GetChannelPartnerComplianceLimit(complianceProgramName, transactionTypes);
			
			//AL-1014 Transactional Log User Story
			MongoDBLogger.Info<Limit>(mgiContext.CustomerSessionId, limit, "GetProductMinimum", AlloyLayerName.BIZ,
				ModuleName.Transaction, "End GetProductMinimum - MGI.Biz.Compliance.Impl.LimitServiceImpl",
				mgiContext);

			return limit.PerTransactionMinimum.GetValueOrDefault(0.00M);
		}

		#endregion

		#region Private methods
		private decimal CalculateTransactionLimits(long customerSessionId, string complianceProgramName,
												TransactionTypes transactionType, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			List<string> details = new List<string>();
			details.Add("Compliance ProgramName : " + complianceProgramName);
			details.Add("Transaction Type : " + Convert.ToString(transactionType));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "CalculateTransactionLimits", AlloyLayerName.BIZ,
				ModuleName.Transaction, "Begin CalculateTransactionLimits - MGI.Biz.Compliance.Impl.LimitServiceImpl",
				mgiContext);
			#endregion

			CustomerSession partnerCustomerSession = _ptnrCustSessSvc.Lookup(customerSessionId);
			long customerId = partnerCustomerSession.Customer.Id;

			List<ComplianceTransaction> complianceTransactions = _txnSvc.Get(customerId);

			//Adding current Shopping cart items to complianceTransactions list
			if (mgiContext.ShouldIncludeShoppingCartItems && partnerCustomerSession.ActiveShoppingCart != null)
			{
				List<ShoppingCartTransaction> cartTransactions = partnerCustomerSession.ActiveShoppingCart.ShoppingCartTransactions.Where(x => x.CartItemStatus == ShoppingCartItemStatus.Added).ToList();
				complianceTransactions.AddRange(PopulateComplianceTransactions(cartTransactions));
			}

			Limit limit = GetChannelPartnerComplianceLimit(complianceProgramName, transactionType);

			decimal maxAmount = GetMaximumLimit(transactionType, complianceTransactions, limit);
			decimal maximumAmount = maxAmount < limit.PerTransactionMinimum ? 0 : maxAmount;

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(maximumAmount), "CalculateTransactionLimits", AlloyLayerName.BIZ,
				ModuleName.Transaction, "End CalculateTransactionLimits - MGI.Biz.Compliance.Impl.LimitServiceImpl",
				mgiContext);
			#endregion

			return maximumAmount;
		}

		private Limit GetChannelPartnerComplianceLimit(string complianceProgramName, TransactionTypes transactionType)
		{
			ComplianceProgram complianceProgram = _coreComplianceProgramService.Get(complianceProgramName);

			if (complianceProgram == null)
				throw new BizComplianceException(BizComplianceException.COMPLIANCE_PROGRAM_NOT_FOUND);

			LimitType limitType = complianceProgram.LimitTypes.FirstOrDefault(x => x.Name == transactionType.ToString());
			Limit limit = new Limit();

			if (limitType != null)
			{
				limit = limitType.Limits.Count == 0 ? new Limit() : limitType.Limits.First();
			}

			return limit;
		}

		private decimal GetMaximumLimit(TransactionTypes transactionType, List<ComplianceTransaction> complianceTransactions, Limit limit)
		{
			decimal maxPossibleAmount = limit.PerTransactionMaximum.GetValueOrDefault(decimal.MaxValue);

			decimal xDayTrxsTotalAmount = decimal.MinValue;

			foreach (var xDayLimit in limit.NDaysLimit)
			{
				if (maxPossibleAmount != 0)
				{
					xDayTrxsTotalAmount = GetPeriodOfTransactionsAmount(complianceTransactions, transactionType, xDayLimit.Key);
					var maxAmount = xDayLimit.Value - xDayTrxsTotalAmount < 0 ? 0 : xDayLimit.Value - xDayTrxsTotalAmount;

					if (maxPossibleAmount > maxAmount)
					{
						maxPossibleAmount = maxAmount;
					}
					if (maxPossibleAmount == 0 || maxPossibleAmount < limit.PerTransactionMinimum)
					{
						NLogger.Info(string.Format(" Limit for {0} day(s) exceeded by {1}", xDayLimit.Key, Math.Abs(xDayLimit.Value - xDayTrxsTotalAmount)));
					}
				}
			}
			return maxPossibleAmount;
		}

		private decimal GetPeriodOfTransactionsAmount(List<ComplianceTransaction> complianceTransactions, TransactionTypes transactionType, int period)
		{
			DateTime dateRange = DateTime.Now.Date.AddDays(-(period - 1));
			complianceTransactions = complianceTransactions.Where(x => x.DTTerminalCreate >= dateRange).ToList();

			switch (transactionType)
			{
				case TransactionTypes.Cash:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.Cash);
					break;
				case TransactionTypes.Check:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.Check);
					break;
				case TransactionTypes.Funds:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.Funds);
					break;
				case TransactionTypes.BillPay:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.BillPay);
					break;
				case TransactionTypes.MoneyOrder:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.MoneyOrder);
					break;
				case TransactionTypes.MoneyTransfer:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.MoneyTransfer);
					break;
				case TransactionTypes.CashWithdrawal:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.CashWithdrawal);
					break;
				case TransactionTypes.LoadToGPR:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.LoadToGPR);
					break;
				case TransactionTypes.ActivateGPR:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.ActivateGPR);
					break;
				case TransactionTypes.DebitGPR:
					complianceTransactions = complianceTransactions.FindAll(x => x.TransactionType == (int)TransactionTypes.DebitGPR);
					break;
			}
			return complianceTransactions.Sum(o => o.Amount);
		}

		private List<ComplianceTransaction> PopulateComplianceTransactions(List<ShoppingCartTransaction> cartTransactions)
		{
			List<Transaction> transactions = new List<Transaction>();
			List<ComplianceTransaction> complianceTransactions = new List<ComplianceTransaction>();
			if (cartTransactions != null)
			{
				transactions = cartTransactions.FindAll(t => t.CartItemStatus == ShoppingCartItemStatus.Added).ConvertAll<Transaction>(t => (Transaction)t.Transaction);
			}
			foreach (Transaction transaction in transactions)
			{
				ComplianceTransaction complianceTransaction = new ComplianceTransaction()
				{
					rowguid = transaction.rowguid,
					TransactionId = transaction.Id,
					TransactionType = (int)transaction.Type,
					Amount = transaction.Amount,
					Fee = transaction.Fee,
					State = transaction.CXEState,
					DTTerminalCreate = transaction.DTTerminalCreate
				};
				complianceTransactions.Add(complianceTransaction);
			}
			return complianceTransactions;
		}
		#endregion
	}
}
