using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using System.Collections;
//TODO Merge using System.Web.Http;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Channel.Zeo.Web.Common;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    [Authorize(Roles = "Manager, SystemAdmin, Tech, Teller")]
	public class DiagnosticInfoController : BaseController
	{
		[CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerSearch")]
		public ActionResult DiagnosticInfo()
		{
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response alloyResponse = new ZeoClient.Response();
            ZeoClient.ZeoContext context = GetZeoContext();
            Session["activeButton"] = "diagnostics";
			var htSessions = (Hashtable)Session["HTSessions"];
			var agentSession = ((ZeoClient.AgentSession)(htSessions["TempSessionAgent"]));

			NpsDiagnosticModel diagnosticInfo = new NpsDiagnosticModel();
			if (agentSession.TerminalId != 0)
			{
                alloyResponse = alloyServiceClient.GetNpsdiagnosticInfo(agentSession.TerminalId, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                ZeoClient.Terminal terminal = alloyResponse.Result as ZeoClient.Terminal;

                diagnosticInfo = new NpsDiagnosticModel()
                {

                    TerminalName = terminal.Name,
                    StationId = terminal.PeripheralName,
                    LocationId = terminal.LocationName,
                };
			}
			return View("DiagnosticInfo", diagnosticInfo);
		}
	}
}
