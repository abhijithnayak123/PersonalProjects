using MGI.Cxn.Check.Contract;
using MGI.Cxn.Check.Data;
using NUnit.Framework;
using Spring.Data.Generic;
using Spring.Testing.NUnit;
using System;
using System.Collections.Generic;
using MGI.Cxn.Check.Certegy.Data;
using MGI.Cxn.Check.Certegy.Impl;
using MGI.Cxn.Check.Certegy.Certegy;
using System.Globalization;
using MGI.Cxn.Check.Certegy.Contract;
using MGI.Common.Util;

namespace MGI.Cxn.Check.Certegy.Test
{
	[TestFixture]
	class Certegy_IO_Fixture : AbstractTransactionalSpringContextTests
	{
		public MGIContext MgiContext { get; set; }
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.Check.Certegy.Test/MGI.Cxn.Check.Certegy.Test/MGI.Cxn.Check.Certegy.Test.Spring.xml" }; }
		}


		public IIO CertegyIO { get; set; }
		
		[Test]
		public void Can_Diagnostics()
		{
			Credential credential = GetCredential();
			MgiContext = GetMGIContext();

			CertegyIO.Diagnostics(credential, MgiContext);
		}

		[Test]
		public void Can_Authorize_Check()
		{
			decimal checkAmount = 28.70M;
			Account certegyAccount = GetAccount();
			CheckInfo checkInformation = GetCheckInformation(checkAmount);
			Credential credential = GetCredential();
			MgiContext = GetMGIContext();
			Transaction transaction = SetUpTransactionData(1000000008, certegyAccount, checkInformation, credential, MgiContext);
			authorizeResponse certegyResponse = null;

			certegyResponse = CertegyIO.AuthorizeCheck(transaction, credential, MgiContext);			

			Assert.That(certegyResponse, Is.Not.Null);			
		}

		[Test]
		public void Can_Reverse_Check()
		{
			decimal checkAmount = 100M;
			Account certegyAccount = GetAccount();
			CheckInfo checkInformation = GetCheckInformation(checkAmount);
			Credential credential = GetCredential();
			MgiContext = GetMGIContext();
			Transaction transaction = SetUpTransactionData(1000000008, certegyAccount, checkInformation, credential, MgiContext);
			reverseResponse certegyResponse = null;

			certegyResponse = CertegyIO.ReverseCheck(transaction, credential, MgiContext);

			Assert.That(certegyResponse, Is.Not.Null);			
		}

		#region Private Methods

		private Credential GetCredential()
		{
			var credential = new Credential()
			{
				CertificateName = "Todd Bowersox",
				ChannelPartnerId = 1,				
				DeviceIP = "12.227.206.70",
				DeviceType = "P",
				//ServiceUrl = "https://transtest2.certegy.com/mepca2/PCAService",
				ServiceUrl = "https://pca-transtest2.FNIS.com/mepca2/PCAService",
				//ServiceUrl = "https://pca-transtest2.FNIS.com/mepca/PCAService",
				//ServiceUrl = "https://pca-prod.FNIS.com/mepca/PCAService",
				//ServiceUrl = "https://pca-dr.FNIS.com/mepca/PCAService",				
				Version = "1.2"
			};

			return credential;
		}

		private static Account GetAccount()
		{
			var certegyAccount = new Account()
			{
				FirstName = "John",
				LastName = "Smith",
				SecondLastName = null,
				Address1 = "Apple St.",
				City = "AnyTown",
				State = "FL",
				Zip = "33716",
				DateOfBirth = DateTime.ParseExact("05/02/1960", "MM/dd/yyyy", CultureInfo.InvariantCulture),
				Phone = "4151234567",
				IdState = "VA",
				IDType = "DRIVER'S LICENSE",				
				Idcardnumber = "782436485",
				Ssn = "226589994",				
			};
			return certegyAccount;
		}		

		private CheckInfo GetCheckInformation(decimal amount)
		{
			return new CheckInfo
			{				
				Amount = amount,
				Type = CheckType.GovtUSOther,
				MicrEntryType = (int)MGI.Common.Util.CheckEntryTypes.ScanWithImage,
				IssueDate = DateTime.Today,
				Micr = "9t061000010t123456711o2829",
				
			};
		}		

		private Transaction SetUpTransactionData(long transactionId, Account account, CheckInfo check, Credential credential, MGIContext mgiContext)
		{	
			string micrEntryType = check.MicrEntryType == 3 ? "M" : "S";

			return new Transaction
			{
				Id = transactionId,
				CheckAmount = check.Amount,
				Micr = check.Micr,
				CheckDate = check.IssueDate,
				SiteID = mgiContext.CertegySiteId,
				IdType = "VA",
				TranType = "C",
				FundsAvail = "I",
				ExpansionType = "TOAD",
				Version = credential.Version,				
				DeviceIP = credential.DeviceIP,
				DeviceId = mgiContext.TerminalName,
				DeviceType = credential.DeviceType,
				MicrEntryType = micrEntryType,
				AlloySubmitType = (int)check.Type,
				AlloyReturnType = (int)check.Type,
				CertegySubmitType = "G",
				CertegyReturnType = "Y",
				CertegyAccount = account,
				CheckStatus = CheckStatus.Approved,
				ChannelPartnerID = mgiContext.ChannelPartnerId,
				DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
				DTServerCreate = DateTime.Now
			};
		}

		private MGIContext GetMGIContext()
		{
			MGIContext mgiContext = new MGIContext() {
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				ChannelPartnerId = 1,
				LocationName = "test",
				CertegySiteId = "1109541077350101",
				TerminalName = "MGI - Terminal"
			};
			return mgiContext;
		}

		#endregion
	}
}
