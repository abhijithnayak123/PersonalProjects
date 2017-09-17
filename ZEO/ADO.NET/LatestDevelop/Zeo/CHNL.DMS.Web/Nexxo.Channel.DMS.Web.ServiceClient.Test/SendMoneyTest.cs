using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.ServiceClient.Test
{
    [TestFixture]
    public class SendMoneyTest
    {
        public Desktop DeskTop { get; set; }
        //private ShoppingCart cart = null;
		//int cardPresentedType = 0;
        [SetUp]
        public void Setup()
        {
            DeskTop = new Desktop();
        }

        [Test]
        public void GetReceiverTransactionInfoTest()
        {
			//AgentSession agent = DeskTop.AuthenticateAgent("anil", "Anil@123", "centris", "Anil");
			//long PAN = 1000000008565267;
			//string beneId = "6D5AF137-DD84-482E-81DC-6A6ECA41FEEA";
			//CustomerSession customerSession = DeskTop.InitiateCustomerSession(agent.SessionId, PAN, cardPresentedType);
           
            /*BeneficiaryTransactionInfo beneInfo = DeskTop.GetBeneficiaryTransactionInfo(agent.SessionId, customerSession.CustomerSessionId, PAN,"",beneId);
            Assert.True(beneInfo != null);
            Assert.True(beneInfo.FeeTiers.ElementAt(0).Fee.Amount >= 0);
            Assert.True(beneInfo.MinTransactionAmount > 0);
            Assert.True(beneInfo.MaxTransactionAmount > 0);*/
        }

        [Test]
        public void BeginTransactionTest()
        {
			//AgentSession agent = DeskTop.AuthenticateAgent("anil", "Anil@123", "centris", "Anil");
			//long PAN = 1000000008565267;
			//string beneId = "6D5AF137-DD84-482E-81DC-6A6ECA41FEEA";
			//CustomerSession customerSession = DeskTop.InitiateCustomerSession(agent.SessionId, PAN , cardPresentedType);
			//string transactionId = string.Empty;
			//GetReceiverTransactionInfoTest();
           // TransactionDetails transDetails = DeskTop.BeginTransaction(agent.SessionId, customerSession.CustomerSessionId, PAN, beneId, 100);
           // Assert.True(transDetails != null);
        }
    }
}