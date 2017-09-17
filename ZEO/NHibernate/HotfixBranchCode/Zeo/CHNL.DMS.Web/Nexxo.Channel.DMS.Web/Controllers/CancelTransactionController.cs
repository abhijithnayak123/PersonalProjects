using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
namespace MGI.Channel.DMS.Web.Controllers
{
	public class CancelTransactionController : Controller
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
				AddlDetails = str[3]
			};

			ViewBag.IsException = false;
			ViewBag.ExceptionMsg = null;

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
			ViewBag.ExceptionMsg = null;

			return PartialView("_ActOnMyBehalf", sysmsg);
		}

		private string[] splitstring(string message) // Any Other Condition has to be checked ?
		{
			string[] strmessage = null;

			if (message.Contains("|"))
				strmessage = message.Split(new string[] { "|" }, StringSplitOptions.None);
			else
			{
				strmessage = new string[4];
				strmessage[0] = "MGiAlloy";
				strmessage[1] = "0.0";
				strmessage[2] = message;
				strmessage[3] = message;
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
            return PartialView("_CancelTransaction");
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
	}
}