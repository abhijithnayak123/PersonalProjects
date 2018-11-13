using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using System;
using System.Web;

namespace MGI.Channel.DMS.Web.Common
{
	public static class TerminalIdentifier
	{
        public static void IdentifyTerminal(long agentSessionId)
		{
			Desktop desktop = new Desktop();
			string channelPartnerName;
			bool isterminalNotSetup = true;
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			if (HttpContext.Current.Session["ChannelPartnerName"] == null)
				channelPartnerName = System.Configuration.ConfigurationManager.AppSettings.Get("ChannelPartner");
			else
				channelPartnerName = HttpContext.Current.Session["ChannelPartnerName"].ToString();

			ChannelPartner channelPartner = desktop.GetChannelPartner(channelPartnerName, mgiContext);
			string channelPartnerId = channelPartner.Id.ToString();

			if (channelPartner.TIM == (short)TerminalIdentificationMechanism.YubiKey)
			{
				isterminalNotSetup = isTerminalAvailable(agentSessionId, TerminalIdentificationMechanism.YubiKey, mgiContext);
			}
			else if (channelPartner.TIM == (short)TerminalIdentificationMechanism.Cookie)
			{
				isterminalNotSetup = isTerminalAvailable(agentSessionId, TerminalIdentificationMechanism.Cookie, mgiContext);
			}
			else if (channelPartner.TIM == (short)TerminalIdentificationMechanism.HostName)
			{
				if (HttpContext.Current.Session["HostName"] != null)
				{ isterminalNotSetup = isTerminalAvailable(agentSessionId, TerminalIdentificationMechanism.HostName, mgiContext, channelPartnerId); }
				else
				{ throw new Exception("MGiAlloy|0.0|PS terminal not setup correctly. Please contact the System Administrator.|PS needs to be setup by System Administrator before continuing with MGi Alloy operation."); }
			}

			if (isterminalNotSetup && channelPartner.TIM != (short)TerminalIdentificationMechanism.HostName)
				throw new Exception("MGiAlloy|0.0|MGi Alloy terminal not setup correctly.|Please setup terminal by clicking Hardware -> Terminal.");
		}

        public static bool IsTerminalAvailableForHostName(long agentSessionId)
		{
			string channelPartnerName = string.Empty;
			Desktop desktop = new Desktop();
			bool isterminalNotSetup = false;
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			if (HttpContext.Current.Session["ChannelPartnerName"] == null)
				channelPartnerName = System.Configuration.ConfigurationManager.AppSettings.Get("ChannelPartner");
			else
				channelPartnerName = HttpContext.Current.Session["ChannelPartnerName"].ToString();

			ChannelPartner channelPartner = desktop.GetChannelPartner(channelPartnerName, mgiContext);
			string channelPartnerId = channelPartner.Id.ToString();

			if (channelPartner.TIM == (short)TerminalIdentificationMechanism.HostName)
			{ isterminalNotSetup = isTerminalAvailable(agentSessionId, TerminalIdentificationMechanism.HostName, mgiContext, channelPartnerId); }

			return isterminalNotSetup;
		}

		private static bool isTerminalAvailable(long agentSessionId, TerminalIdentificationMechanism TIM, MGI.Channel.DMS.Server.Data.MGIContext mgiContext, string channelPartnerId = "")
		{
			bool isterminalNotSetup = true;
			switch (TIM)
			{
				case TerminalIdentificationMechanism.YubiKey:
					bool isYubiKeyEnabled = Convert.ToBoolean(HttpContext.Current.Session["YubiKeyValidationRequired"]);
					if (isYubiKeyEnabled && HttpContext.Current.Request.Cookies["Yubikeys"] != null)
					{
						isterminalNotSetup = false;
					}
					break;
				case TerminalIdentificationMechanism.Cookie:
					HttpCookie terminalCookie = HttpContext.Current.Request.Cookies["TerminalCookie"];
					if (terminalCookie != null)
					{
						isterminalNotSetup = false;
					}
					break;
				case TerminalIdentificationMechanism.HostName:
					string hostName = string.Empty;
					Desktop desktop = new Desktop();
					hostName = HttpContext.Current.Session["HostName"].ToString();
					Terminal hostTerminal = null;

					if (!string.IsNullOrWhiteSpace(hostName))
					{ hostTerminal = desktop.LookupTerminal(agentSessionId, hostName, Convert.ToInt32(channelPartnerId), mgiContext); }

					if (hostTerminal != null)
					{ isterminalNotSetup = false; }

					break;
				default:
					break;
			}
			return isterminalNotSetup;
		}
	}
}