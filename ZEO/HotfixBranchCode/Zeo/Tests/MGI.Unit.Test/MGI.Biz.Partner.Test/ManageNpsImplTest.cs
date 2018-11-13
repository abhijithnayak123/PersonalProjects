using NUnit.Framework;
using MGI.Unit.Test;
//using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using System.Collections.Generic;
//using MGI.Core.Partner.Contract;
//using MGI.Core.Partner.Data;
using System;
using MGI.Biz.Partner.Data;
//using MGI.Core.Partner.Data;


namespace MGI.Biz.Partner.Test
{
	[TestFixture]
	public class ManageNpsImplTest : BaseClass_Fixture
	{
		public MGI.Biz.Partner.Contract.IManageNpsTerminal BIZPartnerNpsTerminal { private get; set; }

		[Test]
		public void Create_NPS_Terminal()
		{
			NpsTerminal npsTerminal = new NpsTerminal() { Description = "test", ChannelPartnerId= 34, ChannelPartner = new ChannelPartner() { Name = "TCF", Id = 34 } };
			MGIContext context = new MGIContext() { };

			bool isRes = BIZPartnerNpsTerminal.Create(100000000, npsTerminal, context);

			Assert.True(isRes);
		}

		[Test]
		public void Update_NPS_Terminal()
		{
			NpsTerminal npsTerminal = new NpsTerminal() { Description = "test2", ChannelPartnerId = 34, ChannelPartner = new ChannelPartner() { Name = "TCF", Id = 34 } };
			MGIContext context = new MGIContext() { };

			bool isRes = BIZPartnerNpsTerminal.Update(100000000, npsTerminal, context);

			Assert.True(isRes);
		}

		[Test]
		public void Lookup_Id_NPS_Terminal()
		{
			MGIContext context = new MGIContext() { };
			NpsTerminal npsTerminal = BIZPartnerNpsTerminal.Lookup(1000000000, new Guid("11111111-1111-1111-1111-111111111222"), context);

			Assert.NotNull(npsTerminal);
			Assert.NotNull(npsTerminal.Description);
		}

		[Test]
		public void Lookup_Guid_NPS_Terminal()
		{
			MGIContext context = new MGIContext() { };
			NpsTerminal npsTerminal = BIZPartnerNpsTerminal.Lookup(1000000000, "172.18.1111.100", context);

			Assert.NotNull(npsTerminal);
			Assert.NotNull(npsTerminal.Description);
		}

		[Test]
		public void Lookup_IPAddress_NPS_Terminal()
		{
			MGIContext context = new MGIContext() { };
			NpsTerminal npsTerminal = BIZPartnerNpsTerminal.Lookup("1000000000", 1000000000, context);

			Assert.NotNull(npsTerminal);
			Assert.NotNull(npsTerminal.Description);
		}

		[Test]
		public void Lookup_ChannelPartner_NPS_Terminal()
		{
			MGIContext context = new MGIContext() { };
			NpsTerminal npsTerminal = BIZPartnerNpsTerminal.Lookup(1000000000, "test1", new ChannelPartner(), context);

			Assert.NotNull(npsTerminal);
			Assert.NotNull(npsTerminal.Description);
		}

		[Test]
		public void Get_Location_By_ID()
		{
			MGIContext context = new MGIContext() { };
			List<NpsTerminal> npsTerminals = BIZPartnerNpsTerminal.GetByLocationID(1000000000, 10000000, context);

			Assert.NotNull(npsTerminals);
			Assert.True(npsTerminals.Count > 0);
		}
	}
}
