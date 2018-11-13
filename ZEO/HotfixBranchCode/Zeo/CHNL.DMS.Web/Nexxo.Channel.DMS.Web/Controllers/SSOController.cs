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
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;



namespace MGI.Channel.DMS.Web.Controllers
{
    [AllowAnonymous]
    public class SSOController : SSOBaseController
    {
         Desktop desktop = new Desktop();

        private Hashtable HTSessions = new Hashtable();
       
        public ActionResult Authenticate(SSOLogin ssoModel)
        {
            Session.Clear();
            try
            {
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
                string issuer = SSOUtil.GetIssuer(ssoModel.SAMLResponse);
                string channelPartnerName = (string)NexxoUtil.GetDictionaryValue(SSOUtil.SSOChannelPartner(), issuer);
                Session["ChannelPartnerName"] = channelPartnerName;

				NLogHelper.Info(string.Format("Channel Partner Name {0}", channelPartnerName));				
				ssoModel.channelPartner = desktop.GetChannelPartner(channelPartnerName, mgiContext);
				ssoModel.ChannelPartnerName = channelPartnerName;
                ssoModel.Issuer = issuer;
				ClearCookie();
            }
            catch (Exception ex)
            {
                SSOErrorCodes error;
                NLogHelper.Error(ex);
                bool iserror = Enum.TryParse<SSOErrorCodes>(ex.Message, out error);
                if (iserror)
                {
                    ssoModel.ErrorCode = error;
                }
                else
                {
                    ssoModel.ErrorCode = SSOErrorCodes.ApplicationError;
                }
                return View("Login", ssoModel);
            }
            ssoModel.ErrorCode = SSOErrorCodes.NoError;
            return View("Login", ssoModel);
        }

        [HttpPost]
        public ActionResult SSOLogin(SSOLogin ssoModel)
        {

            //Nlogger.SetContext(HttpContext.Session.SessionID.ToString(), null);          
			NLogHelper.Info("$$$ Start of SSO Login $$$");
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            BaseSSO baseSSO = GetSSO(ssoModel.ChannelPartnerName);
            AgentSession agentSession = null;
            AgentSSO ssoAgent = null;
            try
            {
                 agentSession = baseSSO.AuthorizeSSOAgent(ssoModel.SAMLResponse, ssoModel.Issuer, ssoModel, out ssoAgent);
            }
            catch (Exception ex)
            {
                SSOErrorCodes error;
				NLogHelper.Error("SSOLogin :"+ ex.Message);
				NLogHelper.Error(ex.StackTrace);				
               
                 bool iserror = Enum.TryParse<SSOErrorCodes>(ex.Message, out error);
                if (iserror)
                {
                    ssoModel.ErrorCode = error;
                }
                else
                {
                    ssoModel.ErrorCode = SSOErrorCodes.ApplicationError;
                }
                return View("Login", ssoModel);
            }

            if (agentSession == null || agentSession.SessionId == "0")
            {
				NLogHelper.Info("MGi Alloy authentication failed");
				ViewBag.AuthResult = "MGiAlloy Authentication failed";
                ssoModel.ErrorCode = SSOErrorCodes.ApplicationError;
                return View("Login", ssoModel);
            }

            Session["agentId"] = agentSession.Agent.Id;
            // adding this for Role-based screen display
            if (agentSession.Agent.Id != 0)
                Session["UserRoleId"] = desktop.GetUser(long.Parse(agentSession.SessionId), agentSession.Agent.Id, mgiContext).UserRoleId;

            Session["sessionId"] = agentSession.SessionId;
            if (agentSession.Terminal != null)
                Session["CurrentLocation"] = agentSession.Terminal.Location.LocationName;
            HTSessions.Add("TempSessionAgent", agentSession);
            HTSessions.Add("AgentSessionId", agentSession.SessionId);
            HTSessions.Add("ExpDays", "30");
            HTSessions.Add("UserId", agentSession.Agent.UserName);

			NLogHelper.Debug("Agent Id: {0}", agentSession.Agent.Id);
			NLogHelper.Debug("Agent SessionId: {0}", agentSession.SessionId);
                     
            int authstatus = agentSession.Agent.AuthenticationStatus;
            if (authstatus == 1)
            {
                var authTicket = new FormsAuthenticationTicket(
                           1,                           // version
                           ssoAgent.UserName,           // username
                           DateTime.Now,                // creation
                           DateTime.Now.AddMinutes(Session.Timeout + 1), // expiration
                           false,                       // persistent?
                           ssoAgent.Role.role           // user data
                       );
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);
            }

            Session["HTSessions"] = HTSessions;
            Session["UserNameText"] = agentSession.Agent.Name;
            Session["UserIDText"] = agentSession.Agent.UserName;
            Session["SSOLogin"] = true;
            TempData["IsChooseLocation"] = false;
            bool isWesternUnionProvider = ssoModel.channelPartner.Providers.Any(x => x.ProcessorName == "WesternUnion");
            if (isWesternUnionProvider)
            {            
                return RedirectToAction("AgentBannerMessage", "WesternUnionBanner");
            }
            else
            {
                return RedirectToAction("CustomerSearch", "CustomerSearch");
            }
        }

        public ActionResult Logout()
        {
            SSOLogin ssoModel = new SSOLogin();
            ssoModel.ChannelPartnerName = (string)Session["ChannelPartnerName"];
            ClearSessions();
			ClearCookie();
			return View(ssoModel);
        }

		private void ClearCookie()
		{
			string[] myCookies = Request.Cookies.AllKeys;
			foreach (var cookie in myCookies)
			{
				if (cookie != "ASP.NET_SessionId")
				{
					Response.Cookies[cookie].Expires = DateTime.Now;
				}
			}
		}

        private void ClearSessions()
        {
            Session.RemoveAll();
        }
    }
}
