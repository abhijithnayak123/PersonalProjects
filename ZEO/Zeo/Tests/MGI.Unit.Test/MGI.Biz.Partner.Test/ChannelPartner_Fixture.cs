using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MGI.Unit.Test.MGI.Biz.Partner.Test
{
	[TestFixture]
	public class ChannelPartner_Fixture : BaseClass_Fixture
	{
		public IChannelPartnerService BIZPartnerChannelPartnerService { get; set; }

		[Test]
		public void Can_Get_Locations()
		{
			long agentSessionId = 1000000000;
			string channelPartner = "TCF";
			MGIContext mgiContext = new MGIContext() { };

			List<string> locations = BIZPartnerChannelPartnerService.Locations(agentSessionId, channelPartner, mgiContext);

			Assert.AreNotEqual(locations.Count, 0);
		}

		[Test]
		public void Can_Get_ChannelPartner_Config_BY_ChannelPartnerName()
		{
			string channelPartnerName = "TCF";
			MGIContext mgiContext = new MGIContext() { };

			ChannelPartner channelPartner = BIZPartnerChannelPartnerService.ChannelPartnerConfig(channelPartnerName, mgiContext);

			Assert.IsNotNull(channelPartner);
		}

		[Test]
		public void Can_Get_ChannelPartner_Config_BY_ChannelPartnerId()
		{
			int channelPartnerId = 34;
			MGIContext mgiContext = new MGIContext() { };

			ChannelPartner channelPartner = BIZPartnerChannelPartnerService.ChannelPartnerConfig(channelPartnerId, mgiContext);

			Assert.IsNotNull(channelPartner);
		}

		[Test]
		public void Can_Get_ChannelPartner_Config()
		{
			Guid rowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			MGIContext mgiContext = new MGIContext() { };

			ChannelPartner channelPartner = BIZPartnerChannelPartnerService.ChannelPartnerConfig(rowGuid, mgiContext);

			Assert.IsNotNull(channelPartner);
		}

		[Test]
		public void Can_Get_All_TipsAndOffers()
		{
			long agentSessionId = 1000000000;
			string channelPartner = "TCF";
			MGIContext mgiContext = new MGIContext() { };
			string language = "EN";
			string viewName = "";

			List<TipsAndOffers> tipsAndOffers = BIZPartnerChannelPartnerService.GetTipsAndOffers(agentSessionId, channelPartner, language, viewName, mgiContext);

			Assert.AreNotEqual(tipsAndOffers.Count, 0);
		}

		[Test]
		public void Can_Get_All_Group()
		{
			string channelPartner = "TCF";
			MGIContext mgiContext = new MGIContext() { };

			List<string> groups = BIZPartnerChannelPartnerService.GetGroups(channelPartner, mgiContext);

			Assert.AreNotEqual(groups.Count, 0);
		}

		[Test]
		public void Can_Get_ChannelPartner_Certificate_Info()
		{
			long channelPartnerId = 34;
			string issuer = "";
			MGIContext mgiContext = new MGIContext() { };

			ChannelPartnerCertificate channelPartnerCertificate = BIZPartnerChannelPartnerService.GetChannelPartnerCertificateInfo(channelPartnerId, issuer, mgiContext);

			Assert.IsNotNull(channelPartnerCertificate);
		}
	}
}
