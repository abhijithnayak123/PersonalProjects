using AutoMapper;
using MGI.Biz.MoneyTransfer.Data;
using MGI.Biz.Partner.Data.Transactions;
using MGI.Channel.Shared.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using bizBills = MGI.Biz.Partner.Data.Transactions.BillPay;
using bizCheck = MGI.Biz.Partner.Data.Transactions.Check;
using bizFunds = MGI.Biz.Partner.Data.Transactions.Funds;
using bizMoneyOrder = MGI.Biz.Partner.Data.Transactions.MoneyOrder;
using bizMoneyTransfer = MGI.Biz.Partner.Data.Transactions.MoneyTransfer;
using bizCash = MGI.Biz.Partner.Data.Transactions.Cash;
using bizShoppingCart = MGI.Biz.Partner.Data.ShoppingCart;
using bizShoppingCartCheckoutStatus = MGI.Biz.Partner.Data.ShoppingCartCheckoutStatus;
using bizShoppingCartService = MGI.Biz.Partner.Contract.IShoppingCartService;
using SharedData = MGI.Channel.Shared.Server.Data;
using ShoppingCartImplDTO = MGI.Channel.Shared.Server.Data.ShoppingCart;

namespace MGI.Channel.Shared.Server.Impl
{
	public partial class SharedEngine : IShoppingCartService
	{
		#region Injected Services

		bizShoppingCartService BIZShoppingCartService { get; set; }

		#endregion

		#region ShoppingCartService Data Mapper

		internal static void ShoppingCartConverter()
		{
			Mapper.CreateMap<bizShoppingCartCheckoutStatus, SharedData.ShoppingCartCheckoutStatus>();
			Mapper.CreateMap<SharedData.ShoppingCartCheckoutStatus, bizShoppingCartCheckoutStatus>();
            Mapper.CreateMap<Biz.Partner.Data.ParkedTransaction, ParkedTransaction>();
		}

		#endregion

		#region IShoppingCartService Impl

        public bool RemoveCheck(long customerSessionId, long checkId, bool isParkedTransaction, MGIContext mgiContext)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			List<string> details = new List<string>();
			details.Add("Check Id : " + checkId);
			details.Add("Is Parked Transaction : " + (isParkedTransaction ? "Yes" : "No"));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin RemoveCheck - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion

			bool isCheckRemovedFromCart = false;
			bool isCheckCancelled = CPEngineService.Cancel(customerSessionId, checkId.ToString(), mgiContext);
			if (isCheckCancelled == true)
			{
				isCheckRemovedFromCart = BIZShoppingCartService.RemoveCheck(customerSessionId, checkId, isParkedTransaction, mgiContext);
			}
			bizShoppingCart shoppingcart = BIZShoppingCartService.Get(customerSessionId, mgiContext);

			List<MGI.Biz.Partner.Data.Transactions.Check> availableChecks = null;

			if (shoppingcart != null)
			{
				availableChecks = shoppingcart.Checks.Where(x => x.Id > checkId).ToList();
				foreach (var check in availableChecks)
				{
					CPEngineService.Resubmit(customerSessionId, check.Id, mgiContext);
				} 
			}

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.ListInfo<bizCheck>(customerSessionId, availableChecks, "RemoveCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End RemoveCheck - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion

			return isCheckRemovedFromCart;
		}

        public void RemoveCheckFromCart(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			string id = "Check Id : " + checkId;

			MongoDBLogger.Info<string>(customerSessionId, id, "RemoveCheckFromCart", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin RemoveCheckFromCart - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion

			BIZShoppingCartService.RemoveCheck(customerSessionId, checkId, false, mgiContext);
			CPEngineService.UpdateStatusOnRemoval(customerSessionId, checkId);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.Info<string>(customerSessionId, id, "RemoveCheckFromCart", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End RemoveCheckFromCart - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion
		}

        public void RemoveFunds(long customerSessionId, long fundsId, bool isParkedTransaction, MGIContext mgiContext)
		{

			#region AL-3372 transaction information for GPR cards.
			List<string> details = new List<string>();
			details.Add("Funds Id : " + Convert.ToString(fundsId));
			details.Add("Is Parked Transaction : " + (isParkedTransaction ? "Yes" : "No"));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveFunds", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin RemoveFunds - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion

			BizFundsService.Cancel(customerSessionId, fundsId, mgiContext);

			BIZShoppingCartService.RemoveFunds(customerSessionId, fundsId, isParkedTransaction, mgiContext);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveFunds", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End RemoveFunds - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion
		}

        public void RemoveBillPay(long customerSessionId, long billPayId, bool isParkedTransaction, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Bill Pay ID:" + Convert.ToString(billPayId));
			details.Add("Is Parked Transaction:" + Convert.ToString(isParkedTransaction));
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveBillPay", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"Begin RemoveBillPay - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
			mgiContext.RequestType = Data.RequestType.CANCEL.ToString();
			try
			{

				BizBillPayService.Commit(customerSessionId, billPayId, mgiContext);

				BizBillPayService.Cancel(customerSessionId, billPayId, mgiContext);

				BIZShoppingCartService.RemoveBillPay(customerSessionId, billPayId, isParkedTransaction, mgiContext);
			}
			catch (Exception)
			{
				BizBillPayService.UpdateTransactionStatus(customerSessionId, billPayId, mgiContext);

				BIZShoppingCartService.RemoveBillPay(customerSessionId, billPayId, isParkedTransaction, mgiContext);

				return;
			}

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "RemoveBillPay", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"End RemoveBillPay - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
		}

        public void RemoveMoneyOrder(long customerSessionId, long moneyOrderId, bool isParkedTransaction, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			List<string> details = new List<string>();
			details.Add("MoneyOrder Id : " + moneyOrderId);
			details.Add("Is Parked Transaction : " + (isParkedTransaction ? "Yes" : "No"));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin RemoveMoneyOrder - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion

			BIZShoppingCartService.RemoveMoneyOrder(customerSessionId, moneyOrderId, isParkedTransaction, mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End RemoveMoneyOrder - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion
		}

        public void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, bool isParkedTransaction, MGIContext mgiContext)
		{
			#region AL-3370 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Transaction Id:" + Convert.ToString(moneyTransferId));
			details.Add("Parked Transaction:" + Convert.ToString(isParkedTransaction));
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "RemoveMoneyTransfer", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"Begin RemoveMoneyTransfer - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
			long agentSessionId = 0L;
			SharedData.MoneyTransferTransaction moneyTransfer = GetMoneyTransferTransaction(agentSessionId, customerSessionId, moneyTransferId, mgiContext);

			if (moneyTransfer.TransactionType == ((int)SharedData.TransferType.SendMoney).ToString())
				mgiContext.SMTrxType = (SharedData.MTReleaseStatus.Cancel).ToString();
			else if (moneyTransfer.TransactionType == ((int)SharedData.TransferType.RecieveMoney).ToString())
				mgiContext.RMTrxType = (SharedData.MTReleaseStatus.Cancel).ToString();

			try
			{
				if (string.IsNullOrEmpty(moneyTransfer.TransactionSubType) && moneyTransfer.TransactionType != ((int)SharedData.TransferType.Refund).ToString())
				{
					//Call WU first to cancel the trx only of SendMoney,ReceiveMoney
					bizMoneyTransferEngine.Commit(customerSessionId, moneyTransferId, mgiContext);
				}

				//Update states in PTNR, CXE for cancel
				bizMoneyTransferEngine.Cancel(customerSessionId, moneyTransferId, mgiContext);

				//finally remove from shopping cart
				BIZShoppingCartService.RemoveMoneyTransfer(customerSessionId, moneyTransferId, isParkedTransaction, mgiContext);
			}
			catch (Exception)
			{
				//Update states in PTNR, CXE for cancel
				bizMoneyTransferEngine.UpdateTransactionStatus(customerSessionId, moneyTransferId, mgiContext);

				//finally remove from shopping cart
				BIZShoppingCartService.RemoveMoneyTransfer(customerSessionId, moneyTransferId, isParkedTransaction, mgiContext);

				return;
			}

			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "RemoveMoneyTransfer", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"End RemoveMoneyTransfer - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
		}

		/// <summary>
		/// AL-2729 Removing Cash In Transacions
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="cashInId"></param>
		/// <param name="mgiContext"></param>
		public void RemoveCashIn(long customerSessionId, long cashInId, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			string id = Convert.ToString(cashInId);

			MongoDBLogger.Info<string>(customerSessionId, id, "RemoveCashIn", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin RemoveCashIn - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion
			
			CashEngine.Cancel(customerSessionId, cashInId, mgiContext); // Updating cashIn transaction state to Cancel

			BIZShoppingCartService.RemoveCashIn(customerSessionId, cashInId, mgiContext); // Removing CashIn tranaction from shopping cart 

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, id, "RemoveCashIn", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End RemoveCashIn - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion
		}

		public ShoppingCartImplDTO ShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "ShoppingCart", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"Begin ShoppingCart - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
			bizShoppingCart shoppingCart = BIZShoppingCartService.Get(customerSessionId, mgiContext);

			ShoppingCartImplDTO cart = new ShoppingCartImplDTO();//ShoppingCartService.GetShoppingCart(alloyId.ToString());          

			if (shoppingCart == null)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<ShoppingCartImplDTO>(customerSessionId, cart, "ShoppingCart", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
					"End ShoppingCart - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
				#endregion
				return cart;
			}

			cart.Checks = Checks(shoppingCart, customerSessionId, mgiContext);
			cart.CheckTotal = cart.Checks.Sum(d => d.Amount);
			cart.Bills = Bills(customerSessionId, shoppingCart, mgiContext);
			cart.BillTotal = cart.Bills.Sum(d => d.Amount) + cart.Bills.Sum(d => d.Fee);
			cart.MoneyTransfers = MoneyTransfers(customerSessionId, shoppingCart, mgiContext);
			cart.MoneyTransfeTotal = cart.MoneyTransfers.Sum(d => d.Amount) + cart.MoneyTransfers.Sum(d => d.Fee);
			cart.GprCards = GprCards(customerSessionId, shoppingCart, mgiContext);
			cart.GprCardTotal = cart.GprCards.Sum(d => d.ActivationFee) + cart.GprCards.Sum(d => d.LoadAmount) + cart.GprCards.Sum(d => d.LoadFee) + cart.GprCards.Sum(d => d.WithdrawAmount) + cart.GprCards.Sum(d => d.WithdrawFee);
			cart.Cash = CartCash(shoppingCart);
			cart.CashInTotal = cart.Cash.Where(x => x.CashType == ProductType.CashIn.ToString()).Sum(x => x.Amount);
			cart.CustomerSessionId = customerSessionId;
			cart.Id = shoppingCart.Id;
			cart.MoneyOrders = MoneyOrders(shoppingCart, customerSessionId);
			cart.MoneyOrderTotal = (cart.MoneyOrders == null) ? 0 : cart.MoneyOrders.Sum(d => d.Amount) + cart.MoneyOrders.Sum(d => d.Fee);
			cart.IsReferral = shoppingCart.IsReferral; //US1800 Referral promotions – Free check cashing to referrer and referee 

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<ShoppingCartImplDTO>(customerSessionId, cart, "ShoppingCart", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"End ShoppingCart - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
			return cart;
		}

		public SharedData.ShoppingCartCheckoutStatus Checkout(long customerSessionId, decimal cashToCustomer, string cardNumber, SharedData.ShoppingCartCheckoutStatus shoppingCartstatus, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			List<string> details = new List<string>();
			details.Add("CashToCustomer :" + Convert.ToString(cashToCustomer));
			details.Add("CardNumber :" + Convert.ToString(cardNumber));
			details.Add("ShoppingCartstatus :" + Convert.ToString(shoppingCartstatus));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "Checkout", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin Shopping Cart Checkout - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion
			
			bizShoppingCartCheckoutStatus bizshoppingCartstatus = Mapper.Map<bizShoppingCartCheckoutStatus>(shoppingCartstatus);

			bizShoppingCartCheckoutStatus shoppingCartCheckoutStatus = BIZShoppingCartService.Checkout(customerSessionId, cashToCustomer, cardNumber, bizshoppingCartstatus, mgiContext);

			SharedData.ShoppingCartCheckoutStatus shoppingCartWithStatus = Mapper.Map<SharedData.ShoppingCartCheckoutStatus>(shoppingCartCheckoutStatus);

			#region AL-1071 Transactional Log class for MO flow
			string status = "ShoppingCart Status:" + Convert.ToString(shoppingCartWithStatus);

			MongoDBLogger.Info<string>(customerSessionId, status, "Checkout", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End Shopping Cart Checkout - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion

			return shoppingCartWithStatus;
		}

		public SharedData.Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(shoppingCartId), "GenerateReceiptsForShoppingCart", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"Begin GenerateReceiptsForShoppingCart - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
			bizShoppingCart shoppingCart = BIZShoppingCartService.GetCartById(customerSessionId, shoppingCartId, mgiContext);
			SharedData.Receipts receipts = new SharedData.Receipts();
			receipts = GenerateReceiptsForShoppingCart(shoppingCart, customerSessionId, mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<SharedData.Receipts>(customerSessionId, receipts, "GenerateReceiptsForShoppingCart", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"End GenerateReceiptsForShoppingCart - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
			return receipts;
		}

		public void CloseShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "CloseShoppingCart", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"Begin CloseShoppingCart - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
			BIZShoppingCartService.CloseShoppingCart(customerSessionId, mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "CloseShoppingCart", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"End CloseShoppingCart - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
		}

        public void ReSubmitCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			#region AL-3371 Transactional Log User Story(Process check)
			string id = "Check Id : " + checkId;

			MongoDBLogger.Info<string>(customerSessionId, id, "ReSubmitCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin ReSubmitCheck - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion

			bizShoppingCart shoppingcart = BIZShoppingCartService.Get(customerSessionId, mgiContext);

			var availableChecks = shoppingcart.Checks.Where(x => x.Id > checkId);

			foreach (var checkTrx in availableChecks)
			{
				CPEngineService.Resubmit(customerSessionId, checkTrx.Id, mgiContext);
			}

			mgiContext.IsParked = true;

			CPEngineService.Resubmit(customerSessionId, checkId, mgiContext);

			#region AL-3371 Transactional Log User Story(Process check)
			MongoDBLogger.ListInfo<bizCheck>(customerSessionId, availableChecks.ToList(), "ReSubmitCheck", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End ReSubmitCheck - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion
		}


        public void ReSubmitMO(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrder Id : " + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "ReSubmitMO", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "Begin ReSubmitMO - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion

			bizShoppingCart shoppingcart = BIZShoppingCartService.Get(customerSessionId, mgiContext);

			var availableMOs = shoppingcart.MoneyOrders.Where(x => x.Id > moneyOrderId);

			foreach (var trx in availableMOs)
			{
				MoneyOrderEngineService.Resubmit(customerSessionId, trx.Id, mgiContext);
			}

			mgiContext.IsParked = true;

			MoneyOrderEngineService.Resubmit(customerSessionId, moneyOrderId, mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.ListInfo<bizMoneyOrder>(customerSessionId, availableMOs.ToList(), "ReSubmitMO", AlloyLayerName.SERVICE,
				ModuleName.ShoppingCart, "End ReSubmitMO - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine",
				mgiContext);
			#endregion
		}

		public void PostFlush(long customerSessionId, MGIContext mgiContext)
		{
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "PostFlush", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"Begin PostFlush - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
			BIZShoppingCartService.PostFlush(customerSessionId, mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, string.Empty, "PostFlush", AlloyLayerName.SERVICE, ModuleName.ShoppingCart,
				"End PostFlush - MGI.Channel.Shared.Server.Impl.ShoppingCartEngine", mgiContext);
			#endregion
		}

        public List<ParkedTransaction> GetAllParkedShoppingCartTransactions()
        {
            var parkedTrxs = BIZShoppingCartService.GetAllParkedShoppingCartTransactions();
            List<ParkedTransaction> parkedTrans = Mapper.Map<List<Biz.Partner.Data.ParkedTransaction>, List<ParkedTransaction>>(parkedTrxs);
            return parkedTrans;
        }

		#endregion

		#region Private methods



		private bool IsCashOverCounter(long customerSessionId, MGIContext mgiContext)
		{
			// chk for channel partner config for CashOverCounter		
			MGI.Biz.Partner.Data.ChannelPartner channelpartner = ChannelPartnerService.ChannelPartnerConfig(mgiContext.ChannelPartnerId.ToString(), mgiContext);

			return channelpartner.CashOverCounter;
		}

		private SharedData.Receipts GenerateReceiptsForShoppingCart(bizShoppingCart cart, long customerSessionId, MGIContext mgiContext)
		{
			SharedData.Receipts receipts = new SharedData.Receipts();
			receipts.receipts = new List<SharedData.Receipt>();
			receipts.receiptType = new List<string>();

			if (cart == null)
				return receipts;

			List<bizCheck> checks = cart.Checks.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Committed).ToList<bizCheck>();
			List<bizBills> bills = cart.BillPays.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Committed).ToList<bizBills>();
			List<bizFunds> funds = cart.Funds.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Committed).ToList<bizFunds>();
			List<bizMoneyTransfer> xfers = cart.MoneyTransfers.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Committed).ToList<bizMoneyTransfer>();
			List<bizMoneyOrder> moneyOrders = cart.MoneyOrders.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Committed).OrderBy(x => x.Id).ToList<bizMoneyOrder>();
            List<bizCash> cashTrans = cart.CashInTransactions.Where(x => x.CXEState == (int)MGI.Core.CXE.Data.TransactionStates.Committed).ToList<bizCash>();
			if (checks.Any())
			{
				foreach (var check in checks)
				{
					try
					{
						receipts.receiptType.Add(string.Format("Process Check - TrnxID : {0}", check.Id.ToString()));
					}
					catch { }
				}
			}

            if (bills.Any())
            {
                foreach (var bill in bills)
                {
                    try
                    {
                        MGI.Biz.Catalog.Data.Product _product = ProductCatalog.Get(bill.ProductId);
                        MGI.Core.Partner.Contract.ProviderIds providerId =
							(MGI.Core.Partner.Contract.ProviderIds)_product.ProviderId;

						receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", ProductType.BillPay.ToString(), bill.Id.ToString()));
                    }
                    catch { }
                }

			}

			if (funds.Any())
			{
				if (funds.Where(x => x.TransactionType == SharedData.FundType.None.ToString()).Count() > 0)
				{
					try
					{
						bizFunds fundActivation = funds.Where(x => x.TransactionType == SharedData.FundType.None.ToString()).First();

						receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", ProductType.GPRActivation.ToString(), fundActivation.Id.ToString()));
					}
					catch { }
				}
				else
				{
					foreach (var fund in funds)
					{
						try
						{
							if (fund.TransactionType == SharedData.FundType.Debit.ToString())
							{
								receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", ProductType.GPRWithdraw.ToString(), fund.Id.ToString()));
							}
							else if (fund.TransactionType == SharedData.FundType.AddOnCard.ToString())
							{
								receipts.receiptType.Add(string.Format(string.Format("Companion Card Order - TrnxID : {0}", fund.Id.ToString())));
							}
							else 
							{
								receipts.receiptType.Add(string.Format(string.Format("{0} - TrnxID : {1}", ProductType.GPRLoad.ToString(), fund.Id.ToString())));
							}
						}
						catch { }
					}
				}
			}

            if (xfers.Any())
            {
                foreach (var xfer in xfers)
                {
                    try
                    {
                        if (xfer.TransferType == (int)SharedData.TransferType.SendMoney && xfer.TransactionSubType != ((int)SharedData.TransactionSubType.Cancel).ToString() && xfer.TransactionSubType != ((int)SharedData.TransactionSubType.Refund).ToString())
                        {
							receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", ProductType.SendMoney.ToString(), xfer.Id.ToString()));
                        }
                        else if (xfer.TransferType == (int)SharedData.TransferType.RecieveMoney)
                        {
							receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", ProductType.ReceiveMoney.ToString(), xfer.Id.ToString()));
                        }
                        else if (xfer.TransferType == (int)SharedData.TransferType.Refund)
                        {
                            receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", ProductType.Refund.ToString(), xfer.Id.ToString()));
                        }
                    }
                    catch { }
                }
            }
            if (moneyOrders.Any())
            {
                foreach (var moneyOrder in moneyOrders)
                {
                    try
					{
						receipts.receiptType.Add(string.Format("{0} - TrnxID : {1}", ProductType.MoneyOrder.ToString(), moneyOrder.Id.ToString()));
                    }
                    catch { }
                }
            }

			if (receipts.receiptType.Count > 0 || cashTrans.Any())
			    receipts.receiptType.Add(string.Format("Summary - CartID : {0}", cart.Id));

			//US1800 Referral promotions – Free check cashing to referrer and referee 
			if (cart.IsReferral)
				receipts.receiptType.Add("Coupon");

			return receipts;
		}


		private List<SharedData.MoneyTransfer> MoneyTransfers(long customerSessionId, bizShoppingCart shoppingCart, MGIContext mgiContext)
		{
			List<SharedData.MoneyTransfer> moneyTransfer = new List<SharedData.MoneyTransfer>();

			List<bizMoneyTransfer> moneyTransfers = shoppingCart.MoneyTransfers.FindAll(x => x.TransactionSubType == ((int)SharedData.TransactionSubType.Refund).ToString());

			TransactionRequest request = new TransactionRequest();

			foreach (var each in shoppingCart.MoneyTransfers)
			{
				if (each.TransactionSubType != ((int)SharedData.TransactionSubType.Refund).ToString() && each.TransactionSubType != ((int)SharedData.TransactionSubType.Cancel).ToString())
				{
					request.CXNTransactionId = each.CXNId;
					MGI.Biz.MoneyTransfer.Data.MoneyTransferTransaction transaction = bizMoneyTransferEngine.Get(customerSessionId, request, mgiContext);

					SharedData.MoneyTransfer mt = MoneyTransferMapper(transaction, each);
					moneyTransfer.Add(mt);
				}
				else if (each.TransactionSubType == ((int)SharedData.TransactionSubType.Cancel).ToString())
				{
					var isRefundCancel = false;

					if (moneyTransfers != null)
					{
						if (moneyTransfers.Exists(x => x.OriginalTransactionId == each.OriginalTransactionId))
						{
							isRefundCancel = true;
						}
					}

					if (!isRefundCancel)
					{
						request.CXNTransactionId = each.CXNId;

						MGI.Biz.MoneyTransfer.Data.MoneyTransferTransaction transaction = bizMoneyTransferEngine.Get(customerSessionId, request, mgiContext);

						SharedData.MoneyTransfer mt = MoneyTransferMapper(transaction, each);
						moneyTransfer.Add(mt);
					}
				}
			}

			return moneyTransfer;
		}


        private List<SharedData.Check> Checks(bizShoppingCart shoppingCart, long customerSessionId, MGIContext mgiContext)
		{
            long agentSessionId = 0L;
			List<SharedData.Check> checks = new List<SharedData.Check>();
			foreach (var tcheck in shoppingCart.Checks)
			{
				//Cxn.Check.Data.CheckTrx transaction = ChexarGateway.Get(tcheck.Id);
				MGI.Biz.CPEngine.Data.CheckTransaction transaction = CPEngineService.GetTransaction(agentSessionId, customerSessionId, tcheck.Id.ToString(), mgiContext);
				SharedData.Check check = CheckMapper(transaction, tcheck);
				checks.Add(check);
			}
			return checks;
		}


		private SharedData.Check CheckMapper(MGI.Biz.CPEngine.Data.CheckTransaction trxFee, MGI.Biz.Partner.Data.Transactions.Check tcheck)
		{


			return new SharedData.Check()
			{
				Id = tcheck.Id.ToString(),
				Amount = tcheck.Amount,
				Fee = trxFee.Fee,
				StatusDescription = tcheck.Description,
				Status = ((int)tcheck.CXEState).ToString(),
				StatusMessage = trxFee.DeclineMessage,
				BaseFee = trxFee.BaseFee,
				DiscountApplied = trxFee.DiscountApplied,
				DiscountName = trxFee.DiscountName,
                DmsStatusMessage = trxFee.DmsDeclineMessage
			};
		}

		private List<SharedData.GprCard> GprCards(long customerSessionId, bizShoppingCart shoppingCart, MGIContext mgiContext)
		{
			List<SharedData.GprCard> GprCards = new List<SharedData.GprCard>();
			foreach (var each in shoppingCart.Funds)
			{
				/****************************AL-138 Changes************************************************/
				//     User Story Number: AL-138 | Server |   Developed by: Sunil Shetty     Date: 01.04.2015
				//      Purpose: Partner ID has been passed instead of CXN Id
				MGI.Biz.FundsEngine.Data.Funds transaction = BizFundsService.Get(customerSessionId, each.Id, mgiContext);
				SharedData.GprCard gprCard = GprCardMapper(customerSessionId, transaction, each, mgiContext);

				GprCards.Add(gprCard);
			}
			return GprCards;
		}

		private SharedData.Bill BillPayMapper(MGI.Biz.BillPay.Data.BillPayTransaction transaction, BillPay bill)
		{
			SharedData.Bill billPay = new SharedData.Bill();

			billPay.Id = bill.Id.ToString();
			billPay.Fee = bill.Fee;
			billPay.AccountNumber = transaction.AccountNumber;
			billPay.Amount = bill.Amount;
			billPay.BillerId = long.Parse(bill.Description);
			billPay.Status = bill.CXEState.ToString();
			billPay.CustomerFirstName = transaction.Account.FirstName;
			billPay.CustomerLastName = transaction.Account.LastName;
			billPay.CustomerPhoneNumber = transaction.Account.ContactPhone;
			billPay.CustomerState = transaction.Account.State;
			billPay.CustomerZip = transaction.Account.PostalCode;
			billPay.CustomerCity = transaction.Account.City;
			billPay.AcceptedFee = true;
			billPay.BillerName = transaction.BillerName;
			billPay.BillTotal = bill.Amount + bill.Fee;
			billPay.BillId = bill.Id.ToString();
			billPay.BillerId = bill.ProductId;
			return billPay;
		}

		private List<SharedData.Bill> Bills(long customerSessionId, bizShoppingCart shoppingCart, MGIContext mgiContext)
		{
			List<SharedData.Bill> bills = new List<SharedData.Bill>();
			foreach (var each in shoppingCart.BillPays)
			{
				MGI.Biz.BillPay.Data.BillPayTransaction transaction = BizBillPayService.GetTransaction(customerSessionId, each.Id, mgiContext);
				SharedData.Bill bill = BillPayMapper(transaction, each);
				bills.Add(bill);
			}
			return bills;
		}


		private SharedData.MoneyTransfer MoneyTransferMapper(MGI.Biz.MoneyTransfer.Data.MoneyTransferTransaction transaction, MGI.Biz.Partner.Data.Transactions.MoneyTransfer moneyTransfer)
		{

			SharedData.MoneyTransfer moneytransfer = new SharedData.MoneyTransfer()
			{
				AcceptedFee = true,
				Amount = moneyTransfer.Amount,
				DestinationAmount = transaction.DestinationPrincipalAmount != 0 ? Convert.ToDecimal(transaction.DestinationPrincipalAmount) : 0,
				DestinationCountry = transaction.DestinationCountryCode,
				DestinationCurrency = transaction.DestinationCurrencyCode,
				ExchangeRate = transaction.ExchangeRate != 0 ? Convert.ToDecimal(transaction.ExchangeRate) : 0,
				Fee = moneyTransfer.Fee,
				Id = moneyTransfer.Id.ToString(),
				MoneyTransferTotal = transaction.GrossTotalAmount != 0 ? Convert.ToDecimal(transaction.GrossTotalAmount) : 0,
				OtherFee = transaction.Fee != 0 ? Convert.ToDecimal(transaction.Fee) : 0,
				OtherTax = transaction.TaxAmount != 0 ? Convert.ToDecimal(transaction.TaxAmount) : 0,
				ReceiverAddress = transaction.Receiver.Address,
				ReceiverCity = transaction.Receiver.City,
				ReceiverFirstName = transaction.ReceiverFirstName,
				ReceiverLastName = transaction.ReceiverLastName,
				ReceiverState = transaction.DestinationState,
				SourceAmount = transaction.TransactionAmount != 0 ? Convert.ToDecimal(transaction.TransactionAmount) : 0,
				SourceCountry = transaction.OriginatingCountryCode,
				SourceCurrency = transaction.OriginatingCurrencyCode,
				Status = moneyTransfer.CXEState.ToString(),
				TransferTax = transaction.TaxAmount != 0 ? Convert.ToDecimal(transaction.TaxAmount) : 0,
				TransferType = int.Parse(transaction.TransactionType),
				TransactionSubType = transaction.TransactionSubType,
				OriginalTransactionId = moneyTransfer.OriginalTransactionId,
				SenderFirstName = transaction.Account.FirstName,
				SenderLastName = transaction.Account.LastName,
				SenderMiddleName = transaction.Account.MiddleName,
				SenderSecondLastName = transaction.Account.SecondLastName,
				ConfirmationNumber = transaction.ConfirmationNumber,
				MetaData = transaction.MetaData

			};
			return moneytransfer;
		}

		private SharedData.GprCard GprCardMapper(long customerSessionId, MGI.Biz.FundsEngine.Data.Funds transaction, bizFunds fund, MGIContext mgiContext)
		{
			SharedData.GprCard gpr = new SharedData.GprCard();

			gpr.AccountNumber = transaction.Account.AccountNumber;
			gpr.CardNumber = transaction.Account.CardNumber;
			gpr.Id = fund.Id.ToString();
			gpr.ItemType = transaction.FundsType.ToString();
			gpr.Status = fund.CXEState.ToString();
			gpr.StatusDescription = fund.Description;
			gpr.StatusMessage = transaction.FundDescription;
			gpr.TransactionId = fund.CXEId;
			if (gpr.ItemType == SharedData.FundType.Credit.ToString())
			{
				gpr.LoadAmount = fund.Amount;
				gpr.LoadFee = transaction.BaseFee - transaction.DiscountApplied;
				gpr.BaseFee = transaction.BaseFee;
				gpr.DiscountApplied = transaction.DiscountApplied;
				gpr.DiscountName = transaction.DiscountName;
				gpr.NetFee = transaction.BaseFee - transaction.DiscountApplied;
			}
			else if (gpr.ItemType == SharedData.FundType.Debit.ToString())
			{
				gpr.WithdrawAmount = fund.Amount;
				gpr.WithdrawFee = fund.Fee;
				gpr.BaseFee = transaction.BaseFee;
				gpr.DiscountApplied = transaction.DiscountApplied;
				gpr.DiscountName = transaction.DiscountName;
				gpr.NetFee = transaction.BaseFee - transaction.DiscountApplied;
			}
			else if (gpr.ItemType == SharedData.FundType.AddOnCard.ToString())
			{
				gpr.InitialLoadAmount = fund.Amount;
				gpr.ActivationFee = fund.Fee;
				MGI.Biz.Customer.Data.Customer customer = BizCustomerService.GetCustomer(customerSessionId, fund.AddOnCustomerId, mgiContext);
				gpr.AddOnCustomerName = string.Concat(customer.Profile.FirstName, " ", customer.Profile.LastName);
			}
			else
			{
				gpr.InitialLoadAmount = fund.Amount;
				gpr.ActivationFee = fund.Fee;
			}

			return gpr;
		}

		private List<SharedData.CartCash> CartCash(bizShoppingCart shoppingCart)
		{
			List<SharedData.CartCash> cashInTransactions = new List<SharedData.CartCash>();
			foreach (var tcash in shoppingCart.CashInTransactions)
			{
				SharedData.CartCash cash = new SharedData.CartCash() { Amount = tcash.Amount, Id = tcash.Id.ToString(), Status = tcash.CXEState.ToString(), CashType = tcash.TransactionType };
				cashInTransactions.Add(cash);
			}
			return cashInTransactions;
		}

		private List<SharedData.MoneyOrder> MoneyOrders(bizShoppingCart shoppingCart, long customerSessionId)
		{
			List<SharedData.MoneyOrder> moneyOrders = new List<SharedData.MoneyOrder>();
			MGIContext mgiContext = null;
			foreach (var tMoneyOrder in shoppingCart.MoneyOrders)
			{
				MGI.Biz.MoneyOrderEngine.Data.MoneyOrder trxFee = MoneyOrderEngineService.GetMoneyOrderStage(customerSessionId, tMoneyOrder.Id, mgiContext);
				SharedData.MoneyOrder moneyOrder = new SharedData.MoneyOrder
				{
					Amount = tMoneyOrder.Amount,
					Fee = trxFee.Fee,
					Id = tMoneyOrder.Id.ToString(),
					Status = tMoneyOrder.CXEState.ToString(),
					StatusDescription = tMoneyOrder.Description,
					BaseFee = trxFee.BaseFee,
					DiscountApplied = trxFee.DiscountApplied,
					DiscountName = trxFee.DiscountName
				};
				moneyOrders.Add(moneyOrder);
			}
			return moneyOrders;
		}

		#endregion

	}
}
