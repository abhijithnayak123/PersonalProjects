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
	public class TerminalServiceImpl_Fixture : AbstractPartnerTest
	{
		public INpsTerminal PartnerNpsTerminalService { private get; set; }
		public IManageLocations CorePartnerLocationService { private get; set; }
		public ITerminalService PartnerTerminalService { private get; set; }

		[Test]
		public void Can_Lookup_NpsTerminal()
		{
			long Id = 10000196;

			NpsTerminal npsTerminal = PartnerNpsTerminalService.Lookup(Id);

			Assert.That(npsTerminal, Is.Not.Null);
			Assert.That(npsTerminal.Name, Is.EqualTo("DMS QA Bay"));
			Assert.That(npsTerminal.Port, Is.EqualTo("8732"));

		}

		[Test]
		public void Can_Create_NpsTerminal()
		{
			long locationId = 2;

			long Id = 10000024;

			NpsTerminal Terminal = PartnerNpsTerminalService.Lookup(Id);

			ChannelPartner Channel = new ChannelPartner()
			{
				rowguid = Terminal.ChannelPartner.rowguid
			};
			NpsTerminal newNpsTerminal = new NpsTerminal()
			{
				Name = "DMS Bay",
				Description = "EPSON printer/scanner located in DMS bay",
				IpAddress = "172.18.100.63",
				Port = "8732",
				Location = CorePartnerLocationService.GetAll().FirstOrDefault(loc=> loc.Id == locationId),
				Status = "Not Available",
				ChannelPartner = Channel
				
			};

			bool isSuccess = PartnerNpsTerminalService.Create(newNpsTerminal);

			Assert.That(isSuccess, Is.True);

			NpsTerminal npsTerminal = PartnerNpsTerminalService.Lookup(newNpsTerminal.Id);

			Assert.That(npsTerminal, Is.Not.Null);
			Assert.That(npsTerminal.Name, Is.EqualTo("DMS Bay"));
			Assert.That(npsTerminal.IpAddress, Is.EqualTo("172.18.100.63"));

			//SetComplete();

		}

		[Test]
		public void Can_Update_NpsTerminal()
		{
			var ipAddress = "172.18.100.63";
			NpsTerminal npsTerminal = PartnerNpsTerminalService.Lookup(ipAddress);

			npsTerminal.Name = "DMS QA Bay";
			bool isSuccess = PartnerNpsTerminalService.Update(npsTerminal);

			Assert.That(isSuccess, Is.True);

			npsTerminal = PartnerNpsTerminalService.Lookup(npsTerminal.Id);

			Assert.That(npsTerminal, Is.Not.Null);
			Assert.That(npsTerminal.Name, Is.EqualTo("DMS QA Bay"));

			//SetComplete();
		}

		

		[Test]
		public void Can_Get_NpsTerminals_ByLocationID()
		{
			long locationId = 1000000003;

			List<NpsTerminal> npsTerminals = PartnerNpsTerminalService.GetByLocationID(locationId);

			Assert.That(npsTerminals, Is.Not.Null);
			Assert.That(npsTerminals, Is.Not.Empty);
		}
	}
}
