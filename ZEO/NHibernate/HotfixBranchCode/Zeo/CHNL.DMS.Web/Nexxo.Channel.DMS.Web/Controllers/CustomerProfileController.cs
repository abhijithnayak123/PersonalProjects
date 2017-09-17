using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using System.Text;
using System.Globalization;
using MGI.Channel.DMS.Web.ServiceClient;
using System.Collections;
using System.Threading;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Web.Controllers
{
	[SkipNoDirectAccess]
	public class CustomerProfileController : BaseController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult CustomerProfile()
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			string maskedNumber = string.Empty;
			MGI.Channel.DMS.Web.Models.CustomerProfile customerProfile = new MGI.Channel.DMS.Web.Models.CustomerProfile();
			customerProfile.CardBalance = new MGI.Channel.DMS.Web.Models.CardBalance();
			string cardNumber = customerProfile.customerSession.Customer.Fund.CardNumber;
			string processorAccountId = string.Empty;

			if (!string.IsNullOrEmpty(cardNumber) && cardNumber.Length >= 4 && cardNumber != Constants.PREPAID_CARD_NOT_ACTIVE)
			{
				cardNumber = cardNumber.Length > 4 ? cardNumber.Substring(cardNumber.Length - 4) : cardNumber;
				processorAccountId = "**** **** **** " + cardNumber;
			}
			else
			{
				processorAccountId = cardNumber;
			}

			if (customerProfile.customerSession.Customer.Fund.IsGPRCard)
			{
				ViewBag.GPRCardExists = true;
				if (cardNumber == Constants.PREPAID_CARD_NOT_ACTIVE)
				{
					customerProfile.customerSession.Customer.Fund.CardBalance = -1.00M;
				}
				else
				{
					Server.Data.CardInfo cardInfo = GetCardBalance(customerProfile.customerSession.CustomerSessionId, mgiContext);
					customerProfile.CardBalance.Balance = cardInfo.Balance;
					CardStatus cardStatus = (CardStatus)cardInfo.CardStatus;
					customerProfile.CardBalance.CardStatus = cardStatus.ToString();
					if (MGI.Common.Util.NexxoUtil.GetDictionaryValueIfExists(cardInfo.MetaData, "IsPrimaryCardHolder") != null)
					{
						bool cardType = MGI.Common.Util.NexxoUtil.GetBoolDictionaryValueIfExists(cardInfo.MetaData, "IsPrimaryCardHolder");
						customerProfile.CardBalance.CardType = cardType == true ?
							MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.Primarycard : MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.Companioncard;
					}
					string expMsg = Convert.ToString(TempData["GPRDown"]);
					if (expMsg != string.Empty)
					{
						ViewBag.ExceptionMsg = expMsg;
						ViewBag.IsException = true;
					}
				}
			}
			else
				ViewBag.GPRCardExists = false;

			string accountNumber = processorAccountId;

			if (customerProfile.customerSession.Customer.Fund.IsGPRCard)
			{
				maskedNumber = accountNumber;
				customerProfile.MaskedCardNumber = maskedNumber.ToString();
			}

			customerProfile.Address1 = string.Format("{0} {1}", customerProfile.customerSession.Customer.Address.Address1, customerProfile.customerSession.Customer.Address.Address2);
			customerProfile.Address1 = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(customerProfile.Address1.ToLower());

			customerProfile.City = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(customerProfile.customerSession.Customer.Address.City.ToLower());
			customerProfile.StateZipCode = string.Format("{0}, {1}", customerProfile.customerSession.Customer.Address.State, customerProfile.customerSession.Customer.Address.PostalCode);

			customerProfile.PhoneNumber = string.IsNullOrEmpty(customerProfile.customerSession.Customer.Phone1.Number) ? string.Empty :

			String.Format("{0:###-###-####}", Convert.ToInt64(customerProfile.customerSession.Customer.Phone1.Number));
			customerProfile.PhoneType = string.IsNullOrEmpty(customerProfile.customerSession.Customer.Phone1.Type) ? string.Empty : customerProfile.customerSession.Customer.Phone1.Type;

			return PartialView("_CustomerProfile", customerProfile);
		}

		private Server.Data.CardInfo GetCardBalance(string customerSessionId, MGIContext mgiContext)
		{
			Server.Data.CardInfo cardInfo = new Server.Data.CardInfo();
			if (Session["CardBalance"] == null)
			{
				Desktop client = new Desktop();
				try
				{
					cardInfo = client.GetCardBalance(customerSessionId, mgiContext);
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("1003.2107"))
					{
						cardInfo.Balance = decimal.MinValue;
						cardInfo.CardStatus = 100;
						TempData["GPRDown"] = ex.Message;
					}
				}
				Session["CardBalance"] = cardInfo;
			}
			else
			{
				cardInfo = (Server.Data.CardInfo)Session["CardBalance"];
			}
			return cardInfo;
		}
	}
}
