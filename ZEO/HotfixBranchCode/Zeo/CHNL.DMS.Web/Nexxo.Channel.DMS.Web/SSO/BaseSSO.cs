using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Xml;
using Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace MGI.Channel.DMS.Web.SSO
{
	public abstract class BaseSSO
	{
		protected const string SSOAgentSessionName = "SSO_AGENT_SESSION";
		protected const string SSOCert = "SSO_CERT";
		public abstract AgentSession AuthorizeSSOAgent(string SAMLResponse, string issuer, SSOLogin ssoModel, out AgentSSO ssoAgent);
		Dictionary<string, object> ssoAgentSession = new Dictionary<string, object>();
		#region AuthorizeSSO Agent

		/// <summary>
		/// 
		/// </summary>
		/// <param name="samlResponse"></param>
		/// <param name="certPath"></param>
		/// <param name="assertion"></param>
		/// <returns></returns>
		protected virtual SSOErrorCodes ValidateSAML(string samlResponse, string issuer, long channelpartnerId, string signatureXPath, out XmlDocument assertion)
		{
			// todo: This is VerifySAML
			assertion = new XmlDocument();
			assertion.PreserveWhitespace = false;

			NLogHelper.Info("Incoming SAML Response:" + Environment.NewLine + samlResponse);

			try
			{
				//AL-2181 Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw,
				//to overcome it we are making xmlresolver null
				byte[] byteArray = Encoding.UTF8.GetBytes(samlResponse);
				using (MemoryStream stream = new MemoryStream(byteArray))
				{
					XElement doc = XElement.Load(stream);
					XDocument Xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(doc));
					assertion.InnerXml = string.Concat(Xdoc.Declaration.ToString(), Xdoc.ToString());
				}
				//Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw,
				//to overcome it we are making xmlresolver null
				assertion.XmlResolver = null;
				NLogHelper.Info(string.Format(@"SAML response:\n{0}", assertion.InnerXml));
			}
			catch (Exception ex)
			{
				NLogHelper.Error(string.Format("Error loading SAML: {0}", ex.Message));
				return SSOErrorCodes.SAMLParseError;
			}

			//use a namespace manager to avoid the worst of xpaths
			XmlNamespaceManager ns = AddSAMLNameSpace(assertion);
			//create a new xml doc for the signature - preserve whitespace!
			var xmlDocForSignature = new XmlDocument { PreserveWhitespace = true };
			xmlDocForSignature.XmlResolver = null;

			xmlDocForSignature.LoadXml(samlResponse);

			// get the signature element, as XmlDsigNamespaceUrl
			XmlElement signature = xmlDocForSignature.DocumentElement["Signature", SignedXml.XmlDsigNamespaceUrl];
			if (signature == null)
			{
				NLogHelper.Error("Could not find ds:Signature in SAML response");
				return SSOErrorCodes.SAMLSignatureNotFound;
			}

			// create the signed xml document that will be checked with the cert
			var signedXml = new SignedXml(xmlDocForSignature);
			signedXml.LoadXml(signature);

			//load the certificate
			X509Certificate2 certificate;// = new X509Certificate2();

			try
			{
				certificate = GetSSOCertificate(channelpartnerId, issuer);

				if (certificate == null)
					return SSOErrorCodes.SAMLCertificateNotFound;
			}
			catch (Exception ex)
			{
				NLogHelper.Error(string.Format("Exception loading certificate: {0}", ex.Message));
				return SSOErrorCodes.SAMLCertificateNotFound;
			}
			CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

			// verify the signature
			if (!signedXml.CheckSignature(certificate, true))
			{
				NLogHelper.Info("CheckSignature failed");
				return SSOErrorCodes.SAMLVerificationFailure;
			}

			return SSOErrorCodes.NoError;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="assertion"></param>
		/// <param name="ssoAgent"></param>
		/// <returns></returns>
		protected virtual SSOErrorCodes ReadAttributes(XmlDocument assertion, out AgentSSO ssoAgent)
		{
			// todo: ReadAttributes
			ssoAgent = null;
			XmlNode attributeStatement;
			SSOErrorCodes ssoError = ReadAttributeStatement(assertion, out attributeStatement);
			if (ssoError != SSOErrorCodes.NoError)
			{
				return ssoError;
			}
			// pull the relevant nodes out of the attributes
			string agentUserName = GetAttributeNode(attributeStatement, "UserID");
			string agentFirstName = GetAttributeNode(attributeStatement, "firstName");
			string agentLastName = GetAttributeNode(attributeStatement, "lastName");
			string agentFullName = GetAttributeNode(attributeStatement, "fullName");
			string groupMembership = GetAttributeNode(attributeStatement, "role");

			if (string.IsNullOrWhiteSpace(agentFirstName) || string.IsNullOrWhiteSpace(agentLastName) || string.IsNullOrWhiteSpace(groupMembership) || string.IsNullOrWhiteSpace(agentUserName))
			{
				NLogHelper.Info("Could not find the required attributes in SAML response");
				return SSOErrorCodes.SAMLAttributeNotFound;
			}

			// all good - set up an agent object
			ssoAgent = new AgentSSO();
			ssoAgent.UserName = agentUserName;
			ssoAgent.FirstName = agentFirstName;
			ssoAgent.LastName = agentLastName;
			ssoAgent.FullName = agentFullName;
			string role = groupMembership;
			int rollId;
			ssoError = GetNexxoUserRole(role, out rollId);
			if (ssoError != SSOErrorCodes.NoError)
			{
				return ssoError;
			}
			ssoAgent.Role = new UserRole()
			{
				role = role,
				Id = rollId
			};

			return SSOErrorCodes.NoError;
		}

		protected virtual AgentSession CreateAndAuthenticateAgent(AgentSSO ssoAgent, string channelPartnerId, string terminalName, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			Desktop desktop = new Desktop();
			return desktop.AuthenticateSSO(ssoAgent, channelPartnerId, terminalName, mgiContext);
		}

		/// <summary>
		/// This creates saml namespace
		/// </summary>
		/// <param name="assertion"></param>
		/// <returns></returns>
		protected XmlNamespaceManager AddSAMLNameSpace(XmlDocument assertion)
		{
			XmlDocument xdoc = assertion;
			return SSOUtil.AddSAMLNameSpace(assertion);
		}

		protected string GetAttributeNode(XmlNode samlAttributes, string attributeName)
		{
			foreach (XmlNode node in samlAttributes.ChildNodes)
			{
				XmlAttribute att = node.Attributes["Name"];

				if (string.Equals(att.Value, attributeName, StringComparison.InvariantCultureIgnoreCase))
				{
					string attributeValue = GetAttributeValue(node);
					ssoAgentSession.Add(attributeName, attributeValue);
					HttpContext.Current.Session[SSOAgentSessionName] = ssoAgentSession;
					return attributeValue;
				}
			}
			return null;
		}

		protected string GetAttributeValue(XmlNode node)
		{
			return node.HasChildNodes ? node.FirstChild.InnerText : string.Empty;
		}

		protected SSOErrorCodes GetNexxoUserRole(string role, out int rollId)
		{
			NLogHelper.Info("user role: " + role);

			rollId = 0;
			UserRoles userRole;
			bool isRoleValid = Enum.TryParse<UserRoles>(role, out userRole);

			if (isRoleValid)
			{
				rollId = (int)userRole;
				return SSOErrorCodes.NoError;
			}
			return SSOErrorCodes.UserRoleNotFound;
		}

		protected string GetTerminalName(long agentSessionId, ChannelPartner channelPartner, string hostName)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			string terminalName = string.Empty;
			Desktop desktop = new Desktop();
			// Yubikey authentication not applicable to this mode of entry
			if (channelPartner.TIM == (short)TerminalIdentificationMechanism.Cookie)
			{
				HttpCookie terminalCookie = HttpContext.Current.Request.Cookies["TerminalCookie"];
				if (terminalCookie != null)
					terminalName = terminalCookie.Values["TerminalIdentifier"].ToString();
			}
			else if (channelPartner.TIM == (short)TerminalIdentificationMechanism.HostName)
			{
				if (!string.IsNullOrEmpty(hostName) && (desktop.LookupTerminal(agentSessionId, hostName, Convert.ToInt32(channelPartner.Id), mgiContext) != null))
					terminalName = hostName;
			}

			return terminalName;
		}

		protected bool IsPeripheralServerSetUp(long agentSessionId, string terminalName, long channelPartnerId)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop desktop = new Desktop();
			bool hasSetup = false;
			Server.Data.Terminal terminal = desktop.LookupTerminal(agentSessionId, terminalName, (int)channelPartnerId, mgiContext);
			if (terminal != null && terminal.PeripheralServer != null)
			{
				if (!string.IsNullOrWhiteSpace(terminal.PeripheralServer.PeripheralServiceUrl))
					hasSetup = true;
			}
			return hasSetup;
		}

		protected string Base64Decode(string base64EncodedData)
		{
			return SSOUtil.Base64Decode(base64EncodedData);
		}

		private X509Certificate2 GetSSOCertificate(long channelpartnerId, string issuer)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop desktop = new Desktop();
			ChannelPartnerCertificate channelPartnerCertificate = desktop.GetChannelPartnerCertificateInfo(channelpartnerId, issuer, mgiContext);

			X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			// Open the store.
			certificateStore.Open(OpenFlags.ReadWrite | OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
			// Find the certificate with the specified subject.
			X509Certificate2Collection certificates = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, channelPartnerCertificate.ThumbPrint, false);
			certificateStore.Close();
			if (certificates != null && certificates.Count > 0)
			{
				return certificates[0];
			}
			return null;
		}

		#endregion

		protected SSOErrorCodes ReadAttributeStatement(XmlDocument assertion, out XmlNode xmlAttributeNode)
		{
			xmlAttributeNode = null;

			//Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw,
			//to overcome it we are making xmlresolver null
			assertion.XmlResolver = null;

			XmlNamespaceManager ns = AddSAMLNameSpace(assertion);
			// get the SSO attributes
			xmlAttributeNode = assertion.SelectSingleNode("/samlp:Response/saml:Assertion/saml:AttributeStatement", ns);

			if (xmlAttributeNode == null)
			{
				NLogHelper.Info("Could not find saml:AttributeStatement in SAML response");
				return SSOErrorCodes.SAMLAttributeStatementNotFound;
			}
			return SSOErrorCodes.NoError;
		}

		public Dictionary<string, object> GetSSOAgentSession(string sessionName)
		{
			//creating dictionary to return as collection.
			Dictionary<string, object> sessionContext = new Dictionary<string, object>();
			sessionContext = (Dictionary<string, object>)HttpContext.Current.Session[sessionName];
			return sessionContext;
		}
	}
}
