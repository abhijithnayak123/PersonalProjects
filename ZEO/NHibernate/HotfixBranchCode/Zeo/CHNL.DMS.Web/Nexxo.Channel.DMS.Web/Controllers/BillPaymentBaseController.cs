using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using SharedData = MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
    /// <summary>
    /// This class performs a BillPayment Parent controller.
    /// </summary>
    public class BillPaymentBaseController : BaseController
    {
        /// <summary>
        /// Action method for AutoCompleteBillPayee
        /// </summary>
        /// <param name="term">The Parameter type of String for term</param>
        /// <returns>JsonResult</returns>
        public JsonResult AutoCompleteBillPayee(string term)
        {
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Desktop deskTop = new Desktop();
            List<string> namesList = new List<string>();
            long customerSessionId = GetCustomerSessionId();
            //var namesList = billers;
            try
            {
                long channelPartnerId = deskTop.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext).Id;
				namesList = deskTop.GetBillers(customerSessionId, channelPartnerId, term, mgiContext);
                return Json(namesList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return Json(namesList, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Action method for BillPayeeFee
        /// </summary>
        /// <param name="billPayeeName">The parameter type of String for billPayeeName</param>
        /// <returns>JsonResult</returns>
		public JsonResult PopulateBillPayee(string billPayeeNameOrCode)
		{
			Desktop deskTop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			long customerSessionId = GetCustomerSessionId();
            SharedData.FavoriteBiller favoriteBiller = new SharedData.FavoriteBiller();
			long channelPartnerId = deskTop.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext).Id;
            SharedData.Product biller = deskTop.GetBiller(customerSessionId, channelPartnerId, billPayeeNameOrCode, mgiContext);
			if (biller != null)
			{
				favoriteBiller = deskTop.GetFavoriteBiller(customerSessionId, billPayeeNameOrCode, mgiContext);
			}

            //Below code is for MGI - Reseting the AccountNumberRetryCount
            if (Session["SelectedBiller"] != null && !Session["SelectedBiller"].ToString().Contains(billPayeeNameOrCode))
            {
                Session["AccountNumberRetryCount"] = "1"; 
            }

			return Json(favoriteBiller, JsonRequestBehavior.AllowGet);
		}

        public JsonResult GetBillerInfo(string billerNameOrCode)
        {
            try
            {
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
                Desktop desktop = new Desktop();
                long channelPartnerId = desktop.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext).Id;
                CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
                long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

                BillerInfo billerInfo = desktop.GetBillerInfo(customerSessionId, billerNameOrCode, mgiContext);
                return Json(billerInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
				throw ex;
            }
        }
    }
}
