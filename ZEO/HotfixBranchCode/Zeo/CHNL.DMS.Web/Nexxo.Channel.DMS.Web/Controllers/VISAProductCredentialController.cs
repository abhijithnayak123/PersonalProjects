using System;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using MGI.Security.Voltage;



namespace MGI.Channel.DMS.Web.Controllers
{
	public class VISAProductCredentialController : ProductCredentialController
	{
		public MGI.Common.Util.NLoggerCommon NLogger = new MGI.Common.Util.NLoggerCommon();

		[HttpGet]
		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
		public ActionResult ProductCredential(string transactionId)
		{
			ProductCredentialViewModel productCredential = new ProductCredentialViewModel();
			MGIContext mgiContext = new MGIContext();
			if (!string.IsNullOrWhiteSpace(transactionId))
			{
				productCredential = GetProductCredentialModel(transactionId, mgiContext);
				return View("ProductCredential", productCredential);
			}

			Session["activeButton"] = "productcredential";
			productCredential = PopulateProductCredential(mgiContext);

			if (productCredential.HasGPRCard)
			{
				DMS.Server.Data.CardInfo cardInfo = GetCardInfo(productCredential.customerSession.CustomerSessionId, mgiContext);
				int daysSinceClosed = CalculateDaysAfterClosure(cardInfo);

				if (daysSinceClosed > 60)
				{
					productCredential.HasGPRCard = false;
					productCredential.CardNumber = string.Empty;
				}
			}

			if (!productCredential.HasGPRCard)
				return View("ProductCredential", productCredential);
			else
				return RedirectToAction("VisaTransactionHistory", "VISAProductCredential");
		}

		public ActionResult VisaTransactionHistory()
		{
			MGIContext mgiContext = new MGIContext();
			Session["activeButton"] = "productcredential";
			VisaTransactionHistory visaHistory = new VisaTransactionHistory();
			Session["transactionStatus"] = "Posted";
			Session["accountNumber"] = visaHistory.customerSession.Customer.Fund.AccountNumber;

			DMS.Server.Data.CardInfo cardInfo = GetCardInfo(visaHistory.customerSession.CustomerSessionId, mgiContext);
			visaHistory.IsAccountClosed = IsAccountClosed(cardInfo);
			visaHistory.DisableMaintenance = ShouldDisableMainteance(cardInfo);
			visaHistory.DaysSinceClosed = CalculateDaysAfterClosure(cardInfo);
			visaHistory.CardBalance = cardInfo.Balance;
			visaHistory.CardStatus = (CardStatus)cardInfo.CardStatus;

			if (visaHistory.IsAccountClosed)
			{
				// removes 90 from the Date Range dropdown list
				visaHistory.DateRanges = visaHistory.DateRanges.Take(2);
			}

			return View("VisaTransactionHistory", visaHistory);
		}

		[HttpGet]
		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
		public ActionResult OpenAccount()
		{
			MGIContext mgiContext = new MGIContext();
			Session["activeButton"] = "productcredential";
			ProductCredentialViewModel productCredential = PopulateProductCredential(mgiContext);

			productCredential.HasGPRCard = false;
			productCredential.CardNumber = string.Empty;

			return View("ProductCredential", productCredential);
		}

		[HttpPost]
		public ActionResult VisaTransactionHistory(VisaTransactionHistory visaHistory)
		{
			Session["activeButton"] = "productcredential";
			Session["dateRange"] = visaHistory.DateRange;
			Session["transactionStatus"] = visaHistory.TransactionStatus;
			Session["accountNumber"] = visaHistory.customerSession.Customer.Fund.AccountNumber;

			if (visaHistory.IsAccountClosed)
			{
				// removes 90 from the Date Range dropdown list
				visaHistory.DateRanges = visaHistory.DateRanges.Take(2);
			}
			return View("VisaTransactionHistory", visaHistory);
		}

		[HttpPost]
		public ActionResult GetTransactionHistory(string sessionId, string sidx, string sord, int page = 1, int rows = 5)
		{
			Desktop client = new Desktop();

			int dateRange = Session["dateRange"] == null ? 30 : Convert.ToInt32(Session["dateRange"]);

			TransactionStatus transactionStatus = Session["transactionStatus"] == null ? TransactionStatus.Posted :
				(TransactionStatus)Enum.Parse(typeof(TransactionStatus), Convert.ToString(Session["transactionStatus"]));

			TransactionHistoryRequest transactionHistoryRequest = new TransactionHistoryRequest();

			transactionHistoryRequest.AliasId = Convert.ToInt64(Session["accountNumber"]);
			transactionHistoryRequest.DateRange = dateRange;
			transactionHistoryRequest.TransactionStatus = transactionStatus;

			long customerId = Convert.ToInt64(sessionId);

			MGIContext mgiContext = new MGIContext();
			try
			{
				List<CardTransactionHistory> transactions = client.GetTransactionHistory(customerId, transactionHistoryRequest, mgiContext);
				IQueryable<CardTransactionHistory> filteredtransactions = transactions.AsQueryable();

				Expression<System.Func<CardTransactionHistory, object>> expression = null;

				if (!String.IsNullOrEmpty(sidx))
				{
					expression = GetExpression(sidx, expression);

					if (sord.ToUpper() == "DESC")
					{
						filteredtransactions = filteredtransactions.OrderByDescending(expression);
					}
					else
					{
						filteredtransactions = filteredtransactions.OrderBy(expression);
					}
				}
				else
				{
					filteredtransactions = filteredtransactions.OrderByDescending(x => x.TransactionDateTime);
				}

				var sortedtransactions = filteredtransactions;
				var totalRecords = filteredtransactions.Count();
				var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

				//Building the grid data for Posted type transactions
				var data = transactionHistoryRequest.TransactionStatus == TransactionStatus.Posted ?
						  (from s in sortedtransactions
						   select new
						   {
							   cell = new object[] { s.PostedDateTime.ToString(), s.TransactionDateTime.ToString(), s.MerchantName, s.Location,
				 					s.TransactionDescription,s.TransactionAmount.ToString(("C2")),s.ActualBalance.ToString(("C2")),
				 					s.AvailableBalance.ToString(("C2"))}
						   }).ToList() :

						  //Building the grid data for Pending type transactions
						  transactionHistoryRequest.TransactionStatus == TransactionStatus.Pending ?
						  (from s in sortedtransactions
						   select new
						   {
							   cell = new object[] {s.TransactionDateTime.ToString(), s.MerchantName, s.Location,
				 					s.TransactionDescription,s.TransactionAmount.ToString(("C2")),s.ActualBalance.ToString(("C2")),
				 					s.AvailableBalance.ToString(("C2"))}
						   }).ToList() :

						  //Building the grid data for Denied type transactions
						  (from s in sortedtransactions
						   select new
							 {
								 cell = new object[] { s.TransactionDateTime.ToString(), s.MerchantName, s.Location,
				 					s.TransactionDescription,s.TransactionAmount.ToString(("C2")),s.DeclineReason,s.ActualBalance.ToString(("C2")),
				 					s.AvailableBalance.ToString(("C2"))}
							 }).ToList();


				var jsonData = new
				{
					total = totalPages,
					page = page,
					records = totalRecords,
					rows = data.Skip((page - 1) * rows).Take(rows)
				};

				return Json(jsonData);
			}
			catch (Exception ex)
			{
				return Json(
						new
						{
							success = false,
							err = ex.Message
						}, JsonRequestBehavior.AllowGet);
			}
		}

		#region AL-324/Visa Prepaid Phase

		public ActionResult ClosePrepaidAccount()
		{
			PrePaidCard card = new PrePaidCard();
			card.Name = card.customerSession.Customer.PersonalInformation.FName + " " + card.customerSession.Customer.PersonalInformation.LName;
			card.CardNumber = card.customerSession.Customer.Fund.CardNumber;
			card.CardNumber = "**** **** **** " + card.CardNumber.Substring(card.CardNumber.Length - 4);

			return PartialView("_ClosePrepaidAccountPopUp", card);
		}
		[HttpPost]
		public ActionResult ClosePrepaidAccount(long customerSessionId)
		{
			Desktop desktop = new Desktop();
			MGIContext mgiContext = new MGIContext();
			try
			{
				bool status = desktop.CloseAccount(customerSessionId, mgiContext);
				Server.Data.CardInfo cardInfo = new Server.Data.CardInfo();
				cardInfo = desktop.GetCardBalance(Convert.ToString(customerSessionId), mgiContext);
				Session["CardBalance"] = cardInfo;
				return Json(status, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(
					new
					{
						success = false,
						data = ex.Message
					}, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult CardMaintenancePopUp()
		{
			Desktop client = new Desktop();
			MGIContext mgiContext = new MGIContext();
			CardMaintenanceViewModel cardCredential = new CardMaintenanceViewModel();
			Server.Data.CardInfo cardInfo = client.GetCardBalance(Convert.ToString(cardCredential.customerSession.CustomerSessionId), mgiContext);
			Session["CardBalance"] = cardInfo;
			cardCredential.CardBalance = new Models.CardBalance();
			cardCredential.CardBalance.CardStatusId = cardInfo.CardStatus;
			CardStatus cardStatus = (CardStatus)cardInfo.CardStatus;
			cardCredential.CardBalance.CardStatus = cardStatus.ToString();

			List<KeyValuePair<string, string>> items = client.GetPrepaidActions(cardCredential.CardBalance.CardStatus.ToLower());
			List<SelectListItem> prepaidActions = GetSelectListItems(items);

			List<KeyValuePair<string, string>> shippingItem = client.GetShippingTypes(long.Parse(cardCredential.customerSession.CustomerSessionId), mgiContext);

			List<SelectListItem> shippingTypes = GetSelectListItems(shippingItem);

			cardCredential.ActionForCardReplace = prepaidActions;
			cardCredential.ShippingTypes = shippingTypes;



			return PartialView("_CardMaintenance", cardCredential);
		}

		[HttpPost]
		public ActionResult CardMaintenance(long customerSessionId, string prepaidAction, string shippingType, string panNumber, string cvv)
		{
			try
			{
				Desktop client = new Desktop();
				bool status = false;
				MGIContext mgiContext = new MGIContext();
				Server.Data.CardInfo cardInfo = new Server.Data.CardInfo();
				SecureData secure = new SecureData(NLogger.Logger);
				string decryptedCardNumber = string.Empty;
				if (!string.IsNullOrWhiteSpace(panNumber))
				{
					decryptedCardNumber = secure.Decrypt(panNumber, cvv);
				}
				int cardStatus = Convert.ToInt32(prepaidAction);
				CardMaintenanceInfo cardMaintenanceInfo = new CardMaintenanceInfo()
				{
					CardStatus = prepaidAction,
					ShippingType = shippingType,
					CardNumber = decryptedCardNumber,
					SelectedCardStatus = prepaidAction
				};
				if ((cardStatus == (int)CardStatus.LostCard || cardStatus == (int)CardStatus.StolenCard))
				{
					client.UpdateCardStatus(customerSessionId, cardMaintenanceInfo, mgiContext);
					status = client.ReplaceCard(customerSessionId, cardMaintenanceInfo, mgiContext);
				}
				else if (cardStatus == (int)CardStatus.Damaged)
				{
					//AL-4781 Changes
					cardInfo = client.GetCardBalance(Convert.ToString(customerSessionId), mgiContext);
					if (cardInfo.CardStatus != (int)CardStatus.Active)
					{
						cardMaintenanceInfo.CardStatus = cardInfo.CardStatus.ToString();  //update card status to current card status
					}
					status = client.ReplaceCard(customerSessionId, cardMaintenanceInfo, mgiContext);
				}
				else
				{
					status = client.UpdateCardStatus(customerSessionId, cardMaintenanceInfo, mgiContext);
				}

				cardInfo = client.GetCardBalance(Convert.ToString(customerSessionId), mgiContext);
				Session["CardBalance"] = cardInfo;
				return Json(status, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(
						new
						{
							success = "false",
							Err = ex.Message
						}, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public ActionResult CardReplacementConfirmation(string data)
		{
			CardMaintenanceViewModel cardCredential = new CardMaintenanceViewModel();
			//0 is active and 3 is suspended
			if (data == "0")
				ViewBag.ConfirmationMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ResourceManager.GetString(cardCredential.channelPartner.Name + "CardReplacementConfirmationMessageSuspendToActive");
			else if (data == "3")
				ViewBag.ConfirmationMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ResourceManager.GetString(cardCredential.channelPartner.Name + "CardReplacementConfirmationMessageActiveToSuspend");
			else
				ViewBag.ConfirmationMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ResourceManager.GetString(cardCredential.channelPartner.Name + "CardReplacementConfirmationMessage");
			return PartialView("_CardReplacementConfirmation");
		}

		[HttpPost]
		public ActionResult ShippingFee(string shippingType, long customerSessionId)
		{
			Desktop desktop = new Desktop();
			MGIContext context = new MGIContext();

			CardMaintenanceInfo cardMaintenance = new CardMaintenanceInfo()
			{
				ShippingType = shippingType
			};
			double fee = desktop.GetShippingFee(customerSessionId, cardMaintenance, context);
			string shippingFeeMessage = string.Format(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ShippingFeeMessage, fee);//"$24.95 replacement fee will be deducted from balance";

			var data = Json(new
			{
				success = true,
				fee = fee,
				message = shippingFeeMessage
			});
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult AssociateCard()
		{
			ProductCredentialViewModel productCredentialViewModel = new ProductCredentialViewModel();

			return View("AssociateCard", productCredentialViewModel);
		}

		[HttpPost]
		[CustomHandleErrorAttribute(ViewName = "AssociateCard", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductCredentialViewModel")]
		public ActionResult AssociateCard(ProductCredentialViewModel productCredentialViewModel)
		{
			Desktop client = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			SecureData secure = new SecureData(NLogger.Logger);
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			string decryptedCardNumber = secure.Decrypt(productCredentialViewModel.CardNumber, productCredentialViewModel.CVV);
			FundsProcessorAccount account = new FundsProcessorAccount()
			{
				CardNumber = decryptedCardNumber
			};
			long id = client.AssociateCard(long.Parse(productCredentialViewModel.customerSession.CustomerSessionId), account, mgiContext);
			customerSession.Customer.Fund.CardNumber = decryptedCardNumber;
			customerSession.Customer.Fund.IsGPRCard = true;
			Session["IsGPRCard"] = customerSession.Customer.Fund.IsGPRCard;
			Session["CustomerSession"] = customerSession;
			ViewBag.CardSuccessfulAssociated = id;
			return View("AssociateCard", productCredentialViewModel);
		}

		public ActionResult AssociateCardConfirmation()
		{
			CardMaintenanceViewModel cardMaintenance = new CardMaintenanceViewModel();
			ViewBag.ConfirmationMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ResourceManager.GetString(cardMaintenance.channelPartner.Name + "AssociateCardConfirmation");
			return PartialView("_AssociateCardConfirmation");
		}
		public ActionResult SuccessfulCardClosureConformation()
		{
			return PartialView("_SuccessfulCardClosurePopUp");
		}

		public ActionResult FundFee(string prepaidAction, long customerSessionId)
		{
			Desktop desktop = new Desktop();
			MGIContext context = new MGIContext();

			CardMaintenanceInfo cardMaintenance = new CardMaintenanceInfo()
			{
				CardStatus = prepaidAction
			};
			double fee = desktop.GetFundFee(customerSessionId, cardMaintenance, context);
			string visaFeeMessage = "No replacement fee";
			if (fee != 0)
				visaFeeMessage = string.Format(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.VisaFeeMessage, fee);

			var data = Json(new
			{
				success = true,
				fee = fee,
				message = visaFeeMessage
			});
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult OrderComapanionCard()
		{
			DMS.Server.Data.CardInfo cardInfo = new Server.Data.CardInfo();
			if (Session["CardBalance"] != null)
			{
				cardInfo = (DMS.Server.Data.CardInfo)Session["CardBalance"];
			}
			var data = Json(new
			{
				Err_Msg = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.OrderCompanionCardMessage,
				IsPrimaryCardHolder = MGI.Common.Util.NexxoUtil.GetBoolDictionaryValueIfExists(cardInfo.MetaData, "IsPrimaryCardHolder"),

			});
			return Json(data, JsonRequestBehavior.AllowGet);
		}
		public ActionResult OrderAddOnCard()
		{
			Prospect prospect = (Prospect)TempData["Prospect"];
			CardMaintenanceViewModel model = new CardMaintenanceViewModel();
			Desktop client = new Desktop();
			Funds fund = new Funds()
			{
				Fee = 0,
				Amount = 0
			};
			MGIContext mgiContext = new MGIContext();
			mgiContext.AddOnCustomerId = Convert.ToInt64(TempData["AddOnAlloyId"]);

			client.RequestAddOnCard(long.Parse(model.customerSession.CustomerSessionId), fund, mgiContext);

			return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
		}

		#endregion

		#region Private Methods
		private Expression<Func<CardTransactionHistory, object>> GetExpression(string sidx, Expression<System.Func<CardTransactionHistory, object>> expression)
		{
			switch (sidx.ToLower())
			{
				case "posteddatetime": expression = t => t.PostedDateTime;
					break;
				case "transactiondatetime": expression = t => t.TransactionDateTime;
					break;
				case "merchantname": expression = t => t.MerchantName;
					break;
				case "location": expression = t => t.Location;
					break;
				case "transactionamount": expression = t => t.TransactionAmount;
					break;
				case "transactiondescription": expression = t => t.TransactionDescription;
					break;
				case "availablebalance": expression = t => t.AvailableBalance;
					break;
				case "actualbalance": expression = t => t.ActualBalance;
					break;
			}
			return expression;
		}

		private ProductCredentialViewModel PopulateProductCredential(MGIContext mgiContext)
		{
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			Desktop desktop = new Desktop();
			ProductCredentialViewModel productCredential = null;
			// Customer is linked to GPR Card
			if (!string.IsNullOrEmpty(customerSession.Customer.Fund.CardNumber) && customerSession.Customer.Fund.IsGPRCard)
			{
				string cardNumber = customerSession.Customer.Fund.CardNumber;
				cardNumber = (cardNumber.Length > 4 && cardNumber != Constants.PREPAID_CARD_NOT_ACTIVE) ? cardNumber.Substring(cardNumber.Length - 4) : cardNumber;
				productCredential = new ProductCredentialViewModel()
				{
					Name = string.Format("{0} {1}", customerSession.Customer.PersonalInformation.FName, customerSession.Customer.PersonalInformation.LName),
					CardNumber = cardNumber != Constants.PREPAID_CARD_NOT_ACTIVE ? string.Format("**** **** **** {0}", cardNumber) : cardNumber,
					HasGPRCard = true
				};
			}
			else
			{
				TransactionFee trxFee = desktop.GetFundsFee(long.Parse(customerSession.CustomerSessionId), 0, FundType.None, mgiContext);

				productCredential = new ProductCredentialViewModel()
				{
					Name = string.Format("{0} {1}", customerSession.Customer.PersonalInformation.FName, customerSession.Customer.PersonalInformation.LName),
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

		private SelectListItem DefaultListItem()
		{
			return new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true };
		}

		private bool IsAccountClosed(string sessionId)
		{
			Desktop client = new Desktop();
			MGIContext mgiContext = new MGIContext();
			return (CardStatus)client.GetCardBalance(sessionId, mgiContext).CardStatus == CardStatus.Closed ? true : false;
		}

		private bool IsAccountClosed(DMS.Server.Data.CardInfo cardInfo)
		{
			return cardInfo.ClosureDate != null ? true : false;
		}

		private Server.Data.CardInfo GetCardInfo(string sessionId, MGIContext mgiContext)
		{
			Desktop client = new Desktop();
			return client.GetCardBalance(sessionId, mgiContext);
		}

		private List<SelectListItem> GetSelectListItems(List<KeyValuePair<string, string>> listItems)
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			selectListItems.Add(DefaultListItem());
			foreach (var item in listItems)
			{
				selectListItems.Add(new SelectListItem() { Text = item.Key, Value = item.Value });
			}
			return selectListItems;
		}

		private bool ShouldDisableMainteance(string sessionId, MGIContext mgiContext)
		{
			bool shouldDisable = false;

			Desktop desktop = new Desktop();
			Server.Data.CardInfo cardBalance = desktop.GetCardBalance(sessionId, mgiContext);
			CardStatus visaCardStatus = (CardStatus)cardBalance.CardStatus;

			switch (visaCardStatus)
			{
				case CardStatus.Active:
				case CardStatus.Suspended:
				case CardStatus.CardIssued:
				case CardStatus.LostCard:
				case CardStatus.StolenCard:
					shouldDisable = false;
					break;
				case CardStatus.PendingCardIssuance:
				case CardStatus.ExpiredCard:
				case CardStatus.PendingAccountClosure:
				case CardStatus.Closed:
				case CardStatus.ClosedForFraud:
				case CardStatus.ReturnedUndeliverable:
				case CardStatus.ResearchRequired:
				case CardStatus.Voided:
				case CardStatus.Damaged:
				case CardStatus.Stale:
				case CardStatus.PendingDestruction:
				case CardStatus.Destroyed:
				case CardStatus.ClosedDueToUpgrade:
				case CardStatus.ClosedForDeceased:
					shouldDisable = true;
					break;
			}

			return shouldDisable;
		}

		private bool ShouldDisableMainteance(Server.Data.CardInfo cardInfo)
		{
			bool shouldDisable = false;

			if (cardInfo.ClosureDate != null)
			{
				shouldDisable = true;
			}
			else
			{
				CardStatus visaCardStatus = (CardStatus)cardInfo.CardStatus;

				switch (visaCardStatus)
				{
					case CardStatus.Active:
					case CardStatus.Suspended:
					case CardStatus.CardIssued:
					case CardStatus.LostCard:
					case CardStatus.StolenCard:
						shouldDisable = false;
						break;
					case CardStatus.PendingCardIssuance:
					case CardStatus.ExpiredCard:
					case CardStatus.PendingAccountClosure:
					case CardStatus.Closed:
					case CardStatus.ClosedForFraud:
					case CardStatus.ReturnedUndeliverable:
					case CardStatus.ResearchRequired:
					case CardStatus.Voided:
					case CardStatus.Damaged:
					case CardStatus.Stale:
					case CardStatus.PendingDestruction:
					case CardStatus.Destroyed:
					case CardStatus.ClosedDueToUpgrade:
					case CardStatus.ClosedForDeceased:
						shouldDisable = true;
						break;
				}
			}

			return shouldDisable;
		}

		private int CalculateDaysAfterClosure(Server.Data.CardInfo cardInfo)
		{
			int days = 0;

			if (cardInfo.ClosureDate != null)
			{
				TimeSpan timeDifference = DateTime.Now - (DateTime)cardInfo.ClosureDate;
				days = timeDifference.Days;
			}

			return days;
		}

		private ProductCredentialViewModel GetProductCredentialModel(string transactionId, MGIContext mgiContext)
		{
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			Desktop desktop = new Desktop();
			ProductCredentialViewModel productCredential = null;
			long agentSessionId = long.Parse(Session["sessionId"].ToString());
			FundTransaction fundTransaction = desktop.GetFundTransaction(agentSessionId, long.Parse(customerSession.CustomerSessionId), Convert.ToInt64(transactionId), mgiContext);

			productCredential = new ProductCredentialViewModel()
			{
				Name = string.Format("{0} {1}", customerSession.Customer.PersonalInformation.FName, customerSession.Customer.PersonalInformation.LName),
				HasGPRCard = false,
				CardNumber = MGI.Common.Util.NexxoUtil.GetDictionaryValueIfExists(fundTransaction.MetaData, "PAN"),
				MaskCardNumber = MGI.Common.Util.NexxoUtil.GetDictionaryValueIfExists(fundTransaction.MetaData, "PAN"),
				ProxyId = MGI.Common.Util.NexxoUtil.GetDictionaryValueIfExists(fundTransaction.MetaData, "ProxyId"),
				PseudoDDA = MGI.Common.Util.NexxoUtil.GetDictionaryValueIfExists(fundTransaction.MetaData, "PseudoDDA"),
				ExpirationDate = MGI.Common.Util.NexxoUtil.GetDictionaryValueIfExists(fundTransaction.MetaData, "ExpirationDate"),
				InitialLoad = fundTransaction.Amount,
				TransactionId = long.Parse(transactionId),
				PromoCode = fundTransaction.PromoCode
			};

			return productCredential;
		}


		#endregion
	}
}
