using System;
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
    }
}