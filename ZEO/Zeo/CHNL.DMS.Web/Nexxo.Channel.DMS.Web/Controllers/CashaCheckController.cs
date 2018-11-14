using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion


namespace TCF.Channel.Zeo.Web.Controllers
{
    public class CashaCheckController : BaseController
    {
        // Abey : Adding the get method as part of fix - DE1087
        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "CheckImage", MasterName = "_Common")]
        public ActionResult CashACheckPost()
        {   
            CheckImage checkImage = new CheckImage();
            return View("CheckImage", checkImage);
        }
        // End Changes

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "CheckImage", MasterName = "_Common")]
        public ActionResult CashACheckPost(CashACheck checkCash)
        {
			System.Collections.Hashtable HtLocaluse = (System.Collections.Hashtable)Session["HTSessions"];
			string peripheralServiceUrl = string.Empty;
            int onUSCheckType = 0;
            ZeoClient.AgentSession agentSession;

			if (HtLocaluse != null)
			{
				agentSession = ((ZeoClient.AgentSession)(HtLocaluse["TempSessionAgent"]));
				if (agentSession.TerminalId != 0)
				{
					if (!string.IsNullOrWhiteSpace(agentSession.PeripheralServiceUrl))
					{
						peripheralServiceUrl = agentSession.PeripheralServiceUrl;
					}
				}
			}

			CheckImage checkImage = new CheckImage() 
			{
				CheckFrontImage = checkCash.CheckFrontImage,
				CheckBackImage = checkCash.CheckBackImage,
				MICRCode = checkCash.MICRCode,
				NpsId = checkCash.NpsId,
				CheckFrontImage_TIFF = checkCash.CheckFrontImage_TIFF,
				CheckBackImage_TIFF = checkCash.CheckBackImage_TIFF,
				MicrErrorMessage = checkCash.MicrErrorMessage,
				//NpsURL = string.Format("{0}GetImage?id={1}",peripheralServiceUrl, checkCash.MICRCode),
				AccountNumber = checkCash.AccountNumber,
				RoutingNumber = checkCash.RoutingNumber,
                CheckNumber = checkCash.CheckNumber
            };

            if (!string.IsNullOrWhiteSpace(checkCash.AccountNumber) && !string.IsNullOrWhiteSpace(checkCash.CheckNumber) && 
                !string.IsNullOrWhiteSpace(checkCash.RoutingNumber) && Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsOnUsActive"]))
            {
                ZeoClient.MICRDetails micrDetails = new ZeoClient.MICRDetails()
                {
                    RoutingNumber = checkCash.RoutingNumber.All(c => char.IsDigit(c)) ? long.Parse(checkCash.RoutingNumber) : 0,
                    CheckNumber = long.Parse(checkCash.CheckNumber),
                    AccountNumber = long.Parse(checkCash.AccountNumber)
                };
                ZeoClient.CheckProviderDetails checkProviderDetails = verifyProvider(micrDetails);

                checkImage.CheckTypeId = onUSCheckType = checkProviderDetails.CheckTypeId;
                checkImage.ProductProviderCode = checkProviderDetails.ProductProviderCode;
            }
            else
            {
                checkImage.ProductProviderCode = ZeoClient.HelperProviderId.Ingo;
            }

            //Updating the Provider Id in the ZEO Context Session after verying the Check details.
            //and setting the SSOAttributes for passing to teller inquiry.
            //Starts Here
            ZeoClient.ZeoContext zeoContext = GetZeoContext();
            zeoContext.ProviderId = (int)checkImage.ProductProviderCode;
            zeoContext.OnUSChecktype = onUSCheckType;

            zeoContext.Context = new Dictionary<string, object>();
            zeoContext.SSOAttributes = GetSSOAttributes("SSO_AGENT_SESSION");
            Session["ZeoContext"] = zeoContext;
            //Ends Here

            if (checkCash.MicrError == 1)
            {
                Session["MicrErrorCount"] = Session["MicrErrorCount"]==null ? checkCash.MicrError : Convert.ToInt32(Session["MicrErrorCount"]) + 1;
                if (Session["MicrErrorCount"] != null && Convert.ToInt32(Session["MicrErrorCount"]) == 1)
                {
                    checkImage.IsDisabled = true;
                    checkImage.MicrErrorMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.MicrCountError;
                }
                else
                {
                    checkImage.IsDisabled = false;
                    checkImage.MicrErrorMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.BadMicrWarning;
                }
            }
            else
            {
                checkImage.IsDisabled = false;
                Session["MicrErrorCount"] = null;
            }
            return View("CheckImage", checkImage);
        }

        [HttpGet]
        public ActionResult ScanACheck()
        {
            CashACheck cashacheck = new CashACheck();
            return View("ScanACheck", cashacheck);
        }

        private ZeoClient.CheckProviderDetails verifyProvider(ZeoClient.MICRDetails micrDetails)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response response = alloyServiceClient.GetCheckProvider(micrDetails, GetZeoContext());
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

            ZeoClient.CheckProviderDetails checkProviderDetails = response.Result as ZeoClient.CheckProviderDetails;

            return checkProviderDetails;
        }
    }
}