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
    public class CheckImageController : BaseController
    {

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "CheckImage", MasterName = "_Common")]
        public ActionResult PostCheckImage(CheckImage checkImage)
        {
            try
            {
                long customerSessionId = GetCustomerSessionId();
                CheckAmount checkAmount = new CheckAmount();
                Session["MicrErrorCount"] = null;
                ZeoClient.ZeoContext context = GetZeoContext();
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

                checkAmount.ProductProviderCode = checkImage.ProductProviderCode;

                checkAmount.LCheckTypes = GetCheckTypes(checkImage.ProductProviderCode);
 		        checkAmount.LPromotionCodes = new List<SelectListItem>() {
                    new SelectListItem() { Value = string.Empty, Text = string.Empty, Selected = true } };

                if (checkImage.CheckTypeId != 0)
                {
                    //checkAmount.IsCheckTypeEditable = false;
                    checkAmount.CheckType = Convert.ToString(checkImage.CheckTypeId);
                }
                
                checkAmount.LCheckTypes.First<SelectListItem>().Text = "Select";

                return View("CheckAmount", checkAmount);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }
    }
}
