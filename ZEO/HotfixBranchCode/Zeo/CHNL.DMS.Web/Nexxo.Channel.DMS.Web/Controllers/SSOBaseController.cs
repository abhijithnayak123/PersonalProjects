using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Web.SSO;
using MGI.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MGI.Channel.DMS.Web.Controllers
{
	[SkipNoDirectAccess]
	public class SSOBaseController : Controller
	{
        public static BaseSSO GetSSO(string channelPartnerName)
		{
            //Todo: If channel Partner name is empty, throw exception.

            switch (channelPartnerName)
			{
                case "TCF":
                    return new TCF();
                default:
                    return new Okta();
			}
		}

		protected override void OnException(ExceptionContext exceptionContext)
		{
			if (exceptionContext.Exception != null)
			{
				NLogHelper.Error(exceptionContext);
			}
		}		

        protected string getTerminalName(Desktop desktop, ChannelPartner channelPartner, string hostName)
        {
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            string terminalName = string.Empty;
            long agentSessionId = GetAgentSessionId();
            // Yubikey authentication not applicable to this mode of entry
            if (channelPartner.TIM == (short)TerminalIdentificationMechanism.Cookie)
            {
                HttpCookie terminalCookie = Request.Cookies["TerminalCookie"];
                if (terminalCookie != null)
                    terminalName = terminalCookie.Values["TerminalIdentifier"].ToString();
            }
            else if (channelPartner.TIM == (short)TerminalIdentificationMechanism.HostName)
            {
                if (!string.IsNullOrEmpty(hostName) && (desktop.LookupTerminal(agentSessionId, hostName, Convert.ToInt32(channelPartner.Id), mgiContext) != null))
                    terminalName = hostName;
            }

            return terminalName;
        }

        protected int getNexxoUserRole(List<string> roles)
        {
            foreach (var role in roles)
            {
                NLogHelper.Info("role:" + role);

				if (role.ToLower().Contains("app_nexxo_teller"))
                {
                    NLogHelper.Info("found teller role");
                    return 1;
                }
				else if (role.ToLower().Contains("app_nexxo_compliancemgr"))
                {
                    NLogHelper.Info("found compliance manager role");
                    return 3;
                }
				else if (role.ToLower().Contains("app_nexxo_manager"))
                {
                    NLogHelper.Info("found manager role");
                    return 2;
                }
				else if (role.ToLower().Contains("app_nexxo_sysadmin"))
                {
                    NLogHelper.Info("found sysadmin role");
                    return 4;
                }
            }
            NLogHelper.Info("No role found. taken default as teller.");
            // if no matching roles found, return Teller role
            return 1;
        }

        protected long GetAgentSessionId()
        {
            long agentSessionId = 0L;
            if (Session["HTSessions"] != null)
            {
                agentSessionId = long.Parse(((Hashtable)Session["HTSessions"])["AgentSessionId"].ToString());
            }
            return agentSessionId;
        }
	}	
}
