using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using MGI.Channel.DMS.Web.Models;
using System.Diagnostics;
using MGI.Channel.DMS.Web.Common;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Web.SSO
{
    public class Okta : BaseSSO
    {
        public const string SignXPath = "/samlp:Response";
     
        public override Server.Data.AgentSession AuthorizeSSOAgent(string samlResponse, string issuer, SSOLogin ssoModel, out AgentSSO ssoAgent)
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

            string terminalName = GetTerminalName(0,  channelPartner, ssoModel.hostName);
			NLogHelper.Info("$$$ Get Terminal Name$$$");                
			NLogHelper.Debug(terminalName);          

            if (!string.IsNullOrWhiteSpace(terminalName))
            {
                HttpContext.Current.Session["IsTerminalSetup"] = 1;
                HttpContext.Current.Session["IsPeripheralServerSetUp"] = IsPeripheralServerSetUp(0, terminalName, channelPartner.Id) ? 1 : 0;
            }
            // Authenticate the agent in Nexxo DMS
            AgentSession agentSession = CreateAndAuthenticateAgent(ssoAgent, channelPartner.Id.ToString(), terminalName, mgiContext);
            return agentSession;
        }
    }
}