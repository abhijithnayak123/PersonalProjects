using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.Visa.Data;
using NUnit.Framework;
using Spring.Testing.NUnit;
using System;
using System.Collections.Generic;

namespace MGI.Cxn.Fund.Visa.Test
{
	[TestFixture]
	public class Gateway_Fixture : AbstractTransactionalSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.Fund.Visa.Test/MGI.Cxn.Fund.Visa.Test/MGI.Cxn.Fund.Visa.Test.Spring.xml" }; }
		}
		public IFundProcessor CxnFundsVisaProcessor { get; set; }

		public MGI.Common.Util.MGIContext mgiContext = new MGI.Common.Util.MGIContext();

		#region Positive Register test case

		[Test]
		public void Should_Register_Visa_Account()
		{
			CardAccount cardAccount = new CardAccount()
			{
				FirstName = "JOHN",
				LastName = "PARKER",
				AccountNumber = "1000127551",
				MailingAddress1 = "TEST",
				MailingAddress2 = "",
				MailingCity = "CA",
				MailingState = "CA",
				MailingZipCode = "95642",
				CountryCode = "USA",
				DateOfBirth = Convert.ToDateTime("10/10/1950"),
				ProxyId = "0000000000079614193",
				CardNumber = "4756755000017750",
				PseudoDDA = "39900000000096242",
				ExpirationDate = "11/2018"
			};
			mgiContext.ChannelPartnerId = 34;
			mgiContext.TimeZone = "Eastern Standard Time";
			ProcessorResult processorResult = new ProcessorResult();

			long accountId = CxnFundsVisaProcessor.Register(cardAccount, mgiContext, out processorResult);

			Assert.That(accountId, Is.AtLeast(100000000));
		}

		#endregion

		#region Negative Register test cases

		//[ExpectedException("NHibernate.Exceptions.GenericADOException")]
		//[Test]
		//public void Should_Not_Register_Visa_Account_When_FirstName_Not_Available()
		//{
		//	CardAccount cardAccount = new CardAccount()
		//	{
		//		LastName = "PARKER",
		//		AccountNumber = "1000127551",
		//		MailingAddress1 = "TEST",
		//		MailingAddress2 = "",
		//		MailingCity = "CA",
		//		MailingState = "CA",
		//		MailingZipCode = "95642",
		//		CountryCode = "USA",
		//		DateOfBirth = Convert.ToDateTime("10/10/1950"),
		//		ProxyId = "0000000000079613979",
		//		CardNumber = "4756755000044294",
		//		PseudoDDA = "39900000000096242",
		//		ExpirationDate = "11/2018"
		//	};
		//	Dictionary<string, object> context = new Dictionary<string, object>() 
		//	{
		//		{"TimeZone", "Eastern Standard Time"},
		//		 {"ChannelPartnerId", 34}
		//	};
		//	ProcessorResult processorResult = new ProcessorResult();

		//	long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

		//}

		[ExpectedException("System.ArgumentException")]
		[Test]
		public void Should_Not_Register_Visa_Account_When_CardNumber_Parameter_Null()
		{
			CardAccount cardAccount = null;
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			long accountId = CxnFundsVisaProcessor.Register(cardAccount, mgiContext, out processorResult);
		}

		[ExpectedException("System.NullReferenceException")]
		[Test]
		public void Should_Not_Register_Visa_Account_When_Context_Parameter_Null()
		{
			CardAccount cardAccount = GetCardAccount();
			ProcessorResult processorResult = new ProcessorResult();

			mgiContext = null;

			long accountId = CxnFundsVisaProcessor.Register(cardAccount, mgiContext, out processorResult);
		}

		[Test]
		public void Should_Not_Register_Visa_Account_When_TimeZone_Unavailable_In_Context_Parameter()
		{
			CardAccount cardAccount = new CardAccount()
			{
				FirstName = "JOHN",
				LastName = "PARKER",
				AccountNumber = "1000127551",
				MailingAddress1 = "TEST",
				MailingAddress2 = "",
				MailingCity = "CA",
				MailingState = "CA",
				MailingZipCode = "95642",
				CountryCode = "USA",
				DateOfBirth = Convert.ToDateTime("10/10/1950"),
				ProxyId = "0000000000079614194",
				CardNumber = "4756755000016670",
				PseudoDDA = "39900000000096242",
				ExpirationDate = "11/2018"
			};
			mgiContext = new MGI.Common.Util.MGIContext();
			ProcessorResult processorResult = new ProcessorResult();

			long accountId = CxnFundsVisaProcessor.Register(cardAccount, mgiContext, out processorResult);
		}

		#endregion

		#region Positive Card Activate test case

		[Test]
		public void Can_Activate_Card()
		{
			CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			long accountId = CxnFundsVisaProcessor.Register(cardAccount, mgiContext, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};

			long transactionId = CxnFundsVisaProcessor.Activate(accountId, fundRequest, mgiContext, out processorResult);

			Assert.That(transactionId, Is.AtLeast(1000000000));
		}

		[Test]
		public void Can_Activate_CompanianCard()
		{
			CardAccount cardAccount = GetCardAccount();

			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L, CardExpiryPeriod = 36 };
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.Context.Add("FundsAccount", cardAccount);
			ProcessorResult processorResult = new ProcessorResult();

			CxnFundsVisaProcessor.Commit(1000000002, mgiContext, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};


		}

		#endregion

		#region Negative Card Activate test case

		[Test]
		[ExpectedException("System.ArgumentException")]
		public void Should_Not_Activate_Card_When_AccountId_InValid()
		{
			//CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			long accountId = 0L;

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 50.00M
			};

			long transactionId = CxnFundsVisaProcessor.Activate(accountId, fundRequest, mgiContext, out processorResult);
		}

		[Test]
		[ExpectedException("System.ArgumentException")]
		public void Should_Not_Activate_Card_When_FundRequest_Null()
		{
			//CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = null;

			long transactionId = CxnFundsVisaProcessor.Activate(1000000012, fundRequest, mgiContext, out processorResult);
		}

		[Test]
		[ExpectedException("System.NullReferenceException")]
		public void Should_Not_Activate_Card_When_Context_Parameter_Null()
		{
			//CardAccount cardAccount = GetCardAccount();
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 50.00M
			};

			mgiContext = null;

			long transactionId = CxnFundsVisaProcessor.Activate(100000001, fundRequest, mgiContext, out processorResult);
		}

		[Test]
		public void Should_Not_Activate_Card_When_TimeZone_Unavailable_In_Context_Parameter()
		{
			//CardAccount cardAccount = GetCardAccount();

			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 50.00M
			};

			mgiContext = new MGI.Common.Util.MGIContext();

			long transactionId = CxnFundsVisaProcessor.Activate(100000001, fundRequest, mgiContext, out processorResult);
		}

		//[Test]
		//[ExpectedException("System.ArgumentException")]
		//public void Should_Not_Activate_Card_When_ChannelPartnerId_Unavailable_In_Context_Parameter()
		//{
		//    CardAccount cardAccount = GetCardAccount();
		//    Dictionary<string, object> context = new Dictionary<string, object>() 
		//    {
		//        {"TimeZone", "Eastern Standard Time"},   
		//                  {"ChannelPartnerId", 1}
		//    };
		//    ProcessorResult processorResult = new ProcessorResult();

		//    long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

		//    FundRequest fundRequest = new FundRequest()
		//    {
		//        Amount = 20.00M
		//    };

		//    long transactionId = CxnFundsVisaProcessor.Activate(accountId, fundRequest, context, out processorResult);
		//}

		#endregion

		#region Positive GetBalance test case

		[Test]
		public void Can_Get_Balance()
		{
			long accountId = 1000000000;
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 33 };
			ProcessorResult processorResult = new ProcessorResult();

			MGI.Cxn.Fund.Data.CardInfo balance = CxnFundsVisaProcessor.GetBalance(accountId, mgiContext, out processorResult);

			Assert.That(balance, Is.Not.Null);
			Assert.That(balance.Balance, Is.GreaterThan(0M));
		}

		[Test]
		public void Can_Get_Primary_Alias_ID()
		{
			long accountId = 100000001;
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 33 };
			ProcessorResult processorResult = new ProcessorResult();

			MGI.Cxn.Fund.Data.CardInfo balance = CxnFundsVisaProcessor.GetBalance(accountId, mgiContext, out processorResult);

			Assert.That(balance, Is.Not.Null);
			Assert.That(balance.Balance, Is.GreaterThan(0M));
		}

		#endregion

		#region Positive Load test case

		[Test]
		public void Can_Load_To_Card()
		{
			//CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};

			long transactionId = CxnFundsVisaProcessor.Load(100000001, fundRequest, mgiContext, out processorResult);

			Assert.That(transactionId, Is.AtLeast(1000000000));
		}

		#endregion

		#region Negative Load test case

		[Test]
		[ExpectedException("System.ArgumentException")]
		public void Should_Not_Load_To_Card_When_AccountId_InValid()
		{
			CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			long accountId = 0L;

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};

			long transactionId = CxnFundsVisaProcessor.Load(accountId, fundRequest, mgiContext, out processorResult);
		}

		[Test]
		[ExpectedException("System.ArgumentException")]
		public void Should_Not_Load_To_Card_When_FundRequest_Null()
		{
			//CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = null;

			long transactionId = CxnFundsVisaProcessor.Load(100000001, fundRequest, mgiContext, out processorResult);
		}

		[Test]
		[ExpectedException("System.NullReferenceException")]
		public void Should_Not_Load_To_Card_When_Context_Parameter_Null()
		{
			//CardAccount cardAccount = GetCardAccount();
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};

			mgiContext = null;

			long transactionId = CxnFundsVisaProcessor.Load(100000001, fundRequest, mgiContext, out processorResult);
		}

		[Test]
		[ExpectedException("System.NullReferenceException")]
		public void Should_Not_Load_To_Card_When_TimeZone_Unavailable_In_Context_Parameter()
		{
			//CardAccount cardAccount = GetCardAccount();
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};

			mgiContext = new MGI.Common.Util.MGIContext();

			long transactionId = CxnFundsVisaProcessor.Load(1000000012, fundRequest, mgiContext, out processorResult);
		}

		//[Test]
		//[ExpectedException("System.ArgumentException")]
		//public void Should_Not_Load_To_Card_When_ChannelPartnerId_Unavailable_In_Context_Parameter()
		//{
		//    CardAccount cardAccount = GetCardAccount();
		//    Dictionary<string, object> context = new Dictionary<string, object>() 
		//    {
		//        {"TimeZone", "Eastern Standard Time"}
		//          //{"ChannelPartnerId", 34}

		//    };
		//    ProcessorResult processorResult = new ProcessorResult();

		//    long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

		//    FundRequest fundRequest = new FundRequest()
		//    {
		//        Amount = 20.00M
		//    };

		//    long transactionId = CxnFundsVisaProcessor.Load(accountId, fundRequest, context, out processorResult);
		//}

		#endregion

		#region Positive Withdraw test case

		[Test]
		public void Can_Withdraw_From_Card()
		{
			//CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};

			long transactionId = CxnFundsVisaProcessor.Withdraw(100000001, fundRequest, mgiContext, out processorResult);

			Assert.That(transactionId, Is.AtLeast(1000000000));
		}

		#endregion

		#region Negative Withdraw test case

		[Test]
		[ExpectedException("System.ArgumentException")]
		public void Should_Not_Withdraw_From_Card_When_AccountId_InValid()
		{
			CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			long accountId = 0L;

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};

			long transactionId = CxnFundsVisaProcessor.Withdraw(accountId, fundRequest, mgiContext, out processorResult);
		}

		[Test]
		[ExpectedException("System.ArgumentException")]
		public void Should_Not_Withdraw_From_Card_When_FundRequest_Null()
		{
			//CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34L };
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = null;

			long transactionId = CxnFundsVisaProcessor.Withdraw(1000000012, fundRequest, mgiContext, out processorResult);
		}

		[Test]
		[ExpectedException("System.NullReferenceException")]
		public void Should_Not_Withdraw_From_Card_When_Context_Parameter_Null()
		{
			//CardAccount cardAccount = GetCardAccount();
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};

			mgiContext = null;

			long transactionId = CxnFundsVisaProcessor.Withdraw(1000000012, fundRequest, mgiContext, out processorResult);
		}

		[Test]
		[ExpectedException("System.NullReferenceException")]
		public void Should_Not_Withdraw_From_Card_When_TimeZone_Unavailable_In_Context_Parameter()
		{
			//CardAccount cardAccount = GetCardAccount();
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 20.00M
			};

			mgiContext = new MGI.Common.Util.MGIContext();

			long transactionId = CxnFundsVisaProcessor.Withdraw(1000000012, fundRequest, mgiContext, out processorResult);
		}

		//[Test]
		//[ExpectedException("System.ArgumentException")]
		//public void Should_Not_Withdraw_From_Card_When_ChannelPartnerId_Unavailable_In_Context_Parameter()
		//{
		//    CardAccount cardAccount = GetCardAccount();
		//    Dictionary<string, object> context = new Dictionary<string, object>() 
		//    {
		//        {"TimeZone", "Eastern Standard Time"},
		//        {"ChannelPartnerId", "1"}
		//    };
		//    ProcessorResult processorResult = new ProcessorResult();

		//    long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

		//    FundRequest fundRequest = new FundRequest()
		//    {
		//        Amount = 20.00M
		//    };

		//    long transactionId = CxnFundsVisaProcessor.Withdraw(accountId, fundRequest, context, out processorResult);
		//}

		#endregion

		#region Positive Commit test case

		[Test]
		public void Can_Commit_Load_To_Card()
		{
			CardAccount cardAccount = GetCardAccount();
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34 };
			ProcessorResult processorResult = new ProcessorResult();

			//long accountId = CxnFundsVisaProcessor.Register(cardAccount, context, out processorResult);

			Fund.Data.CardInfo balance = CxnFundsVisaProcessor.GetBalance(100000001, mgiContext, out processorResult);

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 50.00M
			};

			long transactionId = CxnFundsVisaProcessor.Load(100000001, fundRequest, mgiContext, out processorResult);

			CxnFundsVisaProcessor.Commit(transactionId, mgiContext, out processorResult);

			FundTrx transaction = CxnFundsVisaProcessor.Get(transactionId, mgiContext);

			//decimal newBalance = fundRequest.Amount + balance.Balance;

			Assert.That(transactionId, Is.AtLeast(1000000000));

		}
		[Test]
		public void Can_get_Shipping_Types_Card()
		{

			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34 };


			List<Cxn.Fund.Data.ShippingTypes> shippingTypes = CxnFundsVisaProcessor.GetShippingTypes(34);

			Assert.That(shippingTypes.Count, Is.GreaterThan(0));

		}
		#endregion

		#region Positive GetTransactionHistory test case

		[Test]
		public void Can_GetTransactionHistory()
		{
			long accountId = 100000001;
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34 };

			var transactionHistoryRequest = new TransactionHistoryRequest()
			{
				DateRange = 90,
				TransactionStatus = TransactionStatus.Posted
			};

			List<TransactionHistory> transactionHistoryList = CxnFundsVisaProcessor.GetTransactionHistory(accountId, transactionHistoryRequest, mgiContext);

			Assert.That(transactionHistoryList, Is.Not.Empty);
			Assert.That(transactionHistoryList.Count, Is.AtLeast(1));
		}

		[Test]
		public void Can_CloseAccount()
		{
			long accountId = 100000001;

			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34 };
			bool couldClose = CxnFundsVisaProcessor.CloseAccount(accountId, mgiContext);

			Assert.That(couldClose, Is.True);
		}

		[Test]
		public void Can_UpdateCardStatus()
		{
			long accountId = 100000001;
			//string cardStatus = "3"; //suspended
			string cardStatus = "5"; //lost
			//string cardStatus = "6"; //stolen
			CardMaintenanceInfo card = new CardMaintenanceInfo() { CardStatus = cardStatus, ShippingType = "" };
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34 };

			bool couldClose = CxnFundsVisaProcessor.UpdateCardStatus(accountId, card, mgiContext);

			Assert.That(couldClose, Is.True);
		}

		[Test]
		public void Can_ReplaceCard()
		{
			long accountId = 100000001;
			//string cardStatus = "3"; //suspended
			string cardStatus = "5"; //lost
			//string cardStatus = "6"; //stolen
			CardMaintenanceInfo card = new CardMaintenanceInfo()
			{
				CardStatus = cardStatus,
				ShippingType = "2" // standard
			};

			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 34, LocationStateCode = "AZ" };

			bool couldReplace = CxnFundsVisaProcessor.ReplaceCard(accountId, card, mgiContext);

			Assert.That(couldReplace, Is.True);
		}

		#endregion

		#region Private Methods

		private static CardAccount GetCardAccount()
		{
			var cardAccount = new CardAccount()
			{
				FirstName = "JOHN",
				LastName = "PARKER",
				AccountNumber = "1000127551",
				Address1 = "8910 Ridgeline Blvd",
				Address2 = null,
				State = "CA",
				MailingAddress1 = "TEST",
				MailingAddress2 = "",
				MailingCity = "CA",
				MailingState = "CA",
				MailingZipCode = "95642",
				CountryCode = "USA",
				DateOfBirth = Convert.ToDateTime("10/10/1950"),
				ProxyId = "0000000000079614192",
				CardNumber = "4756755000017750",
				PseudoDDA = "39900000000096242",
				ExpirationDate = "11/2018",
				Phone = "9135698975",
				GovernmentId = "727888811",
				GovtIDCountry = "USA",
				GovtIDType = "1",
				City = "Highlands Ranch",
				ZipCode = "95642",
				MothersMaidenName = "Smith"
			};
			return cardAccount;
		}

		#endregion

		[Test]
		public void GetShippingTypes()
		{
			List<ShippingTypes> shippingTypes = new List<ShippingTypes>();
			shippingTypes = CxnFundsVisaProcessor.GetShippingTypes(34);
			Assert.That(shippingTypes.Count, Is.GreaterThan(0));
		}

		[Test]
		public void GetVisaShippingFee()
		{
			int shippingFeeType = (int)ShippingFeeType.InstantIssueReplaceLostOrStolen;
			CardMaintenanceInfo cardMaintenanceInfo = new CardMaintenanceInfo()
			{
				ShippingType = Convert.ToString(shippingFeeType)
			};
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 33 };
			double shippingFee = CxnFundsVisaProcessor.GetShippingFee(cardMaintenanceInfo, mgiContext);
			Assert.That(shippingFee, Is.GreaterThanOrEqualTo(0M));
		}

		[Test]
		public void GetVisaFee()
		{
			int cardStatus = (int)Data.CardStatus.ReplaceCard;
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 33 };
			CardMaintenanceInfo cardMaintenanceInfo = new CardMaintenanceInfo();
			cardMaintenanceInfo.CardStatus = Convert.ToString(cardStatus);
			double fundFee = CxnFundsVisaProcessor.GetFundFee(cardMaintenanceInfo, mgiContext);
			Assert.That(fundFee, Is.GreaterThanOrEqualTo(0M));
		}

		[Test]
		public void GetVisaFeeCode()
		{
			int cardStatus = (int)Data.CardStatus.Lost;
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 33 };
			CardMaintenanceInfo cardMaintenanceInfo = new CardMaintenanceInfo();
			cardMaintenanceInfo.CardStatus = Convert.ToString(cardStatus);
			double fundFee = CxnFundsVisaProcessor.GetFundFee(cardMaintenanceInfo, mgiContext);
			Assert.That(fundFee, Is.GreaterThan(0M));
		}

		[Test]
		public void Can_AssociateCard()
		{
			string cardNumber = "4756756000186663";
			bool isNewCard = true;
			mgiContext = new MGI.Common.Util.MGIContext() { TimeZone = "Eastern Standard Time", ChannelPartnerId = 33 };
			CardAccount cardAccount = new CardAccount()
			{
				CardNumber = cardNumber,
				LastName = "SAKALA",
				SSN = "121541254"
			};
			long accountId = CxnFundsVisaProcessor.AssociateCard(cardAccount, mgiContext, isNewCard);
			Assert.That(accountId, Is.GreaterThan(0));
		}
	}
}
