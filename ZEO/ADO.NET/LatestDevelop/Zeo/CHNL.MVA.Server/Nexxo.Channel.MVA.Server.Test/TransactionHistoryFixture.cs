using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using Spring.Context;

namespace MGI.Channel.MVA.Server.Test
{
    [TestFixture]
    class TransactionHistoryFixture
    {
        IMVAService MVAEngineTest { get; set; }
        private static string channelPartnerName = string.Empty;
        private static string cardNumber = "4756750000227411";
        private static string MVA_ENGINE = "MVAEngine";

        [SetUp]
        public void Setup()
        {
            channelPartnerName = System.Configuration.ConfigurationManager.AppSettings["ChannelPartnerName"].ToString();
            IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            MVAEngineTest = (IMVAService)ctx.GetObject(MVA_ENGINE);
            MVAEngineTest.SetSelf(MVAEngineTest);
        }

        [Test]
        public void GetBillPayTransaction()
        {

            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
            Trace.WriteLine("GetBillers By Name");
            var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);
            BillFee billfee = MVAEngineTest.GetBillPaymentFee(customerSessionId, billers[0].BillerCode, "1234567890", 109);
            Assert.IsNotNull(billfee);
            var providerAttributes = MVAEngineTest.GetBillPaymentProviderAttributes(customerSessionId, billers[0].BillerCode, billfee.TransactionId);
            Assert.IsNotNull(providerAttributes);
            var billPayment = new BillPayment()
            {
                PaymentAmount = 109,
                Fee = billfee.DeliveryMethods[0].FeeAmount,
                BillerName = billers[0].ProductName,
                AccountNumber = "1234567890",
                BillerId = Convert.ToInt64(billers[0].Id),
            };
            Dictionary<string, object> MetaData = new Dictionary<string, object>();
            billPayment.MetaData = MetaData;
            if (providerAttributes.Count != 0)
            {
                for (int i = 0; i < providerAttributes.Count; i++)
                {
                    if (providerAttributes[i].DataType == "TextBox")
                        billPayment.MetaData.Add(providerAttributes[i].TagName, "abc" + i);
                    if (providerAttributes[i].DataType == "Dropdown")
                        billPayment.MetaData.Add(providerAttributes[i].TagName, providerAttributes[i].Values.Keys.First());
                }
            }
            long transactionId = MVAEngineTest.ValidateBillPayment(customerSessionId, billPayment, billfee.TransactionId);
            Assert.IsNotNull(transactionId);
            var receipt = MVAEngineTest.Checkout(customerSessionId);
            var billPayTransaction = MVAEngineTest.GetBillPayTransaction(customerSessionId, transactionId);

            Assert.IsNotNull(billPayTransaction);

        }
    }
}
