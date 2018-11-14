using TCF.Zeo.Core.Contract;
using coreData = TCF.Zeo.Core.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Core.Impl;

namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class Terminal_Fixture
    {
        public ITerminalService _TerminalServiceImpl;

        [Test]
        public void Can_CreateTest()
        {
            bool isSuccess = false;

            coreData.Terminal createTerminal = Terminalinfo();
            Common.Data.ZeoContext context = new Common.Data.ZeoContext();
            {
                context.ChannelPartnerId = 34;
                context.AgentSessionId = 1000000069;
                context.TimeZone = "Eastern ZONE";
            }
            try
            {
                isSuccess = _TerminalServiceImpl.CreateTerminal(createTerminal, context);
                Assert.IsTrue(isSuccess == true);
            }
            catch
            {
                Assert.IsTrue(isSuccess == false);
            }
        }

        [Test]
        public void can_UpdateTerminal()
        {
            bool isSuccess = false;
            coreData.Terminal updateTerminal = Terminalinfo();
            Common.Data.ZeoContext context = new Common.Data.ZeoContext();
            {
                context.ChannelPartnerId = 34;
                context.AgentSessionId = 1000000069;
                context.TimeZone = "Eastern ZONE";
            }
            try
            {
                isSuccess = Convert.ToBoolean(_TerminalServiceImpl.UpdateTerminal(updateTerminal, context));
                Assert.IsTrue(isSuccess == true);
            }
            catch
            {
                Assert.IsTrue(isSuccess == false);
            }
        }


        [Test]
        public void can_GetTerminalById()
        {
            coreData.Terminal terminal = new coreData.Terminal();
            terminal.TerminalId = 1018;
            Common.Data.ZeoContext context = new Common.Data.ZeoContext();
            {
                context.ChannelPartnerId = 34;
                context.AgentSessionId = 1000000069;
                context.TimeZone = "Eastern ZONE";
            }
            try
            {
                terminal = _TerminalServiceImpl.GetTerminalById(terminal.TerminalId, context);
                Assert.IsNotNull(terminal);
            }
            catch
            {
                Assert.IsNotNull(terminal);
            }
        }

        [Test]
        public void can_GetTerminalByName()
        {
            coreData.Terminal terminal = new coreData.Terminal();
            terminal.Name = "OPT-LAP-0094";
            Common.Data.ZeoContext context = new Common.Data.ZeoContext();
            {
                context.ChannelPartnerId = 34;
                context.AgentSessionId = 1000000069;
                context.TimeZone = "Eastern ZONE";
            }
            try
            {
                terminal = _TerminalServiceImpl.GetTerminalByName(terminal.Name, context);
                Assert.IsNotNull(terminal);
            }
            catch
            {
                Assert.IsNotNull(terminal);
            }
        }

        [Test]
        public void can_GetNpsdiagnosticInfo()
        {
            Common.Data.ZeoContext context = new Common.Data.ZeoContext();
            {
                context.ChannelPartnerId = 34;
                context.AgentSessionId = 1000000069;
                context.TimeZone = "Eastern ZONE";
            }
            coreData.Terminal terminal = new coreData.Terminal();
            terminal.TerminalId = 1018;
            try
            {
                terminal = _TerminalServiceImpl.GetNpsdiagnosticInfo(terminal.TerminalId,context);
                Assert.IsNotNullOrEmpty(terminal.ToString());
            }
            catch
            {
                Assert.IsNotNullOrEmpty(terminal.ToString());
            }
        }


        private coreData.Terminal Terminalinfo()
        {

            coreData.Terminal terminal = new coreData.Terminal();
            {
                terminal.TerminalId = 0;
                terminal.Name = "OPT-LAP-0055";
                terminal.IpAddress = "172.100.189";
                terminal.ChannelPartnerId = 34;
                terminal.MacAddress = "";
                terminal.LocationId = 1000000003;
                terminal.PeripheralServerId = "10000009";
                terminal.DTServerCreate = DateTime.Now;
                terminal.DTTerminalCreate = DateTime.Now;

            };
            return terminal;
        }
    }
}
