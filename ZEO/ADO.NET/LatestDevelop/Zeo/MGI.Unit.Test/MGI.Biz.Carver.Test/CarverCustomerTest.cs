using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Unit.Test.MockData;
using AutoMapper;
using MGI.Biz.Customer.Contract;
using MGI.Biz.Customer.Data;
using MGI.Cxn.Common.Processor.Contract;
using MGI.Cxn.Customer.Contract;
using MGI.Common.Util;
using System.Diagnostics;
using CxnCustomerData = MGI.Cxn.Customer.Data;
using CCISConnectData = MGI.Cxn.Customer.CCIS.Data;
using BizCustomerProfile = MGI.Biz.Customer.Data;
using MGI.Biz.Carver.Impl;


namespace MGI.Biz.Carver.Test
{
    [TestFixture]
    class CarverCustomerTest
    {
        MGI.Biz.Carver.Impl.CarverCustomer impl;

        private void Setup()
        {
            impl = new CarverCustomer();

            impl.CxnClientCustomerService = new MockClientCustomerServiceCarver();

        }

        [Test]
        public void FetchAllTest()
        {
            Setup();
            
            var cxncontext = new Dictionary<string, object>();
            cxncontext.Add("ChannelPartnerId", 28);
            
            var customerLookUpcriteria = new Dictionary<string, object>();
            
            var cust = impl.FetchAll(12, customerLookUpcriteria, cxncontext);
           
            Assert.NotNull(cust);
            Assert.Greater(cust.Count, 0);
        }
    }
}
