using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using NHibernate;
using NHibernate.Context;

using MGI.Cxn.MoneyTransfer.WU.Impl;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.Contract;
using Spring.Testing.NUnit;

namespace MGI.Cxn.MoneyTransfer.WU.Test
{
    [TestFixture]
    public class WUMoneyTransferSetupTest : AbstractTransactionalSpringContextTests
    {
        public IMoneyTransferSetup CXNMoneyTransferSetup { get; set; }

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Cxn.MoneyTransfer.WU.Test/MGI.Cxn.MoneyTransfer.WU.Test/CXNTestSpring.xml" }; }
        }

        [Test]
        public void GetCountryTest()
        {
            var country = CXNMoneyTransferSetup.GetCountries();
            Assert.IsTrue(country.Count > 0);
        }

        [Test]
        public void ShouldGetStateTest()
        {
            var state = CXNMoneyTransferSetup.GetStates("US");
            Assert.True(state.Count > 0);
        }

        [Test]
        public void ShouldNotGetStateTest()
        {
            var state = CXNMoneyTransferSetup.GetStates("AU");
            Assert.True(state.Count == 0);
        }

        [Test]
        public void ShouldGetCityTest()
        {
			var city = CXNMoneyTransferSetup.GetCities("QROO");
            Assert.IsTrue(city.Count > 0);
        }

        [Test]
        public void ShouldNotGetcityTest()
        {
            var city = CXNMoneyTransferSetup.GetCities("INM");
            Assert.IsTrue(city.Count == 0);
        }
    }
}

