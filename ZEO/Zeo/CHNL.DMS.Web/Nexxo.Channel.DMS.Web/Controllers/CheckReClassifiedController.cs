using System;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class CheckReClassifiedController : BaseController
    {
        public ActionResult ReClassified(CheckDeclinedReasons reClassifiedCheck)
        {
            TempData["CheckFrontImage"] = reClassifiedCheck.CheckFrontImage;
            return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reClassifiedCheck"></param>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult CancelCheck(string checkId)
        {
            try
            {              
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

                long customerSessionId = GetCustomerSessionId();

                ZeoClient.ZeoContext context = GetCheckLogin();

                ZeoClient.Response response = alloyServiceClient.RemoveCheck(long.Parse(checkId), context);

                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                return RedirectToAction("ProductInformation", "Product");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }
    }
}
