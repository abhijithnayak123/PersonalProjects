using System;
using System.Web.Mvc;
using System.Linq;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using System.Collections.Generic;
using TCF.Zeo.Common.Util;
#endregion


namespace TCF.Channel.Zeo.Web.Controllers
{
    /// <summary>
    /// This class performs a MoneyOrderController Controller.
    /// </summary>
    public class MoneyOrderSetupController : BaseController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult MoneyOrder()
        {
            try
            {
                decimal amount = 0;
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                context.PromotionCode = string.Empty;
                //For highlighting MoneyOrder left menu
                Session["activeButton"] = "moneyorder";
                MoneyOrderSetup moneyOrder = new MoneyOrderSetup();
                ZeoClient.Response response = alloyServiceClient.GetMoneyOrderFee(amount, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.TransactionFee trxfee = (response.Result as List<ZeoClient.TransactionFee>)?.FirstOrDefault();
                moneyOrder.Fee = trxfee.NetFee;
                moneyOrder.BaseFee = trxfee.BaseFee;
                moneyOrder.NetFee = trxfee.NetFee;
                moneyOrder.DiscountApplied = trxfee.DiscountApplied;
                moneyOrder.PromotionCode = trxfee.PromotionCode;
                moneyOrder.DiscountName = trxfee.PromotionCode == string.Empty ? "Not Applicable" : trxfee.PromotionDescription;
                moneyOrder.BaseFeeWithCurrency = "$ " + trxfee.BaseFee == null ? "0" : trxfee.BaseFee.ToString("0.00");
                moneyOrder.DiscountAppliedWithCurrency = "$ " + trxfee.DiscountApplied == null ? "0" : trxfee.DiscountApplied.ToString("0.00");
                moneyOrder.NetFeeWithCurrency = "$ " + trxfee.NetFee == null ? "0" : trxfee.NetFee.ToString("0.00");
                moneyOrder.IsSystemApplied = trxfee.IsSystemApplied;

                return View("MoneyOrderSetup", moneyOrder);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[CustomHandleErrorAttribute(ViewName = "MoneyOrderSetup", MasterName = "_Common")]
		public ActionResult SetUpMoneyOrder(MoneyOrderSetup moneyOrder)
		{
			try
			{
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.MoneyOrder moneyOrderService = new ZeoClient.MoneyOrder();
                ZeoClient.MoneyOrderPurchase moneyOrderPurchase = new ZeoClient.MoneyOrderPurchase();

                moneyOrderPurchase.Amount = moneyOrder.Amount;
                moneyOrderPurchase.Fee = moneyOrder.Fee;
                context.PromotionCode = moneyOrderPurchase.PromotionCode = moneyOrder.PromotionCode;

                if (moneyOrder.PromotionCode?.ToLower() == "other")
                {
                    context.PromotionCode = moneyOrderPurchase.PromotionCode = moneyOrder.ManualPromocode?.ToUpper();
                }
                context.IsSystemApplied = moneyOrderPurchase.IsSystemApplied = moneyOrder.IsSystemApplied;

				ZeoClient.Response response = alloyServiceClient.PurchaseMoneyOrder(moneyOrderPurchase, context);
				if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

				moneyOrderService = response.Result as ZeoClient.MoneyOrder;
				if (moneyOrderService.State == (int)TransactionStatus.Authorized)
				{
					Session["validtionforRefresh"] = 1;

					return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
				}
				else
				{
					return View("MoneyOrderSetup", moneyOrder);
				}
			}
			catch (Exception ex)
			{
				VerifyException(ex); return null;
			}
		}

		/// <summary>
		/// US1799 Targeted promotions for check cashing and money order
		/// New JsonResult method to getFee in MoneyOrder screen after entering promotion Code
		/// </summary>
		/// <param name="promotionCode"></param>
		/// <param name="amount"></param>
		/// <param name="IsSystemApplied"></param>
		/// <returns></returns>
		public JsonResult GetMoneyOrderPromotion(string promotionCode, decimal amount, bool IsSystemApplied = true)
		{
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();

			try
			{
                context.IsSystemApplied = IsSystemApplied;
                ZeoClient.Response response = alloyServiceClient.GetFeeBOnPromoCode(amount, promotionCode, context);
				if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.TransactionFee trxfee = response.Result as ZeoClient.TransactionFee;

                var isValid = true;

                if (trxfee == null)
                    isValid = false;

                var jsonData = new
                {
                    success = isValid,
                    data = trxfee,
                    errorMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.PromotionEligibilityMessage
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
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


		[HttpGet]
		[CustomHandleErrorAttribute(ViewName = "MoneyOrderSetup", MasterName = "_Common")]
		public ActionResult GetMoneyOrderFee(string moneyOrderAmount, string promotionCode, bool IsSystemApplied = true)
		{
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();

            try
            {
                decimal amount = 0;
                decimal.TryParse(moneyOrderAmount, out amount);
                promotionCode = string.IsNullOrEmpty(promotionCode) ? "" : promotionCode;
                context.PromotionCode = promotionCode;
                context.IsSystemApplied = IsSystemApplied;
                context.ProviderId = (int)Helper.ProviderId.Continental;

                ZeoClient.Response response = alloyServiceClient.GetMoneyOrderFee(amount, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                List<ZeoClient.TransactionFee> trxFees = response.Result as List<ZeoClient.TransactionFee>;

                Session["ApplicablePromotion"] = trxFees;
                List<SelectListItem> selectItems = new List<SelectListItem>();

                trxFees = trxFees.FindAll(i => i.PromotionId != 0);

                selectItems.Add(new SelectListItem() { Text = string.Empty, Value = string.Empty });

                if (trxFees.Count > 0)
                    trxFees.ForEach(i => selectItems.Add(new SelectListItem() { Text = i.PromotionCode, Value = i.PromotionCode }));

                selectItems.Add(new SelectListItem() { Text = "Other", Value = "other" });

                return Json(new { success = true, data = selectItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                context.PromotionCode = null;
                context.ProviderId = 0;

                //if (ex.Message.Contains("6015"))
                //{
                //    return Json(
                //        new
                //        {
                //            success = "NotValid",
                //            data = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.PromotionCodeValidation
                //        }, JsonRequestBehavior.AllowGet);
                //}
                //else
                VerifyException(ex); return null;
            }

        }

        public ActionResult CalculateFee(string promocode)
        {
            try
            {
                ZeoClient.TransactionFee trxFee = null;
                List<ZeoClient.TransactionFee> trxFees = Session["ApplicablePromotion"] as List<ZeoClient.TransactionFee>;


                if (string.IsNullOrWhiteSpace(promocode) || promocode.ToUpper() == "SELECT")
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

    }
}
