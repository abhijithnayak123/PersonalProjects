using System.Diagnostics;
using NUnit.Framework;
using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;

using Spring.Context;
using MGI.Channel.Consumer.Server.Contract;
using MGI.Channel.MVA.Server.Impl;

namespace MGI.Channel.MVA.Server.Test
{
    [TestFixture]
    class CustomerEngineFixture 
    {
        IMVAService MVAEngineTest { get; set; }
        private static string MVA_ENGINE = "MVAEngine";
        private static string channelPartnerName ="Synovus";
		private static string cardNumber = "4855078900005392";

		[SetUp]
		public void Setup()
		{
			channelPartnerName = System.Configuration.ConfigurationManager.AppSettings["ChannelPartnerName"].ToString();
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			MVAEngineTest = (IMVAService)ctx.GetObject(MVA_ENGINE);
			MVAEngineTest.SetSelf(MVAEngineTest);
		}

        [Test]
        public void InitiateCustomerSessionTest()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            Trace.WriteLine("Customer session is initiated with the Id : " + customerSession.CustomerSessionId + "..!");
            Assert.IsNotNullOrEmpty(customerSession.CustomerSessionId);
        }

        [Test]
        public void IsValidSSNTest()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            Trace.WriteLine("Customer session is initiated with the Id : " + customerSession.CustomerSessionId + "..!");
            Assert.IsNotNullOrEmpty(customerSession.CustomerSessionId);
            
            //getting actual SSN of customer
            string SSN = customerSession.Customer.SSN;

            if(string.IsNullOrEmpty(SSN) || SSN.Length < 4)
            {          
                Assert.IsTrue(true);
            }
            else
            {
                string last4DigitsOfSSN = SSN.Substring(SSN.Length - 4);
                Assert.IsTrue(MVAEngineTest.IsValidSSN(customerSession.CustomerSessionId, last4DigitsOfSSN));
                Assert.IsFalse(MVAEngineTest.IsValidSSN(customerSession.CustomerSessionId, "xxxx"));
            }
        }
    }
}
