using System;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion
namespace TCF.Channel.Zeo.Web.Controllers
{
    public class ApprovedCheckTransactionController : BaseController
    {
        /// <summary>
        /// We are not using this method
        /// </summary>
        /// <param name="checkTransact"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AcceptFee(Models.CheckTransaction checkTransact)
        {
            //If condition Added for CancelTransaction popup
            if (checkTransact.Source == null && checkTransact.Source != TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.AcceptFeeButton.ToString())
            {
                checkTransact.Source = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CancelTransactionText.ToString();
            }

            if (checkTransact.Source.ToString() == TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.AcceptFeeButton.ToString())
            {
                if (checkTransact.NetAmount <= 0)
                {
                    ViewBag.Navigation = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.AcceptFeeButton;
                    ViewBag.BalanceZeroMessage = "Net to customer amount should be greater than zero.";
                    return View("ApprovedCheckTransaction", checkTransact);
                }

                TempData["CheckFrontImage"] = checkTransact.CheckFrontImage;
                return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
            }
            else if (checkTransact.Source.ToString() == TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CancelTransactionText.ToString())
            {
                try
                {
                    ZeoClient.ZeoContext context = GetCheckLogin();
                    //Response response = null;// desktop.RemoveCheque(long.Parse(checkTransact.customerSession.CustomerSessionId), long.Parse(checkTransact.CheckId), mgiContext);
                    //if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
                    CheckDetails checkCancel = new CheckDetails();
                    checkCancel.CheckFrontImage = checkTransact.CheckFrontImage;
                    checkCancel.CheckLimit = checkTransact.CheckLimit;
                    checkCancel.CheckId = checkTransact.CheckId;

                    ViewBag.Navigation = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CheckCancelled;

                    return View("CheckCancel", checkCancel);
                }
                catch (Exception ex)
                {
                    CheckDetails checkCancel = new CheckDetails();
                    checkCancel.CheckFrontImage = checkTransact.CheckFrontImage;
                    checkCancel.CheckLimit = checkTransact.CheckLimit;
                    ViewBag.Navigation = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CheckCancelled;
                    VerifyException(ex);
                    return View("CheckCancel", checkCancel);
                }
            }
            ViewBag.Navigation = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CheckApproved;
            return View("ApprovedCheckTransaction", checkTransact);
        }

        /// <summary>
        /// We are not using this method
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="checkId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AcceptFee(string customerSessionId, string checkId)
        {
            try
            {
                ZeoClient.ZeoContext context =  GetCheckLogin();
                //Response response = desktop.RemoveCheque(long.Parse(customerSessionId), long.Parse(checkId), mgiContext);
                //if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
                return new JsonResult();
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }
    }
}