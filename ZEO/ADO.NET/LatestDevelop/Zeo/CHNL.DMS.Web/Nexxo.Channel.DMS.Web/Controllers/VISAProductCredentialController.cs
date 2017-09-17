using System;
using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using TCF.Zeo.Security.Voltage;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Common.Logging.Impl;
#endregion


namespace TCF.Channel.Zeo.Web.Controllers
{
    public class VISAProductCredentialController : ProductCredentialController
    {
        NLoggerCommon NLogger = new NLoggerCommon();
        ZeoClient.Response alloyResponse = new ZeoClient.Response();

        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult ProductCredential(string transactionId)
        {
            ProductCredentialViewModel productCredential = new ProductCredentialViewModel();

            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.Response cardInfoResponse = new ZeoClient.Response();
            try
            {
                if (!string.IsNullOrWhiteSpace(transactionId))
                {
                    productCredential = GetProductCredentialModel(transactionId, context);
                    return View("ProductCredential", productCredential);
                }

                Session["activeButton"] = "productcredential";
                productCredential = PopulateProductCredential(context);

                if (productCredential.HasGPRCard)
                {
                    cardInfoResponse = GetCardInfo(context);
                    if (WebHelper.VerifyException(cardInfoResponse)) throw new ZeoWebException(cardInfoResponse.Error.Details);
                    ZeoClient.CardBalanceInfo cardInfo = cardInfoResponse.Result as ZeoClient.CardBalanceInfo;

                    if (cardInfo.CardStatus == (int)CardStatus.Closed)
                    {
                        ZeoClient.Response accountInfo = GetAccountInfo(context);
                        if (WebHelper.VerifyException(accountInfo)) throw new ZeoWebException(cardInfoResponse.Error.Details);
                        ZeoClient.FundsAccount fundAcctInfo = accountInfo.Result as ZeoClient.FundsAccount;
                        Session["ExistingGPRCardInfo"] = fundAcctInfo;
                    }

                    int daysSinceClosed = CalculateDaysAfterClosure(cardInfo);

                    if (daysSinceClosed > 60)
                    {
                        productCredential.HasGPRCard = false;
                        productCredential.CardNumber = string.Empty;

                        if (cardInfo.IsFraud)
                            throw new ZeoWebException(GetErrorMessage("1003.100.8101", context));
                    }
                }

                if (!productCredential.HasGPRCard)
                    return View("ProductCredential", productCredential);
                else
                    return RedirectToAction("VisaTransactionHistory", "VISAProductCredential");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpGet]
        [CustomHandleErrorAttribute(ControllerName = "VISAProductCredential", ActionName = "VisaTransactionHistory", ResultType = "redirect")]
        public ActionResult OpenAccount()
        {
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                Session["activeButton"] = "productcredential";
                ProductCredentialViewModel productCredential = PopulateProductCredential(context);

                productCredential.HasGPRCard = false;
                productCredential.CardNumber = string.Empty;

                ZeoClient.CardBalanceInfo cardInfo = Session["CardBalance"] as ZeoClient.CardBalanceInfo;

                if (cardInfo.IsFraud)
                    throw new ZeoWebException(GetErrorMessage("1003.100.8101", context));

                return View("ProductCredential", productCredential);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [CustomHandleErrorAttribute(ViewName = "VisaTransactionHistory", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.VisaTransactionHistory")]
        public ActionResult VisaTransactionHistory(bool IsException = false, string ExceptionMessage = "")
        {
            ZeoClient.ZeoContext context = GetZeoContext();
            Session["activeButton"] = "productcredential";
            Session["transactionStatus"] = "Posted";

            VisaTransactionHistory visaHistory = new VisaTransactionHistory();

            try
            {
                alloyResponse = GetCardInfo(context);

                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);

                ZeoClient.CardBalanceInfo cardInfo = alloyResponse.Result as ZeoClient.CardBalanceInfo;

                visaHistory.IsAccountClosed = (cardInfo.CardStatus == (int)CardStatus.Closed || cardInfo.CardStatus == (int)CardStatus.ClosedForFraud) ? true : false;
                visaHistory.DisableMaintenance = ShouldDisableMainteance(cardInfo);
                visaHistory.DaysSinceClosed = CalculateDaysAfterClosure(cardInfo);
                visaHistory.CardBalance = cardInfo.Balance;
                visaHistory.CardStatus = (CardStatus)cardInfo.CardStatus;

                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                if (visaHistory.IsAccountClosed)
                {
                    // removes 90 from the Date Range dropdown list
                    visaHistory.DateRanges = visaHistory.DateRanges.Take(2);
                }

                return View("VisaTransactionHistory", visaHistory);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "VisaTransactionHistory", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.VisaTransactionHistory")]
        public ActionResult VisaTransactionHistory(VisaTransactionHistory visaHistory)
        {
            Session["activeButton"] = "productcredential";
            Session["dateRange"] = visaHistory.DateRange;
            Session["transactionStatus"] = visaHistory.TransactionStatus;

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
            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

            int dateRange = Session["dateRange"] == null ? 30 : Convert.ToInt32(Session["dateRange"]);

            ZeoClient.HelperTransactionStatus transactionStatus = Session["transactionStatus"] == null ? ZeoClient.HelperTransactionStatus.Posted :
                (ZeoClient.HelperTransactionStatus)Enum.Parse(typeof(ZeoClient.HelperTransactionStatus), Convert.ToString(Session["transactionStatus"]));

            ZeoClient.TransactionHistoryRequest transactionHistoryRequest = new ZeoClient.TransactionHistoryRequest();

            transactionHistoryRequest.DateRange = dateRange;
            transactionHistoryRequest.TransactionStatus = transactionStatus;

            long customerId = Convert.ToInt64(sessionId);

            ZeoClient.ZeoContext context = GetZeoContext();

            try
            {
                alloyResponse = client.GetCardTransactionHistory(transactionHistoryRequest, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                List<ZeoClient.VisaTransactionHistory> transactions = alloyResponse.Result as List<ZeoClient.VisaTransactionHistory>;
                IQueryable<ZeoClient.VisaTransactionHistory> filteredtransactions = transactions.AsQueryable();

                Expression<System.Func<ZeoClient.VisaTransactionHistory, object>> expression = null;

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
                var data = transactionHistoryRequest.TransactionStatus == ZeoClient.HelperTransactionStatus.Posted ?
                          (from s in sortedtransactions
                           select new
                           {
                               cell = new object[] { s.PostedDateTime.ToString("MM/dd/yyyy hh:mm:ss tt"), s.TransactionDateTime.ToString("MM/dd/yyyy hh:mm:ss tt"), s.MerchantName, s.Location,
                                     s.TransactionDescription,s.TransactionAmount.ToString(("C2")),s.ActualBalance.ToString(("C2")),
                                     s.AvailableBalance.ToString(("C2"))}
                           }).ToList() :

                          //Building the grid data for Pending type transactions
                          transactionHistoryRequest.TransactionStatus == ZeoClient.HelperTransactionStatus.Pending ?
                          (from s in sortedtransactions
                           select new
                           {
                               cell = new object[] {s.TransactionDateTime.ToString("MM/dd/yyyy hh:mm:ss tt"), s.MerchantName, s.Location,
                                     s.TransactionDescription,s.TransactionAmount.ToString(("C2")),s.ActualBalance.ToString(("C2")),
                                     s.AvailableBalance.ToString(("C2"))}
                           }).ToList() :

                          //Building the grid data for Denied type transactions
                          (from s in sortedtransactions
                           select new
                           {
                               cell = new object[] { s.TransactionDateTime.ToString("MM/dd/yyyy hh:mm:ss tt"), s.MerchantName, s.Location,
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
                VerifyException(ex);
                return Json(
                        new
                        {
                            success = false,
                            err = ViewBag.ExceptionMessage
                        }, JsonRequestBehavior.AllowGet);
            }
        }

        #region AL-324/Visa Prepaid Phase

        public ActionResult ClosePrepaidAccount()
        {
            PrePaidCard card = new PrePaidCard();
            card.Name = card.CustomerSession.Customer.FirstName + " " + card.CustomerSession.Customer.LastName;
            card.CardNumber = card.CustomerSession.CardNumber;
            card.CardNumber = "**** **** **** " + card.CardNumber.Substring(card.CardNumber.Length - 4);

            return PartialView("_ClosePrepaidAccountPopUp", card);
        }

        [HttpPost]
        public ActionResult ClosePrepaidAccount(long customerSessionId)
        {
            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response cardInfoResponse = new ZeoClient.Response();
            ZeoClient.ZeoContext context = GetZeoContext();

            try
            {
                alloyResponse = client.CloseAccount(context);

                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                cardInfoResponse = client.GetCardBalance(context);
                if (WebHelper.VerifyException(cardInfoResponse)) throw new ZeoWebException(cardInfoResponse.Error.Details);
                Session["CardBalance"] = cardInfoResponse.Result;
                return Json(alloyResponse.Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return Json(
                    new
                    {
                        success = false,
                        data = ViewBag.ExceptionMessage
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CardMaintenancePopUp()
        {
            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();

            CardMaintenanceViewModel cardCredential = new CardMaintenanceViewModel();
            ZeoClient.Response cardBalanceResponse = new ZeoClient.Response();
            ZeoClient.Response shippingTypeResponse = new ZeoClient.Response();
            ZeoClient.Response prepaidActionResponse = new ZeoClient.Response();

            try
            {
                cardBalanceResponse = client.GetCardBalance(context);
                if (WebHelper.VerifyException(cardBalanceResponse)) throw new ZeoWebException(cardBalanceResponse.Error.Details);
                ZeoClient.CardBalanceInfo cardInfo = cardBalanceResponse.Result as ZeoClient.CardBalanceInfo;

                Session["CardBalance"] = cardInfo;
                cardCredential.CardBalance = new CardBalance();
                cardCredential.CardBalance.CardStatusId = cardInfo.CardStatus;
                CardStatus cardStatus = (CardStatus)cardInfo.CardStatus;
                cardCredential.CardBalance.CardStatus = cardStatus.ToString();

                prepaidActionResponse = client.GetPrepaidActions(cardCredential.CardBalance.CardStatus.ToLower(), context);
                if (WebHelper.VerifyException(prepaidActionResponse)) throw new ZeoWebException(prepaidActionResponse.Error.Details);
                List<KeyValuePair<string, string>> items = prepaidActionResponse.Result as List<KeyValuePair<string, string>>;
                List<SelectListItem> prepaidActions = GetSelectListItems(items);

                shippingTypeResponse = client.GetShippingTypes(context);
                if (WebHelper.VerifyException(shippingTypeResponse)) throw new ZeoWebException(shippingTypeResponse.Error.Details);
                List<ZeoClient.ShippingTypes> shippingTypes = (List<ZeoClient.ShippingTypes>)shippingTypeResponse.Result;


                List<KeyValuePair<string, string>> shippingItems = new List<KeyValuePair<string, string>>();

                foreach (var shippingType in shippingTypes)
                {
                    shippingItems.Add(new KeyValuePair<string, string>(shippingType.Name, shippingType.Code));
                }

                cardCredential.ActionForCardReplace = prepaidActions;
                cardCredential.ShippingTypes = GetSelectListItems(shippingItems);

                return PartialView("_CardMaintenance", cardCredential);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        public ActionResult CardMaintenance(long customerSessionId, string prepaidAction, string shippingType, string panNumber, string cvv)
        {
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

                SecureData secure = new SecureData();
                string decryptedCardNumber = string.Empty;
                ZeoClient.Response updateCardResponse = new ZeoClient.Response();
                ZeoClient.Response cardInfoResponse = new ZeoClient.Response();
                ZeoClient.Response statusResponse = new ZeoClient.Response();

                if (!string.IsNullOrWhiteSpace(panNumber))
                {
                    decryptedCardNumber = secure.Decrypt(panNumber, cvv);
                }
                int cardStatus = Convert.ToInt32(prepaidAction);
                ZeoClient.CardMaintenanceInfo cardMaintenanceInfo = new ZeoClient.CardMaintenanceInfo()
                {
                    CardStatus = prepaidAction,
                    ShippingType = shippingType,
                    CardNumber = decryptedCardNumber,
                    SelectedCardStatus = prepaidAction
                };
                if ((cardStatus == (int)Common.CardStatus.LostCard || cardStatus == (int)Common.CardStatus.StolenCard))
                {
                    updateCardResponse = client.UpdateCardStatus(cardMaintenanceInfo, context);
                    if (WebHelper.VerifyException(updateCardResponse)) throw new ZeoWebException(updateCardResponse.Error.Details);
                    statusResponse = client.ReplaceCard(cardMaintenanceInfo, context);
                }
                else if (cardStatus == (int)Common.CardStatus.Damaged)
                {
                    //AL-4781 Changes
                    cardInfoResponse = client.GetCardBalance(context);
                    if (WebHelper.VerifyException(cardInfoResponse)) throw new ZeoWebException(cardInfoResponse.Error.Details);
                    ZeoClient.CardBalanceInfo cardInfo = cardInfoResponse.Result as ZeoClient.CardBalanceInfo;
                    if (cardInfo.CardStatus != (int)Common.CardStatus.Active)
                    {
                        cardMaintenanceInfo.CardStatus = cardInfo.CardStatus.ToString();  //update card status to current card status
                    }
                    statusResponse = client.ReplaceCard(cardMaintenanceInfo, context);
                }
                else
                {
                    statusResponse = client.UpdateCardStatus(cardMaintenanceInfo, context);
                }

                if (WebHelper.VerifyException(statusResponse)) throw new ZeoWebException(statusResponse.Error.Details);

                cardInfoResponse = client.GetCardBalance(context);
                if (WebHelper.VerifyException(cardInfoResponse)) throw new ZeoWebException(cardInfoResponse.Error.Details);
                Session["CardBalance"] = cardInfoResponse.Result as ZeoClient.CardBalanceInfo;

                return Json(Convert.ToBoolean(statusResponse.Result), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return Json(
                    new
                    {
                        success = false,
                        data = ViewBag.ExceptionMessage
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CardReplacementConfirmation(string data)
        {
            ZeoClient.ZeoContext context = GetZeoContext();
            //0 is active and 3 is suspended
            if (data == "0")
                ViewBag.ConfirmationMessage = App_GlobalResources.Nexxo.ResourceManager.GetString(context.ChannelPartnerName + "CardReplacementConfirmationMessageSuspendToActive");
            else if (data == "3")
                ViewBag.ConfirmationMessage = App_GlobalResources.Nexxo.ResourceManager.GetString(context.ChannelPartnerName + "CardReplacementConfirmationMessageActiveToSuspend");
            else
                ViewBag.ConfirmationMessage = App_GlobalResources.Nexxo.ResourceManager.GetString(context.ChannelPartnerName + "CardReplacementConfirmationMessage");
            return PartialView("_CardReplacementConfirmation");
        }

        [HttpPost]
        public ActionResult ShippingFee(string shippingType, long customerSessionId)
        {
            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.CardMaintenanceInfo cardMaintenance = new ZeoClient.CardMaintenanceInfo()
            {
                ShippingType = shippingType
            };

            try
            {
                alloyResponse = client.GetShippingFee(cardMaintenance, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                double fee = Convert.ToDouble(alloyResponse.Result);
                string shippingFeeMessage = string.Format(App_GlobalResources.Nexxo.ShippingFeeMessage, fee);

                var data = Json(new
                {
                    success = true,
                    fee = fee,
                    message = shippingFeeMessage
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                VerifyException(ex);
                return Json(
                    new
                    {
                        success = false,
                        data = ViewBag.ExceptionMessage
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AssociateCard()
        {
            ProductCredentialViewModel productCredentialViewModel = new ProductCredentialViewModel();

            return View("AssociateCard", productCredentialViewModel);
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "AssociateCard", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductCredentialViewModel")]
        public ActionResult AssociateCard(ProductCredentialViewModel productCredentialViewModel)
        {
            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();

            SecureData secure = new SecureData();
            ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];

            try
            {
                string decryptedCardNumber = secure.Decrypt(productCredentialViewModel.CardNumber, productCredentialViewModel.CVV);
                ZeoClient.FundsAccount account = new ZeoClient.FundsAccount()
                {
                    CardNumber = decryptedCardNumber
                };

                alloyResponse = client.AssociateCard(account, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                customerSession.CardNumber = decryptedCardNumber;
                customerSession.IsGPRCustomer = true;
                Session["IsGPRCard"] = customerSession.IsGPRCustomer;
                Session["CustomerSession"] = customerSession;
                ViewBag.CardSuccessfulAssociated = alloyResponse.Result;
                return View("AssociateCard", productCredentialViewModel);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult AssociateCardConfirmation()
        {
            ZeoClient.ZeoContext context = GetZeoContext();
            ViewBag.ConfirmationMessage = App_GlobalResources.Nexxo.ResourceManager.GetString(context.ChannelPartnerName + "AssociateCardConfirmation");
            return PartialView("_AssociateCardConfirmation");
        }
        public ActionResult SuccessfulCardClosureConformation()
        {
            return PartialView("_SuccessfulCardClosurePopUp");
        }

        public ActionResult FundFee(string prepaidAction, long customerSessionId)
        {
            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.CardMaintenanceInfo cardMaintenance = new ZeoClient.CardMaintenanceInfo()
            {
                CardStatus = prepaidAction
            };

            try
            {
                alloyResponse = client.GetFundFee(cardMaintenance, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                double fee = Convert.ToDouble(alloyResponse.Result);
                string visaFeeMessage = "No replacement fee";
                if (fee != 0)
                    visaFeeMessage = string.Format(App_GlobalResources.Nexxo.VisaFeeMessage, fee);

                var data = Json(new
                {
                    success = true,
                    fee = fee,
                    message = visaFeeMessage
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return Json(
                    new
                    {
                        success = false,
                        data = ViewBag.ExceptionMessage
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult OrderComapanionCard()
        {
            ZeoClient.CardBalanceInfo cardInfo = new ZeoClient.CardBalanceInfo();
            if (Session["CardBalance"] != null)
            {
                cardInfo = (ZeoClient.CardBalanceInfo)Session["CardBalance"];
            }
            var data = Json(new
            {
                Err_Msg = App_GlobalResources.Nexxo.OrderCompanionCardMessage,
                IsPrimaryCardHolder = Helper.GetBoolDictionaryValueIfExists(cardInfo.MetaData, "IsPrimaryCardHolder"),
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderAddOnCard()
        {
            CardMaintenanceViewModel model = new CardMaintenanceViewModel();

            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

            ZeoClient.Funds fund = new ZeoClient.Funds()
            {
                Fee = 0,
                Amount = 0
            };
            ZeoClient.ZeoContext context = GetZeoContext();

            try
            {
                context.AddOnCustomerId = Convert.ToInt64(TempData["AddOnAlloyId"]);
                alloyResponse = client.IssueAddOnCard(fund, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        #endregion

        #region Private Methods
        private Expression<Func<ZeoClient.VisaTransactionHistory, object>> GetExpression(string sidx, Expression<System.Func<ZeoClient.VisaTransactionHistory, object>> expression)
        {
            switch (sidx.ToLower())
            {
                case "posteddatetime":
                    expression = t => t.PostedDateTime;
                    break;
                case "transactiondatetime":
                    expression = t => t.TransactionDateTime;
                    break;
                case "merchantname":
                    expression = t => t.MerchantName;
                    break;
                case "location":
                    expression = t => t.Location;
                    break;
                case "transactionamount":
                    expression = t => t.TransactionAmount;
                    break;
                case "transactiondescription":
                    expression = t => t.TransactionDescription;
                    break;
                case "availablebalance":
                    expression = t => t.AvailableBalance;
                    break;
                case "actualbalance":
                    expression = t => t.ActualBalance;
                    break;
            }
            return expression;
        }

        private ProductCredentialViewModel PopulateProductCredential(ZeoClient.ZeoContext Context)
        {
            ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];
            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

            ProductCredentialViewModel productCredential = new ProductCredentialViewModel();

            if (!string.IsNullOrEmpty(customerSession.Customer.CardNumber) && customerSession.IsGPRCustomer)
            {
                string cardNumber = customerSession.Customer.CardNumber;
                cardNumber = (cardNumber.Length > 4) ? cardNumber.Substring(cardNumber.Length - 4) : cardNumber;
                productCredential = new ProductCredentialViewModel()
                {
                    Name = string.Format("{0} {1}", customerSession.Customer.FirstName, customerSession.Customer.LastName),
                    CardNumber = string.Format("**** **** **** {0}", cardNumber),
                    HasGPRCard = true
                };
            }
            else
            {
                productCredential.Name = string.Format("{0} {1}", productCredential.CustomerSession.Customer.FirstName, productCredential.CustomerSession.Customer.LastName);
            }
            return productCredential;
        }

        private ZeoClient.Response GetCardInfo(ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

            alloyResponse = client.GetCardBalance(context);
            return alloyResponse;
        }


        private bool ShouldDisableMainteance(ZeoClient.CardBalanceInfo cardInfo)
        {
            bool shouldDisable = false;

            if (cardInfo.ClosureDate != null)
            {
                shouldDisable = true;
            }
            else
            {
                Common.CardStatus visaCardStatus = (Common.CardStatus)cardInfo.CardStatus;

                switch (visaCardStatus)
                {
                    case Common.CardStatus.Active:
                    case Common.CardStatus.Suspended:
                    case Common.CardStatus.CardIssued:
                    case Common.CardStatus.LostCard:
                    case Common.CardStatus.StolenCard:
                        shouldDisable = false;
                        break;
                    case Common.CardStatus.PendingCardIssuance:
                    case Common.CardStatus.ExpiredCard:
                    case Common.CardStatus.PendingAccountClosure:
                    case Common.CardStatus.Closed:
                    case Common.CardStatus.ClosedForFraud:
                    case Common.CardStatus.ReturnedUndeliverable:
                    case Common.CardStatus.ResearchRequired:
                    case Common.CardStatus.Voided:
                    case Common.CardStatus.Damaged:
                    case Common.CardStatus.Stale:
                    case Common.CardStatus.PendingDestruction:
                    case Common.CardStatus.Destroyed:
                    case Common.CardStatus.ClosedDueToUpgrade:
                    case Common.CardStatus.ClosedForDeceased:
                        shouldDisable = true;
                        break;
                }
            }

            return shouldDisable;
        }

        private int CalculateDaysAfterClosure(ZeoClient.CardBalanceInfo cardInfo)
        {
            int days = 0;

            if (cardInfo.ClosureDate != null)
            {
                TimeSpan timeDifference = DateTime.Now - (DateTime)cardInfo.ClosureDate;
                days = timeDifference.Days;
            }

            return days;
        }

        private ProductCredentialViewModel GetProductCredentialModel(string transactionId, ZeoClient.ZeoContext context)
        {
            ZeoClient.CustomerSession customerSessionAO = (ZeoClient.CustomerSession)Session["CustomerSession"];

            ProductCredentialViewModel productCredential = null;

            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

            alloyResponse = client.GetFundTransaction(long.Parse(transactionId), true, context);
            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
            ZeoClient.Funds fundTransaction = alloyResponse.Result as ZeoClient.Funds;

            //This code is added because on Editing the VISA transaction, If the expiration date is in single digit
            //random value will be displayed in the Expiration field in Card registration screen. 
            //Solution - In case of single digit, prefixed "0" with the single digit to display the proper date in expiration date field. 

            //Starts Here
            string[] expiryDate = fundTransaction.ExpirationDate.Split('/');

            if (expiryDate.Length > 1)
            {
                string expMonth = expiryDate[0].Length == 1 ?
                    expiryDate[0].PadLeft(2, '0') : expiryDate[0];

                expiryDate[0] = expMonth;
            }
            //Ends Here

            productCredential = new ProductCredentialViewModel()
            {
                Name = string.Format("{0} {1}", customerSessionAO.Customer.FirstName, customerSessionAO.Customer.LastName),
                HasGPRCard = false,
                CardNumber = fundTransaction.FullCardNumber,
                MaskCardNumber = fundTransaction.CardNumber,
                ProxyId = fundTransaction.ProxyId,
                PseudoDDA = fundTransaction.PseudoDDA,
                ExpirationDate = string.Concat(expiryDate[0], "/", expiryDate[1]),
                InitialLoad = fundTransaction.Amount,
                TransactionId = long.Parse(transactionId),
                PromoCode = fundTransaction.PromoCode
            };

            return productCredential;
        }

        private ZeoClient.Response GetAccountInfo(ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

            alloyResponse = client.LookupForPANFund(context);
            return alloyResponse;
        }

        #endregion
    }
}
