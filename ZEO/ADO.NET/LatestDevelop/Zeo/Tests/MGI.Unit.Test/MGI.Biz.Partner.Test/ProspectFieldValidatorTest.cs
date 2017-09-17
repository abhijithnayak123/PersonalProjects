using System.Collections.Generic;
using NUnit.Framework;
using MGI.Biz.Partner.Impl;
using System;
using MGI.Common.Util;
using MGI.Biz.Partner.Contract;


namespace MGI.Biz.Customer.Test
{
    [TestFixture]
    public class ProspectFieldValidatorTest
    {
        ProspectFieldValidator impl;

        private void Setup()
        {
            impl = new ProspectFieldValidator();

        }

        [Test]
        public void ValidateSSNTest()
        {
            Setup();

            var prospect = new Partner.Data.Prospect()
            {
                SSN = "457555462",
            };

            impl.ValidateSSN(prospect);
        }

        [Test]
        public void ValidateDOBTest()
        {
            Setup();

            var prospect = new Partner.Data.Prospect()
            {
                DateOfBirth = new DateTime(1950, 10, 10),
            };

            impl.ValidateDOB(prospect);
        }

        [Test]
        [ExpectedException(typeof(BizCustomerException))]
        public void ValidateAddressTest()
        {
            Setup();

            var prospect = new Partner.Data.Prospect()
            {
                PostalCode ="123"
            };

            impl.ValidateAddress(prospect);
        }

        [Test]
        [ExpectedException(typeof(BizCustomerException))]
        public void ValidateEmailTest()
        {
            Setup();

            var prospect = new Partner.Data.Prospect()
            {
                Email = "abcd"
            };

            impl.ValidateEmail(prospect);
           
        }

        [Test]
        public void ValidatePhoneTest()
        {
            Setup();

            var prospect = new Partner.Data.Prospect()
            {
                Phone1 = "7812241019"
            };

            impl.ValidatePhone(prospect);
        }

    
    }
}