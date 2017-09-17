//using MGI.Channel.DMS.Server.Data;
//using MGI.Channel.DMS.Web.Common;
//using MGI.Channel.DMS.Web.Models;
//using MGI.Channel.DMS.Web.ServiceClient;
//using MGI.Channel.Shared.Server.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace MGI.Channel.DMS.Web.Controllers
//{
//	public class TSysProductCredentialController : ProductCredentialController
//	{
//		[HttpGet]
//		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
//		public ActionResult ProductCredential()
//		{
//			Session["activeButton"] = "productcredential";
//			ProductCredentialViewModel productCredential = PopulateProductCredential();
//			return View("ProductCredential", productCredential);
//		}


//		#region Private Methods
//		private ProductCredentialViewModel PopulateProductCredential()
//		{
//			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

//			List<SelectListItem> fraudScores = PopulateFraudScrores();
//			List<SelectListItem> resolutions = PopulateResolutions();

//			Desktop desktop = new Desktop();
//			MGIContext mgiContext = new MGIContext(); 

//			ProductCredentialViewModel productCredential = null;
//			// Customer is linked to GPR Card
//			if (!string.IsNullOrEmpty(customerSession.Customer.Fund.CardNumber) && customerSession.Customer.Fund.IsGPRCard)
//			{
//				//customerSession.Customer.Profile.FraudScore = 30;
//				//customerSession.Customer.Profile.Resolution = "Verified DOB With Birth certificate";
//				string cardNumber = customerSession.Customer.Fund.CardNumber;
//				cardNumber = (cardNumber.Length > 4 && cardNumber != Constants.PREPAID_CARD_NOT_ACTIVE) ? cardNumber.Substring(cardNumber.Length - 4) : cardNumber;
//				productCredential = new ProductCredentialViewModel()
//				{
//					Name = string.Format("{0} {1}", customerSession.Customer.PersonalInformation.FName, customerSession.Customer.PersonalInformation.LName),
//					CardNumber = cardNumber != Constants.PREPAID_CARD_NOT_ACTIVE ? string.Format("**** **** **** {0}", cardNumber) : cardNumber,
//					FraudScores = fraudScores,
//					Resolutions = resolutions,
//					FraudScore = customerSession.Customer.FraudScore,
//					Resolution = customerSession.Customer.Resolution,
//					HasGPRCard = true
//				};
//			}
//			else
//			{
//				TransactionFee trxFee = desktop.GetFundsFee(long.Parse(customerSession.CustomerSessionId), 0, FundType.None, mgiContext);

//				productCredential = new ProductCredentialViewModel()
//				{
//					Name = string.Format("{0} {1}", customerSession.Customer.PersonalInformation.FName, customerSession.Customer.PersonalInformation.LName),
//					FraudScores = fraudScores,
//					Resolutions = resolutions,
//					ActivationFee = trxFee.NetFee,
//					HasGPRCard = false,
//					BaseFee = trxFee.BaseFee,
//					DiscountApplied = trxFee.DiscountApplied,
//					DiscountName = trxFee.DiscountName,
//					NetFee = trxFee.NetFee,
//					BaseFeeWithCurrency = Convert.ToString(trxFee.BaseFee),
//					DiscountAppliedWithCurrency = Convert.ToString(trxFee.DiscountApplied),
//					NetFeeWithCurrency = Convert.ToString(trxFee.NetFee)
//				};
//			}
//			return productCredential;
//		}

//		private static List<SelectListItem> PopulateResolutions()
//		{
//			List<SelectListItem> resolutions = new List<SelectListItem>();
//			// Populate Resolutions from enum
//			foreach (Resolutions item in Enum.GetValues(typeof(Resolutions)))
//			{
//				string resolution = item.ToString().Replace("_Or_", "/").Replace("_", " ");
//				resolutions.Add(new SelectListItem() { Text = resolution, Value = resolution });
//			}
//			resolutions.Insert(0, new SelectListItem() { Text = "", Value = "0", Selected = true });

//			return resolutions;
//		}

//		private static List<SelectListItem> PopulateFraudScrores()
//		{
//			List<SelectListItem> fraudScores = new List<SelectListItem>();
//			// Populate Fraud scores
//			for (int index = 10; index <= 50; index += 10)
//			{
//				fraudScores.Add(new SelectListItem() { Text = index.ToString(), Value = index.ToString() });
//			}
//			fraudScores.Insert(0, new SelectListItem() { Text = "", Value = "0", Selected = true });

//			return fraudScores;
//		}

//		#endregion
//	}
//}
