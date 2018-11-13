using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using MGI.Channel.DMS.Web.ServiceClient;
using System.Globalization;
using MGI.Channel.DMS.Web.Models;
using MGI.Common.Util;
using System.Diagnostics;
using System.Configuration;
using MGI.Channel.DMS.Web.Common;
using System.Collections.Specialized;
using System.Collections;
using MGI.Channel.Shared.Server.Data;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
	[Authorize(Roles = "Teller, Manager, ComplianceManager, SystemAdmin, Tech")]
	public class BaseController : Controller
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ViewName"></param>
		/// <param name="masterName"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		protected override ViewResult View(string ViewName, string masterName, object model)
		{
			return PrepareView(ViewName, masterName, model);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ViewName"></param>
		/// <param name="masterName"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		private ViewResult PrepareView(string ViewName, string masterName, object model)
		{
			ViewResult renderview = null;
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			if (!(ViewName.ToLower().Contains("login")) && (!(ViewName.ToLower().Contains("manageusers"))) && (!(ViewName.ToLower().Contains("location"))) && (!(ViewName.ToLower().Contains("newuser"))))
			{
				string optionalFilter = null;

				if (ViewName.ToLower() == "prepaidcard")
				{
					bool GPRCardExists = (bool)model.GetType().GetProperty("GPRCardExists").GetValue(model, null);
					if (!GPRCardExists)
						optionalFilter = "Activation";
				}
				else if (ViewName.ToLower() == "transactionsummary")
				{
					string SummaryTitle = (string)model.GetType().GetProperty("SummaryTitle").GetValue(model, null);
					if (SummaryTitle == "Prepaid Card")
						optionalFilter = "Prepaid Card";
				}

				Desktop deskTop = new Desktop();
				ViewBag.TipsAndOffersMessage = deskTop.GetTipsAndOffersForChannelPartner(GetAgentSessionId(), ViewName, CultureInfo.CurrentUICulture.ToString(), Session["ChannelPartnerName"].ToString().Trim(), optionalFilter, mgiContext);
				PopulateCheckProcessorInfo(mgiContext);
			}


			if (!(ViewName.ToLower().Contains("login") || ViewName.ToLower().Contains("landing")))
			{
				if (Session != null && Session["ChannelPartnerName"] != null)
				{
					if (System.IO.File.Exists(Server.MapPath(VirtualPathUtility.ToAbsolute("~/Views/" + Session["ChannelPartnerName"].ToString().Trim() + "/" + ViewName + ".cshtml"))))
					{
						renderview = base.View("~/Views/" + Session["ChannelPartnerName"].ToString().Trim() + "/" + ViewName + ".cshtml", masterName, model);
					}
					else if (System.IO.File.Exists(Server.MapPath(VirtualPathUtility.ToAbsolute("~/Views/Shared" + "/" + ViewName + ".cshtml"))))
					{
						renderview = base.View("~/Views/Shared/" + ViewName + ".cshtml", masterName, model);
					}
					else
					{
						renderview = base.View("~/Views/Nexxo/" + ViewName + ".cshtml", masterName, model);
					}
				}
				else
				{
					renderview = base.View("~/Views/Nexxo/" + ViewName + ".cshtml", masterName, model);
				}

				if (renderview != null)
					return renderview;
			}

			return base.View(ViewName, masterName, model);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ViewName"></param>
		/// <returns></returns>
		protected new ViewResult View(string ViewName)
		{
			return View(ViewName, null, null);
		}

		/// <summary>
		/// Something about what the <c>MySomeFunction</c> does
		/// with some of the sample like
		/// <code>
		/// Some more code statement to comment it better
		/// </code>
		/// For more information seee <see cref="http://www.me.com"/>
		/// </summary>
		/// <param name="someObj">What the input to the function is</param>
		/// <returns>What it returns</returns>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			//To prevent book marking and copy paste url
			//if (filterContext.HttpContext.Request.UrlReferrer == null && filterContext.HttpContext.Session["SSOLogin"] == null)
			//{
			//	filterContext.Result = RedirectToAction("Create", "Login");
			//}
			/*
				Need to think about a better solution for this.
			*/
			bool islandingpage = filterContext.HttpContext.Request.RawUrl.Trim('/').Contains("LandingPage") ? true : false;
			bool isPersonalInformation = filterContext.HttpContext.Request.RawUrl.Trim('/').Contains("PersonalInformation") ? true : false;
			bool isYubiKeyValidtionRequired = Convert.ToBoolean(filterContext.HttpContext.Session["YubiKeyValidationRequired"]);
			bool IsPersonalInfoRequiredToProceed = PersonalInfoRequiredToProceed(filterContext.HttpContext.Request.RawUrl.Trim('/'));

			Session["CurrentPage"] = filterContext.HttpContext.Request.RawUrl.Trim('/').Contains("LandingPage") ? "LandingPage" : null;

			/* 
			 * Below code handles both if user is not logged in or user does not provided yubikey and 
			 * when user provided yubikey but not logged in. No changes required for this code block 
			 * from here on.
			*/
			if ((filterContext.HttpContext.Session["LoginName"] == null && isYubiKeyValidtionRequired) || Session["HTSessions"] == null)
			{
				if (filterContext.HttpContext.Request.QueryString["channelpartnername"] != null)
				{
					string channelPartnerName = filterContext.HttpContext.Request.QueryString["channelpartnername"].ToString();
					filterContext.Result = RedirectToAction("Create", "Login", new { ChannelPartner = channelPartnerName });
				}
				else
				{
					filterContext.Result = RedirectToAction("Create", "Login");
				}

				return;
			}
			/*
			 * This should handle if the user is logged in, but not have created a customer profile 
			 * and trying to access other sucessive pages
			 */
			else if (filterContext.HttpContext.Session["CustomerProspect"] == null && Session["CurrentPage"] == null && !islandingpage && !isPersonalInformation && IsPersonalInfoRequiredToProceed)
			{ filterContext.Result = RedirectToAction("PersonalInformation", "CustomerRegistration", new { newCustomer = true }); return; }
			else if (filterContext.HttpContext.Session["CustomerProspect"] == null && Session["CurrentPage"] != null && (!islandingpage))
			{ filterContext.Result = RedirectToAction("LandingPage", "LandingPage"); return; }



			base.OnActionExecuting(filterContext);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pageName"></param>
		/// <returns></returns>
		private bool PersonalInfoRequiredToProceed(string pageName)
		{
			if ((pageName.Contains("IdentificationInformation") || pageName.Contains("EmploymentDetails") ||
					 pageName.Contains("PinDetails") || pageName.Contains("ProfileSummary")) && pageName.StartsWith("CustomerSearch") == false)
				return true;
			else
				return false;
		}

		private T GetInstance<T>(string type)
		{
			return (T)Activator.CreateInstance(Type.GetType(type));
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			if (filterContext.Exception != null)
			{
				NLogHelper.Error(filterContext);
			}
			if (!filterContext.HttpContext.Request.IsAjaxRequest())
			{
				ViewBag.IsException = true;
				ViewBag.ExceptionMsg = System.Web.HttpUtility.JavaScriptStringEncode(filterContext.Exception.Message);

				filterContext.ExceptionHandled = true;

				if (filterContext.Controller.ViewBag.ViewName != null && filterContext.Controller.ViewBag.ModelType == null && filterContext.Controller.ViewBag.ResultType == null)
				{
					filterContext.Result = PrepareView(filterContext.Controller.ViewBag.ViewName, filterContext.Controller.ViewBag.MasterName, filterContext.Controller.ViewBag.ParamValue);
				}
				else if (filterContext.Controller.ViewBag.ViewName != null && filterContext.Controller.ViewBag.ResultType == "prepare")
				{
					if (filterContext.Controller.ViewBag.ModelType != null)
					{
						var model = GetInstance<object>(filterContext.Controller.ViewBag.ModelType);

						filterContext.Result = PrepareView(filterContext.Controller.ViewBag.ViewName, filterContext.Controller.ViewBag.MasterName, model);
					}
					else
						filterContext.Result = PrepareView(filterContext.Controller.ViewBag.ViewName, filterContext.Controller.ViewBag.MasterName, null);
				}
				else if (filterContext.Controller.ViewBag.ResultType == "redirect")
				{
					if (filterContext.Controller.ViewBag.ActionName != null)
						filterContext.Result = RedirectToAction((string)filterContext.Controller.ViewBag.ActionName, (string)filterContext.Controller.ViewBag.ControllerName, new { IsException = ViewBag.IsException, ExceptionMsg = ViewBag.ExceptionMsg });
				}
			}
			else
			{
				filterContext.ExceptionHandled = true;
				filterContext.Result = new JsonResult
				{
					Data = new { success = false, data = System.Web.HttpUtility.JavaScriptStringEncode(filterContext.Exception.Message) },
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};
			}

		}

		protected bool IsPeripheralServerSetUp(Server.Data.Terminal terminal)
		{
			bool hasSetup = false;
			if (terminal != null && terminal.PeripheralServer != null)
			{
				if (!string.IsNullOrWhiteSpace(terminal.PeripheralServer.PeripheralServiceUrl))
					hasSetup = true;
			}
			return hasSetup;
		}


		private void PopulateCheckProcessorInfo(MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			if (Session["HTSessions"] != null)
			{
				System.Collections.Hashtable htSessions = (System.Collections.Hashtable)Session["HTSessions"];
				MGI.Channel.DMS.Server.Data.AgentSession agentSession = ((MGI.Channel.DMS.Server.Data.AgentSession)(htSessions["TempSessionAgent"]));
				Desktop desktop = new Desktop();
				MGI.Channel.DMS.Server.Data.CheckProcessorInfo info = desktop.GetCheckProcessorInfo(agentSession.SessionId, mgiContext);
				if (info != null)
				{
					ViewBag.CheckProcessorInfo = info;
				}
			}
		}

		protected string GetChannelPartnerName()
		{
			string channelPartnerName = string.Empty;
			if (Session["ChannelPartnerName"] != null)
			{
				channelPartnerName = Session["ChannelPartnerName"].ToString();
			}

			return channelPartnerName;
		}


		public Dictionary<string, object> GetSSOAgentSession(string sessionName)
		{
			//creating dictionary to return as collection.
			Dictionary<string, object> sessionContext = new Dictionary<string, object>();
			sessionContext = (Dictionary<string, object>)Session[sessionName];
			return sessionContext;
		}

		protected long GetCustomerSessionId()
		{
			long customerSessionId = 0L;
			if (Session["CustomerSession"] != null)
			{
				customerSessionId = long.Parse(((CustomerSession)Session["CustomerSession"]).CustomerSessionId);
			}
			return customerSessionId;
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

		public MGI.Channel.DMS.Server.Data.MGIContext GetCheckLogin(long customerSessionId) //TODO: Pass customerid and add null check
		{
			CashACheck cashacheck = new CashACheck();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			ChannelPartnerProductProvider channelPartnerProductProvider = cashacheck.channelPartner.Providers.Find(x => x.ProcessorName.ToLower() == "ingo");
			// channelPartnerProductProvider will not be null if the processor is ingo
			if (channelPartnerProductProvider != null)
			{
				Desktop desktop = new Desktop();
				CheckLogin chexarLogin = new CheckLogin();
				if (HttpContext.Session["ChexarLogin"] == null)
				{
					chexarLogin = desktop.GetCheckLogin(customerSessionId, mgiContext);
					HttpContext.Session["ChexarLogin"] = chexarLogin;
					mgiContext.URL = chexarLogin.URL;
					mgiContext.IngoBranchId = chexarLogin.BranchId;
					mgiContext.CompanyToken = chexarLogin.CompanyToken;
					mgiContext.EmployeeId = chexarLogin.EmployeeId;
				}
				else
				{
					chexarLogin = (CheckLogin)HttpContext.Session["ChexarLogin"];
					mgiContext.URL = chexarLogin.URL;
					mgiContext.IngoBranchId = chexarLogin.BranchId;
					mgiContext.CompanyToken = chexarLogin.CompanyToken;
					mgiContext.EmployeeId = chexarLogin.EmployeeId;
				}
				return mgiContext;
			}
			else
				return new MGI.Channel.DMS.Server.Data.MGIContext();
		}
	}
}
