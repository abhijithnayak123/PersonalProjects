using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class ProductController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProductInformation(bool IsException = false, string ExceptionMsg = "")
        {
            Session["activeButton"] = null;
            Desktop desktop = new Desktop();
            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			//In Cash Drawer screen if clicked on "Cancel" and "Yes" its coming to "product" screen without customer session 
			//activated.   
            if (customerSession == null)
            {
                CustomerSearch customerSearch = new CustomerSearch();
                return View("CustomerSearch", customerSearch);
            }

            ViewBag.IsException = IsException;
            ViewBag.ExceptionMsg = ExceptionMsg;

            ViewBag.Navigation = Resources.NexxoSiteMap.ProductInformation;
            return View("ProductInformation", new ProductInfo());
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
		public ActionResult SendMoney()
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Session["isCashierAgree"] = "true";
			Session["activeButton"] = "moneytransfer";
			if (Session["SendMoneyModel"] != null)
			{
				Session.Remove("SendMoneyModel"); 
			}

            Desktop desktop = new Desktop();
            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

            SendMoney sendMoney = new SendMoney();
            ViewBag.Navigation = Resources.NexxoSiteMap.SendMoney;
            string viewName = "";
            string masterName = "_Common";
            object model = new object();

			if (customerSession.Customer.IsWUGoldCard)
				Session["CustomerHasGoldCard"] = "true";
			else
				Session["CustomerHasGoldCard"] = "false";

			if (customerSession.Customer.IsWUGoldCard || TempData["SkipGoldCard"] != null)
			{
				if (string.IsNullOrWhiteSpace(customerSession.TipsAndOffers) && (customerSession.Customer.IsWUGoldCard || (TempData["SkipGoldCard"] != null && !Convert.ToBoolean(TempData["SkipGoldCard"].ToString()))))
				{
					PopulatTipsAndOffersMessage(desktop, customerSession);
				}
				string channelPartnerName = GetChannelPartnerName();
				sendMoney.LCountry = desktop.GetXfrCountries(long.Parse(customerSession.CustomerSessionId), mgiContext);
				sendMoney.LCountry = desktop.GetXfrCountries(long.Parse(customerSession.CustomerSessionId), mgiContext);
				sendMoney.LStates = desktop.GetXfrStates(long.Parse(customerSession.CustomerSessionId), string.Empty, mgiContext);
				sendMoney.LDelivertyMethods = desktop.DefaultSelectListItem();

				if ( channelPartnerName == "MGI"  )
				{
					sendMoney.LCities = desktop.DefaultSelectListItem();
				}
				else
				{
					sendMoney.LCities = desktop.GetXfrCities(long.Parse(customerSession.CustomerSessionId), string.Empty, mgiContext);
				}
				sendMoney.LDeliveryOptions = desktop.DefaultSelectListItem();
				sendMoney.LActOnMyBehalf = desktop.GetActBeHalfList();

                sendMoney.FrequentReceivers = desktop.GetFrequentReceivers(long.Parse(customerSession.CustomerSessionId), mgiContext);
                viewName = "SendMoney";
                model = sendMoney;
            }
            else
            {
                Session["lookupGoldCard"] = null;
                WesternUnionDetails wuDetailsModel = new WesternUnionDetails();
                wuDetailsModel.EditGoldCardFrom = "sendmoney";
                viewName = "EnrollWesternUnionGoldCard";
                model = wuDetailsModel;
            }

            if (sendMoney.FrequentReceivers != null)
            {
                if (sendMoney.FrequentReceivers.Count > 0)
                    Session["AreThereReceivers"] = "true";
                else
                    Session["AreThereReceivers"] = "False";
            }
            return View(viewName, masterName, model);
        }

		/// <summary>
		/// US2054
		/// </summary>
		/// <returns></returns>
		public JsonResult CashierLocationState()
		{
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			AgentDetails agentDetails = new AgentDetails();
			bool isStateAvailable = false;
			string agentFirstName = "";
			string agentLastName = "";
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			if (Session["HTSessions"] != null)
			{
				System.Collections.Hashtable htSessions = (System.Collections.Hashtable)Session["HTSessions"];
				MGI.Channel.DMS.Server.Data.AgentSession agentSession = ((MGI.Channel.DMS.Server.Data.AgentSession)(htSessions["TempSessionAgent"]));
				if (agentSession !=null)
				{
					if (agentSession != null)
					{
						string stateCode = agentSession.Terminal.Location.State;
						if (!string.IsNullOrWhiteSpace(stateCode))
						{
							bool isLocationState = desktop.IsSWBStateXfer(long.Parse(customerSession.CustomerSessionId), stateCode, mgiContext);
							if (isLocationState)
							{
								CashierDetails cashierDetails = desktop.GetAgentXfer(long.Parse(agentSession.SessionId), mgiContext);
								isStateAvailable = true;
								agentFirstName = cashierDetails.AgentFirstName;
								agentLastName = cashierDetails.AgentLastName;
							}
						}
					}
				}
			}
			var jsonData = new
			{
				isStateAvailable,
				agentFirstName,
				agentLastName
			};
			return Json(jsonData);
		}
	
		/// <summary>
		/// US2054 
		/// </summary>
		/// <param name="agentFirstName"></param>
		/// <param name="agentLastName"></param>
		/// <returns></returns>
		public ActionResult CashierValidatePopup(string agentFirstName, string agentLastName)
		{
			AgentDetails agentDetails = new AgentDetails();
			agentDetails.AgentFirstName = agentFirstName;
			agentDetails.AgentLastName = agentLastName;
			return PartialView("_partialCashierValidaton", agentDetails);
		}

        public ActionResult ReceiveMoney()
        {
            Session["isCashierAgree"] = "true";
            Session["activeButton"] = "receivemoney";
            ReceiveMoney receive = new ReceiveMoney();
            return View("ReceiveMoney", "_Common", receive);
        }

        public ActionResult TipsAndOffers()
        {
            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

            Models.TipsAndOffersViewModel tipsAndOffers = new Models.TipsAndOffersViewModel();
            if (customerSession != null)
                tipsAndOffers.Message = customerSession.TipsAndOffers;
            else
                tipsAndOffers.Message = string.Empty;

            return PartialView("_tipsAndOffers", tipsAndOffers);
        }

        public ActionResult IsSSNExists()
        {
            BaseModel model = new BaseModel();
            bool isSSNExists = string.IsNullOrWhiteSpace(model.customerSession.Customer.SSN);
            return Json(isSSNExists, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowSSNValidationWarning()
        {
            BaseModel baseModel = new BaseModel();
            return PartialView("_SSNValidationWarning", baseModel.customerSession.Customer.CIN);
        }

        private void PopulatTipsAndOffersMessage(Desktop desktop, CustomerSession customerSession)
        {
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            MGI.Channel.Shared.Server.Data.CardInfo cardInfo = desktop.GetCardInfoXfer(Convert.ToInt64(customerSession.CustomerSessionId), mgiContext);

            System.Text.StringBuilder tipsAndOffersBuilder = new System.Text.StringBuilder();

			// This is a quick fix for the reference error.
			if (cardInfo != null)
			{
				tipsAndOffersBuilder.AppendFormat("The customer has earned {0} Gold Points", cardInfo.TotalPointsEarned);
				if (!string.IsNullOrWhiteSpace(cardInfo.PromoCode))
				{
					tipsAndOffersBuilder.Append(string.Format(" and a Western Union Promo Code: {0}", cardInfo.PromoCode));
				}
				customerSession.TipsAndOffers = tipsAndOffersBuilder.ToString();
			}

            Session["CustomerSession"] = customerSession;
        }

    }
}
