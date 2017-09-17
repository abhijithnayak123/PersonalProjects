using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using NHibernate;
using NHibernate.Context;
using MGI.Cxn.MoneyTransfer.WU.Impl;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.Contract;
using Spring.Testing.NUnit;
using MGI.Common.DataAccess.Contract;
using System.Security.Cryptography.X509Certificates;
using MGI.Cxn.WU.Common.Data;
using MGI.Cxn.WU.Common.Contract;
using MGI.Common.Util;
using MGI.Cxn.MoneyTransfer.WU.ReceiveMoneySearch;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyValidation;
using MGI.Cxn.MoneyTransfer.WU.ReceiveMoneyPay;

namespace MGI.Cxn.MoneyTransfer.WU.Test
{
	[TestFixture]
	public class WUMoneyTransferProcessorTest : AbstractTransactionalSpringContextTests
	{
		public IMoneyTransfer CXNMoneyTransferProcessor { get; set; }
		public IIO WUIO { private get; set; }
		public WUGateway WUGateway { private get; set; }
		public IMoneyTransfer moneytransfer { get; set; }
		public MGIContext MgiContext { get; set; }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.MoneyTransfer.WU.Test/MGI.Cxn.MoneyTransfer.WU.Test/CXNTestSpring.xml" }; }
		}

		//private Dictionary<string, object> context = new Dictionary<string, object>() 
		//{
		//	{"TimeZone", "Eastern Standard Time"},
		//	{"ChannelPartnerId", "33"},				
		//	{"LocationId", 13139925},
		//	{"ProcessorID", 14},
		//	{"ProviderID", 301},
		//	//{"SMTrxType", MTReleaseStatus.Hold},
		//	{"ReferenceNumber", DateTime.Now.ToString("yyyyMMddhhmmssff")},
		//	{"WUCounterId", 1313992501},
		//	{"CheckUserName" , 13139925}
		//};

		[SetUp]
		public void Setup()
		{
			//IApplicationContext ctx = ContextRegistry.GetContext();
			//Processor = (IMoneyTransfer)ctx.GetObject("MoneyTransfer");
			MgiContext = new MGIContext() {
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				WUCounterId = "1313992501",
				ProviderId = 301,
				ProcessorId = 14,
				SMTrxType = MTReleaseStatus.Hold.ToString(),
				ChannelPartnerId = 33,
				DestinationCountryCode = "SE",
				DestinationCurrencyCode = "SEK"
			};
		}

		[Test]
		public void Can_UpdateGoldCardPoints()
		{
			string cardpoints = "100";
			CXNMoneyTransferProcessor.UpdateGoldCardPoints(1000030041, cardpoints, MgiContext);

			TransactionRequest request = new TransactionRequest();
			request.TransactionId = 1000030041;

			Transaction transaction = CXNMoneyTransferProcessor.GetTransaction(request, MgiContext);
			string wucardPoints = transaction.LoyaltyCardPoints;

			Assert.That(wucardPoints, Is.EqualTo(cardpoints));
		}

		[Test]
		public void Can_SendCommitCXNMethod()
		{		

			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata["Country"] = "US";
			metadata["StateName"] = "CA";
			metadata["IsFixedOnSend"] = "True";
			
			DeliveryService service = new DeliveryService()
			{
				Code = "000",
				Name = "MONEY IN MINUTES"
			};
			FeeRequest request = new FeeRequest()
			{
				AccountId = 1000000000,
				Amount = 60,
				DeliveryService = service,
				ReceiverFirstName = "TestFirstName",
				ReceiverLastName = "TestLastName",
				ReceiveCountryCode = "SE",
				MetaData = metadata,
				ReceiverId = 1000000000,
				ReceiveCountryCurrency = "SEK"
			};

			FeeResponse response = CXNMoneyTransferProcessor.GetFee(request, MgiContext);
			ValidateRequest validaterequest = ValidateRequest(response, null);

			Assert.That(response, Is.Not.Null);
			//Assert.That(response.FeeInformations.Count, Is.GreaterThan(0));

			ValidateResponse mtresponse = CXNMoneyTransferProcessor.Validate(validaterequest, MgiContext);
			Assert.That(mtresponse, Is.Not.Null);
			Assert.IsTrue(mtresponse.TransactionId == response.TransactionId);

			bool iserror = CXNMoneyTransferProcessor.Commit(mtresponse.TransactionId, MgiContext);
			Assert.IsFalse(iserror);
		}


		private ValidateRequest ValidateRequest(FeeResponse response, MGI.Cxn.MoneyTransfer.Data.Receiver receiver)
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata["Country"] = "US";
			metadata["State"] = "CA";
			metadata["ProceedWithLPMTError"] = "False";
			metadata["ExpectedPayoutStateCode"] = "";
			metadata["ExpectedPayoutCity"] = "";

			ValidateRequest request = new ValidateRequest()
			{
				DateOfBirth = "1980-10-10",
				ReceiverId = receiver != null ? receiver.Id : 0,
				ReceiverFirstName = receiver != null ? receiver.FirstName : "TestFirstName",
				ReceiverLastName = receiver != null ? receiver.LastName : "TestLastName",
				ReceiveCurrency = "USD",
				State = "CA",
				TransferType = MoneyTransferType.Send,
				TransactionId = response.TransactionId,
				DeliveryService = "000",
				PrimaryIdPlaceOfIssue = "CALIFORNIA",
				PrimaryIdCountryOfIssue = "UNITED STATES",
				CountryOfBirth = "UNITED STATES",
				PrimaryIdNumber = "k3210123",
				PrimaryIdType = "DRIVER'S LICENSE",
				MetaData = metadata,				
			};
			return request;
		}

		[Test]
		public void GetFee()
		{
			DeliveryService ds = new DeliveryService();
			ds.Code = "002";
			ds.Name = "MONEY IN MINUTES";

			FeeRequest feeRequest = new FeeRequest()
			{
				AccountId = 1000000183,
			ReceiverId = 1000000000,
			
			FeeRequestType = MoneyTransfer.Data.FeeRequestType.AmountIncludingFee,
			Amount = 150.0M,
			DeliveryService = ds,
			IsDomesticTransfer = true,
			ReceiveAmount = 150.0M,
//			FeeRequestType = FeeRequestType.AmountIncludingFee,
			PersonalMessage = "This is a test",
			ReceiveCountryCode = "IN",
			ReceiveCountryCurrency = "INR",
			ReceiverFirstName = "A",
			ReceiverLastName = "B",
			MetaData = new Dictionary<string, object>()
		};
	
			feeRequest.MetaData.Add("StateName", "CALIFORNIA");
			var getFee = CXNMoneyTransferProcessor.GetFee(feeRequest, MgiContext);
			Assert.That(getFee, Is.Not.Null);
		}



		
				

		[Test]
		public void Commit()
		{
			long transactionId = 1000001244;
			var commit = CXNMoneyTransferProcessor.Commit(transactionId, MgiContext);
			Assert.That(commit, Is.Not.Null);
		}

		[Test]
		public void Modify()
		{
			long transactionId = 1000000084;
			CXNMoneyTransferProcessor.Modify(transactionId, MgiContext);
		}

		[Test]
		public void StageModify()
		{
			ModifyRequest modifyRequest = new ModifyRequest();
			modifyRequest.TransactionId = 1000000056;
			modifyRequest.FirstName = "FirstName";
			modifyRequest.LastName = "LastName";
			modifyRequest.MiddleName = "MiddleName";
			modifyRequest.SecondLastName = "SecondLastName";
			modifyRequest.ConfirmationNumber = "111111";
			modifyRequest.TestAnswer = "TestAnswer";
			modifyRequest.TestQuestion = "TestQuestion";
			modifyRequest.TestQuestionAvailable = "TestQuestionAvailable";

			var stageModify = CXNMoneyTransferProcessor.StageModify(modifyRequest, MgiContext);
			Assert.That(stageModify, Is.Not.Null);
		}

		[Test]
		public void DisplayWUCardAccountInfo()
		{
			long cxnAccountId = 1000000183;
			var displayWUCardAccountInfo = CXNMoneyTransferProcessor.DisplayWUCardAccountInfo(cxnAccountId);
			Assert.AreEqual(cxnAccountId, displayWUCardAccountInfo.Id);
		}

		[Test]
		public void GetWUCardAccount()
		{
			long cxnAccountId = 1000000001;
			var getWUCardAccount = CXNMoneyTransferProcessor.GetWUCardAccount(cxnAccountId);
			Assert.That(getWUCardAccount, Is.Not.Null);
		}

		[Test]
		public void WUCardEnrollment()
		{
			Account account = new Account()
			{
				Address = "Near KIMS College, BSK 3rd Stage",
				City = "Bangalore",
				ContactPhone = "9292939392",
				Email = "t@t.com",
				FirstName = "Krishnapratap",
				LastName = "Vedula",
				MobilePhone = "9388383848",
				PostalCode = "94010",
				LoyaltyCardNumber = "500584404",
				LevelCode = "1",
				State = "CA",
				SmsNotificationFlag = "True"
			};

			MGI.Cxn.MoneyTransfer.Data.PaymentDetails paymentDetails = new MoneyTransfer.Data.PaymentDetails();
			paymentDetails.TransactionId = 1000000175;

			var wuCardEnrollment = CXNMoneyTransferProcessor.WUCardEnrollment(account, paymentDetails, MgiContext);
			Assert.That(wuCardEnrollment, Is.Not.Null);
		}

		[Test]
		public void WUCardLookup()
		{
			MGI.Cxn.MoneyTransfer.Data.CardLookupDetails cardLookupDetails = new MGI.Cxn.MoneyTransfer.Data.CardLookupDetails();
			cardLookupDetails.AccountNumber = "1000000056";
			cardLookupDetails.firstname = "Krishnapratap";
			cardLookupDetails.lastname = "Vedula";

			var wuCardLookup = CXNMoneyTransferProcessor.WUCardLookup(1000000056, cardLookupDetails, MgiContext);
			Assert.That(wuCardLookup, Is.Not.Null);
		}

		[Test]
		public void UseGoldcard()
		{
			var useGoldcard = CXNMoneyTransferProcessor.UseGoldcard(1000000157, "501082963", MgiContext);
			Assert.That(useGoldcard, Is.Not.Null);
		}

		[Test]
		public void GetCardInfo()
		{
			var getCardInfo = CXNMoneyTransferProcessor.GetCardInfo("500584404", MgiContext);
			Assert.That(getCardInfo, Is.Not.Null);
		}

		[Test]
		public void GetAccount()
		{
			long cxnAccountId = 1000000183;
			var getAccount = CXNMoneyTransferProcessor.GetAccount(cxnAccountId, MgiContext);
			Assert.AreEqual(cxnAccountId, getAccount.Id);
		}

		[Test]
		public void AddAccount()
		{
			Account account = new Account()
			{
				Address = "Near KIMS College, BSK 3rd Stage",
				City = "Bangalore",
				ContactPhone = "9292939392",
				Email = "test@test.com",
				FirstName = "Krishnapratap",
				LastName = "Vedula",
				MobilePhone = "9388383848",
				PostalCode = "560085",
				LoyaltyCardNumber = "4658030394949494",
				LevelCode = "1",
				State = "Karnataka",
				SmsNotificationFlag = "True"
			};

			var addAccount = CXNMoneyTransferProcessor.AddAccount(account, MgiContext);
			Assert.That(addAccount, Is.Not.Null);
		}

		[Test]
		public void UpdateAccount()
		{
			Account account = new Account()
			{
				Address = "Near KIMS College, BSK 3rd Stage",
				City = "Bangalore",
				ContactPhone = "9292939392",
				Email = "test@test.com",
				FirstName = "Krishnapratap",
				LastName = "Vedula",
				MobilePhone = "9388383848",
				PostalCode = "560085",
				LoyaltyCardNumber = "4658030394949494",
				LevelCode = "1",
				State = "Karnataka",
				SmsNotificationFlag = "True",
				rowguid = new Guid("58B24E06-D127-4031-91C0-F9EC9DA09C0F")
			};

			var updateAccount = CXNMoneyTransferProcessor.UpdateAccount(account, MgiContext);
			Assert.That(updateAccount, Is.Not.Null);
		}		

		[Test]
		public void GetActiveReceiver()
		{
			long Id = 1000000000;
            var getActiveReceiver = CXNMoneyTransferProcessor.GetReceiver(Id);
			Assert.That(getActiveReceiver, Is.Not.Null);
		}		

		[Test]
		public void GetFrequentReceivers()
		{
			long Id = 1000000006;
			var getFrequentReceivers = CXNMoneyTransferProcessor.GetFrequentReceivers(Id);
			Assert.That(getFrequentReceivers, Is.Not.Null);
		}

		[Test]
		public void Can_DeleteFaviouteReceiver()
		{
			
			MGI.Cxn.MoneyTransfer.Data.Receiver receiver = new MGI.Cxn.MoneyTransfer.Data.Receiver();
			receiver.Id = 1000000000;

			var getActiveReceiver = CXNMoneyTransferProcessor.DeleteFavoriteReceiver(receiver, MgiContext);
			Assert.IsTrue(getActiveReceiver);
		}		

		[Test]
		public void GetBannerMsgs()
		{
			var getBannerMsgs = CXNMoneyTransferProcessor.GetBannerMsgs(MgiContext);
			Assert.That(getBannerMsgs, Is.Not.Empty);
		}

		[Test]
		public void GetDeliveryServices()
		{
			DeliveryServiceRequest request = new DeliveryServiceRequest()
			{
				CountryCode = "US",
				CountryCurrency = "USD",
				Type = DeliveryServiceType.Option
			};

			var deliveryServices = new List<DeliveryService>();

			string state = "California";
			string stateCode = "CA";
			string city = "California";
			string deliveryService = string.Empty;

			var getDeliveryServices = WUIO.GetDeliveryServices(request, state, stateCode, city, deliveryService, MgiContext);
			Assert.That(getDeliveryServices, Is.Not.Null);
		}

		[Test]
		public void GetStatus()
		{
			string confirmationNumber = "3290868661";
			var getStatus = CXNMoneyTransferProcessor.GetStatus(confirmationNumber, MgiContext);			
			Assert.That(getStatus, Is.Not.Null);
		}

		[Test]
		public void GetPastReceivers()
		{
			long customerSessionId = 9919199191919;
			string cardNumber = "500584404";
			var getPastReceivers = CXNMoneyTransferProcessor.GetPastReceivers(customerSessionId, cardNumber, MgiContext);
			Assert.That(getPastReceivers, Is.Not.Null);
		}

		[Test]
		public void GetReceiver()
		{
			long customerSessionId = 1000000000;
			var getReceiver = CXNMoneyTransferProcessor.GetReceiver(customerSessionId);
			Assert.That(getReceiver, Is.Not.Null);
		}

		[Test]
		public void GetRefundReasons()
		{
			ReasonRequest request = new ReasonRequest();
			request.TransactionType = "ReceiveMoney";
			var getRefundReasons = CXNMoneyTransferProcessor.GetRefundReasons(request, MgiContext);
			Assert.That(getRefundReasons, Is.Not.Null);
		}

		[Test]
		public void Search()
		{
			MGI.Cxn.MoneyTransfer.Data.SearchRequest searchRequest = new MoneyTransfer.Data.SearchRequest();
			searchRequest.SearchRequestType = SearchRequestType.Modify;
			searchRequest.TransactionId = 1000002284;
			searchRequest.ConfirmationNumber = "3290868661";

			var search = CXNMoneyTransferProcessor.Search(searchRequest, MgiContext);
			Assert.That(search, Is.Not.Null);			
		}
		/// <summary>
		/// Once send money transaction did will get the transaction id then pass it over here do the refund test cases will execute sucessfully other is wont work.
		/// </summary>
		[Test]
		public void Refund() //
		{
			MGI.Cxn.MoneyTransfer.Data.RefundRequest refundRequest = new RefundRequest();
			refundRequest.TransactionId = 1000002274;
			refundRequest.ReasonCode = "W9203";
			refundRequest.ReasonDesc = "RFD - Wrong Information";
			refundRequest.RefundStatus = "N";
			refundRequest.Comments = "Test";
			refundRequest.ReferenceNumber = null;

			var refund = CXNMoneyTransferProcessor.Refund(refundRequest, MgiContext);
			Assert.That(refund, Is.Not.Null);		
		}

		[Test]
		public void Validate()
		{
		    ValidateRequest validateRequest = new ValidateRequest();
			validateRequest.TransactionId = 1000000056;
			var validate = CXNMoneyTransferProcessor.Validate(validateRequest, MgiContext);			
			Assert.That(validate, Is.Not.Null);
		}

		[Test]
		public void GetTransaction()
		{
			TransactionRequest request = new TransactionRequest();
			request.TransactionId = 1000000044;

			var getTransaction = CXNMoneyTransferProcessor.GetTransaction(request, MgiContext);
			Assert.That(getTransaction, Is.Not.Null);			
		 }

		[Test]
		public void SaveReceiver()
		{
			MGI.Cxn.MoneyTransfer.Data.Receiver receiver = new MGI.Cxn.MoneyTransfer.Data.Receiver()
			{
				Address = "Bangalore",
				City = "Bangalore",
				Country = "India",
				CountryOfBirth = "India",
				CustomerId = null,
				DeliveryMethod = "Money",
				DeliveryOption = "SpeedMoney",
				DateOfBirth = DateTime.Now.AddYears(-23),
				DTServerCreate = DateTime.Now,
				DTServerLastModified = DateTime.Now,
                DTTerminalCreate = DateTime.Now,
                DTTerminalLastModified = DateTime.Now,
				FirstName = "Ashok",
				Gender = "Male",
				LastName = "LastName",
				Occupation = "Software Engineer",
				PhoneNumber = "9878767654",
				PickupCity = "Hyderabad",
				PickupCountry = "India",
				PickupState_Province = "Andhra Pradesh",
				rowguid = Guid.NewGuid(),
				SecondLastName = "Gandamaneni",
				State_Province = "Andhra Pradesh",
				Status = "Active",
				ZipCode = "5600028"
			};

			var saveReceiver = CXNMoneyTransferProcessor.SaveReceiver(receiver, MgiContext);
			Assert.That(saveReceiver, Is.Not.Null);
		}		

		[Test]
		public void GetFrequentRecipients()
		{
			long customerId = 1000000000000490;
			var getFrequentReceipients = CXNMoneyTransferProcessor.GetFrequentReceivers(customerId);
			Assert.That(getFrequentReceipients, Is.Not.Null);
		}	

        [Test]
        public void Get_Transalated_Country_Name()
        {
            string language = "es";
			string englishCountryName = "Afghanistan";
			string name = CXNMoneyTransferProcessor.GetCountryTransalation(englishCountryName, language);

			Assert.IsNotNull(name);
			Assert.AreEqual("Afganistán", name);
        }

        [Test]
        public void Get_Transalated_Delivery_Service_Name()
        {
            string language = "es";
			string englishDeliveryName = "DINERO  EN MINUTOS/IN MINUTES";
			string name = CXNMoneyTransferProcessor.GetDeliveryServiceTransalation(englishDeliveryName, language);

            Assert.IsNotNull(name);
			Assert.AreEqual("DINERO EN MINUTOS", name);
        }
	}	
}
