using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.Models;
using System.Linq.Expressions;
using System.Reflection;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using System.Configuration;
using System.Collections;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class WesternUnionBannerController : BaseController
	{
		[CustomHandleErrorAttribute(ViewName = "AgentBannerMessage", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.WesternUnionBanner")]
		public ActionResult AgentBannerMessage()
		{
			Desktop desktop = new Desktop();
			WesternUnionBanner westernUnionBanner = new WesternUnionBanner();
			string bannerMessage = string.Empty;
			string errorMessage = "Unable to establish communication with Western Union. Please setup MGi Alloy Terminal Location.";
			System.Collections.Hashtable htSessions = (System.Collections.Hashtable)Session["HTSessions"];
			MGI.Channel.DMS.Server.Data.AgentSession agentSession = ((MGI.Channel.DMS.Server.Data.AgentSession)(htSessions["TempSessionAgent"]));
			if (agentSession.Terminal == null || string.IsNullOrEmpty(agentSession.Terminal.Name))
			{
				bannerMessage = errorMessage;
			}
			else
			{
				if (agentSession.Terminal != null && agentSession.Terminal.Location != null)
				{
					try
					{
						MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
						bannerMessage = desktop.WUGetAgentBannerMessage(Convert.ToInt64(agentSession.SessionId), mgiContext);
						if (bannerMessage == string.Empty)
						{
							bannerMessage = errorMessage;
						}
					}
					catch(Exception ex)
					{
						bannerMessage = errorMessage;
						throw ex;
					}
				}
				else
				{
					bannerMessage = errorMessage;
				}
			}
			if (Session["IsTerminalSetup"] == null)
			{
				Session["IsTerminalSetup"] = 0;
				//throw new Exception("DMS is not setup correctly and Transactions are currently disabled. Please contact the System Administrator to help solve the problem.");
			}
			TerminalIdentifier.IdentifyTerminal(long.Parse(agentSession.SessionId));
			TempData["IsChooseLocation"] = TerminalIdentifier.IsTerminalAvailableForHostName(long.Parse(agentSession.SessionId));
			westernUnionBanner.BannerMessage = bannerMessage;
			return View("AgentBannerMessage", "_Menu", westernUnionBanner);
		}

	}
}
