using TCF.Channel.Zeo.Web.Common;
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion


namespace TCF.Channel.Zeo.Web.SSO
{
	public class TCF : BaseSSO
	{
		public const string SignXPath = "/samlp:Response";

		public override ZeoClient.AgentSession AuthorizeSSOAgent(string samlResponse, string issuer, Models.SSOLogin ssoModel, out ZeoClient.AgentSSO ssoAgent)
		{
            ZeoClient.ZeoContext context = new ZeoClient.ZeoContext();

       
            ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response response = new ZeoClient.Response();
           
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


            response = serviceClient.ChannelPartnerConfigByName(ssoModel.ChannelPartnerName, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            ZeoClient.ChannelPartner channelPartner = response.Result as ZeoClient.ChannelPartner;

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

            context.Context = new Dictionary<string, object>();
            context.SSOAttributes = GetSSOAgentSession("SSO_AGENT_SESSION");
            // Authenticate the agent in Nexxo DMS
            ZeoClient.AgentSession agentSession = CreateAndAuthenticateAgent(ssoAgent, channelPartner.Id.ToString(), terminalName, context);
			return agentSession;
		}

		protected override SSOErrorCodes ReadAttributes(XmlDocument assertion, out ZeoClient.AgentSSO ssoAgent)
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
			ssoAgent = new ZeoClient.AgentSSO();
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
            ssoAgent.RoleId = rollId;
            ssoAgent.role = role;
			ssoAgent.ClientAgentIdentifier = tellerNumber;
			return SSOErrorCodes.NoError;
		}
	}
}