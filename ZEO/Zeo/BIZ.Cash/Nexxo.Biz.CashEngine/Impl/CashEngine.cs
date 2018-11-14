using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Biz.CashEngine.Contract;

using MGI.Core.CXE.Data.Transactions.Commit;
using MGI.Core.CXE.Contract;
using CxeAccount = MGI.Core.CXE.Data.Account;
using MGI.Common.DataAccess.Contract;
using MGI.Core.CXE.Data;
using MGI.Biz.CashEngine.Data;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using CxeCustomer = MGI.Core.CXE.Data.Customer;

using CxeCash = MGI.Core.CXE.Data.Transactions.Stage.Cash;

using PTNRCash = MGI.Core.Partner.Data.Transactions.Cash;
using PTNRCashService = MGI.Core.Partner.Contract.ITransactionService<MGI.Core.Partner.Data.Transactions.Cash>;

using AutoMapper;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Biz.CashEngine.Impl
{
	public class CashEngine : ICashEngine
	{
        public CashEngine()
		{
            NLogger = new NLoggerCommon();
        }

		private ICashService _cashService;

		public ICashService CashService
		{
			set { _cashService = value; }
		}
        public NLoggerCommon NLogger { get; set; }
		public ICustomerSessionService CustomerSessionService { private get; set; }
		public MGI.Core.CXE.Contract.ICustomerService CashCustomerService { private get; set; }
		public PTNRCashService PTNRCashService { private get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="amount"></param>
		/// <param name="cashTrxType"></param>
		/// <returns></returns>
		private long RecordTransaction(CustomerSession customerSession, decimal amount, CashTransactionType cashTrxType, TransactionStates txnState)
		{
			MGI.Core.CXE.Data.Customer cxeCustomer = CashCustomerService.Lookup(customerSession.Customer.CXEId);
			MGI.Core.CXE.Data.Account cxeCashAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)MGI.Core.CXE.Data.AccountTypes.Cash);
			CxeCash cash = new CxeCash()
			{
				Account = cxeCashAccount,
				Amount = amount,
                //Changes for timestamp
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.AgentSession.Terminal.Location.TimezoneID),
                DTServerCreate = DateTime.Now,
                DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.AgentSession.Terminal.Location.TimezoneID),
                DTServerLastModified = DateTime.Now,
				rowguid = Guid.NewGuid(),
				CashTrxType = (int)cashTrxType
			};

			long id = _cashService.Create(cash);
			_WritePTNRCash(id, amount, (int)cashTrxType, customerSession, cxeCashAccount, txnState);
			return id;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cxeCashId"></param>
		/// <param name="amount"></param>
		/// <param name="CashType"></param>
		/// <param name="custSession"></param>
		/// <param name="cxeCashAccount"></param>
		private void _WritePTNRCash(long cxeCashId, decimal amount, int CashType, CustomerSession custSession, MGI.Core.CXE.Data.Account cxeCashAccount, TransactionStates txnState)
		{
			//There is no confirmation number for cash transaction checked with Vinay
			PTNRCash ptnrCash = new PTNRCash
			{
				Id = cxeCashId,
				CXEId = cxeCashId,
				//sending cxe id as cash does not have cxn
				CXNId = cxeCashId,
				Amount = amount,
				//sending 0 as fees as cash does not have fees
				Fee = 0,
				CustomerSession = custSession,
				CXEState = (int)txnState,
				CXNState = (int)TransactionStates.Committed,
			    //Changes for timestamp
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(custSession.AgentSession.Terminal.Location.TimezoneID),
                DTServerCreate = DateTime.Now,
				Account = custSession.Customer.FindAccountByCXEId(cxeCashAccount.Id),
				CashType = CashType,
				ConfirmationNumber = string.Empty
			};

			PTNRCashService.Create(ptnrCash);
		}

		/// <summary>
		/// Stage the cash in transaction
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public long CashIn(long customerSessionId, decimal amount, MGIContext mgiContext)
		{
            CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
			return RecordTransaction(customerSession, amount, CashTransactionType.CashIn, TransactionStates.Authorized);
		}

		/// <summary>
		/// Stage and commit the CashOut transaction
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public long CashOut(long customerSessionId, decimal amount, MGIContext mgiContext)
		{
            CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
			return RecordTransaction(customerSession, amount, CashTransactionType.CashOut, TransactionStates.Committed);
            
            //In case of cashout do the commit
		}

		/// <summary>
		/// Commit the CashIn transaction.
		/// </summary>
		/// <param name="cxeTxnId"></param>
		/// <returns></returns>
		public int Commit(long customerSessionId, long cxeTxnId, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cxeTxnId), "Commit", AlloyLayerName.BIZ, ModuleName.ShoppingCart,
									  "Begin Commit-MGI.Biz.CashEngine.Impl.CashEngine", mgiContext);
			#endregion
			_cashService.Commit(cxeTxnId, mgiContext.TimeZone);
			PTNRCashService.UpdateCXEStatus(cxeTxnId, (int)TransactionStates.Committed);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(TransactionStates.Committed), "Commit", AlloyLayerName.BIZ, ModuleName.ShoppingCart,
									  "End Commit-MGI.Biz.CashEngine.Impl.CashEngine", mgiContext);
			#endregion
			return (int)TransactionStates.Committed;
		}

		/// <summary>
		/// Cancel the CashIn transaction
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="cashInId"></param>
		/// <param name="mgiContext"></param>
		public void Cancel(long customerSessionId, long cashInId, MGIContext mgiContext)
		{
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cashInId), "Cancel", AlloyLayerName.BIZ,
				ModuleName.CashIn, "Begin Cancel - MGI.Biz.CashEngine.Impl.CashEngine",
				mgiContext);
			#endregion
			PTNRCash ptnrCash = PTNRCashService.Lookup(cashInId);

			// Update CXE Space
			_cashService.Update(ptnrCash.CXEId, TransactionStates.Canceled, mgiContext.TimeZone);

			// Update Partner Space
			PTNRCashService.UpdateStates(ptnrCash.Id, (int)TransactionStates.Canceled, (int)TransactionStates.Canceled);
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "Cancel", AlloyLayerName.BIZ,
				ModuleName.CashIn, "End Cancel - MGI.Biz.CashEngine.Impl.CashEngine",
				mgiContext);
			#endregion

		}

		//AL-2729 user story for updating cash in transaction
        public long Update(long customerSessionId, long trxId, decimal amount, MGIContext mgiContext)
		{
			#region AL-3370 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Transaction Id: " + Convert.ToString(trxId));
			details.Add("Amount: " + Convert.ToString(amount));
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "Update", AlloyLayerName.BIZ,
				ModuleName.CashIn, "Begin Update - MGI.Biz.CashEngine.Impl.CashEngine",
				mgiContext);
			#endregion
            CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
			
			CxeCustomer cxeCustomer = CashCustomerService.Lookup(customerSession.Customer.CXEId);
			CxeAccount _cxeAccount = cxeCustomer.Accounts.FirstOrDefault(a => a.Type == (int)AccountTypes.Cash);

            var ptnrCash = PTNRCashService.Lookup(trxId);

            _cashService.UpdateAmount(ptnrCash.CXEId, amount, mgiContext.TimeZone);

            // Update Partner Space
			PTNRCashService.UpdateAmount(ptnrCash.Id, amount);

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "Update", AlloyLayerName.BIZ,
				ModuleName.CashIn, "End Update - MGI.Biz.CashEngine.Impl.CashEngine",
				mgiContext);
			#endregion
            return trxId;
		}
	}
}
