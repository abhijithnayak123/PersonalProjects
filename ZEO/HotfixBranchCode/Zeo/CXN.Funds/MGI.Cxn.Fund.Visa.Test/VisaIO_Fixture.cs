using MGI.Common.Util;
using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.Visa.Data;
using MGI.Cxn.Fund.Visa.Impl;
using NUnit.Framework;
using Spring.Testing.NUnit;
using System;
using System.Collections.Generic;

namespace MGI.Cxn.Fund.Visa.Test
{
	[TestFixture]
	public class VisaIO_Fixture : AbstractTransactionalSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.Fund.Visa.Test/MGI.Cxn.Fund.Visa.Test/MGI.Cxn.Fund.Visa.Test.Spring.xml" }; }
		}

		public IO VisaIO { get; set; }
		MGIContext mgiContext = new MGIContext();

		#region Unit test cases
		[Test]
		public void Can_Diagnostics()
		{
			Credential credential = BuildCredential(33);
			VisaIO.Diagnostics(credential);
		}

		[Test]
		public void Can_GetCardInfoByProxyId()
		{
			List<string> proxys = new List<string>(){
				"0000000000083633930",
				"0000000000083308890"	,
				"0000000000083308872"	,
				"0000000000083308970"	,
				"0000000000083308934"	,
				"0000000000083308916"	,
				"0000000000083308872"
			};
			string proxyId = string.Empty;
			foreach (var proxy in proxys)
			{
				proxyId = proxy;
			Credential credential = BuildCredential(33);
			Data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);

			Assert.That(cardInfo.AliasId, Is.AtLeast(1));
			Assert.That(cardInfo.AliasId, Is.AtLeast(20754703));
			}
		}

		[Test]
		public void Can_GetPsedoDDAFromAliasId()
		{
			Credential credential = BuildCredential(33);
			string proxyId = "79621773";
			Data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);
			string pseudoDDA = VisaIO.GetPsedoDDAFromAliasId(cardInfo.AliasId, credential);

			Assert.That(pseudoDDA, Is.Not.Null);
			Assert.That(pseudoDDA, Is.EqualTo("39900000000023860"));
		}

		[Test]
		public void Can_GetBalance()
		{
			Credential credential = BuildCredential(33);
			string proxyId = "0000000000083308890";
			Data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);

			long amountToLoad = 30;
			LoadResponse loadResponse = VisaIO.Load(cardInfo.AliasId, amountToLoad, credential);

			Fund.Visa.Data.CardBalance cardBalance = VisaIO.GetBalance(cardInfo.AliasId, credential);

			Assert.That(cardBalance, Is.Not.Null);
			Assert.That(cardBalance.Balance, Is.GreaterThan(0));
		}

		[Test]
		[ExpectedException(ExpectedMessage = "Limit violation detected")]
		public void Can_not_LoadToCard_When_Limit_Exceeds()
		{
			string proxyId = "79621773";
			Credential credential = BuildCredential(33);
			Data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);
			long amountToLoad = 5;

			LoadResponse loadResponse = VisaIO.Load(cardInfo.AliasId, amountToLoad, credential);

			Assert.That(loadResponse, Is.Not.Null);
			Assert.That(loadResponse.TransactionKey, Is.EqualTo(0.0));
		}

		[Test]
		public void Can_WithdrawToCard()
		{
			string proxyId = "79621773";
			Credential credential = BuildCredential(33);
			Data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);
			long amountToWithdraw = 30;
			LoadResponse loadResponse = VisaIO.Load(cardInfo.AliasId, amountToWithdraw, credential);
			VisaIO.Withdraw(cardInfo.AliasId, amountToWithdraw, credential);

			Assert.That(true, Is.True);
		}

		[Test]
		public void Can_GetTransactionHistory()
		{
			Credential credential = BuildCredential(33);

			TransactionHistoryRequest transactionHistoryRequest = new TransactionHistoryRequest
			{
				AliasId = 20753003,
				DateRange = 90,
				TransactionStatus = TransactionStatus.Posted
			};

			List<TransactionHistory> transactionHistoryList = VisaIO.GetTransactionHistory(transactionHistoryRequest, credential);

			Assert.That(transactionHistoryList, Is.Not.Empty);
			Assert.That(transactionHistoryList.Count, Is.AtLeast(1));
		}

		[Test]
		public void Can_CloseAccount()
		{

			string proxyId = "79620078";
			Credential credential = BuildCredential(33);
			Data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);

			Assert.That(cardInfo.Status, Is.Not.EqualTo("9"));

			long aliasId = 20754305;

			bool couldClose = VisaIO.CloseAccount(aliasId, credential);

			Assert.That(couldClose, Is.True);

			cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);
			Assert.That(cardInfo.Status, Is.EqualTo("9"));

		}

		[Test]
		public void Can_UpdateCardStatus()
		{
			Credential credential = BuildCredential(33);

			long aliasId = 20753003;
			string cardStatus = "5";

			bool isStatusChanged = VisaIO.UpdateCardStatus(aliasId, cardStatus, credential);

			Assert.That(isStatusChanged, Is.True);

			string proxyId = "79613979";

			Data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);

			Assert.That(cardInfo.Status, Is.EqualTo("5"));

		}

		[Test]
		public void Can_ReplaceCard()
		{
			Credential credential = BuildCredential(33);
			string proxyId = "79613979";
			long aliasId = 20753003;

			Data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);

			int expirationYear = cardInfo.CardIssueDate.AddYears(4).Year;
			int expirationMonth = cardInfo.CardIssueDate.Month;

			if (cardInfo.CardIssueDate.Day > 15)
			{
				expirationMonth += 1;
			}

			var cardMaintenanceInfo = new CardMaintenanceInfo()
			{
				CardClass = 7,
				CardStatus = cardInfo.Status,
				ShippingType = "2", // Standard
				ShippingFee = 0.00,
				ExpiryMonth = expirationMonth,
				ExpiryYear = expirationYear
			};


			bool isStatusChanged = VisaIO.ReplaceCard(aliasId, cardMaintenanceInfo, credential);

			Assert.That(isStatusChanged, Is.True);

			cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential);

			Assert.That(cardInfo.Status, Is.EqualTo("5"));

		}

		[Test]
		public void can_TrimAddress()
		{
			string address = "ADDRESS ADDRESS ADRESS ADDRESS ADDRESS ADRESS";
			int size = 30;

			string massagedAddress = NexxoUtil.MassagingValue(address, size);

			Assert.That(massagedAddress.Length, Is.EqualTo(size));
		}

		[Test]
		public void cannot_TrimAddress()
		{
			string address = "ADDRESS ADDRESS ADRESS";
			int size = 30;

			string massagedAddress = NexxoUtil.MassagingValue(address, size);

			Assert.That(massagedAddress.Length, Is.LessThan(size));
		}

		[Test]
		public void can_retain_Address()
		{
			string address = "ADDRESS ADDRESS ADRESS ADDRESS";
			int size = 30;

			string massagedAddress = NexxoUtil.MassagingValue(address, size);

			Assert.That(massagedAddress.Length, Is.EqualTo(size));
		}

		[Test]
		public void Do_Check_Email()
		{
			string email = "test@mgi.com";

			Assert.That(VisaIO.GetTrimmedEmail(email), Is.EqualTo(email));
		}

		[Test]
		public void Do_Validate_Email()
		{
			string email = "testmailsynovustcfvisadps@MoneyGramInternationlIC.com";

			Assert.That(VisaIO.GetTrimmedEmail(email), Is.Empty);
		}

		[Test]
		public void Can_TrimCity()
		{
			string city = "Square Street Junction";
			int size = 19;
			string maggasedCity = NexxoUtil.MassagingValue(city, size);

			Assert.That(maggasedCity.Length, Is.EqualTo(size));
		}

		[Test]
		public void Cannot_TrimCity()
		{
			string city = "Square Street";
			int size = 19;
			string maggasedCity = NexxoUtil.MassagingValue(city, size);

			Assert.That(maggasedCity.Length, Is.LessThan(size));
		}

		[Test]
		public void Can_GetCardInfoByCardNumber()
		{
			string cardNumber = "4756757000182512";
			Credential credential = BuildCredential(33);
			Data.CardInfo cardInfo = VisaIO.GetCardInfoByCardNumber(cardNumber, credential);

			Assert.That(cardInfo.AliasId, Is.AtLeast(1));
			Assert.That(cardInfo.SSN, Is.Not.Empty);
			//Assert.That(cardInfo.AliasId, Is.AtLeast(20754703));
		}

		[Test]
		public void Can_GetCardHolderInfo()
		{
			long aliasId = 90000001966853;
			Credential credential = BuildCredential(33);
			Data.CardInfo cardInfo = VisaIO.GetCardHolderInfo(aliasId, credential);

			Assert.That(cardInfo.AliasId, Is.AtLeast(1));
		}


		#endregion

		#region Private Methods

		private Credential BuildCredential(long ChannelPartnerId)
		{
			Credential credential = new Credential();

			if (ChannelPartnerId == 33)
			{
				credential = new Credential()
				{
					ServiceUrl = "https://proxy.ic.local/visa/websrv_prepaid/v15_10/prepaidservices",
					CertificateName = "Synovus DPS Prepaid Web Service (CTE WSI)",
					UserName = "prc96702.webserv",
					Password = "Synovus1",
					ClientNodeId = 368769,
					CardProgramNodeId = 368770,
					SubClientNodeId = -1,
					StockId = "967CS001",
					ChannelPartnerId = 33,
				};
			}
			else if (ChannelPartnerId == 34)
			{
				credential = new Credential()
				{
					ServiceUrl = "https://proxy.ic.local/visa/websrv_prepaid/v15_10/prepaidservices",
					CertificateName = "TCF Nexxo Web Services (CTE WSI)",
					UserName = "prc1279.webserv",
					Password = "pKzWRV24r4",
					ClientNodeId = 12081,
					CardProgramNodeId = 250012,
					SubClientNodeId = -1,
					StockId = "127CS201",
					ChannelPartnerId = 34,
				};
			}
			return credential;
		}

		#endregion

	}
}
