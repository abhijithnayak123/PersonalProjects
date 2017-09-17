using TCF.Channel.Zeo.Web.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion


namespace TCF.Channel.Zeo.Web.Common
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class NoDirectAccessAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipNoDirectAccessAttribute), true).Any() ||
				 filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(SkipNoDirectAccessAttribute), true).Any())
				return;
            /*As part of AL-1317, we have made these changes and will call this method if we change the URL manually. After analysis and discussion, we have concluded that
            we are not calling this method as we are handling this scenario in javascript*/
            if (filterContext.HttpContext.Request.UrlReferrer == null ||
						filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
			{
				NLogHelper.Info("Action Name : " + filterContext.ActionDescriptor.ActionName);
				NLogHelper.Info("Controller Name : "+filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
				//As part of the AL-1317, we are parking all the transactions. Is this the requirement..??
				ParkTransactions(filterContext);
			}
		}

		private static void ParkTransactions(ActionExecutingContext filterContext)
		{
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = (ZeoClient.ZeoContext)HttpContext.Current.Session["ZeoContext"];
            ZeoClient.Response response;
            ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)HttpContext.Current.Session["CustomerSession"];

			if (customerSession != null && (customerSession.Customer.ProfileStatus.ToString() == Helper.ProfileStatus.Active.ToString() || customerSession.Customer.ProfileStatus.ToString() == ZeoClient.HelperProfileStatus.Inactive.ToString()))
			{
                response = alloyServiceClient.GetShoppingCart(context.CustomerSessionId,context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.ShoppingCart shoppingCart = response.Result as ZeoClient.ShoppingCart;

                ShoppingCartSummary shoppingCartSummary = ShoppingCartHelper.ShoppingCartSummary(shoppingCart);
				if (shoppingCartSummary.Items.Count > 0)
				{
					foreach (var item in shoppingCart.Checks)
					{
						try
						{
                            ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.Checks,context);
						}
						catch { }
					}

					foreach (var item in shoppingCart.Bills)
					{
						try
						{
                            ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.BillPay, context);
                        }
                        catch { }
					}

					foreach (var item in shoppingCart.MoneyTransfers)
					{
						try
						{
							if (item.TransferType == (int)Helper.MoneyTransferType.Send)
								ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.SendMoney, context);
							else if (item.TransferType == (int)Helper.MoneyTransferType.Receive)
								ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.ReceiveMoney, context);
						}
						catch { }
					}

					foreach (var item in shoppingCart.MoneyOrders)
					{
						try
						{
							ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.MoneyOrder, context);
						}
						catch { }
					}

					foreach (var item in shoppingCart.GPRCards)
					{
						try
						{
							if (item.ItemType == Helper.FundType.Credit.ToString())
								ShoppingCartHelper.ParkShoppingCartTrx(long.Parse(item.Id), ProductType.GPRLoad, context);
							else if (item.ItemType == Helper.FundType.Debit.ToString())
								ShoppingCartHelper.ParkShoppingCartTrx(long.Parse(item.Id), ProductType.GPRWithdraw, context);
							else if (item.ItemType == Helper.FundType.Activation.ToString())
								ShoppingCartHelper.ParkShoppingCartTrx(long.Parse(item.Id), ProductType.GPRActivation, context);
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
