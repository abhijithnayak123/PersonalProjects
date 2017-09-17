using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Server.Data;
using System.Collections;
using MGI.Channel.DMS.Web.Common;
using System.ServiceModel;
using MGI.Channel.DMS.Web.ServiceClient;
using System.Reflection;
using System.Linq.Expressions;
using System.Globalization;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class ReportController : BaseController
	{
		[CustomHandleErrorAttribute(ViewName = "ReportsLandingPage", MasterName = "_ReportLayout", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CashDrawerReport")]
		public ActionResult ShowCashDrawerReport(string localDate, string localTime)
		{
			//DateTime dt = DateTime.Now.ToUniversalTime();
			//var currentTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt, TimeZoneInfo.Utc.Id, TimeZoneInfo.Local.Id).ToString();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop desktop = new Desktop();
			AgentSession agentSession = (AgentSession)((Hashtable)Session["HTSessions"])["TempSessionAgent"];
			CashDrawerReport report = new CashDrawerReport();
			if (agentSession.Terminal != null)
			{
				var obj = desktop.CashDrawerReport(long.Parse(agentSession.SessionId), agentSession.Agent.Id, agentSession.Terminal.Location.Id, mgiContext);
				report.ReportDate = obj.ReportingDate.ToString("MM/dd/yyyy");
				report.ReportTime = DateTime.Now.ToString("hh:mm tt");
				report.TellerName = agentSession.Agent.Name;
				report.LocationName = agentSession.Terminal.Location.LocationName;
				report.CashIn = Math.Round(obj.CashIn, 2);
				report.CashOut = Math.Round(obj.CashOut, 2);
				report.SubTotal = Math.Round((obj.CashIn - obj.CashOut), 2);
				report.PrepareReportForPrint(obj.ReportTemplate);
				report.SubTotalToDisplay = Math.Abs(report.SubTotal);
			}
			else
			{
				throw new Exception("MGiAlloy|0.0|PS terminal not setup correctly. Please contact the System Administrator.|PS needs to be setup by System Administrator before continuing with MGi Alloy operation.");
			}
			return View("CashDrawerReport", report);
		}

		[CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerSearch")]
		public ActionResult DisplayLandingPage()
		{
			CashDrawerReport report = new CashDrawerReport();
			return View("ReportsLandingPage", report);
		}

		[CustomHandleErrorAttribute(ViewName = "ReportsLandingPage", MasterName = "_ReportLayout", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CashDrawerReport")]
		public ActionResult ShowCashManagementReports()
		{
			Session["activeButton"] = "cashreport";
			CashDrawerReport report = new CashDrawerReport();
			return View("CashManagementReports", report);
		}
	}
}
