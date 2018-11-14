using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;
using Spring.Context;
using MGI.Channel.Shared.Server.Data;
using Spring.Testing.NUnit;

namespace MGI.Channel.DMS.Server.Test
{
	[TestFixture]
	public class ProspectTests : AbstractTransactionalSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }

		}

		IDesktopService DeskTopTest { get; set; }
		private static string DESKTOP_ENGINE = "DesktopEngine";
		private Dictionary<string, string> _context = new Dictionary<string, string>();

		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);
			_context.Add("ChannelPartnerId", "34");
			_context.Add("Language", "EN");
		}

		[Test]
		public void TestToString()
		{
			Guid msgUUID = new Guid("00000000-0000-0000-0000-000000000000");
			List<string> Groups = new List<string>();
			string expectedOut = BuildProspectString("", "", "", "", "", "", DateTime.MinValue, "", "", "", "", "", "", "",
				"", "", "", "", "", false, "", "", "", "", "", "", null, false, "", false, "", "", false, false, "", "", "", "", "", msgUUID, "", false, Groups, false, "", "", "", "", "", "", "", "", "", "");

			Console.WriteLine("expected");
			Console.WriteLine(expectedOut);

			Prospect p = new Prospect()
			{
				FName = "",
				SecondaryCountryCitizenShip = "",
				Groups = Groups

			};

			//p.Groups.Add(Groups)
			//p.Groups.Insert(0, "Groups1");


			string x = "";
			Assert.DoesNotThrow(() => x = p.ToString());
			Console.WriteLine("actual");
			Console.WriteLine(x);

			Assert.IsTrue(x == expectedOut);

			Identification id = new Identification { Country = "UNITED STATES", ExpirationDate = new DateTime(2020, 1, 1), GovernmentId = "A1234567", IDType = "DRIVER'S LICENSE", State = "CALIFORNIA" };

			//Guid p = new Guid("00000000-0000-0000-0000-000000000000");

			expectedOut = BuildProspectString("", "William", "Robert", "Thornton", "", "", DateTime.MinValue, "", "", "", "", "", "", "",
				"", "", "", "", "", false, "", "", "", "", "", "", id, false, "", false, "", "", false, false, "", "", "", "", "", msgUUID, "",
				false, Groups, false, "", "", "", "", "", "", "", "", "", "");
			Console.WriteLine("expected");
			Console.WriteLine(expectedOut);

			Prospect p1 = new Prospect()
			{
				FName = "William",
				MName = "Robert",
				LName = "Thornton",
				ID = id,
				Groups = Groups

			};
			//p.FName = "William";
			//p.MName = "Robert";
			//p.LName = "Thornton";
			//p.ID = id;

			Assert.DoesNotThrow(() => x = p1.ToString());
			Console.WriteLine("actual");
			Console.WriteLine(x);

			Assert.IsFalse(x == expectedOut);
		}

		private string BuildProspectString(string Gender, string FName, string MName, string LName, string LName2, string MoMaName,
			DateTime DOB, string Address1, string Address2, string City, string State, string PostalCode, string Phone1, string Phone1Type,
			string Phone1Provider, string Phone2, string Phone2Type, string Phone2Provider, string PIN, bool TextMsgOptIn,
			string Email, string Occupation, string OccupationDescription, string Employer, string EmployerPhone, string SSN, Identification ID,
			bool IsAccountHolder, string ReferralCode, bool DoNotCall, string CardNumber, string WUGoldCardNumber, bool WUSMSNotification,
			bool MailingAddressDifferent, string MailingAddress1, string MailingAddress2, string MailingCity, string MailingState,
			string MailingZipCode,
			 Guid ChannelPartnerId, string ReceiptLanguage, bool ProfileStatus, List<string> Groups, bool ClientProfileStatus,
			string PartnerAccountNumber,
			string RelationshipAccountNumber, string BankId, string BranchId, string ProgramId, string Notes, string ClientID,
			string LegalCode, string PrimaryCountryCitizenShip,
			string SecondaryCountryCitizenShip)
		{
			Groups.Insert(0, "Group1");
			Groups.Insert(1, "Group2");
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Prospect:");
			sb.AppendLine(string.Format("	Gender: {0}", Gender));
			sb.AppendLine(string.Format("	FName: {0}", FName));
			sb.AppendLine(string.Format("	MName: {0}", MName));
			sb.AppendLine(string.Format("	Gender: {0}", Gender));
			sb.AppendLine(string.Format("	LName: {0}", LName));
			sb.AppendLine(string.Format("	LName2: {0}", LName2));
			sb.AppendLine(string.Format("	MoMaName: {0}", MoMaName));
			sb.AppendLine(string.Format("	Date of Birth: {0}", DOB.ToShortDateString()));
			sb.AppendLine(string.Format("	Address1: {0}", Address1));
			sb.AppendLine(string.Format("	Address2: {0}", Address2));
			sb.AppendLine(string.Format("	City: {0}", City));
			sb.AppendLine(string.Format("	State: {0}", State));
			sb.AppendLine(string.Format("	PostalCode: {0}", PostalCode));
			sb.AppendLine(string.Format("	Phone1: {0}", Phone1));
			sb.AppendLine(string.Format("	Phone1Type: {0}", Phone1Type));
			sb.AppendLine(string.Format("	Phone1Provider: {0}", Phone1Provider));
			sb.AppendLine(string.Format("	Phone2: {0}", Phone2));
			sb.AppendLine(string.Format("	Phone2Type: {0}", Phone2Type));
			sb.AppendLine(string.Format("	Phone2Provider: {0}", Phone2Provider));
			sb.AppendLine(string.Format("	PIN Length: {0}", string.IsNullOrEmpty(PIN) ? 0 : PIN.Length));
			sb.AppendLine(string.Format("	TextMsgOptIn: {0}", TextMsgOptIn));
			sb.AppendLine(string.Format("	Email: {0}", Email));
			sb.AppendLine(string.Format("	Occupation: {0}", Occupation));
			sb.AppendLine(string.Format("	Description: {0}", OccupationDescription));
			sb.AppendLine(string.Format("	Employer: {0}", Employer));
			sb.AppendLine(string.Format("	EmployerPhone: {0}", EmployerPhone));
			sb.AppendLine(string.Format("	SSN Length: {0}", string.IsNullOrEmpty(SSN) ? 0 : SSN.Length));
			sb.AppendLine(string.Format("	{0}", ID == null ? "" : BuildIDString(ID.Country, ID.IDType, ID.State, ID.GovernmentId, ID.ExpirationDate)));
			sb.AppendLine(string.Format("	IsAccountHolder: {0}", IsAccountHolder));
			sb.AppendLine(string.Format("	ReferralCode: {0}", ReferralCode));
			sb.AppendLine(string.Format("	DoNotCall: {0}", DoNotCall));
			sb.AppendLine(string.Format("	MailingAddressDifferent: {0}", MailingAddressDifferent));
			sb.AppendLine(string.Format("	MailingAddress1: {0}", MailingAddress1));
			sb.AppendLine(string.Format("	MailingAddress2: {0}", MailingAddress2));
			sb.AppendLine(string.Format("	MailingCity: {0}", MailingCity));
			sb.AppendLine(string.Format("	MailingState: {0}", MailingState));
			sb.AppendLine(string.Format("	MailingZipCode: {0}", MailingZipCode));
			sb.AppendLine(string.Format("   ChannelPartnerId : {0}", ChannelPartnerId));
			sb.AppendLine(string.Format("   ReceiptLanguage : {0}", ReceiptLanguage));
			sb.AppendLine(string.Format("   ProfileStatus : {0}", ProfileStatus));
			if (!string.IsNullOrEmpty(CardNumber))
				sb.AppendLine(string.Format("   CardNumber: {0}", string.Format("xx{0}", CardNumber.Substring(CardNumber.Length - 4))));
			else
				sb.AppendLine("   CardNumber is empty");
			if (!string.IsNullOrEmpty(WUGoldCardNumber))
				sb.AppendLine(string.Format("	WUGoldCardNumber: {0}", WUGoldCardNumber));
			else
				sb.AppendLine("   GoldCardNumber is empty");
			sb.AppendLine(string.Format("	WUSMSNotification: {0}", WUSMSNotification));
			sb.AppendLine(string.Format("	WUSMSNotification: {0}", WUSMSNotification));
			sb.AppendLine(string.Format("   Groups: {0}", string.Join(",", Groups.ToArray())));

			sb.AppendLine(string.Format("   CustomerProfileStatus : {0}", ClientProfileStatus));
			if (!string.IsNullOrEmpty(PartnerAccountNumber))
				sb.AppendLine(string.Format("   PartnerAccountNumber : {0}", PartnerAccountNumber.Substring(0, 6) + "XXXXXX" + PartnerAccountNumber.Substring(PartnerAccountNumber.Length - 4, 4)));

			else
				sb.AppendLine("   PartnerAccountNumber is empty");
			if (!string.IsNullOrEmpty(RelationshipAccountNumber))
				sb.AppendLine(string.Format("   RelationshipAccountNumber : {0}", RelationshipAccountNumber));
			else
				sb.AppendLine("   RelationshipAccountNumber is empty");
			if (!string.IsNullOrEmpty(BankId))
				sb.AppendLine(string.Format("   BankId : {0}", BankId));
			else
				sb.AppendLine("   BankId is empty");
			if (!string.IsNullOrEmpty(BranchId))
				sb.AppendLine(string.Format("   BranchId : {0}", BranchId));
			else
				sb.AppendLine("   BranchId is empty");
			sb.AppendLine(string.Format("   ProgramId : {0}", ProgramId));
			sb.AppendLine(string.Format("	ClientID: {0}", ClientID));
			sb.AppendLine(string.Format("	LegalCode: {0}", LegalCode));
			sb.AppendLine(string.Format("	PrimaryCountryCitizenShip: {0}", PrimaryCountryCitizenShip));
			sb.AppendLine(string.Format("	SecondaryCountryCitizenShip: {0}", SecondaryCountryCitizenShip));
			//	sb.AppendLine(string.Format("   Groups: {0}", Groups));
			return sb.ToString();
		}

		private string BuildIDString(string Country, string IDType, string State, string ID, DateTime? ExpirationDate)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("ID:");
			sb.AppendLine(string.Format("	Country: {0}", Country));
			// sb.AppendLine(string.Format("	IDType: {0}", IDType));
			sb.AppendLine(string.Format("	State: {0}", State));
			sb.AppendLine(string.Format("	ID: {0}", ID));
			sb.AppendLine(string.Format("	ExpirationDate: {0}", ExpirationDate));
			return sb.ToString();
		}

		//[Test]
		//public void GetProspectTest()
		//{
		//	string agentSessionId = "1000001228";
		//	//customer.PAN = "1000000008564989";
		//	long AlloyID = 1000000000002320;
		//	//AgentSession Session = null; // DeskTopTest.Authenticate("anil", "Anil@123", "centris", "Anil");
		//	Prospect prospect = DeskTopTest.GetProspect(agentSessionId, AlloyID, _context);

		//	Assert.AreEqual("RAJ", prospect.FName, "Retrieved wrong data");
		//}
	}
}
