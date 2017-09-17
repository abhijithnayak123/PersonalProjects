using System;
using System.Web;
using System.Xml;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.SSO
{
	public class Okta : BaseSSO
	{
		public const string SignXPath = "/samlp:Response";

		public override ZeoClient.AgentSession AuthorizeSSOAgent(string samlResponse, string issuer, SSOLogin ssoModel, out ZeoClient.AgentSSO ssoAgent)
		{
           
            ZeoClient.Response response = new ZeoClient.Response();
            ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = new ZeoClient.ZeoContext();
         
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


            response = alloyClient.ChannelPartnerConfigByName(ssoModel.ChannelPartnerName, context);
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
            // Authenticate the agent in Nexxo DMS
            ZeoClient.AgentSession agentSession = CreateAndAuthenticateAgent(ssoAgent, channelPartner.Id.ToString(), terminalName, context);
			return agentSession;
		}
	}
}