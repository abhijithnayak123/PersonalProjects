using System;
using System.Collections.Generic;
using System.Web.Mvc;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class SendMoneyBaseController : BaseController
	{
		[HttpGet]
		public JsonResult GetStates(string countryCode)
		{
			try
			{
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
				long customerSessionId = GetCustomerSessionId();
				ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response = alloyServiceClient.GetXfrStates(countryCode, context);
				if (VerifyException(response)) return null;
				List<SelectListItem> states = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

				return Json(states, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				VerifyException(ex); return null;
			}
		}
	}
}
