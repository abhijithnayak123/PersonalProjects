using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using System.Collections;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class ApprovedCheckTransactionController : BaseController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="checkTransact"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult AcceptFee(Models.CheckTransaction checkTransact)
		{
			//If condition Added for CancelTransaction popup
			if (checkTransact.Source == null && checkTransact.Source != MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.AcceptFeeButton.ToString())
			{
				checkTransact.Source = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CancelTransactionText.ToString();
			}

			if (checkTransact.Source.ToString() == MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.AcceptFeeButton.ToString())
			{
				if (checkTransact.NetAmount <= 0)
				{
					ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.AcceptFeeButton;
					ViewBag.BalanceZeroMessage = "Net to customer amount should be greater than zero.";
					return View("ApprovedCheckTransaction", checkTransact);
				}

				TempData["CheckFrontImage"] = checkTransact.CheckFrontImage;
				return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
			}
			else if (checkTransact.Source.ToString() == MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CancelTransactionText.ToString())
			{
				try
				{
					Desktop desktop = new Desktop();
					MGI.Channel.DMS.Server.Data.MGIContext mgiContext = GetCheckLogin(Convert.ToInt64(checkTransact.customerSession.CustomerSessionId));
					desktop.RemoveCheque(long.Parse(checkTransact.customerSession.CustomerSessionId), long.Parse(checkTransact.CheckId), mgiContext);

					CheckDetails checkCancel = new CheckDetails();
					checkCancel.CheckFrontImage = checkTransact.CheckFrontImage;
					checkCancel.CheckLimit = checkTransact.CheckLimit;
					checkCancel.CheckId = checkTransact.CheckId;

					ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckCancelled;

					return View("CheckCancel", checkCancel);
				}
				catch (Exception)
				{
					CheckDetails checkCancel = new CheckDetails();
					checkCancel.CheckFrontImage = checkTransact.CheckFrontImage;
					checkCancel.CheckLimit = checkTransact.CheckLimit;
					//ViewBag.ErrorMessage = ex.Message;
					ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckCancelled;

					return View("CheckCancel", checkCancel);
				}
			}
			ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckApproved;
			return View("ApprovedCheckTransaction", checkTransact);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="checkId"></param>
		/// <returns></returns>
		[HttpGet]
		public JsonResult AcceptFee(string customerSessionId, string checkId)
		{
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = GetCheckLogin(long.Parse(customerSessionId));
			desktop.RemoveCheque(long.Parse(customerSessionId), long.Parse(checkId), mgiContext);

			return new JsonResult();
		}
	}
}