using System;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using System.Collections;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Channel.Zeo.Web.Common;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
	public class ReportController : BaseController
	{
		[CustomHandleErrorAttribute(ViewName = "ReportsLandingPage", MasterName = "_ReportLayout", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CashDrawerReport")]
		public ActionResult ShowCashDrawerReport(string localDate, string localTime)
		{
            try
            {

                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.AgentSession agentSession = (ZeoClient.AgentSession)((Hashtable)Session["HTSessions"])["TempSessionAgent"];
			   CashDrawerReport report = new CashDrawerReport();
			if (agentSession.TerminalName != null)
			{
                    response = serviceClient.GetCashDrawerReceipt(agentSession.AgentID, Convert.ToInt64(context.LocationId),context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    ZeoClient.CashDrawerReceipt receipt = response.Result as ZeoClient.CashDrawerReceipt;

                    report.ReportDate = receipt.ReportingDate.ToString("MM/dd/yyyy");
                    report.ReportTime = DateTime.Now.ToString("hh:mm tt");
                    report.TellerName = agentSession.Name;
                    report.LocationName = agentSession.LocationName;
                    report.CashIn = Math.Round(receipt.CashIn, 2);
                    report.CashOut = Math.Round(receipt.CashOut, 2);
                    report.SubTotal = Math.Round((receipt.CashIn - receipt.CashOut), 2);
                    report.PrepareReportForPrint(receipt.ReportTemplate);
                    report.SubTotalToDisplay = Math.Abs(report.SubTotal);
                }
			else
			{
				throw new Exception("Zeo|0.0|PS terminal not setup correctly. Please contact the System Administrator.|PS needs to be setup by System Administrator before continuing with Zeo operation.");
			}
			return View("CashDrawerReport", report);
            }
            catch(Exception ex)
            {
                VerifyException(ex); return null;
            }
		}

		[CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerSearch")]
		public ActionResult DisplayLandingPage()
		{
			CashDrawerReport report = new CashDrawerReport();
			return View("ReportsLandingPage", report);
		}

		[CustomHandleErrorAttribute(ViewName = "ReportsLandingPage", MasterName = "_ReportLayout", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CashDrawerReport")]
		public ActionResult ShowCashManagementReports()
		{
			Session["activeButton"] = "cashreport";
			CashDrawerReport report = new CashDrawerReport();
			return View("CashManagementReports", report);
		}
	}
}
