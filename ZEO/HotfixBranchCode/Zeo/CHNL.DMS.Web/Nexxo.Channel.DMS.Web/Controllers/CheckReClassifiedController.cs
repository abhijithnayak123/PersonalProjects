using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class CheckReClassifiedController : BaseController
	{
		public ActionResult ReClassified(CheckDeclinedReasons reClassifiedCheck)
		{
			TempData["CheckFrontImage"] = reClassifiedCheck.CheckFrontImage;
			return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reClassifiedCheck"></param>
		/// <returns></returns>
		public ActionResult CancelCheck(string checkId)
		{
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			MGIContext mgiContext = GetCheckLogin(long.Parse(customerSession.CustomerSessionId));
			bool isUpdateStatusOnRemoval = desktop.RemoveCheque(long.Parse(customerSession.CustomerSessionId), long.Parse(checkId), mgiContext);

			if (!isUpdateStatusOnRemoval)
			{
				desktop.RemoveCheckFromCart(long.Parse(customerSession.CustomerSessionId), long.Parse(checkId), mgiContext);
			}
			return RedirectToAction("ProductInformation", "Product");
		}
	}
}
