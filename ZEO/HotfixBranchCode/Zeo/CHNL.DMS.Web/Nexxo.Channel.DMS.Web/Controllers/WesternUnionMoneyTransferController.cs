using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class WesternUnionMoneyTransferController : SendMoneyBaseController
	{
		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
		public ActionResult MoneyTransfer()
		{
			Session["isCashierAgree"] = "true";
			Session["activeButton"] = "moneytransfer";
			if (Session["SendMoneyModel"] != null)
			{
				Session.Remove("SendMoneyModel");
			}

			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			long customerSessionId = GetCustomerSessionId();
			SendMoney sendMoney = new SendMoney();
			ViewBag.Navigation = Resources.NexxoSiteMap.SendMoney;
			MGIContext mgiContext = new MGIContext();
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
					PopulatTipsAndOffersMessage(desktop, customerSession, mgiContext);
				}
				string channelPartnerName = GetChannelPartnerName();

				sendMoney.LCountry = desktop.GetXfrCountries(customerSessionId, mgiContext);
				sendMoney.LCountry = desktop.GetXfrCountries(customerSessionId, mgiContext);
				sendMoney.LStates = desktop.GetXfrStates(customerSessionId, string.Empty, mgiContext);
				sendMoney.LDelivertyMethods = desktop.DefaultSelectListItem();

				if (channelPartnerName == "MGI")
				{
					sendMoney.LCities = desktop.DefaultSelectListItem();
				}
				else
				{
					sendMoney.LCities = desktop.GetXfrCities(customerSessionId, string.Empty, mgiContext);
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

		private void PopulatTipsAndOffersMessage(Desktop desktop, CustomerSession customerSession, MGIContext mgiContext)
		{
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
