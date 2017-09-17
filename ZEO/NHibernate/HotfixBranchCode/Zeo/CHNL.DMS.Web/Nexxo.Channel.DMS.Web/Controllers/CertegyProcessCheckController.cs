using MGI.Channel.DMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Common.Util;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class CertegyProcessCheckController : CashaCheckController
	{
		public ActionResult ProcessCheck()
		{
			Session["activeButton"] = "processcheck";
            Session["MicrErrorCount"] = null;
			ProductInfo productInfo = new ProductInfo();
			Models.CashACheck cashCheck = new CashACheck();

			cashCheck.CheckLimit = productInfo.CheckLimit;
			cashCheck.Processor = cashCheck.Providers.FirstOrDefault(x => x.Name.ToLower() == "processcheck").Name;
			
			ViewBag.Navigation = Resources.NexxoSiteMap.ProcessCheck;
			
			return View("CheckCashing", cashCheck);
		}

		[HttpGet]
		public ActionResult CheckScan()
		{
			Session["activeButton"] = "processcheck";
			ProductInfo productInfo = new ProductInfo();
			CashACheck cashacheck = new CashACheck();
			cashacheck.CheckLimit = productInfo.CheckLimit;			
			var certegyProvider = cashacheck.channelPartner.Providers.FirstOrDefault(x => x.ProcessorName == "Certegy" && x.ProductName=="ProcessCheck");
			if (certegyProvider != null)
			{
				if (certegyProvider.CheckEntryType == CheckEntryTypes.ScanWithImage)
				{
					return RedirectToAction("ScanACheck", "CashaCheck");
				}
				else if (certegyProvider.CheckEntryType == CheckEntryTypes.ScanWithoutImage)
				{
					return View("ScanCheckMICR", cashacheck);
				}
				else
				{
					return View("ManualCheckMCIR", cashacheck); 
				}
			}
			// By default check scanning with image
			return RedirectToAction("ScanACheck", "CashaCheck");
		}

		[HttpPost]
		public ActionResult ScanCheckMICR(CashACheck cashacheck)
		{
            if (cashacheck.MicrError == 1)
            {
                Session["MicrErrorCount"] = Session["MicrErrorCount"] == null ? cashacheck.MicrError : Convert.ToInt32(Session["MicrErrorCount"]) + 1;
                if (Session["MicrErrorCount"] != null && Convert.ToInt32(Session["MicrErrorCount"]) == 1)
                {
                    cashacheck.IsDisabled = true;
                    cashacheck.MicrErrorMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.MicrCountError;
                }
                else
                {
                    cashacheck.IsDisabled = false;
                }
            }
            else
            {
                cashacheck.IsDisabled = false;
                Session["MicrErrorCount"] = null;
            }

			return View("ScanMICR", cashacheck);
		}

		public ActionResult CheckReScan()
		{
			return RedirectToAction("CheckScan", "CertegyProcessCheck");
		}

		[HttpPost]
		public ActionResult CheckDetailsPost(CashACheck cashcheck)
		{
			CheckAmount checkAmount = new CheckAmount();
            Session["MicrErrorCount"] = null;
			checkAmount.RoutingNumber = cashcheck.RoutingNumber;
			checkAmount.AccountNumber = cashcheck.AccountNumber;
			checkAmount.CheckNumber = cashcheck.CheckNumber;
			checkAmount.CheckEstablishmentFee = "";
			checkAmount.NpsId = cashcheck.NpsId ?? string.Empty;
			checkAmount.MICRCode = cashcheck.MICRCode ?? string.Empty;
			ModelState.Clear();
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			long customerSessionId = GetCustomerSessionId();
			checkAmount.LCheckTypes = desktop.GetCheckTypes(customerSessionId, mgiContext);

			checkAmount.LCheckTypes.First<SelectListItem>().Text = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.SelectCheckType;

			return View("CheckAmount", checkAmount);
		}
		public ActionResult RemoveCheckPopUp(string transactionId)
		{
			ParkTransaction parkTransaction = new ParkTransaction();
			parkTransaction.TransactionId =Convert.ToInt64(transactionId);
			return PartialView("_CertegyCheckPopUp", parkTransaction);
		}
		public ActionResult DeleteCheckPopUp(string transactionId)
		{
			ViewBag.transactionId = transactionId;
			return PartialView("_CertegyCheckRemove");
			
		}

		public ActionResult RemoveCheckFromCart(string transactionId)
		{
			CustomerSession currentCustomer = (CustomerSession)Session["CustomerSession"];

			long customerSessionId = Convert.ToInt64(currentCustomer.CustomerSessionId);
			Desktop desktop = new Desktop();
			long checkId = Convert.ToInt64(transactionId);
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			desktop.RemoveCheckFromCart(customerSessionId, checkId, mgiContext);
			return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
		}
	}
}
