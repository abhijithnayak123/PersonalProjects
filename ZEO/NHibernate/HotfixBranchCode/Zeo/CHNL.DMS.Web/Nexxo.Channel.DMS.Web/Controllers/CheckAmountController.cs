using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using TransactionFee = MGI.Channel.Shared.Server.Data.TransactionFee;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class CheckAmountController : BaseController
	{
		// Abey : Adding the get method as part of fix - DE1087
		[HttpGet]
		[CustomHandleErrorAttribute(ViewName = "CheckCashingProgress", MasterName = "_Common")]
		public ActionResult CheckAmount()
		{
			CheckCashingProgress chkSubmit = new CheckCashingProgress();
			return View("CheckCashingProgress", chkSubmit);
		}
		// - End changes




		[HttpPost]
		[CustomHandleErrorAttribute(ViewName = "CheckAmount", MasterName = "_Common")]
		public ActionResult CheckAmount(CheckAmount checkAmountModel)
		{
			ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckAmountTitle;

			//try
			//{
			//if (checkAmountModel.CheckLimit >= checkAmountModel.CheckAmount)
			//{
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
                chkSubmit.DiscountApplied=checkAmountModel.DiscountApplied;
                chkSubmit.DiscountName=checkAmountModel.DiscountName;
                chkSubmit.PromotionCode=checkAmountModel.PromotionCode;

			//return RedirectToAction("CheckStatusProcessor", "CheckCashingPrg");
			ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ProcessingCheck;
			return View("CheckCashingProgress", chkSubmit);
			//}
			//else
			//{
			//    ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckAmountTitle;
			//    ViewBag.ExceptionMessage = string.Format("Sorry, We are Unable to Process Your Check. You can Submit a Check up to ${0} at this time.", checkAmountModel.CheckLimit.ToString("0.00"));
			//    return View("CheckAmount",checkAmountModel); 
			////}
			//}
			//catch
			//{
			//    ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckAmountTitle;
			//    return View("CheckAmount", checkAmountModel);
			//}
		}

		[HttpGet]
		[CustomHandleErrorAttribute(ViewName = "CheckAmount", MasterName = "_Common")]
		public ActionResult GetCheckFee(string CheckTypeId, string checkAmount, string promotionCode, bool IsSystemApplied = true)
		{
			try
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				Desktop uClient = new Desktop();
				CustomerSession customerSession = new CustomerSession();
				customerSession = (CustomerSession)Session["CustomerSession"];

				decimal amount = decimal.Parse(checkAmount);
				string nCheckTypeId = CheckTypeId;
				promotionCode = string.IsNullOrEmpty(promotionCode) ? "" : promotionCode;

				CheckSubmission checkSubmit = new CheckSubmission();
				checkSubmit.IsSystemApplied = IsSystemApplied;
				checkSubmit.CheckType = nCheckTypeId;
				checkSubmit.Amount = amount;
				checkSubmit.PromoCode = promotionCode;

				TransactionFee trxFee = uClient.GetCheckFee(customerSession.CustomerSessionId, checkSubmit, mgiContext);

				decimal feeValue = trxFee.NetFee;
				string strFeeValue = feeValue.ToString("0.00");
				return Json(new { success = true, data = trxFee }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("3017"))
				{
					return Json(
						new
						{
							success = "NotValid",
							data = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PromotionCodeValidation
						}, JsonRequestBehavior.AllowGet);
				}
				else
					throw ex;
			}

		}


	}
}
