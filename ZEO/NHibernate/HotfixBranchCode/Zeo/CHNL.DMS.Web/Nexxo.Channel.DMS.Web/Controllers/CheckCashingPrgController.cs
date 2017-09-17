using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Web.Common;
using System.Threading;
using MGI.Channel.DMS.Server.Data;
using System.ServiceModel;
using System.Collections;
using System.IO;
using System.Drawing;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Web.Controllers
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
            checkCashProgress.CheckSubmitted = "true";

            Desktop cashCheck = new Desktop();
            string agentSessionId = ((Hashtable)Session["HTSessions"])["AgentSessionId"].ToString();
            long customerSessionId = GetCustomerSessionId();
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();

            CheckSubmission checkSubmit = new CheckSubmission();
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
            checkSubmit.MicrEntryType = GetMicrEntryType(checkCashProgress, checkSubmit);
            checkCashProgress.customerSession = (CustomerSession)Session["CustomerSession"];
            Check checkStatus = null;
            mgiContext = GetCheckLogin(Convert.ToInt64(checkCashProgress.customerSession.CustomerSessionId));
            checkStatus = cashCheck.SubmitCheck(checkCashProgress.customerSession.CustomerSessionId, checkSubmit, mgiContext);

            var providers = checkCashProgress.Providers.ToList();
            //If chexar changed the check type, the system should show the confirmation screen.
            if (checkStatus.ValidatedType != null && checkStatus.SelectedType != checkStatus.ValidatedType)
            {
                Desktop uClient = new Desktop();
                ModelState.Clear();
                CheckDeclinedReasons checkReClassified = new CheckDeclinedReasons();
                checkReClassified.CheckFrontImage = checkCashProgress.CheckFrontImage;
                checkReClassified.CheckDeclinedReason = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckReClassified;
                checkReClassified.CheckId = checkStatus.Id.ToString();
                checkReClassified.CheckBackImage = checkCashProgress.CheckBackImage;
                checkReClassified.CheckAmount = checkCashProgress.CheckAmount;
                checkReClassified.MICRCode = checkCashProgress.MICRCode;
                checkReClassified.CheckDate = checkCashProgress.CheckDate;
                checkReClassified.CheckEstablishmentFee = checkStatus.Fee.ToString("0.00");
                checkReClassified.LCheckTypes = uClient.GetCheckTypes(customerSessionId, mgiContext);
                checkReClassified.CheckType = checkStatus.ValidatedType;
                checkReClassified.isRepresentable = true;
                checkReClassified.CheckStatus = checkStatus.Status;

                checkReClassified.CheckFrontImage_TIFF = checkCashProgress.CheckFrontImage_TIFF;
                checkReClassified.CheckBackImage_TIFF = checkCashProgress.CheckBackImage_TIFF;
                checkReClassified.PromotionCode = checkCashProgress.PromotionCode;
                checkReClassified.CheckPromotionDetails = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckPromotionDetails;
                checkReClassified.DiscountName = checkCashProgress.DiscountName;
                checkReClassified.DiscountApplied = checkCashProgress.DiscountApplied;
                if (providers.Any(x => x.ProcessorId == (long)ProviderIds.Certegy))
                {
                    if (checkStatus.Fee > checkStatus.SelectedFee)
                    {
                        ViewBag.ModifiedFee = checkStatus.Fee;
                        ViewBag.OriginalFee = checkStatus.SelectedFee;
                    }
                    ViewBag.IsApproved = true;
                }
                return View("CheckReClassified", checkReClassified);
            }
            else if (checkStatus.Status.ToLower() == "approved")
            {
                if (providers.Any(x => x.ProcessorId == (long)ProviderIds.Certegy))
                {
                    TempData["CheckStatus"] = checkStatus.Status;
                    TempData["IsReclassify"] = false;
                }
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


                Desktop uClient = new Desktop();
                checkDeclined.LCheckTypes = uClient.GetCheckTypes(customerSessionId, mgiContext);

                checkDeclined.LCheckTypes.First<SelectListItem>().Text = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.SelectCheckType;

                checkDeclined.isRepresentable = true;

                checkDeclined.CheckStatus = checkStatus.Status;

                //All declined checks are hard declined as of now. Change this logic whenever soft decline introduced.
                checkDeclined.isRepresentable = false;
                checkDeclined.PromotionCode = checkCashProgress.PromotionCode;
                checkDeclined.PrintReceiptOnDecline = false;
                if (providers.Any(x => x.ProcessorId == (long)ProviderIds.Certegy))
                {
                    if (NexxoUtil.GetCertegyDeclineCodes().Any(x => x.Equals(checkStatus.DeclineCode)))
                        checkDeclined.PrintReceiptOnDecline = true;
                }
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
                CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
                mgiContext = GetCheckLogin(Convert.ToInt64((customerSession.CustomerSessionId)));
                Check checkStatusEx = cashCheck.GetCheckStatus(customerSession.CustomerSessionId.ToString(), checkStatus.Id.ToString(), mgiContext);
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
        

		private int GetMicrEntryType(CheckCashingProgress checkCashProgress, CheckSubmission checkSubmit)
		{
			int entryType = 1;
			ChannelPartnerProductProvider certegyProvider = checkCashProgress.channelPartner.Providers.FirstOrDefault(x => x.ProcessorName == "Certegy" && x.ProductName == "ProcessCheck");
			if (certegyProvider != null)
			{
				entryType = (int)certegyProvider.CheckEntryType;
			}
			return entryType;
		}
	}
}
