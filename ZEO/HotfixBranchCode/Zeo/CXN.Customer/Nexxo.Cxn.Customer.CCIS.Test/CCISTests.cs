using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Spring.Testing.NUnit;
using MGI.Cxn.Customer.CCIS.Impl;
using MGI.Cxn.Customer.Contract;
using CCISConnectData = MGI.Cxn.Customer.CCIS.Data.CCISConnect;
using CxnCustomerData = MGI.Cxn.Customer.Data.CustomerProfile;

namespace MGI.Cxn.Customer.CCIS.Test
{
    [TestFixture]
    public class CCISTests : AbstractTransactionalSpringContextTests
    {
		public MGI.Common.Util.MGIContext MgiContext { get; set; }
        public IClientCustomerService CCISGateway { private get; set; }

        #region Private Properties

        private Dictionary<string, object> customerLookUpCriteria;

        #endregion

        #region Spring Assembly

        protected override string[] ConfigLocations
        { get { return new string[] { "assembly://MGI.Cxn.Customer.CCIS.Test/MGI.Cxn.Customer.CCIS.Test/MGI.Cxn.Customer.CCIS.Test.Spring.xml" }; } }
        # endregion

        #region CCISTestMethodsImpl
        /// <summary>
        /// This test method for CCISConnectDB US 1983
        /// </summary>
        [Test]
        public void TestCCISConnectCustomerLookUp()
        {
            List<CxnCustomerData> objcCISConnects = new List<CxnCustomerData>();
            customerLookUpCriteria = new Dictionary<string, object>();
            customerLookUpCriteria.Add("DateofBirth", "10/10/1950 12:00:00 AM");
            customerLookUpCriteria.Add("PhoneNumber", "2125551212");
            customerLookUpCriteria.Add("ZipCode", "00212");
            customerLookUpCriteria.Add("LastName", "Sample");
			objcCISConnects = CCISGateway.FetchAll(customerLookUpCriteria, MgiContext);
            Assert.IsNotNull(objcCISConnects);
            foreach (var objcCISConnect in objcCISConnects)
            {
                Assert.AreEqual(objcCISConnect.Phone1, "2125551212");
            }

            if (objcCISConnects.Count > 0)
            {
                Console.WriteLine(" {0} CCISCustomers Found in the CCISConnectDB", objcCISConnects.Count);
            }
            else
            {
                Console.WriteLine(" No CCISCustomer Found in the CCISConnectDB");
            }

        }
        #endregion

    }
}
