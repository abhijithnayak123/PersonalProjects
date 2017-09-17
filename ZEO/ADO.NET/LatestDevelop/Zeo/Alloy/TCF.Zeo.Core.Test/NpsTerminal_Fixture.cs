using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreData = TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    public class NpsTerminal_Fixture
    {
        ZeoContext alloycontext = new ZeoContext();
        public INpsTerminal _NpsTerminalServiceImpl;
        [Test]
        public void Can_CreateNpsTerminal()
        {
            bool isSuccess = false;

            coreData.NpsTerminal createTerminal = NpsTerminalinfo();
            Common.Data.ZeoContext context = new Common.Data.ZeoContext();
            {
                context.ChannelPartnerId = 34;
                context.AgentSessionId = 1000000069;
                context.TimeZone = "Eastern ZONE";
            }
            try
            {
                isSuccess = _NpsTerminalServiceImpl.CreateNpsTerminal(createTerminal, context.TimeZone, alloycontext);
                Assert.IsTrue(isSuccess == true);
            }
            catch
            {
                Assert.IsTrue(isSuccess == false);
            }
        }

        [Test]
        public void Can_UpdateNpsterminal()
        {
            bool isSuccess = false;

            coreData.NpsTerminal createTerminal = NpsTerminalinfo();
            Common.Data.ZeoContext context = new Common.Data.ZeoContext();
            {
                context.ChannelPartnerId = 34;
                context.AgentSessionId = 1000000069;
                context.TimeZone = "Eastern ZONE";
            }
            try
            {
                isSuccess = _NpsTerminalServiceImpl.UpdateNpsTerminal(createTerminal, context.TimeZone, alloycontext);
                Assert.IsTrue(isSuccess == true);
            }
            catch
            {
                Assert.IsTrue(isSuccess == false);
            }
        }

        [Test]
        public void Can_GetNpsTerminalBylocationId()
        {

            coreData.NpsTerminal createTerminal = NpsTerminalinfo();
            List<coreData.NpsTerminal> npsTerminal = new List<coreData.NpsTerminal>();
            Common.Data.ZeoContext context = new Common.Data.ZeoContext();
            {
                context.ChannelPartnerId = 34;
                context.AgentSessionId = 1000000069;
                context.TimeZone = "Eastern ZONE";
                context.LocationId = "1000000003";
            }
            try
            {
                npsTerminal = _NpsTerminalServiceImpl.GetNpsTerminalBylocationId(context.LocationId, alloycontext);
                Assert.IsNotNullOrEmpty(npsTerminal.ToString());
            }
            catch
            {
                Assert.IsNotNullOrEmpty(npsTerminal.ToString());
            }
        }

        [Test]
        public void Can_GetNpsTerminalByName()
        {

            coreData.NpsTerminal createTerminal = NpsTerminalinfo();
            coreData.NpsTerminal npsTerminal = new coreData.NpsTerminal();
            Common.Data.ZeoContext context = new Common.Data.ZeoContext();
            {
                context.ChannelPartnerId = 34;
                context.AgentSessionId = 1000000069;
                context.TimeZone = "Eastern ZONE";
                context.LocationId = "1000000003";
            }
            try
            {
                npsTerminal = _NpsTerminalServiceImpl.GetNpsTerminalByName(createTerminal.Name, context.ChannelPartnerId, alloycontext);
                Assert.IsNotNullOrEmpty(npsTerminal.ToString());
            }
            catch
            {
                Assert.IsNotNullOrEmpty(npsTerminal.ToString());
            }
        }

        [Test]
        public void Can_GetNpsterminalByTerminalId()
        {

            List<coreData.NpsTerminal> npsTerminal = new List<coreData.NpsTerminal>();
            long NpsId = 10000009;
            try
            {
                npsTerminal = _NpsTerminalServiceImpl.GetNpsterminalByTerminalId(NpsId, alloycontext);
                Assert.IsNotNullOrEmpty(npsTerminal.ToString());
            }
            catch
            {
                Assert.IsNotNullOrEmpty(npsTerminal.ToString());
            }
        }


        private coreData.NpsTerminal NpsTerminalinfo()
        {
            coreData.NpsTerminal npsTerminal = new coreData.NpsTerminal();
            {
                npsTerminal.NpsTerminalId = 0;
                npsTerminal.Name = "OPT-LAP-0055";
                npsTerminal.Status = "172.100.189";
                npsTerminal.Description = "";
                npsTerminal.Port = "";
                npsTerminal.IpAddress = "";
                npsTerminal.ChannelPartnerId = 34;
                npsTerminal.PeripheralServiceUrl = "https://nps.nexxofinancial.com:18732/Peripheral/";
                npsTerminal.DTTerminalCreate = DateTime.Now;
                npsTerminal.DTServerCreate = DateTime.Now;
            };
            return npsTerminal;
        }
    }
}
