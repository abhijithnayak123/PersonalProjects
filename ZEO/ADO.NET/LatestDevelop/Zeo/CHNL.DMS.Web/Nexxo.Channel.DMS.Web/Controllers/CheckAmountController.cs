using System;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

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
				chkSubmit.AccountNumber = checkAmountModel.AccountNumber ?? string.Empty;
				chkSubmit.RoutingNumber = checkAmountModel.RoutingNumber ?? string.Empty;
				chkSubmit.CheckNumber = checkAmountModel.CheckNumber ?? string.Empty;
				chkSubmit.DiscountApplied = checkAmountModel.DiscountApplied;
				chkSubmit.DiscountName = checkAmountModel.DiscountName;
				chkSubmit.PromotionCode = checkAmountModel.PromotionCode;
                chkSubmit.FeeAdjustmentId = checkAmountModel.FeeAdjustmentId;

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
		public ActionResult GetCheckFee(string CheckTypeId, string checkAmount, string promotionCode, bool IsSystemApplied = true)
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

				checkfeeReponse = alloyServiceClient.GetCheckFee(checkSubmit, context);
				if (WebHelper.VerifyException(checkfeeReponse)) throw new ZeoWebException(checkfeeReponse.Error.Details);
                ZeoClient.TransactionFee trxFee = checkfeeReponse.Result as ZeoClient.TransactionFee;

				decimal feeValue = trxFee.NetFee;
				string strFeeValue = feeValue.ToString("0.00");
				return Json(new { success = true, data = trxFee }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("6016"))
				{
					return Json(
						new
						{
							success = "NotValid",
							data = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.PromotionCodeValidation
						}, JsonRequestBehavior.AllowGet);
				}
				else
					VerifyException(ex); return null;
			}

		}


	}
}
