using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Collections;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
using static TCF.Zeo.Common.Util.Helper;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class TransactionHistoryController : BaseController
    {
        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult TransactionHistory(bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;
                Session["activeButton"] = "transactionhistory";
                ZeoClient.ZeoContext context = GetZeoContext();
                DateTime dateRange = DateTime.Now.AddDays(-60);

                TransactionHistoryModel customerTranHistory = new TransactionHistoryModel()
                {
                    Locations = GetLocations(dateRange, context)
                };
                Session["TransactionType"] = customerTranHistory.TransactionType;
                Session.Remove("TransactionType");

                ViewBag.Navigation = Resources.NexxoSiteMap.TransactionHistory;
                return View("TransactionHistory", customerTranHistory);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult GetTransactionHistory(string alloyId, int page = 1, int rows = 5)
        {
            try
            {
                ZeoClient.ZeoServiceClient service = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response txnResponse = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();
                long customerId = Convert.ToInt64(alloyId);

                string transactionType = Convert.ToString(Session["TransactionType"]) == "0" ?
                                                   null : Convert.ToString(Session["TransactionType"]);

                string location = Convert.ToString(Session["Location"]) == string.Empty ? null : Convert.ToString(Session["Location"]);

                double period = Session["DateRange"] == null ? 60 : Convert.ToDouble(Session["DateRange"]);
                DateTime dateRange = DateTime.Now.AddDays(-period);

                ZeoClient.TransactionHistorySearchCriteria criteria = new ZeoClient.TransactionHistorySearchCriteria()
                {
                    DatePeriod = dateRange,
                    TransactionType = (string.IsNullOrWhiteSpace(transactionType) ? null : transactionType),
                    CustomerId = customerId,
                    LocationName = location
                };

                txnResponse = service.GetCustomerTransactions(criteria, context);

                if (WebHelper.VerifyException(txnResponse)) throw new ZeoWebException(txnResponse.Error.Details);
                List<ZeoClient.TransactionHistory> transactions = txnResponse.Result as List<ZeoClient.TransactionHistory>;

                IQueryable<ZeoClient.TransactionHistory> filteredtransactions = transactions.AsQueryable();

                var sortedtransactions = filteredtransactions;
                var totalRecords = filteredtransactions.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

                var data = (from s in sortedtransactions
                            select new
                            {
                                id = s.TransactionId,
                                cell = new object[] { s.TransactionDate.ToString("MM/dd/yyyy hh:mm:ss tt"), s.Teller, s.SessionID.ToString(), s.TransactionId.ToString(), s.Location, s.TransactionType, s.TransactionStatus, s.TransactionDetail, s.TotalAmount.ToString(("C2")) }
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
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ControllerName = "TransactionHistory", ActionName = "TransactionHistory", ResultType = "redirect")]
        public ActionResult TransactionHistory(TransactionHistoryModel transactionHistoryModel)
        {
            try
            {
                Session["Location"] = transactionHistoryModel.Location;
                Session["DateRange"] = transactionHistoryModel.DateRange;
                Session["TransactionType"] = transactionHistoryModel.TransactionType;

                Session["CustomerSessionId"] = GetCustomerSessionId();

                DateTime dateRange = DateTime.Now.AddDays(-Convert.ToDouble(transactionHistoryModel.DateRange));

                ZeoClient.ZeoContext context = GetZeoContext();

                TransactionHistoryModel transactionHistory = new TransactionHistoryModel()
                {
                    Locations = GetLocations(dateRange, context),
                    Location = transactionHistoryModel.Location,
                    DateRange = transactionHistoryModel.DateRange,
                    TransactionType = transactionHistoryModel.TransactionType
                };

                return View("TransactionHistory", transactionHistory);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult GetAgentTransHistory(int page = 1, int rows = 5)
        {
            long agentSessionId = GetAgentSessionId();
            try
            {

                #region AO
                ZeoClient.ZeoServiceClient service = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response alloyResponse = new ZeoClient.Response();
                #endregion

                int currentAgentId = Session["agentId"] == null ? 0 : Convert.ToInt32(Session["agentId"]);
                bool showAllReport = Session["ShowAllReport"] == null ? false : (bool)Session["ShowAllReport"];

                ZeoClient.Response response = service.GetAgentDetails(agentSessionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.Agent currentAgent = response.Result as ZeoClient.Agent;


                long? agentId = Session["DropDownAgentId"] == null || (string)Session["DropDownAgentId"] == "Select" ? currentAgentId : Convert.ToInt64(Session["DropDownAgentId"]);

                long transactionId = Session["TransactionID"] == null ? 0 : Convert.ToInt64(Session["TransactionID"]);

                string location = Session["CurrentLocation"] == null ? string.Empty : Session["CurrentLocation"].ToString();

                int duration = Convert.ToInt32(ConfigurationManager.AppSettings["AgentTransactionHistoryDuration"]);

                if (currentAgent.UserRoleId == (int)UserRoles.Manager || currentAgent.UserRoleId == (int)UserRoles.SystemAdmin || currentAgent.UserRoleId == (int)UserRoles.ComplianceManager || currentAgent.UserRoleId == (int)UserRoles.Tech)
                {
                    agentId = Session["DropDownAgentId"] == null ? null : agentId;
                }

                string transactionType = Session["TransactionType"] == null ? null : Convert.ToString(Session["TransactionType"]);

                ZeoClient.TransactionHistorySearchCriteria criteria = new ZeoClient.TransactionHistorySearchCriteria()
                {
                    AgentId = Convert.ToInt64(agentId),
                    TransactionType = transactionType,
                    ShowAll = showAllReport,
                    LocationName = location,
                    DatePeriod = DateTime.Now.AddDays(-duration),
                    TransactionId = transactionId
                };

                alloyResponse = service.GetAgentTransactions(criteria, context);

                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                List<ZeoClient.TransactionHistory> transactions = alloyResponse.Result as List<ZeoClient.TransactionHistory>;

                IQueryable<ZeoClient.TransactionHistory> filteredtransactions = transactions.AsQueryable();

                var sortedtransactions = filteredtransactions;
                var totalRecords = filteredtransactions.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

                var data = (from s in sortedtransactions
                            select new
                            {
                                id = s.TransactionId,
                                cell = new object[] { s.TransactionDate.ToString("MM/dd/yyyy hh:mm:ss tt"), s.Teller, s.SessionID.ToString(), s.TransactionId.ToString(), s.CustomerName, s.TransactionStatus, s.TransactionType, s.TransactionDetail, s.TotalAmount.ToString(("C2")) }
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
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerSearch")]
        public ActionResult GetAgentTransactionHistory(bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;
                Session["activeButton"] = "transhistory";
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                AgentTransactionHistory agentTranHistory = new AgentTransactionHistory();

                int currentAgentId = Session["AgentId"] == null ? 0 : Convert.ToInt32(Session["AgentId"]);

                Session["DropDownAgentId"] = null;
                Session["TransactionType"] = null;
                Session["ShowAllReport"] = null;
                Session["ApplyCriteria"] = false;

                ZeoClient.Response response = serviceClient.GetAgentDetails(context.AgentSessionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.Agent currentAgent = response.Result as ZeoClient.Agent;
                ViewBag.IsAgentTeller = false;
                if (currentAgent.UserRoleId == (int)UserRoles.Manager || currentAgent.UserRoleId == (int)UserRoles.SystemAdmin || currentAgent.UserRoleId == (int)UserRoles.Tech || currentAgent.UserRoleId == (int)UserRoles.ComplianceManager)
                {
                    List<SelectListItem> agentDetails = new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = string.Empty, Selected = true } };
                    response = serviceClient.GetAgents(currentAgent.LocationId, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    List<ZeoClient.Agent> agents = (response.Result as List<ZeoClient.Agent>);
                    foreach (var item in agents)
                    {
                        agentDetails.Add(new SelectListItem() { Text = item.AgentFullName, Value = item.AgentID.ToString() });
                    }
                    agentTranHistory.Agents = agentDetails;
                }
                else if (currentAgent.UserRoleId == (int)UserRoles.Teller)
                {
                    agentTranHistory.Agents = new List<SelectListItem>() { new SelectListItem() { Value = currentAgentId.ToString(), Text = currentAgent.AgentFullName } };
                    ViewBag.IsAgentTeller = true;
                }
                else
                    agentTranHistory.Agents = new List<SelectListItem>() { new SelectListItem() { Value = "Select", Text = "Select" } };


                return View("AgentTransHistory", agentTranHistory);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ControllerName = "TransactionHistory", ActionName = "GetAgentTransactionHistory", ResultType = "redirect")]
        public ActionResult GetAgentTransactionHistory(AgentTransactionHistory agentTransactionHistory)
        {
            try
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                Session["TransactionType"] = agentTransactionHistory.TransactionType;
                Session["DropDownAgentId"] = agentTransactionHistory.Agent;
                Session["ShowAllReport"] = agentTransactionHistory.IsTransactionStatusSelected;
                Session["ApplyCriteria"] = true;
                Session["TransactionID"] = agentTransactionHistory.TransactionID;

                AgentTransactionHistory agentTranHistory = new AgentTransactionHistory();

                int currentAgentId = Session["AgentId"] == null ? 0 : Convert.ToInt32(Session["AgentId"]);

                ZeoClient.Response response = serviceClient.GetAgentDetails(context.AgentSessionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.Agent currentAgent = response.Result as ZeoClient.Agent;

                ViewBag.IsAgentTeller = false;

                if (currentAgent.UserRoleId == (int)UserRoles.Manager || currentAgent.UserRoleId == (int)UserRoles.SystemAdmin || currentAgent.UserRoleId == (int)UserRoles.Tech || currentAgent.UserRoleId == (int)UserRoles.ComplianceManager)
                {
                    List<SelectListItem> agentDetails = new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = string.Empty, Selected = true } };
                    response = serviceClient.GetAgents(currentAgent.LocationId, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    List<ZeoClient.Agent> agents = (response.Result as List<ZeoClient.Agent>);
                    foreach (var item in agents)
                    {
                        agentDetails.Add(new SelectListItem() { Text = item.AgentFullName, Value = item.AgentID.ToString() });
                    }
                    agentTranHistory.Agents = agentDetails;
                }
                else if (currentAgent.UserRoleId == (int)UserRoles.Teller)
                {
                    agentTranHistory.Agents = new List<SelectListItem>() { new SelectListItem() { Value = currentAgentId.ToString(), Text = currentAgent.AgentFullName } };
                    ViewBag.IsAgentTeller = true;
                }
                else
                    agentTranHistory.Agents = new List<SelectListItem>() { new SelectListItem() { Value = "Select", Text = "Select" } };

                return View("AgentTransHistory", agentTranHistory);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        [SkipNoDirectAccess]
        public ActionResult AgentDailySummaryReport()
        {
            try
            {
                #region AO
                ZeoClient.ZeoServiceClient service = new ZeoClient.ZeoServiceClient();

                ZeoClient.Response alloyResponse = new ZeoClient.Response();
                #endregion
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.TransactionHistorySearchCriteria criteria = new ZeoClient.TransactionHistorySearchCriteria()
                {
                    AgentId = context.AgentId,
                    TransactionType = null,
                    ShowAll = false,
                    LocationName = context.LocationName,
                    DatePeriod = DateTime.Now,
                    TransactionId = 0
                };
                TransactionHistoryViewModel transactions = new TransactionHistoryViewModel();
                alloyResponse = service.GetAgentTransactions(criteria, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                transactions.Transactions = (alloyResponse.Result as List<ZeoClient.TransactionHistory>).ToList();
                transactions.AgentId = context.AgentId;
                transactions.AgentFullName = context.AgentFirstName + " " + context.AgentLastName;
                transactions.TransactionSummaryDate = DateTime.Today.ToString("MM/dd/yyyy");

                return View("AgentDailySummaryReport", transactions);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public ActionResult GetPopup(string transactionId)
        //{
        //    string sessionId = Session["sessionId"].ToString();
        //    string[] receiptData = desktopClient.GetReceiptData(transactionId, sessionId, mgiContext);
        //    string base64Str = PrepareReceiptForPrinting(receiptData);

        //    TransactionHistoryPopup transactionHistoryPopup = new TransactionHistoryPopup { FundPaymentId = transactionId, ReceiptData = base64Str };
        //    return PartialView("_TransactionHistoryPopup", transactionHistoryPopup);
        //}

        public ActionResult DisplayTransactionDetails(string transactionId, long dt, string transactionType, string transactionStatus, bool isAgentSession = false, string CustSessionId = "")
        {

            try
            {
                #region AO
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.ZeoServiceClient service = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response alloyResponse = new ZeoClient.Response();

                #endregion

                long agentSessionId = GetAgentSessionId();
                long customerSessionId = 0L;
                if (string.IsNullOrEmpty(CustSessionId))
                    customerSessionId = GetCustomerSessionId();
                else
                    customerSessionId = long.Parse(CustSessionId);

                if (isAgentSession)
                    ViewBag.IsAgentSession = true;

                long trxId = Convert.ToInt64(transactionId);

                #region Check
                if (transactionType.ToLower().Contains("check"))
                {
                    alloyResponse = service.GetCheckTranasactionDetails(trxId, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    ZeoClient.CheckTransactionDetails check = alloyResponse.Result as ZeoClient.CheckTransactionDetails;

                    string declineCode = check.DeclineErrorCode > 0 ? check.DeclineErrorCode + " - " : "";

                    ProcessCheckTransactionDetailsViewModel tranCheckDetails = new ProcessCheckTransactionDetailsViewModel()
                    {
                        TransactionType = transactionType,
                        TransactionId = transactionId,
                        TransactionStatus = transactionStatus,
                        Fee = check.BaseFee,
                        Discount = check.DiscountApplied != 0 ? Math.Abs(check.DiscountApplied) : 0,
                        PromotionName = check.DiscountApplied != 0 ? check.DiscountName : "NA",
                        PromotionDescription = check.DiscountApplied != 0 ? check.DiscountDescription : "NA",
                        NetFee = check.Fee,
                        Total = check.Amount - check.Fee,
                        Amount = check.Amount,
                        CheckNumber = check.CheckNumber,
                        CheckType = check.CheckType,
                        //ProviderName = ((ProviderIds)check.ProviderId).ToString(),
                        Reason = declineCode + check.DeclineMessage
                    };

                    return PartialView("_ProcessCheckTransactionDetails", tranCheckDetails);

                }
                #endregion

                #region Prepaid
                else if (transactionType.ToLower().Contains("prepaid"))
                {
                    alloyResponse = service.GetFundTransaction(trxId, false, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    ZeoClient.Funds fundTransaction = alloyResponse.Result as ZeoClient.Funds;

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
                        ProviderName = ((Helper.ProviderId)fundTransaction.ProviderId).ToString(),
                        CardNumber = GetDisplayCardNumber(fundTransaction.CardNumber),
                        NewCardBalance = fundTransaction.CardBalance

                    };

                    if (fundTransaction.FundsType == ZeoClient.HelperFundType.Activation || fundTransaction.FundsType == ZeoClient.HelperFundType.AddOnCard)
                    {
                        return PartialView("_PrepaidActiveTransactionDetails", tranCheckDetails);
                    }
                    else if (fundTransaction.FundsType == ZeoClient.HelperFundType.Credit)
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
                #endregion

                #region Send Money
                else if (transactionType.ToLower().Contains("send"))
                {
                    if (context.CustomerSessionId != 0)
                    {
                        GetCustomerSessionCounterId((int)Helper.ProviderId.WesternUnion, context);
                    }
                    alloyResponse = new ZeoClient.ZeoServiceClient().GetMoneyTransferTransaction(trxId, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    ZeoClient.MoneyTransferTransaction trx = alloyResponse.Result as ZeoClient.MoneyTransferTransaction;

                    string receiverAddress = string.Empty;

                    //TODO receiver address need to populated

                    receiverAddress += (string.IsNullOrWhiteSpace(trx.Address) ? string.Empty : trx.Address);
                    receiverAddress += (string.IsNullOrWhiteSpace(trx.City) ? string.Empty : (string.IsNullOrWhiteSpace(receiverAddress) ? trx.City : ", " + trx.City));
                    receiverAddress += (string.IsNullOrWhiteSpace(trx.State) ? string.Empty : (string.IsNullOrWhiteSpace(receiverAddress) ? trx.State : ", " + trx.State));
                    receiverAddress += (string.IsNullOrWhiteSpace(trx.Country) ? string.Empty : (string.IsNullOrWhiteSpace(receiverAddress) ? trx.Country : ", " + trx.Country));
                    receiverAddress += (string.IsNullOrWhiteSpace(trx.PostalCode) ? string.Empty : (string.IsNullOrWhiteSpace(receiverAddress) ? trx.PostalCode : ", " + trx.PostalCode));


                    MoneyTransferTransactionDetailsViewModel tranDetails = new MoneyTransferTransactionDetailsViewModel()
                    {

                        TransactionType = transactionType,
                        TransactionId = transactionId,
                        TransactionStatus = transactionStatus,
                        Fee = trx.Fee + trx.PromotionDiscount,
                        Discount = trx.PromotionDiscount * -1,
                        PromotionName = string.IsNullOrEmpty(trx.PromotionsCode) ? "NA" : trx.PromotionsCode,
                        PromotionDescription = string.IsNullOrEmpty(trx.PromoMessage) ? "NA" : trx.PromoMessage,
                        TransferAmount = trx.OriginatorsPrincipalAmount,
                        NetFee = trx.Fee,
                        Total = trx.GrossTotalAmount,
                        ReceiverName = (string.IsNullOrEmpty(trx.ReceiverFirstName) ? " " : trx.ReceiverFirstName) + " " + (string.IsNullOrEmpty(trx.ReceiverLastName) ? " " : trx.ReceiverLastName) + " " + (string.IsNullOrEmpty(trx.ReceiverSecondLastName) ? " " : trx.ReceiverSecondLastName),
                        ReceiverAddress = receiverAddress,
                        TestQuestion = trx.TestQuestion,
                        TestAnswer = trx.TestAnswer,
                        MTCN = trx.MTCN,
                        isModifiedOrRefunded = trx.IsModifiedOrRefunded,
                        TransactionSubType = trx.TransactionSubType,
                        TransferTax = (trx.MetaData.ContainsKey("TransferTax")) ? Convert.ToDecimal(Helper.GetDecimalDictionaryValueIfExists(trx.MetaData, "TransferTax")) : 0,
                        ProviderName = ((Helper.ProviderId)trx.ProviderId).ToString()
                    };
                    if (customerSessionId != 0)
                    {
                        ZeoClient.ReasonRequest request = new ZeoClient.ReasonRequest()
                        {
                            TransactionType = "REFUND"
                        };
                        tranDetails.RefundCategory = DefaultSelectList();
                        var htSessions = (Hashtable)Session["HTSessions"];
                        if (htSessions != null)
                        {
                            var agentSession = ((ZeoClient.AgentSession)(htSessions["TempSessionAgent"]));
                            if (agentSession != null)
                            {
                                ZeoClient.Response response = service.GetLocationById(Convert.ToInt32(context.LocationId), context);
                                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                                List<ZeoClient.Location> locations = response.Result as List<ZeoClient.Location>;
                                if (locations!=null && locations.Any() && !string.IsNullOrWhiteSpace(locations.FirstOrDefault().State))
                                {
                                    context.StateCode = locations.FirstOrDefault().State;
                                    alloyResponse = service.IsSWBStateXfer(context);
                                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                                    bool isLocationState = Convert.ToBoolean(alloyResponse.Result);
                                    if (isLocationState)
                                    {
                                        alloyResponse = service.GetAgentDetails(long.Parse(agentSession.SessionId), context);
                                        if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                                        ZeoClient.Agent cashierDetails = alloyResponse.Result as ZeoClient.Agent;

                                        tranDetails.isCashierState = true;
                                        tranDetails.AgentFirstName = cashierDetails.AgentFirstName;
                                        tranDetails.AgentLastName = cashierDetails.AgentLastName;
                                    }
                                }
                            }
                        }

                        string payStatus = string.Empty;
                        //Checking this condition as we dont this information in agent transaction history
                        if (context.CustomerSessionId > 0)
                        {
                            alloyResponse = service.GetStatus(trx.MTCN, context);
                            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                            tranDetails.PayStatus = alloyResponse.Result as string;
                        }

                        #region Shopping cart need to integrated
                        ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                        ZeoClient.ShoppingCart shoppingCart = new ZeoClient.ShoppingCart();

                        ZeoClient.Response shoppingCartResponse = serviceClient.GetShoppingCart(customerSessionId, context);
                        if (WebHelper.VerifyException(shoppingCartResponse)) throw new ZeoWebException(shoppingCartResponse.Error.Details);
                        shoppingCart = shoppingCartResponse.Result as ZeoClient.ShoppingCart;

                        if (shoppingCart.MoneyTransfers != null && shoppingCart.MoneyTransfers.Count > 0)
                            tranDetails.isAddedtoShoppingCart = shoppingCart.MoneyTransfers.Where(x => x.OriginalTransactionId == Convert.ToInt64(transactionId)).Any();
                        #endregion

                        tranDetails.PayStatusCodeMessage = GetPayStatusMessage(tranDetails.PayStatus);
                    }

                    return PartialView("_SendMoneyTransactionDetails", tranDetails);
                }
                #endregion

                #region Receive Money
                else if (transactionType.ToLower().Contains("receive"))
                {
                    alloyResponse = new ZeoClient.ZeoServiceClient().GetMoneyTransferTransaction(trxId, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    ZeoClient.MoneyTransferTransaction trx = alloyResponse.Result as ZeoClient.MoneyTransferTransaction;

                    MoneyTransferTransactionDetailsViewModel tranDetails = new MoneyTransferTransactionDetailsViewModel()
                    {
                        TransactionType = transactionType,
                        TransactionId = transactionId,
                        TransactionStatus = transactionStatus,
                        Fee = trx.Fee,
                        Discount = 0,
                        PromotionName = "NA",
                        PromotionDescription = "NA",
                        TransferAmount = trx.AmountToReceiver,
                        NetFee = trx.Fee,
                        Total = trx.AmountToReceiver + trx.Fee,
                        SenderName = trx.SenderName,
                        ProviderName = ((Helper.ProviderId)trx.ProviderId).ToString(),

                        MTCN = trx.MTCN
                    };
                    return PartialView("_ReceiveMoneyTransactionDetails", tranDetails);
                }
                #endregion

                #region MoneyOrder
                else if (transactionType.ToLower().Contains("moneyorder"))
                {

                    alloyResponse = service.GetMoneyOrderTransaction(trxId, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    ZeoClient.MoneyOrder MOtransaction = alloyResponse.Result as ZeoClient.MoneyOrder;

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
                        ProviderName = ((Helper.ProviderId)MOtransaction.ProviderId).ToString()
                    };
                    return PartialView("_MoneyOrderTransactionDetails", tranMoneyOrderDetails);
                }
                #endregion

                #region BillPay

                else if (transactionType.ToLower().Contains("billpay"))
                {

                    alloyResponse = service.GetBillPayTransaction(trxId, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    ZeoClient.BillPayTransaction transaction = alloyResponse.Result as ZeoClient.BillPayTransaction;

                    string promoCode = Convert.ToString(Helper.GetDictionaryValueIfExists(transaction.MetaData, "PromoCoupon"));
                    string promoDescription = Convert.ToString(Helper.GetDictionaryValueIfExists(transaction.MetaData, "PromotionDescription"));
                    promoCode = string.IsNullOrWhiteSpace(promoCode) ? "NA" : promoCode;
                    promoDescription = string.IsNullOrWhiteSpace(promoDescription) ? "NA" : promoDescription;

                    BillPayTransactionDetailsViewModel billPayModel = new BillPayTransactionDetailsViewModel()
                    {
                        TransactionType = transactionType,
                        TransactionId = transactionId,
                        TransactionStatus = transactionStatus,
                        Fee = (transaction.MetaData.ContainsKey("UnDiscountedFee")) ? Convert.ToDecimal(Helper.GetDecimalDictionaryValueIfExists(transaction.MetaData, "UnDiscountedFee")) : transaction.Fee,
                        Discount = Convert.ToDecimal(Helper.GetDecimalDictionaryValueIfExists(transaction.MetaData, "DiscountedFee")),
                        Amount = transaction.Amount,
                        PromotionName = promoCode,
                        PromotionDescription = promoDescription,
                        NetFee = transaction.Fee,
                        Total = transaction.Amount + transaction.Fee,

                        Payee = transaction.BillerName,
                        AccountNumber = transaction.AccountNumber.MaskAccountNumber(),
                        TenantId = transaction.MetaData.ContainsKey("TenantId") ? transaction.MetaData["TenantId"].ToString() : "",
                        MTCN = transaction.MetaData.ContainsKey("MTCN") ? transaction.MetaData["MTCN"] == null ? string.Empty : transaction.MetaData["MTCN"].ToString() : "",
                    };
                    return PartialView("_BillPayTransactionDetails", billPayModel);
                }
                #endregion

                #region Cash
                else if (transactionType.ToLower().Contains("cash"))
                {
                    alloyResponse = service.GetCashTransaction(long.Parse(transactionId), context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    ZeoClient.CashTransaction transaction = alloyResponse.Result as ZeoClient.CashTransaction;

                    CashTransactionDetailsViewModel cashModel = new CashTransactionDetailsViewModel()
                    {
                        Amount = transaction.Amount,
                        TransactionId = transactionId,
                        TransactionStatus = transaction.TransactionStatus,
                        TransactionType = transaction.TransactionType
                    };
                    return PartialView("_CashTransactionDetails", cashModel);
                }
                #endregion

            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return null;
            }

            return RedirectToAction("TransactionHistory");
        }

        public JsonResult GetReceiptData(string transactionId, long dt, string transactiontype, bool isSummaryReceiptRequired = false, bool isReprint = false)
        {
            try
            {
                ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response result = new ZeoClient.Response();
                ZeoClient.Receipt receipt = null;
                List<ZeoClient.Receipt> receiots = new List<ZeoClient.Receipt>();
                ZeoClient.ZeoContext context = GetZeoContext();

                long agentSessionId = long.Parse(Session["sessionId"].ToString());
                long customerSessionId = GetCustomerSessionId();
                List<ZeoClient.ReceiptData> receipts = new List<ZeoClient.ReceiptData>();
                ZeoClient.Response response;
                Dictionary<string, object> ssoCookie = GetSSOAttributes("SSO_AGENT_SESSION");
                int transactionTypeValue = 0;
                if (isSummaryReceiptRequired)
                {

                    if (transactiontype.ToLower().Contains("check"))
                    {
                        transactionTypeValue = 1;
                    }

                    else if (transactiontype.ToLower().Contains("billpay"))
                    {
                        transactionTypeValue = 2;
                    }

                    else if (transactiontype.ToLower().Contains("send") || transactiontype.ToLower().Contains("receive") || transactiontype.ToLower().Contains("refund"))
                    {
                        transactionTypeValue = 3;
                    }

                    else if (transactiontype.ToLower().Contains("moneyorder"))
                    {
                        transactionTypeValue = 5;
                    }

                    else if (transactiontype.ToLower().StartsWith("gpr") || transactiontype.ToLower().StartsWith("prepaid") || transactiontype.ToLower().StartsWith("companion"))
                    {
                        transactionTypeValue = 6;
                    }

                    else if (transactiontype.ToLower().StartsWith("cash"))
                    {
                        transactionTypeValue = 7;
                    }
                  

                    response = client.GetCustomerSessionId(Convert.ToInt64(transactionId), transactionTypeValue, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    string customerSessionIdValue =  response.Result.ToString();

                    response = client.GetSummaryReceipt(Convert.ToInt64(customerSessionIdValue), context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    receipt = response.Result as ZeoClient.Receipt; 
                    receiots = new List<ZeoClient.Receipt>() { receipt };
                    if (receiots.Count < 1)
                    {
                        return Json(new { data = "Receipt Template Not Found", success = false });
                    }
                    var jsonSummaryReceipt = new
                    {
                        data = receiots,
                        success = true
                    };

                    return Json(jsonSummaryReceipt, JsonRequestBehavior.AllowGet);
                }

                if (transactiontype.ToLower().Contains("check"))
                {
                    result = client.GetCheckReceipt(Convert.ToInt64(transactionId), isReprint, context);
                }
                else if (transactiontype.ToLower().StartsWith("gpr") || transactiontype.ToLower().StartsWith("prepaid") || transactiontype.ToLower().StartsWith("companion"))
                {
                    result = client.GetFundReceipt(Convert.ToInt64(transactionId), isReprint, context);
                }
                else if (transactiontype.ToLower().Contains("send") || transactiontype.ToLower().Contains("receive") || transactiontype.ToLower().Contains("refund"))
                {
                    result = client.GetMoneyTransferReceipt(Convert.ToInt64(transactionId), isReprint, context);
                }
                else if (transactiontype.ToLower().Contains("moneyorder"))
                {
                    result = client.GetMoneyOrderReceipt(long.Parse(transactionId), isReprint, context);
                }
                else if (transactiontype.ToLower().Contains("billpay"))
                {
                    result = client.GetBillpayReceipt(long.Parse(transactionId), isReprint, context);
                }
                else if (transactiontype.ToLower().Contains("summary"))
                {
                    result = client.GetSummaryReceipt(customerSessionId, context);
                }
                else if (transactiontype.ToLower().Contains("coupon"))
                {
                    result = client.GetCouponReceipt(customerSessionId, null);
                }
                if (WebHelper.VerifyException(result)) throw new ZeoWebException(result.Error.Details);
                receipt = result.Result as ZeoClient.Receipt;
                receiots = new List<ZeoClient.Receipt>() { receipt };
                if (receiots.Count < 1)
                {
                    return Json(new { data = "Receipt Template Not Found", success = false });
                }

                var jsonData = new
                {
                    data = receiots,
                    success = true
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult GetDoddfrankReceiptData(string transactionId, long dt)
        {
            try
            {
                ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                List<ZeoClient.Receipt> data = new List<ZeoClient.Receipt>();
                ZeoClient.Response response;

                response = client.GetDoddFranckRecipt(long.Parse(transactionId), false, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                data = new List<ZeoClient.Receipt>() { response.Result as ZeoClient.Receipt };

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
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        //public JsonResult GetCheckDeclinedReceiptData(string transactionId, long dt)
        //{
        //    try
        //    {
        //        List<ReceiptData> data = new List<ReceiptData>();
        //        Response response = new DMS.Server.Data.Response();


        //        long customerSessionId = GetCustomerSessionId();
        //        MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
        //        long agentSessionId = long.Parse(Session["sessionId"].ToString());

        //        response = desktop.GetCheckDeclinedReceiptData(agentSessionId, customerSessionId, long.Parse(transactionId), mgiContext);
        //        if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
        //        data = response.Result as List<ReceiptData>;
        //        if (data == null)
        //        {
        //            return Json(new { data = "Receipt Template Not Found", success = false });
        //        }
        //        var jsonData = new
        //        {
        //            data = data.ToArray(),
        //            success = true
        //        };
        //        return Json(jsonData, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        VerifyException(ex); return null;
        //    }
        //}

        public JsonResult GetTransactionForModify(string MTCN)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.SearchRequest request = new ZeoClient.SearchRequest()
                {
                    ConfirmationNumber = MTCN,
                    SearchRequestType = ZeoClient.HelperSearchRequestType.Modify
                };

                ZeoClient.Response serviceResponse = alloyServiceClient.Search(request, context);
                if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                ZeoClient.SearchResponse moneyTransferModify = serviceResponse.Result as ZeoClient.SearchResponse;
                var jsonData = new { success = true, moneyTransferModify.FirstName, moneyTransferModify.LastName, moneyTransferModify.SecondLastName, moneyTransferModify.TestQuestion, moneyTransferModify.TestAnswer, moneyTransferModify.MiddleName };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult GetRefundStatus(string MTCN)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                long customerSessionId = GetCustomerSessionId();

                MoneyTransferTransactionDetailsViewModel tranDetails = new MoneyTransferTransactionDetailsViewModel();

                ZeoClient.Response serviceResponse = new ZeoClient.Response();

                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.SearchRequest searchRequest = new ZeoClient.SearchRequest()
                {
                    ConfirmationNumber = MTCN,
                    SearchRequestType = ZeoClient.HelperSearchRequestType.Refund
                };

                serviceResponse = alloyServiceClient.Search(searchRequest, context);
                if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                ZeoClient.SearchResponse response = serviceResponse.Result as ZeoClient.SearchResponse;

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
                else if (RefundFlag == Helper.RefundType.FullAmount.ToString())
                {
                    tranDetails.RefundStatusDesc = "FULL REFUND AVAILABLE";
                }
                else if (RefundFlag == Helper.RefundType.PrincipalAmount.ToString())
                {
                    tranDetails.RefundStatusDesc = "PRINCIPAL REFUND AVAILABLE";
                }

                tranDetails.FeeRefund = response.FeeRefund;

                ZeoClient.ReasonRequest request = new ZeoClient.ReasonRequest()
                {
                    TransactionType = transactiontype
                };

                tranDetails.RefundCategory = GetRefundReasons(customerSessionId, request, context);
                tranDetails.RefundStatus = RefundFlag;

                var jsonData = new { success = true, tranDetails };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        //WU
        public JsonResult SendMoneyRefundSubmit(ZeoClient.SendMoneyRefundRequest moneyTransferRefund)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                long customerSessionId = GetCustomerSessionId();

                ZeoClient.Response response = alloyServiceClient.SendMoneyRefund(moneyTransferRefund, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                var jsonData = new { success = true };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        //MGI
        //This is used for MoneyGram channel partner
        //public ActionResult SendMoneyStageRefundSubmit(RefundSendMoneyRequest moneyTransferRefund)
        //{
        //    try
        //    {
        //        Desktop desktop = new Desktop();
        //        MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
        //        CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
        //        bool isAddedtoShoppingCart = false;
        //        DMS.Server.Data.Response response = new DMS.Server.Data.Response();
        //        ShoppingCart shoppingCart = new ShoppingCart();

        //        Response shoppingCartResponse = desktop.ShoppingCart(customerSession.CustomerSessionId);
        //        if (WebHelper.VerifyException(shoppingCartResponse)) throw new AlloyWebException(shoppingCartResponse.Error.Details);
        //        shoppingCart = shoppingCartResponse.Result as ShoppingCart;

        //        if (shoppingCart.MoneyTransfers != null && shoppingCart.MoneyTransfers.Count > 0)
        //            isAddedtoShoppingCart = shoppingCart.MoneyTransfers.Where(x => x.OriginalTransactionId == Convert.ToInt64(moneyTransferRefund.TransactionId)).Any();


        //        if (!isAddedtoShoppingCart)
        //        {
        //            response = desktop.StageRefundSendMoney(Convert.ToInt64(customerSession.CustomerSessionId), moneyTransferRefund, mgiContext);
        //            if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
        //            long PtnrTrxId = long.Parse(response.Result.ToString());

        //            var jsonData = new { success = true };

        //            return Json(jsonData, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            var jsonData = new { doRedirect = true };
        //            return Json(jsonData, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        VerifyException(ex); return null;
        //    }
        //}

        public ActionResult SendMoneyModifySubmit(ZeoClient.ModifyRequest moneyTransferModify)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.Response serviceResponse = alloyServiceClient.StageModify(moneyTransferModify, context);
                if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                ZeoClient.ModifyResponse response = serviceResponse.Result as ZeoClient.ModifyResponse;
                TempData["sendMoneyModifyIds"] = response;

                var jsonData = new { success = true };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }

        }

        public ActionResult SendMoneyRefundSuccess(string MTCN)
        {
            ZeoClient.SendMoneyRefundRequest refundSendMoney = new ZeoClient.SendMoneyRefundRequest();
            refundSendMoney.ConfirmationNumber = MTCN;
            return PartialView("_SendMoneyRefundSuccess", refundSendMoney);

        }

        public ActionResult SendMoneyRefundOK(ZeoClient.SendMoneyRefundRequest moneyTrfRefund)
        {
            Session["activeButton"] = "Receive Money";
            ReceiveMoney receive = new ReceiveMoney();
            receive.WesternUnionMTCN = moneyTrfRefund.ConfirmationNumber;
            return View("ReceiveMoney", "_Common", receive);
        }

        public ActionResult SendMoneyModifySuccess(string MTCN)
        {
            ZeoClient.SendMoneyRefundRequest MoneyTransferCancel = new ZeoClient.SendMoneyRefundRequest();
            MoneyTransferCancel.ConfirmationNumber = MTCN;
            return PartialView("_SendMoneyModifySuccess", MoneyTransferCancel);
        }

        public ActionResult SendMoneyRefundFail()
        {
            ZeoClient.SendMoneyRefundRequest MoneyTransferCancel = new ZeoClient.SendMoneyRefundRequest();
            return PartialView("_SendMoneyRefundFail", MoneyTransferCancel);
        }

        [CustomHandleError(ActionName = "TransactionHistory", ControllerName = "TransactionHistory", ResultType = "redirect")]
        public ActionResult SendMoneyConfirm()
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                Session["activeButton"] = "Send Money";

                if (TempData["sendMoneyModifyIds"] == null)
                    return View();

                ZeoClient.ModifyResponse sendMoneyModifyIds = (ZeoClient.ModifyResponse)TempData["sendMoneyModifyIds"];

                TempData["sendMoneyModifyIds"] = sendMoneyModifyIds;

                long transactionId = sendMoneyModifyIds.ModifyTransactionId;

                ZeoClient.Response response = alloyServiceClient.GetMoneyTransferTransaction(transactionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.MoneyTransferTransaction wtTrx = response.Result as ZeoClient.MoneyTransferTransaction;

                ZeoClient.Response receiverResponse = alloyServiceClient.GetReceiver(wtTrx.ReceiverId, context);
                if (WebHelper.VerifyException(receiverResponse)) throw new ZeoWebException(receiverResponse.Error.Details);
                ZeoClient.Receiver receiver = receiverResponse.Result as ZeoClient.Receiver;

                string expectedPayoutCityName = Helper.GetDictionaryValueIfExists(wtTrx.MetaData, "ExpectedPayoutCity");
                string deliveryOptionDesc = Helper.GetDictionaryValueIfExists(wtTrx.MetaData, "DeliveryOptionDesc");
                string receiveAgent = Helper.GetDictionaryValueIfExists(wtTrx.MetaData, "ReceiveAgentName");
                decimal transferTax = Convert.ToDecimal(Helper.GetDecimalDictionaryValueIfExists(wtTrx.MetaData, "TransferTax"));

                SendMoney sendMoney = new SendMoney()
                {
                    TransactionId = transactionId,
                    FirstName = wtTrx.ReceiverFirstName,
                    LastName = wtTrx.ReceiverLastName,
                    SecondLastName = wtTrx.ReceiverSecondLastName,
                    //PickUpLocation = wtTrx.ReceiverAddress, //Value not there in the Biz layer. So not sure why this is required.
                    City = expectedPayoutCityName,
                    StateProvince = receiver.State_Province,
                    Country = wtTrx.DestinationCountryCode,
                    CountryCode = wtTrx.DestinationCountryCode,
                    DeliveryMethodDesc = wtTrx.DeliveryServiceDesc,
                    DeliveryOptionDesc = deliveryOptionDesc,
                    PromoDiscount = wtTrx.PromotionDiscount,
                    CouponPromoCode = wtTrx.PromotionsCode,
                    TransferAmount = wtTrx.OriginatorsPrincipalAmount, //Since this screen is for SendMoney TransactionAmt will be OriginalPrincipalAmt else it will be DestinationPrincipalAmount.
                    TransferFee = wtTrx.Charges,  //Previously Fee was mapped to Charges in the CXN layer. Now Charges only taken instead of Fee.
                    OriginalFee = wtTrx.Charges + wtTrx.PromotionDiscount,
                    Amount = wtTrx.GrossTotalAmount,
                    ReceiverCityName = receiver.City,
                    PickupState = wtTrx.DestinationState,
                    PickupCity = expectedPayoutCityName,
                    ReceiveAgent = string.IsNullOrEmpty(receiveAgent) ? "NA" : receiveAgent,
                    ReceiverName = string.Format("{0} {1} {2} {3}", wtTrx.ReceiverFirstName, receiver.MiddleName, wtTrx.ReceiverLastName, wtTrx.ReceiverSecondLastName),
                    TransferTax = transferTax
                };

                response = alloyServiceClient.GetXfrCountries(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                sendMoney.LCountry = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                if (sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.Country) != null)
                {
                    sendMoney.Country = sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.Country).Text;
                    string countryCode = sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.CountryCode).Value;

                    response = alloyServiceClient.GetXfrStates(countryCode, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    sendMoney.LStates = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                    if (sendMoney.LStates.FirstOrDefault(x => x.Value == receiver.PickupState_Province) != null)
                        sendMoney.PickupState = sendMoney.LStates.FirstOrDefault(x => x.Value == receiver.PickupState_Province).Text;
                }

                if (sendMoney.LCountry.FirstOrDefault(x => x.Value == wtTrx.DestinationCountryCode) != null)
                    sendMoney.PickupCountry = sendMoney.LCountry.FirstOrDefault(x => x.Value == wtTrx.DestinationCountryCode).Text;

                sendMoney.enableEditContinue = true;
                ViewBag.Navigation = Resources.NexxoSiteMap.SendMoney;
                if (sendMoney.CountryCode.ToLower() == "us" || sendMoney.CountryCode.ToLower() == "usa" || sendMoney.CountryCode.ToLower() == "united states")
                {
                    sendMoney.isDomesticTransfer = true;
                    sendMoney.isDomesticTransferVal = "true";
                }
                else
                    sendMoney.isDomesticTransferVal = "false";

                //if (mtTrx.ProviderId == Convert.ToInt32(ProviderIds.MoneyGram))
                //{
                //    return View("SendMoneyConfirmMoneyGram", sendMoney);
                //}

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
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
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

        private List<SelectListItem> GetLocations(DateTime dateRange, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient svc = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response response = svc.GetCustomerTransactionLocations(dateRange, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            List<string> locations = response.Result as List<string>;
            List<SelectListItem> locationList = new List<SelectListItem>();
            locationList.Add(new SelectListItem { Value = string.Empty, Text = "All", Selected = true });
            locations.ForEach(item => locationList.Add(new SelectListItem { Text = item, Value = item }));
            return locationList;

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
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_CAN;
                        break;
                    case PayStatus.PURG:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_PURG;
                        break;
                    case PayStatus.OVLM:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_OVLM;
                        break;
                    case PayStatus.OVLQ:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_OVLQ;
                        break;
                    case PayStatus.SECQ:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_SECQ;
                        break;
                    case PayStatus.QQC1:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_QQC1;
                        break;
                    case PayStatus.BUST:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_BUST;
                        break;
                    case PayStatus.OFAC:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_OFAC;
                        break;
                    case PayStatus.FBST:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_FBST;
                        break;
                    case PayStatus.FBLK:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_FBLK;
                        break;
                    case PayStatus.ACPT:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_ACPT;
                        break;
                    case PayStatus.AUTH:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_AUTH;
                        break;
                    case PayStatus.QQC2:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_QQC2;
                        break;
                    case PayStatus.ACRQ:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_ACRQ;
                        break;
                    case PayStatus.CUBA:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_CUBA;
                        break;
                    case PayStatus.SWPA:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_SWPA;
                        break;
                    case PayStatus.HOLD:
                        message = App_GlobalResources.Nexxo.SendMoneyModifyHOLD;
                        break;
                    case PayStatus.PKUP:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_PKUP;
                        break;
                    case PayStatus.PKPQ:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_PKPQ;
                        break;
                    case PayStatus.UNAV:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_UNAV;
                        break;
                    case PayStatus.PHD:
                        message = App_GlobalResources.Nexxo.PayRequestStatusCode_PHD;
                        break;
                }
            }

            return message;
        }

        private string GetFundTitle(int fundType)
        {
            string title = string.Empty;
            string titleFormat = "PREPAID CARD - {0}";
            if (fundType == (int)ZeoClient.HelperFundType.Debit)
                title = string.Format(titleFormat, "WITHDRAW");
            else if (fundType == (int)ZeoClient.HelperFundType.Credit)
                title = string.Format(titleFormat, "LOAD");
            else
                title = string.Format(titleFormat, "ACTIVATE");

            return title;
        }

        //private string PrepareReceiptForPrinting(SharedData.Receipt receipt)
        //{
        //    return receipt.Lines[0];
        //    // return PrepareReceiptForPrinting(receipt.Lines.ToArray());// receipt.Lines[0];
        //}

        private List<ZeoClient.ReceiptData> PrepareContentReceiptForPrinting(List<ZeoClient.ReceiptData> receiptDatas)
        {

            List<ZeoClient.ReceiptData> printLines = new List<ZeoClient.ReceiptData>();

            ZeoClient.ReceiptData receiptData;

            for (int i = 0; i < receiptDatas.Count; i++)
            {
                receiptData = new ZeoClient.ReceiptData();
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

        private object GetDictionaryValueIfExists(Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            return null;
        }
    }
}

