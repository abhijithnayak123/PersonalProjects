using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using NUnit.Framework;
using MGI.Core.Partner.Contract;
using AutoMapper;
using System.Reflection;
using NHibernate;
using Spring.Context;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Test
{
    [TestFixture]
    public class NPSTerminalFixture : AbstractPartnerTest
    {

        IManageNpsTerminal BIZPartnerNpsTerminalService { get; set; }
		//public IManageTerminal BIZPartnerNpsTerminalMappingService { private get; set; }
		public MGI.Core.Partner.Contract.IManageLocations CorePartnerLocationService { private get; set; }
		MGI.Biz.Partner.Contract.IChannelPartnerService ChannelPartnerService { get; set; }
		ISession session { get; set; }
		private static string INpsTerminals = "BIZPartnerNpsTerminalService";
        public MGIContext mgiContext { get; set; }
        long agentSessionId = 0;

        [SetUp]
        public void Setup()
        {
            Mapper.CreateMap<MGI.Biz.Partner.Data.Location, MGI.Core.Partner.Data.Location>();
            Mapper.CreateMap<MGI.Core.Partner.Data.Location, MGI.Biz.Partner.Data.Location>();
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			BIZPartnerNpsTerminalService = (IManageNpsTerminal)ctx.GetObject(INpsTerminals);
        }

        [Test]
        public void Can_Create_Terminal()
        {
			long locationId = 1000000001;
            
            MGI.Core.Partner.Data.Location coreLocation = CorePartnerLocationService.GetAll().FirstOrDefault(loc => loc.Id == locationId);            
            MGI.Biz.Partner.Data.Location bizLocation  = Mapper.Map<MGI.Biz.Partner.Data.Location>(coreLocation);
           
            NpsTerminal newNpsTerminal = new NpsTerminal()
            {
                Name = "Optmx1",
                Description = "EPSON printer/scanner located in DMS bay",
                IpAddress = "172.18.100.65",
                Port = "8732",
                Location = bizLocation,
                Status = "Not Available",				
				ChannelPartnerId = 33
            };

            bool isSuccess = BIZPartnerNpsTerminalService.Create(agentSessionId, newNpsTerminal, mgiContext);

            Assert.That(isSuccess, Is.True);

			//SetComplete();            
        }

    }
}
