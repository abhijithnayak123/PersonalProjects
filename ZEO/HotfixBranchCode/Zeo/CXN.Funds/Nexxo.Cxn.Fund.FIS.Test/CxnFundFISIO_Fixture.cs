using System.Collections.Generic;
using MGI.Cxn.Fund.FIS.Impl;
using NUnit.Framework;
using Spring.Testing.NUnit;
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.Data;
using System;
using MGI.Common.DataProtection.Contract;

namespace MGI.Cxn.Fund.FIS.Test
{
    [TestFixture]
    public class CxnFundFISIO_Fixture : AbstractTransactionalDbProviderSpringContextTests
    {
        public IO FISIO { get; set; }
        public IFundProcessor FISGateway { get; set; }

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Cxn.Fund.FIS.Test/MGI.Cxn.Fund.FIS.Test/CxnFundFISTestSpring.xml" }; }
        }

        [Test]
        public void Can_Register()
        {
            Random rand = new Random(1235487);
            string randomnumercstring = (Math.Abs(rand.Next(54687) * 10000)).ToString();
            Dictionary<string, object> context = new Dictionary<string, object>();
            ProcessorResult result;

            CardAccount account = new CardAccount()
            {
                AccountNumber = randomnumercstring,
                Address1 = randomnumercstring + " Address 1",
                CardBalance = Convert.ToDecimal(randomnumercstring),
                CardNumber = randomnumercstring,
                City = randomnumercstring + " City",
                CountryCode = "US",
                DateOfBirth = DateTime.Now.AddYears(-25),
                FirstName = randomnumercstring.Substring(0, 4) + " First Name"
            };
            long cxnid = long.MinValue;
            cxnid = FISGateway.Register(account, context, out result);

            Assert.That(cxnid, Is.GreaterThan(long.MinValue));
            //SetComplete();
        }

        [Test]
        public void Can_Authenticate()
        {
            Dictionary<string, object> context = new Dictionary<string, object>();
            ProcessorResult processorResult = new ProcessorResult();

            long result = FISIO.Authenticate("5222650112845220", context, out processorResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Can_GetBalance()
        {
            long accountId = 123456789;
            Dictionary<string, object> context = new Dictionary<string, object>();
            ProcessorResult processorResult = new ProcessorResult();

            decimal result = FISIO.GetBalance(accountId, context, out processorResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Can_Load()
        {
            long accountId = 123456789;
            Dictionary<string, object> context = new Dictionary<string, object>();
            ProcessorResult processorResult = new ProcessorResult();
            FundRequest fundRequest = new FundRequest();

            long result = FISIO.Load(accountId, fundRequest, context, out processorResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Can_Withdraw()
        {
            long accountId = 123456789;
            Dictionary<string, object> context = new Dictionary<string, object>();
            ProcessorResult processorResult = new ProcessorResult();
            FundRequest fundRequest = new FundRequest();

            long result = FISIO.Withdraw(accountId, fundRequest, context, out processorResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Can_Lookup()
        {
            long accountId = 1000000002;// 224430000;

            CardAccount cardAccount = FISGateway.Lookup(accountId);

            Assert.That(cardAccount, Is.Not.Null);
        }

        [Test]
        public void Can_LookupCardAccount()
        {
            long accountId = 1000000002;// 224430000;

            CardAccount cardAccount = FISGateway.LookupCardAccount(accountId);

            Assert.That(cardAccount, Is.Not.Null);
        }

        [Test]
        public void Can_Activate()
        {
            string cardNumber = "4422660000022494";
            long accountId = 1000000002;
            decimal amount = 20m;
            var fundRequest = new MGI.Cxn.Fund.Data.FundRequest { Amount = amount };
            Dictionary<string, object> context = new Dictionary<string, object>();
            context.Add("ChannelPartnerId", 33L);

            ProcessorResult processorResult;

            long result = FISGateway.Activate(accountId, fundRequest, context, out processorResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void Can_GetPanForCardNumber()
        {
            string cardNumber = "4422660000022494";

            Dictionary<string, object> context = new Dictionary<string, object>();
            context.Add("ChannelPartnerId", 33L);

            ProcessorResult processorResult;

            long result = FISGateway.Authenticate(cardNumber, context, out processorResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Can_UpdateAmount()
        {
            string cardNumber = "4422661000030362";
            long cxnAccountId = 224430000;
            decimal amount = 5m;
            decimal newAmount = 10m;
            var fundRequest = new MGI.Cxn.Fund.Data.FundRequest { Amount = amount };
            Dictionary<string, object> context = new Dictionary<string, object>();
            context.Add("ChannelPartnerId", 33L);

            ProcessorResult processorResult;

            long cxnTxnId = FISGateway.Withdraw(cxnAccountId, fundRequest, context, out processorResult);

            if (cxnTxnId == 0)
            {
                cxnTxnId = 1000000002;
            }
            //Assert.IsTrue(processorResult.IsSuccess);
            //Assert.IsTrue(cxnTxnId > 0);

            long cxnTxnId2 = FISGateway.UpdateAmount(cxnTxnId, newAmount, TimeZone.CurrentTimeZone.StandardName);

            Assert.AreEqual(cxnTxnId, cxnTxnId2);

            MGI.Cxn.Fund.Data.FundTrx trx = FISGateway.Get(cxnTxnId);

            Assert.AreEqual(newAmount, trx.TransactionAmount);
        }

        [Test]
        public void Can_UpdateRegistrationDetails()
        {
            long cxnAccountId = 1000000002;
            string cardNumber = "4422661000030362";
            Dictionary<string, object> context = new Dictionary<string, object>();
            
            FISGateway.UpdateRegistrationDetails(cxnAccountId, cardNumber, TimeZone.CurrentTimeZone.StandardName);
        }

        [Test]
        public void Can_Commit()
        {
            long transactionId = 1000000002;
            string cardNumber = "4422661000030362";
            Dictionary<string, object> context = new Dictionary<string, object>();
            context.Add("TimeZone", TimeZone.CurrentTimeZone.StandardName);
            ProcessorResult result;

            FISGateway.Commit(transactionId, context, out result, cardNumber);
        }
    }
}
