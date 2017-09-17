using System;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
using System.Linq.Expressions;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    //TODO: Are we using this Controller..??. If not we have to remove this.
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
        /// we are not uing this
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        //[HttpPost]
        //[CustomHandleErrorAttribute(ViewName = "CheckStatus", MasterName = "_Common")]
        //public ActionResult CheckStatus(int page = 1, int rows = 5)
        //{
        //    ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];

        //    //try
        //    //{
        //    var openChecks = new List<Check>().AsQueryable();

        //    IQueryable<Check> filteredchecks = openChecks;

        //    var sortedChecks = filteredchecks;

        //    var totalRecords = filteredchecks.Count();
        //    var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

        //    var data = (from s in sortedChecks
        //                select new
        //                {
        //                    id = s.Id,
        //                    cell = new object[]
        //                    {
        //                        s.Id,
        //                        s.SubmissionDate.ToString("MM/dd/yyyy hh:mm:ss", CultureInfo.CreateSpecificCulture("en-US")),
        //                        s.Amount,
        //                        s.StatusMessage,
        //                        s.Status
        //                    }
        //                }).ToArray();

        //    var jsonData = new
        //    {
        //        total = totalPages,
        //        page = page,
        //        records = totalRecords,
        //        rows = data.Skip((page - 1) * rows).Take(rows)
        //    };

        //    return Json(jsonData);
        //    //}
        //    //catch (SystemException ex)
        //    //{
        //    //    throw ex;
        //    //}
        //}

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
        // this method not used any where
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult AcceptDecline(string id)
        {
            ZeoClient.ZeoContext context = GetCheckLogin();
            ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.Check checkStatus = null;
            try
            {
                CheckStatus checkTemp = new Models.CheckStatus();
                string checkId = checkStatus.Id.ToString();

                if (checkTemp.CustomerSession != null)
                {

                    ZeoClient.Response getStatusResponse = alloyClient.GetCheckStatus(Convert.ToInt64(id), context);
                    if (WebHelper.VerifyException(getStatusResponse)) throw new ZeoWebException(getStatusResponse.Error.Details);
                    checkStatus = getStatusResponse.Result as ZeoClient.Check;
                }
                if (checkStatus.Status.ToLower() == "approved")
                {
                    CheckTransaction checkTransaction = new CheckTransaction();
                    checkTransaction.AmountCredited = checkStatus.Amount;
                    checkTransaction.CheckCashingFee = checkStatus.Fee;
                    checkTransaction.CustomerSession = checkTemp.CustomerSession;
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
                    ViewBag.Navigation = App_GlobalResources.Nexxo.CheckApproved;
                    Session["updateShoppingCart"] = true;
                    return View("ApprovedCheckTransaction", checkTransaction);
                }

                else if (checkStatus.Status.ToLower() == "declined")
                {
                    CheckDeclinedReasons checkDeclinedReasons = new CheckDeclinedReasons();
                    checkDeclinedReasons.CustomerSession = checkTemp.CustomerSession;
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
                    ViewBag.Navigation = App_GlobalResources.Nexxo.CheckDeclined;

                    //removing pending check item if its went to decline.
                    //begin
                    //Response canResubmitResponse = desktop.CanResubmit(GetCustomerSessionId(), checkId, mgiContext);
                    //if (WebHelper.VerifyException(canResubmitResponse)) throw new AlloyWebException(canResubmitResponse.Error.Details);
                    //checkDeclinedReasons.isRepresentable = (bool)canResubmitResponse.Result;
                    ZeoClient.Response response = alloyClient.RemoveCheck(long.Parse(checkId), context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

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
                    checkPending.CheckId = checkStatus.Id.ToString();
                    checkPending.CheckInProcessMessage = checkStatus.StatusMessage;
                    checkPending.CheckInProcessMessageDetails = checkStatus.StatusDescription;
                    ViewBag.Navigation = App_GlobalResources.Nexxo.CheckPending;

                    return View("CheckPending", checkPending);
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }
    }
}
