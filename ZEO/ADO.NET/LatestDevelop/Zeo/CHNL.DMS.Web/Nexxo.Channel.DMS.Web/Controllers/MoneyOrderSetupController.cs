using System;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
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
                //For highlighting MoneyOrder left menu
                Session["activeButton"] = "moneyorder";
				MoneyOrderSetup moneyOrder = new MoneyOrderSetup();
                ZeoClient.Response response = alloyServiceClient.GetMoneyOrderFee(amount, context);
				if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.TransactionFee trxfee = response.Result as ZeoClient.TransactionFee;
				moneyOrder.Fee = trxfee.NetFee;
				moneyOrder.BaseFee = trxfee.BaseFee;
				moneyOrder.NetFee = trxfee.NetFee;
				moneyOrder.DiscountApplied = trxfee.DiscountApplied;
				moneyOrder.PromotionCode = trxfee.DiscountName;
				moneyOrder.DiscountName = trxfee.DiscountName == string.Empty ? "Not Applicable" : trxfee.DiscountDescription;
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
                context.PromotionCode = promotionCode;
                context.IsSystemApplied = IsSystemApplied;
                ZeoClient.Response response = alloyServiceClient.GetMoneyOrderFee(amount, context);
				if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                ZeoClient.TransactionFee trxfee = response.Result as ZeoClient.TransactionFee;
				return Json(
						new
						{
							success = true,
							data = trxfee
						}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
                context.PromotionCode = null;

                if (ex.Message.Contains("6015"))
					return Json(
							new
							{
								success = "NotValid",
								data = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.PromotionCodeValidation
							}, JsonRequestBehavior.AllowGet);
				else
					VerifyException(ex); return null;
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
				ZeoClient.Response response = alloyServiceClient.GetMoneyOrderFee(amount, context);
				if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                ZeoClient.TransactionFee trxFee = response.Result as ZeoClient.TransactionFee;
				return Json(new { success = true, data = trxFee }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
                context.PromotionCode = null;

                if (ex.Message.Contains("6015"))
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
