using System;
using System.Diagnostics;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using System.Net;
using System.IO;
using MGI.Channel.DMS.Server.Data;
using System.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Collections;
using MGI.Common.Util;
//TODO Merge using System.Web.Http;

namespace MGI.Channel.DMS.Web.Controllers
{
    [Authorize(Roles = "Manager, SystemAdmin, Tech, Teller")]
	public class DiagnosticInfoController : BaseController
	{
		[CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerSearch")]
		public ActionResult DiagnosticInfo()
		{
			Session["activeButton"] = "diagnostics";
			Desktop desktop = new Desktop();
			var htSessions = (Hashtable)Session["HTSessions"];
			var agentSession = ((Server.Data.AgentSession)(htSessions["TempSessionAgent"]));

			NpsDiagnosticModel diagnosticInfo = new NpsDiagnosticModel();
			if (agentSession.Terminal != null)
			{
				Terminal terminal = agentSession.Terminal;
				
				diagnosticInfo = new NpsDiagnosticModel()
				{
					TerminalName = terminal.Name,
					StationId = terminal.PeripheralServer.Name,
					LocationId = terminal.PeripheralServer.Location.LocationName
				};
			}
			return View("DiagnosticInfo", diagnosticInfo);
		}
	}
}
