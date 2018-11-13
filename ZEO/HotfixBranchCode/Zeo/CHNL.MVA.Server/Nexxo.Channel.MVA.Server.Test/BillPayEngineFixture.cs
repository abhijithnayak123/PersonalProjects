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
    class BillPayEngineFixture
    {
        IMVAService MVAEngineTest { get; set; }
		private static string channelPartnerName = "";
		private static string cardNumber = "2935000098705584";//"4756750000227411";
        private static string MVA_ENGINE = "MVAEngine";
        [SetUp]
		public void Setup()
		{
			channelPartnerName = System.Configuration.ConfigurationManager.AppSettings["ChannelPartnerName"].ToString();
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			MVAEngineTest = (IMVAService)ctx.GetObject(MVA_ENGINE);
			MVAEngineTest.SetSelf(MVAEngineTest);
		}

        #region Billpay Biller Fixture

        [Test]
        public void GetBillers()
        {
            Trace.WriteLine("GetBillers By Name");
            var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);
        }

        [Test]
        public void GetFrequentBiller()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

            Trace.WriteLine("GetBillers By Name");
            var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);
            BillFee billfee = MVAEngineTest.GetBillPaymentFee(customerSessionId, billers[0].BillerCode, "1234567890", 110);
            Assert.IsNotNull(billfee);
            var providerAttributes = MVAEngineTest.GetBillPaymentProviderAttributes(customerSessionId, billers[0].BillerCode, billfee.TransactionId);
            Assert.IsNotNull(providerAttributes);
            var billPayment = new BillPayment()
            {
                PaymentAmount = 110,
                Fee = billfee.DeliveryMethods[0].FeeAmount,
                BillerName = billers[0].ProductName,
                AccountNumber = "1234567890",
                BillerId = Convert.ToInt64(billers[0].Id),
            };
            billPayment.MetaData = new Dictionary<string, object>();
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
            long validateBillPayment = MVAEngineTest.ValidateBillPayment(customerSessionId, billPayment, billfee.TransactionId);
            Assert.IsNotNull(validateBillPayment);
            var receipt = MVAEngineTest.Checkout(customerSessionId);

            Trace.WriteLine("GetBillers By Name");
            var billers1 = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);

            FavoriteBiller favoriteBiller = new FavoriteBiller()
            {
                BillerId = billers1[0].Id.ToString(),
                AccountNumber = "1234567890",
                BillerCode = billers1[0].BillerCode,
            };
            MVAEngineTest.AddFavoriteBiller(customerSessionId, favoriteBiller);

            var frequentBillers = MVAEngineTest.GetFrequentBillers(customerSessionId);

            Assert.IsTrue(frequentBillers.Count > 0);

        }

        [Test]
        public void GetAddFavoriteBiller()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
            
            FavoriteBiller favoriteBiller = new FavoriteBiller()
            {
                BillerId = "100005546",
				AccountNumber = "1000127551",
                BillerCode = "10601",

            };

            MVAEngineTest.AddFavoriteBiller(customerSessionId, favoriteBiller);
        }
        
        [Test]
        public void GetUpdateFavoriteBillerAccountNumber()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
            FavoriteBiller favoriteBiller = new FavoriteBiller()
            {
                BillerId = "100000004",
				AccountNumber = "1000127551",
                BillerCode = "8925",
            };
            MVAEngineTest.AddFavoriteBiller(customerSessionId, favoriteBiller);
            long billerId = Convert.ToInt64(favoriteBiller.BillerId);
            bool result = MVAEngineTest.UpdateFavoriteBillerAccountNumber(customerSessionId, billerId, "123456654");
            Assert.IsTrue(result);
        }
        
        [Test]
        public void GetUpdateFavoriteBillerStatus()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
            FavoriteBiller favoriteBiller = new FavoriteBiller()
            {
                BillerId = "100000005",
				AccountNumber = "1000127551",
                BillerCode = "4660",
            };
            MVAEngineTest.AddFavoriteBiller(customerSessionId, favoriteBiller);
            long billerId = Convert.ToInt64(favoriteBiller.BillerId);
            bool result = MVAEngineTest.UpdateFavoriteBillerStatus(customerSessionId, billerId, true);
            Assert.IsTrue(result);
        }

        [Test]
        public void GetBillerInfo()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

            Trace.WriteLine("GetBillers By Name");
            var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);

            var billerinfo=MVAEngineTest.GetBillerInfo(customerSessionId, billers[0].BillerCode);

            Assert.IsNotNull(billerinfo);
        }
        #endregion

        #region Billpay Trx Fixture

        [Test]
        public void GetBillpayFee()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

            Trace.WriteLine("GetBillers By Name");
            var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);

            var fee = MVAEngineTest.GetBillPaymentFee(customerSessionId, billers[0].BillerCode, "1234567890", 120);

            Assert.IsNotNull(fee);

        }

        [Test]
        public void GetBillPaymentProviderAttributes()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

            Trace.WriteLine("GetBillers By Name");
            var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);

            var fee = MVAEngineTest.GetBillPaymentFee(customerSessionId, billers[0].BillerCode, "1234567890", 130);

            Assert.IsNotNull(fee);

            var provAttr = MVAEngineTest.GetBillPaymentProviderAttributes(customerSessionId, billers[0].BillerCode, fee.TransactionId);

            Assert.IsNotNull(provAttr);
        }


        [Test]
        public void GetValidateBillPayment()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
            Trace.WriteLine("GetBillers By Name");
            var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);
            BillFee billfee = MVAEngineTest.GetBillPaymentFee(customerSessionId, billers[0].BillerCode, "1234567890", 150);
            Assert.IsNotNull(billfee);
            var providerAttributes = MVAEngineTest.GetBillPaymentProviderAttributes(customerSessionId, billers[0].BillerCode, billfee.TransactionId);
            Assert.IsNotNull(providerAttributes);
            var billPayment = new BillPayment()
            {
                PaymentAmount = 100,
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
                    if (providerAttributes[i].DataType=="TextBox")
                        billPayment.MetaData.Add(providerAttributes[i].TagName, "abc"+i);
                     if (providerAttributes[i].DataType=="Dropdown")
                         billPayment.MetaData.Add(providerAttributes[i].TagName, providerAttributes[i].Values.Keys.First());
                }
            }
            long validateBillPayment = MVAEngineTest.ValidateBillPayment(customerSessionId, billPayment, billfee.TransactionId,2);
            Assert.IsNotNull(validateBillPayment);
        }

        [Test]
        public void CommitBillPayment()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
            Trace.WriteLine("GetBillers By Name");
            var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);
            BillFee billfee = MVAEngineTest.GetBillPaymentFee(customerSessionId, billers[0].BillerCode, "1234567890", 150);
            Assert.IsNotNull(billfee);
            var providerAttributes = MVAEngineTest.GetBillPaymentProviderAttributes(customerSessionId, billers[0].BillerCode, billfee.TransactionId);
            Assert.IsNotNull(providerAttributes);
            var billPayment = new BillPayment()
            {
                PaymentAmount = 150,
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
            long validateBillPayment = MVAEngineTest.ValidateBillPayment(customerSessionId, billPayment, billfee.TransactionId);
            Assert.IsNotNull(validateBillPayment);
            var receipt = MVAEngineTest.Checkout(customerSessionId);


        }

        [Test]
        public void GetBillerLastTransaction()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

            Trace.WriteLine("GetBillers By Name");
            var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
            Assert.IsNotNull(billers);
            BillFee billfee = MVAEngineTest.GetBillPaymentFee(customerSessionId, billers[0].BillerCode, "1234567890", 100);
            Assert.IsNotNull(billfee);
            var providerAttributes = MVAEngineTest.GetBillPaymentProviderAttributes(customerSessionId, billers[0].BillerCode, billfee.TransactionId);
            Assert.IsNotNull(providerAttributes);
            var billPayment = new BillPayment()
            {
                PaymentAmount = 100,
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
            long validateBillPayment = MVAEngineTest.ValidateBillPayment(customerSessionId, billPayment, billfee.TransactionId);
            Assert.IsNotNull(validateBillPayment);
            var receipt = MVAEngineTest.Checkout(customerSessionId);

            var trx = MVAEngineTest.GetBillerLastTransaction(billers[0].BillerCode, customerSessionId);

            Assert.IsNotNull(trx);
        }
        [Test]
        public void GetPastTransactions()
        {
            CustomerSession customerSession = MVAEngineTest.InitiateCustomerSession(cardNumber, channelPartnerName);
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

            for (int j = 108; j < 111; j++)
            {
    
                Trace.WriteLine("GetBillers By Name");
                var billers = MVAEngineTest.GetBillers(channelPartnerName, "USE 1580 FOR GEICO");
                Assert.IsNotNull(billers);
                BillFee billfee = MVAEngineTest.GetBillPaymentFee(customerSessionId, billers[0].BillerCode, "1234567890", j);
                Assert.IsNotNull(billfee);
                var providerAttributes = MVAEngineTest.GetBillPaymentProviderAttributes(customerSessionId,
                                                                                        billers[0].BillerCode,
                                                                                        billfee.TransactionId);
                Assert.IsNotNull(providerAttributes);
                var billPayment = new BillPayment()
                                      {
                                          PaymentAmount = j,
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
                            billPayment.MetaData.Add(providerAttributes[i].TagName,
                                                     providerAttributes[i].Values.Keys.First());
                    }
                }
                long validateBillPayment = MVAEngineTest.ValidateBillPayment(customerSessionId, billPayment,
                                                                             billfee.TransactionId);
                Assert.IsNotNull(validateBillPayment);
                var receipt = MVAEngineTest.Checkout(customerSessionId);

            }
            var billPayTransaction = MVAEngineTest.GetPastTransactions(customerSessionId, "BillPay");
            Assert.IsTrue(billPayTransaction.Count ==3);
        }
        #endregion

    }
}
