using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.Shared.Server.Data;
using TransactionFee = MGI.Channel.Shared.Server.Data.TransactionFee;

namespace MGI.Channel.DMS.Web.Controllers
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
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common" ,ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
		public ActionResult MoneyOrder()
		{
			MGIContext mgiContext = new MGIContext();
			Session["activeButton"] = "moneyorder";//For highlighting MoneyOrder left menu
			MoneyOrderSetup moneyOrder = new MoneyOrderSetup();
			Desktop desktop = new Desktop();
			CustomerSession customerSession = new CustomerSession();
			customerSession = (CustomerSession)Session["CustomerSession"];
			decimal amount = 0;
			TransactionFee trxfee = desktop.GetMoneyOrderFee(long.Parse(customerSession.CustomerSessionId), new MGI.Channel.DMS.Server.Data.MoneyOrderData { Amount = amount, PromotionCode = string.Empty, IsSystemApplied = true }, mgiContext);
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
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[CustomHandleErrorAttribute(ViewName = "MoneyOrderSetup", MasterName = "_Common")]
		public ActionResult SetUpMoneyOrder(MoneyOrderSetup moneyOrder)
		{
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			MoneyOrder moneyOrderService = new MoneyOrder();
			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase()
			{
				Amount = moneyOrder.Amount,
				Fee = moneyOrder.Fee,
				PromotionCode = moneyOrder.PromotionCode,
				IsSystemApplied = moneyOrder.IsSystemApplied
			};

			moneyOrderService = desktop.PurchaseMoneyOrder(long.Parse(customerSession.CustomerSessionId), moneyOrderPurchase, mgiContext);

			if (moneyOrderService.Status == Constants.STATUS_AUTHORIZED)
			{
				
				Session["validtionforRefresh"] = 1;
				
				return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
			}
			else
			{
				return View("MoneyOrderSetup", moneyOrder);
			}
		}

		/// <summary>
		/// US1799 Targeted promotions for check cashing and money order
		/// New JsonResult method to getFee in MoneyOrder screen after entering promotion Code
		/// </summary>
		/// <param name="promotionCode"></param>
		/// <param name="Amount"></param>
		/// <param name="IsSystemApplied"></param>
		/// <returns></returns>
        public JsonResult GetMoneyOrderPromotion(string promotionCode, decimal Amount, bool IsSystemApplied=true)
        {
            try
            {
                Desktop desktop = new Desktop();
                CustomerSession customerSession = new CustomerSession();
				MGIContext mgiContext = new MGIContext();
                customerSession = (CustomerSession)Session["CustomerSession"];
                decimal amount = Amount; 
                TransactionFee trxfee = desktop.GetMoneyOrderFee(long.Parse(customerSession.CustomerSessionId), new MGI.Channel.DMS.Server.Data.MoneyOrderData { Amount = amount, PromotionCode = promotionCode , IsSystemApplied= IsSystemApplied}, mgiContext);
                return Json(
						new { 
							success = true,
							data = trxfee 
						}, JsonRequestBehavior.AllowGet);
			}
            catch(Exception ex)
			{
				if (ex.Message.Contains("3017"))
					return Json(
							new { 
								success = "NotValid", 
								data = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PromotionCodeValidation 
							},JsonRequestBehavior.AllowGet);
				else
					throw ex;
			}
		}


		[HttpGet]
		[CustomHandleErrorAttribute(ViewName = "MoneyOrderSetup", MasterName = "_Common")]
		public ActionResult GetMoneyOrderFee(string moneyOrderAmount, string promotionCode, bool IsSystemApplied = true)
		{
			try
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				Desktop desktop = new Desktop();
				CustomerSession customerSession = new CustomerSession();
				customerSession = (CustomerSession)Session["CustomerSession"];

				decimal amount = decimal.Parse(moneyOrderAmount);
				promotionCode = string.IsNullOrEmpty(promotionCode) ? "" : promotionCode;

				MoneyOrderData moneyOrderData = new MoneyOrderData();
				moneyOrderData.IsSystemApplied = IsSystemApplied;
				moneyOrderData.Amount = amount;

				TransactionFee trxFee = desktop.GetMoneyOrderFee(long.Parse(customerSession.CustomerSessionId), moneyOrderData, mgiContext);
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
