using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Server.Data;
using System.Collections;
using MGI.Channel.DMS.Web.Common;
using System.ServiceModel;
using MGI.Channel.DMS.Web.ServiceClient;
using System.Reflection;
using System.Linq.Expressions;
using System.Globalization;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class CheckStatusController : BaseController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult CheckStatus()
		{
			CheckStatus checkStatus = new CheckStatus();
			return View("CheckStatus", checkStatus);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		[HttpPost]
		[CustomHandleErrorAttribute(ViewName = "CheckStatus", MasterName = "_Common")]
		public ActionResult CheckStatus(int page = 1, int rows = 5)
		{
			Desktop checkCashClient = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			//try
			//{
			var openChecks = new List<Check>().AsQueryable();

			IQueryable<Check> filteredchecks = openChecks;

			var sortedChecks = filteredchecks;

			var totalRecords = filteredchecks.Count();
			var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

			var data = (from s in sortedChecks
						select new
						{
							id = s.Id,
							cell = new object[] 
                            { 
                                s.Id, 
                                s.SubmissionDate.ToString("MM/dd/yyyy hh:mm:ss", CultureInfo.CreateSpecificCulture("en-US")), 
                                s.Amount, 
                                s.StatusMessage, 
                                s.Status
                            }
						}).ToArray();

			var jsonData = new
			{
				total = totalPages,
				page = page,
				records = totalRecords,
				rows = data.Skip((page - 1) * rows).Take(rows)
			};

			return Json(jsonData);
			//}
			//catch (SystemException ex)
			//{
			//    throw ex;
			//}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="fieldName"></param>
		/// <param name="sortOrder"></param>
		/// <returns></returns>
		private IQueryable<T> SortIQueryable<T>(IQueryable<T> data, string fieldName, string sortOrder)
		{
			if (string.IsNullOrWhiteSpace(fieldName)) return data;
			if (string.IsNullOrWhiteSpace(sortOrder)) return data;

			var param = Expression.Parameter(typeof(T), "i");
			Expression conversion = Expression.Convert(Expression.Property(param, fieldName), typeof(object));
			var mySortExpression = Expression.Lambda<Func<T, object>>(conversion, param);

			return (sortOrder == "desc") ? data.OrderByDescending(mySortExpression)
				: data.OrderBy(mySortExpression);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
		public ActionResult AcceptDecline(string id)
		{
			Desktop desktop = new Desktop();
			MGIContext mgiContext = new MGIContext();
			Check checkStatus = null;
			//try
			//{
			CheckStatus checkTemp = new Models.CheckStatus();
			string checkId = checkStatus.Id.ToString();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			if (checkTemp.customerSession != null)
			{
				mgiContext = GetCheckLogin(Convert.ToInt64(checkTemp.customerSession.CustomerSessionId));
				checkStatus = desktop.GetCheckStatus(checkTemp.customerSession.CustomerSessionId.ToString(), id.ToString(), mgiContext);
			}
			if (checkStatus.Status.ToLower() == "approved")
			{
				MGI.Channel.DMS.Web.Models.CheckTransaction checkTransaction = new MGI.Channel.DMS.Web.Models.CheckTransaction();
				checkTransaction.AmountCredited = checkStatus.Amount;
				checkTransaction.CheckCashingFee = checkStatus.Fee;
				checkTransaction.customerSession = checkTemp.customerSession;
				checkTransaction.CheckId = checkId;
				try
				{
					checkTransaction.CheckFrontImage = Convert.ToBase64String(FileUtility.GetFileData(System.Configuration.ConfigurationManager.AppSettings.Get("TempFilePath") + checkStatus.Id + "Front.txt"));
					checkTransaction.CheckBackImage = Convert.ToBase64String(FileUtility.GetFileData(System.Configuration.ConfigurationManager.AppSettings.Get("TempFilePath") + checkStatus.Id + "Back.txt"));
				}
				catch
				{
					checkTransaction.CheckFrontImage = null;
					checkTransaction.CheckBackImage = null;
				}
				checkTransaction.CheckSubmissionDate = checkStatus.SubmissionDate;
				checkTransaction.NetAmount = checkStatus.Amount + checkStatus.Fee;
				checkTransaction.CheckStatus = checkStatus.Status;
				ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckApproved;
				Session["updateShoppingCart"] = true;
				return View("ApprovedCheckTransaction", checkTransaction);
			}

			else if (checkStatus.Status.ToLower() == "declined")
			{
				CheckDeclinedReasons checkDeclinedReasons = new CheckDeclinedReasons();
				checkDeclinedReasons.customerSession = checkTemp.customerSession;
				checkDeclinedReasons.CheckDeclinedReason = checkStatus.StatusMessage;
				checkDeclinedReasons.CheckDeclinedReasonDetails = checkStatus.StatusDescription;
				try
				{
					checkDeclinedReasons.CheckFrontImage = Convert.ToBase64String(FileUtility.GetFileData(System.Configuration.ConfigurationManager.AppSettings.Get("TempFilePath") + checkStatus.Id + "Front.txt"));
					checkDeclinedReasons.CheckBackImage = Convert.ToBase64String(FileUtility.GetFileData(System.Configuration.ConfigurationManager.AppSettings.Get("TempFilePath") + checkStatus.Id + "Back.txt"));
				}
				catch
				{
					checkDeclinedReasons.CheckFrontImage = null;
					checkDeclinedReasons.CheckBackImage = null;
				}
				checkDeclinedReasons.CheckId = checkId;
				ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckDeclined;

				//removing pending check item if its went to decline.
				//begin
				checkDeclinedReasons.isRepresentable = desktop.CanResubmit(long.Parse(checkTemp.customerSession.CustomerSessionId), checkId, mgiContext);
				desktop.RemoveCheque(long.Parse(checkTemp.customerSession.CustomerSessionId), long.Parse(checkId), mgiContext);
				//end

				return View("CheckDeclined", checkDeclinedReasons);
			}
			else
			{
				CheckPending checkPending = new CheckPending();
				try
				{
					checkPending.CheckFrontImage = Convert.ToBase64String(FileUtility.GetFileData(System.Configuration.ConfigurationManager.AppSettings.Get("TempFilePath") + checkStatus.Id + "Front.txt"));
				}
				catch
				{
					checkPending.CheckFrontImage = null;
				}
				checkPending.CheckId = checkStatus.Id;
				checkPending.CheckInProcessMessage = checkStatus.StatusMessage;
				checkPending.CheckInProcessMessageDetails = checkStatus.StatusDescription;
				ViewBag.Navigation = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckPending;

				return View("CheckPending", checkPending);
			}
			//}
			//catch (Exception exception)
			//{
			//    ViewBag.ExceptionMessage = exception.Message;
			//    return View("ProductInformation", new ProductInfo());
			//}
		}
	}
}