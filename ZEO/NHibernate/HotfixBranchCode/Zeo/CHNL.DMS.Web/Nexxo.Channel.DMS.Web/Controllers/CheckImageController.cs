using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.Common;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class CheckImageController : BaseController
    {

        //[HttpGet]
        //public ActionResult CheckImage()
        //{
        //    CheckCashingProgress chkSubmit = new CheckCashingProgress();
        //    return View("CheckCashingProgress", chkSubmit);
        //}

      
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "CheckImage", MasterName = "_Common")]
        public ActionResult PostCheckImage(CheckImage checkImage)
        {
            long customerSessionId = GetCustomerSessionId();
            CheckAmount checkAmount = new CheckAmount();
            Session["MicrErrorCount"] = null;
			MGIContext mgiContext = new MGIContext();
			checkAmount.CheckFrontImage = checkImage.CheckFrontImage ?? string.Empty;
			checkAmount.CheckBackImage = checkImage.CheckBackImage ?? string.Empty;
			checkAmount.MICRCode = checkImage.MICRCode ?? string.Empty;
			checkAmount.NpsId = checkImage.NpsId ?? string.Empty;

			checkAmount.CheckFrontImage_TIFF = checkImage.CheckFrontImage_TIFF ?? string.Empty;
			checkAmount.CheckBackImage_TIFF = checkImage.CheckBackImage_TIFF ?? string.Empty;
			checkAmount.RoutingNumber = checkImage.RoutingNumber ?? string.Empty;
			checkAmount.AccountNumber = checkImage.AccountNumber ?? string.Empty;
			checkAmount.CheckNumber = checkImage.CheckNumber ?? string.Empty;
            checkAmount.CheckEstablishmentFee = "";

             Desktop uClient = new Desktop();
			 checkAmount.LCheckTypes = uClient.GetCheckTypes(customerSessionId, mgiContext);

             checkAmount.LCheckTypes.First<SelectListItem>().Text = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.SelectCheckType;

            return View("CheckAmount", checkAmount);
        }

    }
}