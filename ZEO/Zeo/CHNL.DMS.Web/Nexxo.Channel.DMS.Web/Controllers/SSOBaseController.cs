using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.SSO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using Helper = TCF.Zeo.Common.Util.Helper;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
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
                    return new TCF.Channel.Zeo.Web.SSO.TCF();
                default:
                    return new Okta();
			}
		}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NLogHelper.Info(string.Format("Begin action call {0}/{1}.", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName));
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            NLogHelper.Info(string.Format("Completed action call {0}/{1}.", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName));
        }

        protected override void OnException(ExceptionContext exceptionContext)
		{
			if (exceptionContext.Exception != null)
			{
				NLogHelper.Error(exceptionContext);
			}
		}		

        protected string getTerminalName(ZeoClient.ChannelPartner channelPartner, string hostName)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = new ZeoClient.ZeoContext()
            {
                ChannelPartnerId = channelPartner.Id
            };
            string terminalName = string.Empty;
            long agentSessionId = GetAgentSessionId();
            // Yubikey authentication not applicable to this mode of entry
            if (channelPartner.TIM == (short)Helper.TerminalIdentificationMechanism.Cookie)
            {
                HttpCookie terminalCookie = Request.Cookies["TerminalCookie"];
                if (terminalCookie != null)
                    terminalName = terminalCookie.Values["TerminalIdentifier"].ToString();
            }
            else if (channelPartner.TIM == (short)Helper.TerminalIdentificationMechanism.HostName)
            {
                if (!string.IsNullOrEmpty(hostName) && (alloyServiceClient.GetTerminalByName(hostName, context) != null))
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
       

        #region Verify Exception

        internal bool VerifyException(ZeoClient.Response response)
		{
			bool isErrorRaised = false;
			if (response.Error != null)
			{
				var errorDetails = response.Error.Details.Split('|');
				string errorType = errorDetails.Count() == 5 ? errorDetails[4].ToString() : Helper.ErrorType.ERROR.ToString();
				if (errorType == Helper.ErrorType.ERROR.ToString() || errorType == Helper.ErrorType.WARNING.ToString())
				{
					ViewBag.IsException = true;
					ViewBag.IsExceptionRaised = true;
					ViewBag.ExceptionMessage = response.Error.Details;
					isErrorRaised = true;
				}
			}
			return isErrorRaised;
		}

		internal bool VerifyException(Exception ex)
		{
			ViewBag.IsException = true;
			ViewBag.IsExceptionRaised = true;
			Exception faultException = ex as FaultException;
			if (faultException != null)
			{
				ViewBag.ExceptionMessage = WebHelper.GetAppSettingValue("FaultExceptionMessage");
				return true;
			}
			Exception endpointException = ex as EndpointNotFoundException;
			if (endpointException != null)
			{
				ViewBag.ExceptionMessage = WebHelper.GetAppSettingValue("EndpointNotFoundExceptionMessage");
				return true;
			}
			Exception commException = ex as CommunicationException;
			if (commException != null)
			{
				ViewBag.ExceptionMessage = WebHelper.GetAppSettingValue("CommunicationExceptionMessage");
				return true;
			}
			Exception timeOutException = ex as TimeoutException;
			if (timeOutException != null)
			{
				ViewBag.ExceptionMessage = WebHelper.GetAppSettingValue("TimeoutExceptionMessage");
				return true;
			}
			Exception alloyWebException = ex as ZeoWebException;
			if (alloyWebException != null)
			{
				ViewBag.ExceptionMessage = alloyWebException.Message;
				return true;
			}

			ViewBag.ExceptionMessage = WebHelper.GetAppSettingValue("UnhandledExceptionMessage");
			return true;
		}
		#endregion

	}
}
