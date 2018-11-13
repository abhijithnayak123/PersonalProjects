using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.Contract;
using Spring.Testing.NUnit;
using MGI.Cxn.Fund.FirstView.Data;
using MGI.Common.Util;

namespace MGI.Cxn.Fund.FirstView.Test
{
    [TestFixture]
    public class FirstViewFixture : AbstractTransactionalSpringContextTests
    {
        public IFundProcessor FundProcessor { get; set; }

		MGIContext mgiContext = new MGIContext(); 
        protected override string[] ConfigLocations
        {
			get { return new string[] { "assembly://MGI.Cxn.Fund.FirstView.Test/MGI.Cxn.Fund.FirstView.Test/Cxn.Fund.FirstView.Test.Spring.xml" }; }
        }

        [Test]
        public void Can_Register()
        {
            CardAccount cardDetails = new CardAccount()
            {
                AccountNumber = "1111111",
                Address1 = "67/68, Sudev Complex",
                Address2 = "",
                CardNumber = "5397380000440798",
                City = "Bangalore",
                CountryCode = "IN",
                DateOfBirth = DateTime.Parse("07/08/1977")
            };

			

            ProcessorResult processorResult = new ProcessorResult();
			//long Register(CardAccount cardAccount, Dictionary<string, object> context, out ProcessorResult processorResult);
			FundProcessor.Register(cardDetails, mgiContext, out processorResult);

            Assert.That(processorResult.Exception == null, Is.True);
            SetComplete();
        }

		[Test]
		public void Can_GetBalance()
		{
			long accountId = 1000000002;

			
			ProcessorResult result = null;
			decimal balance = FundProcessor.GetBalance(accountId, mgiContext, out result).Balance;

			Assert.That(result.IsSuccess, Is.True);
			Assert.That(result.Exception, Is.Null);
		}

        [Test]
        public void Can_Load()
        {
            long accountId = 1000000002;

            FundRequest fundRequest = new FundRequest(){
                 Amount = 100
            };

            ProcessorResult result = new ProcessorResult();
			CardInfo cardBalance = FundProcessor.GetBalance(accountId, mgiContext, out result);
			decimal priorBalance = cardBalance.Balance;
			Assert.That(priorBalance, Is.AtLeast(1));

			long cxnTrxId = FundProcessor.Load(accountId, fundRequest, mgiContext, out result);
            Assert.That(result.Exception, Is.Null);
            Assert.That(cxnTrxId, Is.AtLeast(1));

			FundProcessor.Commit(cxnTrxId, mgiContext, out result);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.IsSuccess, Is.True);

			cardBalance = FundProcessor.GetBalance(accountId, mgiContext, out result);
			decimal balance = cardBalance.Balance;
			Assert.That(balance, Is.EqualTo(priorBalance + 100));

			SetComplete();
        }

		[Test]
		public void Can_Withdraw()
		{
			long accountId = 1000000002;

			FundRequest fundRequest = new FundRequest()
			{
				Amount = 100
			};

			Dictionary<string, object> context = new Dictionary<string, object>();
			context["TerminalID"] = "k00001";
			context["ProcessorID"] = 14;
			context["TimeZone"] = "Eastern Standard Time";

			ProcessorResult result = new ProcessorResult();
			CardInfo cardBalance = FundProcessor.GetBalance(accountId, mgiContext, out result);
			decimal priorBalance = cardBalance.Balance;
			Assert.That(priorBalance, Is.AtLeast(1));

			long cxnTrxId = FundProcessor.Withdraw(accountId, fundRequest, mgiContext, out result);
			Assert.That(result.Exception, Is.Null);
			Assert.That(cxnTrxId, Is.AtLeast(1));

			FundProcessor.Commit(cxnTrxId, mgiContext, out result);

			cardBalance = FundProcessor.GetBalance(accountId, mgiContext, out result);
			decimal balance = cardBalance.Balance;
			Assert.That(balance, Is.EqualTo(priorBalance - fundRequest.Amount));

			SetComplete();
		}

		[Test]
		public void Can_Authenticate()
		{

			
			

			ProcessorResult result = null;
			string CardNumber = "5397380000440798";

			long accountNumber = FundProcessor.Authenticate(CardNumber, mgiContext, out result);

			Assert.That(accountNumber, Is.EqualTo(2993345));
		}

		[Test]
		public void Can_Lookup()
		{
			long accountId = 1000000002;

			

			CardAccount cardAccount = FundProcessor.Lookup(accountId);

			Assert.That(cardAccount, Is.Not.Null);
			Assert.That(cardAccount.AccountNumber, Is.EqualTo("2993345"));
			Assert.That(cardAccount.FirstName, Is.EqualTo("nexxo").IgnoreCase);
			Assert.That(cardAccount.LastName, Is.EqualTo("test").IgnoreCase);
			Assert.That(cardAccount.ZipCode, Is.EqualTo("30340").IgnoreCase);
			Assert.That(cardAccount.City, Is.EqualTo("Atlanta").IgnoreCase);
		}
    }
}
