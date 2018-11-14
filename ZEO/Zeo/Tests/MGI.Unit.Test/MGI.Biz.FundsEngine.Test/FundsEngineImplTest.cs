using System.Collections.Generic;
using CXEData = MGI.Core.CXE.Data;
using NUnit.Framework;
using MGI.Cxn.Common.Processor.Util;
using MGI.Biz.FundsEngine.Data;
using MGI.Common.Util;
using MGI.Biz.FundsEngine.Contract;
using MGI.Unit.Test;
using Moq;
using MGI.Cxn.Fund.Data;
using MGI.Biz.Common.Data;
using MGI.Cxn.Fund.Contract;
using MGI.Biz.Compliance.Contract;

namespace MGI.Biz.FundsEngine.Test
{
	[TestFixture]
	public class FundsEngineImplTest : BaseClass_Fixture
	{
		public IFundsEngine InterceptedBIZFundsEngine { get; set; }

		[Test]
		public void Can_Add_Fund_Account()
		{
			FundsAccount fundAccount = new FundsAccount();

			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";
			long customerSessionId = 1000000001;

			long accountId = InterceptedBIZFundsEngine.Add(customerSessionId, fundAccount, mgiContext);

			Assert.AreNotEqual(accountId, 0);
			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.Register(It.IsAny<CardAccount>(), It.IsAny<MGIContext>(), out processorResult), Times.AtLeastOnce());
		}

		[Test]
		public void Can_AuthenticateCard()
		{
			string cardNumber = "1594872631593261";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";
			long customerSessionId = 1000000001;

			bool auto = InterceptedBIZFundsEngine.AuthenticateCard(customerSessionId, cardNumber, string.Empty, string.Empty, mgiContext);

			Assert.IsTrue(auto);
			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.Authenticate(It.IsAny<string>(), It.IsAny<MGIContext>(), out processorResult), Times.Once());
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Invalid_CustomerSession()
		{
			string cardNumber = "1594872631593261";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";
			long customerSessionId = 0;

			bool auto = InterceptedBIZFundsEngine.AuthenticateCard(customerSessionId, cardNumber, string.Empty, string.Empty, mgiContext);
		}

		[Test]
		public void Can_Activate_Card()
		{
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 100,
				Fee = 5
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.Activate(customerSessionId, fund, mgiContext);

			Assert.AreNotEqual(accountId, 0);
			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.Activate(It.IsAny<long>(), It.IsAny<FundRequest>(), It.IsAny<MGIContext>(), out processorResult), Times.Exactly(1));
		}

		[Test]
		public void Can_Update_Activate_Card()
		{
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 100,
				Fee = 5
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";
			mgiContext.TrxId = 1000000001;

			long accountId = InterceptedBIZFundsEngine.Activate(customerSessionId, fund, mgiContext);

			Assert.AreNotEqual(accountId, 0);
		}


		[Test]
		public void Can_Withdraw()
		{
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 100,
				Fee = 5
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.Withdraw(customerSessionId, fund, mgiContext);

			Assert.AreNotEqual(accountId, 0);
			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.Withdraw(It.IsAny<long>(), It.IsAny<FundRequest>(), It.IsAny<MGIContext>(), out processorResult), Times.Exactly(1));
		}

		[Test]
		public void Can_Load()
		{
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 100,
				Fee = 5
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.Load(customerSessionId, fund, mgiContext);

			Assert.AreNotEqual(accountId, 0);
			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.Load(It.IsAny<long>(), It.IsAny<FundRequest>(), It.IsAny<MGIContext>(), out processorResult), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Balance()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			MGI.Biz.FundsEngine.Data.CardInfo cardBalance = InterceptedBIZFundsEngine.GetBalance(customerSessionId, mgiContext);

			Assert.IsNotNull(cardBalance);
			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.GetBalance(It.IsAny<long>(), It.IsAny<MGIContext>(), out processorResult), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Commit_Fund_Trxn()
		{
			string cardNumber = "";
			long customerSessionId = 1000000001;
			long trxnId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerName = "TCF";

			int accountId = InterceptedBIZFundsEngine.Commit(customerSessionId, trxnId, mgiContext, cardNumber);

			Assert.AreNotEqual(accountId, 0);

			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.Commit(It.IsAny<long>(), It.IsAny<MGIContext>(), out processorResult, It.IsAny<string>()), Times.AtLeastOnce());
			CXEFundsService.Verify(moq => moq.Commit(It.IsAny<long>(), It.IsAny<string>()), Times.AtLeastOnce());
			CXEFundsService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Card_Account()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			MGI.Biz.FundsEngine.Data.FundsAccount fundAccount = InterceptedBIZFundsEngine.GetAccount(customerSessionId, mgiContext);

			Assert.IsNotNull(fundAccount);

			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.GetBalance(It.IsAny<long>(), It.IsAny<MGIContext>(), out processorResult), Times.AtLeastOnce());
			ProcessorRouter.Verify(moq => moq.Lookup(It.IsAny<long>()), Times.AtLeastOnce());
		}

		[Test]
		public void Get_Minimum_Load_Amount()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;

			decimal initialLoad = InterceptedBIZFundsEngine.GetMinimumLoadAmount(customerSessionId, true, mgiContext);

			Assert.AreNotEqual(initialLoad, 0);
		}

		[Test]
		public void Can_Cancel_Fund_Trxn()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";
			long fundsId = 1000000003;

			InterceptedBIZFundsEngine.Cancel(customerSessionId, fundsId, mgiContext);

			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.Cancel(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.Exactly(0));
			CXEFundsService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>()), Times.Exactly(1));
		}

		[Test]
		public void Can_GetTransactionHistory()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";
			MGI.Biz.FundsEngine.Data.TransactionHistoryRequest request = new MGI.Biz.FundsEngine.Data.TransactionHistoryRequest();

			var history = InterceptedBIZFundsEngine.GetTransactionHistory(customerSessionId, request, mgiContext);

			ProcessorRouter.Verify(moq => moq.GetTransactionHistory(It.IsAny<long>(), It.IsAny<MGI.Cxn.Fund.Data.TransactionHistoryRequest>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Close_Account()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";

			bool status = InterceptedBIZFundsEngine.CloseAccount(customerSessionId, mgiContext);

			Assert.True(status);
			ProcessorRouter.Verify(moq => moq.CloseAccount(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Update_Card_States()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";
			MGI.Biz.FundsEngine.Data.CardMaintenanceInfo cardMaintenanceInfo = new MGI.Biz.FundsEngine.Data.CardMaintenanceInfo() { };

			bool status = InterceptedBIZFundsEngine.UpdateCardStatus(customerSessionId, cardMaintenanceInfo, mgiContext);

			Assert.True(status);
			ProcessorRouter.Verify(moq => moq.UpdateCardStatus(It.IsAny<long>(), It.IsAny<MGI.Cxn.Fund.Data.CardMaintenanceInfo>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Replace_Card()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";
			MGI.Biz.FundsEngine.Data.CardMaintenanceInfo cardMaintenanceInfo = new MGI.Biz.FundsEngine.Data.CardMaintenanceInfo() { };

			bool status = InterceptedBIZFundsEngine.ReplaceCard(customerSessionId, cardMaintenanceInfo, mgiContext);

			Assert.True(status);
			ProcessorRouter.Verify(moq => moq.ReplaceCard(It.IsAny<long>(), It.IsAny<MGI.Cxn.Fund.Data.CardMaintenanceInfo>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Fee()
		{
			long customerSessionId = 1000000001;
			decimal amount = 10;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";

			TransactionFee fee = InterceptedBIZFundsEngine.GetFee(customerSessionId, amount, FundType.Credit, mgiContext);

			Assert.IsNotNull(fee);
		}

		[Test]
		public void Can_Get_Funds_Trxn()
		{
			long customerSessionId = 1000000001;
			long trxnId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";

			Funds fund = InterceptedBIZFundsEngine.Get(customerSessionId, trxnId, mgiContext);

			Assert.IsNotNull(fund);
			ProcessorRouter.Verify(moq => moq.Get(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Update_Amount()
		{
			long customerSessionId = 1000000001;
			decimal amount = 10;
			long trxnId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.UpdateAmount(trxnId, amount, customerSessionId, FundType.Credit, mgiContext);

			Assert.AreNotEqual(accountId, 0);
			ProcessorRouter.Verify(moq => moq.UpdateAmount(It.IsAny<long>(), It.IsAny<FundRequest>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Shipping_Types()
		{

			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerId = 33;

			List<MGI.Biz.FundsEngine.Data.ShippingTypes> shippingTypes = InterceptedBIZFundsEngine.GetShippingTypes(mgiContext);
			Assert.That(shippingTypes.Count, Is.GreaterThan(0));
			ProcessorRouter.Verify(moq => moq.GetShippingTypes(It.IsAny<long>()), Times.AtLeastOnce());
		}



		[Test]
		public void Can_Get_Shipping_Fee()
		{

			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerId = 33;

			MGI.Biz.FundsEngine.Data.CardMaintenanceInfo cardMaintenanceInfo = new MGI.Biz.FundsEngine.Data.CardMaintenanceInfo() { };
			double shippingFee = InterceptedBIZFundsEngine.GetShippingFee(cardMaintenanceInfo, mgiContext);

			Assert.That(shippingFee, Is.GreaterThan(0));
			ProcessorRouter.Verify(moq => moq.GetShippingFee(It.IsAny<MGI.Cxn.Fund.Data.CardMaintenanceInfo>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_AssociateCard_With_account()
		{
			FundsAccount fundAccount = new FundsAccount();
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			long customerSessionId = 1000000001;
			fundAccount = InterceptedBIZFundsEngine.GetAccount(customerSessionId, mgiContext);
			long cxnFundId = InterceptedBIZFundsEngine.AssociateCard(customerSessionId, fundAccount, mgiContext);
			Assert.That(cxnFundId, Is.GreaterThan(0));
		}


		[Test]
		public void Can_AssociateCard_Without_account()
		{
			FundsAccount fundAccount = new FundsAccount();
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			long customerSessionId = 1000000001;
			long cxnFundId = InterceptedBIZFundsEngine.AssociateCard(customerSessionId, fundAccount, mgiContext);
			Assert.That(cxnFundId, Is.GreaterThan(0));
		}

		[Test]
		public void Can_Fund_Fee()
		{
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			Data.CardMaintenanceInfo cxnCardMaintenanceInfo = new Data.CardMaintenanceInfo();
			cxnCardMaintenanceInfo.CardStatus = "5";			
			double fundFee = InterceptedBIZFundsEngine.GetFundFee(cxnCardMaintenanceInfo, mgiContext);
			Assert.That(fundFee, Is.GreaterThan(0));

		}

		[Test]
		public void Can_Order_CompanionCard()
		{
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 100,
				Fee = 5
			};

			long accountId = InterceptedBIZFundsEngine.IssueAddOnCard(customerSessionId, fund, mgiContext);

			Assert.AreNotEqual(accountId, 0);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Commit_Trx_With_CXNId()
		{
			string cardNumber = "";
			long customerSessionId = 1000000001;
			long trxnId = 1000000010;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerName = "TCF";

			int accountId = InterceptedBIZFundsEngine.Commit(customerSessionId, trxnId, mgiContext, cardNumber);
		}

		[Test]
		public void Can_AssociateCard_WithOut_PTNRAccount()
		{
			FundsAccount fundAccount = new FundsAccount();
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			long customerSessionId = 1000000014;

			long cxnFundId = InterceptedBIZFundsEngine.AssociateCard(customerSessionId, fundAccount, mgiContext);
			
			Assert.That(cxnFundId, Is.GreaterThan(0));
		}

		[Test]
		public void Can_Add_Fund_Account_WithOut_PTNRAccount()
		{
			FundsAccount fundAccount = new FundsAccount();

			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";
			long customerSessionId = 1000000101;

			long accountId = InterceptedBIZFundsEngine.Add(customerSessionId, fundAccount, mgiContext);

			Assert.AreNotEqual(accountId, 0);
			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.Register(It.IsAny<CardAccount>(), It.IsAny<MGIContext>(), out processorResult), Times.AtLeastOnce());
		}

		[Test]
		public void Can_AuthenticateCard_WithOut_CXNAccount()
		{
			string cardNumber = "4756756000179825";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";
			long customerSessionId = 1000000001;

			bool auto = InterceptedBIZFundsEngine.AuthenticateCard(customerSessionId, cardNumber, string.Empty, string.Empty, mgiContext);

			Assert.IsFalse(auto);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_AuthenticateCard_With_ProviderError()
		{
			string cardNumber = "4756756000179826";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";
			long customerSessionId = 1000000001;

			bool auto = InterceptedBIZFundsEngine.AuthenticateCard(customerSessionId, cardNumber, string.Empty, string.Empty, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_AuthenticateCard_With_Unknown_Error()
		{
			string cardNumber = "4756756000179827";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";
			long customerSessionId = 1000000001;

			bool auto = InterceptedBIZFundsEngine.AuthenticateCard(customerSessionId, cardNumber, string.Empty, string.Empty, mgiContext);
		}

		[Test]
		public void Can_Cancel_Fund_Trxn_Cancel()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";
			long fundsId = 1000000004;

			InterceptedBIZFundsEngine.Cancel(customerSessionId, fundsId, mgiContext);

			ProcessorResult processorResult = new ProcessorResult() { IsSuccess = true };
			ProcessorRouter.Verify(moq => moq.Cancel(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Commit_Fund_AddOnCard_Trxn()
		{
			string cardNumber = "";
			long customerSessionId = 1000000001;
			long trxnId = 1000000005;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			int accountId = InterceptedBIZFundsEngine.Commit(customerSessionId, trxnId, mgiContext, cardNumber);

			Assert.AreNotEqual(accountId, 0);
		}

		[Test]
		[ExpectedException(typeof(FundException))]
		public void Can_Get_Account_With_Exception()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext(){ AlloyId = 1000000001};
			mgiContext.ChannelPartnerName = "TCF";

			MGI.Biz.FundsEngine.Data.FundsAccount fundAccount = InterceptedBIZFundsEngine.GetAccount(customerSessionId, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(FundException))]
		public void Can_Get_Account_With_Exception_MajorCode()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { AlloyId = 1000000002 };
			mgiContext.ChannelPartnerName = "TCF";

			MGI.Biz.FundsEngine.Data.FundsAccount fundAccount = InterceptedBIZFundsEngine.GetAccount(customerSessionId, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_UpdateCardStatus_WithOut_PTNRAccount()
		{
			long customerSessionId = 2000000015;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";
			MGI.Biz.FundsEngine.Data.CardMaintenanceInfo cardMaintenanceInfo = new MGI.Biz.FundsEngine.Data.CardMaintenanceInfo() { };

			bool status = InterceptedBIZFundsEngine.UpdateCardStatus(customerSessionId, cardMaintenanceInfo, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Close_Account_WithOut_PTNRAccount()
		{
			long customerSessionId = 2000000015;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";

			bool status = InterceptedBIZFundsEngine.CloseAccount(customerSessionId, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Replace_Card_WithOut_PTNRAccount()
		{
			long customerSessionId = 2000000015;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";
			MGI.Biz.FundsEngine.Data.CardMaintenanceInfo cardMaintenanceInfo = new MGI.Biz.FundsEngine.Data.CardMaintenanceInfo() { };

			bool status = InterceptedBIZFundsEngine.ReplaceCard(customerSessionId, cardMaintenanceInfo, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Get_Balance_WithOut_PTNRAccount()
		{
			long customerSessionId = 1000000100;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			MGI.Biz.FundsEngine.Data.CardInfo cardBalance = InterceptedBIZFundsEngine.GetBalance(customerSessionId, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Get_Balance_WithOut_CXEAccount()
		{
			long customerSessionId = 1000000102;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			MGI.Biz.FundsEngine.Data.CardInfo cardBalance = InterceptedBIZFundsEngine.GetBalance(customerSessionId, mgiContext);
		}

		[Test]
		public void Can_Get_Minimun_Load_To_Cart()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;

			decimal initialLoad = InterceptedBIZFundsEngine.GetMinimumLoadAmount(customerSessionId, false, mgiContext);

			Assert.AreNotEqual(initialLoad, 0);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Cancel_WithOut_PTNRAccount()
		{
			long customerSessionId = 2000000015;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";
			long fundsId = 1000000004;

			InterceptedBIZFundsEngine.Cancel(customerSessionId, fundsId, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Validate_Withdraw_Amount()
		{
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 0,
				Fee = 5
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.Withdraw(customerSessionId, fund, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Validate_Withdraw_Amount_Agains_Fee()
		{
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 5,
				Fee = 10
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.Withdraw(customerSessionId, fund, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Update_Amount_WithOut_PTNRAccount()
		{
			long customerSessionId = 2000000015;
			decimal amount = 10;
			long trxnId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.UpdateAmount(trxnId, amount, customerSessionId, FundType.Debit, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizComplianceLimitException))]
		public void Can_Update_Amount_Agains_Compliance()
		{
			long customerSessionId = 1000000001;
			decimal amount = 110;
			long trxnId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.UpdateAmount(trxnId, amount, customerSessionId, FundType.Debit, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Get_Transaction_History_WithOut_CXNId()
		{
			long customerSessionId = 2000000015;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";
			MGI.Biz.FundsEngine.Data.TransactionHistoryRequest request = new MGI.Biz.FundsEngine.Data.TransactionHistoryRequest();

			var history = InterceptedBIZFundsEngine.GetTransactionHistory(customerSessionId, request, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Order_CompanionCard_WithOut_CXNId()
		{
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 100,
				Fee = 10
			};

			long accountId = InterceptedBIZFundsEngine.IssueAddOnCard(customerSessionId, fund, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Load_Amount_WithOut_CXNFund()
		{
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 56,
				Fee = 5
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.Load(customerSessionId, fund, mgiContext);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Load_Amount_WithOut_CXNAccount()
		{
			long customerSessionId = 1000000001;
			Funds fund = new Funds()
			{
				Amount = 58,
				Fee = 5
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "TCF";

			long accountId = InterceptedBIZFundsEngine.Load(customerSessionId, fund, mgiContext);
		}

		[Test]
		public void Can_Get_All_Trxn()
		{
			long customerSessionId = 1000000001;
			long trxnId = 1000000006;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 34;
			mgiContext.ChannelPartnerName = "TCF";

			Funds fund = InterceptedBIZFundsEngine.Get(customerSessionId, trxnId, mgiContext);

			Assert.IsNotNull(fund);

			trxnId = 1000000007;
			fund = InterceptedBIZFundsEngine.Get(customerSessionId, trxnId, mgiContext);

			Assert.IsNotNull(fund);

			trxnId = 1000000008;
			fund = InterceptedBIZFundsEngine.Get(customerSessionId, trxnId, mgiContext);

			Assert.IsNotNull(fund);

			trxnId = 1000000009;
			fund = InterceptedBIZFundsEngine.Get(customerSessionId, trxnId, mgiContext);

			Assert.IsNotNull(fund);
		}

		[Test]
		[ExpectedException(typeof(BizFundsException))]
		public void Can_Get_Account_WithOut_Card_Account()
		{
			long customerSessionId = 1000000104;
			MGIContext mgiContext = new MGIContext() { };
			mgiContext.ChannelPartnerName = "TCF";

			MGI.Biz.FundsEngine.Data.FundsAccount fundAccount = InterceptedBIZFundsEngine.GetAccount(customerSessionId, mgiContext);
		}
	}
}