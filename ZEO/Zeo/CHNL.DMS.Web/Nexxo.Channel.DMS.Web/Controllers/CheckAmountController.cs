using System;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using System.Collections.Generic;

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class CheckAmountController : BaseController
    {
        // Abey : Adding the get method as part of fix - DE1087
        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "CheckCashingProgress", MasterName = "_Common")]
        public ActionResult CheckAmount()
        {
            try
            {
                CheckCashingProgress chkSubmit = new CheckCashingProgress();
                return View("CheckCashingProgress", chkSubmit);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }
        // - End changes

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "CheckAmount", MasterName = "_Common")]
        public ActionResult CheckAmount(CheckAmount checkAmountModel)
        {
            ViewBag.Navigation = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CheckAmountTitle;

            try
            {
                CheckAmountStatus checkSubmit = new CheckAmountStatus();
                checkSubmit.CheckAmount = checkAmountModel.CheckAmount;
                checkSubmit.MICRCode = checkAmountModel.MICRCode ?? string.Empty;
                checkSubmit.CheckFrontImage = checkAmountModel.CheckFrontImage;
                checkSubmit.CheckBackImage = checkAmountModel.CheckBackImage;
                checkSubmit.CheckLimit = checkAmountModel.CheckLimit;


                // return View("CheckCashingConfirmation",checkSubmit); 

                CheckCashingProgress chkSubmit = new CheckCashingProgress();
                chkSubmit.CheckAmount = checkAmountModel.CheckAmount;
                chkSubmit.MICRCode = checkAmountModel.MICRCode ?? string.Empty;
                chkSubmit.CheckFrontImage = checkAmountModel.CheckFrontImage;
                chkSubmit.CheckBackImage = checkAmountModel.CheckBackImage;
                chkSubmit.CheckLimit = checkAmountModel.CheckLimit;
                chkSubmit.CheckId = checkAmountModel.CheckId;
                chkSubmit.NetAmount = checkAmountModel.NetAmount;

                chkSubmit.CheckDate = checkAmountModel.CheckDate;
                chkSubmit.CheckEstablishmentFee = checkAmountModel.CheckEstablishmentFee;
                chkSubmit.CheckType = checkAmountModel.CheckType;

                chkSubmit.NpsId = checkAmountModel.NpsId;

                chkSubmit.CheckFrontImage_TIFF = checkAmountModel.CheckFrontImage_TIFF;
                chkSubmit.CheckBackImage_TIFF = checkAmountModel.CheckBackImage_TIFF;
                chkSubmit.IsSystemApplied = checkAmountModel.IsSystemApplied;
                chkSubmit.PromotionCode = checkAmountModel.PromotionCode;
                checkSubmit.ManualPromocode = checkAmountModel.ManualPromocode;
                

                chkSubmit.AccountNumber = checkAmountModel.AccountNumber ?? string.Empty;
                chkSubmit.RoutingNumber = checkAmountModel.RoutingNumber ?? string.Empty;
                chkSubmit.CheckNumber = checkAmountModel.CheckNumber ?? string.Empty;
                chkSubmit.DiscountApplied = checkAmountModel.DiscountApplied;
                chkSubmit.DiscountName = checkAmountModel.DiscountName;
                // chkSubmit.PromotionCode = checkAmountModel.PromotionCode;
                chkSubmit.FeeAdjustmentId = checkAmountModel.FeeAdjustmentId;
                chkSubmit.ProductProviderCode = checkAmountModel.ProductProviderCode;

                //return RedirectToAction("CheckStatusProcessor", "CheckCashingPrg");
                ViewBag.Navigation = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.ProcessingCheck;
                return View("CheckCashingProgress", chkSubmit);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

		[HttpGet]
		[CustomHandleErrorAttribute(ViewName = "CheckAmount", MasterName = "_Common")]
		public ActionResult GetCheckFee(string CheckTypeId, string checkAmount, string promotionCode, string productProviderCode, bool IsSystemApplied = true)
		{
			try
			{
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response checkfeeReponse = new ZeoClient.Response();
                context.CustomerSessionId = GetCustomerSessionId();

                decimal amount = decimal.Parse(checkAmount);
                string nCheckTypeId = CheckTypeId;
                promotionCode = string.IsNullOrEmpty(promotionCode) ? "" : promotionCode;

                ZeoClient.CheckSubmission checkSubmit = new ZeoClient.CheckSubmission();
                checkSubmit.IsSystemApplied = IsSystemApplied;
                checkSubmit.CheckType = nCheckTypeId;
                checkSubmit.Amount = amount;
                checkSubmit.PromoCode = promotionCode;
                ZeoClient.HelperProviderId productProviderId;
                Enum.TryParse(productProviderCode, out productProviderId);
                checkSubmit.ProductProviderCode = getProviderId(productProviderId, amount);

                return Json(new { success = true, data = getApplicablePromotion(checkSubmit) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }

        }

        public ActionResult ShowWarrningPopUp()
        {
            return PartialView("_partialShowWarrningPopUp");
        }

        public ActionResult CalculateFee(string promocode)
        {
            try
            {
                ZeoClient.TransactionFee trxFee = null;
                List<ZeoClient.TransactionFee> trxFees = Session["ApplicablePromotion"] as List<ZeoClient.TransactionFee>;


                if (string.IsNullOrWhiteSpace(promocode) || promocode.ToUpper() == "NONE")
                    trxFee = trxFees.Find(i => i.PromotionId == 0);
                else
                    trxFee = trxFees.Find(x => x.PromotionCode == promocode);

                return Json(new { success = true, data = trxFee }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult GetFeeOnPromocode(string checkTypeId, string checkAmount, int productProviderCode, string promotionCode)
        {
            try
            {
                decimal amount = decimal.Parse(checkAmount);
                promotionCode = string.IsNullOrEmpty(promotionCode) ? "" : promotionCode;

                ZeoClient.CheckSubmission checkSubmit = new ZeoClient.CheckSubmission();
                checkSubmit.CheckType = checkTypeId;
                checkSubmit.Amount = amount;


                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                context.PromotionCode = promotionCode;
                ZeoClient.Response checkfeeReponse = new ZeoClient.Response();
                context.ProviderId = productProviderCode; //To be removed -- after we get the provider Id based on the MICR, CheckNumber, Routing number
                checkfeeReponse = alloyServiceClient.GetFeeBasedOnPromoCode(checkSubmit, context);
                if (WebHelper.VerifyException(checkfeeReponse)) throw new ZeoWebException(checkfeeReponse.Error.Details);
                ZeoClient.TransactionFee trxFees = checkfeeReponse.Result as ZeoClient.TransactionFee;
                var isValid = true;

                if (trxFees == null)
                    isValid = false;

                var jsonData = new
                {
                    success = isValid,
                    data = trxFees,
                    errorMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.PromotionEligibilityMessage
                };


                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var jsonData = new
                {
                    success = false,
                    data = "",
                    errorMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.InvalidPromo
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCheckType(int providerId)
        {
            try
            {
                List<SelectListItem> checkTypes = GetCheckTypes((ZeoClient.HelperProviderId)providerId);

                return Json(checkTypes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult VerifyCheckProvider(string checkNumber, string routingNumber, string accountNumber)
        {
            try
            {
                CheckAmount checkAmount = new Models.CheckAmount()
                {
                    RoutingNumber = routingNumber,
                    CheckNumber = checkNumber,
                    AccountNumber = accountNumber
                };
                ZeoClient.CheckProviderDetails checkProviderDetails = verifyProvider(checkAmount);

                var jsondata = new
                {
                    success = true,
                    data = checkProviderDetails.CheckTypeId
                };
                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        private List<SelectListItem> getApplicablePromotion(ZeoClient.CheckSubmission checkSubmit)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.Response checkfeeReponse = new ZeoClient.Response();
            context.ProviderId = (int)checkSubmit.ProductProviderCode; //To be removed -- after we get the provider Id based on the MICR, CheckNumber, Routing number
            checkfeeReponse = alloyServiceClient.GetCheckFee(checkSubmit, context);
            if (WebHelper.VerifyException(checkfeeReponse)) throw new ZeoWebException(checkfeeReponse.Error.Details);
            List<ZeoClient.TransactionFee> trxFees = checkfeeReponse.Result as List<ZeoClient.TransactionFee>;
            Session["ApplicablePromotion"] = trxFees;
            List<SelectListItem> selectItems = new List<SelectListItem>();

            trxFees = trxFees.FindAll(i => i.PromotionId != 0);

            selectItems.Add(new SelectListItem() { Text = string.Empty, Value = string.Empty });

            if (trxFees.Count > 0)
                trxFees.ForEach(i => selectItems.Add(new SelectListItem() { Text = i.PromotionCode, Value = i.PromotionCode, Selected = i.IsGroupPromo }));

            selectItems.Add(new SelectListItem() { Text = "OTHER", Value = "Other" });

            return selectItems;
        }
    }
}
