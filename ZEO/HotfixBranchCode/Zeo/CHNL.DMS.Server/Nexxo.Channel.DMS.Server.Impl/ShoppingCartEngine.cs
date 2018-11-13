using System;
using System.Linq;
using System.Collections.Generic;
using Spring.Transaction.Interceptor;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;

using bizShoppingCartService = MGI.Biz.Partner.Contract.IShoppingCartService;

using ShoppingCartImplDTO = MGI.Channel.Shared.Server.Data.ShoppingCart;

using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Data.Transactions;
using MGI.Biz.MoneyTransfer.Data;

using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.Contract;
using CxnData = MGI.Cxn.BillPay.Data;
using CxnContract = MGI.Cxn.BillPay.Contract;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.Check.Contract;

using bizShoppingCart = MGI.Biz.Partner.Data.ShoppingCart;
using bizFunds = MGI.Biz.Partner.Data.Transactions.Funds;
using bizCustomerSession = MGI.Biz.Customer.Data.CustomerSession;
using bizCheck = MGI.Biz.Partner.Data.Transactions.Check;
using bizBills = MGI.Biz.Partner.Data.Transactions.BillPay;
using bizMoneyTransfer = MGI.Biz.Partner.Data.Transactions.MoneyTransfer;
using bizCash = MGI.Biz.Partner.Data.Transactions.Cash;
using bizMoneyOrder = MGI.Biz.Partner.Data.Transactions.MoneyOrder;
using bizParkedTransaction = MGI.Biz.Partner.Data.ParkedTransaction;
using ParkedTransactionDTO = MGI.Channel.Shared.Server.Data.ParkedTransaction;

using CXNFundsType = MGI.Cxn.Fund.Data.RequestType;
using MGI.Cxn.Check.Data;
using AutoMapper;

using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : Contract.IShoppingCartService
	{
		bizShoppingCartService BIZShoppingCartService { get; set; }
		public ICheckProcessor ChexarGateway { get; set; }
		private IDesktopService Self;

		/// <summary>
		/// 
		/// </summary>
		internal static void ShoppingCartConverter()
		{

		}

		#region IShoppingCartService Impl

		#region Shared Methods

		[Transaction()]
        public bool RemoveCheck(long customerSessionId, long checkId, bool isParkedTransaction, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.RemoveCheck(customerSessionId, checkId, isParkedTransaction, context);

			//BIZShoppingCartService.RemoveCheck(customerSessionId, checkId);
			//CPEngineService.Cancel(customerSessionId.ToString(), checkId.ToString());
		}   
		[Transaction()]
		public void RemoveCheckFromCart(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			SharedEngine.RemoveCheckFromCart(customerSessionId, checkId, context);
		}
		[Transaction()]
		public void AddMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BIZShoppingCartService.AddMoneyTransfer(customerSessionId, moneyTransferId, context);
		}

		[Transaction()]
		public void AddCash(long customerSessionId, long cashTxnId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BIZShoppingCartService.AddCash(customerSessionId, cashTxnId, context);
		}

		[Transaction()]
        public void RemoveFunds(long customerSessionId, long fundsId, bool isParkedTransaction, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizFundsService.Cancel(customerSessionId, fundsId, context);

			SharedEngine.RemoveFunds(customerSessionId, fundsId, isParkedTransaction, context);
		}

		[Transaction()]
        public void RemoveBillPay(long customerSessionId, long billPayId, bool isParkedTransaction, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			SharedEngine.RemoveBillPay(customerSessionId, billPayId, isParkedTransaction, context);
		}

		[Transaction()]
        public void RemoveMoneyOrder(long customerSessionId, long moneyOrderId, bool isParkedTransaction, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			List<string> details = new List<string>();
			details.Add("MoneyOrder Id : " + moneyOrderId);
			details.Add("Is Parked Transaction : " + (isParkedTransaction ? "Yes" : "No"));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin RemoveMoneyOrder - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion
			
			SharedEngine.RemoveMoneyOrder(customerSessionId, moneyOrderId, isParkedTransaction, context);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End RemoveMoneyOrder - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion

			//BIZShoppingCartService.RemoveMoneyOrder(customerSessionId, moneyOrderId);
		}

		[Transaction()]
        public void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, bool isParkedTransaction, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			SharedEngine.RemoveMoneyTransfer(customerSessionId, moneyTransferId, isParkedTransaction, context);
		}

		[Transaction()]
		public void RemoveCashIn(long customerSessionId, long cashId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			SharedEngine.RemoveCashIn(customerSessionId, cashId, context);
		}

		[Transaction()]
		public ShoppingCartImplDTO ShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.ShoppingCart(customerSessionId, context);
		}

		[Transaction()]
		public SharedData.ShoppingCartCheckoutStatus Checkout(long customerSessionId, decimal cashToCustomer,  string cardNumber, SharedData.ShoppingCartCheckoutStatus shoppingCartstatus, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.Checkout(customerSessionId, cashToCustomer, cardNumber, shoppingCartstatus, context);
		}

		[Transaction()]
		public SharedData.Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return SharedEngine.GenerateReceiptsForShoppingCart(customerSessionId, shoppingCartId, context);
		}

		[Transaction()]
		public void CloseShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "CloseShoppingCart", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin CloseShoppingCart - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion

			SharedEngine.CloseShoppingCart(customerSessionId, context);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "CloseShoppingCart", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End CloseShoppingCart - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion


			//BIZShoppingCartService.CloseShoppingCart(customerSessionId);
		}

		[Transaction()]
		public void ReSubmitCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			//Author : Abhijith
			//User Story : AL-3371
			//Description : As Tier 1 or Tier 2 support personnel, I need step-by-step transaction information for Check Processing.
			//Starts Here
			string id = "Check Id : " + checkId;

			MongoDBLogger.Info<string>(customerSessionId, id, "ReSubmitCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin ReSubmitCheck - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			//Ends Here

			SharedEngine.ReSubmitCheck(customerSessionId, checkId, context);

			//Author : Abhijith
			//User Story : AL-3371
			//Description : As Tier 1 or Tier 2 support personnel, I need step-by-step transaction information for Check Processing.
			//Starts Here
			MongoDBLogger.Info<string>(customerSessionId, id, "ReSubmitCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End ReSubmitCheck - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			//Ends Here
		}

		[Transaction()]
		public void ReSubmitMO(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			string id = "Check Id : " + checkId;

			MongoDBLogger.Info<string>(customerSessionId, id, "ReSubmitMO", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin ReSubmitMO - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion

			SharedEngine.ReSubmitMO(customerSessionId, checkId, context);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<string>(customerSessionId, id, "ReSubmitMO", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End ReSubmitMO - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion
		}

        [Transaction(ReadOnly=true)]
        public List<ParkedTransactionDTO> GetAllParkedShoppingCartTransactions()
        {
            return SharedEngine.GetAllParkedShoppingCartTransactions();
        }
		#endregion

		#region DMS Methods

		public void SetSelf(IDesktopService dts)
		{
			Self = dts;
		}

		[Transaction()]
		public void AddCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3371 Transactional Log User Story(Process check)
			string id = "Check Id : " + Convert.ToString(checkId);

			MongoDBLogger.Info<string>(customerSessionId, id, "AddCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin AddCheck - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion

			BIZShoppingCartService.AddCheck(customerSessionId, checkId, context);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(customerSessionId, id, "AddCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End AddCheck - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion
		}

		[Transaction()]
		public void AddFunds(long customerSessionId, long fundsId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3372 transaction information for GPR cards.
			string id = "Funds Id : " + Convert.ToString(fundsId);

			MongoDBLogger.Info<string>(customerSessionId, id, "AddFunds", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin AddFunds - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
		#endregion

			BIZShoppingCartService.AddFunds(customerSessionId, fundsId, context);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, id, "AddFunds", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End AddFunds - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
		#endregion

		}

		[Transaction()]
		public void AddBillPay(long customerSessionId, long billPayId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(billPayId), "AddBillPay", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"Begin AddBillPay - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine", context);
			#endregion
			BIZShoppingCartService.AddBillPay(customerSessionId, billPayId, context);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "AddBillPay", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"End AddBillPay - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine", context);
			#endregion
		}

		[Transaction()]
		public void AddMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrder Id : " + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "AddMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin AddMoneyOrder - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion


			BIZShoppingCartService.AddMoneyOrder(customerSessionId, moneyOrderId, context);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<string>(customerSessionId, id, "AddMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End AddMoneyOrder - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion

		}

		[Transaction()]
		public void ParkCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3371 Transactional Log User Story(Process check)
			string id = "Check Id : " + checkId;

			MongoDBLogger.Info<string>(customerSessionId, id, "ParkCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin ParkCheck - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion

			long agentSessionId = 0L;
			BIZShoppingCartService.ParkCheck(customerSessionId, checkId, context);
			MGI.Biz.CPEngine.Data.CheckTransaction check = CPEngineService.GetTransaction(agentSessionId, customerSessionId, checkId.ToString(), context);

			if (check.Status == CheckStatus.Pending.ToString())
			{
				MessageCenterService.Update(customerSessionId, new MGI.Biz.Partner.Data.AgentMessage()
				{
					Amount = check.Amount.ToString(),
					AgentId = int.Parse(Convert.ToString(CustomerSessionSvc.Lookup(Convert.ToInt64(customerSessionId)).AgentSession.Agent.Id)),
					TransactionState = check.Status,
					IsParked = true,
					IsActive = true,
					Transaction = BIZShoppingCartService.Get(customerSessionId, context).Checks.Where(x => x.Id == checkId).FirstOrDefault()
				}, context);
			}

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<MGI.Biz.CPEngine.Data.CheckTransaction>(customerSessionId, check, "ParkCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End ParkCheck - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion

		}

		[Transaction()]
		public void ParkFunds(long customerSessionId, long fundsId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3372 transaction information for GPR cards.
			string id = "Funds Id : " + Convert.ToString(fundsId);

			MongoDBLogger.Info<string>(customerSessionId, id, "ParkFunds", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin ParkFunds - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion

			BIZShoppingCartService.ParkFunds(customerSessionId, fundsId, context);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, id, "ParkFunds", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End ParkFunds - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion
		}
		[Transaction()]
		public void ParkBillPay(long customerSessionId, long billPayId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(billPayId), "ParkBillPay", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"Begin ParkBillPay - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine", context);
			#endregion
			BIZShoppingCartService.ParkBillPay(customerSessionId, billPayId, context);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "ParkBillPay", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"End ParkBillPay - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine", context);
			#endregion
		}
		[Transaction()]
		public void ParkMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(moneyTransferId), "ParkMoneyTransfer", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "Begin ParkMoneyTransfer- MGI.Channel.DMS.Server.Impl.ShoppingCartEngine", context);
			#endregion
			BIZShoppingCartService.ParkMoneyTransfer(customerSessionId, moneyTransferId, context);

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "ParkMoneyTransfer", AlloyLayerName.SERVICE, ModuleName.MoneyTransfer,
									  "End ParkMoneyTransfer- MGI.Channel.DMS.Server.Impl.ShoppingCartEngine", context);
			#endregion
		}
		[Transaction()]
		public void ParkMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrder Id : " + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "ParkMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin ParkMoneyOrder - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion

			BIZShoppingCartService.ParkMoneyOrder(customerSessionId, moneyOrderId, context);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<string>(customerSessionId, id, "ParkMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End ParkMoneyOrder - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine",
				context);
			#endregion
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added new method to Get IsReferralApplicable for the ChannelPartner
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		[Transaction()]
		public bool IsReferralApplicable(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return BIZShoppingCartService.IsReferralApplicable(customerSessionId, context);
		}

		[Transaction()]
		public void PostFlush(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "PostFlush", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"Begin PostFlush - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine", context);
			#endregion
			SharedEngine.PostFlush(customerSessionId, context);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "PostFlush", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"End PostFlush - MGI.Channel.DMS.Server.Impl.ShoppingCartEngine", context);
			#endregion
		}


		#endregion

		#endregion
	}
}
