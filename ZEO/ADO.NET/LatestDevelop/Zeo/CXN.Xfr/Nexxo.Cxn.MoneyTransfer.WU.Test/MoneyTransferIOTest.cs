using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Testing.NUnit;
//using MGI.Cxn.MoneyTransfer.WU.Contract;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using NHibernate;
using NHibernate.Context;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.WU.Common.Data;


namespace MGI.Cxn.MoneyTransfer.WU.Test
{
	public class MoneyTransferIOTest : AbstractTransactionalSpringContextTests
	{
		//public IWUMoneyTransferIO CXNMoneyTransferIOSetup { get; set; }
        public IMoneyTransfer CXNMoneyTransferProcessor { get; set; }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.MoneyTransfer.WU.Test/MGI.Cxn.MoneyTransfer.WU.Test/CXNTestSpring.xml" }; }
		}


		//[Test]
		//public void SendMoneyValidation()
		//{
		//	SendMoneyValidateRequest Request = new SendMoneyValidateRequest();
		//	MGI.Cxn.WU.Common.Data.Sender sender = new MGI.Cxn.WU.Common.Data.Sender();
		//	sender.FirstName = "Reddy";
		//	sender.LastName = "ma";
		//	sender.AddressAddrLine1 = "100 Summit Ave";
		//	sender.FirstName = "Reddy";
		//	sender.LastName = "ma";
		//	sender.AddressAddrLine1 = "100 Summit Ave";
		//	sender.AddressCity = "MONTVALE";
		//	sender.AddressState = "NJ";
		//	sender.AddressPostalCode = "07645";
		//	sender.CountryCode = "US";
		//	sender.CurrencyCode = "USD";
		//	sender.NameType = WUEnums.name_type.D;
		//	sender.ContactPhone = "2018202100";
		//	sender.PreferredCustomerAccountNumber = "112233445";
		//	sender.PreferredCustomerLevelCode = "DA5";
		//	sender.Email = "surendra.p@opus.com";
		//	sender.ContactPhone = "2018202100";

		//	Promotions promo = new Promotions();
		//	promo.coupons_promotions = "A027";
		//	promo.sender_promo_code = "A027";


		//	MGI.Cxn.WU.Common.Data.Receiver reciever = new MGI.Cxn.WU.Common.Data.Receiver();
		//	reciever.FirstName = "surendra";
		//	reciever.LastName = "srinu";
		//	reciever.NameType = WUEnums.name_type.D;
		//	Address add = new Address() { Addr_line1 = "100 Summit Ave", City = "MONTVALE", State = "NJ", Postal_code = "07645" };
		//	reciever.Address = add;

		//	CountryCurrencyInfo recording = new CountryCurrencyInfo() { country_code = "US", currency_code = "USD" };
		//	CountryCurrencyInfo destination = new CountryCurrencyInfo() { country_code = "IND", currency_code = "INR" };
		//	CountryCurrencyInfo orginating = new CountryCurrencyInfo() { country_code = "US", currency_code = "USD" };
		//	MGI.Cxn.WU.Common.Data.PaymentDetails paydts = new MGI.Cxn.WU.Common.Data.PaymentDetails()
		//	{
		//		destination_country_currency = destination,
		//		originating_country_currency = orginating,
		//		recording_country_currency = recording,
		//		transaction_type = WUEnums.Transaction_type.WMN,
		//		Payment_type = WUEnums.Payment_type.Cash,
		//		expectedPayoutLoc_City = "new york",
		//		expectedPayoutLoc_StateCode = "ny"
		//	};
		//	Financials financials = new Financials() { originators_principal_amount = 24600, originators_principal_amountSpecified = true };
		//	DeliveryServices ds = new DeliveryServices()
		//	{
		//		Code = "000"
		//		//Identification_question = new Identification_question() 
		//		//{ Question = "WHAT IS PROJECT NAME", Answer = "H2H" }
		//	};


		//	CountryCurrencyInfo recording1 = new CountryCurrencyInfo() { country_code = "US", currency_code = "USD" };
		//	CountryCurrencyInfo destination1 = new CountryCurrencyInfo() { country_code = "IND", currency_code = "INR" };
		//	CountryCurrencyInfo orginating1 = new CountryCurrencyInfo() { country_code = "US", currency_code = "USD" };
		//	MGI.Cxn.WU.Common.Data.PaymentDetails paydts1 = new MGI.Cxn.WU.Common.Data.PaymentDetails()
		//	{
		//		destination_country_currency = destination1,
		//		originating_country_currency = orginating1,
		//		recording_country_currency = recording1,
		//		transaction_type = WUEnums.Transaction_type.WMN,
		//		Payment_type = WUEnums.Payment_type.Cash,
		//		expectedPayoutLoc_City = "new york",
		//		expectedPayoutLoc_StateCode = "ny"
		//	};
		//	Financials financials1 = new Financials() { originators_principal_amount = 24600, originators_principal_amountSpecified = true };
		//	DeliveryServices ds1 = new DeliveryServices() { Code = "000", Identification_question = new Identification_question() { Question = "WHAT IS PROJECT NAME", Answer = "H2H" } };

		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add("ChannelPartnerId", 33);
		//	Request.sender = sender;
		//	Request.reciever = reciever;
		//	Request.paymentDetails = paydts;
		//	Request.financials = financials1;
		//	Request.deliveryservices = ds1;
		//	Request.sender = sender;
		//	Request.reciever = reciever;
		//	Request.paymentDetails = paydts1;
		//	Request.financials = financials;
		//	Request.deliveryservices = ds;

		//	SendMoneyValidateResponse response = new SendMoneyValidateResponse();
		//	//response = CXNMoneyTransferIOSetup.SendMoneyValidate(Request, context);
		//}


		//[Test]
		//public void SendMoneyStoreCommit()
		//{
		//	long transactionId = 1000000187;
		//	MoneyTransfer.Data.Sender sender = new MoneyTransfer.Data.Sender() { AddressAddrLine1 = "111 ANZA BLVD", AddressCity = "BURLINGAME", AddressPostalCode = "94010", AddressState = "CA", ContactPhone = "6505803551", FirstName = "SEND", LastName = "MONEY" };
		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add("ChannelPartnerId", 33);


		//	CXNMoneyTransferProcessor.Commit(transactionId, context);
	

		//	SendMoneyStoreRequest request = new SendMoneyStoreRequest();

		//	request.sender = new Cxn.WU.Common.Data.Sender();
		//	request.sender.AddressAddrLine1 = "100 Summit Avee";
		//	//request.sender.AddressAddrLine2 = "Bangalore";
		//	request.sender.AddressCity = "MONTVALE";
		//	request.sender.AddressPostalCode = "07645";
		//	request.sender.AddressState = "NJ";
		//	request.sender.ContactPhone = "9878767654";
		//	request.sender.CountryCode = "US";
		//	//request.sender.CountryName = "India";
		//	request.sender.CurrencyCode = "USD";
		//	request.sender.Email = "surendra.p@opus.com";
		//	request.sender.FirstName = "ReddyRaj";
		//	request.sender.LastName = "ma";
		//	//request.sender.MiddleName = "Kumar";
		//	request.sender.MobilePhone = "9878767654";
		//	request.sender.NameType = WUEnums.name_type.D;
		//	//request.sender.PreferredCustomerAccountNumber = "3423141234213";
		//	//request.sender.PreferredCustomerLevelCode = "3421421";
		//	//request.sender.SmsNotificationFlag = "true";
		//	request.sender.ContactPhone = "2018202100";
		//	request.sender.AddressPostalCode = "07645";



		//	request.reciever = new Cxn.WU.Common.Data.Receiver();
		//	request.reciever.Address = new Address() { Addr_line1 = "100 Summit Ave", City = "MONTVALE", State = "NJ" };
		//	request.reciever.City = "Bangalore";
		//	request.reciever.FirstName = "surendra";
		//	//request.reciever.Gender = "Male";
		//	request.reciever.LastName = "srinu";
		//	request.reciever.MiddleName = "Kumar";
		//	request.reciever.NameType = WUEnums.name_type.D;


		//	request.paymentDetails = new Cxn.WU.Common.Data.PaymentDetails()
		//	{
		//		destination_country_currency = new CountryCurrencyInfo()
		//		 {
		//			 country_code = "US",
		//			 currency_code = "USD"
		//		 },
		//		originating_country_currency = new CountryCurrencyInfo()
		//		{
		//			country_code = "US",
		//			currency_code = "USD"
		//		},
		//		transaction_type = WUEnums.Transaction_type.WMN,
		//		Payment_type = WUEnums.Payment_type.Cash,
		//		//Exchange_Rate = 1,
		//		duplicate_detection_flag = "D",
		//		//expectedPayoutLoc_City = "new york",
		//		expectedPayoutLoc_StateCode = "ny"
		//	};


		//	request.Financials = new Financials()
		//	{
		//		originators_principal_amount = 3244,
		//		gross_total_amount = 3900,
		//		charges = 656,

		//	};
		//	request.deliveryservices = new DeliveryServices()
		//	{
		//		//Code = "000"
		//	};

		//	request.Mtcn = "3970289750";
		//	request.NewMtcn = "1329883970289750";
		//	//            context.Add("ChannelPartnerId", 33);
		//	//SendMoneyStoreResponse response = CXNMoneyTransferIOSetup.SendMoneyStore(request, context);

		//}

		//[Test]
		//public void ModifySendMoneySearch()
		//{
		//	MGI.Cxn.MoneyTransfer.WU.Data.ModifySendMoneySearchRequest Request = new MGI.Cxn.MoneyTransfer.WU.Data.ModifySendMoneySearchRequest();
		//	MGI.Cxn.WU.Common.Data.PaymentTransaction paymentTransaction = new MGI.Cxn.WU.Common.Data.PaymentTransaction();
		//	paymentTransaction.mtcn = "4630332980";//"2470332540";//"1770444080";
		//	Request.paymentTransaction = paymentTransaction; 

		//	MGI.Cxn.MoneyTransfer.WU.Data.ModifySendMoneySearchResponse response = new MGI.Cxn.MoneyTransfer.WU.Data.ModifySendMoneySearchResponse();
			
		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add("ChannelPartnerId", 33);

		//	//response = CXNMoneyTransferIOSetup.ModifySendMoneySearch(Request,context);
		//	//Assert.IsTrue(response.fusion_status == "W/C"); 

		//}
		//[Test]
		//public void ModifySendMoney()
		//{

		//	MGI.Cxn.MoneyTransfer.WU.Data.ModifySendMoneySearchRequest Request = new MGI.Cxn.MoneyTransfer.WU.Data.ModifySendMoneySearchRequest();
		//	MGI.Cxn.WU.Common.Data.Sender sender = new MGI.Cxn.WU.Common.Data.Sender();
		//	sender.FirstName = "ASHOK";
		//	sender.LastName = "KUMAR";
		//	sender.NameType = WUEnums.name_type.D;
		//	sender.AddressCity = "SAN BRUNO"; 
		//	sender.AddressState  = "CA";
		//	sender.AddressPostalCode = "94066";
		//	sender.AddressAddrLine1  = "111 ANZA BLVD";	
		//	sender.AddressStreet = "111 ANZA BLVD";
		//	sender.ContactPhone = "9878767654" ;
		//	//Request.sender = sender;


		//	MGI.Cxn.WU.Common.Data.Receiver receiver = new MGI.Cxn.WU.Common.Data.Receiver();
		//	receiver.FirstName = "BILL";
		//	receiver.LastName = "GATES";
		//	receiver.NameType = WUEnums.name_type.D;
		//	receiver.Address = new Address { State = "GA" };
		////	Request.receiver = receiver; 	

		//	CountryCurrencyInfo recording = new CountryCurrencyInfo() { country_code = "US", currency_code = "USD" };
		//	CountryCurrencyInfo destination = new CountryCurrencyInfo() { country_code = "US", currency_code = "USD" };
		//	CountryCurrencyInfo orginating = new CountryCurrencyInfo() { country_code = "US", currency_code = "USD" };

		//	MGI.Cxn.WU.Common.Data.PaymentDetails paydts = new MGI.Cxn.WU.Common.Data.PaymentDetails()
		//	{
		//		destination_country_currency = destination,
		//		originating_country_currency = orginating,				
		//		transaction_type = WUEnums.Transaction_type.WMN,
		//		Payment_type = WUEnums.Payment_type.Cash,				
		//		expectedPayoutLoc_StateCode = "AL",
		//		//Originating_city = "COLUMBUSGA3",
		//		Originating_state = "GA",
		//		Exchange_Rate = 1.0000000

		//	};
		//	Request.paymentDetails = paydts; 

		//	Financials financials = new Financials()
		//	{
		//		originators_principal_amount = 8200, 
		//		originators_principal_amountSpecified = true ,
		//	  destination_principal_amount = 8200, 
		//		destination_principal_amountSpecified = true,
		//	  gross_total_amount = 9400,
		//		charges = 1200,
		//		Total_discounted_charges = 1200,
		//		Total_undiscounted_charges = 1200                  
		//	};
	
		//	Request.financials = financials; 
		//	DeliveryServices ds = new DeliveryServices()
		//	{
		//		Identification_question = new Identification_question()
		//		{ Question = "WHAT IS PROJECT NAME", Answer = "H2H" }
		//	};
		//	Request.deliveryServices = ds;
		//	Request.mtcn = "4630332980";//"2470332540";//"4560245118";
		//	Request.new_mtcn = "1405584630332980";//"1405282470332540"; //"1404384560245118";
		//	Request.money_transfer_key = "4186535553";//"1108331785";

		//	ModifySendMoneyResponse response = new  ModifySendMoneyResponse();
		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add("ChannelPartnerId", 33);

		//	//response = CXNMoneyTransferIOSetup.ModifySendMoney(Request, context); 

		//}

		//[Test]
		//public void Modify_SendMoneyTest()
		//{
		//	MGI.Cxn.MoneyTransfer.Data.ModifySendMoneySearchRequest searchRequest = new MGI.Cxn.MoneyTransfer.Data.ModifySendMoneySearchRequest();
		//	MGI.Cxn.MoneyTransfer.Data.PaymentTransaction paymentTransaction = new MGI.Cxn.MoneyTransfer.Data.PaymentTransaction();
		//	paymentTransaction.mtcn = "4490587373";//"0890575879";//"4490587373";

		//	searchRequest.paymentTransaction = paymentTransaction;

		//	if (searchRequest.paymentTransaction != null)
		//	{
		//		searchRequest.paymentTransaction.mtcn = paymentTransaction.mtcn;
		//	}

		//	MGI.Cxn.MoneyTransfer.Data.PaymentDetails paydts = new MGI.Cxn.MoneyTransfer.Data.PaymentDetails();
		//	searchRequest.paymentTransaction.paymentDetails.TestQuestion = "TEST MODIFY";
		//	searchRequest.paymentTransaction.paymentDetails.TestAnswer = "TEST ANSWER";
		//	searchRequest.paymentTransaction.paymentDetails = paydts;

		//	MGI.Cxn.MoneyTransfer.Data.Receiver receiver = new MGI.Cxn.MoneyTransfer.Data.Receiver();
		//	receiver.FirstName = "ASHOK";
		//	receiver.LastName = "KUMAR";
		//	searchRequest.paymentTransaction.receiver = receiver;

		//	long transactionId = 1000002279;//1000002273;//1000001907;//1000002266;
            
		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add("ChannelPartnerId", 33);
		//	context.Add("TimeZone", "Central Mountain Time");
		//	long tranId = CXNMoneyTransferProcessor.CommitSendMoneyModify(transactionId, context);
		//   //Assert.IsNotEmpty(tranId);
		//	SetComplete();
		//	Assert.IsTrue(tranId > 0);
		//}
	}
}