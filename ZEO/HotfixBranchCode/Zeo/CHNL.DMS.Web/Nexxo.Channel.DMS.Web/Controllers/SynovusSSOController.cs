using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using Security.Cryptography;

using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Web.ServiceClient.DMSService;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Server.Data;
using MGI.Common.Sys;
using NexxoSOAPFault = MGI.Common.Sys.NexxoSOAPFault;
using MGI.Channel.DMS.Web.SSO;
using System.Web.Security;
using MGI.Common.Util;


namespace MGI.Channel.DMS.Web.Controllers
{
	[AllowAnonymous]
	public class SynovusSSOController : SSOBaseController
	{
		Desktop desktop = new Desktop();

		private Hashtable HTSessions = new Hashtable();
		private static string cookieName = "FSSESSION_EDIR";
		private string BackChannelUrl = ConfigurationManager.AppSettings["SynovusSSOUrl"];
		private static string channelPartnerName = "Synovus";
		//
		// GET: /SynovusSSO/
		public ActionResult Login()
		{
			Session.Clear();
			Session["ChannelPartnerName"] = channelPartnerName;
			NLogHelper.Debug("Channel Partner Name {0}", channelPartnerName);

			SSOLogin ssoModel = new SSOLogin
			{
				ErrorCode = SSOErrorCodes.NoError
			};

			return View(ssoModel);
		}

		[HttpPost]
		public ActionResult Login(SSOLogin ssoModel)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			NLogHelper.Info("$$$ Start of SSO Login $$$");

			//look for the Synovus Authentication cookie
			if (Request.Cookies.Count == 0 || Request.Cookies[cookieName] == null)
			{
				ssoModel.ErrorCode = SSOErrorCodes.CookieNotFound;
				return View(ssoModel);
			}

			// Authenticate the cookie and retrieve agent details
			AgentSSO ssoAgent;
			NLogHelper.Debug("cookie value:" + Request.Cookies[cookieName].Value);

			SSOErrorCodes ssoAuthError = AuthorizedSSOAgent(Request.Cookies[cookieName].Value, out ssoAgent);
			if (ssoAuthError != SSOErrorCodes.NoError)
			{
				ssoModel.ErrorCode = ssoAuthError;
				NLogHelper.Debug("SSOErrorCode: " + ssoAuthError.ToString());
				return View(ssoModel);
			}

			ChannelPartner channelPartner = desktop.GetChannelPartner(channelPartnerName, mgiContext);

			if (!string.IsNullOrEmpty(ssoModel.hostName))
			{
				Session["HostName"] = ssoModel.hostName;
			}

			string terminalName = getTerminalName(desktop, channelPartner, ssoModel.hostName);
			//Trace.Unindent();
			NLogHelper.Info("$$$ Get Terminal Name$$$");
			NLogHelper.Debug(terminalName);

			//Commented to address Auto Terminal Setup Issue

			//if (terminalName == string.Empty)
			//{
			//    ssoModel.ErrorCode = SSOErrorCodes.TerminalNotSetup;
			//    return View(ssoModel);
			//}


			if (!string.IsNullOrWhiteSpace(terminalName))
            {
                Session["IsTerminalSetup"] = 1;
                Session["IsPeripheralServerSetUp"] = IsPeripheralServerSetUp(0, terminalName, channelPartner.Id) ? 1 : 0;
            }

			//Session["IsTerminalSetup"] = 1;
			// Authenticate the agent in Nexxo DMS
			AgentSession agentSession = desktop.AuthenticateSSO(ssoAgent, channelPartner.Id.ToString(), terminalName, mgiContext);

			if (agentSession.SessionId == "0")
			{
				NLogHelper.Info("MGi Alloy authentication failed");
				ViewBag.AuthResult = "MGiAlloy Authentication failed";
				return View(ssoModel);
			}

			Session["agentId"] = agentSession.Agent.Id;
			
			Session["sessionId"] = agentSession.SessionId;
			if (agentSession.Terminal != null)
				Session["CurrentLocation"] = agentSession.Terminal.Location.LocationName;
			HTSessions.Add("TempSessionAgent", agentSession);
			HTSessions.Add("AgentSessionId", agentSession.SessionId);
			HTSessions.Add("ExpDays", "30");
			HTSessions.Add("UserId", agentSession.Agent.UserName);

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
			string bannerMessage = string.Empty;

            // adding this for Role-based screen display
            if (agentSession.Agent.Id != 0)
                Session["UserRoleId"] = desktop.GetUser(GetAgentSessionId(), agentSession.Agent.Id, mgiContext).UserRoleId;
            
            if (String.IsNullOrEmpty(terminalName))
			{
				TempData["BannerMessage"] = "Unable to establish communication with Western Union. Please setup MGi Alloy Terminal Location.";
				NLogHelper.Info("Unable to establish communication with Western Union. Please setup MGi Alloy Terminal Location.");
				return RedirectToAction("AgentBannerMessage", "WesternUnionBanner");
			}
			else
			{
				if (agentSession.Terminal != null && agentSession.Terminal.Location != null)
				{
					try
					{

						bannerMessage = desktop.WUGetAgentBannerMessage(Convert.ToInt64(agentSession.SessionId), mgiContext);
					}

					catch
					{
						NLogHelper.Error("Unable to establish communication with Western Union. Please setup MGi Alloy Terminal");
						bannerMessage = "Unable to establish communication with Western Union. Please setup MGi Alloy Terminal.";

					}
				}
				else
				{

					NLogHelper.Error("Unable to establish communication with Western Union. Please setup MGi Alloy Terminal");
					bannerMessage = "Unable to establish communication with Western Union. Please setup MGi Alloy Terminal Location.";
				}
			}
			if (bannerMessage != string.Empty)
			{
				TempData["BannerMessage"] = bannerMessage;
				return RedirectToAction("AgentBannerMessage", "WesternUnionBanner");
			}
			else
			{
				return RedirectToAction("CustomerSearch", "CustomerSearch");
			}

		}

		private SSOErrorCodes AuthorizedSSOAgent(string cookieValue, out AgentSSO ssoAgent)
		{
			ssoAgent = null;
			NLogHelper.Error("Called Synovus sso controller AuthorizedSSOAgent method.");
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BackChannelUrl);

			if (request.CookieContainer == null)
				request.CookieContainer = new CookieContainer();
			request.CookieContainer.Add(new Cookie(cookieName, cookieValue, "/", ".synovus.com"));


			HttpWebResponse response;
			XmlDocument assertion = new XmlDocument();
            try
            {
                /****************************Begin TA-50 Changes************************************************/
                //     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
                //     Purpose: On Vera Code Scan, This call contains a Improper Resource Shutdown or Release flaw, which we solved by adding using block for response
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    NLogHelper.Info(string.Format("status code returned from {0}: {1}", BackChannelUrl, response.StatusCode));
                    NLogHelper.Info(string.Format("response: {0}", new StreamReader(response.GetResponseStream()).ReadToEnd()));

                    return SSOErrorCodes.AuthenticationFailure;
                }
                NLogHelper.Error("Verified SSO response.StatusCode");
            }

            catch (Exception ex)
            {
                NLogHelper.Error(string.Format("Error in web request: {0}", ex.Message));
                return SSOErrorCodes.AuthenticationFailure;
            }

            assertion.PreserveWhitespace = true;

			try
			{
				NLogHelper.Error("Response stream {0}", response.GetResponseStream());
				/****************************Begin TA-50 Changes************************************************/
				//     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
				//     Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw, to overcome it we making xmlresolver null
				assertion.XmlResolver = null;
				assertion.Load(response.GetResponseStream());

				NLogHelper.Info(string.Format(@"SAML response:\n{0}", assertion.InnerXml));
			}
			catch (Exception ex)
			{
				NLogHelper.Error(string.Format("Error loading SAML: {0}", ex.ToString()));
				return SSOErrorCodes.SAMLParseError;
			}

			// use a namespace manager to avoid the worst of xpaths
			var ns = new XmlNamespaceManager(assertion.NameTable);
			ns.AddNamespace("samlp", @"urn:oasis:names:tc:SAML:2.0:protocol");
			ns.AddNamespace("saml", @"urn:oasis:names:tc:SAML:2.0:assertion");
			ns.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);

			// get the signature XML node
			var signNode = assertion.SelectSingleNode("/samlp:Response/saml:Assertion/ds:Signature", ns);
			if (signNode == null)
			{
				NLogHelper.Info("Could not find ds:Signature in SAML response");
				return SSOErrorCodes.SAMLSignatureNotFound;
			}

			// load the signed xml
			var signedXml = new SignedXml((XmlElement)assertion.SelectSingleNode("/samlp:Response/saml:Assertion", ns));
			signedXml.LoadXml((XmlElement)signNode);

			// load the certificate
			X509Certificate2 certificate;
			try
			{
				certificate = new X509Certificate2(ConfigurationManager.AppSettings["SynovusSSOCertificatePath"]);
			}
			catch (Exception ex)
			{

				NLogHelper.Error(string.Format("Exception loading certificate: {0}", ex.Message));
				return SSOErrorCodes.SAMLCertificateNotFound;
			}

			// check the cert itself
			if (!certificate.Verify())
				return SSOErrorCodes.SAMLCertificateInvalid;

            //Add the below code for SHA-2 Support, since .NET FW 4.0 does not have inbuild SHA-2 support.

            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

			// verify the signature
			if (!signedXml.CheckSignature(certificate, true))
			{

				NLogHelper.Info("CheckSignature failed");
				return SSOErrorCodes.SAMLVerificationFailure;
			}

			// reload the doc but without whitespace, for simple node searches
			XmlDocument cleanAssertion = new XmlDocument();
			/****************************Begin TA-50 Changes************************************************/
			//     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
			//     Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw, to overcome it we making xmlresolver null and using load instead of loadxml
			cleanAssertion.XmlResolver = null;
			if (assertion.InnerXml.Length > 0)
			{
				cleanAssertion.LoadXml(assertion.InnerXml);
			}
			else
				return SSOErrorCodes.ApplicationError;
			/****************************End TA-50 Changes************************************************/

			// get the SSO attributes
			var attributeStatement = cleanAssertion.SelectSingleNode("/samlp:Response/saml:Assertion/saml:AttributeStatement", ns);

			if (attributeStatement == null)
			{
				NLogHelper.Info("Could not find saml:AttributeStatement in SAML response");
				return SSOErrorCodes.SAMLAttributeStatementNotFound;
			}

			// pull the relevant nodes out of the attributes
			var agentIdNode = getAttributeNode(attributeStatement, "workforceid");
			var agentFirstNameNode = getAttributeNode(attributeStatement, "givenName");
			var agentLastNameNode = getAttributeNode(attributeStatement, "sn");
			var groupMembershipNode = getAttributeNode(attributeStatement, "groupMembership");

			if (agentIdNode == null || agentFirstNameNode == null || agentLastNameNode == null || groupMembershipNode == null)
			{

				NLogHelper.Info("Could not find attributes in SAML response");
				return SSOErrorCodes.SAMLAttributeNotFound;
			}

			// all good - set up an agent object
			ssoAgent = new AgentSSO();
			ssoAgent.UserName = getAttributeValue(agentIdNode);
			ssoAgent.FirstName = getAttributeValue(agentFirstNameNode);
			ssoAgent.LastName = getAttributeValue(agentLastNameNode);
			ssoAgent.Role = new UserRole { Id = getNexxoUserRole(getAttributeValues(groupMembershipNode)) };
			ssoAgent.Role.role = ((UserRoles)ssoAgent.Role.Id).ToString();

			return SSOErrorCodes.NoError;
		}

		private XmlNode getAttributeNode(XmlNode samlAttributes, string attributeName)
		{
			foreach (XmlNode node in samlAttributes.ChildNodes)
			{
				XmlAttribute att = node.Attributes["Name"];

				if (att.Value == attributeName)
					return node;
			}

			return null;
		}

		private string getAttributeValue(XmlNode node)
		{
			return node.HasChildNodes ? node.FirstChild.InnerText : string.Empty;
		}

		private List<string> getAttributeValues(XmlNode node)
		{
			List<string> attributeValues = new List<string>();

			if (node.HasChildNodes)
			{
				foreach (XmlNode n in node.ChildNodes)
					attributeValues.Add(n.InnerText);
			}

			return attributeValues;
		}
        
        protected bool IsPeripheralServerSetUp(long agentSessionId, string terminalName, long channelPartnerId)
        {
            Desktop desktop = new Desktop();
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            bool hasSetup = false;
            Server.Data.Terminal terminal = desktop.LookupTerminal(agentSessionId, terminalName, (int)channelPartnerId, mgiContext);
            if (terminal != null && terminal.PeripheralServer != null)
            {
                if (!string.IsNullOrWhiteSpace(terminal.PeripheralServer.PeripheralServiceUrl))
                    hasSetup = true;
	}
            return hasSetup;
}

	}
}
