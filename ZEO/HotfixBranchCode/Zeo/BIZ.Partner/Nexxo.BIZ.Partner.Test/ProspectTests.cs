using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using Spring.Testing.NUnit;
using Spring.Data.Core;
using Spring.Context;
using Spring.Context.Support;

using ProspectDTO = MGI.Biz.Partner.Data.Prospect;
using PTNRProspect = MGI.Core.Partner.Data.Prospect;
using IPtnrCustomerService = MGI.Core.Partner.Contract.ICustomerService;

using ICoreDataStructureService = MGI.Core.Partner.Contract.INexxoDataStructuresService;
using NexxoIdTypeDto = MGI.Core.Partner.Data.NexxoIdType;

using ICXECustomerService = MGI.Core.CXE.Contract.ICustomerService;
using MGI.Biz.Partner.Impl;

namespace MGI.Biz.Partner.Test
{
    [TestFixture]
    public class ProspectTests : AbstractPartnerTest
    {
        public CustomerProspectService BIZCustomerProspectService { get; set; }        

        [SetUp]
        public void setup()
        {

        }

        [Test]
        public void can_GetCustomerProspect()
        {
            Data.SessionContext context = new Data.SessionContext()
            { 
            };

			long alloyId = 1000000000000020;

			Data.Prospect prospect = BIZCustomerProspectService.GetProspect(0, context, alloyId, null);

            Assert.IsNotNull(prospect);
        }
    }
}
