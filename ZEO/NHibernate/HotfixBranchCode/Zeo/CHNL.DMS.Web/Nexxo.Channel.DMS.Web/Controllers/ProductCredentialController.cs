using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Security.Voltage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Funds = MGI.Channel.Shared.Server.Data.Funds;
using FundsProcessorAccount = MGI.Channel.Shared.Server.Data.FundsProcessorAccount;
using FundType = MGI.Channel.Shared.Server.Data.FundType;
using TransactionFee = MGI.Channel.Shared.Server.Data.TransactionFee;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class ProductCredentialController : BaseController
	{
		NLoggerCommon NLogger = new NLoggerCommon();

		[HttpPost]
		[CustomHandleErrorAttribute(ViewName = "ProductCredential", MasterName = "_Common")]
		public ActionResult ProductCredential(ProductCredentialViewModel productCredential)
		{
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			mgiContext.TrxId = productCredential.TransactionId;
			productCredential.Resolutions = PopulateResolutions();
			productCredential.FraudScores = PopulateFraudScrores();

			if (productCredential.HasGPRCard)
			{
				//return View("ProductInformation", new ProductInfo());
				return RedirectToAction("ProductInformation", "Product");
			}
			else
			{
				string cardId = Guid.NewGuid().ToString();
				Funds funds = null;

				string cardNumber = productCredential.CardNumber == null ? string.Empty : productCredential.CardNumber.Replace(" ", "");

				if (!string.IsNullOrWhiteSpace(cardNumber))
				{
					SecureData secure = new SecureData(NLogger.Logger);
					cardNumber = secure.Decrypt(cardNumber, productCredential.CVV);
					productCredential.CardNumber = cardNumber;
					Session.Add("CardNumber",!string.IsNullOrEmpty(cardNumber)? cardNumber.Substring(cardNumber.Length - 4):string.Empty);
					customerSession.Customer.Fund.CardNumber = cardNumber;
					customerSession.Customer.Fund.IsGPRCard = false;
					Session["CustomerSession"] = customerSession;
				}

				if (!string.IsNullOrWhiteSpace(productCredential.PurchaseFee))
				{
					string[] amounts = productCredential.PurchaseFee.Split(' ');
					string fee = string.Empty;
					if (amounts.Length > 0)
					{
						fee = amounts[1];
					}
					if (!string.IsNullOrWhiteSpace(fee))
					{
						productCredential.ActivationFee = Convert.ToDecimal(fee);
					}
				}

				string Id = RegisterAccount(productCredential, customerSession.CustomerSessionId, mgiContext);

				ShoppingCart cart = desktop.ShoppingCart(customerSession.CustomerSessionId);
				if (cart.GprCards == null)
				{
					funds = new Funds()
					{
						Amount = productCredential.InitialLoad,
						Fee = productCredential.ActivationFee,
						PromoCode = productCredential.PromoCode
					};
					Id = desktop.ActivateGPRCard(customerSession.CustomerSessionId, funds, mgiContext).ToString();
				}
				else if (cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).Count() <= 1)
				{
					funds = new Funds()
					{
						Amount = productCredential.InitialLoad,
						Fee = productCredential.ActivationFee,
						PromoCode = productCredential.PromoCode
					};
					Id = desktop.ActivateGPRCard(customerSession.CustomerSessionId, funds, mgiContext).ToString();
				}

				return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
			}
		}

		#region Private Methods

		private ProductCredentialViewModel PopulateProductCredential(MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			List<SelectListItem> fraudScores = PopulateFraudScrores();
			List<SelectListItem> resolutions = PopulateResolutions();

			Desktop desktop = new Desktop();

			ProductCredentialViewModel productCredential = null;
			// Customer is linked to GPR Card
			if (!string.IsNullOrEmpty(customerSession.Customer.Fund.CardNumber) && customerSession.Customer.Fund.IsGPRCard)
			{
				//customerSession.Customer.Profile.FraudScore = 30;
				//customerSession.Customer.Profile.Resolution = "Verified DOB With Birth certificate";
				string cardNumber = customerSession.Customer.Fund.CardNumber;
				cardNumber = (cardNumber.Length > 4 && cardNumber != Constants.PREPAID_CARD_NOT_ACTIVE) ? cardNumber.Substring(cardNumber.Length - 4) : cardNumber;
				productCredential = new ProductCredentialViewModel()
				{
					Name = string.Format("{0} {1}", customerSession.Customer.PersonalInformation.FName, customerSession.Customer.PersonalInformation.LName),
					CardNumber = cardNumber != Constants.PREPAID_CARD_NOT_ACTIVE ? string.Format("**** **** **** {0}", cardNumber) : cardNumber,
					FraudScores = fraudScores,
					Resolutions = resolutions,
					FraudScore = customerSession.Customer.FraudScore,
					Resolution = customerSession.Customer.Resolution,
					HasGPRCard = true
				};
			}
			else
			{
				TransactionFee trxFee = desktop.GetFundsFee(long.Parse(customerSession.CustomerSessionId), 0, FundType.None, mgiContext);

				productCredential = new ProductCredentialViewModel()
				{
					Name = string.Format("{0} {1}", customerSession.Customer.PersonalInformation.FName, customerSession.Customer.PersonalInformation.LName),
					FraudScores = fraudScores,
					Resolutions = resolutions,
					ActivationFee = trxFee.NetFee,
					HasGPRCard = false,
					BaseFee = trxFee.BaseFee,
					DiscountApplied = trxFee.DiscountApplied,
					DiscountName = trxFee.DiscountName,
					NetFee = trxFee.NetFee,
					BaseFeeWithCurrency = Convert.ToString(trxFee.BaseFee),
					DiscountAppliedWithCurrency = Convert.ToString(trxFee.DiscountApplied),
					NetFeeWithCurrency = Convert.ToString(trxFee.NetFee)
				};
			}
			return productCredential;
		}

		private static List<SelectListItem> PopulateResolutions()
		{
			List<SelectListItem> resolutions = new List<SelectListItem>();
			// Populate Resolutions from enum
			foreach (Resolutions item in Enum.GetValues(typeof(Resolutions)))
			{
				string resolution = item.ToString().Replace("_Or_", "/").Replace("_", " ");
				resolutions.Add(new SelectListItem() { Text = resolution, Value = resolution });
			}
			resolutions.Insert(0, new SelectListItem() { Text = "", Value = "0", Selected = true });

			return resolutions;
		}

		private static List<SelectListItem> PopulateFraudScrores()
		{
			List<SelectListItem> fraudScores = new List<SelectListItem>();
			// Populate Fraud scores
			for (int index = 10; index <= 50; index += 10)
			{
				fraudScores.Add(new SelectListItem() { Text = index.ToString(), Value = index.ToString() });
			}
			fraudScores.Insert(0, new SelectListItem() { Text = "", Value = "0", Selected = true });

			return fraudScores;
		}

		private string RegisterAccount(ProductCredentialViewModel productCredential, string customerSessionId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			Desktop client = new Desktop();

			FundsProcessorAccount fundAccount = new FundsProcessorAccount()
			{
				CardNumber = productCredential.CardNumber,
				AccountNumber = productCredential.AccountNumber,
				FraudScore = Convert.ToInt32(productCredential.FraudScore),
				Resolution = productCredential.Resolution,

				ProxyId = productCredential.ProxyId,
				PseudoDDA = productCredential.PseudoDDA,
				ExpirationDate = productCredential.ExpirationDate				
			};

			return client.RegisterCard(fundAccount, customerSessionId, mgiContext).ToString();
		}

		private string TruncateCardNumber(string accountIdentifier)
		{
			return accountIdentifier.Length >= 4 ? accountIdentifier.Substring(accountIdentifier.Length - 4) : accountIdentifier;
		}

		#endregion
	}
}
