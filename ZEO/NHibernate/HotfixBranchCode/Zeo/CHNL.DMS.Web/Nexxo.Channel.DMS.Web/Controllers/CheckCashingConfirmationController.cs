using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Web.Common;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class CheckCashingConfirmationController : BaseController
    {
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName="CheckCashingConfirmation",MasterName="_Common")]
        public ActionResult CheckPost(CheckAmountStatus checkAmtStatus)
        {
            if (checkAmtStatus.Source.ToString() == MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.NextButtonText.ToString())
            {
                CheckCashingProgress checkSubmit = new CheckCashingProgress();
                checkSubmit.CheckAmount = checkAmtStatus.CheckAmount;
                checkSubmit.CheckBackImage = checkAmtStatus.CheckBackImage;
                checkSubmit.CheckFrontImage = checkAmtStatus.CheckFrontImage;
                checkSubmit.MICRCode = checkAmtStatus.MICRCode;
                checkSubmit.CheckLimit = checkAmtStatus.CheckLimit;

                return View("CheckCashingProgress", checkSubmit);
            }
                // this won't be required now
            else
            {
                CheckAmount checkCorrect = new CheckAmount();
                checkCorrect.CheckAmount = checkAmtStatus.CheckAmount;
                checkCorrect.CheckFrontImage = checkAmtStatus.CheckFrontImage;
                checkCorrect.CheckBackImage = checkAmtStatus.CheckBackImage;
                checkCorrect.MICRCode = checkAmtStatus.MICRCode;
                checkCorrect.CheckLimit = checkAmtStatus.CheckLimit;

                return View("CheckAmount", checkCorrect);
            }
        }
    }
}