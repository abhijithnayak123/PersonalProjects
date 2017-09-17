using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Server.Data;
using System.IO;
using System.Drawing;
using System.Configuration;

namespace MGI.Channel.DMS.Web.Controllers
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
			MGI.Channel.DMS.Server.Data.AgentSession agentSession;

			if (HtLocaluse != null)
			{
				agentSession = ((MGI.Channel.DMS.Server.Data.AgentSession)(HtLocaluse["TempSessionAgent"]));
				if (agentSession.Terminal != null && agentSession.Terminal.PeripheralServer != null)
				{
					if (!string.IsNullOrWhiteSpace(agentSession.Terminal.PeripheralServer.PeripheralServiceUrl))
					{
						peripheralServiceUrl = agentSession.Terminal.PeripheralServer.PeripheralServiceUrl;
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
                    checkImage.MicrErrorMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.MicrCountError;
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