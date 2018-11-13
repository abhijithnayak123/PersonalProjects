using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Contract;
using Spring.Context;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;
using Spring.Testing.NUnit;

namespace MGI.Channel.DMS.Server.Test
{
	[TestFixture]
	public class CustomerEngineTest : AbstractTransactionalSpringContextTests
	{


		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }

		}

		private static string DESKTOP_ENGINE = "DesktopEngine";
		public MGIContext mgiContext { get; set; }
		IDesktopService DeskTopTest { get; set; }

		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);

            mgiContext.ChannelPartnerId = 34;
            //mgiContext.Language = "EN";
		}

		[Test]
		public void GetTransactionHistoryTest()
		{		
			long customerSessionId = 1000000000000240;
			string transactionType = "SendMoney";
			string location = "Robbinsdale";
			DateTime dateRange = System.DateTime.Now.AddMonths(-4);

            List<TransactionHistory> transactions = DeskTopTest.GetTransactionHistory(0, Convert.ToInt64(customerSessionId), transactionType, location, dateRange, mgiContext);
			Assert.AreEqual(transactions.Count, 2, "PASS");
		}

		[Test]
		public void GetReceiptDataTest()
		{			
			long customerSessionId = 1000001100;
			string transactionType = "SendMoney";
			string location = "Robbinsdale";
			DateTime dateRange = System.DateTime.Now.AddMonths(-4);

            List<TransactionHistory> transactions = DeskTopTest.GetTransactionHistory(0, Convert.ToInt32(customerSessionId), transactionType, location, dateRange, mgiContext);

			//string FundPaymentId = transactions[0].TransactionId;
			//List<string> receipt = DeskTopTest.GetReceiptData( customerSession.CustomerSessionId, FundPaymentId, _context );

			//Assert.IsNotEmpty( receipt, "Failed to load receipt data" );

		}

		[Test]
		public void RecordIdentificationConfirmation()
		{
			AgentSession agentSession = new AgentSession();  // DeskTopTest.Authenticate("Anil", "Anil@123", "centris", "Anil");
			//agentSession.SessionId = "1000001489";
			//CustomerAuthentication customer = new CustomerAuthentication();
			//customer.AlloyID = 1000000000002540;
			////	customer.PAN = "1000000008564989";

			CustomerSession customerSession = new CustomerSession();//DeskTopTest.InitiateCustomerSession(agentSession.SessionId, customer, _context);
			//string customerSessionId = "1234";
			//string agentSessionId = "13532";
			string msg = "Failed";
			try
			{
                msg = DeskTopTest.RecordIdentificationConfirmation(long.Parse(customerSession.CustomerSessionId), agentSession.SessionId, true, mgiContext);
			}
			catch 
			{
				Assert.AreEqual("Failed", msg,"Recording identification confirmation failed");
			}
		}

		//[Test]
		//public void RegisterCard()
		//{
			//AgentSession agentSession = null; // DeskTopTest.Authenticate("Anil", "Anil@123", "centris", "Anil");

			//CustomerAuthentication customer = new CustomerAuthentication();

			//		customer.PAN = "1000000008564989";

			//CustomerSession customerSession = DeskTopTest.InitiateCustomerSession(agentSession.SessionId, customer, _context);
			//GprCard gpr = new GprCard
			//{
			//    AccountNumber = customer.PAN,
			//    ActivatedBy = agentSession.Agent.Id.ToString(),
			//    Active = true,
			//    CardNumber = "434343434",
			//    ChannelPartnerId = "",
			//    DateActivated = DateTime.Today,
			//    ExpiryDate = DateTime.Now.AddDays( 90 ),
			//    PAN = customer.PAN,
			//    TempCard = false,
			//    TempCardNumber = ""

			//};
			//string msg = DeskTopTest.RegisterCard( agentSession.SessionId, gpr );

			//Assert.AreEqual( "Success", msg, "Recording identification confirmation failed" );

		//}


	}
}
