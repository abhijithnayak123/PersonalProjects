using TCF.Channel.Zeo.Web.Common;
using System;
using System.Web.Mvc;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class MessageStoreController : BaseController
    {
        public ActionResult GetExceptionMessage(string messageKey)
		{
			try
			{
                ZeoClient.ZeoServiceClient alloyServiceCLient = new ZeoClient.ZeoServiceClient();

                ZeoClient.Response response = alloyServiceCLient.GetMessage(messageKey, GetZeoContext());
				if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.Message msg = response.Result as ZeoClient.Message;
				string errorMessage = string.Join("|", new object[] { msg.Processor, msg.MessageKey, msg.Content, msg.AddlDetails, Helper.ErrorType.ERROR });
			    return Json(
						new
						{
							success = true,
							message = errorMessage
						}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				VerifyException(ex); return null;
			}

		}
    }
}
