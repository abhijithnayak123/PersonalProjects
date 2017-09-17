using System;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
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
            try
            {

                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                response = alloyClient.GetShoppingCart(context.CustomerSessionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.ShoppingCart shoppingCart = response.Result as ZeoClient.ShoppingCart;

                //Re verify the status of pending checks.
                if (shoppingCart.Checks != null)
                {
                    var check = shoppingCart.Checks.Where(c => c.Status == (TransactionStatus.Pending).ToString("D")).FirstOrDefault();
                    if (check != null)
                    {
                        //Do not throw the exception in Cart summary, if login failed from INGO 
                        try
                        {
                            alloyClient.GetCheckStatus(Convert.ToInt64(check.Id), GetCheckLogin());
                        }
                        catch { }

                        response = alloyClient.GetShoppingCart(context.CustomerSessionId, context);

                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                        shoppingCart = response.Result as ZeoClient.ShoppingCart;


                    }
                }

                ShoppingCartSummary shoppingCartSummary = ShoppingCartHelper.ShoppingCartSummary(shoppingCart);

                bool hasGPRActivation = (shoppingCart.GPRCards == null) ? false : shoppingCart.GPRCards.Exists(x => x.ItemType == Helper.FundType.Activation.ToString());

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
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpGet]
        public ActionResult ShoppingCartCheckout(bool IsException = false, string ExceptionMessage = "")
        {

            ZeoClient.Response response = new ZeoClient.Response();
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();

            ViewBag.IsException = IsException;
            ViewBag.ExceptionMessage = ExceptionMessage;

            ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();
            ZeoClient.ShoppingCart shoppingCart = new ZeoClient.ShoppingCart();

            Session["activeButton"] = null;


            try
            {
                response = alloyServiceClient.GetShoppingCart(context.CustomerSessionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                shoppingCart = response.Result as ZeoClient.ShoppingCart;

                shoppingCartDetail = ShoppingCartHelper.ShoppingCartDetailed(shoppingCart);

                if (shoppingCartDetail.CustomerSession.IsGPRCustomer)
                {
                    shoppingCartDetail.CardHolder = shoppingCartDetail.CustomerSession.IsGPRCustomer;
                    shoppingCartDetail.MinimumLoadAmount = GetMinimumLoadAmount(false, context);
                }

                shoppingCartDetail.LoadToCardToDisplay = 0;

                Session["PendingChecks"] = false;

                if (Session["ShoppingCartCheckOutStatus"] == null || Session["ShoppingCartCheckOutStatus"].ToString() == "")
                    Session["ShoppingCartCheckOutStatus"] = ZeoClient.HelperShoppingCartCheckoutStatus.InitialCheckout;

                if (shoppingCartDetail.Items.Any())
                {
                    Session["IsShoppingCartExists"] = true;
                    if (shoppingCart.Checks.Any(c => c.Status == ((int)TransactionStatus.Pending).ToString()))
                        Session["PendingChecks"] = true;

                    if (shoppingCart.GPRCards != null)
                        UpdatePrepaidCard(ref shoppingCartDetail, shoppingCart, context);

                    shoppingCartDetail.CashToCustomer = shoppingCartDetail.DueToCustomer + shoppingCart.CashInTotal;
                    shoppingCartDetail.CashCollected = shoppingCartDetail.PreviousCashCollected = shoppingCart.Cash.Where(x => x.CashType == ZeoClient.HelperCashType.CashIn).Sum(x => x.Amount);

                    if (shoppingCartDetail.DueToCustomer < 0)
                        ViewBag.NetDueMessage = "Net Due from Customer $";
                    else
                        ViewBag.NetDueMessage = "Net Due to Customer $";

                }

                if (shoppingCartDetail.Items.Any(i => i.DiscountApplied < 0))
                    ViewBag.PromotionText = "* Promotion applied to transaction";


                ViewBag.cardStatus = GetPrepaidCardStatus();

                return View("ShoppingCartCheckout", shoppingCartDetail);
            }
            catch (Exception ex)
            {
                VerifyException(ex);

                if (shoppingCartDetail.CustomerSession.IsGPRCustomer)
                    shoppingCartDetail.CardHolder = shoppingCartDetail.CustomerSession.IsGPRCustomer;

                ViewBag.cardStatus = GetPrepaidCardStatus();

                return View("ShoppingCartCheckout", shoppingCartDetail);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartCheckout"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ActionName = "ShoppingCartCheckout", ControllerName = "ShoppingCart", ResultType = "redirect")]
        public ActionResult ShoppingCartCheckout(ShoppingCartDetail shoppingCartDetail, string submit, string recalc)
        {
            ShoppingCartSuccess shoppingCartSuccess = null;
            bool processMoneyOrder = false;
            long customerSessionId = GetCustomerSessionId();
            long agentSeesionId = GetAgentSessionId();

            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response alloyResponse = new ZeoClient.Response();
            ZeoClient.ZeoContext context = GetZeoContext();

            try
            {
                alloyResponse = alloyServiceClient.GetShoppingCart(customerSessionId, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                ZeoClient.ShoppingCart shoppingCart = alloyResponse.Result as ZeoClient.ShoppingCart;


                if (ModelState.IsValid)
                {
                    CheckCounterId(shoppingCart, context);

                    ValidatePendingCheckAndPrepaidCard(agentSeesionId, shoppingCartDetail, shoppingCart, context);

                    shoppingCartDetail.CashToCustomer = CalculateCashToCustomer(shoppingCartDetail, shoppingCart);

                    shoppingCartDetail.CardHolder = shoppingCartDetail.CustomerSession.IsGPRCustomer;

                    //Update the prepaid transaction tables, in case customer wishes to adjust the Load or withdraw amount from shopping cart checkout.

                    ShoppingCartHelper.UpdateFundsTransaction(shoppingCartDetail, shoppingCart, context);

                    if (decimal.Round(shoppingCartDetail.CashToCustomer, 2) < 0)
                        throw new ZeoWebException(GetErrorMessage(ShoppingCartException.INSUFFICIENT_FUNDS, context));


                    if (recalc != null)
                        throw new Exception("ReloadException");


                    string cardNumber = shoppingCartDetail.CustomerSession.CardNumber ?? string.Empty;


                    context.IsReferral = shoppingCartDetail.IsReferral;
                    context.SSOAttributes = GetSSOAttributes("SSO_AGENT_SESSION");


                    if (shoppingCart.Checks.Any())
                        context = GetCheckLogin();

                    alloyResponse = alloyServiceClient.ShoppingCartCheckout(shoppingCartDetail.CashToCustomer, (ZeoClient.HelperShoppingCartCheckoutStatus)Session["ShoppingCartCheckOutStatus"], context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    ZeoClient.HelperShoppingCartCheckoutStatus shoppingCartCheckoutStatus = (ZeoClient.HelperShoppingCartCheckoutStatus)alloyResponse.Result;

                    Session["ShoppingCartCheckOutStatus"] = shoppingCartCheckoutStatus;

                    switch (shoppingCartCheckoutStatus)
                    {

                        case ZeoClient.HelperShoppingCartCheckoutStatus.Completed:

                            Session["ShoppingCartCheckOutStatus"] = null;

                            UpdatePrepaidCard(ref shoppingCartDetail);

                            GetShoppingCartSuccess(shoppingCartDetail.CashToCustomer, ref shoppingCartSuccess, shoppingCart);
                            TempData["shoppingCartSuccess"] = shoppingCartSuccess;

                            if (shoppingCartSuccess.ReceiptCount <= 0)
                                return RedirectToAction("ProductInformation", "Product");

                            if (Session["CardNo"] != null)
                                Session.Remove("CardNo");

                            Session.Remove("IsShoppingCartExists");

                            return RedirectToAction("ShoppingCartCheckoutSuccess", "ShoppingCart");

                        case ZeoClient.HelperShoppingCartCheckoutStatus.MOPrinting:
                            processMoneyOrder = true;
                            break;

                        case ZeoClient.HelperShoppingCartCheckoutStatus.CashOverCounter:
                            UpdatePrepaidCard(ref shoppingCartDetail); // Card balance session to be updated once after withdrawing the amount
                            ViewBag.CashOverCounter = true;
                            break;

                        default:
                            break;
                    }

                }

                alloyResponse = alloyServiceClient.GetShoppingCart(customerSessionId, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                shoppingCart = alloyResponse.Result as ZeoClient.ShoppingCart;
                ShoppingCartDetail cartDetail = ShoppingCartHelper.ShoppingCartDetailed(shoppingCart);

                cartDetail.LoadToCardToDisplay = shoppingCartDetail.LoadToCard;
                cartDetail.CashToCustomer = shoppingCartDetail.CashToCustomer;
                cartDetail.CardHolder = shoppingCartDetail.CardHolder;
                cartDetail.CardBalance = shoppingCartDetail.CardBalance;
                cartDetail.MinimumLoadAmount = shoppingCartDetail.MinimumLoadAmount;
                shoppingCartDetail = cartDetail;

                if (shoppingCart.GPRCards != null)
                    UpdatePrepaidCard(ref shoppingCartDetail, shoppingCart, context);

                if (processMoneyOrder)
                {
                    @ViewBag.ProcessMoneyOrder = true;
                    @ViewBag.CashPreviouslyCollected = shoppingCart.Cash.Where(x => x.CashType == ZeoClient.HelperCashType.CashIn).Sum(x => x.Amount);
                    Session["MoneyOrder"] = shoppingCart.MoneyOrders.Where(x => x.State == (int)TransactionStatus.Processing).FirstOrDefault();
                }

                ViewBag.cardStatus = GetPrepaidCardStatus();

                if (shoppingCartDetail.DueToCustomer < 0)
                    ViewBag.NetDueMessage = "Net Due from Customer $";
                else
                    ViewBag.NetDueMessage = "Net Due to Customer $";


                return View("ShoppingCartCheckout", shoppingCartDetail);
            }
            catch (Exception ex)
            {
                if (shoppingCartSuccess != null)
                    return View("CheckoutSuccess", shoppingCartSuccess);

                if (ex.Message != "ReloadException")
                    VerifyException(ex);

                return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");

            }
        }

        private decimal CalculateCashToCustomer(ShoppingCartDetail shoppingCartDetail, ZeoClient.ShoppingCart shoppingCart)
        {
            decimal cashToCustomer = 0M;

            cashToCustomer = (shoppingCartDetail.DueToCustomer - shoppingCartDetail.LoadToCard + shoppingCartDetail.WithdrawFromCard + shoppingCartDetail.CashCollected);

            //During cash over counter we are getting insufficient funds exception,so added CashOverCounter check to take previousCashIn
            if ((ZeoClient.HelperShoppingCartCheckoutStatus)Session["ShoppingCartCheckOutStatus"] == ZeoClient.HelperShoppingCartCheckoutStatus.CashCollected || (ZeoClient.HelperShoppingCartCheckoutStatus)Session["ShoppingCartCheckOutStatus"] == ZeoClient.HelperShoppingCartCheckoutStatus.CashOverCounter)
            {
                if (shoppingCart.Cash.Any(x => x.CashType == ZeoClient.HelperCashType.CashIn))
                {
                    decimal previousCashIn = shoppingCart.Cash.Where(x => x.CashType == ZeoClient.HelperCashType.CashIn).LastOrDefault().Amount;

                    cashToCustomer = cashToCustomer + previousCashIn;
                }
            }

            return cashToCustomer;
        }

        private void UpdatePrepaidCard(ref ShoppingCartDetail shoppingCartDetail, ZeoClient.ShoppingCart shoppingCart, ZeoClient.ZeoContext context)
        {
            shoppingCartDetail.LoadToCardToDisplay = shoppingCart.GPRCards.Where(x => x.Status == TransactionStatus.Authorized.ToString("D")).Sum(x => x.LoadAmount);

            shoppingCartDetail.LoadFee = shoppingCartDetail.WithdrawFee = 0M;

            shoppingCartDetail.CardHolder = shoppingCartDetail.CustomerSession.IsGPRCustomer;

            //AL-514 Fix for when MO fails and click on submit doubling of CashToCustomer and PreviouslyCashCollected  

            shoppingCartDetail.WithdrawFromCard = shoppingCart.GPRCards.Where(x => x.ItemType == ZeoClient.HelperFundType.Debit.ToString() && x.Status != TransactionStatus.Committed.ToString("D")).Sum(x => x.WithdrawAmount);

            if (shoppingCart.GPRCards.Any(x => x.ItemType == ZeoClient.HelperFundType.Activation.ToString()))
            {
                shoppingCartDetail.CardHolder = shoppingCartDetail.IsCardActivationTrx = true;

                bool IsCardActivate = true;

                ViewBag.InitLoadAmt = "true";

                if (shoppingCart.GPRCards.Any(x => x.ItemType == ZeoClient.HelperFundType.Activation.ToString() && x.Status == TransactionStatus.Committed.ToString("D")))
                {
                    //if gpr card activation was part of shopping cart and has been successful then update the current customer session to update the profile applet.
                    string currentSessionCardNumber = shoppingCart.GPRCards.Where(x => x.ItemType == ZeoClient.HelperFundType.Activation.ToString() && x.Status == TransactionStatus.Committed.ToString("D")).FirstOrDefault().CardNumber;
                    IsCardActivate = false;
                    shoppingCartDetail.CustomerSession.IsGPRCustomer = true;
                    shoppingCartDetail.CustomerSession.CardNumber = currentSessionCardNumber;
                    Session["CustomerSession"] = shoppingCartDetail.CustomerSession;
                    if (Session["CardNumber"] != null)
                        Session["CardNumber"] = currentSessionCardNumber;

                }

                shoppingCartDetail.MinimumLoadAmount = GetMinimumLoadAmount(IsCardActivate, context);
            }
        }

        private void UpdatePrepaidCard(ref ShoppingCartDetail shoppingCartDetail)
        {
            //if gpr card activation was part of shopping cart and has been successful then update the current customer session to update the profile applet.
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.Response alloyResponse;

            if (shoppingCartDetail.IsCardActivationTrx)
            {
                alloyResponse = alloyServiceClient.GetPrepaidCardNumber(context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                string currentSessionCardNumber = (string)alloyResponse.Result;

                shoppingCartDetail.CustomerSession.IsGPRCustomer = shoppingCartDetail.CardHolder = true;

                shoppingCartDetail.CustomerSession.Customer.CardNumber = ((Models.Customer)Session["Customer"]).CardNumber = currentSessionCardNumber;
                Session["CustomerSession"] = shoppingCartDetail.CustomerSession;
                if (Session["CardNumber"] != null)
                    Session["CardNumber"] = currentSessionCardNumber;
                Session["IsGPRCard"] = true;
                GetCardBalance(alloyServiceClient, context);
            }
            else if (shoppingCartDetail.CardHolder && Session["CardBalance"] != null && (shoppingCartDetail.LoadToCard > 0 || shoppingCartDetail.WithdrawFromCard > 0))
            {
                ZeoClient.CardBalanceInfo cardInfo = GetCardBalance(alloyServiceClient, context);
                shoppingCartDetail.CardBalance = cardInfo.Balance;
            }
        }

        private ZeoClient.CardBalanceInfo GetCardBalance(ZeoClient.ZeoServiceClient alloyServiceClient, ZeoClient.ZeoContext context)
        {
            ZeoClient.CardBalanceInfo cardInfo = null;
            ZeoClient.Response response;

            response = alloyServiceClient.GetCardBalance(context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            cardInfo = response.Result as ZeoClient.CardBalanceInfo;

            Session["CardBalance"] = cardInfo;

            return cardInfo;
        }

        private void ValidatePendingCheckAndPrepaidCard(long agentSeesionId, ShoppingCartDetail shoppingCartDetail, ZeoClient.ShoppingCart shoppingCart, ZeoClient.ZeoContext context)
        {
            //Showing pop-up for not adding more than one GPR transactions
            if (shoppingCart.GPRCards != null && shoppingCart.GPRCards.Exists(x => x.Status == TransactionStatus.Committed.ToString("D") && (shoppingCartDetail.WithdrawFromCard > 0 || shoppingCartDetail.LoadToCard > 0)))
                throw new ZeoWebException(GetErrorMessage(ShoppingCartException.GPR_TRANSACTION_ALREADY_EXIST, context));


            if (shoppingCart.Checks != null && shoppingCart.Checks.Any(c => c.Status != TransactionStatus.Pending.ToString("D") && c.Status != TransactionStatus.Authorized.ToString("D") && c.Status != TransactionStatus.Committed.ToString("D")))
                throw new ZeoWebException(GetErrorMessage(ShoppingCartException.DECLINED_CHECK_EXIST, context));


            if (shoppingCart.MoneyOrders != null && shoppingCart.MoneyOrders.Any(c => Convert.ToString(c.State) == TransactionStatus.Failed.ToString("D")))
                throw new ZeoWebException(GetErrorMessage(ShoppingCartException.FAILED_MONEYORDER, context));


            if (shoppingCart.GPRCards != null)
            {
                if (shoppingCartDetail.CardBalance < (Math.Abs(shoppingCartDetail.WithdrawFromCard)) && shoppingCartDetail.WithdrawFromCard > 0)
                    throw new ZeoWebException(GetErrorMessage(ShoppingCartException.AVAILABLE_BALANCE_LESS, context));

                if (shoppingCart.GPRCards.Any(x => x.ItemType == ZeoClient.HelperFundType.Activation.ToString()) && shoppingCartDetail.WithdrawFromCard > 0)
                    throw new ZeoWebException(GetErrorMessage(ShoppingCartException.CARD_NOT_ACTIVATED, context));
            }

        }

        private void GetShoppingCartSuccess(decimal cashToCustomer, ref ShoppingCartSuccess shoppingCartSuccess, ZeoClient.ShoppingCart cart)
        {
            ZeoClient.ZeoContext context = GetZeoContext();

            shoppingCartSuccess = new ShoppingCartSuccess();

            if (cashToCustomer > 0)
                shoppingCartSuccess.CashToCustomer = cashToCustomer;

            shoppingCartSuccess.CheckOutResult = true;

            shoppingCartSuccess.FrankCheck = false;

            shoppingCartSuccess.CheckCount = 0;

            if (cart.Checks != null && cart.Checks.Count > 0)
            {
                shoppingCartSuccess.FrankCheck = cart.IsCheckFrank;
                if (shoppingCartSuccess.FrankCheck)
                {
                    shoppingCartSuccess.CheckCount = cart.Checks.Count;
                    shoppingCartSuccess.CheckData = getCheckFrankCount(cart);
                }
            }



            ZeoClient.Response receiptsResponse = new ZeoClient.ZeoServiceClient().GenerateReceiptsForShoppingCart(GetCustomerSessionId(), cart.Id, context);
            if (WebHelper.VerifyException(receiptsResponse)) throw new ZeoWebException(receiptsResponse.Error.Details);
            ZeoClient.Receipts Receipts = receiptsResponse.Result as ZeoClient.Receipts;

            if (Receipts != null && Receipts.receiptType != null && Receipts.receiptType.Count > 0)
            {
                shoppingCartSuccess.ReceiptCount = Receipts.receiptType.Count;
                shoppingCartSuccess.ReceiptType = ShoppingCartHelper.GetReceiptsType(Receipts);
            }
        }

        private int? GetPrepaidCardStatus()
        {
            if (Session["CardBalance"] != null)
                return ((ZeoClient.CardBalanceInfo)Session["CardBalance"]).CardStatus;

            return null;
        }

        private decimal GetMinimumLoadAmount(bool IsCardActivateOrLoadToCard, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient alloyService = new ZeoClient.ZeoServiceClient();

            ZeoClient.Response response = alloyService.GetMinimumLoadAmount(IsCardActivateOrLoadToCard, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

            return Convert.ToDecimal(response.Result);
        }

        // ShoppingCart Checkout - Park / Remove Cart item
        public JsonResult ParkAndDeleteShoppingCartItems()
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response;

                response = alloyServiceClient.GetShoppingCart(context.CustomerSessionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                ZeoClient.ShoppingCart shoppingCart = response.Result as ZeoClient.ShoppingCart;

                if (shoppingCart != null && shoppingCart.Id != 0)
                {
                    foreach (var item in shoppingCart.Checks)
                    {
                        try
                        {
                            if (item.Status != TransactionStatus.Committed.ToString("D"))
                                ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.Checks, context);
                        }
                        catch { }
                    }

                    foreach (var item in shoppingCart.Bills)
                    {
                        try
                        {
                            if (item.Status != TransactionStatus.Committed.ToString("D"))
                                RemoveShoppingCartTrx(item.Id, ProductType.BillPay);
                        }
                        catch { }
                    }

                    foreach (var item in shoppingCart.MoneyTransfers)
                    {
                        try
                        {
                            if (item.Status != TransactionStatus.Committed.ToString("D"))
                            {
                                if (item.TransferType == (int)ZeoClient.HelperMoneyTransferType.Send)
                                    RemoveShoppingCartTrx(item.Id, ProductType.SendMoney);
                                else if (item.TransferType == (int)ZeoClient.HelperMoneyTransferType.Receive)
                                    RemoveShoppingCartTrx(item.Id, ProductType.ReceiveMoney);
                            }
                        }
                        catch { }
                    }

                    foreach (var item in shoppingCart.MoneyOrders)
                    {
                        try
                        {
                            if (Convert.ToString(item.State) != TransactionStatus.Committed.ToString("D"))
                                ShoppingCartHelper.ParkShoppingCartTrx(item.Id, ProductType.MoneyOrder, context);
                        }
                        catch { }
                    }

                    foreach (var item in shoppingCart.GPRCards)
                    {
                        try
                        {
                            if (item.Status != TransactionStatus.Committed.ToString("D"))
                            {
                                switch ((ZeoClient.HelperFundType)Enum.Parse(typeof(ZeoClient.HelperFundType), item.ItemType))
                                {
                                    case ZeoClient.HelperFundType.Credit:
                                        RemoveShoppingCartTrx(long.Parse(item.Id), ProductType.GPRLoad);
                                        break;
                                    case ZeoClient.HelperFundType.Debit:
                                        RemoveShoppingCartTrx(long.Parse(item.Id), ProductType.GPRWithdraw);
                                        break;
                                    case ZeoClient.HelperFundType.Activation:
                                        RemoveShoppingCartTrx(long.Parse(item.Id), ProductType.GPRActivation);
                                        break;
                                }
                            }
                        }
                        catch { }
                    }

                    foreach (var item in shoppingCart.Cash)
                    {
                        try
                        {
                            if (item.Status != TransactionStatus.Committed.ToString("D"))
                            {
                                RemoveShoppingCartTrx(long.Parse(item.Id), ProductType.CashIn);
                            }
                        }
                        catch { }

                    }

                }
                //alloyContext.IsAvailable = true;
                alloyServiceClient.UpdateCounterId(context);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        // ShoppingCart Checkout - ShoppingCart Success
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [CustomHandleErrorAttribute(ViewName = "CheckoutSuccess", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ShoppingCartSuccess")]
        public ActionResult ShoppingCartCheckoutSuccess()
        {
            ShoppingCartSuccess shoppingCartSuccess = new ShoppingCartSuccess();
            if (TempData["shoppingCartSuccess"] != null)
            {
                shoppingCartSuccess = TempData["shoppingCartSuccess"] as ShoppingCartSuccess;
                TempData.Keep("shoppingCartSuccess");
            }
            ViewBag.customerSessionId = GetCustomerSessionId();
            return View("CheckoutSuccess", shoppingCartSuccess);
        }

        // ShoppingCart Checkout - ShoppingCart Success
        /// <summary>
        /// To get any Exception Occured in ShoppingCartCheckoutSuccess Page
        /// </summary>
        /// <returns></returns>
        public JsonResult ShoppingCartCheckOutSuccessDetails()
        {
            var jsonData = new { data = "Zeo | 1003.6004 | Some Exception Occured in ShoppingCartCheckOutSuccess Page | Some Exception Occured in ShoppingCartCheckOutSuccess Page", Success = true };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        // ShoppingCart Checkout - Confirm for ShoppingCart Success
        [CustomHandleErrorAttribute(ViewName = "CheckoutConfirm", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ShoppingCartSuccess")]
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
            try
            {
                ProductType prodtype = ProductType.None;
                Enum.TryParse(product, out prodtype);
                RemoveShoppingCartTrx(long.Parse(id), prodtype);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                if (!string.IsNullOrEmpty(screenName) && screenName.ToLower() == "checkout")
                {
                    return RedirectToAction("ShoppingCartCheckout", "ShoppingCart", new { IsException = true, ExceptionMessage = ViewBag.ExceptionMessage });
                }
            }
            if (!string.IsNullOrEmpty(screenName) && screenName.ToLower() == "checkout")
            {
                return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
            }
            return RedirectToAction("ProductInformation", "Product");
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

            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            long trxId = long.Parse(id);


            try
            {
                ZeoClient.Response response = alloyServiceClient.GetShoppingCart(context.CustomerSessionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.ShoppingCart shoppingCart = response.Result as ZeoClient.ShoppingCart;

                bool isMobileEnabled = false; ////isMobileEnabledForTextMessage(customerSession); note: This feature is not using in current alloy applications..
                string message = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.ParkMessage;

                ParkTransaction parkTransaction = new ParkTransaction();


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

                Product providerConfig = parkTransaction.Providers.FirstOrDefault(x => x.ProcessorName == "WesternUnion" && x.Name == "ReceiveMoney");

                if (providerConfig != null)
                    CanParkReceiveMoney = providerConfig.CanParkReceiveMoney;

                ViewBag.CanPark = CanParkReceiveMoney;

                if (!CanParkReceiveMoney && prodtype == ProductType.ReceiveMoney)
                    message = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.DeleteMessage;

                ViewBag.Message = message;

                switch (prodtype)
                {
                    case ProductType.Checks:
                        ZeoClient.Check checkdetails = shoppingCart.Checks.Find(x => x.Id == trxId);
                        parkTransaction.Amount = checkdetails.Amount;
                        parkTransaction.Detail = checkdetails.StatusDescription;
                        parkTransaction.IconName = "processcheckGreen.png";
                        if (!string.IsNullOrEmpty(checkdetails.DiscountName))
                            parkTransaction.CheckMOPromotionAlertMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CheckMOPromotionAlertMessage;
                        break;

                    case ProductType.BillPay:
                        ZeoClient.BillPayTransaction billpayDetails = shoppingCart.Bills.Find(x => x.Id == trxId);
                        parkTransaction.Amount = billpayDetails.Amount;
                        parkTransaction.Detail = string.Format("{0} A/C {1}", billpayDetails.BillerName, billpayDetails.AccountNumber);
                        parkTransaction.IconName = "BillPayGreen.png";
                        break;

                    case ProductType.ReceiveMoney:
                    case ProductType.SendMoney:
                        ZeoClient.MoneyTransfer moneyTransactionDetails = shoppingCart.MoneyTransfers.Find(x => x.Id == trxId);
                        parkTransaction.Amount = moneyTransactionDetails.Amount;
                        parkTransaction.Detail = string.Format("{0} - {1}", moneyTransactionDetails.ReceiverFirstName + moneyTransactionDetails.ReceiverLastName, moneyTransactionDetails.DestinationCountry);
                        parkTransaction.IconName = prodtype == ProductType.SendMoney ? "SendMoneyGreen.png" : "ReceiveMoneyGreen.png";
                        break;

                    case ProductType.GPRActivation:
                        ZeoClient.GPRCard cardTransaction = shoppingCart.GPRCards.Find(x => x.Id == id);
                        parkTransaction.Amount = cardTransaction.LoadAmount;
                        parkTransaction.Detail = string.Format("Activate ****{0}", ShoppingCartHelper.getAcctLast4Digits(cardTransaction.CardNumber));
                        parkTransaction.IconName = "CredentialsGreen.png";

                        if (!string.IsNullOrEmpty(cardTransaction.DiscountName))
                            parkTransaction.CheckMOPromotionAlertMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CheckMOPromotionAlertMessage;

                        if (cardTransaction.InitialLoadAmount != 0)
                            parkTransaction.Amount = cardTransaction.InitialLoadAmount;
                        break;

                    case ProductType.AddOnCard:
                        ZeoClient.GPRCard cardTrx = shoppingCart.GPRCards.Find(x => x.Id == id);
                        parkTransaction.Amount = cardTrx.LoadAmount;
                        parkTransaction.Detail = string.Format("Companion Card Order - [{0}]", cardTrx.AddOnCustomerName);
                        parkTransaction.IconName = "CredentialsGreen.png";
                        if (cardTrx.InitialLoadAmount != 0)
                            parkTransaction.Amount = cardTrx.InitialLoadAmount;
                        break;

                    case ProductType.MoneyOrder:
                        ZeoClient.MoneyOrder moneyOrderTransactionDetails = shoppingCart.MoneyOrders.Find(x => x.Id == Convert.ToInt64(id));
                        parkTransaction.Amount = moneyOrderTransactionDetails.Amount;
                        parkTransaction.Detail = moneyOrderTransactionDetails.StatusDescription;
                        parkTransaction.IconName = "MoneyOrderGreen.png";
                        if (!string.IsNullOrEmpty(moneyOrderTransactionDetails.DiscountName))
                            parkTransaction.CheckMOPromotionAlertMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.CheckMOPromotionAlertMessage;
                        break;

                }
                if (isMobileEnabled)  // Note: This features is not using in current alloy applications.
                {
                    // TODO : need to implement as part of shopping cart & Customer session
                    ////if (!string.IsNullOrEmpty(customerSession.Customer.Phone1.Number) && customerSession.Customer.Phone1.Type == "Cell")
                    ////{
                    ////    string phonenumber = customerSession.Customer.Phone1.Number;
                    ////    parkTransaction.MobileNumber = phonenumber.Substring(0, 3) + '-' + phonenumber.Substring(3, 3) + '-' + phonenumber.Substring(6);
                    ////}
                    ////else if (!string.IsNullOrEmpty(customerSession.Customer.Phone2.Number) && customerSession.Customer.Phone2.Type == "Cell")
                    ////{
                    ////    string phonenumber = customerSession.Customer.Phone2.Number;
                    ////    parkTransaction.MobileNumber = phonenumber.Substring(0, 3) + '-' + phonenumber.Substring(3, 3) + '-' + phonenumber.Substring(6);
                    ////}

                    parkTransaction.InfoMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.SMSDisableAlert;
                }
                else
                {
                    parkTransaction.InfoMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.SMSEnableAlert;
                }


                if (parkTransaction.TransactionType == "GPRActivation")
                {
                    ViewBag.Message = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.VisaCardParkMessage;
                    return PartialView("_RemoveTransaction", parkTransaction);
                }
                else
                {
                    return PartialView("_ParkTransactionItem", parkTransaction);
                }
            }
            catch (Exception ex)
            {
                ParkTransaction parkTransaction = new ParkTransaction();
                bool CanParkReceiveMoney = true;
                //REVIEW-SC: Use enum, instead of string comparision
                Product providerConfig = parkTransaction.Providers.FirstOrDefault(x => x.ProcessorName == "WesternUnion" && x.Name == "ReceiveMoney");
                if (providerConfig != null)
                    CanParkReceiveMoney = providerConfig.CanParkReceiveMoney;

                ViewBag.CanPark = CanParkReceiveMoney;
                return PartialView("_ParkTransactionItem", parkTransaction);
            }
        }

        // ShoppingCart Checkout - Parking Transaction
        public ActionResult ParkShoppingCartItem(string id, string screenName, string Product)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ProductType prodtype = ProductType.None;
                Enum.TryParse(Product, out prodtype);
                ShoppingCartHelper.ParkShoppingCartTrx(long.Parse(id), prodtype, context);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return RedirectToAction("ShoppingCartCheckout", "ShoppingCart", new { IsException = true, ExceptionMessage = ViewBag.ExceptionMessage });
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
            Session["ShoppingCartCheckOutStatus"] = ZeoClient.HelperShoppingCartCheckoutStatus.CashOverCounter;
            return PartialView("_GPRWithdrawAmountPopup");
        }

        // GPR WithDraw - Cash Collected
        public ActionResult CollectCashFromCustomerPopup(string collectcash)
        {
            double collectCash = Convert.ToDouble(collectcash);
            ViewBag.collectcash = Math.Round(collectCash, 2);
            ViewBag.CashOverCounter = false;
            Session["ShoppingCartCheckOutStatus"] = ZeoClient.HelperShoppingCartCheckoutStatus.CashCollected;
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

        ////We are not using this Action method
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
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.Response shoppingCartResponse = alloyServiceClient.GetShoppingCart(context.CustomerSessionId, context);
                if (VerifyException(shoppingCartResponse)) return null;
                ZeoClient.ShoppingCart shoppingCart = shoppingCartResponse.Result as ZeoClient.ShoppingCart;

                //get moneyorder with processing state
                ZeoClient.MoneyOrder moneyOrder = shoppingCart.MoneyOrders.First(x => x.State == (int)TransactionStatus.Processing);

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
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
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
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.MoneyOrder moneyOrderProcessing = Session["MoneyOrder"] as ZeoClient.MoneyOrder;

            Models.MoneyOrderConfirm moneyOrderConfirm = new MoneyOrderConfirm();

            ZeoClient.MoneyOrder moneyOrderTransaction = new ZeoClient.MoneyOrder()
            {
                Id = moneyOrderProcessing.Id,
                CheckNumber = moneyOrderImage.CheckNumber,
                AccountNumber = moneyOrderImage.AccountNumber,
                RoutingNumber = moneyOrderImage.RoutingNumber,
                MICR = moneyOrderImage.MICR,
                FrontImage = string.IsNullOrEmpty(moneyOrderImage.FrontImage) ? null : Convert.FromBase64String(moneyOrderImage.FrontImage),
                BackImage = string.IsNullOrEmpty(moneyOrderImage.BackImage) ? null : Convert.FromBase64String(moneyOrderImage.BackImage)
            };

            ZeoClient.Response alloyResponse = alloyServiceClient.UpdateMoneyOrder(moneyOrderTransaction, context);
            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);

            if (moneyOrderProcessing != null)
            {
                try
                {
                    alloyResponse = alloyServiceClient.GenerateCheckPrintForMoneyOrder(moneyOrderProcessing.Id, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);

                    ZeoClient.MoneyOrderCheckPrint moneyOrderCheckPrint = alloyResponse.Result as ZeoClient.MoneyOrderCheckPrint;
                    moneyOrderConfirm.PrintData = ShoppingCartHelper.PrepareCheckForPrinting(moneyOrderCheckPrint.Lines);
                }
                catch (Exception ex)
                {
                    //If Check print template not generated then redirect to ScanAMoneyOrder view
                    ViewBag.IsMoneyOrderPrintException = true;
                    ViewBag.ExceptionMessage = ex.Message;
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
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.MoneyOrder moneyOrderProcessing = Session["MoneyOrder"] as ZeoClient.MoneyOrder;

                Models.MoneyOrderConfirm moneyOrderConfirm = new MoneyOrderConfirm();

                ZeoClient.MoneyOrder moneyOrderTransaction = new ZeoClient.MoneyOrder()
                {
                    Id = moneyOrderProcessing.Id,
                    CheckNumber = moneyOrderImage.CheckNumber,
                    AccountNumber = moneyOrderImage.AccountNumber,
                    RoutingNumber = moneyOrderImage.RoutingNumber,
                    MICR = moneyOrderImage.MICR,
                    FrontImage = string.IsNullOrEmpty(moneyOrderImage.FrontImage) ? null : Convert.FromBase64String(moneyOrderImage.FrontImage),
                    BackImage = string.IsNullOrEmpty(moneyOrderImage.BackImage) ? null : Convert.FromBase64String(moneyOrderImage.BackImage)
                };

                ZeoClient.Response response = alloyServiceClient.UpdateMoneyOrder(moneyOrderTransaction, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                return PartialView("_MoneyOrderPrintConfirm", moneyOrderImage);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        //Park Pending Check
        //private bool isMobileEnabledForTextMessage(CustomerSession currentCustomer)
        //{
        //    bool isEnabled = false;

        //    if (!string.IsNullOrEmpty(currentCustomer.Customer.Phone1.Number) && currentCustomer.Customer.Phone1.Type == "Cell" && currentCustomer.Customer.Preferences.SMSEnabled)
        //        isEnabled = true;
        //    else if (!string.IsNullOrEmpty(currentCustomer.Customer.Phone2.Number) && currentCustomer.Customer.Phone2.Type == "Cell" && currentCustomer.Customer.Preferences.SMSEnabled)
        //        isEnabled = true;

        //    return isEnabled;
        //}

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
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.MoneyOrder moneyOrderProcessing = Session["MoneyOrder"] as ZeoClient.MoneyOrder;

                if (moneyOrderProcessing != null)
                {
                    alloyServiceClient.UpdateMoneyOrderStatus(moneyOrderProcessing.Id, (int)TransactionStatus.Failed, context);
                }

                Session["ShoppingCartCheckOutStatus"] = ZeoClient.HelperShoppingCartCheckoutStatus.MOPrintingCancelled;

                return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        //Check Franking US1421 changes  //TODO: below code to be removed.
        //////private bool IsCheckFranking(MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        //////{
        //////    Desktop client = new Desktop();
        //////    Response response = client.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext);
        //////    if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
        //////    ChannelPartner channelpartner = response.Result as ChannelPartner;
        //////    return channelpartner.IsCheckFrank;
        //////}

        // Check Franking US1421 changes
        private string[] getCheckFrankCount(ZeoClient.ShoppingCart shoppingCart)
        {
            string[] checkdata = new string[shoppingCart.Checks.Count];
            int i = -1;

            foreach (ZeoClient.Check check in shoppingCart.Checks)
            {
                checkdata[++i] = check.Id.ToString();
            }

            return checkdata;
        }

        //Check Franking US1421 changes
        public ActionResult DisplayCheckFrankingDetails(string Id, long dt, int chkslno)
        {
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response getFrankDataResponse = new ZeoClient.Response();

                getFrankDataResponse = alloyClient.GetCheckFrankingData(long.Parse(Id), context);
                if (WebHelper.VerifyException(getFrankDataResponse)) throw new ZeoWebException(getFrankDataResponse.Error.Details);
                ZeoClient.CheckFrankingDetails checkFrankDetails = getFrankDataResponse.Result as ZeoClient.CheckFrankingDetails;

                CheckFrankingDetailsViewModel chkfrankdetails = new CheckFrankingDetailsViewModel()
                {
                    MICR = checkFrankDetails.MICR,
                    Amount = checkFrankDetails.Amount.ToString("0,00"),
                    FrankData = checkFrankDetails.FrankData,
                    chkSlno = chkslno,
                    DisplayMsg = string.Format(App_GlobalResources.Nexxo.CheckFrankingMessage, checkFrankDetails.Amount.ToString("0.00")),
                    TransactionID = Id
                };

                TempData["FrankText"] = System.Web.HttpUtility.HtmlEncode(checkFrankDetails.FrankData.Split('|')[0].Split(':')[1]);
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

        [HttpPost]
        public JsonResult UpdateCheckTransactionFranked(string transactionId)
        {
            ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();

            alloyClient.UpdateCheckTransactionFranked(long.Parse(transactionId), GetZeoContext());

            var jsonData = new { success = true };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        // Remove Item from ShoppingCart
        private bool RemoveShoppingCartTrx(long id, ProductType productType)
        {
            bool isItemRemoved = false;
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.Response response;
            try
            {
                switch (productType)
                {
                    case ProductType.Checks:
                        context = GetCheckLogin();
                        response = alloyServiceClient.RemoveCheck(id, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        isItemRemoved = true;
                        break;

                    case ProductType.BillPay:
                        GetCustomerSessionCounterId((int)Helper.ProviderId.WesternUnion, context);
                        response = alloyServiceClient.RemoveBillPay(id, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        isItemRemoved = true;
                        break;

                    case ProductType.ReceiveMoney:
                    case ProductType.SendMoney:
                        GetCustomerSessionCounterId((int)Helper.ProviderId.WesternUnion, context);
                        response = alloyServiceClient.RemoveMoneyTransfer(id, (int)productType, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        isItemRemoved = true;
                        break;

                    case ProductType.GPRActivation:
                        bool hasFundsAccount = false;
                        ZeoClient.FundsAccount fundsAccount = Session["ExistingGPRCardInfo"] as ZeoClient.FundsAccount;
                        if (fundsAccount != null)
                        {
                            hasFundsAccount = true;
                            response = alloyServiceClient.UpdateGPRAccount(fundsAccount, context);
                            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                            response = null;
                        }
                        removeFund(id, out isItemRemoved, alloyServiceClient, context, hasFundsAccount, out response);
                        break;
                    case ProductType.GPRLoad:
                    case ProductType.GPRWithdraw:
                    case ProductType.AddOnCard:
                        removeFund(id, out isItemRemoved, alloyServiceClient, context, false, out response);
                        break;

                    case ProductType.MoneyOrder:
                        response = alloyServiceClient.RemoveMoneyOrder(id, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        isItemRemoved = true;
                        break;

                    case ProductType.CashIn:
                        response = alloyServiceClient.RemoveCashIn(context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        isItemRemoved = true;
                        break;
                }
                return isItemRemoved;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void removeFund(long id, out bool isItemRemoved, ZeoClient.ZeoServiceClient alloyServiceClient, ZeoClient.ZeoContext context,bool hasFundsAccount, out ZeoClient.Response response)
        {
            response = alloyServiceClient.RemoveFund(id, hasFundsAccount, context); // This is from remove shopping cart 
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            isItemRemoved = true;
        }

        private void CheckCounterId(ZeoClient.ShoppingCart shoppingCart, ZeoClient.ZeoContext zeoContext)
        {
            if (shoppingCart.Bills.Count >= 1 || shoppingCart.MoneyTransfers.Count >= 1)
            {
                GetCustomerSessionCounterId((int)Helper.ProviderId.WesternUnion, zeoContext);
            }
        }

        // TCF - Post Flush
        public ActionResult PostFlushShoppingCart()
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response = new ZeoClient.Response();
                string result = string.Empty;
                try
                {
                    context.SSOAttributes = GetSSOAttributes("SSO_AGENT_SESSION");
                    decimal cardBalance = GetPrepaidCardBalance();
                    response = alloyServiceClient.PostFlush(cardBalance, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                }
                catch (Exception ex)
                {
                    result = Convert.ToString(ex.Message);
                }

                return Json(new { success = true, ErrorMsg = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        // TODO : below code has to be removed.
        //////////// Certegy related Changes
        //////////[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //////////public ActionResult ShowCertegyConfirmationPopup(string CheckStatus)
        //////////{
        //////////    return PartialView("_CertegyApprovedPopUp");
        //////////}

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
                success = true,
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
            if (Session["PendingChecks"] != null && bool.Parse(Convert.ToString(Session["PendingChecks"])) == true)
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
