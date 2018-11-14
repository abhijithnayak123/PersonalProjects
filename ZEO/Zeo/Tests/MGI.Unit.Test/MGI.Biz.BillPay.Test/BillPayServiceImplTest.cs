using MGI.Cxn.BillPay.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using MGI.Core.CXE.Data;
using MGI.Common.Util;
using BizBillPayService = MGI.Biz.BillPay.Contract.IBillPayService;
using BizBillPay = MGI.Biz.BillPay;
using Moq;
using MGI.Biz.BillPay.Data;
using MGI.Biz.Compliance.Contract;

namespace MGI.Unit.Test
{
	[TestFixture]
	public class BillPaymentServiceImplTest : BaseClass_Fixture
	{
		public BizBillPayService BizBillPayService { private get; set; }

		#region Private Methods
		private Dictionary<string, object> GetMetaData()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "Location", "" } ,
                { "SessionCookie", "" } ,
                { "AailableBalance", "" } ,
                { "AccountHolder", "" } ,
                { "Attention", "" } ,
                { "Reference", "" },
                { "DateOfBirth", "" } ,
                { "DeliveryCode", "000" } 
            };
			return dictionary;
		}
		#endregion Private Methods

		MGIContext context = new MGIContext()
		{
			Context = new Dictionary<string, object>(),
			ProcessorId = 13,
			AgentId = 500021,
			TimeZone = "Eastern Standard Time",
			WUCounterId = "990000402",
			LocationName = "TCF Service Desk",
			ChannelPartnerId = 34,
			CheckUserName = "test"
		};

		[Test]
		public void ValidatePaymentTest()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();

			decimal billAmount = 100m;
			//decimal fee = 1m;
			long billerID = 14563;
			string BillerName = "REGIONAL ACCEPTANCE";

			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 33;
			context.CheckUserName = "test";

			long customerSessionId = 1000000006;
			context.TrxId = 1000000001;
			long transactionID = BizBillPayService.Validate(customerSessionId, new BizBillPay.Data.BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = 1M, billerID = billerID, MetaData = metadata }, context);

			CXEBillPayService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<TransactionStates>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.AtLeastOnce());

			Assert.That(transactionID, Is.AtLeast(1));
		}

		[Test]
		public void Can_Validate_BillPay_Trxn_With_No_TrxnId()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			decimal billAmount = 100m;
			long billerID = 14563;
			string BillerName = "REGIONAL ACCEPTANCE";
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ProviderId = 401;
			mgiContext.ChannelPartnerId = 33;
			long customerSessionId = 1000000006;

			long transactionID = BizBillPayService.Validate(customerSessionId, new BizBillPay.Data.BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = 1M, billerID = billerID, MetaData = metadata }, mgiContext);

			CXEBillPayService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<TransactionStates>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.AtLeastOnce());
			Assert.That(transactionID, Is.AtLeast(1));
		}

		[Test]
		public void CommitTest()
		{
			MGIContext context = new MGIContext();
			context.TimeZone = "Eastern Standard Time";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 34;
			context.RequestType = "RELEASE";
			context.ProviderId = 401;
			long customerSessionId = 1000000006;
			long transactionId = 1000000001;

			BizBillPayService.Commit(customerSessionId, transactionId, context);

			CXEBillPayService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());

			CXEBillPayService.Verify(moq => moq.Commit(It.IsAny<long>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Commit_Transaction_With_Out_ProviderId_Null_In_MGIContext()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			decimal billAmount = 100m;
			decimal fee = 1m;
			long billerID = 1234;
			string BillerName = "REGIONAL ACCEPTANCE";

			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.TimeZone = "Eastern Standard Time";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 33;
			context.RequestType = "RELEASE";

			long customerSessionId = 1000000006; // this won't work - just making it compile. Must get the customersession in the setup.
			var GetfeeTransaction = BizBillPayService.GetFee(customerSessionId, BillerName, "1234561830", 100, new BizBillPay.Data.Location(), context);
			context.TrxId = GetfeeTransaction.TransactionId;
			long transactionID = BizBillPayService.Validate(customerSessionId, new BizBillPay.Data.BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = fee, billerID = billerID, MetaData = metadata }, context);

			BizBillPayService.Commit(customerSessionId, transactionID, context);

			CXEBillPayService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());

			CXEBillPayService.Verify(moq => moq.Commit(It.IsAny<long>()), Times.AtLeastOnce());
		}

		[Test]
		public void GetCardInfoTest()
		{
			//Arrange
			MGIContext context = new MGIContext();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 34;
			context.CheckUserName = "test";
			long customerSessionId = 1000000006;
			//Act
			BizBillPay.Data.CardInfo cardInfo = BizBillPayService.GetCardInfo(customerSessionId, context);
			//Assert
			Assert.IsNotNull(cardInfo);
		}

		[Test]
		public void GetLocationsTest()
		{
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 34;
			context.CheckUserName = "test";
			long customerSessionId = 1000000006;

			BizBillPay.Data.BillPayLocation location = BizBillPayService.GetLocations(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, context);

			Assert.IsNotNull(location);
		}

		[Test]
		public void GetFeeTest()
		{
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ChannelPartnerId = 33;
			context.TrxId = 1000000001;

			BizBillPay.Data.Location location1 = new BizBillPay.Data.Location()
			{
				Id = "1000000022",
				Type = "03",
				Name = "Kingfisher Fl",
			};

			long customerSessionId = 1000000006;

			BizBillPay.Data.Fee fee = BizBillPayService.GetFee(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, location1, context);

			Assert.IsNotNull(fee);
		}

		[Test]
		public void UpdateWUCardDetailsTest()
		{
			long customerSessionId = 1000000006;
			long trxId = BizBillPayService.UpdateWUCardDetails(customerSessionId, "12121541245", context);
			Assert.That(trxId, Is.GreaterThan(0));
		}

		[Test]
		public void GetPreferredProductsTest()
		{
			long customerSessionId = 1000000000;

			List<BizBillPay.Data.Product> products = BizBillPayService.GetPreferredProducts(customerSessionId, customerSessionId, context);

			Assert.NotNull(products);
		}

		[Test]
		public void GetBillerLastTransactionTest()
		{
			long customerSessionId = 1000000000;

			BizBillPay.Data.BillPayTransaction bpTrx = BizBillPayService.GetBillerLastTransaction(customerSessionId, "14563", 1000000000000000, context);

			Assert.NotNull(bpTrx);
		}

		[Test]
		public void UpdateFavoriteBillerStatusTest()
		{
			long customerSessionId = 1000000006;

			bool isUpdated = BizBillPayService.UpdateFavoriteBillerStatus(customerSessionId, 1234561830, true, context);

			Assert.That(isUpdated, Is.True);
		}

		[Test]
		public void UpdateFavoriteBillerAccountNumberTest()
		{
			long customerSessionId = 1000000006;

			bool isUpdated = BizBillPayService.UpdateFavoriteBillerAccountNumber(customerSessionId, 1234561830, "1234561830", context);

			Assert.That(isUpdated, Is.True);
		}

		[Test]
		public void GetBillerInfoTest()
		{
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 34;
			context.ChannelPartnerName = "1";
			context.ChannelPartnerRowGuid = Guid.Parse("E46A6297-77D1-4AC9-9548-ECD75DE3E66E");
			context.TrxId = 1000014984;

			long customerSessionId = 1000000006;

			var billerInfo = BizBillPayService.GetBillerInfo(customerSessionId, "REGIONAL ACCEPTANCE", context);
			Assert.NotNull(billerInfo.BillerState);
		}

		[Test]
		public void GetProviderAttributesTest()
		{
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 34;
			context.ChannelPartnerName = "1";
			context.ChannelPartnerRowGuid = Guid.Parse("E46A6297-77D1-4AC9-9548-ECD75DE3E66E");
			context.TrxId = 1000014984;

			long customerSessionId = 1000000006;

			var providerAtrributes = BizBillPayService.GetProviderAttributes(customerSessionId, "REGIONAL ACCEPTANCE", "Kingfisher Fl", context);
			Assert.NotNull(providerAtrributes);
			Assert.NotNull(providerAtrributes[0].Label);
		}

		[Test]
		public void GetBillPayFeeTest()
		{
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 34;
			context.ChannelPartnerName = "1";
			context.ChannelPartnerRowGuid = Guid.Parse("E46A6297-77D1-4AC9-9548-ECD75DE3E66E");
			context.TrxId = 1000014984;

			long customerSessionId = 1000000000;
			decimal fee = BizBillPayService.GetBillPayFee(customerSessionId, "Western Union", context);

			Assert.That(fee, Is.GreaterThan(0));
		}

		[Test]
		public void Can_Add_BillPay_Trxn()
		{
			long customerSessionId = 1000000006;
			long transactionID = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ProviderId = 401;
			mgiContext.ChannelPartnerId = 33;

			BizBillPayService.Add(customerSessionId, transactionID, mgiContext);

			BillPayService.Verify(moq => moq.Commit(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Add_BillPay_Trxn_WithOut_ProviderId()
		{
			long customerSessionId = 1000000006;
			long transactionID = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerId = 33;

			BizBillPayService.Add(customerSessionId, transactionID, mgiContext);

			BillPayService.Verify(moq => moq.Commit(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_FavoriteBiller()
		{
			long customerSessionId = 1000000006;
			string BillerName = "REGIONAL ACCEPTANCE";
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34 };

			FavoriteBiller favoriteBiller = BizBillPayService.GetFavoriteBiller(customerSessionId, BillerName, mgiContext);

			Assert.IsNotNull(favoriteBiller);
		}

		[Test]
		public void Can_Get_BillPay_Trxn()
		{
			long customerSessionId = 1000000006;
			long transactionID = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerId = 33;

			BizBillPay.Data.BillPayTransaction billPayTrxn = BizBillPayService.GetTransaction(customerSessionId, transactionID, mgiContext);

			Assert.IsNotNull(billPayTrxn);
		}

		[Test]
		public void Can_Add_Past_Biller()
		{
			long customerSessionId = 1000000006;
			string cardNumber = "123456";
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerId = 33;

			BizBillPayService.AddPastBillers(customerSessionId, cardNumber, mgiContext);

			BillPayService.Verify(moq => moq.GetPastBillers(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Add_Favorite_Biller()
		{
			long customerSessionId = 1000000006;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerId = 33;
			FavoriteBiller favoriteBiller = new FavoriteBiller() { BillerId = "14563", ProviderId = 401 };

			BizBillPayService.AddFavoriteBiller(customerSessionId, favoriteBiller, mgiContext);

			CXEBillPaySetup.Verify(m => m.Update(It.IsAny<CustomerPreferedProduct>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Cancel_BillPay_Trxn()
		{
			long customerSessionId = 1000000006;
			long transactionID = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerId = 33;

			BizBillPayService.Cancel(customerSessionId, transactionID, mgiContext);

			CXEBillPayService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Delete_Favorite_Biller()
		{
			long customerSessionId = 1000000006;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerId = 33;
			long billerId = 14563;

			BizBillPayService.DeleteFavoriteBiller(customerSessionId, billerId, mgiContext);

			CXEBillPaySetup.Verify(moq => moq.Update(It.IsAny<CustomerPreferedProduct>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Update_Transaction_Status_To_Failed()
		{
			long customerSessionId = 1000000006;
			long transactionID = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerId = 33;

			BizBillPayService.UpdateTransactionStatus(customerSessionId, transactionID, mgiContext);

			CXEBillPayService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		[ExpectedException(typeof(BizComplianceLimitException))]
		public void ValidatePaymentTest_For_LessAmount()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			long billerID = 14563;
			string BillerName = "REGIONAL ACCEPTANCE";

			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 33;
			context.CheckUserName = "test";

			long customerSessionId = 1000000006;
			context.TrxId = 1000000001;
			decimal billAmount = 5m;
			BizBillPayService.Validate(customerSessionId, new BizBillPay.Data.BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = 1M, billerID = billerID, MetaData = metadata }, context);			
		}

		[Test]
		[ExpectedException(typeof(BizComplianceLimitException))]
		public void ValidatePaymentTest_For_MoreAmount()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			long billerID = 14563;
			string BillerName = "REGIONAL ACCEPTANCE";

			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 33;
			context.CheckUserName = "test";

			long customerSessionId = 1000000006;
			context.TrxId = 1000000001;
			decimal billAmount = 200m;
			BizBillPayService.Validate(customerSessionId, new BizBillPay.Data.BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = 1M, billerID = billerID, MetaData = metadata }, context);
		}
	

		[Test]
		public void GetLocationsTest_No_TrnxID()
		{
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 34;
			context.CheckUserName = "test";
			context.TrxId = 1000000006;
			long customerSessionId = 1000000006;

			BizBillPay.Data.BillPayLocation location = BizBillPayService.GetLocations(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, context);

			Assert.IsNotNull(location);
		}

		[Test]
		public void Can_Add_Past_Biller_Without_ChannelPartnerId()
		{
			long customerSessionId = 1000000006;
			string cardNumber = "123456";
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();

			BizBillPayService.AddPastBillers(customerSessionId, cardNumber, mgiContext);

			BillPayService.Verify(moq => moq.GetPastBillers(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Add_Favorite_Biller_Without_BillerID()
		{
			long customerSessionId = 1000000006;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerId = 33;
			FavoriteBiller favoriteBiller = new FavoriteBiller();
			BizBillPayService.AddFavoriteBiller(customerSessionId, favoriteBiller, mgiContext);

			CXEBillPaySetup.Verify(m => m.Create(It.IsAny<CustomerPreferedProduct>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Occuptation_WithoutTrnxId()
		{
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ChannelPartnerId = 33;
			context.TrxId = 1000000001;

			BizBillPay.Data.Location location1 = new BizBillPay.Data.Location()
			{
				Id = "1000000022",
				Type = "03",
				Name = "Kingfisher Fl",
			};

			long customerSessionId = 1000000013;

			BizBillPay.Data.Fee fee = BizBillPayService.GetFee(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, location1, context);

			Assert.IsNotNull(fee);
		}

		[Test]
		public void Can_Encrypt()
		{
			long customerSessionId = 1000000006;
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ChannelPartnerId = 33;
			FavoriteBiller favoriteBiller = new FavoriteBiller();
			favoriteBiller.AccountNumber = "4756755000044401";
			BizBillPayService.AddFavoriteBiller(customerSessionId, favoriteBiller, mgiContext);

			CXEBillPaySetup.Verify(m => m.Create(It.IsAny<CustomerPreferedProduct>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Decrypt()
		{
			long customerSessionId = 1000000006;
			string BillerName = "REGIONAL ACCEPTANCE";
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 28 };

			FavoriteBiller favoriteBiller = BizBillPayService.GetFavoriteBiller(customerSessionId, BillerName, mgiContext);
			Assert.IsNotNull(favoriteBiller);
		}

		[Test]
		public void Can_Add_New_Favorite_Biller()
		{
			long customerSessionId = 1000000006;
			string BillerName = "REGIONAL ACCEPTANCE";
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 28 };
			FavoriteBiller favoriteBiller = BizBillPayService.GetFavoriteBiller(customerSessionId, BillerName, mgiContext);
			Assert.IsNotNull(favoriteBiller);
		}
		[Test]
		public void GetBillerLastTransactionTest_With_No_Account()
		{
			long customerSessionId = 1000000000;
			context.ChannelPartnerId = 28;
			BizBillPay.Data.BillPayTransaction bpTrx = BizBillPayService.GetBillerLastTransaction(customerSessionId, "1234561830", 1000000000000000, context);
			Assert.IsNull(bpTrx);
		}

		[Test]
		public void Can_GetCustomer_MobileNumber_IsFirstPhone()
		{
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ProcessorId = 13;
			context.AgentId = 500021;
			context.TimeZone = "Eastern Standard Time";
			context.WUCounterId = "990000402";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 34;
			context.CheckUserName = "test";
			context.TrxId = 1000000006;
			long customerSessionId = 1000000013;

			BizBillPay.Data.BillPayLocation location = BizBillPayService.GetLocations(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, context);

			Assert.IsNotNull(location);
		}

		[Test]
		public void Can_GetFee_Without_Emp_Details()
		{
			long customerSessionId = 1000000017;
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ChannelPartnerId = 34;
			context.TrxId = 1000000001;
			BizBillPay.Data.Location location1 = new BizBillPay.Data.Location()
			{
				Id = "1000000022",
				Type = "03",
				Name = "Kingfisher Fl",
			};
			BizBillPay.Data.Fee fee = BizBillPayService.GetFee(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, location1, context);

			Assert.IsNotNull(fee);
		}
		[Test]
		public void Can_Commit_For_RequestType_Cancel()
		{
			MGIContext context = new MGIContext();
			context.TimeZone = "Eastern Standard Time";
			context.LocationName = "TCF Service Desk";
			context.ChannelPartnerId = 34;
			context.RequestType = "CANCEL";
			context.ProviderId = 401;
			long customerSessionId = 1000000006;
			long transactionId = 1000000001;
			BizBillPayService.Commit(customerSessionId, transactionId, context);
		}
		[Test]
		public void Can_GetFee_With_Occupation()
		{
			long customerSessionId = 1000000018;
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ChannelPartnerId = 34;
			context.TrxId = 1000000001;
			BizBillPay.Data.Location location1 = new BizBillPay.Data.Location()
			{
				Id = "1000000022",
				Type = "03",
				Name = "Kingfisher Fl",
			};
			BizBillPay.Data.Fee fee = BizBillPayService.GetFee(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, location1, context);

			Assert.IsNotNull(fee);
		}
		[Test]
		public void Can_Validate_BillPay_With_NegativeTrnxID()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			decimal billAmount = 100m;
			long billerID = 14563;
			string BillerName = "REGIONAL ACCEPTANCE";
			MGIContext mgiContext = new MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.ProviderId = 401;
			mgiContext.ChannelPartnerId = 33;
			mgiContext.TrxId = -1;
			long customerSessionId = 1000000006;

			long transactionID = BizBillPayService.Validate(customerSessionId, new BizBillPay.Data.BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = 1M, billerID = billerID, MetaData = metadata }, mgiContext);
			Assert.AreEqual(transactionID, 0);
		}
		[Test]
		public void Can_Update_PreferredProducts()
		{
			long customerSessionId = 1000000006;
			string cardNumber = "123456";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 28;
			mgiContext.Context = new Dictionary<string, object>();

			BizBillPayService.AddPastBillers(customerSessionId, cardNumber, mgiContext);

			BillPayService.Verify(moq => moq.GetPastBillers(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		//[Test]
		//public void Can_Stage_BillPayTrns()
		//{
		//	MGIContext context = new MGIContext();
		//	context.Context = new Dictionary<string, object>();
		//	context.ProcessorId = 13;
		//	context.AgentId = 500021;
		//	context.TimeZone = "Eastern Standard Time";
		//	context.WUCounterId = "990000402";
		//	context.LocationName = "TCF Service Desk";
		//	context.ChannelPartnerId = 34;
		//	context.CheckUserName = "test";
		//	long customerSessionId = 1000000019;

		//	BizBillPay.Data.BillPayLocation location = BizBillPayService.GetLocations(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, context);

		//	Assert.IsNotNull(location);
		//}
		[Test]
		public void Can_Create_PTNRTrnx_Without_TrnxID()
		{
			long customerSessionId = 1000000018;
			MGIContext context = new MGIContext();
			context.Context = new Dictionary<string, object>();
			context.ChannelPartnerId = 34;
			context.TrxId = -1;
			BizBillPay.Data.Location location1 = new BizBillPay.Data.Location()
			{
				Id = "1000000022",
				Type = "03",
				Name = "Kingfisher Fl",
			};
			BizBillPay.Data.Fee fee = BizBillPayService.GetFee(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, location1, context);

			Assert.IsNotNull(fee);
		}
		//[Test]
		//public void Can_Get_BillPayAccount_Without_AccountID()
		//{
		//	MGIContext context = new MGIContext();
		//	context.Context = new Dictionary<string, object>();
		//	context.ProcessorId = 13;
		//	context.AgentId = 500021;
		//	context.TimeZone = "Eastern Standard Time";
		//	context.WUCounterId = "990000402";
		//	context.ChannelPartnerId = 28;
		//	context.CheckUserName = "test";
		//	long customerSessionId = 1000000018;

		//	BizBillPay.Data.BillPayLocation location = BizBillPayService.GetLocations(customerSessionId, "REGIONAL ACCEPTANCE", "1", 100, context);

		//	Assert.IsNotNull(location);
		//}
	}
}

