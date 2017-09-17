using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using Spring.Context;
using dmsMasterData = MGI.Channel.Shared.Server.Data.XferMasterData;


namespace MGI.Channel.MVA.Server.Test
{
	[TestFixture]
	class MoneyTransferEngineFixture
	{
		#region Test Data
		IMVAService MVAEngineTest { get; set; }
		private static string MVA_ENGINE = "MVAEngine";
		private static string channelPartnerName = string.Empty;
		private static string moneyTransferProvider = string.Empty;
		private static string cardNumber = "5138800098705584";
		#endregion

		#region Receiver
		Receiver receiverdto = new Receiver()
		{
			Id = 0,
			FirstName = "Testpp",
			LastName = "Receiver",
			SecondLastName = "Lastname",
			MiddleName = "Middle",
			Status = "Active",
			Address = "111 Anza Blvd",
			City = "Burlingame",
			State_Province = "CA",
			ZipCode = "94010",
			PhoneNumber = "6501234567",
			PickupCountry = "USA",
			PickupState_Province = "CA",
			PickupCity = "Burlingame",
			DeliveryMethod = "000",
			DeliveryOption = "002",
			IsReceiverHasPhotoId = true,
			SecurityAnswer = "SecurityAnswer",
			SecurityQuestion = "SecurityQuestion",
		};

		#endregion



		#region XferPaymentDetails
		XferPaymentDetails paymentDetails = new XferPaymentDetails()
		{
			DeliveryMethod = "000",
			DeliveryOption = null,
			DeliveryOptionDesc = null,
			DeliveryServiceDesc = "MONEY IN MINUTES",
			DestinationCountryCode = "USA",
			DestinationCurrencyCode = "USD",
			DestinationPrincipalAmount = 0,
			DestinationState = "CALIFORNIA",
			ExchangeRate = 0,
			ExpectedPayoutCityCode = null,
			ExpectedPayoutLocCity = null,
			ExpectedPayoutStateCode = "CA",
			Fee = 0,// + sendMoney.OtherFees + sendMoney.TransferTax,// + sendMoney.OtherTaxes,
			IsDomesticTransfer = true,
			IsFixedOnSend = true,
			MessageCharge = 0,

			OriginatingCountryCode = "USA",
			OriginatingCurrencyCode = "USD",
			OriginatorsPrincipalAmount = 50,

			OtherFees = 0,
			PaymentType = null,
			PersonalMessages = null,
			ProceedWithLPMTError = false,
			PromoCodeDescription = null,
			PromoMessage = null,
			PromoName = null,
			PromotionDiscount = 0,
			PromotionError = null,
			PromotionsCode = string.Empty,

			ReceiverFirstName = "Test",
			ReceiverLastName = "Receiver",
			ReceiverSecondLastName = "Lastname",
			RecordingcountrycurrencyCountryCode = "USA",
			RecordingcountrycurrencyCurrencyCode = "USD",
			ReferenceNo = null,
			TestAnswer = null,
			TestQuestion = null,
			TestQuestionAvailable = null,
			TranascationType = "1",
			TransactionId = 0,
			TransferTax = 0,
		};

		FeeRequest MGFeeRequest = new FeeRequest()
		{
			Amount = 206.00m,
			ReceiveCountryCode = "USA",
			ReceiveCountryCurrency = "USD",
			TransactionId = 0,
			ReceiveAmount = 0.00m,
			IsDomesticTransfer = true,
			PromoCode = string.Empty,
			ReceiverId = 0,
			ReceiverFirstName = string.Empty,
			ReceiverLastName = string.Empty,
			ReceiverSecondLastName = string.Empty,
			DeliveryService = new DeliveryService()
			{
				Code = "WILL_CALL",
				Name = "10 Minute Service"
			},


		};


		FeeRequest feeRequest = new FeeRequest()
		{
			Amount = 50.00m,
			ReceiveAmount = 0.00m,
			ReceiveCountryCode = "USA",
			ReceiveCountryCurrency = "USD",
			TransactionId = 0,
			//IsDomesticTransfer = IsDomesticTransfer(sendMoney.PickupCountry),
			PromoCode = string.Empty,
			ReceiverId = 0,
			DeliveryService = new DeliveryService()
			{
				Code = "000",
				Name = "MONEY IN MINUTES"
			},
			PersonalMessage = null,
			MetaData = new Dictionary<string, object>()
				{
					{"CityCode", string.Empty},
					{"CityName", string.Empty},
					{"StateCode", string.Empty},
					{"StateName", string.Empty},
					{"TestQuestionOption", string.Empty},
					{"TestQuestion", string.Empty},
					{"TestAnswer", string.Empty},
					{"IsFixedOnSend", string.Empty}		
				},
			ReferenceNo = string.Empty

		};

		#endregion

		[SetUp]
		public void Setup()
		{
			channelPartnerName = System.Configuration.ConfigurationManager.AppSettings["ChannelPartnerName"].ToString();
			moneyTransferProvider = System.Configuration.ConfigurationManager.AppSettings["MoneyTransferProvider"].ToString();
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			MVAEngineTest = (IMVAService)ctx.GetObject(MVA_ENGINE);
			MVAEngineTest.SetSelf(MVAEngineTest);

		}

		#region Money transfer Fixtures

		#region Money transfer Setup Fixtures

		[Test]
		public void GetPastTransactions()
		{

			long CustomerSessionId = 1000000207;

			var PastTransaction = MVAEngineTest.GetPastTransactions(CustomerSessionId, "MoneyTransfer");

			Assert.Greater(Convert.ToInt64(PastTransaction.Count), 0);

		}

		[Test]
		public void GetXfrGetXfrStatesTest()
		{
			int noOfContriesHasStates = 0;
			var Countrieslist = GetXfrCountries();

			Assert.Greater(Convert.ToInt64(Countrieslist.Count), 0);

			foreach (var country in Countrieslist)
			{
				List<dmsMasterData> statelist = new List<dmsMasterData>();
				if (moneyTransferProvider == "WesternUnion")
				{
					if (country.Code == "USA" || country.Code == "MX" || country.Code == "CA")
					{
						statelist = GetXfrGetXfrStates(country.Code);
						Trace.WriteLine(country.Name + " has " + statelist.Count + " States");
					}
				}
				else
				{
					statelist = GetXfrGetXfrStates(country.Code);
					Trace.WriteLine(country.Name + " has " + statelist.Count + " States");
				}

				if (statelist.Count != 0)
					noOfContriesHasStates += 1;
			}
			if (moneyTransferProvider == "WesternUnion")
				Assert.AreEqual(noOfContriesHasStates, 1);
			else
				Assert.AreEqual(4, noOfContriesHasStates);
		}

		[Test]
		public void GetXfrCountriesTest()
		{
			var Countrieslist = GetXfrCountries();
			Trace.WriteLine(Countrieslist.Count + " Contries found..! ");
			Assert.Greater(Convert.ToInt64(Countrieslist.Count), 0);
		}

		#endregion

		#region Money transfer Receiver Fixtures

		[Test]
		public void AddMGReceiverTest()
		{

			CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession("4855SJJSMNHG8315", channelPartnerName);
			Trace.WriteLine("customersession is initiated with Id - " + customerSession.CustomerSessionId);

			Receiver receiverdto = new Receiver()
			{
				FirstName = "Testxfr",
				LastName = "Receiver",
				SecondLastName = "Lastname",
				Status = "Active",
				PhoneNumber = "6501234567",
				PickupCountry = "USA",
				IsReceiverHasPhotoId = true,
				MiddleName = "Middelnamea",
				NickName = "nicjnshve"
				//DeliveryOption = feeInformation.DeliveryOption
				//Id = 0,
				//DeliveryMethod =feeInformation.DeliveryOption,
				//Address = "111 Anza Blvd",
				//City = "Burlingame",
				//State_Province = "CA",
				//ZipCode = "94010",
				//PickupState_Province = "CA",
				//PickupCity = "Burlingame",
			};

			long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);

			Trace.WriteLine("New receiver created with the ID :" + receiverId);
			Assert.Greater(receiverId, 0);

		}

		[Test]
		public void AddEditXferReceiverTest()
		{
			CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(string.Empty, channelPartnerName);
			Trace.WriteLine("customersession is initiated with Id - " + customerSession.CustomerSessionId);

			long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);

			Trace.WriteLine("New receiver created with the ID :" + receiverId);
			Assert.Greater(receiverId, 0);

			Receiver receiver = MVAEngineTest.GetReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverId);
			receiverdto.Id = receiverId;

			CompareReceiver(receiverdto, receiver);

			Trace.WriteLine("Get receiver is sucessfull");

			receiverdto.FirstName = "EditedFistname";
			var id = MVAEngineTest.EditReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);

			receiver = MVAEngineTest.GetReceiver(Convert.ToInt64(customerSession.CustomerSessionId), id);

			CompareReceiver(receiverdto, receiver);

			Trace.WriteLine("Update receiver is sucessfull");
		}

		[Test]
		public void GetReceiverTest()
		{
			long CustomerSessionId = 1000000005;

			IList<Receiver> receivers = MVAEngineTest.GetFrequentReceivers(Convert.ToInt64(CustomerSessionId));

			Receiver receiver = MVAEngineTest.GetReceiver(Convert.ToInt64(CustomerSessionId),
														  1000000001);
			Assert.NotNull(receiver);
		}
		[Test]
		public void GetFrequentReceiverTest()
		{
			long CustomerSessionId = 1000000005;
			IList<Receiver> receivers = MVAEngineTest.GetFrequentReceivers(Convert.ToInt64(CustomerSessionId));
			Assert.Greater(receivers.Count, 0);
		}

		#endregion

		#region Money transfer Trx Fixtures

		[Test]
		public void MGXferGetFeesTest()
		{
			CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
			Trace.WriteLine("Customer session is initiated with the Id : " + Convert.ToInt64(customerSession.CustomerSessionId) + "..!");
			Assert.Greater(Convert.ToInt64(customerSession.CustomerSessionId), 0);

			long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);

			feeRequest.ReceiverId = receiverId;
			Trace.WriteLine("Xfr get fee");
			FeeResponse fee = MVAEngineTest.GetXfrFee(Convert.ToInt64(customerSession.CustomerSessionId), MGFeeRequest);
			feeRequest.TransactionId = fee.TransactionId;
			FeeResponse fee1 = MVAEngineTest.GetXfrFee(Convert.ToInt64(customerSession.CustomerSessionId), feeRequest);

			Assert.IsTrue(fee.FeeInformations != null && fee.FeeInformations.Count > 0);
		}


		//This method was commented as we didnt have correct Promocode for the agent ID ==> PromoCode "7POFFUSD", "1DOFFUSD", "USD5POFF", "USD6POFF", "USD8POFF", "5FLATUP200" against Agent Id:- 30042575
		//[Test]
		//public void MVAGetFeeWithPromocode()
		//{
		//	//cardNumber = "6503894140";

		//	//CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
		//	//Trace.WriteLine("Customer session is initiated with the Id : " + Convert.ToInt64(customerSession.CustomerSessionId) + "..!");
		//	ArrayList _arraylist = new ArrayList { "7POFFUSD", "1DOFFUSD", "USD5POFF", "USD6POFF", "USD8POFF", "5FLATUP200" };
		//	foreach (var item in _arraylist)
		//	{
		//		try
		//		{


		//			long CustomerSessionId = 1000000005;

		//			decimal ammount = 50.00m;


		//			List<dmsMasterData> countrieslist = GetXfrCountries();
		//			Assert.Greater(Convert.ToInt64(countrieslist.Count), 0);

		//			FeeRequest MGFeeRequest = new FeeRequest()
		//			{
		//				Amount = ammount,
		//				ReceiveCountryCode = countrieslist.FirstOrDefault().Code,
		//				IsDomesticTransfer = IsDomesticTransfer(countrieslist.FirstOrDefault().Code),
		//			};

		//			Trace.WriteLine("Xfer get fee");
		//			FeeResponse feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(CustomerSessionId), MGFeeRequest);
		//			FeeInformation feeInfo = feeResponse.FeeInformations.FirstOrDefault();

		//			MGFeeRequest.PromoCode = item.ToString();
		//			MGFeeRequest.DeliveryService = feeInfo.DeliveryService;
		//			MGFeeRequest.ReceiveAgentId = feeInfo.ReceiveAgentId;
		//			MGFeeRequest.ReceiveCountryCurrency = feeInfo.ReceiveCurrencyCode;
		//			MGFeeRequest.ReferenceNo = feeInfo.ReferenceNumber;
		//			MGFeeRequest.TransactionId = feeResponse.TransactionId;

		//			Trace.WriteLine("Xfer get fee Again with promocode");
		//			feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(CustomerSessionId), MGFeeRequest);


		//			FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();

		//			Receiver receiverdto = new Receiver()
		//			{
		//				FirstName = "Testxfr",
		//				LastName = "Receiver",
		//				SecondLastName = "Lastname",
		//				Status = "Active",
		//				PhoneNumber = "650123456",
		//				PickupCountry = countrieslist.FirstOrDefault().Code,
		//				SecurityAnswer = "SecurityAnswer",
		//				SecurityQuestion = "SecurityQ",
		//				Address = "Address",
		//				City = "Berlinagem",
		//				MiddleName = "Mid",
		//				NickName = "kk",
		//				IsReceiverHasPhotoId = true,
		//				State_Province = "CA",
		//				ZipCode = "9412"
		//			};

		//			long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(CustomerSessionId), receiverdto);


		//			ValidateRequest validateRequest = new ValidateRequest()
		//			{
		//				ReceiverId = receiverId,
		//				Amount = feeInformation.Amount,
		//				Fee = feeInformation.Fee,
		//				Tax = feeInformation.Tax,
		//				TransactionId = feeResponse.TransactionId,
		//				TransferType = TransferType.SendMoney,
		//				ReferenceNumber = feeInformation.ReferenceNumber,
		//				State = receiverdto.State_Province,
		//				PersonalMessage = "Meess",
		//				IdentificationAnswer = receiverdto.SecurityAnswer,
		//				IdentificationQuestion = receiverdto.SecurityQuestion,
		//				DeliveryService = feeInformation.DeliveryService.Code,
		//				ReceiverFirstName = receiverdto.FirstName,
		//				ReceiverLastName = receiverdto.LastName,
		//				ReceiverSecondLastName = receiverdto.SecondLastName
		//			};
		//			ValidateResponse validateResponse = MVAEngineTest.ValidateXfr(Convert.ToInt64(CustomerSessionId), validateRequest);

		//			MoneyTransferTransaction trx = MVAEngineTest.GetReceiverLastTransaction(Convert.ToInt64(CustomerSessionId), receiverId);

		//			MVAEngineTest.Checkout(Convert.ToInt64(CustomerSessionId));
		//		}

		//		catch (Exception)
		//		{
					
		//		}
		//	}
		//}

		[Test]
		public void GetReceiverLastTransactionTest()
		{
			MoneyTransferTransaction trx = MVAEngineTest.GetReceiverLastTransaction
				(Convert.ToInt64(1000000016), 1000000104);

			Assert.GreaterOrEqual(trx.ProviderId, 0);
		}

		[Test]
		public void GetReceiverLastTransactionWithoughtTrxTest()
		{
			cardNumber = "6503894148";//"6503894148"
			CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);


			Receiver receiverdto = new Receiver()
			{
				FirstName = "TestEFG",
				LastName = "Receiver",
				SecondLastName = "Lastname",
				Status = "Active",
				PhoneNumber = "650123456",
				PickupCountry = "MEX",
				SecurityAnswer = "SecurityAnswer",
				SecurityQuestion = "SecurityQ",
				Address = "Address",
				City = "Berlinagem",
				MiddleName = "Mid",
				NickName = "kk",
				IsReceiverHasPhotoId = true,
				State_Province = "CA",
				ZipCode = "9412"
			};

			long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);


			MoneyTransferTransaction trx = MVAEngineTest.GetReceiverLastTransaction(Convert.ToInt64(customerSession.CustomerSessionId), receiverId);

			Assert.IsNull(trx);
		}

		[Test]
		public void GetReceiverLastTransactionWithTrxTest()
		{
			cardNumber = "6503894148";//"6503894148"
			CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);

			FeeRequest MGFeeRequest = new FeeRequest()
			{
				Amount = 40.00m,
				ReceiveCountryCode = "MEX",
				IsDomesticTransfer = IsDomesticTransfer("MEX"),
			};

			Trace.WriteLine("Xfer get fee");
			FeeResponse feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(customerSession.CustomerSessionId), MGFeeRequest);

			FeeInformation feeInformation = (FeeInformation)feeResponse.FeeInformations.Where(x => x.ReceiveCurrencyCode == "MXN" && x.DeliveryService.Code == "BANK_DEPOSIT" && x.ReceiveAgentName == "CIBANCO").First();

			Receiver receiverdto = new Receiver()
			{
				FirstName = "Testxfraasd",
				LastName = "Receiver",
				SecondLastName = "Lastname",
				Status = "Active",
				PhoneNumber = "650123456",
				PickupCountry = "MEX",
				SecurityAnswer = "SecurityAnswer",
				SecurityQuestion = "SecurityQ",
				Address = "Address",
				City = "Berlinagem",
				MiddleName = "Mid",
				NickName = "kk",
				IsReceiverHasPhotoId = true,
				State_Province = "CA",
				ZipCode = "9412"
			};

			long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);

			AttributeRequest attributeRequest = new AttributeRequest()
			{
				Amount = MGFeeRequest.Amount,
				DeliveryService = new DeliveryService() { Code = feeInformation.DeliveryService.Code },
				ReceiveCountry = MGFeeRequest.ReceiveCountryCode,
				ReceiveCurrencyCode = feeInformation.ReceiveCurrencyCode,
				ReceiveAgentId = feeInformation.ReceiveAgentId,
			};

			var providerAttr = MVAEngineTest.GetXfrProviderAttributes(Convert.ToInt64(customerSession.CustomerSessionId), feeResponse.TransactionId, attributeRequest);


			Trace.WriteLine("Validate Xfr");
			ValidateRequest validateRequest = new ValidateRequest()
			{
				ReceiverId = receiverId,
				Amount = feeInformation.Amount,
				Fee = feeInformation.Fee,
				Tax = feeInformation.Tax,
				TransactionId = feeResponse.TransactionId,
				TransferType = TransferType.SendMoney,
				PromoCode = MGFeeRequest.PromoCode,
				ReferenceNumber = feeInformation.ReferenceNumber,
				DeliveryService = feeInformation.DeliveryService.Code,
				ReceiverFirstName = receiverdto.FirstName
			};

			validateRequest.MetaData = new Dictionary<string, object>();
			validateRequest.FieldValues = new Dictionary<string, string>();
			validateRequest.MetaData.Add("receiveAgentID", feeInformation.ReceiveAgentId);
			if (providerAttr.Count != 0)
			{
				for (int i = 0; i < providerAttr.Count; i++)
				{
					if (providerAttr[i].DataType == "TextBox")
					{
						if (providerAttr[i].TagName == "customerReceiveNumber")
							validateRequest.MetaData.Add(providerAttr[i].TagName, string.Empty);
						else if (providerAttr[i].TagName == "receiverState")
							validateRequest.MetaData.Add(providerAttr[i].TagName, "cibanco");
						else if (providerAttr[i].TagName == "accountNumber")
							validateRequest.MetaData.Add(providerAttr[i].TagName, "997856438756120956");
					}
					if (providerAttr[i].DataType == "Dropdown")
						validateRequest.FieldValues.Add(providerAttr[i].TagName, providerAttr[i].Values["CIBANCO_143"]);
				}
			}

			ValidateResponse validateResponse = MVAEngineTest.ValidateXfr(Convert.ToInt64(customerSession.CustomerSessionId), validateRequest);

			MVAEngineTest.Checkout(Convert.ToInt64(customerSession.CustomerSessionId));

			MoneyTransferTransaction trx = MVAEngineTest.GetReceiverLastTransaction(Convert.ToInt64(customerSession.CustomerSessionId), receiverId);

			Assert.IsNotNull(trx);
		}

		//   <senderPhotoIdState /> <senderPhotoIdCountry /> are null in DIT2 DB
	//	[Test]
		//public void MGSendMoneyTransactionTest()
		//{

		//	//   <senderPhotoIdState /> <senderPhotoIdCountry /> are null in DIT2 DB

		//	long CustomerSessionId = 1000000090;
		//	// CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
		//	Trace.WriteLine("Customer session is initiated with the Id : " + Convert.ToInt64(CustomerSessionId) + "..!");
		//	Assert.Greater(Convert.ToInt64(CustomerSessionId), 0);

		//	Trace.WriteLine("Xfer get fee");
		//	FeeResponse feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(CustomerSessionId), MGFeeRequest);


		//	Trace.WriteLine("Add receiver");
		//	long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(CustomerSessionId), receiverdto);

		//	Trace.WriteLine("Validate Xfr");
		//	FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();


		//	Trace.WriteLine("GFFP Call");
		//	AttributeRequest attributeRequest = new AttributeRequest()
		//	{
		//		Amount = MGFeeRequest.Amount,
		//		DeliveryService = new DeliveryService() { Code = feeInformation.DeliveryService.Code },
		//		ReceiveCountry = MGFeeRequest.ReceiveCountryCode,
		//		ReceiveCurrencyCode = feeInformation.ReceiveCurrencyCode,
		//		ReceiveAgentId = feeInformation.ReceiveAgentId,
		//		TransferType = MGI.Channel.Shared.Server.Data.TransferType.SendMoney
		//	};


		//	var providerAttr = MVAEngineTest.GetXfrProviderAttributes(Convert.ToInt64(CustomerSessionId), feeResponse.TransactionId, attributeRequest);

		//	Trace.WriteLine("Validate Xfr");
		//	ValidateRequest validateRequest = new ValidateRequest()
		//	{
		//		ReceiverId = receiverId,
		//		Amount = feeInformation.Amount,
		//		Fee = feeInformation.Fee,
		//		Tax = feeInformation.Tax,
		//		TransactionId = feeResponse.TransactionId,
		//		TransferType = TransferType.SendMoney,
		//		PromoCode = feeRequest.PromoCode,
		//		State = "CA",
		//		ReferenceNumber = feeInformation.ReferenceNumber,
		//		DeliveryService = feeInformation.DeliveryService.Code,
		//	};
		//	ValidateResponse validateResponse = new ValidateResponse();
		//	validateResponse = MVAEngineTest.ValidateXfr(Convert.ToInt64(CustomerSessionId), validateRequest);

		//	Trace.WriteLine("Commit Xfr");
		//	MVAEngineTest.Checkout(Convert.ToInt64(CustomerSessionId));
		//	var MGMTtrx = MVAEngineTest.GetXfrTransaction(Convert.ToInt64(CustomerSessionId), validateResponse.TransactionId);
		//}


		//[Test]
		//public void MGSendMoneyTransactionExistingCustomerTest()
		//{
		//	long CustomerSessionId = 1000000207;
		////	CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
		////	Trace.WriteLine("Customer session is initiated with the Id : " + Convert.ToInt64(customerSession.CustomerSessionId) + "..!");
		////	Assert.Greater(Convert.ToInt64(customerSession.CustomerSessionId), 0);

		//	IList<Receiver> frequentReceivers = MVAEngineTest.GetFrequentReceivers(Convert.ToInt64(CustomerSessionId));

		//	long receiverId = frequentReceivers.FirstOrDefault().Id;
		//	Trace.WriteLine("Xfer get fee");
		//	MGFeeRequest.ReceiverId = receiverId;
		//	FeeResponse feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(CustomerSessionId), MGFeeRequest);

		//	FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();
		//	Trace.WriteLine("Validate Xfr");
		//	ValidateRequest validateRequest = new ValidateRequest()
		//	{
		//		ReceiverId = receiverId,
		//		Amount = feeInformation.Amount,
		//		Fee = feeInformation.Fee,
		//		Tax = feeInformation.Tax,
		//		TransactionId = feeResponse.TransactionId,
		//		TransferType = TransferType.SendMoney,
		//		PromoCode = feeRequest.PromoCode,
		//		State = "CA",
		//		ReferenceNumber = feeInformation.ReferenceNumber,
		//	};
		//	ValidateResponse validateResponse = MVAEngineTest.ValidateXfr(Convert.ToInt64(customerSession.CustomerSessionId), validateRequest);

		//	Trace.WriteLine("Commit Xfr");
		//	MVAEngineTest.Checkout(Convert.ToInt64(customerSession.CustomerSessionId));
		//}

		[Test]
		public void MVANewCustomerMTTest()
		{
			CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
			Trace.WriteLine("Customer session is initiated with the Id : " + Convert.ToInt64(customerSession.CustomerSessionId) + "..!");
			Assert.Greater(Convert.ToInt64(customerSession.CustomerSessionId), 0);

			Assert.IsTrue(customerSession.isNewCustomer);
			decimal ammount = 50.00m;

			List<dmsMasterData> countrieslist = GetXfrCountries();
			Trace.WriteLine(countrieslist.Count + " Contries found..! ");
			Assert.Greater(Convert.ToInt64(countrieslist.Count), 0);

			FeeRequest MGFeeRequest = new FeeRequest()
			{
				Amount = ammount,
				ReceiveCountryCode = countrieslist.FirstOrDefault().Code,
				IsDomesticTransfer = IsDomesticTransfer(countrieslist.FirstOrDefault().Code),
				//ReceiveCountryCurrency = countrieslist.FirstOrDefault().,
				//TransactionId = 0,
				//ReceiveAmount = 0.00m,
				//PromoCode = string.Empty,
				//ReceiverId = 0,
				//ReceiverFirstName = string.Empty,
				//ReceiverLastName = string.Empty,
				//ReceiverSecondLastName = string.Empty,
				//DeliveryService = ,
				//AccountId = ,
				//MetaData = ,
				//PersonalMessage = ,
				//ReferenceNo = 
			};

			Trace.WriteLine("Xfer get fee");
			FeeResponse feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(customerSession.CustomerSessionId), MGFeeRequest);

			FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();

			Receiver receiverdto = new Receiver()
			{
				FirstName = "Testxfr",
				LastName = "Receiver",
				SecondLastName = "Lastname",
				Status = "Active",
				PhoneNumber = "6501234567",
				PickupCountry = countrieslist.FirstOrDefault().Code,
				//DeliveryOption = feeInformation.DeliveryOption
				//Id = 0,
				//DeliveryMethod =feeInformation.DeliveryOption,
				//Address = "111 Anza Blvd",
				//City = "Burlingame",
				//State_Province = "CA",
				//ZipCode = "94010",
				//PickupState_Province = "CA",
				//PickupCity = "Burlingame",
			};

			Trace.WriteLine("Add receiver");
			long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);


			Trace.WriteLine("Validate Xfr");
			ValidateRequest validateRequest = new ValidateRequest()
			{
				ReceiverId = receiverId,
				Amount = feeInformation.Amount,
				Fee = feeInformation.Fee,
				Tax = feeInformation.Tax,
				TransactionId = feeResponse.TransactionId,
				TransferType = TransferType.SendMoney,
				ReferenceNumber = feeInformation.ReferenceNumber,
				State = "CA",
				DeliveryService = feeInformation.DeliveryService.Code
				//State = ,
				//MetaData = ,
				//PersonalMessage = ,
				//DeliveryService = ,
				//IdentificationAnswer = ,
				//IdentificationQuestion = ,
				//MessageFee = ,
				//OtherFee = ,
				//PromoCode =,
				//ReceiverFirstName = ,
				//ReceiverLastName = ,
				//ReceiverSecondLastName = 
			};
			ValidateResponse validateResponse = MVAEngineTest.ValidateXfr(Convert.ToInt64(customerSession.CustomerSessionId), validateRequest);

			Trace.WriteLine("Commit Xfr");
			MVAEngineTest.Checkout(Convert.ToInt64(customerSession.CustomerSessionId));

		}

		[Test]
		public void GetMoneyTransferTransaction()
		{
			cardNumber = "6503894148";

			CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
			Trace.WriteLine("Customer session is initiated with the Id : " + Convert.ToInt64(customerSession.CustomerSessionId) + "..!");
			Assert.Greater(Convert.ToInt64(customerSession.CustomerSessionId), 0);

			FeeRequest MGFeeRequest = new FeeRequest()
			{
				Amount = 41.00m,
				ReceiveCountryCode = "MEX",
				IsDomesticTransfer = IsDomesticTransfer("MEX"),
			};

			Trace.WriteLine("Xfer get fee");
			FeeResponse feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(customerSession.CustomerSessionId), MGFeeRequest);

			FeeInformation feeInformation = (FeeInformation)feeResponse.FeeInformations.Where(x => x.ReceiveCurrencyCode == "MXN" && x.DeliveryService.Code == "BANK_DEPOSIT" && x.ReceiveAgentName == "CIBANCO").First();

			Receiver receiverdto = new Receiver()
			{
				FirstName = "YYY",
				LastName = "Receiver",
				SecondLastName = "Lastname",
				Status = "Active",
				PhoneNumber = "650123456",
				PickupCountry = "MEX",
				SecurityAnswer = "SecurityAnswer",
				SecurityQuestion = "SecurityQ",
				Address = "Address",
				City = "Berlinagem",
				MiddleName = "Mid",
				NickName = "kk",
				IsReceiverHasPhotoId = true,
				State_Province = "CA",
				ZipCode = "9412"
			};

			long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);

			AttributeRequest attributeRequest = new AttributeRequest()
			{
				Amount = MGFeeRequest.Amount,
				DeliveryService = new DeliveryService() { Code = feeInformation.DeliveryService.Code },
				ReceiveCountry = MGFeeRequest.ReceiveCountryCode,
				ReceiveCurrencyCode = feeInformation.ReceiveCurrencyCode,
				ReceiveAgentId = feeInformation.ReceiveAgentId,
			};

			var providerAttr = MVAEngineTest.GetXfrProviderAttributes(Convert.ToInt64(customerSession.CustomerSessionId), feeResponse.TransactionId, attributeRequest);


			Trace.WriteLine("Validate Xfr");
			ValidateRequest validateRequest = new ValidateRequest()
			{
				ReceiverId = receiverId,
				Amount = feeInformation.Amount,
				Fee = feeInformation.Fee,
				Tax = feeInformation.Tax,
				TransactionId = feeResponse.TransactionId,
				TransferType = TransferType.SendMoney,
				PromoCode = MGFeeRequest.PromoCode,
				ReferenceNumber = feeInformation.ReferenceNumber,
				DeliveryService = feeInformation.DeliveryService.Code,
				ReceiverFirstName = receiverdto.FirstName
			};

			validateRequest.MetaData = new Dictionary<string, object>();
			validateRequest.FieldValues = new Dictionary<string, string>();
			validateRequest.MetaData.Add("receiveAgentID", feeInformation.ReceiveAgentId);
			if (providerAttr.Count != 0)
			{
				for (int i = 0; i < providerAttr.Count; i++)
				{
					if (providerAttr[i].DataType == "TextBox")
					{
						if (providerAttr[i].TagName == "customerReceiveNumber")
							validateRequest.MetaData.Add(providerAttr[i].TagName, string.Empty);
						else if (providerAttr[i].TagName == "receiverState")
							validateRequest.MetaData.Add(providerAttr[i].TagName, "cibanco");
						else if (providerAttr[i].TagName == "accountNumber")
							validateRequest.MetaData.Add(providerAttr[i].TagName, "997856438756120956");
					}
					if (providerAttr[i].DataType == "Dropdown")
						validateRequest.FieldValues.Add(providerAttr[i].TagName, providerAttr[i].Values["CIBANCO_143"]);
				}
			}

			ValidateResponse validateResponse = MVAEngineTest.ValidateXfr(Convert.ToInt64(customerSession.CustomerSessionId), validateRequest);

			MVAEngineTest.Checkout(Convert.ToInt64(customerSession.CustomerSessionId));

			var MGMTtrx = MVAEngineTest.GetXfrTransaction(Convert.ToInt64(customerSession.CustomerSessionId), validateResponse.TransactionId);

			Assert.IsNotNull(MGMTtrx);
		}

		[Test]
		public void SendMoneySearch()
		{
			CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
			Trace.WriteLine("Customer session is initiated with the Id : " + Convert.ToInt64(customerSession.CustomerSessionId) + "..!");
			Assert.Greater(Convert.ToInt64(customerSession.CustomerSessionId), 0);

			Trace.WriteLine("Xfer get fee");
			FeeResponse feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(customerSession.CustomerSessionId), MGFeeRequest);


			Trace.WriteLine("Add receiver");
			long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);

			Trace.WriteLine("Validate Xfr");
			FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();


			Trace.WriteLine("GFFP Call");
			AttributeRequest attributeRequest = new AttributeRequest()
			{
				Amount = MGFeeRequest.Amount,
				DeliveryService = new DeliveryService() { Code = feeInformation.DeliveryService.Code },
				ReceiveCountry = MGFeeRequest.ReceiveCountryCode,
				ReceiveCurrencyCode = feeInformation.ReceiveCurrencyCode,
				ReceiveAgentId = feeInformation.ReceiveAgentId,
                TransferType = MGI.Channel.Shared.Server.Data.TransferType.SendMoney
			};


			var providerAttr = MVAEngineTest.GetXfrProviderAttributes(Convert.ToInt64(customerSession.CustomerSessionId), feeResponse.TransactionId, attributeRequest);

			Trace.WriteLine("Validate Xfr");
			ValidateRequest validateRequest = new ValidateRequest()
			{
				ReceiverId = receiverId,
				Amount = feeInformation.Amount,
				Fee = feeInformation.Fee,
				Tax = feeInformation.Tax,
				TransactionId = feeResponse.TransactionId,
				TransferType = TransferType.SendMoney,
				PromoCode = feeRequest.PromoCode,
				State = "CA",
				ReferenceNumber = feeInformation.ReferenceNumber,
				DeliveryService = feeInformation.DeliveryService.Code,
			};

			ValidateResponse validateResponse = MVAEngineTest.ValidateXfr(Convert.ToInt64(customerSession.CustomerSessionId), validateRequest);
			Trace.WriteLine("Commit Xfr");
			MVAEngineTest.Checkout(Convert.ToInt64(customerSession.CustomerSessionId));
			var MGMTtrx = MVAEngineTest.GetXfrTransaction(Convert.ToInt64(customerSession.CustomerSessionId), validateResponse.TransactionId);




			// CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
			SendMoneySearchRequest req = new SendMoneySearchRequest()
											 {
												 ConfirmationNumber = MGMTtrx.ConfirmationNumber,
                                                 SearchRequestType =MGI.Channel.Shared.Server.Data.SearchRequestType.Modify,
											 };

			SendMoneySearchResponse response = MVAEngineTest.SendMoneySearch(Convert.ToInt64(customerSession.CustomerSessionId), req);
			Assert.IsNotNull(response.TransactionStatus);


		}
		[Test]
		//public void StageModifySendMoney()
		//{
		//	CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
		//	Trace.WriteLine("Customer session is initiated with the Id : " + Convert.ToInt64(customerSession.CustomerSessionId) + "..!");
		//	Assert.Greater(Convert.ToInt64(customerSession.CustomerSessionId), 0);

		//	Trace.WriteLine("Xfer get fee");
		//	FeeResponse feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(customerSession.CustomerSessionId), MGFeeRequest);


		//	Trace.WriteLine("Add receiver");
		//	long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);

		//	Trace.WriteLine("Validate Xfr");
		//	FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();


		//	Trace.WriteLine("GFFP Call");
		//	AttributeRequest attributeRequest = new AttributeRequest()
		//	{
		//		Amount = MGFeeRequest.Amount,
		//		DeliveryService = new DeliveryService() { Code = feeInformation.DeliveryService.Code },
		//		ReceiveCountry = MGFeeRequest.ReceiveCountryCode,
		//		ReceiveCurrencyCode = feeInformation.ReceiveCurrencyCode,
		//		ReceiveAgentId = feeInformation.ReceiveAgentId,
		//		TransferType = MGI.Channel.Shared.Server.Data.TransferType.SendMoney
		//	};


		//	var providerAttr = MVAEngineTest.GetXfrProviderAttributes(Convert.ToInt64(customerSession.CustomerSessionId), feeResponse.TransactionId, attributeRequest);

		//	Trace.WriteLine("Validate Xfr");
		//	ValidateRequest validateRequest = new ValidateRequest()
		//	{
		//		ReceiverId = receiverId,
		//		Amount = feeInformation.Amount,
		//		Fee = feeInformation.Fee,
		//		Tax = feeInformation.Tax,
		//		TransactionId = feeResponse.TransactionId,
		//		TransferType = TransferType.SendMoney,
		//		PromoCode = feeRequest.PromoCode,
		//		State = "CA",
		//		ReferenceNumber = feeInformation.ReferenceNumber,
		//		DeliveryService = feeInformation.DeliveryService.Code,
		//	};
		//	Trace.WriteLine("Validate Response");
		//	ValidateResponse validateResponse = MVAEngineTest.ValidateXfr(Convert.ToInt64(customerSession.CustomerSessionId), validateRequest);
		//	Trace.WriteLine("Commit Xfr");
		//	MVAEngineTest.Checkout(Convert.ToInt64(customerSession.CustomerSessionId));
		//	Trace.WriteLine("GetXfrTransaction");
		//	var MGMTtrx = MVAEngineTest.GetXfrTransaction(Convert.ToInt64(customerSession.CustomerSessionId), validateResponse.TransactionId);

		//	SendMoneySearchRequest req = new SendMoneySearchRequest()
		//	{
		//		ConfirmationNumber = MGMTtrx.ConfirmationNumber,
		//		SearchRequestType = MGI.Channel.Shared.Server.Data.SearchRequestType.Modify,
		//	};

		//	SendMoneySearchResponse response = MVAEngineTest.SendMoneySearch(Convert.ToInt64(customerSession.CustomerSessionId), req);
		//	Assert.IsNotNull(response.TransactionStatus);
		//	ModifySendMoneyRequest modifySendmoneyReq = new ModifySendMoneyRequest()
		//													{
		//														CancelTransactionId = 0,
		//														ConfirmationNumber = response.ConfirmationNumber,
		//														FirstName = "Testpp",
		//														LastName = "Receiver",
		//														MiddleName = "RAHUL",
		//														ModifyTransactionId = 0,
		//														SecondLastName = "pAMS",
		//														TransactionId = Convert.ToString(validateResponse.TransactionId)

		//													};
		//	ModifySendMoneyResponse modifiedsendResponse = MVAEngineTest.StageModifySendMoney(Convert.ToInt64(customerSession.CustomerSessionId),
		//										   modifySendmoneyReq);

		//	Assert.IsNotNull(modifiedsendResponse.CancelTransactionId);

		//	ModifySendMoneyRequest modifySendMoneyRequest = new ModifySendMoneyRequest()
		//														{
		//															CancelTransactionId = modifiedsendResponse.CancelTransactionId,
		//															ModifyTransactionId = modifiedsendResponse.ModifyTransactionId
		//														};

		//	MVAEngineTest.AuthorizeModifySendMoney(Convert.ToInt64(customerSession.CustomerSessionId), modifySendMoneyRequest);

		//	var receipt = MVAEngineTest.Checkout(Convert.ToInt64(customerSession.CustomerSessionId));


		//}

		//[Test]
		//public void GetStageRefundSendMoney()
		//{
		//	CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
		//	Trace.WriteLine("Customer session is initiated with the Id : " + Convert.ToInt64(customerSession.CustomerSessionId) + "..!");
		//	Assert.Greater(Convert.ToInt64(customerSession.CustomerSessionId), 0);

		//	Trace.WriteLine("Xfer get fee");
		//	FeeResponse feeResponse = MVAEngineTest.GetXfrFee(Convert.ToInt64(customerSession.CustomerSessionId), MGFeeRequest);


		//	Trace.WriteLine("Add receiver");
		//	long receiverId = MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);

		//	Trace.WriteLine("Validate Xfr");
		//	FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();


		//	Trace.WriteLine("GFFP Call");
		//	AttributeRequest attributeRequest = new AttributeRequest()
		//	{
		//		Amount = MGFeeRequest.Amount,
		//		DeliveryService = new DeliveryService() { Code = feeInformation.DeliveryService.Code },
		//		ReceiveCountry = MGFeeRequest.ReceiveCountryCode,
		//		ReceiveCurrencyCode = feeInformation.ReceiveCurrencyCode,
		//		ReceiveAgentId = feeInformation.ReceiveAgentId,
		//		TransferType = MGI.Channel.Shared.Server.Data.TransferType.SendMoney
		//	};


		//	var providerAttr = MVAEngineTest.GetXfrProviderAttributes(Convert.ToInt64(customerSession.CustomerSessionId), feeResponse.TransactionId, attributeRequest);

		//	Trace.WriteLine("Validate Xfr");
		//	ValidateRequest validateRequest = new ValidateRequest()
		//	{
		//		ReceiverId = receiverId,
		//		Amount = feeInformation.Amount,
		//		Fee = feeInformation.Fee,
		//		Tax = feeInformation.Tax,
		//		TransactionId = feeResponse.TransactionId,
		//		TransferType = TransferType.SendMoney,
		//		PromoCode = feeRequest.PromoCode,
		//		State = "CA",
		//		ReferenceNumber = feeInformation.ReferenceNumber,
		//		DeliveryService = feeInformation.DeliveryService.Code,
		//	};
		//	Trace.WriteLine("Validate Response");
		//	ValidateResponse validateResponse = MVAEngineTest.ValidateXfr(Convert.ToInt64(customerSession.CustomerSessionId), validateRequest);
		//	Trace.WriteLine("Commit Xfr");
		//	MVAEngineTest.Checkout(Convert.ToInt64(customerSession.CustomerSessionId));
		//	Trace.WriteLine("GetXfrTransaction");
		//	var MGMTtrx = MVAEngineTest.GetXfrTransaction(Convert.ToInt64(customerSession.CustomerSessionId), validateResponse.TransactionId);

		//	SendMoneySearchRequest req = new SendMoneySearchRequest()
		//	{
		//		ConfirmationNumber = MGMTtrx.ConfirmationNumber,
		//		SearchRequestType = MGI.Channel.Shared.Server.Data.SearchRequestType.Modify,
		//	};

		//	SendMoneySearchResponse response = MVAEngineTest.SendMoneySearch(Convert.ToInt64(customerSession.CustomerSessionId), req);
		//	Assert.AreNotEqual(response.TransactionStatus, "Modify");
		//	ReasonRequest reasonRequest = new ReasonRequest()
		//									  {
		//										  TransactionType = "REFUND"
		//									  };
		//	List<MoneyTransferReason> refundReason = MVAEngineTest.GetRefundReasons(Convert.ToInt64(customerSession.CustomerSessionId), reasonRequest);
		//	SendMoneySearchRequest searchRequest = new SendMoneySearchRequest()
		//	{
		//		ConfirmationNumber = MGMTtrx.ConfirmationNumber,
		//		SearchRequestType = MGI.Channel.Shared.Server.Data.SearchRequestType.Refund,
		//	};

		//	SendMoneySearchResponse refundSearchResponse = MVAEngineTest.SendMoneySearch(Convert.ToInt64(customerSession.CustomerSessionId), searchRequest);

		//	RefundSendMoneyRequest refundRequest = new RefundSendMoneyRequest()
		//											   {
		//												   ConfirmationNumber = MGMTtrx.ConfirmationNumber,
		//												   RefundStatus = refundSearchResponse.RefundStatus,
		//												   TransactionId = Convert.ToString(MGMTtrx.TransactionID)

		//											   };

		//	long refundStatus = MVAEngineTest.StageRefundSendMoney(Convert.ToInt64(customerSession.CustomerSessionId), refundRequest);

		//	var receipt = MVAEngineTest.Checkout(Convert.ToInt64(customerSession.CustomerSessionId));

		//}


		#endregion

		#endregion

		#region Private Methods

		private List<dmsMasterData> GetXfrCountries()
		{
			CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
			// MVAEngineTest.AddReceiver(Convert.ToInt64(customerSession.CustomerSessionId), receiverdto);
			return MVAEngineTest.GetXfrCountries(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId));
		}

		private List<dmsMasterData> GetXfrGetXfrStates(string statecode)
		{
			return MVAEngineTest.GetXfrStates(statecode, channelPartnerName);
		}

		private List<dmsMasterData> GetXfrCities(string statecode)
		{
			return MVAEngineTest.GetXfrCities(statecode, channelPartnerName);
		}

		private void CompareReceiver(Receiver testReceiverDto, Receiver InsertedReceiverDto)
		{

			Assert.IsTrue(testReceiverDto.FirstName == InsertedReceiverDto.FirstName
				&& testReceiverDto.LastName == InsertedReceiverDto.LastName
				&& testReceiverDto.SecondLastName == InsertedReceiverDto.SecondLastName
				&& testReceiverDto.PickupCountry == InsertedReceiverDto.PickupCountry
				&& testReceiverDto.PickupState_Province == InsertedReceiverDto.PickupState_Province
				&& testReceiverDto.Address == InsertedReceiverDto.Address
				&& testReceiverDto.City == InsertedReceiverDto.City
				&& testReceiverDto.State_Province == InsertedReceiverDto.State_Province
				&& testReceiverDto.ZipCode == InsertedReceiverDto.ZipCode
				&& testReceiverDto.PhoneNumber == InsertedReceiverDto.PhoneNumber
				&& testReceiverDto.Status == InsertedReceiverDto.Status
				&& testReceiverDto.State_Province == InsertedReceiverDto.State_Province);

		}

		private static bool IsDomesticTransfer(string countryCode)
		{
			return countryCode.ToLower() == "us" || countryCode.ToLower() == "usa" || countryCode.ToLower() == "united states";
		}
		#endregion


	}
}
