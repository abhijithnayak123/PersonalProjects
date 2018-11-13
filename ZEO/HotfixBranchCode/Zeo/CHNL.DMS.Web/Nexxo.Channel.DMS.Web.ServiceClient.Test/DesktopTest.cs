using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.ServiceClient.Test
{
	[TestFixture]
	public class DeskTopTest
	{
		public Desktop DeskTop { get; set; }   
		int cardPresentedType = 0;
		MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
		[SetUp]
		public void Setup()
		{
			DeskTop = new Desktop();
		}

		long agentSessionId = 1000002070;
		[Test]
		public void GetCustomerBillFee()
		{
			decimal retrievedFee = 0;
			int productId = 125;

			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "33", "OPT-LAP-0128");
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "27", "CCCCCCBLDFHF");

			CustomerSession customerSession
					= DeskTop.InitiateCustomerSession(agentSessionId.ToString(), 1000000000003880, cardPresentedType, mgiContext);

			retrievedFee = DeskTop.GetCustomerBillFee(productId);
			Assert.Greater(retrievedFee, 0);
		}

		[Test]
		public void GetBillersTest()
		{
			//AgentSession Session = DeskTop.AuthenticateAgent("anil", "Anil@123", "centris", "Anil");
			//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "27", "CCCCCCBLDFHF");

			CustomerSession customerSession
					= DeskTop.InitiateCustomerSession(agentSessionId.ToString(), 1000000000003880, cardPresentedType, mgiContext);
			Dictionary<string, object> context = new Dictionary<string, object>();
			//Should be there in agent session object
			int channelPartnerId = 34;
			//This as well should be part of agent session object
			Guid locationRegionId = Guid.NewGuid();
            List<string> billers = DeskTop.GetBillers(long.Parse(customerSession.CustomerSessionId), channelPartnerId, "sac", mgiContext);
			Assert.Greater(billers.Count, 0);
		}

		[Test]
		public void GetTransactionHistroryTest()
		{
			string transactionType = "Check Processing";
			DateTime dateRange = DateTime.Now.AddDays(1);
			string location = "TCF Service Desk";
			bool showAll = true;
			long transactionId = 1000000166;
			int duration = 0;
			long agentId = 500001;
			CustomerSession customerSession
					= DeskTop.InitiateCustomerSession(agentSessionId.ToString(), 1000000000003880, cardPresentedType, mgiContext);
			List<TransactionHistory> trans = DeskTop.GetTransactionHistory(0,  agentId, transactionType, location, showAll, transactionId, duration, mgiContext);
			Assert.True(trans.Count > 0);
		}

		//[Test]
		//public void RegisterCardTest()
		//{
		//	CustomerSession customerSession
		//			= DeskTop.InitiateCustomerSession(agentSessionId.ToString(), 1000000000003880, cardPresentedType);

		//	FundsProcessorAccount cardDetails = new FundsProcessorAccount()
		//	{
		//		AccountNumber = "1111111",
		//		//Address1 = "67/68, Sudev Complex",
		//		//Address2 = "",
		//		CardNumber = "5397380000440772",
		//		//City = "Bangalore",
		//		//CountryCode = "IN",
		//		//DateOfBirth = DateTime.Parse("07/08/1977")
		//	};

		//	long cxeId = DeskTop.RegisterCard(cardDetails, customerSession.CustomerSessionId);
		//	Assert.That(cxeId >= 0, Is.True);
		//}

		//[Test]
		//public void LoadTest()
		//{
		//	//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "27", "CCCCCCBLDFHF");
		//	CustomerSession customerSession
		//			= DeskTop.InitiateCustomerSession("1000001522", 1000000000000240, cardPresentedType);

		//	Funds funds = new Funds()
		//	{
		//		Amount = 100,
		//		Fee = 2
		//	};

		//	decimal previousBalance = DeskTop.GetCardBalance(customerSession.CustomerSessionId);

		//	long cxeTrxId = DeskTop.Load(customerSession.CustomerSessionId, funds);

		//	ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);

		//	//DeskTop.Checkout("1000000000003880", cart, long.Parse(customerSession.CustomerSessionId), 27,customerSession.Customer.Profile.CardBalance);

		//	decimal currentBalance = DeskTop.GetCardBalance(customerSession.CustomerSessionId);

		//	Assert.That(currentBalance == previousBalance + funds.Amount, Is.True);
		//	Assert.That(cxeTrxId > 0, Is.True);
		//}

		//[Test]
		//public void WithdrawTest()
		//{
		//	//AgentSession Session = DeskTop.AuthenticateAgent("SysAdmin", "Sysadmin@123", "27", "Nexxo");
		//	CustomerSession customerSession
		//			= DeskTop.InitiateCustomerSession(agentSessionId.ToString(), 1000000000003880, cardPresentedType);
		//	Funds funds = new Funds()
		//	{
		//		Amount = 100,
		//		Fee = 2
		//	};

		//	decimal previousBalance = DeskTop.GetCardBalance(customerSession.CustomerSessionId);

		//	long cxeTrxId = DeskTop.Withdraw(customerSession.CustomerSessionId, funds);

		//	ShoppingCart cart = DeskTop.ShoppingCart(customerSession.CustomerSessionId);

		//	//DeskTop.Checkout("1000000000003880", cart, long.Parse(customerSession.CustomerSessionId), 27,customerSession.Customer.Profile.CardBalance);

		//	decimal currentBalance = DeskTop.GetCardBalance(customerSession.CustomerSessionId);

		//	Assert.That(currentBalance == previousBalance - funds.Amount, Is.True);
		//	Assert.That(cxeTrxId > 0, Is.True);
		//}
	}
}