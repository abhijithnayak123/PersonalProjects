using System;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class CheckDeclinedController : BaseController
    {
        [CustomHandleErrorAttribute(ViewName = "CheckDeclined", MasterName = "_Common")]
        public ActionResult CheckReSubmit(CheckDeclinedReasons declinedCheck)
        {
            try
            {
                // Redirect to CashA Check - product controller
                if (declinedCheck.Source.ToString() == App_GlobalResources.Nexxo.ResubmitButtonText.ToString())
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
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }


        [CustomHandleErrorAttribute(ViewName = "CheckDeclined", MasterName = "_Common")]
        private ActionResult ReSubmitSoftDeclineCheck(CheckDeclinedReasons declinedCheck)
        {
            ViewBag.Navigation = App_GlobalResources.Nexxo.ProcessingCheck;

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
        }

    }
}
