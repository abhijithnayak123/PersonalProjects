using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using MGI.Unit.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Test
{
	[TestFixture]
	public class ManageTerminalTest : BaseClass_Fixture
	{
		public MGI.Biz.Partner.Contract.IManageTerminal BIZPartnerTerminal { private get; set; }

		[Test]
		public void Lookup_Terminal()
		{
			MGIContext context = new MGIContext() { };

			Terminal terminal = BIZPartnerTerminal.Lookup(100000000, context);

			Assert.NotNull(terminal);
		}

		[Test]
		public void Lookup_Terminal_By_ChannelPartner_Withoutterminal()
		{
			MGIContext context = new MGIContext() { };

			Terminal terminal = BIZPartnerTerminal.Lookup(100000000, "Testing", 34, context);

			Assert.NotNull(terminal);
		}

		[Test]
		public void Lookup_Terminal_By_ChannelPartner_Withterminal()
		{
			MGIContext context = new MGIContext() { };

			Terminal terminal = BIZPartnerTerminal.Lookup(100000000, "Testing", 34, context);

			Assert.NotNull(terminal);
		}

		[Test]
		public void Create_Terminal()
		{
			MGIContext context = new MGIContext() { };
			Terminal terminal = new Terminal() { IpAddress = "172.100.13.16", ChannelPartner = new ChannelPartner() { Name = "TCF" } };

			bool isRes = BIZPartnerTerminal.Create(100000000, terminal, context);

			Assert.True(isRes);
		}

		[Test]
		public void Create_Terminal_WithTIM_HostName_Without_PeripheralServer()
		{
			MGIContext context = new MGIContext() { };
			Terminal terminal = new Terminal() { IpAddress = "172.100.13.16", ChannelPartner = new ChannelPartner() { Name = "TCF", TIM = 3 } };

			bool isRes = BIZPartnerTerminal.Create(100000000, terminal, context);

			Assert.True(isRes);
		}


		[Test]
		public void Create_Terminal_WithTIM_HostName_Without_PeripheralServer_Name()
		{
			MGIContext context = new MGIContext() { };
			Terminal terminal = new Terminal()
			{
				IpAddress = "172.100.13.16",
				ChannelPartner = new ChannelPartner() { Name = "TCF", TIM = 3 },
				Name = "test"
			};

			bool isRes = BIZPartnerTerminal.Create(100000000, terminal, context);

			Assert.True(isRes);
		}

		[Test]
		public void Create_Terminal_WithTIM_HostName_With_PeripheralServer()
		{
			MGIContext context = new MGIContext() { };
			Terminal terminal = new Terminal() { IpAddress = "172.100.13.16", ChannelPartner = new ChannelPartner() { Name = "TCF", TIM = 3 },
				PeripheralServer = new NpsTerminal()
			};

			bool isRes = BIZPartnerTerminal.Create(100000000, terminal, context);

			Assert.True(isRes);
		}

		[Test]
		public void Update_Terminal()
		{
			MGIContext context = new MGIContext() { };
			Terminal terminal = new Terminal() { IpAddress = "172.100.13.16", ChannelPartner = new ChannelPartner() { Name = "TCF" } };

			bool isRes = BIZPartnerTerminal.Update(100000000, terminal, context);

			Assert.True(isRes);
		}

		[Test]
		public void Update_Terminal_WithTIM_HostName_Without_PeripheralServer()
		{
			MGIContext context = new MGIContext() { };
			Terminal terminal = new Terminal() { IpAddress = "172.100.13.16", ChannelPartner = new ChannelPartner() { Name = "TCF", TIM = 3 } };

			bool isRes = BIZPartnerTerminal.Update(100000000, terminal, context);

			Assert.True(isRes);
		}


		[Test]
		public void Update_Terminal_WithTIM_HostName_With_PeripheralServer()
		{
			MGIContext context = new MGIContext() { };
			Terminal terminal = new Terminal()
			{
				IpAddress = "172.100.13.16",
				ChannelPartner = new ChannelPartner() { Name = "TCF", TIM = 3 },
				PeripheralServer = new NpsTerminal()
			};

			bool isRes = BIZPartnerTerminal.Update(100000000, terminal, context);

			Assert.True(isRes);
		}
	}
}
