using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MGI.Channel.DMS.Web.Common
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class NoDirectAccessAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipNoDirectAccessAttribute), true).Any() ||
				 filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(SkipNoDirectAccessAttribute), true).Any())
				return;

			if (filterContext.HttpContext.Request.UrlReferrer == null ||
						filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
			{
				ParkTransactions(filterContext);
			}
		}

		private static void ParkTransactions(ActionExecutingContext filterContext)
		{
			CustomerSession customerSession = (CustomerSession)HttpContext.Current.Session["CustomerSession"];

			if (customerSession != null && (customerSession.ProfileStatus == ProfileStatus.Active || customerSession.ProfileStatus == ProfileStatus.Inactive))
			{
				Desktop desktop = new Desktop();

				ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

				ShoppingCartSummary shoppingCartSummary = ShoppingCartHelper.ShoppingCartSummary(shoppingCart);

				if (shoppingCartSummary.Items.Count > 0)
				{
					foreach (var item in shoppingCart.Checks)
					{
						try
						{
							ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.Checks);
						}
						catch { }
					}

					foreach (var item in shoppingCart.Bills)
					{
						try
						{
							ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.BillPay);
						}
						catch { }
					}

					foreach (var item in shoppingCart.MoneyTransfers)
					{
						try
						{
							if (item.TransferType == (int)TransferType.SendMoney)
								ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.SendMoney);
							else if (item.TransferType == (int)TransferType.RecieveMoney)
								ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.ReceiveMoney);
						}
						catch { }
					}

					foreach (var item in shoppingCart.MoneyOrders)
					{
						try
						{
							ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.MoneyOrder);
						}
						catch { }
					}

					foreach (var item in shoppingCart.GprCards)
					{
						try
						{
							if (item.ItemType == Constants.PREPAID_CARD_LOAD)
								ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.GPRLoad);
							else if (item.ItemType == Constants.PREPAID_CARD_WITHDRAW)
								ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.GPRWithdraw);
							else if (item.ItemType == Constants.PREPAID_CARD_ACTIVATE)
								ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.GPRActivation);
						}
						catch { }
					}
				}

				filterContext.Result = new RedirectToRouteResult(new
							RouteValueDictionary(new { controller = "SSO", action = "Logout", area = "" }));
			}
			else
			{
				filterContext.Result = new RedirectToRouteResult(new
							RouteValueDictionary(new { controller = "SSO", action = "Logout", area = "" }));
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class SkipNoDirectAccessAttribute : ActionFilterAttribute
	{

	}
}
