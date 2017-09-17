using System;
using System.Web;
using TCF.Zeo.Common.Util;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using static TCF.Zeo.Common.Util.Helper;
#endregion

namespace TCF.Channel.Zeo.Web.Common
{
    public static class TerminalIdentifier
    {
        public static void IdentifyTerminal(long agentSessionId, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            string channelPartnerName;
            bool isterminalNotSetup = true;
            if (HttpContext.Current.Session["ChannelPartnerName"] == null)
                channelPartnerName = System.Configuration.ConfigurationManager.AppSettings.Get("ChannelPartner");
            else
                channelPartnerName = HttpContext.Current.Session["ChannelPartnerName"].ToString();
            ZeoClient.Response response = alloyServiceClient.ChannelPartnerConfigByName(channelPartnerName, context);

            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            ZeoClient.ChannelPartner channelPartner = response.Result as ZeoClient.ChannelPartner;

            string channelPartnerId = channelPartner.Id.ToString();

            if (channelPartner.TIM == (short)TerminalIdentificationMechanism.YubiKey)
            {
                isterminalNotSetup = isTerminalAvailable(agentSessionId, Helper.TerminalIdentificationMechanism.YubiKey, context);
            }
            else if (channelPartner.TIM == (short)TerminalIdentificationMechanism.Cookie)
            {
                isterminalNotSetup = isTerminalAvailable(agentSessionId, Helper.TerminalIdentificationMechanism.Cookie, context);
            }
            else if (channelPartner.TIM == (short)TerminalIdentificationMechanism.HostName)
            {
                if (HttpContext.Current.Session["HostName"] != null)
                { isterminalNotSetup = isTerminalAvailable(agentSessionId, Helper.TerminalIdentificationMechanism.HostName, context, channelPartnerId); }
                else
                {
                    throw new ZeoWebException(GetErrorMessage("1000.100.8091"));
                }
            }

            if (isterminalNotSetup && channelPartner.TIM != (short)TerminalIdentificationMechanism.HostName)
                throw new ZeoWebException(GetErrorMessage("1000.100.8092"));
        }

        public static bool IsTerminalAvailableForHostName(long agentSessionId, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            string channelPartnerName = string.Empty;
            bool isterminalNotSetup = false;
            if (HttpContext.Current.Session["ChannelPartnerName"] == null)
                channelPartnerName = System.Configuration.ConfigurationManager.AppSettings.Get("ChannelPartner");
            else
                channelPartnerName = HttpContext.Current.Session["ChannelPartnerName"].ToString();

            ZeoClient.Response response = alloyServiceClient.ChannelPartnerConfigByName(channelPartnerName, context);

            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            ZeoClient.ChannelPartner channelPartner = response.Result as ZeoClient.ChannelPartner;

            string channelPartnerId = channelPartner.Id.ToString();

            if (channelPartner.TIM == (short)TerminalIdentificationMechanism.HostName)
            { isterminalNotSetup = isTerminalAvailable(agentSessionId, Helper.TerminalIdentificationMechanism.HostName, context, channelPartnerId); }

            return isterminalNotSetup;
        }

        private static bool isTerminalAvailable(long agentSessionId, TerminalIdentificationMechanism TIM, ZeoClient.ZeoContext context, string channelPartnerId = "")
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response response = new ZeoClient.Response();

            bool isterminalNotSetup = true;
            switch (TIM)
            {
                case Helper.TerminalIdentificationMechanism.YubiKey:
                    bool isYubiKeyEnabled = Convert.ToBoolean(HttpContext.Current.Session["YubiKeyValidationRequired"]);
                    if (isYubiKeyEnabled && HttpContext.Current.Request.Cookies["Yubikeys"] != null)
                    {
                        isterminalNotSetup = false;
                    }
                    break;
                case Helper.TerminalIdentificationMechanism.Cookie:
                    HttpCookie terminalCookie = HttpContext.Current.Request.Cookies["TerminalCookie"];
                    if (terminalCookie != null)
                    {
                        isterminalNotSetup = false;
                    }
                    break;
                case Helper.TerminalIdentificationMechanism.HostName:
                    string hostName = string.Empty;
                    hostName = HttpContext.Current.Session["HostName"].ToString();
                    ZeoClient.Terminal hostTerminal = null;

                    if (!string.IsNullOrWhiteSpace(hostName))
                    {

                        response = alloyServiceClient.GetTerminalByName(hostName, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        hostTerminal = response.Result as ZeoClient.Terminal;
                    }

                    if (hostTerminal.TerminalId != 0)
                    { isterminalNotSetup = false; }

                    break;
                default:
                    break;
            }
            return isterminalNotSetup;
        }

        private static string GetErrorMessage(string messageKey)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = new ZeoClient.ZeoContext();
            ZeoClient.Response response = alloyServiceClient.GetMessage(messageKey, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            ZeoClient.Message msg = response.Result as ZeoClient.Message;
            string errorMessage = string.Join("|", new object[] { msg.Processor, msg.MessageKey, msg.Content, msg.AddlDetails, Helper.ErrorType.ERROR });
            return errorMessage;
        }

    }
}