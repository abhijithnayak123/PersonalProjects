using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class BillPaymentBaseController : BaseController
    {

        /// <summary>
        /// Action method for AutoCompleteBillPayee
        /// </summary>
        /// <param name="term">The Parameter type of String for term</param>
        /// <returns>JsonResult</returns>
        public JsonResult AutoCompleteBillPayee(string term)
        {
            ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            List<string> namesList = new List<string>();

            try
            {
                ZeoClient.Response response = serviceClient.GetBillers(term, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                namesList = response.Result as List<string>;
            }

            catch { }

            return Json(namesList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action method for BillPayeeFee
        /// </summary>
        /// <param name="billPayeeName">The parameter type of String for billPayeeName</param>
        /// <returns>JsonResult</returns>
        public JsonResult PopulateBillPayee(string billPayeeNameOrCode)
        {
            try
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();
                response = serviceClient.GetBillerDetails(billPayeeNameOrCode, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                if (Session["SelectedBiller"] != null && !Session["SelectedBiller"].ToString().Contains(billPayeeNameOrCode))
                    Session["AccountNumberRetryCount"] = "1";


                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult GetBillerInfo(string billerNameOrCode)
        {
            try
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();

                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.Response response = serviceClient.GetBillerInfo(billerNameOrCode, context);

                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }
    }
}
