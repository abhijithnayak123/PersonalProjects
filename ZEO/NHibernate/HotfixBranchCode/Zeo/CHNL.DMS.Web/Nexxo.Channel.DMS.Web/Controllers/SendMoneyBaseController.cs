using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class SendMoneyBaseController : BaseController
	{
		[HttpGet]
		public JsonResult GetStates(string countryCode)
		{
			Desktop client = new Desktop();
            long customerSessionId = GetCustomerSessionId();
			MGIContext mgiContext = new MGIContext();
			List<SelectListItem> states = client.GetXfrStates(customerSessionId, countryCode, mgiContext);

			return Json(states, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetCountryCurrencyCode(string countryCode)
		{
			Desktop client = new Desktop();
            long customerSessionId = GetCustomerSessionId();
			MGIContext mgiContext = new MGIContext();
			string currenyCode = client.GetCurrencyCode(customerSessionId, countryCode, mgiContext);
			return Json(currenyCode, JsonRequestBehavior.AllowGet);
		}


		[HttpGet]
		public JsonResult AutoCompleteReceiver(string term)
		{
			Desktop desktop = new Desktop();
			List<Receiver> receivers = new List<Receiver>();
			List<string> receiverFullNames = new List<string>();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			try
			{
				long customerSessionId = GetCustomerSessionId();

				receivers = desktop.GetReceivers(Convert.ToInt64(customerSessionId), term, mgiContext);
				receivers = receivers.FindAll(c => c.Status == "Active");
				foreach (var receiver in receivers)
				{
					receiverFullNames.Add(String.Format("{0} {1}", receiver.FirstName, receiver.LastName));
				}
				return Json(receiverFullNames, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return Json(receiverFullNames, JsonRequestBehavior.AllowGet);
			}
		}
 
		public JsonResult GetReceiverByFullName(string fullName)
		{
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			long customerSessionId = GetCustomerSessionId();
			Receiver receiver = desktop.GetReceiverByFullName(customerSessionId, fullName, mgiContext);
			long receiverId = receiver == null ? 0 : receiver.Id;
			return Json(receiverId, JsonRequestBehavior.AllowGet);
		}

	}
}
