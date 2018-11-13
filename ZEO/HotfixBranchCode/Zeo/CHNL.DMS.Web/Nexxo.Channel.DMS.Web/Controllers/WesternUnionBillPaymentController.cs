using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using ServerData = MGI.Channel.DMS.Server.Data;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class WesternUnionBillPaymentController : BillPaymentBaseController
	{
		/// <summary>
		/// This action method will return Bill payment view
		/// </summary>
		/// <returns>BillPayment view</returns>
		[HttpGet]
		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
		public ActionResult BillPayment(bool IsException = false, string ExceptionMsg = "", string billerName = "")
		{
			Session["isCashierAgree"] = "true"; //US2054 - For showing SWB pop-up window for the first time
			Session["activeButton"] = "billpayment";
			Session["billFee"] = null; //AL-877 - Bill Pay transaction where the wrong customer  was assigned as the sender
			string viewName = "";
			string masterName = "_Common";
			object model = new object();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();

			if (IsException || (customerSession.Customer.IsWUGoldCard || TempData["SkipGoldCard"] != null))
			{
				Desktop desktop = new Desktop();
				if (string.IsNullOrWhiteSpace(customerSession.TipsAndOffers))
				{
					PopulatTipsAndOffersMessage(desktop, customerSession, mgiContext);
				}
				Models.WesternUnionBillPayViewModel billPayment = new Models.WesternUnionBillPayViewModel();
				//long alloyId = Convert.ToInt64(customerSession.Customer.Purse.ProcessorAccountId);
				long alloyId = customerSession.Customer.CIN;

				if (Session["BillPaymentRecord"] != null)
				{
					billPayment = (Models.WesternUnionBillPayViewModel)Session["BillPaymentRecord"];
					Session["BillPaymentRecord"] = null;
					if (billPayment != null)
					{

						long billAmount = Convert.ToInt64(billPayment.BillAmount);
						long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

						SharedData.BillPayLocation BillLocations = desktop.GetLocations(customerSessionId, billPayment.BillerName, billPayment.AccountNumber, billAmount, mgiContext);
						List<SharedData.BillerLocation> locations = BillLocations.BillerLocation;
						//Populate billerLocations and assign selected value
						List<SelectListItem> locationList = new List<SelectListItem>();
						List<SelectListItem> deliveryMethods = new List<SelectListItem>();
						if (locations != null && locations.Count > 0)
						{
							foreach (var location in locations)
							{
								bool isSelected = billPayment.BillerLocationName == location.Name;
								locationList.Add(new SelectListItem() { Text = location.Name, Value = location.Type, Selected = isSelected });
							}
						}
						else
						{
							locationList.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable", Selected = true });
						}

						if (Session["billFee"] != null)
						{
							SharedData.BillFee billFee = (SharedData.BillFee)Session["billFee"];
							foreach (var item in billFee.DeliveryMethods)
							{
								bool isSelected = billPayment.BillerDeliveryMethod == item.Code;
								deliveryMethods.Add(new SelectListItem() { Value = item.Code, Text = item.Text, Selected = isSelected });
							}

							billPayment.LLocations = locationList;
							billPayment.LDeliveryMethods = deliveryMethods;
						}
					}
				}
				else
				{
					billPayment.LLocations = desktop.DefaultSelectList();
					billPayment.LDeliveryMethods = desktop.DefaultSelectList();
				}

				ViewBag.IsException = IsException;
				ViewBag.ExceptionMsg = ExceptionMsg;

				billPayment.ProviderName = ProviderIds.WesternUnionBillPay.ToString();
				billPayment.FrequentBillPayees = desktop.GetFrequentBillers(long.Parse(customerSession.CustomerSessionId), alloyId, mgiContext);
				ViewBag.Navigation = Resources.NexxoSiteMap.BillPayment;
				model = billPayment;
				viewName = billPayment.ProviderName;
			}
			else
			{
				//Quick fix for Skip Enrollment
				ViewBag.IsBillPay = true;

				Session["lookupGoldCard"] = null;
				WesternUnionDetails wuDetailsModel = new WesternUnionDetails();
				wuDetailsModel.EditGoldCardFrom = "billpay";
				viewName = "EnrollWesternUnionGoldCard";
				model = wuDetailsModel;
			}
			return View(viewName, masterName, model);
		}

		public JsonResult PopulateBillPayeeLocation(string billPayeeName, string accountNumber, decimal amount)
		{
			try
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				Desktop desktop = new Desktop();
				List<SelectListItem> locationsList = new List<SelectListItem>();

				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
				long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);


				if (Session["billFee"] != null)
				{
					SharedData.BillFee tempBillFee = Session["billFee"] as SharedData.BillFee;
					if (tempBillFee != null)
					{
						mgiContext.TrxId = tempBillFee.TransactionId;
					}
				}

				SharedData.BillPayLocation BillLocations = desktop.GetLocations(customerSessionId, billPayeeName, accountNumber, amount, mgiContext);

				SharedData.BillFee billFee = new SharedData.BillFee();
				billFee.TransactionId = BillLocations.TransactionId;
				Session["billFee"] = billFee;

				List<SharedData.BillerLocation> locations = BillLocations.BillerLocation;

				foreach (var item in locations)
				{
					locationsList.Add(new SelectListItem() { Value = item.Type, Text = item.Name });
				}

				return Json(locationsList, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				throw ex;
			}
		}

		public JsonResult PopulateBillDeliveryMethod(string billPayeeName, string accountNumber, decimal amount, string location, string locationType)
		{
			List<SharedData.BillerLocation> locations = new List<SharedData.BillerLocation>();
			try
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				Desktop desktop = new Desktop();
				List<SelectListItem> deliveryMethods = new List<SelectListItem>();
				SharedData.BillerLocation billerLocation = null;
				if (!string.IsNullOrWhiteSpace(location.Trim()) && location != "Not Applicable")
				{
					billerLocation = new SharedData.BillerLocation()
					{
						Name = location,
						Type = locationType
					};
				}

				if (Session["billFee"] != null)
				{
					SharedData.BillFee tempBillFee = Session["billFee"] as SharedData.BillFee;
					if (tempBillFee != null)
					{
						mgiContext.TrxId = tempBillFee.TransactionId;
					}
				}

				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
				long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

				SharedData.BillFee billFee = desktop.GetFee(customerSessionId, billPayeeName, accountNumber, amount, billerLocation, mgiContext);

				Session["billFee"] = billFee;

				foreach (var item in billFee.DeliveryMethods)
				{
					deliveryMethods.Add(new SelectListItem() { Value = item.Code, Text = item.Text });
				}

				var jsonData = new
				{
					SessionCookie = billFee.SessionCookie,
					DeliveryMethods = deliveryMethods,
					accountHolderName = billFee.AccountHolderName,
					avaliableBalance = billFee.AvailableBalance,
					TransactionID = billFee.TransactionId
				};

				return Json(jsonData, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				throw ex;
			}
		}

		public JsonResult PopulateBillFee(string deliveryMethod)
		{
			try
			{
				if (Session["billFee"] != null)
				{
					SharedData.BillFee billFee = (SharedData.BillFee)Session["billFee"];
					SharedData.DeliveryMethod selectedDeliveryMethod = billFee.DeliveryMethods.FirstOrDefault(c => c.Code == deliveryMethod);
					decimal billPaymentFee = 0;
					if (selectedDeliveryMethod != null)
					{
						billPaymentFee = selectedDeliveryMethod.FeeAmount;
					}
					return Json(billPaymentFee, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(string.Empty, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				throw ex;
			}
		}

		public JsonResult GetProviderAttributes(string billerName, string location)
		{
			Desktop deskTop = new Desktop();

			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			mgiContext.ProviderId = (int)ProviderIds.WesternUnionBillPay;
			
			location = string.Compare(location, "Not Applicable", true) == 0 ? null : location;
			long customerSessionId = GetCustomerSessionId();
			List<SharedData.Field> fields = deskTop.GetProviderAttributes(customerSessionId, billerName, location, mgiContext);

			return Json(fields, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[CustomHandleErrorAttribute(ActionName = "BillPayment", ControllerName = "WesternUnionBillPayment", ResultType = "redirect")]
		public ActionResult BillPayment(Models.WesternUnionBillPayViewModel billPayment)
		{
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
			//long customerId = Convert.ToInt64(customerSession.Customer.Purse.ProcessorAccountId);
			long customerId = customerSession.Customer.CIN;

			billPayment.LLocations = desktop.DefaultSelectList();
			billPayment.LDeliveryMethods = desktop.DefaultSelectList();
			billPayment.FrequentBillPayees = desktop.GetFrequentBillers(customerSessionId, customerId, mgiContext);

			if (Session["billFee"] != null)
			{
				SharedData.BillFee billFee = Session["billFee"] as SharedData.BillFee;
				mgiContext.CityCode = billFee.CityCode;
				mgiContext.TrxId = billFee.TransactionId;
			}

			Session["BillPaymentRecord"] = billPayment;
			SharedData.BillPayment billPay = Mapper(billPayment);

			mgiContext.AlloyId = customerId;
			long transactionID = desktop.ValidateBillPayment(customerSessionId, billPay, mgiContext);

			ServerData.BillPayTransaction trx = desktop.GetBillPayTransaction(GetAgentSessionId(), customerSessionId, transactionID, mgiContext);

			SharedData.Customer customerProfile = customerSession.Customer;
			string cardNumber = Convert.ToString(GetDictionaryValue(trx.MetaData, "WesternUnionCardNumber"));

			BillpaymentReview billPaymentReview = new BillpaymentReview()
			{
				BillpayTransactionId = transactionID,
				BillerName = billPayment.BillerName,
				BillerLocationName = billPayment.BillerLocationName,
				BillerLocationId = billPayment.BillerLocation,
				BillPaymentAccount = billPayment.AccountNumber.MaskAccountNumber(),
				BillPaymentDeliveryMethodCode = billPayment.BillerDeliveryMethod,
				BillPaymentDeliveryMethod = billPayment.SelectedDeliveryMethod,
				BillpaymentAmount = billPayment.BillAmount,
				BillPaymentFee = (trx != null) ? trx.Fee : 0.0M,
				SenderName = customerProfile.PersonalInformation.FName + " " + customerProfile.PersonalInformation.LName,
				SenderAddress1 = customerProfile.Address.Address1 + " " + customerProfile.Address.Address2,
				SenderAddress2 = customerProfile.Address.City + ", " + customerProfile.Address.State + " " + customerProfile.Address.PostalCode,
				SenderEmail = customerProfile.Email,
				SenderPhoneNumber = customerProfile.Phone1.Number,
				SenderWUGoldcardNumber = string.IsNullOrWhiteSpace(cardNumber) ? "NA" : cardNumber,
				CouponPromoCode = billPayment.CouponPromoCode,
				DiscountedFee = Convert.ToDecimal(GetDictionaryValue(trx.MetaData, "DiscountedFee")),
				UnDiscountedFee = Convert.ToDecimal(GetDictionaryValue(trx.MetaData, "UnDiscountedFee"))
			};

			Session.Remove("BillPaymentRecord");
			ViewData = new ViewDataDictionary();
			return View("BillPayReview", "_Common", billPaymentReview);
		}
		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
		public ActionResult CancelBillPayDetails(long Id)
		{
			if (Id != 0)
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				Desktop desktop = new Desktop();

				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

				long customerSessionId = long.Parse(customerSession.CustomerSessionId);

				desktop.CancelBillPayment(customerSessionId, Id, mgiContext);
			}

			ViewBag.Navigation = Resources.NexxoSiteMap.ProductInformation;
			//return View("ProductInformation", new ProductInfo());
			return RedirectToAction("ProductInformation", "Product");
		}

		[CustomHandleErrorAttribute(ViewName = "BillPayReview", MasterName = "_Common")]
		public ActionResult BillPaySubmit(BillpaymentReview billpaymentReview)
		{
			if (Session["billFee"] != null)
				Session.Remove("billFee");

			if (billpaymentReview.ConsumerProtectionMessage)
			{
				Desktop desktop = new Desktop();

				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
				long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
				ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;

				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext()
				{
					RequestType = RequestType.HOLD.ToString()
				};

				//Add to shopping cart and stage in cxn
				desktop.StageBillPayment(customerSessionId, billpaymentReview.BillpayTransactionId, mgiContext);

				return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
			}

			else
			{
				ViewBag.ErrorMessage = "Please select the checkbox";
				return View("BillPayReview", billpaymentReview);
			}
		}

		private SharedData.BillPayment Mapper(Models.WesternUnionBillPayViewModel billpayment)
		{
			return new SharedData.BillPayment()
			{
				PaymentAmount = billpayment.BillAmount,
				Fee = billpayment.BillPaymentFee,
				BillerName = billpayment.BillerName,
				AccountNumber = billpayment.AccountNumber,
				// todo: need to revisit once have more clarity on coupon, promo & alias code
				CouponCode = billpayment.CouponPromoCode,
				BillerId = billpayment.BillerId,
				//WU Specific
				MetaData = new Dictionary<string, object>()
                        {
                            {"DeliveryCode", billpayment.BillerDeliveryMethod},
                            {"Location", billpayment.BillerLocationName},
                            {"SessionCookie", billpayment.SessionCookie},
                            {"Reference",billpayment.Reference},
                            {"AailableBalance",billpayment.AvailableBalance},
                            {"AccountHolder",billpayment.AccountHolder},
                            {"Attention",billpayment.Attention},
                            {"DateOfBirth",billpayment.DateOfBirth}
                        }
				//Todo:Commented By Sakhatech 
				//DeliveryCode = billpayment.BillerDeliveryMethod,//moved to Metadata
				//Location = billpayment.BillerLocationName,//moved to Metadata
				//SessionCookie = billpayment.SessionCookie,//moved to Metadata
				//Reference = billpayment.Reference,//moved to Metadata
				//AailableBalance = billpayment.AvailableBalance,//moved to Metadata
				//AccountHolder = billpayment.AccountHolder,//moved to Metadata
				//Attention = billpayment.Attention,//moved to Metadata
				//DateOfBirth = billpayment.DateOfBirth,//moved to Metadata
				//PromoCode = billpayment.CouponPromoCode,//To be removed?
			};
		}

		private void PopulatTipsAndOffersMessage(Desktop desktop, CustomerSession customerSession, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			SharedData.CardInfo cardInfo = desktop.GetCardInfo(Convert.ToInt64(customerSession.CustomerSessionId), mgiContext);

			if (cardInfo != null)
			{
				System.Text.StringBuilder tipsAndOffersBuilder = new System.Text.StringBuilder();

				tipsAndOffersBuilder.AppendFormat("The customer has earned {0} Gold Points", cardInfo.TotalPointsEarned);

				if (!string.IsNullOrWhiteSpace(cardInfo.PromoCode))
				{
					tipsAndOffersBuilder.Append(string.Format(" and a Western Union Promo Code: {0}", cardInfo.PromoCode));
				}

				customerSession.TipsAndOffers = tipsAndOffersBuilder.ToString();
				Session["CustomerSession"] = customerSession;
			}
		}
		public object GetDictionaryValue(Dictionary<string, object> dictionary, string key)
		{
			if (dictionary.ContainsKey(key) == false)
			{
				throw new Exception(String.Format("{0} not provided in dictionary", key));
			}
			return dictionary[key];
		}

		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Web |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
		// the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
		public ActionResult DeleteFavoriteBiller(string billerID)
		{
			Desktop client = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
			client.DeleteFavoriteBiller(customerSessionId, Convert.ToInt64(billerID), mgiContext);

			Models.BillPaymentViewModel billPayment = new BillPaymentViewModel();
			long alloyId = billPayment.customerSession.Customer.CIN;

			billPayment.FrequentBillPayees = client.GetFrequentBillers(customerSessionId, alloyId, mgiContext);
			return PartialView("_partialFrequentPayees", billPayment);
		}
		//End TA-191 Changes
		/// <summary>
		/// ImportPastBiller controller will fetch new records from UW and add/modify records in tCustomerPreferedProducts Table for Past Billers.
		/// </summary>
		/// <param name="productName"></param>
		/// <returns></returns>
		public ActionResult ImportPastBiller(string productName)
		{
			try
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				#region Added this code block for User Story # US1646 for Past Billers

				Desktop client = new Desktop();
				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
				long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
				SharedData.Account senderDetails = client.DisplayWUCardAccountInfo(customerSessionId, mgiContext);
				mgiContext.ProcessorId = 13;
				mgiContext.ProductType = "billpay";
				mgiContext.AgentId = Convert.ToInt32(Session["agentId"]);
				mgiContext.AgentSessionId = Convert.ToInt64(Session["sessionId"].ToString());

				if (customerSession.Customer.IsWUGoldCard)
					client.AddPastBillers(customerSessionId, senderDetails.LoyaltyCardNumber, mgiContext);

				#endregion

				// This will re-create partial view for FrequentBillers for Bill Pay Screen.

				Models.BillPaymentViewModel billPayment = new BillPaymentViewModel();
				//long alloyId = Convert.ToInt64(customerSession.Customer.Purse.ProcessorAccountId);
				long alloyId = billPayment.customerSession.Customer.CIN;
				billPayment.FrequentBillPayees = client.GetFrequentBillers(customerSessionId, alloyId, mgiContext);
				return PartialView("_partialFrequentPayees", billPayment);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return Json(string.Empty, JsonRequestBehavior.AllowGet);
			}
		}
		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Web |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. the below code displays popup with having biller id in view bag
		public ActionResult DisplayDeleteFavBiller(string id)
		{
			ViewBag.Id = id;
			return PartialView("_partialDeleteFavoriteBiller");
		}
		//End TA-191 Changes
	}
}
