using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class TerminalFixture : AbstractPartnerTest
	{
		public ITerminalService PartnerTerminalService { private get; set; }
		public IManageLocations CorePartnerLocationService { private get; set; }
		public INpsTerminal PartnerNpsTerminalService { private get; set; }
		MGI.Common.Util.MGIContext mgiContext = new Common.Util.MGIContext() { };
		ChannelPartner channelpartner = new ChannelPartner()
		{
			rowguid = new Guid("6D7E785F-7BDD-42C8-BC49-44536A1885FC")
		};
		
		
		[Test]
		public void Can_Lookup_TerminalById()
		{
			long id = 1066;
			Terminal terminal = PartnerTerminalService.Lookup(id);

			//Assert.That(terminal, Is.Not.Null);
			Assert.That(terminal.Name, Is.EqualTo("OPT-LAP-0128"));
		}

		[Test]
		public void Can_Lookup_TerminalBy_Name()
		{
			channelpartner.rowguid = new Guid("6D7E785F-7BDD-42C8-BC49-44536A1885FC");
			string terminalName = "OPT-LAP-0128";
			//Terminal Lookup(string terminalName, ChannelPartner channelPartner, Dictionary<string, object> context);
			Terminal terminal = PartnerTerminalService.Lookup(terminalName, channelpartner, mgiContext);

			Assert.That(terminal, Is.Not.Null);
			Assert.That(terminal.Name, Is.EqualTo("OPT-LAP-0128"));
		}

		
		[Test]
		public void Can_Create_Terminal()
		{
			//long id = 1042;
			long locationId = 1000000003;
			long npsTerminalId = 10000024;
		//	Terminal terminal1 = PartnerTerminalService.Lookup(id);
			Terminal terminal = new Terminal() 
			{
				Name = "OPT-LAP-0039",
				MacAddress = "",
				Location = CorePartnerLocationService.GetAll().FirstOrDefault(loc => loc.Id == locationId),
				PeripheralServer = PartnerNpsTerminalService.Lookup(npsTerminalId),
				IpAddress = "172.18.100.63" ,
				ChannelPartner = channelpartner
			};

			Guid terminalId = PartnerTerminalService.Create(terminal);
			Terminal newTerminal = PartnerTerminalService.Lookup(terminalId);

			Assert.That(newTerminal, Is.Not.Null);
			Assert.That(newTerminal.Name, Is.EqualTo("OPT-LAP-0039"));
			Assert.That(newTerminal.IpAddress, Is.EqualTo("172.18.100.63"));

			//SetComplete();
		}

		[Test]
		public void Can_Update_Terminal()
		{
			long id = 1066;
			Terminal terminal = PartnerTerminalService.Lookup(id);
			terminal.IpAddress = "172.18.100.28";
			bool IsSuccess = PartnerTerminalService.Update(terminal);

			Assert.That(terminal, Is.Not.Null);
			Assert.That(terminal.IpAddress, Is.EqualTo("172.18.100.28"));

			SetComplete();
		}
	}
}
