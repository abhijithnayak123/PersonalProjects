using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient.DMSService;
using MGI.Channel.DMS.Server.Data;
using System.Collections;
using System.Web.Script.Serialization;
using MGI.Channel.DMS.Web.Common;
using System.ServiceModel;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using System.Text;
using FundType = MGI.Channel.Shared.Server.Data.FundType;
using NexxoSOAPFault = MGI.Common.Sys.NexxoSOAPFault;
using SharedData = MGI.Channel.Shared.Server.Data;
using System.Collections.Specialized;

namespace MGI.Channel.DMS.Web.Controllers
{
	[SkipNoDirectAccess]
	public class ShoppingCartController : BaseController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sidx"></param>
		/// <param name="sord"></param>
		/// <param name="page"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult GetShoppingCartSummary(string sidx, string sord, int page = 1, int rows = 5)
		{
			//Verify customer session has initiated. Throw error if NOT.
			if (Session["CustomerSession"] == null)
			{
				throw new Exception("Customer session not initiated");
			}

			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

			//Re verify the status of pending checks.
			if (shoppingCart.Checks != null)
			{
				var check = shoppingCart.Checks.Where(c => c.Status == Constants.STATUS_PENDING).FirstOrDefault();
				if (check != null)
				{
					/****************************Begin TA-91 Changes************************************************/
					//        User Story Number: TA-91 | Web |   Developed by: Sunil Shetty      Date: 26.02.2015
					//        Purpose: 1.preCheckStatusPendingCount keeps count of pending check count before upating status of check
					//				   2.postCheckStatusPendingCheckCount keeps count of pending check count after upating status of check
					//				   3.Count of pending check are checked before and after change of check status, if status changes we carete tempdata.
					int preCheckStatusPendingCount = shoppingCart.Checks.FindAll(m => m.Status == Constants.STATUS_PENDING).Count();
					MGI.Channel.DMS.Server.Data.MGIContext mgiContext = GetCheckLogin(Convert.ToInt64((customerSession.CustomerSessionId)));
					desktop.GetCheckStatus(customerSession.CustomerSessionId, check.Id, mgiContext);
					shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);
					int postCheckStatusPendingCheckCount = shoppingCart.Checks.FindAll(m => m.Status == Constants.STATUS_PENDING).Count();
					if (preCheckStatusPendingCount != postCheckStatusPendingCheckCount)
						TempData["PendingCheckCountChanged"] = true;
					/****************************End TA-91 Changes************************************************/
				}
			}

			ShoppingCartSummary shoppingCartSummary = ShoppingCartHelper.ShoppingCartSummary(shoppingCart);
			bool hasGPRActivation = (shoppingCart.GprCards == null) ? false : shoppingCart.GprCards.Exists(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE);
			if (shoppingCartSummary.Items.Count() > 0)
			{
				decimal feeAmount = 0;
				decimal totAmount = 0;

				feeAmount = shoppingCartSummary.Items.Sum(x => x.Fee);
				if (shoppingCartSummary.Items.Where(x => x.TxnType == "c") != null && shoppingCartSummary.Items.Where(x => x.TxnType == "c").Count() > 0)
					totAmount = shoppingCartSummary.Items.Where(x => x.TxnType == "c").Sum(x => x.Total);

				if (shoppingCartSummary.Items.Where(x => x.TxnType == "d") != null && shoppingCartSummary.Items.Where(x => x.TxnType == "d").Count() > 0)
					totAmount += -1 * (shoppingCartSummary.Items.Where(x => x.TxnType == "d").Sum(x => x.Total));

				ShoppingCartSummaryItem feeItem = new ShoppingCartSummaryItem()
				{
					Product = "Fees",
					Amount = Math.Abs(feeAmount),
					Status = "",
					TxnCount = 1,
					TxnType = "d"
				};

				ShoppingCartSummaryItem totalItem = new ShoppingCartSummaryItem()
				{
					Product = "Total",
					Amount = Math.Abs(totAmount),
					Status = "",
					TxnCount = 1,
					TxnType = totAmount >= 0 ? "c" : "d"
				};

				shoppingCartSummary.Items.Add(feeItem);
				shoppingCartSummary.Items.Add(totalItem);
			}

			var totalRecords = shoppingCartSummary.Items.Count();
			var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

			var data = (from s in shoppingCartSummary.Items
						select new
						{
							cell = new object[] { s.Product, s.TxnCount.ToString(), "$" + Convert.ToDecimal(s.Amount).ToString("0.00"), s.Status, s.TxnType, hasGPRActivation }
						}
						).ToArray();
			var jsonData = new
			{
				total = totalPages,
				page = page,
				records = totalRecords,
				rows = data.Skip((page - 1) * rows).Take(rows)
			};

			return Json(jsonData);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult ShoppingCartCheckout()
		{
			//Verify customer session has initiated. Throw error if NOT.
			if (Session["CustomerSession"] == null)
			{
				return RedirectToAction("ProductInformation", "Product", new { IsException = true, ExceptionMsg = "Customer session not initiated" });
			}

			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();

			if (Session["activeButton"] != null)
			{
				Session["activeButton"] = null;
			}

			try
			{
				ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

				shoppingCartDetail = ShoppingCartHelper.ShoppingCartDetailed(shoppingCart);

				//handling existing GPR customer. 
				if (customerSession.Customer.Fund.IsGPRCard && customerSession.Customer.Fund.CardNumber != Constants.PREPAID_CARD_NOT_ACTIVE)
				{
					shoppingCartDetail.CardHolder = customerSession.Customer.Fund.IsGPRCard;
				}

				shoppingCartDetail.MinimumLoadAmount = desktop.GetMinimumLoadAmount(long.Parse(customerSession.CustomerSessionId), false, mgiContext);
				shoppingCartDetail.LoadFee = desktop.GetFundsFee(long.Parse(customerSession.CustomerSessionId), shoppingCartDetail.LoadToCard, FundType.Credit, mgiContext).NetFee;
				shoppingCartDetail.WithdrawFee = desktop.GetFundsFee(long.Parse(customerSession.CustomerSessionId), shoppingCartDetail.WithdrawFromCard, FundType.Debit, mgiContext).NetFee;
				shoppingCartDetail.LoadToCardToDisplay = 0;

				//Session Values default value setup
				Session["PendingChecks"] = false;
				if (Session["ShoppingCartCheckOutStatus"] == null || Session["ShoppingCartCheckOutStatus"].ToString() == "")
					Session["ShoppingCartCheckOutStatus"] = ShoppingCartCheckoutStatus.InitialCheckout;

				if (shoppingCartDetail.Items.Count != 0)
				{
					//Session Values update
					Session["IsShoppingCartExists"] = true;
					if (shoppingCart.Checks.Where(c => c.Status == Constants.STATUS_PENDING).Any())
						Session["PendingChecks"] = true;

					if (shoppingCart.GprCards != null)
					{
						//Handling gpr card registration scenario.
						if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).Count() > 0)
						{
							shoppingCartDetail.CardHolder = true;
							shoppingCartDetail.MinimumLoadAmount = desktop.GetMinimumLoadAmount(long.Parse(customerSession.CustomerSessionId), true, mgiContext);
							//defect fix DE1863
							ViewBag.InitLoadAmt = "true";
							shoppingCartDetail.IsCardActivationTrx = true;
							shoppingCartDetail.LoadFee = 0;
						}
						else if (shoppingCartDetail.CardHolder)
						{
							shoppingCartDetail.CardHolder = customerSession.Customer.Fund.IsGPRCard;
							shoppingCartDetail.CardBalance = customerSession.Customer.Fund.CardBalance;
						}

						//AL-514 Fix for when MO fails and click on submit doubling of CashToCustomer and PreviouslyCashCollected  
						shoppingCartDetail.WithdrawFromCard = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_WITHDRAW.ToString() && x.Status != Constants.STATUS_COMMITTED).Sum(x => x.WithdrawAmount);

						shoppingCartDetail.LoadToCardToDisplay = shoppingCart.GprCards.Where(x => x.Status == Constants.STATUS_COMMITTED).Sum(x => x.LoadAmount + x.WithdrawAmount);
					}

					shoppingCartDetail.CashToCustomer = shoppingCartDetail.DueToCustomer + shoppingCart.CashInTotal;
					shoppingCartDetail.PreviousCashCollected = shoppingCart.Cash.Where(x => x.CashType == ProductType.CashIn.ToString()).Sum(x => x.Amount);
					shoppingCartDetail.CashCollected = shoppingCartDetail.PreviousCashCollected;


					if (shoppingCartDetail.DueToCustomer < 0)
					{
						ViewBag.NetDueMessage = "Net Due from Customer $";
					}
					else
					{
						ViewBag.NetDueMessage = "Net Due to Customer $";
					}
				}

				//For Promotions
				foreach (var item in shoppingCartDetail.Items)
				{
					if (item.DiscountApplied < 0)
					{
						ViewBag.PromotionText = "* Promotion applied to transaction";
						break;
					}
				}
				shoppingCartDetail.IsReferalSectionEnable = desktop.IsReferralApplicable(long.Parse(customerSession.CustomerSessionId), mgiContext);

				//For Certegy
				string transactionId = (string)TempData["transactionId"];
				ViewBag.transactionId = transactionId;
				ViewBag.IsApproved = (Convert.ToString(TempData["CheckStatus"]) == "Approved");
				ViewBag.IsReclassify = TempData["IsReclassify"];

				//Card Details
				if (Session["CardBalance"] != null)
				{
					MGI.Channel.DMS.Server.Data.CardInfo cardInfo = (MGI.Channel.DMS.Server.Data.CardInfo)Session["CardBalance"];
					ViewBag.cardStatus = cardInfo.CardStatus;
				}

				//Navigate to shoppingcart detailed view for final checkout.
				return View("ShoppingCartCheckout", shoppingCartDetail);
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				ViewBag.IsException = true;
				ViewBag.ExceptionMsg = System.Web.HttpUtility.JavaScriptStringEncode(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
				ViewBag.Navigation = Resources.NexxoSiteMap.ShoppingCartCheckout;
				//Card Details
				if (Session["CardBalance"] != null)
				{
					MGI.Channel.DMS.Server.Data.CardInfo cardInfo = (MGI.Channel.DMS.Server.Data.CardInfo)Session["CardBalance"];
					ViewBag.cardStatus = cardInfo.CardStatus;
				}
				//For Promotions
				shoppingCartDetail.IsReferalSectionEnable = desktop.IsReferralApplicable(long.Parse(customerSession.CustomerSessionId), mgiContext);

				return View("ShoppingCartCheckout", shoppingCartDetail);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="shoppingCartCheckout"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ShoppingCartCheckout(ShoppingCartDetail shoppingCartDetail, string submit, string recalc)
		{
			SharedData.Receipts Receipts;
			Desktop desktop = new Desktop();
			ShoppingCartSuccess shoppingCartSuccess = null;
			bool boolReload = false;
			bool processMoneyOrder = false;

			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			try
			{
				ShoppingCart shoppingCart = desktop.ShoppingCart(shoppingCartDetail.customerSession.CustomerSessionId);

				//Showing pop-up for not adding more than one GPR transactions
				if (shoppingCart.GprCards != null && shoppingCart.GprCards.Exists(x => x.Status == Constants.STATUS_COMMITTED && (shoppingCartDetail.WithdrawFromCard > 0 || shoppingCartDetail.LoadToCard > 0)))
					throw new Exception("Cart already having GPR Transaction, cannot do another GPR Transaction for this customer session");

				if (ModelState.IsValid)
				{
					if ((shoppingCart.Checks != null && shoppingCart.Checks.Count() > 0) || (shoppingCart.MoneyOrders != null && shoppingCart.MoneyOrders.Count() > 0))
					{
						int pendingChecksCount = shoppingCart.Checks.Where(c => c.Status != Constants.STATUS_PENDING && c.Status != Constants.STATUS_AUTHORIZED && c.Status != Constants.STATUS_COMMITTED).Count();
						if (pendingChecksCount > 0)
							throw new Exception("Cannot Complete Shopping Cart with a Declined Check. Remove Check to Continue.");

						int pendingMoneyOrderCount = shoppingCart.MoneyOrders.Where(c => c.Status == Constants.STATUS_FAILED).Count();

						if (pendingMoneyOrderCount > 0)
							throw new Exception("Cannot Complete Shopping Cart with a Failed Money Order. Remove Money Order to Continue.");
					}


					//US1114 
					if (shoppingCart.GprCards != null)
					{
						//US1114 - Changes
						decimal withdrawFee = desktop.GetFundsFee(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), shoppingCartDetail.WithdrawFromCard, FundType.Debit, mgiContext).NetFee;

						if (shoppingCartDetail.customerSession.Customer.Fund.CardBalance < (Math.Abs(shoppingCartDetail.WithdrawFromCard) + withdrawFee) && shoppingCartDetail.WithdrawFromCard > 0)
							throw new Exception("Withdraw Amount and related fee should be less than available balance.");

						if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).Count() > 0 && shoppingCartDetail.WithdrawFromCard > 0)
							throw new Exception("Card not activated, hence cannot withdraw amount.");
					}

					decimal totalDue = (shoppingCartDetail.DueToCustomer - shoppingCartDetail.LoadToCard + shoppingCartDetail.WithdrawFromCard + shoppingCartDetail.CashCollected);
					shoppingCartDetail.CashToCustomer = shoppingCartDetail.DueToCustomer - shoppingCartDetail.LoadToCard + shoppingCartDetail.WithdrawFromCard + shoppingCartDetail.CashCollected;

					if ((ShoppingCartCheckoutStatus)Session["ShoppingCartCheckOutStatus"] == ShoppingCartCheckoutStatus.CashCollected || (ShoppingCartCheckoutStatus)Session["ShoppingCartCheckOutStatus"] == ShoppingCartCheckoutStatus.CashOverCounter)
					{
						if (shoppingCart.Cash.Where(x => x.CashType == ProductType.CashIn.ToString()).Any())
						{
							decimal previousCashIn = shoppingCart.Cash.Where(x => x.CashType == ProductType.CashIn.ToString()).LastOrDefault().Amount;

							totalDue = totalDue + previousCashIn;

							shoppingCartDetail.CashToCustomer = shoppingCartDetail.CashToCustomer + previousCashIn;
						
						}
					}

					//Update the prepaid transaction tables, in case customer wishes to adjust the Load or withdraw amount from shopping cart checkout.
					shoppingCartDetail.CardHolder = shoppingCartDetail.customerSession.Customer.Fund.IsGPRCard;
					shoppingCartDetail.CardBalance = shoppingCartDetail.customerSession.Customer.Fund.CardBalance;

					ShoppingCartHelper.UpdateFundsTransaction(shoppingCartDetail, shoppingCart);

					if (decimal.Round(totalDue, 2) < 0)
					{
						boolReload = true;
						throw new Exception("Insufficient Funds ! Add Cash");
					}

					if (recalc != null)
					{
						throw new Exception("ReloadException");
					}

					//US1164              
					string cardNumber = shoppingCartDetail.customerSession.Customer.Fund.CardNumber == null ? string.Empty : shoppingCartDetail.customerSession.Customer.Fund.CardNumber;

					//Set up migContext Values
					mgiContext.IsReferral = shoppingCartDetail.IsReferral;
					mgiContext.Context = new Dictionary<string, object>();
					mgiContext.Context.Add("SSOAttributes", GetSSOAgentSession("SSO_AGENT_SESSION"));

					//Checkout Method 
					ShoppingCartCheckoutStatus shoppingCartCheckoutStatus = desktop.Checkout(shoppingCartDetail.customerSession.CustomerSessionId, shoppingCartDetail.CashToCustomer, cardNumber, (ShoppingCartCheckoutStatus)Session["ShoppingCartCheckOutStatus"], mgiContext);

					//Store ShoppingCart checkout status in Session
					Session["ShoppingCartCheckOutStatus"] = shoppingCartCheckoutStatus;

					if (shoppingCartCheckoutStatus == ShoppingCartCheckoutStatus.Completed)
					{
						Session["ShoppingCartCheckOutStatus"] = null;

						ShoppingCart cart = desktop.ShoppingCart(shoppingCartDetail.customerSession.CustomerSessionId);

						// close the shopping cart
						desktop.CloseShoppingCart(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), mgiContext);

						//if gpr card activation was part of shopping cart and has been successful then update the current customer session to update the profile applet.
						if (cart.GprCards != null)
						{
							if (cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE && x.Status == Constants.STATUS_COMMITTED).Count() > 0)
							{
								string currentSessionCardNumber = cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE && x.Status == Constants.STATUS_COMMITTED).FirstOrDefault().CardNumber;
								shoppingCartDetail.customerSession.Customer.Fund.IsGPRCard = true;
								shoppingCartDetail.customerSession.Customer.Fund.CardNumber = currentSessionCardNumber;
								Session["CustomerSession"] = shoppingCartDetail.customerSession;
								if (Session["CardNumber"] != null)
									Session["CardNumber"] = currentSessionCardNumber;
                                Session["IsGPRCard"] = true;
							}
							else
							{
								if (Session["CardBalance"] != null)
								{
									Server.Data.CardInfo cardInfo = new Server.Data.CardInfo();
									cardInfo = desktop.GetCardBalance(shoppingCartDetail.customerSession.CustomerSessionId, mgiContext);
									Session["CardBalance"] = cardInfo;
								}
							}
						}

						shoppingCartSuccess = new ShoppingCartSuccess();
						if (shoppingCartDetail.CashToCustomer > 0)
							shoppingCartSuccess.CashToCustomer = shoppingCartDetail.CashToCustomer;
						shoppingCartSuccess.CheckOutResult = true;

						//US1421 Changes
						shoppingCartSuccess.FrankCheck = false;
						shoppingCartSuccess.CheckCount = 0;
						if (shoppingCart.Checks != null && shoppingCart.Checks.Count > 0)
						{
							shoppingCartSuccess.FrankCheck = IsCheckFranking(mgiContext);
							if (shoppingCartSuccess.FrankCheck)
							{
								shoppingCartSuccess.CheckCount = shoppingCart.Checks.Count;
								shoppingCartSuccess.CheckData = getCheckFrankCount(shoppingCart);
							}
						}

						// generate receipts for the shopping cart

						Receipts = desktop.GenerateReceiptsForShoppingCart(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), cart.Id, mgiContext);

						if (Receipts != null && Receipts.receiptType != null && Receipts.receiptType.Count > 0)
						{
							shoppingCartSuccess.ReceiptCount = Receipts.receiptType.Count;
							shoppingCartSuccess.ReceiptType = ShoppingCartHelper.GetReceiptsType(Receipts);
						}
						else
						{
							return RedirectToAction("ProductInformation", "Product");
						}

						TempData["shoppingCartSuccess"] = shoppingCartSuccess;

						//Removing Session Values
						if (Session["CardNo"] != null)
							Session.Remove("CardNo");

						Session.Remove("IsShoppingCartExists");

						// send the user to success screen
						ViewBag.Navigation = "Receipt to Customer";

						return RedirectToAction("ShoppingCartCheckoutSuccess", "ShoppingCart");
					}
					else if (shoppingCartCheckoutStatus == ShoppingCartCheckoutStatus.CashOverCounter)
					{
						ViewBag.CashOverCounter = true;
					}
					else if (shoppingCartCheckoutStatus == ShoppingCartCheckoutStatus.MOPrinting)
					{
						ShoppingCart cart = desktop.ShoppingCart(shoppingCartDetail.customerSession.CustomerSessionId);

						if (cart.MoneyOrders != null && cart.MoneyOrders.Where(m => m.Status == Constants.STATUS_AUTHORIZED).Any())
						{
							MoneyOrder moneyOrder = cart.MoneyOrders.Where(m => m.Status == Constants.STATUS_AUTHORIZED).OrderBy(m => m.Id).First();
							if (moneyOrder != null)
							{
								desktop.UpdateMoneyOrderStatus(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), long.Parse(moneyOrder.Id), Convert.ToInt16(Constants.STATUS_PROCESSING), mgiContext);

								processMoneyOrder = true;
							}
						}
					}
				}

				shoppingCart = desktop.ShoppingCart(shoppingCartDetail.customerSession.CustomerSessionId);
				ShoppingCartDetail cartDetail = ShoppingCartHelper.ShoppingCartDetailed(shoppingCart);

				cartDetail.LoadToCardToDisplay = shoppingCartDetail.LoadToCard;
				cartDetail.CashToCustomer = shoppingCartDetail.CashToCustomer;
				cartDetail.CardHolder = shoppingCartDetail.CardHolder;
				shoppingCartDetail = cartDetail;

				shoppingCartDetail.MinimumLoadAmount = desktop.GetMinimumLoadAmount(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), false, mgiContext);
				shoppingCartDetail.LoadToCardToDisplay = 0;

				if (shoppingCart.GprCards != null)
				{
					//Set Prepaid card holder flag and balance
					shoppingCartDetail.CardHolder = shoppingCartDetail.customerSession.Customer.Fund.IsGPRCard;
					shoppingCartDetail.CardBalance = shoppingCartDetail.customerSession.Customer.Fund.CardBalance;
					shoppingCartDetail.LoadToCardToDisplay = shoppingCart.GprCards.Where(x => x.Status == Constants.STATUS_AUTHORIZED).Sum(x => x.LoadAmount);

					shoppingCartDetail.LoadFee = desktop.GetFundsFee(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), shoppingCartDetail.LoadToCard, FundType.Credit, mgiContext).NetFee;
					shoppingCartDetail.WithdrawFee = desktop.GetFundsFee(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), shoppingCartDetail.WithdrawFromCard, FundType.Debit, mgiContext).NetFee;

					//AL-514 Fix for when MO fails and click on submit doubling of CashToCustomer and PreviouslyCashCollected  
					shoppingCartDetail.WithdrawFromCard = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_WITHDRAW.ToString() && x.Status != Constants.STATUS_COMMITTED).Sum(x => x.WithdrawAmount);

					if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).Count() > 0)
					{
						shoppingCartDetail.CardHolder = true;
						shoppingCartDetail.IsCardActivationTrx = true;
						shoppingCartDetail.LoadFee = 0;
						shoppingCartDetail.MinimumLoadAmount = desktop.GetMinimumLoadAmount(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), true, mgiContext);
						ViewBag.InitLoadAmt = "true";

						if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE && x.Status == Constants.STATUS_COMMITTED).Count() > 0)
						{
							//if gpr card activation was part of shopping cart and has been successful then update the current customer session to update the profile applet.
							string currentSessionCardNumber = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE && x.Status == Constants.STATUS_COMMITTED).FirstOrDefault().CardNumber;
							shoppingCartDetail.customerSession.Customer.Fund.IsGPRCard = true;
							shoppingCartDetail.customerSession.Customer.Fund.CardNumber = currentSessionCardNumber;
							Session["CustomerSession"] = shoppingCartDetail.customerSession;
							if (Session["CardNumber"] != null)
								Session["CardNumber"] = currentSessionCardNumber;
						}
					}
				}

				shoppingCartDetail.IsReferalSectionEnable = desktop.IsReferralApplicable(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), mgiContext);

				if (processMoneyOrder)
				{
					@ViewBag.ProcessMoneyOrder = true;
					@ViewBag.CashPreviouslyCollected = shoppingCart.Cash.Where(x => x.CashType == ProductType.CashIn.ToString()).Sum(x => x.Amount);
				}

				if (Session["CardBalance"] != null)
				{
					MGI.Channel.DMS.Server.Data.CardInfo cardInfo = (MGI.Channel.DMS.Server.Data.CardInfo)Session["CardBalance"];
					ViewBag.cardStatus = cardInfo.CardStatus;
				}

				if (shoppingCartDetail.DueToCustomer < 0)
				{
					ViewBag.NetDueMessage = "Net Due from Customer $";
				}
				else
				{
					ViewBag.NetDueMessage = "Net Due to Customer $";
				}

				ViewBag.Navigation = Resources.NexxoSiteMap.ShoppingCartCheckout;

				return View("ShoppingCartCheckout", shoppingCartDetail);
			}
			catch (Exception ex)
			{
				if (shoppingCartSuccess != null)
				{
					ViewBag.Navigation = "Receipt to Customer";
					return View("CheckoutSuccess", shoppingCartSuccess);
				}

				//get the existing shoppingcart cash collected as it gets overwritten by ShoppingCartDetailed.     
				ShoppingCart shoppingCart = desktop.ShoppingCart(shoppingCartDetail.customerSession.CustomerSessionId);
				ShoppingCartDetail cartDetail = ShoppingCartHelper.ShoppingCartDetailed(shoppingCart);
				if (boolReload)
				{
					cartDetail.LoadToCardToDisplay = shoppingCartDetail.LoadToCard;
					boolReload = false;
				}

				shoppingCartDetail = cartDetail;
				shoppingCartDetail.LoadToCardToDisplay = 0;
				shoppingCartDetail.LoadFee = 0;
				shoppingCartDetail.MinimumLoadAmount = desktop.GetMinimumLoadAmount(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), false, mgiContext);

				if (shoppingCart.Cash != null)
					shoppingCartDetail.CashCollected = shoppingCartDetail.PreviousCashCollected = shoppingCart.Cash.Where(x => x.CashType == ProductType.CashIn.ToString()).Sum(x => x.Amount);

				shoppingCartDetail.CashToCustomer = shoppingCartDetail.CashCollected + shoppingCartDetail.DueToCustomer - shoppingCartDetail.LoadToCard + shoppingCartDetail.WithdrawFromCard;

				//Handling gpr card registration scenario.
				if (shoppingCart.GprCards != null)
				{
					shoppingCartDetail.LoadFee = desktop.GetFundsFee(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), shoppingCartDetail.LoadToCard, FundType.Credit, mgiContext).NetFee;
					shoppingCartDetail.WithdrawFee = desktop.GetFundsFee(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), shoppingCartDetail.WithdrawFromCard, FundType.Debit, mgiContext).NetFee;

					//Set Prepaid card holder flag and balance
					shoppingCartDetail.CardHolder = shoppingCartDetail.customerSession.Customer.Fund.IsGPRCard;
					shoppingCartDetail.CardBalance = shoppingCartDetail.customerSession.Customer.Fund.CardBalance;
					shoppingCartDetail.LoadToCardToDisplay = shoppingCart.GprCards.Where(x => x.Status == Constants.STATUS_AUTHORIZED).Sum(x => x.LoadAmount);

					//AL-514 Fix for when MO fails and click on submit doubling of CashToCustomer and PreviouslyCashCollected  
					shoppingCartDetail.WithdrawFromCard = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_WITHDRAW.ToString() && x.Status != Constants.STATUS_COMMITTED).Sum(x => x.WithdrawAmount);

					//If Prepaid Activate 
					if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).Count() > 0)
					{
						shoppingCartDetail.CardHolder = true;
						shoppingCartDetail.IsCardActivationTrx = true;
						shoppingCartDetail.LoadFee = 0;
						shoppingCartDetail.MinimumLoadAmount = desktop.GetMinimumLoadAmount(long.Parse(shoppingCartDetail.customerSession.CustomerSessionId), true, mgiContext);

						//if gpr card activation was part of shopping cart and has been successful then update the current customer session to update the profile applet.
						if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE && x.Status == Constants.STATUS_COMMITTED).Count() > 0)
						{
							string currentSessionCardNumber = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE && x.Status == Constants.STATUS_COMMITTED).FirstOrDefault().CardNumber;
							shoppingCartDetail.customerSession.Customer.Fund.IsGPRCard = true;
							shoppingCartDetail.customerSession.Customer.Fund.CardNumber = currentSessionCardNumber;
							Session["CustomerSession"] = shoppingCartDetail.customerSession;
							if (Session["CardNumber"] != null)
								Session["CardNumber"] = currentSessionCardNumber;
						}
					}
				}

				if (ex.Message != "ReloadException")
				{
					ViewBag.IsException = true;
					ViewBag.ExceptionMsg = System.Web.HttpUtility.JavaScriptStringEncode(ex.Message);
				}
				else
				{
					if (shoppingCart.Checks.Where(c => c.Status == Constants.STATUS_PENDING).Any())
						Session["PendingChecks"] = true;
					else
						Session["PendingChecks"] = false;
				}

				if (Session["CardBalance"] != null)
				{
					MGI.Channel.DMS.Server.Data.CardInfo cardInfo = (MGI.Channel.DMS.Server.Data.CardInfo)Session["CardBalance"];
					ViewBag.cardStatus = cardInfo.CardStatus;
				}

				if (shoppingCartDetail.DueToCustomer < 0)
				{
					ViewBag.NetDueMessage = "Net Due from Customer $";
				}
				else
				{
					ViewBag.NetDueMessage = "Net Due to Customer $";
				}

				ViewBag.Navigation = Resources.NexxoSiteMap.ShoppingCartCheckout;

				return View("ShoppingCartCheckout", shoppingCartDetail);
			}
		}

		// ShoppingCart Checkout - Park / Remove Cart item
		public JsonResult ParkAndDeleteShoppingCartItems()
		{
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

			if (customerSession != null && shoppingCart != null && shoppingCart.Id != 0)
			{
				foreach (var item in shoppingCart.Checks)
				{
					try
					{
						if (item.Status != Constants.STATUS_COMMITTED.ToString())
							ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.Checks);
					}
					catch { }
				}

				foreach (var item in shoppingCart.Bills)
				{
					try
					{
						if (item.Status != Constants.STATUS_COMMITTED.ToString())
							RemoveShoppingCartTrx(item.Id, ProductType.BillPay);
					}
					catch { }
				}

				foreach (var item in shoppingCart.MoneyTransfers)
				{
					try
					{
						if (item.Status != Constants.STATUS_COMMITTED.ToString())
						{
							if (item.TransferType == (int)TransferType.SendMoney)
								RemoveShoppingCartTrx(item.Id, ProductType.SendMoney);
							else if (item.TransferType == (int)TransferType.RecieveMoney)
								RemoveShoppingCartTrx(item.Id, ProductType.ReceiveMoney);
						}
					}
					catch { }
				}

				foreach (var item in shoppingCart.MoneyOrders)
				{
					try
					{
						if (item.Status != Constants.STATUS_COMMITTED.ToString())
							ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.MoneyOrder);
					}
					catch { }
				}

				foreach (var item in shoppingCart.GprCards)
				{
					try
					{
						if (item.Status != Constants.STATUS_COMMITTED.ToString())
						{
							if (item.ItemType == Constants.PREPAID_CARD_LOAD)
								RemoveShoppingCartTrx(item.Id, ProductType.GPRLoad);
							else if (item.ItemType == Constants.PREPAID_CARD_WITHDRAW)
								RemoveShoppingCartTrx(item.Id, ProductType.GPRWithdraw);
							else if (item.ItemType == Constants.PREPAID_CARD_ACTIVATE)
								RemoveShoppingCartTrx(item.Id, ProductType.GPRActivation);
						}
					}
					catch { }
				}

				foreach (var item in shoppingCart.Cash)
				{
					try
					{
						if (item.Status != Constants.STATUS_COMMITTED.ToString())
						{
							RemoveShoppingCartTrx(item.Id, ProductType.CashIn);
						}
					}
					catch { }

				}

				mgiContext.IsAvailable = true;
				desktop.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), mgiContext);
			}


			return Json(new { success = true }, JsonRequestBehavior.AllowGet);
		}

		// ShoppingCart Checkout - ShoppingCart Success
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// 
		[CustomHandleErrorAttribute(ViewName = "CheckoutSuccess", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ShoppingCartSuccess")]
		public ActionResult ShoppingCartCheckoutSuccess()
		{
			ShoppingCartSuccess shoppingCartSuccess = new ShoppingCartSuccess();
			if (TempData["shoppingCartSuccess"] != null)
			{
				shoppingCartSuccess = TempData["shoppingCartSuccess"] as ShoppingCartSuccess;
				TempData.Keep("shoppingCartSuccess");
			}
			ViewBag.customerSessionId = GetCustomerSessionId();
			ViewBag.Navigation = "Receipt to Customer";
			return View("CheckoutSuccess", shoppingCartSuccess);
		}

		// ShoppingCart Checkout - ShoppingCart Success
		/// <summary>
		/// To get any Exception Occured in ShoppingCartCheckoutSuccess Page
		/// </summary>
		/// <returns></returns>
		public JsonResult ShoppingCartCheckOutSuccessDetails()
		{
			var jsonData = new { data = "MGiAlloy | 1003.6004 | Some Exception Occured in ShoppingCartCheckOutSuccess Page | Some Exception Occured in ShoppingCartCheckOutSuccess Page", Success = true };
			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		// ShoppingCart Checkout - Confirm for ShoppingCart Success
		[CustomHandleErrorAttribute(ViewName = "CheckoutConfirm", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ShoppingCartSuccess")]
		public ActionResult ShoppingCartCheckoutConfirm()
		{
			ShoppingCartSuccess shoppingCartSuccess = TempData["shoppingCartSuccess"] as ShoppingCartSuccess;
			return View("CheckoutConfirm", shoppingCartSuccess);
		}

		// ShoppingCart Checkout - Delete Transaction
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="screenName"></param>
		/// <param name="Product"></param>
		/// <returns></returns>
		public ActionResult DeleteShoppingCartItem(string id, string screenName, string product)
		{
			Desktop desktop = new Desktop();
			CheckCashingProgress checkCashProgress = new CheckCashingProgress();
			ShoppingCart shoppingCart = new ShoppingCart();
			bool isCheckRemoved = false;
			try
			{
				if (Session["CustomerSession"] == null)
				{
					throw new Exception("Customer session not initiated");
				}
				var providers = checkCashProgress.Providers.ToList();
				long customerSessionId = GetCustomerSessionId();
				ProductType prodtype = ProductType.None;
				Enum.TryParse(product, out prodtype);
				switch (prodtype)
				{
					case ProductType.Checks:
						MGI.Channel.DMS.Server.Data.MGIContext mgiContext = GetCheckLogin(Convert.ToInt64(customerSessionId));
						isCheckRemoved = desktop.RemoveCheque(customerSessionId, long.Parse(id), mgiContext);
						if (!isCheckRemoved && providers.Any(x => x.ProcessorId == (long)ProviderIds.Certegy))
						{
							TempData["transactionId"] = id;
						}
						break;

					case ProductType.BillPay:
						desktop.RemoveBill(customerSessionId, long.Parse(id));
						break;

					case ProductType.ReceiveMoney:
						desktop.RemoveMoneyTransfer(customerSessionId, long.Parse(id));
						break;

					case ProductType.SendMoney:
						shoppingCart = desktop.ShoppingCart(customerSessionId.ToString());
						if (shoppingCart.MoneyTransfers.Where(x => x.TransactionSubType == ((int)SharedData.TransactionSubType.Cancel).ToString()
						  || x.TransactionSubType == ((int)SharedData.TransactionSubType.Modify).ToString()).Any())
						{

							long originalTransactionId = shoppingCart.MoneyTransfers.Where(x => x.Id == id).FirstOrDefault().OriginalTransactionId;

							string trxId = shoppingCart.MoneyTransfers.Where(x => x.OriginalTransactionId == originalTransactionId
											 && x.TransactionSubType == ((int)SharedData.TransactionSubType.Cancel).ToString()).FirstOrDefault().Id;
							desktop.RemoveMoneyTransfer(customerSessionId, long.Parse(trxId));

							trxId = shoppingCart.MoneyTransfers.Where(x => x.OriginalTransactionId == originalTransactionId
										 && x.TransactionSubType == ((int)SharedData.TransactionSubType.Modify).ToString()).FirstOrDefault().Id;
							desktop.RemoveMoneyTransfer(customerSessionId, long.Parse(trxId));
						}
						else
							desktop.RemoveMoneyTransfer(customerSessionId, long.Parse(id));
						break;

					case ProductType.GPRActivation:
						//Deleting activation record should delete load record implicitly.
						shoppingCart = desktop.ShoppingCart(customerSessionId.ToString());
						desktop.RemoveFunds(customerSessionId, long.Parse(id));
						if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_LOAD).Any())
						{
							string loadTxnId = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_LOAD).FirstOrDefault().Id;
							desktop.RemoveFunds(customerSessionId, long.Parse(loadTxnId));
						}
						break;

					case ProductType.GPRLoad:
						//Deleting load record should delete activation record if exisit implicitly.
						shoppingCart = desktop.ShoppingCart(customerSessionId.ToString());
						if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).Any())
						{
							string activateTxnId = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).FirstOrDefault().Id;
							if (!string.IsNullOrEmpty(activateTxnId))
							{
								desktop.RemoveFunds(customerSessionId, long.Parse(activateTxnId));
							}
						}
						desktop.RemoveFunds(customerSessionId, long.Parse(id));
						break;

					case ProductType.GPRWithdraw:
					case ProductType.AddOnCard:
						desktop.RemoveFunds(customerSessionId, long.Parse(id));
						break;

					case ProductType.MoneyOrder:
						desktop.RemoveMoneyOrder(customerSessionId, long.Parse(id));
						break;

					case ProductType.Refund:
						desktop.RemoveMoneyTransfer(customerSessionId, long.Parse(id));
						break;

				}
			}
			//catch (FaultException<NexxoSOAPFault> nexxoFault)
			//{
			//    ViewBag.IsException = true;
			//    ViewBag.ExceptionMsg = ExceptionHandler.GetSOAPExceptionMessage(nexxoFault);
			//}
			catch (Exception ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
			}
			if (!string.IsNullOrEmpty(screenName) && screenName.ToLower() == "checkout")
			{
				return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
			}
			else
			{
				return RedirectToAction("ProductInformation", "Product");
			}
		}

		// ShoppingCart Checkout - Delete Transaction
		public ActionResult DeleteCartItemAlert(string id, string screenName, string product)
		{
			ViewBag.deleteCartId = id;
			ViewBag.screenName = screenName;
			ViewBag.product = product;
			return PartialView("_DeleteCartItemAlert");
		}

		// ShoppingCart Checkout - Parking Transaction
		public ActionResult ParkShoppingCartItemAlert(string id, string screenName, string product, string status)
		{
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			try
			{
				if (customerSession == null)
					throw new Exception("Customer session not initiated");
				ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);
				bool isMobileEnabled = isMobileEnabledForTextMessage(customerSession);
				string message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.ParkMessage;
				string certegymessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CertegyMessage;

				ParkTransaction parkTransaction = new ParkTransaction();

				long customerSessionId = long.Parse(customerSession.CustomerSessionId);
				ProductType prodtype = ProductType.None;
				Enum.TryParse(product, out prodtype);
				parkTransaction.TransactionId = long.Parse(id);
				parkTransaction.IsMobileEnabled = isMobileEnabled;
				parkTransaction.TransactionType = product;
				parkTransaction.TransactionStatus = status;

				ViewBag.CartItemId = id;
				ViewBag.screenName = screenName;
				ViewBag.product = product;
				bool CanParkReceiveMoney = true;
				bool isCertegy = true;
				var processorConfig = parkTransaction.Providers.FirstOrDefault(x => x.ProcessorName == "Certegy" && x.Name == "ProcessCheck");

				if (processorConfig != null)
				{
					isCertegy = processorConfig.IsCertegy;
				}

				ViewBag.Certegy = isCertegy;
				ViewBag.Certegymessage = certegymessage;
				var providerConfig = parkTransaction.Providers.FirstOrDefault(x => x.ProcessorName == "WesternUnion" && x.Name == "ReceiveMoney");
				if (providerConfig != null)
				{
					CanParkReceiveMoney = providerConfig.CanParkReceiveMoney;
				}
				ViewBag.CanPark = CanParkReceiveMoney;
				if (!CanParkReceiveMoney && prodtype == ProductType.ReceiveMoney)
				{
					message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.DeleteMessage;
				}
				ViewBag.Message = message;
				switch (prodtype)
				{
					case ProductType.Checks:
						Check checkdetails = shoppingCart.Checks.Find(x => x.Id == id);
						parkTransaction.Amount = checkdetails.Amount;
						parkTransaction.Detail = checkdetails.StatusDescription;
						parkTransaction.IconName = "processcheckGreen.png";
						if (!string.IsNullOrEmpty(checkdetails.DiscountName))
						{
							parkTransaction.CheckMOPromotionAlertMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckMOPromotionAlertMessage;
						}
						break;

					case ProductType.BillPay:
						Bill billpayDetails = shoppingCart.Bills.Find(x => x.BillId == id);
						parkTransaction.Amount = billpayDetails.Amount;
						parkTransaction.Detail = string.Format("{0} A/C {1}", billpayDetails.BillerName, billpayDetails.AccountNumber);
						parkTransaction.IconName = "BillPayGreen.png";
						break;

					case ProductType.ReceiveMoney:
					case ProductType.SendMoney:
						MoneyTransfer moneyTransactionDetails = shoppingCart.MoneyTransfers.Find(x => x.Id == id);
						parkTransaction.Amount = moneyTransactionDetails.Amount;
						parkTransaction.Detail = string.Format("{0} - {1}", moneyTransactionDetails.ReceiverFirstName + moneyTransactionDetails.ReceiverLastName, moneyTransactionDetails.DestinationCountry);
						parkTransaction.IconName = prodtype == ProductType.SendMoney ? "SendMoneyGreen.png" : "ReceiveMoneyGreen.png";
						break;

					case ProductType.GPRActivation:
						GprCard cardTransaction = shoppingCart.GprCards.Find(x => x.Id == id);
						parkTransaction.Amount = cardTransaction.LoadAmount;
						parkTransaction.Detail = string.Format("Activate ****{0}", ShoppingCartHelper.getAcctLast4Digits(cardTransaction.CardNumber.ToString()));
						parkTransaction.IconName = "CredentialsGreen.png";
						if (!string.IsNullOrEmpty(cardTransaction.DiscountName))
						{
							parkTransaction.CheckMOPromotionAlertMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckMOPromotionAlertMessage;
						}
						if (cardTransaction.InitialLoadAmount != 0)
						{
							parkTransaction.Amount = cardTransaction.InitialLoadAmount;
						}
						break;
					case ProductType.AddOnCard:
						GprCard cardTrx = shoppingCart.GprCards.Find(x => x.Id == id);
						parkTransaction.Amount = cardTrx.LoadAmount;
						parkTransaction.Detail = string.Format("Companion Card Order - [{0}]", cardTrx.AddOnCustomerName);
						parkTransaction.IconName = "CredentialsGreen.png";
						if (cardTrx.InitialLoadAmount != 0)
						{
							parkTransaction.Amount = cardTrx.InitialLoadAmount;
						}
						break;
					case ProductType.MoneyOrder:
						MoneyOrder moneyOrderTransactionDetails = shoppingCart.MoneyOrders.Find(x => x.Id == id);
						parkTransaction.Amount = moneyOrderTransactionDetails.Amount;
						parkTransaction.Detail = moneyOrderTransactionDetails.StatusDescription;
						parkTransaction.IconName = "MoneyOrderGreen.png";
						if (!string.IsNullOrEmpty(moneyOrderTransactionDetails.DiscountName))
						{
							parkTransaction.CheckMOPromotionAlertMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.CheckMOPromotionAlertMessage;
						}
						break;
				}
				if (isMobileEnabled)
				{
					if (!string.IsNullOrEmpty(customerSession.Customer.Phone1.Number) && customerSession.Customer.Phone1.Type == "Cell")
					{
						string phonenumber = customerSession.Customer.Phone1.Number;
						parkTransaction.MobileNumber = phonenumber.Substring(0, 3) + '-' + phonenumber.Substring(3, 3) + '-' + phonenumber.Substring(6);
					}
					else if (!string.IsNullOrEmpty(customerSession.Customer.Phone2.Number) && customerSession.Customer.Phone2.Type == "Cell")
					{
						string phonenumber = customerSession.Customer.Phone2.Number;
						parkTransaction.MobileNumber = phonenumber.Substring(0, 3) + '-' + phonenumber.Substring(3, 3) + '-' + phonenumber.Substring(6);
					}

					parkTransaction.InfoMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.SMSDisableAlert;
				}
				else
				{
					parkTransaction.InfoMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.SMSEnableAlert;
				}
				if (processorConfig != null)
				{
					if (processorConfig.ProcessorDisplayName == "Certegy" && parkTransaction.TransactionType == "Checks")
					{
						ViewBag.deleteCartId = id;
						return PartialView("_DeleteCartItemAlert", parkTransaction);
					}
				}

				if (parkTransaction.TransactionType == "GPRActivation")
				{
					ViewBag.Message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.VisaCardParkMessage;
					return PartialView("_RemoveTransaction", parkTransaction);
				}
				else
				{
					return PartialView("_ParkTransactionItem", parkTransaction);
				}
			}
			catch
			{
				ParkTransaction parkTransaction = new ParkTransaction();
				bool CanParkReceiveMoney = true;
				var providerConfig = parkTransaction.Providers.FirstOrDefault(x => x.ProcessorName == "WesternUnion" && x.Name == "ReceiveMoney");
				if (providerConfig != null)
				{
					CanParkReceiveMoney = providerConfig.CanParkReceiveMoney;
				}
				ViewBag.CanPark = CanParkReceiveMoney;
				return PartialView("_ParkTransactionItem", parkTransaction);
			}
		}

		// ShoppingCart Checkout - Parking Transaction
		public ActionResult ParkShoppingCartItem(string id, string screenName, string Product)
		{
			try
			{
				ProductType prodtype = ProductType.None;
				Enum.TryParse(Product, out prodtype);
				ShoppingCartHelper.ParkShoppingCartTrx(id, prodtype);
			}
			catch (Exception ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
			}
			return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
		}

		// ShoppingCart Checkout - popup - To Add Additonal Transaction
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult TransactionDone()
		{
			return PartialView("_AddTransaction");
		}

		//GPR - Alert for redirect to direct Load/Withdraw
		public ActionResult ShowGPRtrxConfirmationMassage()
		{
			return PartialView("_ShoppingCartCheckoutConfirmation");
		}


		// GPR WithDraw - Withdraw popup
		// AL-873: Shopping cart Server side calculation
		public ActionResult GPRWithdrawalAmountPopup(string withdrawAmt, string previousCashCollected, string withdrawFromCard)
		{
			decimal cashCollected = Convert.ToDecimal(previousCashCollected);
			ViewBag.withdrawAmt = withdrawAmt;
			ViewBag.cashCollected = cashCollected;
			Session["ShoppingCartCheckOutStatus"] = ShoppingCartCheckoutStatus.CashOverCounter;
			return PartialView("_GPRWithdrawAmountPopup");
		}

		// GPR WithDraw - Cash Collected
		public ActionResult CollectCashFromCustomerPopup(string collectcash)
		{
			double collectCash = Convert.ToDouble(collectcash);
			ViewBag.collectcash = Math.Round(collectCash, 2);
			ViewBag.CashOverCounter = false;
			Session["ShoppingCartCheckOutStatus"] = ShoppingCartCheckoutStatus.CashCollected;
			return PartialView("_CollectCashFromCustomerPopup");
		}

		//Money Order
		/// <summary>
		/// MoneyOrder scan get operation
		/// </summary>
		/// <returns></returns>
		public ActionResult ScanAMoneyOrder()
		{
			MoneyOrderImage moneyOrderImage = new MoneyOrderImage();

			return PartialView("_MoneyOrderScan", moneyOrderImage);
		}

		// Money Order
		/// <summary>
		/// MoneyOrder scan submit operation
		/// </summary>
		/// <param name="frontImage"></param>
		/// <param name="checkNumber"></param>
		/// <param name="npsId"></param>
		/// <returns></returns>
		public ActionResult ScanMoneyOrder(string frontImage, string checkNumber, string npsId, string micr, string accountNumber, string routingNumber)
		{
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

			//get moneyorder with processing state
			MoneyOrder moneyOrder = shoppingCart.MoneyOrders.Where(x => x.Status == Constants.STATUS_PROCESSING).First();

			//get Amount from current money order transaction
			decimal amount = moneyOrder.Amount;
			MoneyOrderDetails moneyOrderDetails = new MoneyOrderDetails()
			{
				FrontImage = frontImage,
				CheckNumber = checkNumber,
				RoutingNumber = routingNumber,
				AccountNumber = accountNumber,
				MICR = micr,
				Amount = amount,
				NpsId = npsId
			};

			return PartialView("_MoneyOrderConfirm", moneyOrderDetails);
		}

		// Money Order
		/// <summary>
		/// Replace MoneyOrder if MOcheck is not printed properly
		/// </summary>
		/// <returns></returns>
		public ActionResult ReplaceMoneyOrder()
		{
			return PartialView("_MoneyOrderReplaceInstruction");
		}

		//Money Order
		/// <summary>
		/// Money Order confirm Submit operation
		/// </summary>
		/// <param name="MoneyOrderImage"></param>
		/// <returns></returns>
		public ActionResult PrepareMoneyOrderCheck(MoneyOrderImage moneyOrderImage)
		{
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);
			Models.MoneyOrderConfirm moneyOrderConfirm = new MoneyOrderConfirm();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			//get moneyorder with processing state
			MoneyOrder moneyOrderProcessing = shoppingCart.MoneyOrders.Where(x => x.Status == Constants.STATUS_PROCESSING).First();

			MoneyOrder cxeMoneyOrder = desktop.GetMoneyOrderStage(long.Parse(customerSession.CustomerSessionId), long.Parse(moneyOrderProcessing.Id), mgiContext);

			MoneyOrderTransaction moneyOrderTransaction = new MoneyOrderTransaction()
			{
				CheckNumber = moneyOrderImage.CheckNumber,
				AccountNumber = moneyOrderImage.AccountNumber,
				RoutingNumber = moneyOrderImage.RoutingNumber,
				MICR = moneyOrderImage.MICR,
				FrontImage = string.IsNullOrEmpty(moneyOrderImage.FrontImage) ? null : Convert.FromBase64String(moneyOrderImage.FrontImage),
				BackImage = string.IsNullOrEmpty(moneyOrderImage.BackImage) ? null : Convert.FromBase64String(moneyOrderImage.BackImage)
			};

			desktop.UpdateMoneyOrder(long.Parse(customerSession.CustomerSessionId),
												 moneyOrderTransaction, long.Parse(moneyOrderProcessing.Id), mgiContext);

			if (shoppingCart.MoneyOrders.Where(x => x.Status == Constants.STATUS_PROCESSING).Any())
			{
				try
				{
					CheckPrint checkPrint = desktop.GenerateCheckPrintForMoneyOrder(long.Parse(moneyOrderProcessing.Id), long.Parse(customerSession.CustomerSessionId), mgiContext);
					moneyOrderConfirm.PrintData = ShoppingCartHelper.PrepareCheckForPrinting(checkPrint);
				}
				catch (Exception ex)
				{
					//If Check print template not generated then redirect to ScanAMoneyOrder view
					ViewBag.IsMoneyOrderPrintException = true;
					ViewBag.ExceptionMsg = ex.Message;
					return PartialView("_MoneyOrderPrint", moneyOrderConfirm);
				}
			}
			string amount = moneyOrderProcessing.Amount.ToString("0.00");
			moneyOrderConfirm.CheckNumber = moneyOrderImage.CheckNumber;
			moneyOrderConfirm.Amount = Convert.ToDecimal(amount);
			moneyOrderConfirm.AccountNumber = moneyOrderImage.AccountNumber;
			moneyOrderConfirm.RoutingNumber = moneyOrderImage.RoutingNumber;
			moneyOrderConfirm.NpsId = moneyOrderImage.NpsId;
			moneyOrderConfirm.MICR = moneyOrderImage.MICR;
			return PartialView("_MoneyOrderPrint", moneyOrderConfirm);

		}

		//Money Order Print 
		public ActionResult MoneyOrderPrintConfirm(MoneyOrderDetails moneyOrderImage)
		{
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);
			Models.MoneyOrderConfirm moneyOrderConfirm = new MoneyOrderConfirm();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			//get moneyorder with processing state
			MoneyOrder moneyOrderProcessing = shoppingCart.MoneyOrders.Where(x => x.Status == Constants.STATUS_PROCESSING).First();

			MoneyOrderTransaction moneyOrderTransaction = new MoneyOrderTransaction()
			{
				CheckNumber = moneyOrderImage.CheckNumber,
				AccountNumber = moneyOrderImage.AccountNumber,
				RoutingNumber = moneyOrderImage.RoutingNumber,
				MICR = moneyOrderImage.MICR,
				FrontImage = string.IsNullOrEmpty(moneyOrderImage.FrontImage) ? null : Convert.FromBase64String(moneyOrderImage.FrontImage),
				BackImage = string.IsNullOrEmpty(moneyOrderImage.BackImage) ? null : Convert.FromBase64String(moneyOrderImage.BackImage)
			};

			desktop.UpdateMoneyOrder(long.Parse(customerSession.CustomerSessionId),
												 moneyOrderTransaction, long.Parse(moneyOrderProcessing.Id), mgiContext);
			return PartialView("_MoneyOrderPrintConfirm", moneyOrderImage);
		}

		//Park Pending Check
		private bool isMobileEnabledForTextMessage(CustomerSession currentCustomer)
		{
			bool isEnabled = false;

			if (!string.IsNullOrEmpty(currentCustomer.Customer.Phone1.Number) && currentCustomer.Customer.Phone1.Type == "Cell" && currentCustomer.Customer.Preferences.SMSEnabled)
				isEnabled = true;
			else if (!string.IsNullOrEmpty(currentCustomer.Customer.Phone2.Number) && currentCustomer.Customer.Phone2.Type == "Cell" && currentCustomer.Customer.Preferences.SMSEnabled)
				isEnabled = true;

			return isEnabled;
		}

		//Money Order
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="screenName"></param>
		/// <param name="Product"></param>
		/// <returns></returns>
		public ActionResult MoneyOrderCancel()
		{
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			long customerSessionId = GetCustomerSessionId();
			ShoppingCart cart = desktop.ShoppingCart(customerSessionId.ToString());

			if (cart.MoneyOrders != null && cart.MoneyOrders.Where(m => m.Status == Constants.STATUS_PROCESSING).Any())
			{
				MoneyOrder moneyOrder = cart.MoneyOrders.Where(m => m.Status == Constants.STATUS_PROCESSING).OrderBy(m => m.Id).First();
				if (moneyOrder != null)
				{
					desktop.UpdateMoneyOrderStatus(customerSessionId, long.Parse(moneyOrder.Id), Convert.ToInt16(Constants.STATUS_FAILED), mgiContext);
					//desktop.RemoveMoneyOrder(customerSessionId, long.Parse(moneyOrder.Id));
				}
			}

			Session["ShoppingCartCheckOutStatus"] = ShoppingCartCheckoutStatus.MOPrintingCancelled;

			return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
		}

		//Check Franking US1421 changes
		private bool IsCheckFranking(MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			Desktop client = new Desktop();
			ChannelPartner channelpartner = client.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext);
			return channelpartner.IsCheckFrank;
		}

		// Check Franking US1421 changes
		private string[] getCheckFrankCount(ShoppingCart shoppingCart)
		{
			string[] checkdata = new string[shoppingCart.Checks.Count];
			int i = -1;

			foreach (Check check in shoppingCart.Checks)
			{
				checkdata[++i] = check.Id;
			}

			return checkdata;
		}

		//Check Franking US1421 changes
		public ActionResult DisplayCheckFrankingDetails(string Id, long dt, int chkslno)
		{
			try
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				long customerSessionId = GetCustomerSessionId();

				Desktop client = new Desktop();

				CheckTransactionDetails transaction = client.GetCheckTransaction(GetAgentSessionId(), customerSessionId, long.Parse(Id), new MGIContext());

				ChannelPartner channelpartner = client.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext);

				string frankdata = client.GetCheckFrankData(customerSessionId, long.Parse(Id), mgiContext);

				CheckFrankingDetailsViewModel chkfrankdetails = new CheckFrankingDetailsViewModel()
				{
					MICR = string.IsNullOrEmpty(transaction.CheckNumber) ? "0" : transaction.CheckNumber,
					Amount = transaction.Amount.ToString("0.00"),
					FrankData = frankdata,
					chkSlno = chkslno,
					DisplayMsg = string.Format(App_GlobalResources.Nexxo.CheckFrankingMessage, transaction.Amount.ToString("0.00")),
					TransactionID = transaction.Id.ToString()
				};

				TempData["FrankText"] = System.Web.HttpUtility.HtmlEncode(frankdata.Split('|')[0].Split(':')[1]);
				return PartialView("_partialCheckFrankingDetails", chkfrankdetails);
			}
			catch (Exception)
			{
				return PartialView("_partialCheckFrankingDetails", new CheckFrankingDetailsViewModel());

			}

		}

		//Check Franking US1421 changes
		public ActionResult DisplayCheckFrankingCancelDetails(int chkslno)
		{
			TempData["FrankText"] = System.Web.HttpUtility.HtmlDecode(Convert.ToString(TempData["FrankText"]).Replace("%26", ""));
			ViewBag.chkSlno = chkslno + 1;
			ViewBag.cancelText = TempData["FrankText"].ToString();
			TempData.Keep("FrankText");
			return PartialView("_partialCheckFrankingCancelDetails");
		}

		// Remove : Reload shoppingCart for Pending checks
		//[HttpPost]
		//public ActionResult GetShoppingCartPendingChecks()
		//{
		//    Session["IsReload"] = false;

		//    if (Convert.ToBoolean(Session["PendingChecks"]))
		//    {
		//        Desktop desktop = new Desktop();

		//        CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

		//        ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

		//        //Re verify the status of pending checks.
		//        if (shoppingCart.Checks != null)
		//        {
		//            var check = shoppingCart.Checks.Where(c => c.Status == Constants.STATUS_PENDING).FirstOrDefault();
		//            /*****************************Begin TA-91 Changes************************************/
		//            //        User Story Number: TA-91 | Web |   Developed by: Sunil Shetty      Date: 26.02.2015
		//            //        Purpose:  if any check status changes then tempdata is created in GetShoppingCartSummary. that tempadata is checked here. so that if true then we can refresh shoopingcartcheckout page 
		//            if (check == null || Convert.ToBoolean(TempData["PendingCheckCountChanged"]))
		//            {
		//                if (shoppingCart.Checks.Where(c => c.Status == Constants.STATUS_PENDING).Count() == 0)
		//                {
		//                    Session["PendingChecks"] = false;
		//                }
		//                Session["IsReload"] = true;
		//            }
		//            /****************************END TA-91 Changes************************************/
		//        }
		//    }

		//    var jsonData = new
		//    {
		//        reload = Convert.ToBoolean(Session["IsReload"])
		//    };

		//    return Json(jsonData);
		//}

		// Check Franking

		[HttpPost]
		public JsonResult UpdateCheckTransactionFranked(string transactionId)
		{
			Desktop desktop = new Desktop();

			long customerSessionId = GetCustomerSessionId();

			desktop.UpdateCheckTransactionFranked(customerSessionId, long.Parse(transactionId), new MGIContext());

			var jsonData = new { success = true };

			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		// Remove Item from ShoppingCart
		private bool RemoveShoppingCartTrx(string id, ProductType productType)
		{
			Desktop desktop = new Desktop();
			ShoppingCart shoppingCart = new ShoppingCart();
			bool isItemRemoved = false;

			if (Session["CustomerSession"] == null)
				throw new Exception("Customer session not initiated");
			try
			{
				long customerSessionId = GetCustomerSessionId();
				switch (productType)
				{
					case ProductType.Checks:
						MGI.Channel.DMS.Server.Data.MGIContext mgiContext = GetCheckLogin(customerSessionId);
						desktop.RemoveCheque(customerSessionId, long.Parse(id), mgiContext);
						isItemRemoved = true;
						break;

					case ProductType.BillPay:
						desktop.RemoveBill(customerSessionId, long.Parse(id));
						isItemRemoved = true;
						break;

					case ProductType.ReceiveMoney:
						desktop.RemoveMoneyTransfer(customerSessionId, long.Parse(id));
						isItemRemoved = true;
						break;

					case ProductType.SendMoney:
						shoppingCart = desktop.ShoppingCart(customerSessionId.ToString());
						if (shoppingCart.MoneyTransfers.Where(x => x.TransactionSubType == ((int)SharedData.TransactionSubType.Cancel).ToString()
						  || x.TransactionSubType == ((int)SharedData.TransactionSubType.Modify).ToString()).Any())
						{

							long originalTransactionId = shoppingCart.MoneyTransfers.Where(x => x.Id == id).FirstOrDefault().OriginalTransactionId;

							string trxId = shoppingCart.MoneyTransfers.Where(x => x.OriginalTransactionId == originalTransactionId
											 && x.TransactionSubType == ((int)SharedData.TransactionSubType.Cancel).ToString()).FirstOrDefault().Id;
							desktop.RemoveMoneyTransfer(customerSessionId, long.Parse(trxId));

							trxId = shoppingCart.MoneyTransfers.Where(x => x.OriginalTransactionId == originalTransactionId
										 && x.TransactionSubType == ((int)SharedData.TransactionSubType.Modify).ToString()).FirstOrDefault().Id;
							desktop.RemoveMoneyTransfer(customerSessionId, long.Parse(trxId));
						}
						else
							desktop.RemoveMoneyTransfer(customerSessionId, long.Parse(id));
						isItemRemoved = true;
						break;

					case ProductType.GPRActivation:
						shoppingCart = desktop.ShoppingCart(customerSessionId.ToString());
						desktop.RemoveFunds(customerSessionId, long.Parse(id));
						if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_LOAD).Any())
						{
							string loadTxnId = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_LOAD).FirstOrDefault().Id;
							desktop.RemoveFunds(customerSessionId, long.Parse(loadTxnId));
						}
						isItemRemoved = true;
						break;

					case ProductType.GPRLoad:
						shoppingCart = desktop.ShoppingCart(customerSessionId.ToString());
						if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).Any())
						{
							string activateTxnId = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).FirstOrDefault().Id;
							if (!string.IsNullOrEmpty(activateTxnId))
							{
								desktop.RemoveFunds(customerSessionId, long.Parse(activateTxnId));
							}
						}
						isItemRemoved = true;
						desktop.RemoveFunds(customerSessionId, long.Parse(id));
						break;

					case ProductType.GPRWithdraw:
					case ProductType.AddOnCard:
						desktop.RemoveFunds(customerSessionId, long.Parse(id));
						isItemRemoved = true;
						break;

					case ProductType.MoneyOrder:
						desktop.RemoveMoneyOrder(customerSessionId, long.Parse(id));
						isItemRemoved = true;
						break;

					case ProductType.CashIn:
						desktop.RemoveCashIn(customerSessionId, long.Parse(id));
						break;
				}
				return isItemRemoved;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// TCF - Post Flush
		public ActionResult PostFlushShoppingCart()
		{
			Desktop desktop = new Desktop();
			string result = string.Empty;
			long customerSessionId = GetCustomerSessionId();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			mgiContext.Context = new Dictionary<string, object>();
			try
			{
				mgiContext.Context.Add("SSOAttributes", GetSSOAgentSession("SSO_AGENT_SESSION"));
				desktop.PostFlush(customerSessionId, mgiContext);
			}
			catch (Exception ex)
			{
				result = Convert.ToString(ex.Message);
			}

			return Json(new { success = true, ErrorMsg = result }, JsonRequestBehavior.AllowGet);
		}

		// Certegy related Changes
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public ActionResult ShowCertegyConfirmationPopup(string CheckStatus)
		{
			return PartialView("_CertegyApprovedPopUp");
		}

		#region AL-873: Shopping cart Server side calculation

		/// <summary>
		/// This method is to calculate cash to customer amount during withdraw from card
		/// </summary>
		/// <param name="netDueToCustomer">net due to customer amount</param>
		/// <param name="withdrawFrmCardAmount">withdraw amount from card</param>
		/// <param name="previousCashCollected">previous cash collected from the customer</param>
		/// <returns>Cash to customer amount</returns>
		public JsonResult WithdrawChangeFunction(string netDueToCustomer, string withdrawFrmCardAmount, string previousCashCollected)
		{
			decimal cashToCustomer = Convert.ToDecimal(netDueToCustomer) + Convert.ToDecimal(withdrawFrmCardAmount) + Convert.ToDecimal(previousCashCollected);
			var jsonData = new
			{
				cashToCustomer = cashToCustomer.ToString("0.00")
			};
			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// This method is to calculate change due amount during withdraw,load to card,cash collected 
		/// </summary>
		/// <param name="cashCollected">cash collected from the customer</param>
		/// <param name="cashToCustomer">cash to customer amount</param>
		/// <param name="isGprShoppingCartItemExists">Whether GPR cart items exists or not</param>
		/// <returns>change due amount and status of ReCalc button</returns>
		public JsonResult getChangeDue(string cashCollected, string cashToCustomer, string previousCashCollected, string isGprShoppingCartItemExists)
		{
			decimal sum = Convert.ToDecimal(cashCollected) + (Convert.ToDecimal(cashToCustomer) - Convert.ToDecimal(previousCashCollected));
			var jsonData = new
			{
				sum = sum.ToString("0.00"),
				isRecalc = IsReCalcEnabled(sum, Convert.ToBoolean(isGprShoppingCartItemExists.ToLower()))
			};
			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// This method is to calculate cash to customer amount during load the amount to card
		/// </summary>
		/// <param name="netDueToCustomer">net due to customer amount</param>
		/// <param name="loadToCardAmount">load to card amount</param>
		/// <param name="previousCashCollected">previous cash collected from the customer</param>
		/// <returns>Cash to customer amount</returns>
		public JsonResult LoadToCardChangeFunction(string netDueToCustomer, string loadToCardAmount, string previousCashCollected)
		{
			decimal cashToCustomer = (Convert.ToDecimal(netDueToCustomer) - Convert.ToDecimal(loadToCardAmount)) + Convert.ToDecimal(previousCashCollected);
			var jsonData = new
			{
				cashToCustomer = cashToCustomer.ToString("0.00")
			};
			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// This method is to calculate load to card(either load or withdraw) amount while collecting the cash from customer
		/// </summary>
		/// <param name="totalDueToCustomer">Due to customer amount</param>
		/// <param name="netDueToCustomer">Net cash amount</param>
		/// <returns>Load  to card amount(either load or withdraw)</returns>
		public JsonResult CashToCustomerChangeFunction(string totalDueToCustomer, string netDueToCustomer)
		{
			decimal LoadToCard = (Convert.ToDecimal(totalDueToCustomer) - Convert.ToDecimal(netDueToCustomer));
			var jsonData = new
			{
				loadToCard = LoadToCard.ToString("0.00")
			};
			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// This method is to calculate net due to customer amount while changing the Net Due amount
		/// </summary>
		/// <param name="netDueToCustomer">Net due to custome amount</param>
		/// <param name="loadToCardAmount">Load to cart amount</param>
		/// <param name="previousCashCollected">Previous cash collected from the customer</param>
		/// <param name="isGpr"></param>
		/// <returns>Net due to customer amount</returns>
		public JsonResult NetDueToCustomerFunction(string netDueToCustomer, string loadToCardAmount, string previousCashCollected, string isGpr)
		{
			decimal netDueToCust;
			if (isGpr == "True")
			{
				netDueToCust = (Convert.ToDecimal(netDueToCustomer) - Convert.ToDecimal(loadToCardAmount)) + Convert.ToDecimal(previousCashCollected);
			}
			else
			{
				netDueToCust = Convert.ToDecimal(netDueToCustomer) + Convert.ToDecimal(previousCashCollected);
			}

			var jsonData = new
			{
				netDueToCust = netDueToCust.ToString("0.00")
			};
			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// This method is to show/hide the ReCalc button
		/// </summary>
		/// <param name="changeDue">Change due  amount</param>
		/// <param name="isGprShoppingCartItemExists">Whether the GPR cart items exists or not</param>
		/// <returns>Status of ReCalc button</returns>
		private bool IsReCalcEnabled(decimal changeDue, bool isGprShoppingCartItemExists)
		{
			bool isRecalc = false;

			//1. if pending checks, then display Recalc
			if (bool.Parse(Session["PendingChecks"].ToString()) == true)
				isRecalc = true;
			else if (changeDue < 0) // if ChangeDue is less than zero, Cart is not balanced. So display Recalc
				isRecalc = true;
			else if (!isGprShoppingCartItemExists) // if GPR customer and shopping Cart item count is 0, then display Recalc. 
				isRecalc = true;

			return isRecalc;
		}


		#endregion

	}

}
