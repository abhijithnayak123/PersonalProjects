using System;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
using System.Collections;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion
namespace TCF.Channel.Zeo.Web.Controllers
{
    public class CheckCashingPrgController : BaseController
    {
        public ActionResult CheckStatusProcessor()
        {
            CheckCashingProgress checkCashProgress = new CheckCashingProgress();
            return View("CheckCashingProgress", checkCashProgress);
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "CheckCashingProgress", MasterName = "_Common")]
        public ActionResult CheckStatusProcessor(CheckCashingProgress checkCashProgress)
        {
            try
            {
                checkCashProgress.CheckSubmitted = "true";
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                string agentSessionId = ((Hashtable)Session["HTSessions"])["AgentSessionId"].ToString();
                long customerSessionId = GetCustomerSessionId();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.CheckSubmission checkSubmit = new ZeoClient.CheckSubmission();
                checkSubmit.Amount = checkCashProgress.CheckAmount;
                checkSubmit.ImageFormat = "TIFF";
                checkSubmit.FrontImageTIFF = checkCashProgress.CheckFrontImage_TIFF == null ? null : Convert.FromBase64String(checkCashProgress.CheckFrontImage_TIFF);
                checkSubmit.FrontImage = checkCashProgress.CheckFrontImage == null ? null : Convert.FromBase64String(checkCashProgress.CheckFrontImage);
                checkSubmit.BackImage = checkCashProgress.CheckBackImage == null ? null : Convert.FromBase64String(checkCashProgress.CheckBackImage);
                checkSubmit.BackImageTIFF = checkCashProgress.CheckBackImage_TIFF == null ? null : Convert.FromBase64String(checkCashProgress.CheckBackImage_TIFF);

                checkSubmit.CheckType = checkCashProgress.CheckType;
                checkSubmit.Fee = decimal.Parse(checkCashProgress.CheckEstablishmentFee);
                checkSubmit.IssueDate = DateTime.Parse(checkCashProgress.CheckDate);

                checkSubmit.MICR = checkCashProgress.MICRCode ?? string.Empty;
                checkSubmit.PromoCode = checkCashProgress.PromotionCode;
                checkSubmit.IsSystemApplied = checkCashProgress.IsSystemApplied;
                checkSubmit.AccountNumber = checkCashProgress.AccountNumber ?? string.Empty;
                checkSubmit.RoutingNumber = checkCashProgress.RoutingNumber ?? string.Empty;
                checkSubmit.CheckNumber = checkCashProgress.CheckNumber ?? string.Empty;
                checkSubmit.MicrEntryType = GetMicrEntryType(checkCashProgress);
                checkSubmit.FeeAdjustmentId = checkCashProgress.FeeAdjustmentId;
                checkSubmit.DiscountDescription = checkCashProgress.DiscountName;
                checkSubmit.BaseFee = checkCashProgress.BaseFee;
                checkSubmit.AdditionalFee = checkCashProgress.AdditionalFee;
                checkSubmit.DiscountApplied = checkCashProgress.DiscountApplied;

                ZeoClient.Check checkStatus = null;
                context = GetCheckLogin();

                response = alloyServiceClient.SubmitCheck(checkSubmit, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                checkStatus = response.Result as ZeoClient.Check;

                var providers = checkCashProgress.Providers.ToList();
                //If chexar changed the check type, the system should show the confirmation screen.
                if (checkStatus.ValidatedType != null && checkStatus.SelectedType != checkStatus.ValidatedType)
                {
                    ModelState.Clear();
                    CheckDeclinedReasons checkReClassified = new CheckDeclinedReasons();
                    checkReClassified.CheckFrontImage = checkCashProgress.CheckFrontImage;
                    checkReClassified.CheckDeclinedReason = App_GlobalResources.Nexxo.CheckReClassified;
                    checkReClassified.CheckId = checkStatus.Id.ToString();
                    checkReClassified.CheckBackImage = checkCashProgress.CheckBackImage;
                    checkReClassified.CheckAmount = checkCashProgress.CheckAmount;
                    checkReClassified.MICRCode = checkCashProgress.MICRCode;
                    checkReClassified.CheckDate = checkCashProgress.CheckDate;
                    checkReClassified.CheckEstablishmentFee = checkStatus.Fee.ToString("0.00");
                    checkReClassified.LCheckTypes = GetCheckTypes();
                    checkReClassified.CheckType = checkStatus.ValidatedType;
                    checkReClassified.isRepresentable = true;
                    checkReClassified.CheckStatus = checkStatus.Status;
                    checkReClassified.CheckTypeName = checkReClassified.LCheckTypes.FirstOrDefault(i => i.Value == checkStatus.ValidatedType).Text;
                    checkReClassified.CheckFrontImage_TIFF = checkCashProgress.CheckFrontImage_TIFF;
                    checkReClassified.CheckBackImage_TIFF = checkCashProgress.CheckBackImage_TIFF;
                    checkReClassified.CheckPromotionDetails = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CheckPromotionDetails;

                    // If the check is reclassified, Below details may get changed.

                    checkReClassified.DiscountName = checkStatus.DiscountDescription;
                    checkReClassified.DiscountApplied = checkStatus.DiscountApplied;
                    checkReClassified.PromotionCode = checkStatus.DiscountName;

                    return View("CheckReClassified", checkReClassified);
                }
                else if (checkStatus.Status.ToLower() == "approved")
                { 
                    Session["validtionforRefresh"] = 1;
                    TempData["IsReclassify"] = false;
                    return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
                }
                //Declined check
                else if (checkStatus.Status.ToLower() == "declined")
                {
                    CheckDeclinedReasons checkDeclined = new CheckDeclinedReasons();
                    checkDeclined.CheckFrontImage = checkCashProgress.CheckFrontImage;
                    checkDeclined.CheckDeclinedReason = checkStatus.StatusMessage;
                    checkDeclined.CheckDeclinedReasonDetails = checkStatus.StatusDescription;
                    checkDeclined.CheckId = checkStatus.Id.ToString();

                    checkDeclined.CheckBackImage = checkCashProgress.CheckBackImage;
                    checkDeclined.CheckAmount = checkCashProgress.CheckAmount;
                    checkDeclined.CheckEstablishmentFee = checkCashProgress.CheckEstablishmentFee;
                    checkDeclined.CheckDate = checkCashProgress.CheckDate;
                    checkDeclined.CheckType = checkCashProgress.CheckType;

                    checkDeclined.CheckFrontImage_TIFF = checkCashProgress.CheckFrontImage_TIFF;
                    checkDeclined.CheckBackImage_TIFF = checkCashProgress.CheckBackImage_TIFF;

                    ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
                    checkDeclined.LCheckTypes = GetCheckTypes();

                    checkDeclined.LCheckTypes.First<SelectListItem>().Text = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.SelectCheckType;

                    checkDeclined.isRepresentable = true;

                    checkDeclined.CheckStatus = checkStatus.Status;

                    //All declined checks are hard declined as of now. Change this logic whenever soft decline introduced.
                    checkDeclined.isRepresentable = false;
                    checkDeclined.PromotionCode = checkCashProgress.PromotionCode;
                    checkDeclined.PrintReceiptOnDecline = false;                   
                    return View("CheckDeclined", checkDeclined);
                }
                //Pending check
                else
                {
                    CheckPending checkPending = new CheckPending();
                    checkPending.CheckId = checkStatus.Id.ToString();
                    checkPending.CheckFrontImage = checkCashProgress.CheckFrontImage;
                    checkPending.CheckBackImage = checkCashProgress.CheckBackImage;
                    checkPending.CheckInProcessMessage = checkStatus.StatusMessage;
                    checkPending.CheckInProcessMessageDetails = checkStatus.StatusDescription;
                    //--- get waiting time 
                   
                    context = GetCheckLogin();

                    response = alloyServiceClient.GetCheckStatus(Convert.ToInt64(checkStatus.Id), context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    ZeoClient.Check checkStatusEx = response.Result as ZeoClient.Check;
                    if (checkStatusEx.Status == "Pending")
                    {
                        checkPending.CheckInProcessMessageDetails = "Waiting Time " + checkStatusEx.StatusDescription;
                    }
                    else
                    {
                        if (checkStatusEx.DeclineCode == 10)
                        {
                            checkPending.CheckInProcessMessageDetails = "Duplicate Check";
                        }
                    }
                    checkPending.NpsId = checkCashProgress.NpsId;

                    checkPending.CheckFrontImage_TIFF = checkCashProgress.CheckFrontImage_TIFF;
                    checkPending.CheckBackImage_TIFF = checkCashProgress.CheckBackImage_TIFF;
                    checkPending.PromotionCode = checkCashProgress.PromotionCode;
                    checkPending.DiscountName = checkCashProgress.DiscountName;
                    checkPending.DiscountApplied = checkCashProgress.DiscountApplied;
                    checkPending.IsSystemApplied = checkCashProgress.IsSystemApplied;
                    return View("CheckPending", checkPending);
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        private int GetMicrEntryType(CheckCashingProgress checkCashProgress)
        {
            int entryType = 1;
            ////ChannelPartnerProductProvider certegyProvider = checkCashProgress.channelPartner.Providers.FirstOrDefault(x => x.ProcessorName == "Certegy" && x.ProductName == "ProcessCheck");
            ////if (certegyProvider != null)
            ////{
            ////    entryType = (int)certegyProvider.CheckEntryType;
            ////}
            return entryType;
        }
    }
}
