using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.ServiceClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Web.SSO
{
	public class TCF : BaseSSO
	{
		public const string SignXPath = "/samlp:Response";

		public override Server.Data.AgentSession AuthorizeSSOAgent(string samlResponse, string issuer, Models.SSOLogin ssoModel, out AgentSSO ssoAgent)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			string decodedResponse = Base64Decode(samlResponse);
			XmlDocument assertion = new XmlDocument();

			SSOErrorCodes ssoError = ValidateSAML(decodedResponse, issuer, ssoModel.channelPartner.Id, SignXPath, out assertion);
			
			if (ssoError != SSOErrorCodes.NoError)
			{
				throw new Exception(ssoError.ToString());
			}

			ssoError = ReadAttributes(assertion, out ssoAgent);
			if (ssoError != SSOErrorCodes.NoError)
			{
				throw new Exception(ssoError.ToString());
			}

			Desktop desktop = new Desktop();
			ChannelPartner channelPartner = desktop.GetChannelPartner(ssoModel.ChannelPartnerName, mgiContext);

			if (!string.IsNullOrEmpty(ssoModel.hostName))
			{
				HttpContext.Current.Session["HostName"] = ssoModel.hostName;
			}

			string terminalName = GetTerminalName(0, channelPartner, ssoModel.hostName);
			NLogHelper.Info("$$$ Get Terminal Name$$$");
			NLogHelper.Debug(terminalName);

			if (!string.IsNullOrWhiteSpace(terminalName))
			{
				HttpContext.Current.Session["IsTerminalSetup"] = 1;
				HttpContext.Current.Session["IsPeripheralServerSetUp"] = IsPeripheralServerSetUp(0, terminalName, channelPartner.Id) ? 1 : 0;
			}

			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.Context.Add("SSOAttributes", GetSSOAgentSession("SSO_AGENT_SESSION"));
			// Authenticate the agent in Nexxo DMS
			AgentSession agentSession = CreateAndAuthenticateAgent(ssoAgent, channelPartner.Id.ToString(), terminalName, mgiContext);
			return agentSession;
		}

		protected override SSOErrorCodes ReadAttributes(XmlDocument assertion, out AgentSSO ssoAgent)
		{
			ssoAgent = null;
			XmlNode attributeStatement;
			SSOErrorCodes ssoError = ReadAttributeStatement(assertion, out attributeStatement);
			if (ssoError != SSOErrorCodes.NoError)
			{
				return ssoError;
			}
			// pull the relevant nodes out of the attributes
			string userID = GetAttributeNode(attributeStatement, "UserID");
			string firstName = GetAttributeNode(attributeStatement, "givenName");
			string lastName = GetAttributeNode(attributeStatement, "sn");
			string groupMembership = GetAttributeNode(attributeStatement, "role");
			string bankNumber = GetAttributeNode(attributeStatement, "BankNum");
			string lawsonID = GetAttributeNode(attributeStatement, "LawsonID");
			string subClientNodeID = GetAttributeNode(attributeStatement, "DPSBranchID");
			string tellerNumber = GetAttributeNode(attributeStatement, "TellerNum");
			string cashDrawer = GetAttributeNode(attributeStatement, "CashDrawer");
			string branchNumber = GetAttributeNode(attributeStatement, "BranchNum");
			string hostName = GetAttributeNode(attributeStatement, "MachineName");
			string am_pmIndicator = GetAttributeNode(attributeStatement, "AmPmInd");
			string fullName = GetAttributeNode(attributeStatement, "displayName");
			string lu = GetAttributeNode(attributeStatement, "LU");
			//string memebrOf = GetAttributeNode(attributeStatement, "memebrOf");
			string softwareVersion = GetAttributeNode(attributeStatement, "SoftwareVersion");
			string businessDate = GetAttributeNode(attributeStatement, "BusinessDate");


			if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(groupMembership)
				|| string.IsNullOrWhiteSpace(userID))
			{
				NLogHelper.Info("Could not find the required attributes in SAML response");
				return SSOErrorCodes.SAMLAttributeNotFound;
			}

			// all good - set up an agent object
			ssoAgent = new AgentSSO();
			ssoAgent.UserName = userID;
			ssoAgent.FirstName = firstName;
			ssoAgent.LastName = lastName;
			ssoAgent.FullName = fullName;
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
			ssoAgent.ClientAgentIdentifier = tellerNumber;
			return SSOErrorCodes.NoError;
		}
	}
}