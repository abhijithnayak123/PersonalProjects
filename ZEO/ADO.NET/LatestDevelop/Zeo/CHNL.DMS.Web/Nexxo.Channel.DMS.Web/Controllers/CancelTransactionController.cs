using System;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    [SkipNoDirectAccess]
    public class CancelTransactionController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AgentHome()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerProduct()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelTransaction()
        {
            return PartialView("_CancelTransaction");
        }

        public ActionResult DeleteCartItem(string id, string screenName, string product, string status = "")
        {
            ViewBag.deleteCartId = id;
            ViewBag.screenName = screenName;
            ViewBag.product = product;
            if (status == "pending")
            {
                return PartialView("_partialRemoveCheckAlert");
            }
            else
            {
                return PartialView("_DeleteCartItem");
            }
        }

        public ActionResult CancelPopTransaction()
        {
            return PartialView("_CancelReceiverPopupTransaction");
        }

        public ActionResult CancelToSaveReceiverPopup()
        {
            return PartialView("_CancelToSaveReceiverPopup");
        }

        public ActionResult CancelToAcceptFee()
        {
            return PartialView("_CancelTransactionForAcceptFee");
        }

        public ActionResult ShowSwipeMessage()
        {
            return PartialView("_SwipeCard");
        }

        /// <summary>
        /// This is added for User Story # US1956.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult DisplaySystemMessage(long dt, string msg = "")
        {
            if (string.IsNullOrEmpty(msg))
                msg = "Unhandled Error";
            string[] str = splitstring(msg);

            SystemMessage sysmsg = new SystemMessage()
            {
                Type = str[0],
                Number = str[1],
                Message = str[2],
                AddlDetails = str[3],
                ErrorType = str.Count() == 5 ? str[4] : Helper.ErrorType.ERROR.ToString()
            };

            ViewBag.IsException = false;
            ViewBag.ExceptionMessage = null;

            return PartialView("_SystemMessage", sysmsg);
        }

        /// <summary>
        /// This is added for User Story # US1956.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult DisplaySystemMessageForActOnMyBehalf(long dt, string message = "")
        {
            if (string.IsNullOrEmpty(message))
                message = "Unhandled Error";
            string[] strMessageForActOnMyBehalf = splitstring(message);

            SystemMessage sysmsg = new SystemMessage()
            {
                Type = strMessageForActOnMyBehalf[0],
                Number = strMessageForActOnMyBehalf[1],
                Message = strMessageForActOnMyBehalf[2],
                AddlDetails = strMessageForActOnMyBehalf[3]
            };

            ViewBag.IsException = false;
            ViewBag.ExceptionMessage = null;

            return PartialView("_ActOnMyBehalf", sysmsg);
        }

        private string[] splitstring(string message) // Any Other Condition has to be checked ?
        {
            string[] strmessage = null;

            if (message.Contains("|"))
                strmessage = message.Split(new string[] { "|" }, StringSplitOptions.None);
            else
            {
                string errorCode = "1000.100.9999";
                ZeoClient.ZeoContext context = GetZeoContext();
                string errorMessage = GetErrorMessage(errorCode, context);
                strmessage = errorMessage.Split(new string[] { "|" }, StringSplitOptions.None);
                strmessage[2] = message;
            }

            return strmessage;
        }

        public ActionResult ShowSessionWarningPopup()
        {
            return PartialView("_TimeOutPopup");
        }

        public ActionResult CancelMTTransaction(string id, string screenName)
        {
            ViewBag.Id = id;
            ViewBag.ScreenName = screenName;


            return PartialView("_CancelMoneyTransferTransaction");
        }

        /// <summary>
        /// billpayReviewTransaction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CancelBillPayReviewTransaction(long id)
        {
            ViewBag.Id = id;
            return PartialView("_CancelBillPayTransaction");
        }

        public ActionResult DisplayRetryPrintMessage(long dt, string msg, int receiptNo)
        {
            if (string.IsNullOrEmpty(msg))
                msg = "Unhandled Error";

            ViewBag.Message = msg;
            ViewBag.ReceiptNo = receiptNo;

            return PartialView("_partialRetryPrintMessage");
        }

        public ActionResult WarringPopUpToCloseCustomerStatus()
        {
            return PartialView("_partialCustomerStatusAttentionPopup");
        }


        /// <summary>
		/// This is added for User Story # US1956.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public ActionResult DisplaySystemMessageAndRedirect(long dt, string msg = "")
        {
            string[] str = splitstring(msg);
            string controllerName = Convert.ToString(Session["controllerName"]);
            string actionName = Convert.ToString(Session["ActionName"]);


            if (string.IsNullOrWhiteSpace(actionName) || string.IsNullOrWhiteSpace(controllerName))
            {
                return DisplaySystemMessage(dt, msg);
            }
            Session["controllerName"] = Session["ActionName"] = Session["ShouldRedirect"] = null;

            SystemExceptionMessage sysmsg = new SystemExceptionMessage()
            {
                Type = str[0],
                Number = str[1],
                Message = str[2],
                AddlDetails = str[3],
                ErrorType = str.Count() == 5 ? str[4] : Helper.ErrorType.ERROR.ToString(),
                CName = controllerName,
                AName = actionName
            };

            ViewBag.IsException = false;
            ViewBag.ExceptionMessage = null;

            return PartialView("_SystemExceptionMessage", sysmsg);
        }
    }
}
