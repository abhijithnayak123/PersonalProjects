using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using CustomerSession = MGI.Channel.Shared.Server.Data.CustomerSession;
using FundType = MGI.Channel.Shared.Server.Data.FundType;
using SharedData = MGI.Channel.Shared.Server.Data;
using System.Collections;


namespace MGI.Channel.DMS.Web.Controllers
{
	public class TransactionHistoryController : BaseController
	{
		[HttpGet]
		public ActionResult TransactionHistory()
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Session["activeButton"] = "transactionhistory";
			TransactionHistoryModel customerTranHistory = new TransactionHistoryModel()
			{
				Locations = GetLocations(mgiContext)
			};
			Session.Remove("TransactionType");

			ViewBag.Navigation = Resources.NexxoSiteMap.TransactionHistory;
			return View("TransactionHistory", customerTranHistory);
		}

		[HttpPost]
		public ActionResult GetTransactionHistory(string alloyId, int page = 1, int rows = 5)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop client = new Desktop();
			long customerId = Convert.ToInt64(alloyId);
			string transactionType = Convert.ToString(Session["TransactionType"]) == "0" ? string.Empty : Convert.ToString(Session["TransactionType"]);
			long locationId = Convert.ToInt64(Session["Location"]);
			string location = string.Empty;
			if (locationId > 0)
			{
				location = client.GetLocationDetailsForEdit(Convert.ToString(GetAgentSessionId()), locationId, mgiContext).LocationName;
			}
			double period = Session["DateRange"] == null ? 60 : Convert.ToDouble(Session["DateRange"]);
			DateTime dateRange = DateTime.Now.AddDays(-period);

			try
			{
				var transactions = client.GetTransactionHistory(GetCustomerSessionId(), customerId, transactionType, location, dateRange, mgiContext).AsQueryable();
				IQueryable<TransactionHistory> filteredtransactions = transactions;

				var sortedtransactions = filteredtransactions;
				var totalRecords = filteredtransactions.Count();
				var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

				var data = (from s in sortedtransactions
							select new
							{
								id = s.TransactionId,
								cell = new object[] { s.TransactionDate.ToString(), s.Teller, s.SessionId.ToString(), s.TransactionId.ToString(), s.Location, s.TransactionType, s.TransactionStatus, s.TransactionDetail, s.TotalAmount.ToString(("C2")) }
							}
						).ToArray();
				page = page >= totalPages ? totalPages : page;
				var jsonData = new
				{
					total = totalPages,
					page = page,
					records = totalRecords,
					rows = data.Skip((page - 1) * rows).Take(rows)
				};

				return Json(jsonData);
			}
			catch (Exception exception)
			{
				ViewBag.ExceptionMessage = exception.Message;
				TransactionHistoryModel customerTranHistory = new TransactionHistoryModel()
				{
					Locations = GetLocations(mgiContext)
				};
				return View("TransactionHistory", customerTranHistory);
			}
		}

		[HttpPost]
		public ActionResult TransactionHistory(TransactionHistoryModel transactionHistoryModel)
		{
			Session["Location"] = transactionHistoryModel.Location;
			Session["DateRange"] = transactionHistoryModel.DateRange;
			Session["TransactionType"] = transactionHistoryModel.TransactionType;
			Session["CustomerSessionId"] = transactionHistoryModel.customerSession.CustomerSessionId;
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			TransactionHistoryModel transactionHistory = new TransactionHistoryModel()
			{
				Locations = GetLocations(mgiContext),
				Location = transactionHistoryModel.Location,
				DateRange = transactionHistoryModel.DateRange,
				TransactionType = transactionHistoryModel.TransactionType
			};

			return View("TransactionHistory", transactionHistory);
		}

		[HttpPost]
		public ActionResult GetAgentTransHistory(int page = 1, int rows = 5)
		{
			Desktop client = new Desktop();
            long agentSessionId = GetAgentSessionId();
			int currentAgentId = Session["agentId"] == null ? 0 : Convert.ToInt32(Session["agentId"]);
			bool showAllReport = Session["ShowAllReport"] == null ? false : (bool)Session["ShowAllReport"];
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            UserDetails currentAgent = client.GetUser(agentSessionId, currentAgentId, mgiContext);

			string location = string.Empty;

			long? agentId = Session["DropDownAgentId"] == null || (string)Session["DropDownAgentId"] == "Select" ? currentAgentId : Convert.ToInt64(Session["DropDownAgentId"]);

			long transactionId = Session["TransactionID"] == null ? 0 : Convert.ToInt64(Session["TransactionID"]);

			location = Session["CurrentLocation"] == null ? string.Empty : Session["CurrentLocation"].ToString();
			int duration = Convert.ToInt32(ConfigurationManager.AppSettings["AgentTransactionHistoryDuration"]);
            if (currentAgent.UserRoleId == (int)UserRoles.Manager || currentAgent.UserRoleId == (int)UserRoles.SystemAdmin || currentAgent.UserRoleId== (int)UserRoles.ComplianceManager||currentAgent.UserRoleId== (int)UserRoles.Tech)
			{
				//agentId = currentAgent.Id == agentId ? null : agentId;

                agentId = Session["DropDownAgentId"]  == null ? null : agentId;

			}

			string transactionType = Session["TransactionType"] == null ? string.Empty : Session["TransactionType"].ToString();

			try
			{
				var transactions = client.GetTransactionHistory(agentSessionId, agentId, transactionType, location, showAllReport, transactionId, duration, mgiContext).AsQueryable();


				IQueryable<TransactionHistory> filteredtransactions = transactions;

				var sortedtransactions = filteredtransactions;
				var totalRecords = filteredtransactions.Count();
				var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

				var data = (from s in sortedtransactions
							select new
							{
								id = s.TransactionId,
								cell = new object[] { s.TransactionDate.ToString(), s.Teller, s.SessionId.ToString(), s.TransactionId.ToString(), s.CustomerName, s.TransactionStatus, s.TransactionType, s.TransactionDetail, s.TotalAmount.ToString(("C2")) }
							}
						).ToArray();
				page = page >= totalPages ? totalPages : page;
				var jsonData = new
				{
					total = totalPages,
					page = page,
					records = totalRecords,
					rows = data.Skip((page - 1) * rows).Take(rows)
				};

				return Json(jsonData);
			}
			catch (Exception exception)
			{
				ViewBag.ExceptionMessage = exception.Message;
				AgentTransactionHistory agentTranHistory = new AgentTransactionHistory()
				{
                    Agents = client.GetUsers(agentSessionId, currentAgent.LocationId, mgiContext)
				};
				return View("AgentTransHistory", agentTranHistory);
			}
		}

		public ActionResult GetAgentTransactionHistory()
		{
            Session["activeButton"] = "transhistory";
            long agentSessionId = GetAgentSessionId();
			Desktop client = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			AgentTransactionHistory agentTranHistory = new AgentTransactionHistory();

			int currentAgentId = Session["AgentId"] == null ? 0 : Convert.ToInt32(Session["AgentId"]);

			Session["DropDownAgentId"] = null;
			Session["TransactionType"] = null;
			Session["ShowAllReport"] = null;
			Session["ApplyCriteria"] = false;
            UserDetails currentAgent = client.GetUser(agentSessionId, currentAgentId, mgiContext);
			ViewBag.IsAgentTeller = false;
            if (currentAgent.UserRoleId == (int)UserRoles.Manager || currentAgent.UserRoleId == (int)UserRoles.SystemAdmin || currentAgent.UserRoleId == (int)UserRoles.Tech || currentAgent.UserRoleId==(int)UserRoles.ComplianceManager)
            {
                agentTranHistory.Agents = client.GetUsers(agentSessionId, currentAgent.LocationId, mgiContext);

            }
            else if (currentAgent.UserRoleId == (int)UserRoles.Teller)
            {
                agentTranHistory.Agents = new List<SelectListItem>() { new SelectListItem() { Value = currentAgentId.ToString(), Text = currentAgent.FullName } };
                ViewBag.IsAgentTeller = true;
            }
            else
                agentTranHistory.Agents = new List<SelectListItem>() { new SelectListItem() { Value = "Select", Text = "Select" } };


			return View("AgentTransHistory", agentTranHistory);
		}

		[HttpPost]
		public ActionResult GetAgentTransactionHistory(AgentTransactionHistory agentTransactionHistory)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop client = new Desktop();
            long agentSessionId = GetAgentSessionId();
			Session["TransactionType"] = agentTransactionHistory.TransactionType;
			Session["DropDownAgentId"] = agentTransactionHistory.Agent;
			Session["ShowAllReport"] = agentTransactionHistory.IsTransactionStatusSelected;
			Session["ApplyCriteria"] = true;
			Session["TransactionID"] = agentTransactionHistory.TransactionID;

			AgentTransactionHistory agentTranHistory = new AgentTransactionHistory();

			int currentAgentId = Session["AgentId"] == null ? 0 : Convert.ToInt32(Session["AgentId"]);

            UserDetails currentAgent = client.GetUser(agentSessionId, currentAgentId, mgiContext);

			ViewBag.IsAgentTeller = false;

			if (currentAgent.UserRoleId == (int)UserRoles.Manager|| currentAgent.UserRoleId == (int)UserRoles.SystemAdmin || currentAgent.UserRoleId == (int)UserRoles.Tech || currentAgent.UserRoleId==(int)UserRoles.ComplianceManager)
                agentTranHistory.Agents = client.GetUsers(agentSessionId, currentAgent.LocationId, mgiContext);
			else if (currentAgent.UserRoleId == (int)UserRoles.Teller)
			{
				agentTranHistory.Agents = new List<SelectListItem>() { new SelectListItem() { Value = currentAgentId.ToString(), Text = currentAgent.FullName } };
				ViewBag.IsAgentTeller = true;
			}
			else
				agentTranHistory.Agents = new List<SelectListItem>() { new SelectListItem() { Value = "Select", Text = "Select" } };

			return View("AgentTransHistory", agentTranHistory);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult GetPopup(string transactionId)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop desktopClient = new Desktop();
			string sessionId = Session["sessionId"].ToString();
			string[] receiptData = desktopClient.GetReceiptData(transactionId, sessionId, mgiContext);
			string base64Str = PrepareReceiptForPrinting(receiptData);

			TransactionHistoryPopup transactionHistoryPopup = new TransactionHistoryPopup { FundPaymentId = transactionId, ReceiptData = base64Str };
			return PartialView("_TransactionHistoryPopup", transactionHistoryPopup);
		}

        public ActionResult DisplayTransactionDetails(string transactionId, long dt, string transactionType, string transactionStatus, bool isAgentSession = false, string CustSessionId = "")
		{
			try
			{
                long agentSessionId = GetAgentSessionId();

                long customerSessionId=0L;
                if(string.IsNullOrEmpty(CustSessionId))
                    customerSessionId = GetCustomerSessionId();
                else
                    customerSessionId = long.Parse(CustSessionId);

               // long customerSessionId = GetCustomerSessionId();


				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				if (isAgentSession)
					ViewBag.IsAgentSession = true;

				Desktop desktop = new Desktop();

				if (transactionType.ToLower().Contains("check"))
				{
					DMS.Server.Data.CheckTransactionDetails chktransaction = desktop.GetCheckTransaction(agentSessionId, customerSessionId, Convert.ToInt64(transactionId), mgiContext);
					string declineCode = chktransaction.DeclineErrorCode > 0 ? chktransaction.DeclineErrorCode + " - " : "";
					ProcessCheckTransactionDetailsViewModel tranCheckDetails = new ProcessCheckTransactionDetailsViewModel()
					{
						TransactionType = transactionType,
						TransactionId = transactionId,
						TransactionStatus = transactionStatus,
						Fee = chktransaction.BaseFee,
						Discount = chktransaction.DiscountApplied != 0 ? Math.Abs(chktransaction.DiscountApplied) : 0,
						PromotionName = chktransaction.DiscountApplied != 0 ? chktransaction.DiscountName : "NA",
						PromotionDescription = chktransaction.DiscountApplied != 0 ? chktransaction.DiscountDescription : "NA",
						NetFee = chktransaction.Fee,
						Total = chktransaction.Amount - chktransaction.Fee,
						Amount = chktransaction.Amount,
						CheckNumber = chktransaction.CheckNumber,
						CheckType = chktransaction.CheckType,
						ProviderName = ((ProviderIds)chktransaction.ProviderId).ToString(),
						Reason = declineCode + chktransaction.DeclineMessage
					};

					return PartialView("_ProcessCheckTransactionDetails", tranCheckDetails);

				}
				else if (transactionType.ToLower().Contains("prepaid"))
				{
					FundTransaction fundTransaction = desktop.GetFundTransaction(agentSessionId, customerSessionId, Convert.ToInt64(transactionId), mgiContext);
					PrepaidTransactionDetailsViewModel tranCheckDetails = new PrepaidTransactionDetailsViewModel()
					{
						TransactionType = transactionType,
						TransactionId = transactionId,
						TransactionStatus = transactionStatus,
						Fee = fundTransaction.Fee,
						Discount = 0,
						PromotionName = string.IsNullOrWhiteSpace(fundTransaction.PromoCode) ? "NA" : fundTransaction.PromoCode,
						PromotionDescription = "NA",
						NetFee = fundTransaction.Fee,
						Total = fundTransaction.Amount + fundTransaction.Fee,
						ProviderName = ((ProviderIds)fundTransaction.ProviderId).ToString(),
						CardNumber = GetDisplayCardNumber(fundTransaction.CardNumber),
						NewCardBalance = fundTransaction.CardBalance

					};

					if (fundTransaction.FundType == (int)FundType.None || fundTransaction.FundType == (int)FundType.AddOnCard)
					{
						return PartialView("_PrepaidActiveTransactionDetails", tranCheckDetails);
					}
					else if (fundTransaction.FundType == (int)FundType.Credit)
					{
						tranCheckDetails.LoadAmount = fundTransaction.Amount;
						return PartialView("_PrepaidLoadTransactionDetails", tranCheckDetails);
					}
					else
					{
						tranCheckDetails.WithdrawAmount = fundTransaction.Amount;
						return PartialView("_PrepaidWithdrawTransactionDetails", tranCheckDetails);
					}

				}
				else if (transactionType.ToLower().Contains("send"))
				{
					CustomerSession customerSession = new CustomerSession();
					customerSession = (CustomerSession)Session["CustomerSession"];

					SharedData.MoneyTransferTransaction moneytransferTransaction = desktop.GetMoneyTransferTransaction(agentSessionId, customerSessionId, Convert.ToInt64(transactionId), mgiContext);

					string receiverAddress = string.Empty;
					//receiverAddress += (string.IsNullOrEmpty(moneytransferTransaction.ReceiverAddress) ? string.Empty : moneytransferTransaction.ReceiverAddress);
					//receiverAddress += (string.IsNullOrEmpty(moneytransferTransaction.ReceiverCity) ? string.Empty : ", " + moneytransferTransaction.ReceiverCity);
					//receiverAddress += (string.IsNullOrEmpty(moneytransferTransaction.ReceiverState) ? string.Empty : ", " + moneytransferTransaction.ReceiverState);
					//receiverAddress += (string.IsNullOrEmpty(moneytransferTransaction.ReceiverCountry) ? string.Empty : ", " + moneytransferTransaction.ReceiverCountry);
					//receiverAddress += (string.IsNullOrEmpty(moneytransferTransaction.ReceiverZipCode) ? string.Empty : ", " + moneytransferTransaction.ReceiverZipCode);

					receiverAddress += (string.IsNullOrWhiteSpace(moneytransferTransaction.ReceiverAddress) ? string.Empty : moneytransferTransaction.ReceiverAddress);
					receiverAddress += (string.IsNullOrWhiteSpace(moneytransferTransaction.ReceiverCity) ? string.Empty : (string.IsNullOrWhiteSpace(receiverAddress) ? moneytransferTransaction.ReceiverCity : ", " + moneytransferTransaction.ReceiverCity));
					receiverAddress += (string.IsNullOrWhiteSpace(moneytransferTransaction.ReceiverState) ? string.Empty : (string.IsNullOrWhiteSpace(receiverAddress) ? moneytransferTransaction.ReceiverState : ", " + moneytransferTransaction.ReceiverState));
					receiverAddress += (string.IsNullOrWhiteSpace(moneytransferTransaction.ReceiverCountry) ? string.Empty : (string.IsNullOrWhiteSpace(receiverAddress) ? moneytransferTransaction.ReceiverCountry : ", " + moneytransferTransaction.ReceiverCountry));
					receiverAddress += (string.IsNullOrWhiteSpace(moneytransferTransaction.ReceiverZipCode) ? string.Empty : (string.IsNullOrWhiteSpace(receiverAddress) ? moneytransferTransaction.ReceiverZipCode : ", " + moneytransferTransaction.ReceiverZipCode));


					MoneyTransferTransactionDetailsViewModel tranDetails = new MoneyTransferTransactionDetailsViewModel()
					{

						TransactionType = transactionType,
						TransactionId = transactionId,
						TransactionStatus = transactionStatus,
						Fee = moneytransferTransaction.Fee + moneytransferTransaction.PromotionDiscount,
						Discount = moneytransferTransaction.PromotionDiscount * -1,
						PromotionName = string.IsNullOrEmpty(moneytransferTransaction.PromotionsCode) ? "NA" : moneytransferTransaction.PromotionsCode,
						PromotionDescription = "NA",
						TransferAmount = moneytransferTransaction.TransactionAmount,
						NetFee = moneytransferTransaction.Fee,
						Total = moneytransferTransaction.GrossTotalAmount,
						ReceiverName = (string.IsNullOrEmpty(moneytransferTransaction.ReceiverFirstName) ? " " : moneytransferTransaction.ReceiverFirstName) + " " + (string.IsNullOrEmpty(moneytransferTransaction.ReceiverLastName) ? " " : moneytransferTransaction.ReceiverLastName) + " " + (string.IsNullOrEmpty(moneytransferTransaction.ReceiverSecondLastName) ? " " : moneytransferTransaction.ReceiverSecondLastName),
						ReceiverAddress = receiverAddress,  //moneytransferTransaction.ReceiverAddress,  + receiverCity + receiverState + receiverCountry + receiverZipCode
						ProviderName = ((ProviderIds)moneytransferTransaction.ProviderId).ToString(),
						TestQuestion = moneytransferTransaction.TestQuestion,
						TestAnswer = moneytransferTransaction.TestAnswer,
						MTCN = moneytransferTransaction.ConfirmationNumber,
						isModifiedOrRefunded = moneytransferTransaction.IsModifiedOrRefunded,
                        TransactionSubType = moneytransferTransaction.TransactionSubType,
                        TransferTax = (moneytransferTransaction.MetaData.ContainsKey("TransferTax")) ? Convert.ToDecimal(NexxoUtil.GetDecimalDictionaryValueIfExists(moneytransferTransaction.MetaData, "TransferTax")) : moneytransferTransaction.TransferTax
					};
					if (customerSession != null)
					{
						ReasonRequest request = new ReasonRequest()
						{
							TransactionType = "REFUND"
						};
						tranDetails.RefundCategory = desktop.GetRefundReasons(long.Parse(customerSession.CustomerSessionId), request, mgiContext);
						var htSessions = (Hashtable)Session["HTSessions"];
						if (htSessions != null)
						{
							var agentSession = ((Server.Data.AgentSession)(htSessions["TempSessionAgent"]));
							if (agentSession != null)
							{
								string stateCode = agentSession.Terminal.Location.State;
								if (!string.IsNullOrWhiteSpace(stateCode))
								{
									bool isLocationState = desktop.IsSWBStateXfer(long.Parse(customerSession.CustomerSessionId), stateCode, mgiContext);
									if (isLocationState)
									{
										CashierDetails cashierDetails = desktop.GetAgentXfer(long.Parse(agentSession.SessionId), mgiContext);
										tranDetails.isCashierState = true;
										tranDetails.AgentFirstName = cashierDetails.AgentFirstName;
										tranDetails.AgentLastName = cashierDetails.AgentLastName;
									}
								}
							}
						}		

						string payStatus = string.Empty;

						try
						{
							if ((ProviderIds)moneytransferTransaction.ProviderId == ProviderIds.MoneyGram && moneytransferTransaction.IsModifiedOrRefunded == false
								&& (moneytransferTransaction.TransactionSubType == "2" || moneytransferTransaction.TransactionSubType == null))
							{
								SendMoneySearchRequest sendMoneySearchRequest = new SendMoneySearchRequest()
																					{
																						ConfirmationNumber = moneytransferTransaction.ConfirmationNumber,
																						SearchRequestType = SearchRequestType.Modify
																					};

								SendMoneySearchResponse sendMoneySearchResponse = desktop.SendMoneySearch(Convert.ToInt64(customerSession.CustomerSessionId),
																											sendMoneySearchRequest, mgiContext);
								payStatus = sendMoneySearchResponse.TransactionStatus;
							}
							else if ((ProviderIds)moneytransferTransaction.ProviderId == ProviderIds.WesternUnion)
							{
								payStatus = desktop.GetStatus(long.Parse(customerSession.CustomerSessionId), moneytransferTransaction.ConfirmationNumber, mgiContext);
							}
						}
						catch (Exception ex)
						{
							ViewBag.Errormsg = ex.Message;
						}

						tranDetails.PayStatus = payStatus;

						ShoppingCart shoppingCart = new ShoppingCart();

						shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

						if (shoppingCart.MoneyTransfers != null && shoppingCart.MoneyTransfers.Count > 0)
							tranDetails.isAddedtoShoppingCart = shoppingCart.MoneyTransfers.Where(x => x.OriginalTransactionId == Convert.ToInt64(transactionId)).Any();

						tranDetails.PayStatusCodeMessage = GetPayStatusMessage(tranDetails.PayStatus);
					}


					return PartialView("_SendMoneyTransactionDetails", tranDetails);
				}
				else if (transactionType.ToLower().Contains("receive"))
				{
					SharedData.MoneyTransferTransaction moneytransferTransaction = desktop.GetMoneyTransferTransaction(agentSessionId, customerSessionId, Convert.ToInt64(transactionId), mgiContext);

					MoneyTransferTransactionDetailsViewModel tranDetails = new MoneyTransferTransactionDetailsViewModel()
					{
						TransactionType = transactionType,
						TransactionId = transactionId,
						TransactionStatus = transactionStatus,
						Fee = moneytransferTransaction.Fee,
						Discount = 0,
						PromotionName = "NA",
						PromotionDescription = "NA",
						//TransferAmount = moneytransferTransaction.TransactionAmount,
                        TransferAmount = moneytransferTransaction.AmountToReceiver,
						NetFee = moneytransferTransaction.Fee,
                        //Total = moneytransferTransaction.TransactionAmount + moneytransferTransaction.Fee,
                        Total = moneytransferTransaction.AmountToReceiver + moneytransferTransaction.Fee,
						SenderName = moneytransferTransaction.SenderName,
						ProviderName = ((ProviderIds)moneytransferTransaction.ProviderId).ToString(),

						MTCN = moneytransferTransaction.ConfirmationNumber
					};
					return PartialView("_ReceiveMoneyTransactionDetails", tranDetails);
				}
				else if (transactionType.ToLower().Contains("moneyorder"))
				{
					DMS.Server.Data.MoneyOrderTransaction MOtransaction = desktop.GetMoneyOrderTransaction(agentSessionId, customerSessionId, long.Parse(transactionId), mgiContext);

					MoneyOrderTransactionDetailsViewModel tranMoneyOrderDetails = new MoneyOrderTransactionDetailsViewModel()
					{
						TransactionType = transactionType,
						TransactionId = transactionId,
						TransactionStatus = transactionStatus,
						Fee = MOtransaction.BaseFee,
						Discount = MOtransaction.DiscountApplied != 0 ? Math.Abs(MOtransaction.DiscountApplied) : 0,
						PromotionName = MOtransaction.DiscountApplied != 0 ? MOtransaction.DiscountName : "NA",
						PromotionDescription = MOtransaction.DiscountApplied != 0 ? MOtransaction.DiscountDescription : "NA",
						NetFee = MOtransaction.Fee,
						Total = MOtransaction.Amount + MOtransaction.Fee,

						Amount = MOtransaction.Amount,
						CheckNumber = MOtransaction.CheckNumber,
						ProviderName = ((ProviderIds)MOtransaction.ProviderId).ToString()
					};
					return PartialView("_MoneyOrderTransactionDetails", tranMoneyOrderDetails);
				}
				else if (transactionType.ToLower().Contains("billpay"))
				{
					DMS.Server.Data.BillPayTransaction transaction = desktop.GetBillPayTransaction(agentSessionId, customerSessionId, long.Parse(transactionId), mgiContext);

					string promoCode = Convert.ToString(NexxoUtil.GetDictionaryValueIfExists(transaction.MetaData, "PromoCoupon"));
					promoCode = string.IsNullOrWhiteSpace(promoCode) ? "NA" : promoCode;

					BillPayTransactionDetailsViewModel billPayModel = new BillPayTransactionDetailsViewModel()
					{
						TransactionType = transactionType,
						TransactionId = transactionId,
						TransactionStatus = transactionStatus,
						Fee = (transaction.MetaData.ContainsKey("UnDiscountedFee")) ? Convert.ToDecimal(NexxoUtil.GetDecimalDictionaryValueIfExists(transaction.MetaData, "UnDiscountedFee")) : transaction.Fee,
						Discount = Convert.ToDecimal(NexxoUtil.GetDecimalDictionaryValueIfExists(transaction.MetaData, "DiscountedFee")),
						Amount = transaction.Amount,
						PromotionName = promoCode,
						PromotionDescription = "NA",
						NetFee = transaction.Fee,
						Total = transaction.Amount + transaction.Fee,

						ProviderName = ((ProviderIds)transaction.ProviderId).ToString(),
						Payee = transaction.BillerName,
                        AccountNumber = transaction.AccountNumber.MaskAccountNumber(),
						TenantId = transaction.MetaData.ContainsKey("TenantId") ? transaction.MetaData["TenantId"].ToString() : "",
						MTCN = transaction.MetaData.ContainsKey("MTCN") ? transaction.MetaData["MTCN"] == null ? string.Empty : transaction.MetaData["MTCN"].ToString() : "",
					};
					return PartialView("_BillPayTransactionDetails", billPayModel);
				}
				else if (transactionType.ToLower().Contains("cash"))
				{
					CashTransaction transaction = desktop.GetCashTransaction(agentSessionId, customerSessionId, long.Parse(transactionId), mgiContext);
					CashTransactionDetailsViewModel cashModel = new CashTransactionDetailsViewModel()
					{
						Amount = transaction.Amount,
						TransactionId = transactionId,
						TransactionStatus = transaction.TransactionStatus,
						TransactionType = transaction.TransactionType
					};
					return PartialView("_CashTransactionDetails", cashModel);
				}
			}
			catch (Exception ex)
			{
				if (transactionType.ToLower().Contains("check"))
					return PartialView("_ProcessCheckTransactionDetails", new ProcessCheckTransactionDetailsViewModel());
				else if (transactionType.ToLower().Contains("prepaid"))
					return PartialView("_PrepaidActiveTransactionDetails", new PrepaidTransactionDetailsViewModel());
				else if (transactionType.ToLower().Contains("send"))
				{
					ViewBag.Errormsg = ex.Message;
					return PartialView("_SendMoneyTransactionDetails", new MoneyTransferTransactionDetailsViewModel());
				}
				else if (transactionType.ToLower().Contains("receive"))
					return PartialView("_ReceiveMoneyTransactionDetails", new MoneyTransferTransactionDetailsViewModel());
				else if (transactionType.ToLower().Contains("moneyorder"))
					return PartialView("_MoneyOrderTransactionDetails", new ProcessCheckTransactionDetailsViewModel());
				else if (transactionType.ToLower().Contains("billpay"))
					return PartialView("_BillPayTransactionDetails", new BillPayTransactionDetailsViewModel());
			}

			return RedirectToAction("TransactionHistory");
		}

		public JsonResult GetReceiptData(string transactionId, long dt, string transactiontype, bool isSummaryReceiptRequired = false, bool isReprint = false)
		{
			Desktop desktop = new Desktop();

			long agentSessionId = long.Parse(Session["sessionId"].ToString());
            long customerSessionId = GetCustomerSessionId();
			List<ReceiptData> receipts = new List<ReceiptData>();
			Dictionary<string, object> ssoCookie = GetSSOAgentSession("SSO_AGENT_SESSION");

			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			mgiContext.AgentSessionId = agentSessionId;
			if (ssoCookie != null && ssoCookie.ContainsKey("TellerNum"))
			{
				string tellerNumber = Convert.ToString(ssoCookie["TellerNum"]);
				mgiContext.TellerNumber = tellerNumber;
			}
			
			if (isSummaryReceiptRequired)
			{
				receipts = desktop.GetSummaryReceiptForReprint(agentSessionId, customerSessionId, Convert.ToInt64(transactionId), transactiontype, mgiContext);

				var jsonSummaryReceipt = new
				{
					data = receipts,
					success = true
				};

				return Json(jsonSummaryReceipt, JsonRequestBehavior.AllowGet);
			}

            if (transactiontype.ToLower().Contains("check"))
            {
                receipts = desktop.GetCheckReceipt(agentSessionId, customerSessionId, Convert.ToInt64(transactionId), isReprint, mgiContext);
            }
			else if (transactiontype.ToLower().StartsWith("gpr") || transactiontype.ToLower().StartsWith("prepaid") || transactiontype.ToLower().StartsWith("companion"))
            {
                receipts = desktop.GetFundReceipt(agentSessionId, customerSessionId, Convert.ToInt64(transactionId), isReprint, mgiContext);
            }
            else if (transactiontype.ToLower().Contains("send") || transactiontype.ToLower().Contains("receive") || transactiontype.ToLower().Contains("refund"))
            {
                receipts = desktop.GetMoneyTransferReceipt(agentSessionId, customerSessionId, Convert.ToInt64(transactionId), isReprint, mgiContext);
            }
            else if (transactiontype.ToLower().Contains("moneyorder"))
            {
                receipts = desktop.GetMoneyOrderReceipt(agentSessionId, customerSessionId, long.Parse(transactionId), isReprint, mgiContext);
            }
            else if (transactiontype.ToLower().Contains("billpay"))
            {
                receipts = desktop.GetBillPayReceipt(agentSessionId, customerSessionId, long.Parse(transactionId), isReprint, mgiContext);
            }
            else if (transactiontype.ToLower().Contains("summary"))
            {
                receipts = desktop.GetSummaryReceipt(customerSessionId, long.Parse(transactionId), mgiContext);
            }
			else if (transactiontype.ToLower().Contains("coupon"))
			{
				receipts = desktop.GetCouponCodeReceipt(customerSessionId, mgiContext);
			}
            if (receipts.Count < 1)
            {
                return Json(new { data = "Receipt Template Not Found", success = false });
            }

			// var receiptData = (from s in receiptsData select new { Name = s.Name, Line = s.PrintData }).ToArray();

			var jsonData = new
			{
				data = receipts.ToArray(),
				success = true
			};

			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetDoddfrankReceiptData(string transactionId, long dt)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop desktop = new Desktop();
			List<ReceiptData> data = new List<ReceiptData>();
            long customerSessionId = GetCustomerSessionId();
			data = desktop.GetDoddfrankReceipt(long.Parse(Session["sessionId"].ToString()), customerSessionId, long.Parse(transactionId), mgiContext);

			if (data == null)
			{
				return Json(new { data = "Receipt Template Not Found", success = false });
			}

			var jsonData = new
			{
				data = data.ToArray(),
				success = true
			};

			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCheckDeclinedReceiptData(string transactionId, long dt)
		{
			Desktop desktop = new Desktop();
			List<ReceiptData> data = new List<ReceiptData>();
			CustomerSession currentCustomer = (CustomerSession)Session["CustomerSession"];
			long customerSessionId = Convert.ToInt64(currentCustomer.CustomerSessionId);
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			long agentSessionId = long.Parse(Session["sessionId"].ToString());
			data = desktop.GetCheckDeclinedReceiptData(agentSessionId, customerSessionId, long.Parse(transactionId), mgiContext);
			if (data == null)
			{
				return Json(new { data = "Receipt Template Not Found", success = false });
			}
			var jsonData = new
			{
				data = data.ToArray(),
				success = true
			};
			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}



		public JsonResult GetTransactionForModify(string MTCN)
		{
			Desktop desktop = new Desktop();

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			SendMoneySearchRequest request = new SendMoneySearchRequest()
			{
				ConfirmationNumber = MTCN,
				SearchRequestType = SearchRequestType.Modify
			};

			SendMoneySearchResponse moneyTransferModify = desktop.SendMoneySearch(Convert.ToInt64(customerSession.CustomerSessionId), request, mgiContext);

			var jsonData = new { success = true, moneyTransferModify.FirstName, moneyTransferModify.LastName, moneyTransferModify.SecondLastName, moneyTransferModify.TestQuestion, moneyTransferModify.TestAnswer, moneyTransferModify.MiddleName };

			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetRefundStatus(string MTCN)
		{
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			MoneyTransferTransactionDetailsViewModel tranDetails = new MoneyTransferTransactionDetailsViewModel();

			SendMoneySearchRequest searchRequest = new SendMoneySearchRequest()
			{
				ConfirmationNumber = MTCN,
				SearchRequestType = SearchRequestType.Refund
			};

			SendMoneySearchResponse response = desktop.SendMoneySearch(Convert.ToInt64(customerSession.CustomerSessionId), searchRequest, mgiContext);

			string RefundFlag = response.RefundStatus;

			string transactiontype = String.Empty;

			if (RefundFlag == "F")
			{
				transactiontype = "REFUND,F";
				tranDetails.RefundStatusDesc = "FULL REFUND AVAILABLE";
			}
			else if (RefundFlag == "N")
			{
				transactiontype = "REFUND,N";
				tranDetails.RefundStatusDesc = "PRINCIPAL REFUND AVAILABLE";
			}
			else if (RefundFlag == RefundType.FullAmount.ToString())
			{
				tranDetails.RefundStatusDesc = "FULL REFUND AVAILABLE";
			}
			else if (RefundFlag == RefundType.PrincipalAmount.ToString())
			{
				tranDetails.RefundStatusDesc = "PRINCIPAL REFUND AVAILABLE";
			}

			tranDetails.FeeRefund = response.FeeRefund;

			ReasonRequest request = new ReasonRequest()
			{
				TransactionType = transactiontype
			};

			tranDetails.RefundCategory = desktop.GetRefundReasons(long.Parse(customerSession.CustomerSessionId), request, mgiContext);
			tranDetails.RefundStatus = RefundFlag;

			var jsonData = new { success = true, tranDetails };

			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		//WU
		public JsonResult SendMoneyRefundSubmit(RefundSendMoneyRequest moneyTransferRefund)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop desktop = new Desktop();

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			var TrxId = desktop.MoneyTransferRefund(Convert.ToInt64(customerSession.CustomerSessionId), moneyTransferRefund, mgiContext);

			var jsonData = new { success = true };

			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		//MGI
		public ActionResult SendMoneyStageRefundSubmit(RefundSendMoneyRequest moneyTransferRefund)
		{
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			bool isAddedtoShoppingCart = false;

			ShoppingCart shoppingCart = new ShoppingCart();

			shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

			if (shoppingCart.MoneyTransfers != null && shoppingCart.MoneyTransfers.Count > 0)
				isAddedtoShoppingCart = shoppingCart.MoneyTransfers.Where(x => x.OriginalTransactionId == Convert.ToInt64(moneyTransferRefund.TransactionId)).Any();


			if (!isAddedtoShoppingCart)
			{
				long PtnrTrxId = desktop.StageRefundSendMoney(Convert.ToInt64(customerSession.CustomerSessionId), moneyTransferRefund, mgiContext);

				var jsonData = new { success = true };

				return Json(jsonData, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var jsonData = new { doRedirect = true };
				return Json(jsonData, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult SendMoneyModifySubmit(ModifySendMoneyRequest moneyTransferModify)
		{
			Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			ModifySendMoneyResponse response = desktop.StageModifySendMoney(Convert.ToInt64(customerSession.CustomerSessionId), moneyTransferModify, mgiContext);

			TempData["sendMoneyModifyIds"] = response;

			var jsonData = new { success = true };

			return Json(jsonData, JsonRequestBehavior.AllowGet);

		}

		public ActionResult SendMoneyRefundSuccess(string MTCN)
		{
			RefundSendMoneyRequest refundSendMoney = new RefundSendMoneyRequest();
			refundSendMoney.ConfirmationNumber = MTCN;
			return PartialView("_SendMoneyRefundSuccess", refundSendMoney);

		}

		public ActionResult SendMoneyRefundOK(RefundSendMoneyRequest moneyTrfRefund)
		{
			Session["activeButton"] = "Receive Money";
			ReceiveMoney receive = new ReceiveMoney();
			receive.WesternUnionMTCN = moneyTrfRefund.ConfirmationNumber;
			return View("ReceiveMoney", "_Common", receive);
		}

		public ActionResult SendMoneyModifySuccess(string MTCN)
		{
			RefundSendMoneyRequest MoneyTransferCancel = new RefundSendMoneyRequest();
			MoneyTransferCancel.ConfirmationNumber = MTCN;
			return PartialView("_SendMoneyModifySuccess", MoneyTransferCancel);
		}

		public ActionResult SendMoneyRefundFail()
		{
			RefundSendMoneyRequest MoneyTransferCancel = new RefundSendMoneyRequest();
			return PartialView("_SendMoneyRefundFail", MoneyTransferCancel);
		}

		public ActionResult SendMoneyConfirm()
        {
            Desktop desktop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Session["activeButton"] = "Send Money";

            if (TempData["sendMoneyModifyIds"] == null)
                return View();

            ModifySendMoneyResponse sendMoneyModifyIds = (ModifySendMoneyResponse)TempData["sendMoneyModifyIds"];

            TempData["sendMoneyModifyIds"] = sendMoneyModifyIds;

            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

            long transactionId = sendMoneyModifyIds.ModifyTransactionId;

            MoneyTransferTransaction mtTrx = desktop.GetMoneyTransferDetailsTransaction(Convert.ToInt64(customerSession.CustomerSessionId), transactionId, mgiContext);

            string expectedPayoutCityName = NexxoUtil.GetDictionaryValueIfExists(mtTrx.MetaData, "ExpectedPayoutCity");
            string deliveryOptionDesc = NexxoUtil.GetDictionaryValueIfExists(mtTrx.MetaData, "DeliveryOptionDesc");
            string receiveAgent = NexxoUtil.GetDictionaryValueIfExists(mtTrx.MetaData, "ReceiveAgentName");
            decimal transferTax = Convert.ToDecimal(NexxoUtil.GetDecimalDictionaryValueIfExists(mtTrx.MetaData, "TransferTax"));

            SendMoney sendMoney = new SendMoney()
            {
                TransactionId = transactionId,
                FirstName = mtTrx.ReceiverFirstName,
                LastName = mtTrx.ReceiverLastName,
                SecondLastName = mtTrx.ReceiverSecondLastName,
                PickUpLocation = mtTrx.ReceiverAddress,
                City = expectedPayoutCityName,
                StateProvince = mtTrx.Receiver.State_Province,
                Country = mtTrx.DestinationCountryCode,
                CountryCode = mtTrx.DestinationCountryCode,
                DeliveryMethodDesc = mtTrx.DeliveryServiceDesc,
                DeliveryOptionDesc = deliveryOptionDesc,
                PromoDiscount = mtTrx.PromotionDiscount,
                CouponPromoCode = mtTrx.PromotionsCode,
                TransferAmount = mtTrx.TransactionAmount,
                TransferFee = mtTrx.Fee,
                OriginalFee = mtTrx.Fee + mtTrx.PromotionDiscount,
                Amount = mtTrx.GrossTotalAmount,
                ReceiverCityName = mtTrx.Receiver.City,				
                PickupState = mtTrx.DestinationState,
                PickupCity = expectedPayoutCityName,
                ReceiveAgent = string.IsNullOrEmpty(receiveAgent) ? "NA" : receiveAgent,
                ReceiverName = string.Format("{0} {1} {2} {3}", mtTrx.Receiver.FirstName, mtTrx.Receiver.MiddleName, mtTrx.Receiver.LastName, mtTrx.Receiver.SecondLastName),
                TransferTax = transferTax
            };

            sendMoney.LCountry = desktop.GetXfrCountries(long.Parse(customerSession.CustomerSessionId), mgiContext);

			if (sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.Country) != null)
			{
				sendMoney.Country = sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.Country).Text;
				string countryCode = sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.CountryCode).Value;
				sendMoney.LStates = desktop.GetXfrStates(long.Parse(customerSession.CustomerSessionId), countryCode, mgiContext);
				if (sendMoney.LStates.FirstOrDefault(x => x.Value == mtTrx.Receiver.PickupState_Province) != null)
					sendMoney.PickupState = sendMoney.LStates.FirstOrDefault(x => x.Value == mtTrx.Receiver.PickupState_Province).Text;
			}

            if (sendMoney.LCountry.FirstOrDefault(x => x.Value == mtTrx.DestinationCountryCode) != null)
                sendMoney.PickupCountry = sendMoney.LCountry.FirstOrDefault(x => x.Value == mtTrx.DestinationCountryCode).Text;

            sendMoney.enableEditContinue = true;
            ViewBag.Navigation = Resources.NexxoSiteMap.SendMoney;
            if (sendMoney.CountryCode.ToLower() == "us" || sendMoney.CountryCode.ToLower() == "usa" || sendMoney.CountryCode.ToLower() == "united states")
            {
                sendMoney.isDomesticTransfer = true;
                sendMoney.isDomesticTransferVal = "true";
            }
            else
                sendMoney.isDomesticTransferVal = "false";

            if (mtTrx.ProviderId == Convert.ToInt32(ProviderIds.MoneyGram))
            {
                return View("SendMoneyConfirmMoneyGram", sendMoney);
            }
			
			//Checking if Terms and condition pop-up is enabled based on provider and Product for a ChannelPartner
			bool isTncRequired = false;
			var product = sendMoney.Providers.FirstOrDefault(x => x.ProcessorName == "WesternUnion" && x.Name == "MoneyTransfer");
			if (product != null)
			{
				isTncRequired = product.IsTnCForcePrintRequired;
			}
			ViewBag.isTnCForcePrintRequired = isTncRequired;
            return View("SendMoneyConfirm", sendMoney);
        }

		private IQueryable<T> SortIQueryable<T>(IQueryable<T> data, string fieldName, string sortOrder)
		{
			if (string.IsNullOrWhiteSpace(fieldName)) return data;
			if (string.IsNullOrWhiteSpace(sortOrder)) return data;

			var param = Expression.Parameter(typeof(T), "i");
			Expression conversion = Expression.Convert(Expression.Property(param, fieldName), typeof(object));
			var mySortExpression = Expression.Lambda<Func<T, object>>(conversion, param);

			return (sortOrder == "desc") ? data.OrderByDescending(mySortExpression)
				: data.OrderBy(mySortExpression);
		}

		private List<SelectListItem> GetLocations(MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			Desktop desktop = new Desktop();
            //*********************************starts AL-3248 by Divya********************
            // getting all customer transacted locations
            CustomerSession customersession = (CustomerSession)Session["customersession"];           
            double period = Session["DateRange"] == null ? 60 : Convert.ToDouble(Session["DateRange"]);
            DateTime dateRange = DateTime.Now.AddDays(-period);

			var transactions = desktop.GetTransactionHistory(GetCustomerSessionId(), customersession.Customer.CIN, string.Empty, string.Empty, dateRange, mgiContext).AsQueryable();
            IQueryable<TransactionHistory> filteredtransactions = transactions;

            var custTransactionlocations = filteredtransactions.Select(l => new { l.Location }).Distinct().ToList();
            //********************End******************
            List<SelectListItem> locations = new List<SelectListItem>();

			string channelPartner = Session["ChannelPartnerName"].ToString();
			if (!string.IsNullOrWhiteSpace(channelPartner))
			{
				long channelPartnerId = desktop.GetChannelPartner(channelPartner, mgiContext).Id;
				var availableLocations = desktop.GetAllLocationNames().FindAll(loc => loc.ChannelPartnerId == channelPartnerId);

                foreach (var location in availableLocations)
                {
                    if (custTransactionlocations.Exists(cl => cl.Location == location.LocationName))//AL-3248
                    {
                        locations.Add(new SelectListItem() { Text = location.LocationName, Value = location.Id.ToString() });
                    } 
                }
                //*********************************starts AL-3248 by Divya********************
                if (availableLocations.Count > 0)
                {
                    var sortedlocations = locations.OrderBy(sl => sl.Text.ToUpper()).ToList();// sorting here
                    sortedlocations.Insert(0,new SelectListItem() { Text = "All", Value = "0" });
                    locations = sortedlocations;
                }
                //********************End******************
			}
			return locations;
		}

		private string GetDisplayCardNumber(string cardNumber)
		{
			return string.IsNullOrEmpty(cardNumber) ? "" : cardNumber.Replace(cardNumber.Substring(0, 12), "*****");
		}
		private string GetPayStatusMessage(string code)
		{
			PayStatus payStaus = PayStatus.BLANK;
			string message = string.Empty;
			try
			{
				payStaus = (PayStatus)Enum.Parse(typeof(PayStatus), code);
			}
			catch
			{
				payStaus = PayStatus.BLANK;
			}

			if (payStaus != PayStatus.BLANK)
			{
				switch (payStaus)
				{
					case PayStatus.CAN:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_CAN;
						break;
					case PayStatus.PURG:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_PURG;
						break;
					case PayStatus.OVLM:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_OVLM;
						break;
					case PayStatus.OVLQ:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_OVLQ;
						break;
					case PayStatus.SECQ:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_SECQ;
						break;
					case PayStatus.QQC1:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_QQC1;
						break;
					case PayStatus.BUST:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_BUST;
						break;
					case PayStatus.OFAC:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_OFAC;
						break;
					case PayStatus.FBST:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_FBST;
						break;
					case PayStatus.FBLK:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_FBLK;
						break;
					case PayStatus.ACPT:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_ACPT;
						break;
					case PayStatus.AUTH:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_AUTH;
						break;
					case PayStatus.QQC2:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_QQC2;
						break;
					case PayStatus.ACRQ:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_ACRQ;
						break;
					case PayStatus.CUBA:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_CUBA;
						break;
					case PayStatus.SWPA:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_SWPA;
						break;
					case PayStatus.HOLD:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.SendMoneyModifyHOLD;
						break;
					case PayStatus.PKUP:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_PKUP;
						break;
					case PayStatus.PKPQ:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_PKPQ;
						break;
					case PayStatus.UNAV:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_UNAV;
						break;
					case PayStatus.PHD:
						message = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.PayRequestStatusCode_PHD;
						break;
				}
			}

			return message;
		}

		private string GetFundTitle(int fundType)
		{
			string title = string.Empty;
			string titleFormat = "PREPAID CARD - {0}";
			if (fundType == (int)FundType.Debit)
				title = string.Format(titleFormat, "WITHDRAW");
			else if (fundType == (int)FundType.Credit)
				title = string.Format(titleFormat, "LOAD");
			else
				title = string.Format(titleFormat, "ACTIVATE");

			return title;
		}

		private string PrepareReceiptForPrinting(SharedData.Receipt receipt)
		{
			return receipt.Lines[0];
			// return PrepareReceiptForPrinting(receipt.Lines.ToArray());// receipt.Lines[0];
		}

		private List<ReceiptData> PrepareContentReceiptForPrinting(List<ReceiptData> receiptDatas)
		{

			List<ReceiptData> printLines = new List<ReceiptData>();

			ReceiptData receiptData;

			for (int i = 0; i < receiptDatas.Count; i++)
			{
				receiptData = new ReceiptData();
				receiptData.PrintData = receiptDatas[i].PrintData;
				receiptData.Name = "Receipt # " + i.ToString();
				printLines.Add(receiptData);
			}

			return printLines;
		}
		private string PrepareReceiptForPrinting(string[] receipts)
		{
			StringBuilder receiptBuilder = new StringBuilder();
			string base64Str = "";
			var receiptLines = Common.FileUtility.UpdateReceiptDataForLogo(receipts).ToList();

			foreach (string line in receipts)
			{
				receiptBuilder.AppendFormat("{0}\t", line);
			}
			byte[] byteStr = System.Text.Encoding.UTF8.GetBytes(receiptBuilder.ToString());
			base64Str = Convert.ToBase64String(byteStr);

			return base64Str;
		}

		private object GetDictionaryValue(Dictionary<string, object> dictionary, string key)
		{
			if (dictionary.ContainsKey(key) == false)
			{
				throw new Exception(String.Format("{0} not provided in dictionary", key));
			}
			return dictionary[key];
		}

		private object GetDictionaryValueIfExists(Dictionary<string, object> dictionary, string key)
		{
			if (dictionary.ContainsKey(key))
			{
				return dictionary[key];
			}
			return null;
		}

		//private string PrepareReceiptForPrinting(string receipts)
		//{
		//    StringBuilder receiptBuilder = new StringBuilder();
		//    string base64Str = "";
		//    var receiptLines = Common.FileUtility.UpdateReceiptDataForLogo(receipts);

		//    foreach (string line in receipts)
		//    {
		//        receiptBuilder.AppendFormat("{0}\t", line);
		//    }
		//    byte[] byteStr = System.Text.Encoding.UTF8.GetBytes(receiptBuilder.ToString());
		//    base64Str = Convert.ToBase64String(byteStr);

		//    return base64Str;
		//}
	}
}

