using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.Customer.CCIS.Data;
using MGI.Cxn.Customer.Contract;
using NUnit.Framework;
using Spring.Testing.NUnit;
using MGI.Cxn.Customer.CCIS.Impl;
using Moq;

namespace MGI.Cxn.Customer.CCIS.Test
{
    [TestFixture]
    public class CCISExceptionTest : AbstractTransactionalDbProviderSpringContextTests
    {
		public MGI.Common.Util.MGIContext MgiContext { get; set; }
        private Mock<IRepository<CCISConnect>> mocCCISConnectRepo = new Mock<IRepository<CCISConnect>>();
        ProcessorDAL _ProcessorDAL = new ProcessorDAL();

        #region Private Properties

        private Dictionary<string, object> customerLookUpCriteria;

        #endregion

        #region Setup
        /// <summary>
        /// Setup
        /// </summary>
        [TestFixtureSetUp]
        public void fixtSetup()
        {
            _ProcessorDAL.CCISConnectRepo = mocCCISConnectRepo.Object;
        }

        #endregion

        #region Spring Assembly

        protected override string[] ConfigLocations
        { get { return new string[] { "assembly://MGI.Cxn.Customer.CCIS.Test/MGI.Cxn.Customer.CCIS.Test/MGI.Cxn.Customer.CCIS.Test.Spring.xml" }; } }
        # endregion

        #region CCISTestMethodsImpl
        /// <summary>
        /// Test Throws CCIS_LOOKUP_ERROR
        /// </summary>
        [Test]
        public void CCISConnectCustomerLookUpThrows()
        {
            customerLookUpCriteria = new Dictionary<string, object>();
            mocCCISConnectRepo.Setup(m => m.FilterBy(It.IsAny<Expression<Func<CCISConnect, bool>>>())).Throws(new Exception());

			MinorCodeMatch<ClientCustomerException>(() => _ProcessorDAL.LookUp(customerLookUpCriteria, MgiContext), ClientCustomerException.CCIS_LOOKUP_ERROR);
        }
        #endregion

        /// <summary>
        /// Make sure the NexxoException minor code matches
        /// </summary>
        /// <typeparam name="T">NexxoException type</typeparam>
        /// <param name="code">Code that's being checked</param>
        /// <param name="minorCode">Minor code to match</param>
        private void MinorCodeMatch<T>(TestDelegate code, int minorCode) where T : MGI.Common.Sys.NexxoException
        {
            try
            {
                code();
                Assert.IsTrue(false);
            }
            catch (T ex)
            {
                Assert.IsTrue(ex.MinorCode == minorCode);
                Console.WriteLine(ex.Message);

            }
        }
    }
}
