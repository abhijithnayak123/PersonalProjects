using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class DatastructureService_Fixture
    {
        [Test]
        public void TestPhoneTypes()
        {
            ZeoContext alloycontext = new ZeoContext();

            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            List<string> phoneTypes = dataService.PhoneTypes(alloycontext);

            Assert.IsTrue(phoneTypes.Count > 0);
        }

        [Test]
        public void TestMobileProviders()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            List<string> providers = dataService.MobileProviders(alloycontext);

            Assert.IsTrue(providers.Count > 0);
        }

        [Test]
        public void TestMasterCountries()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            long channelPartnerId = 34;
            List<MasterCountry> masterCountries = dataService.MasterCountries(channelPartnerId, alloycontext);

            Assert.IsTrue(masterCountries.Count > 0);
        }


        [Test]
        public void TestStates()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            string country = "840";
            List<string> states = dataService.States(country, alloycontext);

            Assert.IsTrue(states.Count > 0);
        }

        [Test]
        public void TestUSStates()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            List<string> usStates = dataService.USStates(alloycontext);

            Assert.IsTrue(usStates.Count > 0);
        }

        [Test]
        public void TestIdCountries()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            long channelPartnerId = 34;
            List<string> idCountries = dataService.IdCountries(channelPartnerId, alloycontext);

            Assert.IsTrue(idCountries.Count > 0);
        }

        [Test]
        public void TestIdStates()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            long channelPartnerId = 34;
            string country = "UNITED STATES";
            string idType = "DRIVER'S LICENSE";
            List<string> idStates = dataService.IdStates(channelPartnerId, country, idType, alloycontext);

            Assert.IsTrue(idStates.Count > 0);
        }

        [Test]
        public void TestIdTypes()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            long channelPartnerId = 34;
            string country = "UNITED STATES";
            List<string> idTypes = dataService.IdTypes(channelPartnerId, country, alloycontext);

            Assert.IsTrue(idTypes.Count > 0);
        }

        [Test]
        public void TestLegalCodes()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            List<LegalCode> legalCodes = dataService.GetLegalCodes(alloycontext);

            Assert.IsTrue(legalCodes.Count > 0);
        }

        [Test]
        public void TestOccupations()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            List<Occupation> occupations = dataService.GetOccupations(alloycontext);

            Assert.IsTrue(occupations.Count > 0);
        }


        [Test]
        public void TestCountries()
        {
            ZeoContext alloycontext = new ZeoContext();
            DataStructureServiceImpl dataService = new DataStructureServiceImpl();

            long channelPartnerId = 34;
            List<string> countries = dataService.Countries(channelPartnerId, alloycontext);

            Assert.IsTrue(countries.Count > 0);
        }
    }
}
