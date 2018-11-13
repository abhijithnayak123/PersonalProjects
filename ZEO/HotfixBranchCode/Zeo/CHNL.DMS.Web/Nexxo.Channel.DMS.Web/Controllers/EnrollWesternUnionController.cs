using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using System.IO;
using System.Drawing;
using MGI.Channel.Shared.Server.Data;
using CustomerSession = MGI.Channel.Shared.Server.Data.CustomerSession;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class EnrollWesternUnionController : BaseController
	{
		//
		// GET: /EnrollWesternUnion/
		[CustomHandleError(ViewName = "EnrollWesternUnionGoldCard", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.WesternUnionDetails")]
		public ActionResult EnrollWesternUnionGoldCard(string editgoldcardfrom)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			WesternUnionDetails wuDetails = new WesternUnionDetails();
			wuDetails.EditGoldCardFrom = editgoldcardfrom;
			Session["lookupGoldCard"] = false;
			Desktop client = new Desktop();
			CustomerSession customerSeesion = (CustomerSession)Session["CustomerSession"];
			long customerSeesionId = string.IsNullOrEmpty(customerSeesion.CustomerSessionId) ? 0 : Convert.ToInt64(customerSeesion.CustomerSessionId);
			if (customerSeesion.Customer.IsWUGoldCard)
			{
				SharedData.Account senderDetails = client.DisplayWUCardAccountInfo(customerSeesionId, mgiContext);
				wuDetails.WUGoldCardNumber = senderDetails.LoyaltyCardNumber;
			}
			return View("EnrollWesternUnionGoldCard", "_Common", wuDetails);
		}

		[CustomHandleError(ViewName = "EnrollWesternUnionGoldCard", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.WesternUnionDetails")]
		public ActionResult UpdateWUCustomerAccount(string WUGoldCardNumber, string editgoldcardfrom)
		{
			try
			{
				Desktop desktop = new Desktop();
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				CustomerSession customerSession = new CustomerSession();
				customerSession = (CustomerSession)Session["CustomerSession"];
				long customerSeesionId = string.IsNullOrEmpty(customerSession.CustomerSessionId) ? 0 : Convert.ToInt64(customerSession.CustomerSessionId);
				bool IsUpdated = desktop.UpdateCustomerProfile(customerSeesionId, WUGoldCardNumber, mgiContext);

				if (IsUpdated)
				{
					customerSession.Customer.IsWUGoldCard = true;
					Session["CustomerSession"] = customerSession;
				}

				Session["lookupGoldCard"] = false;
				if (editgoldcardfrom == "sendmoney")
				{
					return RedirectToAction("SendMoney", "Product");
				}
				else
				{
					return RedirectToAction("BillPayment", "WesternUnionBillPayment");
				}
			}
			catch (Exception ex)
			{
				WesternUnionDetails wuDetails = new WesternUnionDetails();
				wuDetails.EditGoldCardFrom = editgoldcardfrom;
				ViewBag.IsException = true;
				ViewBag.ExceptionMsg = ex.Message;
                return View("EnrollWesternUnionGoldCard", "_Common", wuDetails);
			}
		}

		public ActionResult RedirectToPage(string editgoldcardfrom)
		{
			if (editgoldcardfrom == "sendmoney")
			{

				return RedirectToAction("SendMoney", "Product");
			}
			else
			{
				return RedirectToAction("BillPayment", "WesternUnionBillPayment");
			}
		}
		public ActionResult SkipGoldCard(string editgoldcardfrom)
		{
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			if (customerSession.Customer.IsWUGoldCard)
				Session["CustomerHasGoldCard"] = "true";
			else
				Session["CustomerHasGoldCard"] = "false";

			if (editgoldcardfrom == "sendmoney")
			{
				TempData["SkipGoldCard"] = "true";
				return RedirectToAction("SendMoney", "Product");
			}
			else
			{
				TempData["SkipGoldCard"] = "true";
				return RedirectToAction("BillPayment", "WesternUnionBillPayment");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sidx"></param>
		/// <param name="sord"></param>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CustomerWUGoldCardNumbersGrid(string sidx, string sord, int page = 1, int rows = 5)
		{
			MGIContext mgiContext = new MGIContext();
			if (Session["lookupGoldCard"] == null || !Convert.ToBoolean(Session["lookupGoldCard"].ToString()))
			{
				var jsonDataEmpty = new
				{
					display = false,
					records = 0
				};

				return Json(jsonDataEmpty, JsonRequestBehavior.AllowGet);
			}

			Desktop client = new Desktop();

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			long customerSessionId = string.IsNullOrEmpty(customerSession.CustomerSessionId) ? 0 : Convert.ToInt64(customerSession.CustomerSessionId);

			CardLookupDetails cardlookupreq = new CardLookupDetails()
			{
				firstname = customerSession.Customer.PersonalInformation.FName,
				lastname = customerSession.Customer.PersonalInformation.LName
			};

			try
			{
				var customers = client.WUCardLookup(customerSessionId, cardlookupreq, mgiContext).AsQueryable();

				var totalRecords = customers.Count();
				var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

				var data = (from s in customers
							select new
							{
								id = s.WUGoldCardNumber,
								cell = new object[] { s.FullName, s.Address, s.ZipCode, s.PhoneNumber, s.WUGoldCardNumber }
							}).ToArray();

				var jsonData = new
				{
					display = true,
					total = totalPages,
					page = page,
					records = totalRecords,
					rows = data.Skip((page - 1) * rows).Take(rows)
				};

				//Session["customersearchprofile"] = true;
				return Json(jsonData, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				throw ex;
			}
			//}
			//catch (Exception ex)
			//{
			//    var jsonDataEmpty = new
			//    {
			//        display = true,
			//        records = 0
			//    };

			//    return Json(jsonDataEmpty, JsonRequestBehavior.AllowGet);
			//}

		}

		[HttpPost]
		[CustomHandleError(ViewName = "EnrollWesternUnionGoldCard", MasterName = "_Common")]
		public ActionResult LookupGoldCard(WesternUnionDetails wuDetails)
		{
			Session["lookupGoldCard"] = true;

			return View("EnrollWesternUnionGoldCard", "_Common", wuDetails);
		}

		[CustomHandleError(ViewName = "EnrollWesternUnionGoldCard", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.WesternUnionDetails")]
		public ActionResult EnrollGoldCard(string editgoldcardfrom)
		{
			try
			{
				Desktop client = new Desktop();
				CustomerSession customerSeesion = (CustomerSession)Session["CustomerSession"];
				WesternUnionDetails wuDetails = new WesternUnionDetails();
				long customerSeesionId = string.IsNullOrEmpty(customerSeesion.CustomerSessionId) ? 0 : Convert.ToInt64(customerSeesion.CustomerSessionId);

				XferPaymentDetails paymentDetails = new XferPaymentDetails();

				MGIContext mgiContext = new MGIContext();

				paymentDetails.DestinationCountryCode = "US";
				paymentDetails.DestinationCurrencyCode = "USD";

                List<SelectListItem> wuCountries = client.GetXfrCountries(customerSeesionId, mgiContext);

				if (customerSeesion.Customer.ID != null && customerSeesion.Customer.ID.Country != null)
				{
					var country = wuCountries.Where(c => c.Text.ToLower() == customerSeesion.Customer.ID.Country.ToLower()).FirstOrDefault();
					string countryCode = string.Empty;
					if (country != null)
						countryCode = country.Value;

				paymentDetails.OriginatingCountryCode = countryCode;
                paymentDetails.OriginatingCurrencyCode = client.GetCurrencyCode(customerSeesionId, countryCode, mgiContext);
				}
				else
				{
					customerSeesion.Customer.ID = new Identification();
				}

				CardDetails cardDetails = client.WUCardEnrollment(customerSeesionId, paymentDetails, mgiContext);

				if (Session["CustomerSession"] != null)
				{
					CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
					customerSeesion.Customer.IsWUGoldCard = !string.IsNullOrEmpty(cardDetails.AccountNumber);
					Session["CustomerSession"] = customerSeesion;
				}

				if (editgoldcardfrom == "sendmoney")
				{
					return RedirectToAction("SendMoney", "Product");
				}
				else
				{
					return RedirectToAction("BillPayment", "WesternUnionBillPayment");
				}
			}
			catch (Exception ex)
			{
				WesternUnionDetails wuDetails = new WesternUnionDetails();
				wuDetails.EditGoldCardFrom = editgoldcardfrom;
				ViewBag.IsException = true;
				ViewBag.ExceptionMsg = ex.Message;
				return View("EnrollWesternUnionGoldCard", "_Common", wuDetails);
			}
		}
	}
}
