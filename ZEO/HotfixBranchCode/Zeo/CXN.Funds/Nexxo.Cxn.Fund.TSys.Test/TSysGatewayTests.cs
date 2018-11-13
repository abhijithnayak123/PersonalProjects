using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;

using NUnit.Framework;
using Spring.Testing.NUnit;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataProtection.Contract;

using CXNData = MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.Contract;

using MGI.Cxn.Fund.TSys.Contract;
using MGI.Cxn.Fund.TSys.Data;
using MGI.Common.Util;

namespace MGI.Cxn.Fund.TSys.Test
{
    [TestFixture]
    public class TSysGatewayTests : AbstractTransactionalDbProviderSpringContextTests
    {
        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Cxn.Fund.TSys.Test/MGI.Cxn.Fund.TSys.Test/springTSysTest.xml" }; }
        }

        private IDataProtectionService _dataProtectionSvc;
        public IDataProtectionService DataProtectionSvc { set { _dataProtectionSvc = value; } }

        private IFundProcessor _tSysProcessor;
        public IFundProcessor TSysProcessor { set { _tSysProcessor = value; } }

		public MGIContext mgiContext { get; set; }

        [TestFixtureSetUp]
        public void Init()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        }

       [TestFixtureSetUp]
        public void SetupContext()
        {
			mgiContext = new MGIContext() {
				LocationId = "13140417L",
				LocationName = "Test Terminal",
				ChannelPartnerId = 33L,
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				TSysPartnerId = "13139925"
			};
        }

        [Test]
        public void Aa_RegisterNew()
        {
            CXNData.CardAccount newAccount = getCardAccount();

            CXNData.ProcessorResult processorResult;
			long accountId = _tSysProcessor.Register(newAccount, mgiContext, out processorResult);

            Assert.IsTrue(accountId > 0);
            Assert.IsFalse((bool)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select Activated from tTSys_Account where Id = {0}", accountId)));
        }

        [Test]
        public void RegisterExisting()
        {
            CXNData.CardAccount newAccount = getCardAccount();
            newAccount.CardNumber = string.Empty;

			mgiContext.IsExistingAccount = true;

            CXNData.ProcessorResult processorResult;
			long accountId = _tSysProcessor.Register(newAccount, mgiContext, out processorResult);

            Assert.IsTrue(accountId > 0);
            Assert.IsTrue((bool)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select Activated from tTSys_Account where Id = {0}", accountId)));
        }

        [Test]
        public void IncompleteRegistration()
        {
            CXNData.CardAccount newAccount = getCardAccount();

            CXNData.ProcessorResult processorResult;
			long accountId = _tSysProcessor.Register(newAccount, mgiContext, out processorResult);

            Assert.IsTrue(accountId > 0);

            CXNData.ProcessorResult result;
			long cxnAccountId = _tSysProcessor.Authenticate(newAccount.CardNumber, mgiContext, out result);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(FundException.ACCOUNT_NOT_FOUND, result.ErrorCode);
            Assert.AreEqual(0, cxnAccountId);
        }

        [Test]
        public void StageActivation()
        {
            CXNData.CardAccount newAccount = getCardAccount();

            CXNData.ProcessorResult processorResult;
			long accountId = _tSysProcessor.Register(newAccount, mgiContext, out processorResult);

            Assert.IsTrue(accountId > 0);

            CXNData.ProcessorResult result;
			long txnId = _tSysProcessor.Activate(accountId, new CXNData.FundRequest { Amount = 0m }, mgiContext, out result);

            Assert.IsTrue(txnId > 0);

			CXNData.FundTrx trx = _tSysProcessor.Get(txnId, mgiContext);

            Assert.AreEqual(0m, trx.TransactionAmount);
        }

        [Test]
        //Pass Test Case
        public void Authenticate()
        {
			string cardNumber = "4756750000234540";
            long cxnAccountId = setupExistingCard(cardNumber);

            CXNData.ProcessorResult result;

			long cxnAccountIdVerify = _tSysProcessor.Authenticate(cardNumber, mgiContext, out result);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(cxnAccountId, cxnAccountIdVerify);
        }

        [Test]
        //Pass Test Case
        public void BadAuthenticate()
        {
			long cxnAccountId = setupExistingCard("4756750000234540");

            CXNData.ProcessorResult result;

			long cxnAccountIdVerify = _tSysProcessor.Authenticate("0000000000000000", mgiContext, out result);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.ErrorCode, FundException.ACCOUNT_NOT_FOUND);
        }

        [Test]
        //Pass Test Case
        public void InactiveCard()
        {
            string cardNumber = "4756750000234540";
            long cxnAccountId = setupExistingCard(cardNumber);

            CXNData.ProcessorResult result;

			long cxnAccountIdVerify = _tSysProcessor.Authenticate(cardNumber, mgiContext, out result);

            Trace.WriteLine(string.Format("result: {0}, error message: {1}", result.ErrorCode, result.ErrorMessage));

            Assert.IsTrue(result.IsSuccess);   
        }

        [Test]
        public void CreditAndDebit()
        {
			string cardNumber = "4756750000234540";
            long cxnAccountId = setupExistingCard(cardNumber);
            decimal amount = 20m;
            var fundRequest = new CXNData.FundRequest { Amount = amount };

            MGI.Cxn.Fund.Data.ProcessorResult processorResult;

			CXNData.CardInfo cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			decimal originalBalance = cardBalance.Balance;
            Trace.WriteLine("originalBalance: " + originalBalance.ToString("c2"));

			long cxnTxnId = _tSysProcessor.Load(cxnAccountId, fundRequest, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

			cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			decimal newBalance = cardBalance.Balance;

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(originalBalance + amount, newBalance);

			cxnTxnId = _tSysProcessor.Withdraw(cxnAccountId, fundRequest, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

            cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			newBalance = cardBalance.Balance;

            Trace.WriteLine("newBalance: " + newBalance.ToString("c2"));

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(originalBalance, newBalance);

			cxnTxnId = _tSysProcessor.Load(cxnAccountId, fundRequest, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

			cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			newBalance = cardBalance.Balance;

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(originalBalance + amount, newBalance);

			cxnTxnId = _tSysProcessor.Withdraw(cxnAccountId, new CXNData.FundRequest { Amount = amount }, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

			cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			newBalance = cardBalance.Balance;

            Trace.WriteLine("newBalance: " + newBalance.ToString("c2"));

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(originalBalance, newBalance);
        }

        [Test]
        //Pass Test Case
        public void CreditAndDebitNew()
        {
			string cardNumber = "4756750000234540";
            long cxnAccountId = setupNewCard(cardNumber);
            decimal amount = 20m;
            decimal loadFee = 4m;
            var fundRequest = new CXNData.FundRequest { Amount = amount };

            MGI.Cxn.Fund.Data.ProcessorResult processorResult;

			CXNData.CardInfo cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			decimal originalBalance = cardBalance.Balance;

            Trace.WriteLine("originalBalance: " + originalBalance.ToString("c2"));

			long cxnTxnId = _tSysProcessor.Load(cxnAccountId, fundRequest, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

			cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			decimal newBalance = cardBalance.Balance;

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(originalBalance + amount - loadFee, newBalance);

			cxnTxnId = _tSysProcessor.Withdraw(cxnAccountId, fundRequest, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

			cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			newBalance = cardBalance.Balance;

            Trace.WriteLine("newBalance: " + newBalance.ToString("c2"));

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(originalBalance - loadFee, newBalance);

			cxnTxnId = _tSysProcessor.Load(cxnAccountId, fundRequest, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

			cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			newBalance = cardBalance.Balance;

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(originalBalance + amount - loadFee, newBalance);

			cxnTxnId = _tSysProcessor.Withdraw(cxnAccountId, new CXNData.FundRequest { Amount = amount - loadFee }, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

			cardBalance = _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out processorResult);
			newBalance = cardBalance.Balance;

            Trace.WriteLine("newBalance: " + newBalance.ToString("c2"));

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(originalBalance, newBalance);
        }

        [Test]
        //Pass Test Case
        public void Lookup()
        {
			string cardNumber = "4756750000234540";
            long cxnAccountId = setupExistingCard(cardNumber);

            CXNData.CardAccount cardAccount = _tSysProcessor.Lookup(cxnAccountId);

            CXNData.ProcessorResult result;
            Assert.AreEqual(cardNumber, cardAccount.CardNumber);
			Assert.AreEqual(cardAccount.CardBalance, _tSysProcessor.GetBalance(cxnAccountId, mgiContext, out result));
        }

        [Test]
        //Pass Test Case
        [ExpectedException(typeof(FundException))]
        public void BadLookup()
        {
            CXNData.CardAccount cardAccount = _tSysProcessor.Lookup(0);
        }

        [Test]
        //Pass Test Case
        public void GetPanForCardNumber()
        {
            string cardNumber = "4422661000030362";
            long cxnAccountId = setupExistingCard(cardNumber);
			long cxnAccountIdVerify = _tSysProcessor.GetPanForCardNumber(cardNumber, mgiContext);

            Assert.AreEqual(cxnAccountId, cxnAccountIdVerify);
        }

        [Test]
        [ExpectedException(typeof(FundException))]
        //Pass Test Case
        public void BadGetPanForCardNumber()
        {
            CXNData.CardAccount cardAccount = _tSysProcessor.Lookup(0);
        }

		//[Test]
		//public void UpdateRegistrationDetails()
		//{
		//	long cxnAccountId = setupExistingCard("4756755000017753");

		//	_tSysProcessor.UpdateRegistrationDetails(cxnAccountId, "4756755000017753", TimeZone.CurrentTimeZone.StandardName);

		//	long cxnAccountId2 = _tSysProcessor.GetPanForCardNumber("4756755000017753", _Context);

		//	Assert.AreEqual(cxnAccountId, cxnAccountId2);
		//}

        [Test]
           //Pass Test Case
        public void UpdateAmount()
        {
            string cardNumber = "4422661000030362";
            long cxnAccountId = setupExistingCard(cardNumber);
            decimal amount = 5m;
            decimal newAmount = 10m;
            var fundRequest = new CXNData.FundRequest { Amount = amount };

            MGI.Cxn.Fund.Data.ProcessorResult processorResult;

			long cxnTxnId = _tSysProcessor.Withdraw(cxnAccountId, fundRequest, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

            long cxnTxnId2 = _tSysProcessor.UpdateAmount(cxnTxnId,fundRequest , TimeZone.CurrentTimeZone.StandardName);

            Assert.AreEqual(cxnTxnId, cxnTxnId2);

			CXNData.FundTrx trx = _tSysProcessor.Get(cxnTxnId, mgiContext);

            Assert.AreEqual(newAmount, trx.TransactionAmount);
        }

        [Test]
        public void GetTrx()
        {
            string cardNumber = "4756750000234540";
            long cxnAccountId = setupExistingCard(cardNumber);
            decimal amount = 20m;
            var fundRequest = new CXNData.FundRequest { Amount = amount };

            MGI.Cxn.Fund.Data.ProcessorResult processorResult;
			long cxnTxnId = _tSysProcessor.Withdraw(cxnAccountId, fundRequest, mgiContext, out processorResult);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);

			CXNData.FundTrx debitTrx = _tSysProcessor.Get(cxnTxnId, mgiContext);

            decimal originalBalance = (decimal)debitTrx.CardBalance;

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(debitTrx.CardBalance, debitTrx.PreviousCardBalance);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

			debitTrx = _tSysProcessor.Get(cxnTxnId, mgiContext);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(debitTrx.CardBalance, debitTrx.PreviousCardBalance - debitTrx.TransactionAmount);

            // compensate for load fee that will be taken out of the first load
            //fundRequest.Amount += 4m;
			cxnTxnId = _tSysProcessor.Load(cxnAccountId, fundRequest, mgiContext, out processorResult);

			debitTrx = _tSysProcessor.Get(cxnTxnId, mgiContext);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.IsTrue(cxnTxnId > 0);
            Assert.AreEqual(debitTrx.CardBalance, debitTrx.PreviousCardBalance);
            Assert.AreNotEqual(originalBalance, debitTrx.CardBalance);

			_tSysProcessor.Commit(cxnTxnId, mgiContext, out processorResult);

			debitTrx = _tSysProcessor.Get(cxnTxnId, mgiContext);

            Assert.IsTrue(processorResult.IsSuccess);
            Assert.AreEqual(debitTrx.CardBalance, debitTrx.PreviousCardBalance + amount);
            Assert.AreEqual(originalBalance, debitTrx.CardBalance);
        }

        private long setupExistingCard(string cardNumber)
        {
			long accountId = 188151501;
			string externalKey = "1000125792";
			long programId = 13139925;
			long userId = 102385956;
            string encryptedCard = _dataProtectionSvc.Encrypt(cardNumber, 0);
			//string encryptedCard = cardNumber;
            AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tTSys_Account(rowguid, ProgramId, ExternalKey, CardNumber, UserId, AccountId, Activated, DOB, DTCreate) values (newid(), {0}, '{1}', '{2}', {3}, {4}, 1, '12/1/1990', '9/18/2013')", programId, externalKey, encryptedCard, userId, accountId));

            return (long)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select Id from tTSys_Account where accountId = {0}", accountId));
        }

        private long setupNewCard(string cardNumber)
        {
			long accountId = 188151501;
			string externalKey = "1000125792";
			long programId = 13139925;
			long userId = 102385956;
            string encryptedCard = _dataProtectionSvc.Encrypt(cardNumber, 0);

            Guid acctGuid = Guid.NewGuid();
            AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tTSys_Account(rowguid, ProgramId, ExternalKey, CardNumber, UserId, AccountId, Activated, DOB, DTCreate) values ('{5}', {0}, '{1}', '{2}', {3}, {4}, 1, '12/1/1990', '9/18/2013')", programId, externalKey, encryptedCard, userId, accountId, acctGuid));
            AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tTSys_Trx(rowguid, AccountPK, TransactionType, Amount, Status, ConfirmationId, Balance, DTCreate,ChannelPartnerID) values (newid(), '{0}', 3, 0, 1, 12345, 0, '12/1/1990', 33)", acctGuid));

            return (long)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select Id from tTSys_Account where accountId = {0}", accountId));
        }

        private CXNData.CardAccount getCardAccount()
        {
            return new CXNData.CardAccount
            {
				CardNumber = "4756755000017753",
				AccountNumber = "1000125792",
				FirstName = "JAN",
                MiddleName = "",
				LastName = "BROWN",
				Address1 = "111 MIDDLE ST",
                Address2 = "",
				City = "COLUMBUS",
				State = "GA",
				ZipCode = "31905",
                CountryCode = "United States",
                DateOfBirth = new DateTime(1946, 02, 14),
				Phone = "706-258-1471",
				SSN = "111-222-333",
                FraudScore = 10,
                Resolution = "unknown"
            };
        }

        [TearDown]
        public void DeleteDBRecords()
        {
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from tTSys_Trx");
            AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from tTSys_Account");
        }
    }
}
