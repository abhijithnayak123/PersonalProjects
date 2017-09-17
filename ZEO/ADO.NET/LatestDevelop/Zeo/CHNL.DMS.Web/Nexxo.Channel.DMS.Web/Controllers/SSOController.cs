using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.SSO;
using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
using System.Collections.Generic;
#endregion


namespace TCF.Channel.Zeo.Web.Controllers
{
    [AllowAnonymous]
    public class SSOController : SSOBaseController
    {

        private Hashtable HTSessions = new Hashtable();

        public ActionResult Authenticate(SSOLogin ssoModel)
        {
            Session.Clear();
            try
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = new ZeoClient.ZeoContext();
                string issuer = SSOUtil.GetIssuer(ssoModel.SAMLResponse);
                string channelPartnerName = (string)Helper.GetDictionaryValue(SSOUtil.SSOChannelPartner(), issuer);
                Session["ChannelPartnerName"] = channelPartnerName;

                NLogHelper.Info(string.Format("Channel Partner Name {0}", channelPartnerName));

                response = serviceClient.ChannelPartnerConfigByName(channelPartnerName, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ssoModel.channelPartner = response.Result as ZeoClient.ChannelPartner;

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
            ZeoClient.ZeoContext context = new ZeoClient.ZeoContext();
            ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
            NLogHelper.Info("$$$ Start of SSO Login $$$");
            BaseSSO baseSSO = GetSSO(ssoModel.ChannelPartnerName);
            ZeoClient.AgentSession agentSession = null;
            ZeoClient.AgentSSO ssoAgent = null;
            try
            {
                agentSession = baseSSO.AuthorizeSSOAgent(ssoModel.SAMLResponse, ssoModel.Issuer, ssoModel, out ssoAgent);

                ZeoClient.Response alloContextResponse = alloyClient.GetZeoContextForAgent(long.Parse(agentSession.SessionId), context);
                if (WebHelper.VerifyException(alloContextResponse)) throw new ZeoWebException(alloContextResponse.Error.Details);
                context = (ZeoClient.ZeoContext)alloContextResponse.Result;

                if (Session["SSO_AGENT_SESSION"] != null)
                { context.SSOAttributes = Session["SSO_AGENT_SESSION"] as Dictionary<string, object>; }

                //Set the AlloyContext in Session when the agent is initiated and use it across the modules.
                Session["ZeoContext"] = context;

                alloContextResponse = alloyClient.GetBINDetails(context);
                if (WebHelper.VerifyException(alloContextResponse)) throw new ZeoWebException(alloContextResponse.Error.Details);
                List<ZeoClient.KeyValuePair> cardBINs = (List<ZeoClient.KeyValuePair>)alloContextResponse.Result;

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("cardBINs", cardBINs);

                // Dictionary is the common Session to all the object using Key value.
                Session["Dictionary"] = dictionary;

            }
            catch (Exception ex)
            {
                SSOErrorCodes error;
                NLogHelper.Error("SSOLogin :" + ex.Message);
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
                NLogHelper.Info("Zeo authentication failed");
                ViewBag.AuthResult = "Zeo Authentication failed";
                ssoModel.ErrorCode = SSOErrorCodes.ApplicationError;
                return View("Login", ssoModel);
            }

            bool isWesternUnionProvider;
            try
            {
                Session["agentId"] = agentSession.AgentID;
                // adding this for Role-based screen display
                if (agentSession.AgentID != 0)
                {

                    //ZeoClient.Response alloContextResponse = alloyClient.GetZeoContextForAgent(long.Parse(agentSession.SessionId), context);
                    //if (WebHelper.VerifyException(alloContextResponse)) throw new ZeoWebException(alloContextResponse.Error.Details);
                    //context = (ZeoClient.ZeoContext)alloContextResponse.Result;

                    ZeoClient.Response response = alloyClient.GetAgentRoleId(agentSession.AgentID, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    Session["UserRoleId"] = response.Result;
                }

                Session["sessionId"] = agentSession.SessionId;
                Session["AgentSession"] = agentSession;

                if (agentSession.LocationName != null)
                    Session["CurrentLocation"] = agentSession.LocationName;
                HTSessions.Add("TempSessionAgent", agentSession);
                HTSessions.Add("AgentSessionId", agentSession.SessionId);
                HTSessions.Add("UserId", agentSession.UserName);

                NLogHelper.Debug("Agent Id: {0}", agentSession.AgentID);
                NLogHelper.Debug("Agent SessionId: {0}", agentSession.SessionId);

                int authstatus = agentSession.AuthenticationStatus;
                if (authstatus == 1)
                {
                    var authTicket = new FormsAuthenticationTicket(
                               1,                           // version
                               ssoAgent.UserName,           // username
                               DateTime.Now,                // creation
                               DateTime.Now.AddMinutes(Session.Timeout + 1), // expiration
                               false,                       // persistent?
                               ssoAgent.role           // user data
                           );
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);
                }

                Session["HTSessions"] = HTSessions;
                Session["UserNameText"] = agentSession.Name;
                Session["UserIDText"] = agentSession.UserName;
                Session["SSOLogin"] = true;
                TempData["IsChooseLocation"] = false;
                isWesternUnionProvider = ssoModel.ProductProvider.Any(x => x.ProcessorName == "WesternUnion");
                if (isWesternUnionProvider)
                {
                    return RedirectToAction("AgentBannerMessage", "WesternUnionBanner");
                }
                else
                {
                    return RedirectToAction("CustomerSearch", "CustomerSearch");
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                isWesternUnionProvider = ssoModel.ProductProvider.Any(x => x.ProcessorName == "WesternUnion");
                if (isWesternUnionProvider)
                {
                    return RedirectToAction("AgentBannerMessage", "WesternUnionBanner", new { IsException = true, ExceptionMessage = System.Web.HttpUtility.JavaScriptStringEncode(ViewBag.ExceptionMessage) });
                }
                else
                {
                    return RedirectToAction("CustomerSearch", "CustomerSearch", new { IsException = true, ExceptionMessage = System.Web.HttpUtility.JavaScriptStringEncode(ViewBag.ExceptionMessage) });
                }
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
