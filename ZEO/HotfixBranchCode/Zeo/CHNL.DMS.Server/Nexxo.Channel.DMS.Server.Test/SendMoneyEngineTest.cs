using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;

using NUnit.Framework;

using Spring.Context;
using Spring.Context.Support;

using System.ServiceModel;
using MGI.Channel.Shared.Server.Data;
using Spring.Testing.NUnit;


namespace MGI.Channel.DMS.Server.Test
{
	[TestFixture]
	public class SendMoneyEngineTest : AbstractTransactionalSpringContextTests
	{

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }

		}

		IDesktopService DeskTopTest { get; set; }
        public MGIContext mgiContext { get; set; }
		private static string DESKTOP_ENGINE = "DesktopEngine";

		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);
		}

		[Test]
		public void GetReceiversTest()
		{
			//  long customerSessionId = 1000000923;
			long customerSessionId = 1000002065;
			IList<Receiver> list = DeskTopTest.GetReceivers(customerSessionId, "Shukla", new MGIContext());

			Assert.AreEqual(list.Count(), 6, list[0].City);
		}

		//[Test]
		//public void GetFrequentReceiversTest()
		//{

		//    string AlloyID = "1000000002966974";
		//    List<Receiver> list = new List<Receiver>();
		//    list = DeskTopTest.GetFrequentReceivers(AlloyID);

		//    if (list.Count == 4)
		//    {
		//        string str = string.Format("{0}-{1}, {2}-{3}, {4}-{5}, {6}-{7}",
		//            list[0].FirstName, list[0].LastName,
		//            list[1].FirstName, list[1].LastName,
		//            list[2].FirstName, list[2].LastName,
		//            list[3].FirstName, list[3].LastName);

		//        Assert.AreEqual(4, list.Count, str);
		//    }
		//    else
		//        Assert.AreEqual(100, list.Count, "err");
		//}

		[Test]
		public void GetReceiverDetailsTest()
		{
			long customerSessionId = 1000002065;

			Receiver rec = new Receiver();
            mgiContext = new MGIContext();
			rec = DeskTopTest.GetReceiver(customerSessionId, "Monu Shuklaa", mgiContext);

			string str = string.Format("name: {0} {1} country:{2} state:{3}, city:{4}",
				rec.FirstName, rec.LastName, rec.PickupCountry, rec.State_Province, rec.City);

			Assert.AreEqual("Monu", rec.FirstName, str);
		}

		[Test]
		public void SaveReceiverTest()
		{

            mgiContext = new MGIContext();
			Receiver receiver = new Receiver()
			{
				Address = "Bangalore",
				City = "Bangalore",
				DeliveryMethod = "Money",
				DeliveryOption = "SpeedMoney",
				FirstName = "Ashok",
				LastName = "LastName",
				PhoneNumber = "9878767654",
				PickupCity = "Hyderabad",
				PickupCountry = "India",
				PickupState_Province = "Andhra Pradesh",
				SecondLastName = "Gandamaneni",
				State_Province = "Andhra Pradesh",
				Status = "Active",
				ZipCode = "5600028"
			};

			long customerSessionId = 1000000923;
            long receiverId = DeskTopTest.AddReceiver(customerSessionId, receiver, mgiContext);

			Assert.Greater(receiverId, 0, "The Receiver not added.");

		}

		//        /// <summary>
		//        /// 
		//        /// </summary>
		//        [Test]
		//        public void GetSelectedBeneficiaryLimitsAndFeesTest()
		//        {

		//            AgentSession agentSession = DeskTopTest.Authenticate("Anil", "Anil@123", "centris", "Anil");

		//            CustomerAuthentication customer = new CustomerAuthentication();

		//            customer.AlloyID = "1000000000858223";
		//            string benId = "10258";
		//            CustomerSession customerSession = DeskTopTest.InitiateCustomerSession(agentSession.SessionId, customer);

		//            BeneficiaryTransactionInfo info = DeskTopTest.GetBeneficiaryTransactionInfo(agentSession.SessionId,customerSession.CustomerSessionId, customer.AlloyID, string.Empty , benId);

		//            Assert.Greater(info.MaxTransactionAmount, 0);
		//            Assert.Greater(info.FeeTiers.Count, 0);
		//        }

		//        [Test]
		//        public void BeginTransactionTest()
		//        {
		//            AgentSession agentSession = DeskTopTest.Authenticate("Anil", "Anil@123", "centris", "Anil");

		//            CustomerAuthentication customer = new CustomerAuthentication();

		//            customer.AlloyID = "1000000000858223";
		//            string benId = "10258";
		//            CustomerSession customerSession = DeskTopTest.InitiateCustomerSession(agentSession.SessionId, customer);

		//            SessionContext sessionContext = new SessionContext
		//            {
		//                AgentId = agentSession.Agent.Id,
		//                AgentName = agentSession.Agent.Name,
		//                CustomerSessionId = customerSession.CustomerSessionId,
		//                DTKiosk = DateTime.Now,
		//            };

		//            string transactionId = "";
		//            TransactionDetails transactionDetails = new TransactionDetails();

		//            transactionDetails = DeskTopTest.BeginTransaction(agentSession.SessionId,customerSession.CustomerSessionId, customer.AlloyID, benId, 100);

		//            Assert.AreEqual(string.IsNullOrWhiteSpace(transactionId), false);
		//            Assert.AreNotEqual(transactionDetails, null);
		//            Assert.AreNotEqual(transactionDetails.DoddFrankDisclosure, null);
		//            Assert.AreNotEqual(transactionDetails.DoddFrankDisclosure.Count, 0);
		//        }

		//        [Test]
		//        public void ConfirmTransactionDetailsTest()
		//        {
		//            AgentSession agentSession = DeskTopTest.Authenticate("Anil", "Anil@123", "centris", "Anil");

		//            CustomerAuthentication customer = new CustomerAuthentication();

		//            customer.AlloyID = "1000000000858223";
		//            //string benId = "10258"; 
		//            string trnId = "122222";
		//            CustomerSession customerSession = DeskTopTest.InitiateCustomerSession(agentSession.SessionId, customer);

		//            SessionContext sessionContext = new SessionContext
		//            {
		//                AgentId = agentSession.Agent.Id,
		//                AgentName = agentSession.Agent.Name,
		//                CustomerSessionId = customerSession.CustomerSessionId,
		//                DTKiosk = DateTime.Now,
		//            };

		//            string res = DeskTopTest.ConfirmTransactionDetails(agentSession.SessionId, customerSession.CustomerSessionId , trnId);
		//        }

		//        [Test]
		//        public void ReceiveTermsAndConditionsTest()
		//        {
		//            AgentSession agentSession = DeskTopTest.Authenticate("Anil", "Anil@123", "centris", "Anil");

		//            CustomerAuthentication customer = new CustomerAuthentication();

		//            customer.AlloyID = "1000000000858223";
		//            //string benId = "10258"; 
		//            string trnId = "122222";
		//            CustomerSession customerSession = DeskTopTest.InitiateCustomerSession(agentSession.SessionId, customer);

		//            SessionContext sessionContext = new SessionContext
		//            {
		//                AgentId = agentSession.Agent.Id,
		//                AgentName = agentSession.Agent.Name,
		//                CustomerSessionId = customerSession.CustomerSessionId,
		//                DTKiosk = DateTime.Now,
		//            };

		//            //DeskTopTest.ReceiveTermsAndConditions(sessionContext, trnId,true );
		//        }


		//        [Test]
		//        public void CommitTransactionTest()
		//        {
		//            AgentSession agentSession = DeskTopTest.Authenticate("Anil", "Anil@123", "centris", "Anil");

		//            CustomerAuthentication customer = new CustomerAuthentication();

		//            customer.AlloyID = "1000000000858223";
		//            //string benId = "10258"; 
		//            string trnId = "122222";
		//            CustomerSession customerSession = DeskTopTest.InitiateCustomerSession(agentSession.SessionId, customer);

		//            SessionContext sessionContext = new SessionContext
		//            {
		//                AgentId = agentSession.Agent.Id,
		//                AgentName = agentSession.Agent.Name,
		//                CustomerSessionId = customerSession.CustomerSessionId,
		//                DTKiosk = DateTime.Now,
		//            };

		//            Receipts receipts = new Receipts();

		//    //DeskTopTest.CommitTransaction(sessionContext, string.Empty, trnId, 10, out receipts);
		//}
	}
}
