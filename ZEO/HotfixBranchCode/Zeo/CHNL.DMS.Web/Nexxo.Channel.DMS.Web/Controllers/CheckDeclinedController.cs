using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.Common;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class CheckDeclinedController : BaseController
    {
        public ActionResult CheckReSubmit(CheckDeclinedReasons declinedCheck)
        {
             // Redirect to CashA Check - product controller
            if (declinedCheck.Source.ToString() == MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ResubmitButtonText.ToString())
            {                
                //return RedirectToAction("CashACheck", "Product");
                return ReSubmitSoftDeclineCheck(declinedCheck);
            }
            else
            {
                // redirect to customer Home Page/ Product SCreen
                return RedirectToAction("ProductInformation", "Product");
            }
        }


        [CustomHandleErrorAttribute(ViewName = "CheckDeclined", MasterName = "_Common")]
        private ActionResult ReSubmitSoftDeclineCheck(CheckDeclinedReasons declinedCheck)
        {
            //try
            //{
                ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ProcessingCheck;

                CheckCashingProgress chkSubmit = new CheckCashingProgress();
                chkSubmit.CheckAmount = declinedCheck.CheckAmount;
                chkSubmit.MICRCode = declinedCheck.MICRCode;
                chkSubmit.CheckFrontImage = declinedCheck.CheckFrontImage;
                chkSubmit.CheckBackImage = declinedCheck.CheckBackImage;
                chkSubmit.CheckLimit = declinedCheck.CheckLimit;
                chkSubmit.CheckId = declinedCheck.CheckId;
                chkSubmit.NetAmount = declinedCheck.NetAmount;

                chkSubmit.CheckDate = declinedCheck.CheckDate;
                chkSubmit.CheckEstablishmentFee = declinedCheck.CheckEstablishmentFee;
                chkSubmit.CheckType = declinedCheck.CheckType;

				chkSubmit.CheckFrontImage_TIFF = declinedCheck.CheckFrontImage_TIFF;
				chkSubmit.CheckBackImage_TIFF = declinedCheck.CheckBackImage_TIFF;
               
                return View("CheckCashingProgress", chkSubmit);            
            //}
            //catch
            //{
            //    ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ProcessingCheck;
            //    return View("CheckDeclined", declinedCheck);
            //}
        }

    }
}
