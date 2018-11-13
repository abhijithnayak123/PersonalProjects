
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ServiceModel;
using NUnit.Framework;
using MGI.Common.Sys;
using MGI.Cxn.Fund.TSys.Data;
using MGI.Cxn.Fund.TSys.Contract;
using MGI.Cxn.Fund.TSys.Impl;

namespace MGI.Cxn.Fund.TSys.Test
{
    [TestFixture]
    public class TSysIOTests
    {
        IO _TSysIO;

        [TestFixtureSetUp]
        public void Init()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            _TSysIO = new IO();
            _TSysIO.APIUserName = "synovustestapi";
            _TSysIO.APIPassword = "Synovusapi1@";
            _TSysIO.CardNumberLogging = true;
        }

        [Test]        
        public void GoodExternalKey()
        {
            _TSysIO.ValidateNewCardAccount(13140417, "extkeynexxo2", 4422661000030396);
        }

        [Test]     
        public void UpdateAccountBadSSN()
        {
            TSysIOProfile profile = new TSysIOProfile
            {
                AccountId = 188135700,
                UserId = 102354369,
                ExternalKey = "extkeynexxo2",
                ProgramId = 13140417,
                FirstName = "Justin",
                MiddleName = "Dev",
                LastName = "Test",
                Address1 = "111 Anza Blvd",
                Address2 = "Suite 200",
                City = "Burlingame",
                State = "CA",
                ZipCode = "94010",
                Country = "USA",
				DateOfBirth = new DateTime(1990, 1, 1),
                Phone = "650-685-5702",
                PhoneType = "cell",
                SSN = "111-222-3333",
                CardNumber = 4422661000030396
            };

            try
            {
                _TSysIO.UpdateCardAccount(profile);
            }
            catch (TSysProviderException tex)
            {
                Assert.AreEqual(tex.Code, "INVALID_IDENTIFICATION");
            }
        }

        [Test]    
        public void UpdateAccount()
        {
            TSysIOProfile profile = new TSysIOProfile
            {
                AccountId = 188135700,
                UserId = 102354369,
                ExternalKey = "extkeynexxo2",
                ProgramId = 13140417,
                FirstName = "Justin",
                MiddleName = "Dev",
                LastName = "Test",
                Address1 = "111 Anza Blvd",
                Address2 = "Suite 200",
                City = "Burlingame",
                State = "CA",
                ZipCode = "94010",
                Country = "USA",
				DateOfBirth = new DateTime(1990, 1, 1),
                Phone = "650-685-5702",
                PhoneType = "cell",
                SSN = "260-53-5522",
                CardNumber = 4422661000030396
            };

            _TSysIO.UpdateCardAccount(profile);
        }


        [Test]
        public void GoodExternalKeyBadCard()
        {
            try
            {
                _TSysIO.ValidateNewCardAccount(13140417, "extkeynexxo2", 4422661000030390);
            }
            catch (TSysProviderException tex)
            {
                Assert.AreEqual(tex.Code, "2099");
            }
        }

        [Test]
        public void BadExternalKey()
        {
            try
            {
                _TSysIO.ValidateNewCardAccount(13140417, "badkey", 4422661000030396);
            }
            catch (TSysProviderException tex)
            {
                Assert.AreEqual(tex.Code, "INVALID_EXTERNAL_KEY");
            }
        }

        [Test]     
        public void ValidateCard()
        {
            _TSysIO.ValidateCard(102354357, 188135691, 4422660000022460);
        }

        [Test]
        [ExpectedException(typeof(TSysProviderException))]  
        public void BadValidateCard()
        {
            _TSysIO.ValidateCard(102354357, 188135691, 4422661000030396);
        }

        [Test]   
        public void ValidateExistingCardAccount()
        {
            TSysIONewUser newUser = _TSysIO.ValidateExistingCardAccount(13140417, "extkeynexxo1");

            Assert.AreEqual(102354357, newUser.UserId);
            Assert.AreEqual(188135691, newUser.AccountId);
        }

        [Test] 
        public void GetActiveCard()
        {
            long cardNumber = _TSysIO.GetActiveCard(102354357, 188135691);

            Assert.AreEqual(4422660000022460, cardNumber);
        }

        [Test]
        //failed 
        public void ActivateAccount()
        {
			long cardToActivate = 4756755000017753;
            // do not activate anything for extkeynexxo2
			_TSysIO.ValidateNewCardAccount(13139925, "1000125792", cardToActivate);



            //TSysIOProfile profile = new TSysIOProfile
            //{
            //    AccountId = 188135691,
            //    UserId = 102354357,
            //    ExternalKey = "extkeynexxo1",
            //    ProgramId = 13140417,
            //    FirstName = "Justin2",
            //    MiddleName = "Dev2",
            //    LastName = "Test2",
            //    Address1 = "112 Anza Blvd",
            //    Address2 = "Suite 201",
            //    City = "Burlingame",
            //    State = "CA",
            //    ZipCode = "94010",
            //    Country = "USA",
            //    DOB = new DateTime(1990, 1, 2),
            //    Phone = "650-685-5703",
            //    PhoneType = "cell",
            //    SSN = "260-53-5523",
            //    CardNumber = cardToActivate
            //};

            //_TSysIO.UpdateCardAccount(profile);
            //_TSysIO.ActivateCardAccount(profile);
        }

        [Test]  
        public void DebitCredit()
        {
            long accountId = 188135691;
            string cardNumber = "4422660000022460";
            decimal amount = 20m;
            decimal balance = _TSysIO.GetBalance(accountId);

            string TSysId = _TSysIO.Withdraw(accountId, amount, "test debit");

            Trace.WriteLine(string.Format("transactionID returned: {0}", TSysId));

            Assert.AreEqual(_TSysIO.GetBalance(accountId), balance - amount);

            TSysId = _TSysIO.Load(cardNumber, amount, "test credit");

            Trace.WriteLine(string.Format("transactionID returned: {0}", TSysId));

            Assert.AreEqual(_TSysIO.GetBalance(accountId), balance);
        }

        [Test]   
        public void ApplyFee()
        {
			long accountId = 188151501;
			string cardNumber = "4756750000234540";
            decimal fee = 4m;
            decimal balance = _TSysIO.GetBalance(accountId);

            string TSysId = _TSysIO.ApplyFee(accountId, fee, "test fee");

            Trace.WriteLine(string.Format("transactionID returned: {0}", TSysId));

            Assert.AreEqual(_TSysIO.GetBalance(accountId), balance - fee);

            // load minimum amount back
            TSysId = _TSysIO.Load(cardNumber, 20m, "test credit");

            Trace.WriteLine(string.Format("Load transactionID returned: {0}", TSysId));

            // withdraw 20 - fee to get the balance back to where it started
            TSysId = _TSysIO.Withdraw(accountId, 20m - fee, "test debit");

            Trace.WriteLine(string.Format("Adjust transactionID returned: {0}", TSysId));

            Assert.AreEqual(_TSysIO.GetBalance(accountId), balance);
        }
    }
}


