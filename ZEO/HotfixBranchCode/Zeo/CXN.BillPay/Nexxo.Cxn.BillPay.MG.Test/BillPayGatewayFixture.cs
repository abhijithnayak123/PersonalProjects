using MGI.Cxn.BillPay.Contract;
using MGI.Cxn.BillPay.Data;
using NUnit.Framework;
using Spring.Testing.NUnit;
using System.Collections.Generic;
using MGI.Cxn.BillPay.MG.Impl;
using MGI.Cxn.BillPay.MG.AgentConnectService;
using System;
using MGData = MGI.Cxn.BillPay.MG.Data;
using MGI.Common.Util;


namespace MGI.Cxn.BillPay.MG.Test
{
    [TestFixture]
    public class BillPayIOFixture : AbstractTransactionalDbProviderSpringContextTests
    {
        public IBillPayProcessor CxnBillPayProcessor { get; set; }
		public MGIContext MgiContext { get; set; }

        protected override string[] ConfigLocations
        {
            get
            {
                return new string[]
                {
                    "assembly://MGI.Cxn.BillPay.MG.Test/MGI.Cxn.BillPay.MG.Test/MGI.Cxn.BillPay.MG.Test.Spring.xml"
                };
            }
        }

        [Test]
        public void CanGetFee()
        {
            int agentId = 43685767;
            string billerCode = "2739";
            MgiContext = GetFeeLookupContext(agentId, billerCode);
			Fee response = CxnBillPayProcessor.GetFee(billerCode, "123456789", 102, null, MgiContext);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.DeliveryMethods.Count, Is.GreaterThan(0));
            Assert.That(response.DeliveryMethods[0].FeeAmount, Is.GreaterThan(0));
            SetComplete();
        }

        [Test]
        public void IsGetFeeUsesSameTransactionSecondTime()
        {
            int agentId = 500000;
            string billerCode = "1579";
            string accountNumner = "1234567890";
			MgiContext = GetFeeLookupContext(agentId, billerCode);
			Fee response = CxnBillPayProcessor.GetFee(billerCode, accountNumner, 550, null, MgiContext);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.DeliveryMethods.Count, Is.GreaterThan(0));
            Assert.That(response.DeliveryMethods[0].FeeAmount, Is.GreaterThan(0));
            long transactionId = response.TransactionId;
			MgiContext.TrxId = transactionId;
			response = CxnBillPayProcessor.GetFee(billerCode, accountNumner, 650, null, MgiContext);
            Assert.AreEqual(response.TransactionId, transactionId);
        }


        [Test]
        public void CanGetBpValidation()
        {
            int agentId = 500000;
            string billerCode = "1579";
            string AccountNumber = "1234567890";
            int Amount = 101;
			MgiContext = GetFeeLookupContext(agentId, billerCode);
			Fee response = CxnBillPayProcessor.GetFee(billerCode, AccountNumber, 100, null, MgiContext);
			MgiContext.TrxId = response.TransactionId;
            BillPayRequest request = new BillPayRequest();
            request = GetBillPayRequest(AccountNumber, Amount);
			long Validate = CxnBillPayProcessor.Validate(MgiContext.CxnAccountId, request, MgiContext);
            Assert.That(Validate, Is.Not.Null);
            SetComplete();
        }

        [Test]
        public void CanGetAccount()
        {
            var request = new BillPayRequest()
            {
                CustomerFirstName = "RAJU",
                CustomerLastName = "MATHAPATI",
                CustomerAddress1 = "TEST1",
                CustomerAddress2 = "address2",
                CustomerCity = "GULBARGA",
                CustomerState = "AA",
                CustomerZip = "58510",
                CustomerStreet = "cust street",
                CustomerDateOfBirth = DateTime.Now,
                CustomerEmail = "customer@nexxo.com",
                CustomerPhoneNumber = "2345678910"
            };

            string timezone = "Central Mountain Time";

            long id = CxnBillPayProcessor.AddBillPayAccount(request, timezone);

            Assert.That(id, Is.GreaterThan(0));

            var billPayAccount = CxnBillPayProcessor.GetBillPayAccount(id);

            Assert.That(billPayAccount.FirstName, Is.EqualTo("RAJU"));
        }

        [Test]
        public void CanAddAccount()
        {
            var request = new BillPayRequest()
            {
                CustomerFirstName = "RAJU",
                CustomerLastName = "MATHAPATI",
                CustomerAddress1 = "TEST1",
                CustomerAddress2 = "address2",
                CustomerCity = "GULBARGA",
                CustomerState = "AA",
                CustomerZip = "58510",
                CustomerStreet = "cust street",
                CustomerDateOfBirth = DateTime.Now,
                CustomerEmail = "customer@nexxo.com",
                CustomerPhoneNumber = "2345678910"
            };

            string timezone = "Central Mountain Time";

            long id = CxnBillPayProcessor.AddBillPayAccount(request, timezone);

            Assert.That(id, Is.GreaterThan(0));
        }

        [Test]
        public void Can_GetProviderAttributes()
        {
            int agentId = 500000;
            string billerCode = "1579";
            string accountNumber = "1234567890";
            //int Amount = 105;
			MgiContext = GetFeeLookupContext(agentId, billerCode);
            // Fee response = CxnBillPayProcessor.GetFee("POM RECOVERIES", "123456789", 100, null, context);
			Fee feeResponse = CxnBillPayProcessor.GetFee(billerCode, accountNumber, 100, null, MgiContext);

			List<Field> response = CxnBillPayProcessor.GetProviderAttributes("", "", MgiContext);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public void CanGetCommit()
        {
            int agentId = 500000;
            string billerCode = "1579";
            string accountNumber = "1234567890";
            int Amount = 105;
			MgiContext = GetFeeLookupContext(agentId, billerCode);
            // Fee response = CxnBillPayProcessor.GetFee("POM RECOVERIES", "123456789", 100, null, context);
			Fee response = CxnBillPayProcessor.GetFee(billerCode, accountNumber, 100, null, MgiContext);
            MgiContext.TrxId = response.TransactionId;
            BillPayRequest request = new BillPayRequest();
            request = GetBillPayRequest(accountNumber, Amount);
			long Validate = CxnBillPayProcessor.Validate(MgiContext.CxnAccountId, request, MgiContext);
			long commit = CxnBillPayProcessor.Commit(response.TransactionId, MgiContext);
            Assert.That(commit, Is.Not.Null);
        }

        #region Private Methods

        private MGIContext GetFeeLookupContext(int agentId, string billerCode)
        {
			MGIContext mgiContext = new MGIContext() {
				RequestType = "Active",
				AgentId = agentId,
				BillerCode = billerCode,
				ChannelPartnerId = 1,
				CxnAccountId = 1000000017,
				TimeZone = "Central Mountain Time"
			};
			return mgiContext;

        }

        private BillPayRequest GetBillPayRequest(string AccountNumber, int Amount)
        {
            BillPayRequest request = new BillPayRequest();
            request.Amount = Amount;
            request.AccountNumber = AccountNumber;
            request.CustomerFirstName = "SSS";
            request.CustomerLastName = "BANGALORE";
            request.CustomerAddress1 = "RHH";
            request.CustomerCity = "HHH";
            request.CustomerState = "MN";
            request.CustomerZip = "67576";
            request.CustomerPhoneNumber = "8879874427";
            request.PrimaryIdNumber = "K5437621";
            request.CustomerDateOfBirth = DateTime.Parse("11/11/1990");
            request.CountryOfBirth = "ALB";
            request.PrimaryIdCountryOfIssue = "UNITED STATES";
            request.PrimaryIdPlaceOfIssue = "CALIFORNIA";
            request.PrimaryIdType = "DRIVER'S LICENSE";
            request.ProductId = 100038046;
            request.SecondIdCountryOfIssue = "US";
            request.SecondIdType = "SSN";

            return request;

        }

        #endregion Private Methods

    }
}
