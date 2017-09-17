using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class EnrollWesternUnionController : BaseController
	{
		//
		// GET: /EnrollWesternUnion/
		[CustomHandleError(ViewName = "EnrollWesternUnionGoldCard", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.WesternUnionDetails")]
		public ActionResult EnrollWesternUnionGoldCard(string editgoldcardfrom)
		{
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                ZeoClient.ZeoContext context = (ZeoClient.ZeoContext)Session["ZeoContext"];
                WesternUnionDetails wuDetails = new WesternUnionDetails();
				wuDetails.EditGoldCardFrom = editgoldcardfrom;
				Session["lookupGoldCard"] = false;

				long customerSessionId = context.CustomerSessionId;
				if (!string.IsNullOrWhiteSpace(context.WUCardNumber))
				{
                    context.ProductType = editgoldcardfrom;
                    wuDetails.WUGoldCardNumber = context.WUCardNumber;
                }

                return View("EnrollWesternUnionGoldCard", "_Common", wuDetails);
			}
			catch (Exception ex)
			{
				VerifyException(ex); return null;
			}
		}

		//[CustomHandleError(ViewName = "EnrollWesternUnionGoldCard", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.WesternUnionDetails")]
		public ActionResult UpdateWUCustomerAccount(string WUGoldCardNumber, string editgoldcardfrom)
		{
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            try
			{
                ZeoClient.ZeoContext context = (ZeoClient.ZeoContext)Session["ZeoContext"];
				context.ProductType = editgoldcardfrom;

                ZeoClient.Response response = alloyServiceClient.UpdateWUAccount(WUGoldCardNumber, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                bool IsUpdated = Convert.ToBoolean(response.Result);
                context.WUCardNumber = WUGoldCardNumber;
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
				VerifyException(ex);
				WesternUnionDetails wuDetails = new WesternUnionDetails();
				wuDetails.EditGoldCardFrom = editgoldcardfrom;
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
            ZeoClient.ZeoContext context = GetZeoContext();

			if (!string.IsNullOrWhiteSpace(context.WUCardNumber))
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
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
			{
                ZeoClient.ZeoContext context = (ZeoClient.ZeoContext)Session["ZeoContext"];
                if (Session["lookupGoldCard"] == null || !Convert.ToBoolean(Session["lookupGoldCard"].ToString()))
				{
					var jsonDataEmpty = new
					{
						display = false,
						records = 0
					};

					return Json(jsonDataEmpty, JsonRequestBehavior.AllowGet);
				}

                ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];

				long customerSessionId = context.CustomerSessionId;

                ZeoClient.CardLookupDetails cardlookupreq = new ZeoClient.CardLookupDetails()
                {
                    FirstName = customerSession.Customer.FirstName,
                    LastName = customerSession.Customer.LastName
                };

                if (TempData["EditGoldCardFrom"] != null)
				{
                    context.ProductType = Convert.ToString(TempData["EditGoldCardFrom"]);
					TempData.Keep("EditGoldCardFrom");
				}


                ZeoClient.Response response = alloyServiceClient.WUCardLookup(cardlookupreq, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                List<ZeoClient.WUCustomerGoldCardResult> customers = response.Result as List<ZeoClient.WUCustomerGoldCardResult>;

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

				return Json(jsonData, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				VerifyException(ex); return null;
			}

		}

		[HttpPost]
		[CustomHandleError(ViewName = "EnrollWesternUnionGoldCard", MasterName = "_Common")]
		public ActionResult LookupGoldCard(WesternUnionDetails wuDetails)
		{
			Session["lookupGoldCard"] = true;
			TempData["EditGoldCardFrom"] = wuDetails.EditGoldCardFrom;
			return View("EnrollWesternUnionGoldCard", "_Common", wuDetails);
		}

		//[CustomHandleError(ViewName = "EnrollWesternUnionGoldCard", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.WesternUnionDetails")]
		public ActionResult EnrollGoldCard(string editgoldcardfrom)
		{
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
			{
                ZeoClient.ZeoContext context = (ZeoClient.ZeoContext)Session["ZeoContext"];
                context.ProductType = editgoldcardfrom;

                WesternUnionDetails wuDetails = new WesternUnionDetails();

                ZeoClient.Response response = alloyServiceClient.WUCardEnrollment(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.CardDetails cardDetails = response.Result as ZeoClient.CardDetails;

                if(cardDetails != null)
                    context.WUCardNumber = cardDetails.AccountNumber;

                Session["AlloyContext"] = context;

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
				VerifyException(ex);
				return View("EnrollWesternUnionGoldCard", "_Common", wuDetails);
			}
		}
	}
}
