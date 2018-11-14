using TCF.Channel.Zeo.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Common
{
    public static class ShoppingCartHelper
    {

        #region Shoppingcart Summary

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        public static ShoppingCartSummary ShoppingCartSummary(ZeoClient.ShoppingCart shoppingCart)
        {
            ShoppingCartSummary shoppingCartSummary = new ShoppingCartSummary();

            //Add transaction summary
            AddCheckSummary(shoppingCartSummary, shoppingCart.Checks);
            AddBillSummary(shoppingCartSummary, shoppingCart.Bills);
            AddMoneyTransferSummary(shoppingCartSummary, shoppingCart.MoneyTransfers);
            AddFundSummary(shoppingCartSummary, shoppingCart.GPRCards);
            AddMoneyOrderSummary(shoppingCartSummary, shoppingCart.MoneyOrders);
            return shoppingCartSummary;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartSummary"></param>
        /// <param name="checks"></param>
        private static void AddCheckSummary(ShoppingCartSummary shoppingCartSummary, List<ZeoClient.Check> checks)
        {
            //Approved check transactions summary
            shoppingCartSummary.CheckSummary(checks, TransactionStatus.Authorized.ToString("D"));
            //Pending check transactions summary
            shoppingCartSummary.CheckSummary(checks, TransactionStatus.Pending.ToString("D"));
            //Failed check transactions summary. Do we need the failed transaction here???
            shoppingCartSummary.CheckSummary(checks, TransactionStatus.Failed.ToString("D"));
            //Declined check transactions summary. Do we need the declined transaction here???
            shoppingCartSummary.CheckSummary(checks, TransactionStatus.Declined.ToString("D"));
            //Failed check transactions summary. Do we need the failed transaction here???
            shoppingCartSummary.CheckSummary(checks, TransactionStatus.Authorization_Failed.ToString("D").ToString());
            //committed check
            shoppingCartSummary.CheckSummary(checks, TransactionStatus.Committed.ToString("D"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartSummary"></param>
        /// <param name="bills"></param>
        private static void AddBillSummary(ShoppingCartSummary shoppingCartSummary, List<ZeoClient.BillPayTransaction> bills)
        {
            //Validated Bill Pay transactions summary
            shoppingCartSummary.BillSummary(bills, TransactionStatus.Authorized.ToString("D"));
            //Failed Bill Pay transactions summary
            shoppingCartSummary.BillSummary(bills, TransactionStatus.Failed.ToString("D"));
            //Succeeded Money Transfer transactions summary
            shoppingCartSummary.BillSummary(bills, TransactionStatus.Committed.ToString("D"));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartSummary"></param>
        /// <param name="moneyTransfers"></param>
        private static void AddMoneyTransferSummary(ShoppingCartSummary shoppingCartSummary, List<ZeoClient.MoneyTransfer> moneyTransfers)
        {
            //Validated Money transfer transactions summary
            shoppingCartSummary.MoneyTransferSummary(moneyTransfers, TransactionStatus.Authorized.ToString("D"));
            //Failed Money transfer transactions summary
            shoppingCartSummary.MoneyTransferSummary(moneyTransfers, TransactionStatus.Failed.ToString("D"));
            //Succeeded Money transfer transactions summary
            shoppingCartSummary.MoneyTransferSummary(moneyTransfers, TransactionStatus.Committed.ToString("D"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartSummary"></param>
        /// <param name="funds"></param>
        private static void AddFundSummary(ShoppingCartSummary shoppingCartSummary, List<ZeoClient.GPRCard> funds)
        {
            // Validated Fund transactions summary
            shoppingCartSummary.FundsSummary(funds, TransactionStatus.Authorized.ToString("D"));
            // Failed Funds transactions summary
            shoppingCartSummary.FundsSummary(funds, TransactionStatus.Failed.ToString("D"));
            //Succeeded Funds transactions summary
            shoppingCartSummary.FundsSummary(funds, TransactionStatus.Committed.ToString("D"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartSummary"></param>
        /// <param name="list"></param>
        private static void AddMoneyOrderSummary(ShoppingCartSummary shoppingCartSummary, List<ZeoClient.MoneyOrder> moneyOrders)
        {
            //Pending MoneyOrder transactions summary
            shoppingCartSummary.MoneyOrderSummary(moneyOrders, (int)TransactionStatus.Pending);
            //Approved MoneyOrder transactions summary
            shoppingCartSummary.MoneyOrderSummary(moneyOrders, (int)TransactionStatus.Authorized);
            //Processing MoneyOrder transactions summary
            shoppingCartSummary.MoneyOrderSummary(moneyOrders, (int)TransactionStatus.Processing);
            //Succeeded MoneyOrder transactions summary
            shoppingCartSummary.MoneyOrderSummary(moneyOrders, (int)TransactionStatus.Committed);
            //Cancelled MoneyOrder transactions summary
            shoppingCartSummary.MoneyOrderSummary(moneyOrders, (int)TransactionStatus.Cancelled);
            //Failed MoneyOrder transactions summary
            shoppingCartSummary.MoneyOrderSummary(moneyOrders, (int)TransactionStatus.Failed);
        }
        #endregion Shoppingcart Summary.

        #region Shoppingcart Detail
        public static ShoppingCartDetail ShoppingCartDetailed(ZeoClient.ShoppingCart shoppingCart)
        {
            ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();

            //Add transaction details.
            shoppingCartDetail.CheckDetail(shoppingCart.Checks);
            shoppingCartDetail.BillDetail(shoppingCart.Bills);
            shoppingCartDetail.MoneyTransferDetail(shoppingCart.MoneyTransfers);
            shoppingCartDetail.FundsDetail(shoppingCart.GPRCards);
            shoppingCartDetail.CashDetail(shoppingCart.Cash);
            shoppingCartDetail.MoneyOrderDetail(shoppingCart.MoneyOrders);

            //Calculate Funds generated and depleted and other sub totals
            CalculateSubTotal(shoppingCartDetail);

            //Set the shopping cart Id if available
            shoppingCartDetail.Id = shoppingCart.Id;

            shoppingCartDetail.IsReferral = shoppingCart.IsReferral;
            shoppingCartDetail.IsReferalSectionEnable = shoppingCart.IsReferralSectionEnabled;
            shoppingCartDetail.LoadFee = shoppingCartDetail.WithdrawFee = 0M;
            shoppingCartDetail.CardBalance = GetPrepaidCardBalance();
            //Finaly return the detailed shoppingcart
            return shoppingCartDetail;
        }

        private static decimal GetPrepaidCardBalance()
        {
             return (HttpContext.Current.Session["CardBalance"] as ZeoClient.CardBalanceInfo) != null
                                             ? (HttpContext.Current.Session["CardBalance"] as ZeoClient.CardBalanceInfo).Balance : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartDetail"></param>
        /// <returns></returns>

        private static void CalculateSubTotal(ShoppingCartDetail detailedShoppingCart)
        {
            decimal generatedAmount = 0.0m;
            decimal depletingAmount = 0.0m;
            decimal generatedtotal = 0.0m;
            decimal depletingtotal = 0.0m;
            decimal initialLoad = 0.0m;

            decimal depletingFee = 0.0m;
            decimal generatedFee = 0.0m;
            decimal activationfee = 0.0m;

            decimal cashcollected = 0;

            //Cash collected prior to current checkout, ie cash collected in checkout failure
            cashcollected = detailedShoppingCart.Items.Where(x => x.Product == ProductType.CashIn.ToString()).Sum(x => x.NetAmount);
            detailedShoppingCart.PreviousCashCollected = cashcollected;

            List<ShoppingCartItem> activenongprShoppingCartItems = detailedShoppingCart.Items.Where(x => !x.Product.StartsWith("GPR")).ToList<ShoppingCartItem>();

            List<ShoppingCartItem> activegprShoppingCartItems = detailedShoppingCart.Items.Where(x => x.Product.StartsWith("GPR")).ToList<ShoppingCartItem>();

            getConsolidatedAmounts(activenongprShoppingCartItems, Helper.FundType.Credit, out generatedtotal, out generatedAmount, out generatedFee);

            detailedShoppingCart.GeneratedTotal = generatedtotal;
            detailedShoppingCart.GeneratedAmount = generatedAmount;
            detailedShoppingCart.GeneratedFee = generatedFee;

            getConsolidatedAmounts(activenongprShoppingCartItems, Helper.FundType.Debit, out depletingtotal, out depletingAmount, out depletingFee);

            detailedShoppingCart.DepletedTotal = depletingtotal;
            detailedShoppingCart.DepletedAmount = depletingAmount;
            detailedShoppingCart.DepletedFee = depletingFee;


            detailedShoppingCart.LoadToCard = activegprShoppingCartItems.Where(x => x.TxnType == Helper.FundType.Credit.ToString()
                && x.Product != "GPRActivation").Sum(x => x.Amount);

            //US1114 - Changes to set the value to withdraw from card. 		
            //detailedShoppingCart.LoadToCard = detailedShoppingCart.LoadToCard - activegprShoppingCartItems.Where(x => x.TxnType == FundType.Debit.ToString()).Sum(x => x.Amount);			
            detailedShoppingCart.WithdrawFromCard = activegprShoppingCartItems.Where(x => x.TxnType == Helper.FundType.Debit.ToString()).Sum(x => x.Amount);

            // Lets handle activation fee as depleted fee. As the activation will be seperate line item in shopping cart detail,
            // get the product based on ProductType = GPRActivation and Sum the fee for the same.
            activationfee = activegprShoppingCartItems.Where(x => x.Product == ProductType.GPRActivation.ToString()).Sum(x => x.Fee);

            detailedShoppingCart.DepletedFee += activationfee;

            initialLoad = activegprShoppingCartItems.Where(x => x.Product == ProductType.GPRActivation.ToString()).Sum(x => x.Amount);
            // since the activation fee is also shown as seperate funds depleting txn, it should also be added to depleted total.
            detailedShoppingCart.DepletedTotal += initialLoad;
            detailedShoppingCart.DepletedTotal += activationfee;

            detailedShoppingCart.TotalFee = detailedShoppingCart.GeneratedFee + detailedShoppingCart.DepletedFee;

            detailedShoppingCart.DueToCustomer = detailedShoppingCart.GeneratedTotal - detailedShoppingCart.DepletedTotal;

            //US1114 - Changes to consider withdraw from card.

            detailedShoppingCart.CashToCustomer = cashcollected + detailedShoppingCart.DueToCustomer - detailedShoppingCart.LoadToCard + detailedShoppingCart.WithdrawFromCard;
        }

        private static void getConsolidatedAmounts(List<ShoppingCartItem> activeShoppingCartItems, Helper.FundType fundType, out decimal netAmount, out decimal amount, out decimal fee)
        {
            netAmount = amount = fee = 0.0m;

            netAmount = activeShoppingCartItems.Where(x => x.TxnType == fundType.ToString()).Sum(x => x.NetAmount);

            amount = activeShoppingCartItems.Where(x => x.TxnType == fundType.ToString()).Sum(x => x.Amount);

            fee = activeShoppingCartItems.Where(x => x.TxnType == fundType.ToString()).Sum(x => x.Fee);
        }

        #endregion

        #region Other

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartDetail"></param>
        public static void UpdateFundsTransaction(ShoppingCartDetail shoppingCartDetail, ZeoClient.ShoppingCart cart, ZeoClient.ZeoContext context)
        {

            ZeoClient.ZeoServiceClient alloyService = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response alloyResponse = new ZeoClient.Response();
            decimal loadAmount = 0m;
            decimal withdrawAmount = 0m;
            long loadTxnId;
            long withdrawTxnId;

            //AL-2729 user story for updating the cash-in transaction
            if (shoppingCartDetail.CashCollected > 0 && HttpContext.Current.Session["ShoppingCartCheckOutStatus"].ToString() == Helper.ShoppingCartCheckoutStatus.CashCollected.ToString())
            {
                alloyResponse = alloyService.CashIn(shoppingCartDetail.CashCollected, context);
                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
            }
            else if (HttpContext.Current.Session["ShoppingCartCheckOutStatus"].ToString() != Helper.ShoppingCartCheckoutStatus.MOPrinting.ToString() && HttpContext.Current.Session["ShoppingCartCheckOutStatus"].ToString() != ZeoClient.HelperShoppingCartCheckoutStatus.CashOverCounter.ToString())
            {
                List<ZeoClient.CartCash> cashInTrans = new List<ZeoClient.CartCash>();

                if (cart != null && cart.Cash != null)
                    cashInTrans = cart.Cash.Where(x => x.CashType == ZeoClient.HelperCashType.CashIn).ToList();

                if (cashInTrans.Count() > 0)
                {
                    alloyResponse = alloyService.UpdateOrCancelCashIn(shoppingCartDetail.CashCollected, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                    // TODO: Below code has to be removed once testing completed.
                    ////if (shoppingCartDetail.CashCollected == 0)
                    ////{
                    ////	foreach (ZeoClient.CartCash cashTran in cashInTrans)
                    ////	{
                    ////		desktop.UpdateCash(customerSessionId, long.Parse(cashTran.Id), shoppingCartDetail.CashCollected, mgiContext);
                    ////		desktop.CancelCashIn(customerSessionId, long.Parse(cashTran.Id));
                    ////	}
                    ////}
                    ////else if (shoppingCartDetail.CashCollected > 0)
                    ////{
                    ////	if (cashInTrans.Count() > 1)
                    ////	{
                    ////		desktop.UpdateCash(customerSessionId, long.Parse(cashInTrans.LastOrDefault().Id), 0, mgiContext);
                    ////		desktop.CancelCashIn(customerSessionId, long.Parse(cashInTrans.LastOrDefault().Id));
                    ////	}
                    ////	desktop.UpdateCash(customerSessionId, long.Parse(cashInTrans.FirstOrDefault().Id), shoppingCartDetail.CashCollected, mgiContext);
                    ////}

                }
                else if (shoppingCartDetail.CashCollected > 0)
                    alloyResponse = alloyService.CashIn(shoppingCartDetail.CashCollected, context);
                    if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);

            }

            if (shoppingCartDetail.CardHolder || cart.GPRCards.Where(x => x.ItemType == Helper.FundType.Activation.ToString()).Select(x => x.TransactionId).Count() > 0)
            {

                decimal loadFee = 0;
                decimal withdrawFee = 0;

                //US1115 - TA3600 - Start
                if (shoppingCartDetail.LoadToCard == 0 && shoppingCartDetail.WithdrawFromCard == 0 && cart.GPRCards != null)
                {
                    loadAmount = cart.GPRCards.Where(x => x.ItemType == Helper.FundType.Credit.ToString()).Sum(y => y.LoadAmount);
                    withdrawAmount = cart.GPRCards.Where(x => x.ItemType == Helper.FundType.Debit.ToString()).Sum(y => y.WithdrawAmount);

                    if (loadAmount > 0)
                    {
                        loadTxnId = cart.GPRCards.Where(x => x.ItemType == Helper.FundType.Credit.ToString() && x.Status != TransactionStatus.Committed.ToString("D")).Select(x => x.TransactionId).FirstOrDefault();
                        if (loadTxnId != 0)
                        {
                            alloyResponse = alloyService.RemoveFund(loadTxnId, false, context); // The changes here to false is unneccssary, 
                            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                        }

                    }
                    else if (withdrawAmount > 0)
                    {
                        withdrawTxnId = cart.GPRCards.Where(x => x.ItemType == Helper.FundType.Debit.ToString() && x.Status != TransactionStatus.Committed.ToString("D")).Select(x => x.TransactionId).FirstOrDefault();
                        if (withdrawTxnId != 0)
                        {
                            alloyResponse = alloyService.RemoveFund(withdrawTxnId, false, context);
                            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                        }
                    }
                }//US1115 - TA3600 - End
                else
                {
                    if (shoppingCartDetail.WithdrawFromCard > 0)
                    {
                        ZeoClient.Funds funds = new ZeoClient.Funds()
                        {
                            Amount = Math.Abs(shoppingCartDetail.WithdrawFromCard),
                            Fee = withdrawFee
                        };
                        /////////// When tested for AL-514 in TCF while do not proceed message more than once coming withdraw happening twice,so added the below code ///////////////
                        long withdrawTransactionId = 0;
                        if (cart.GPRCards != null)
                            withdrawTransactionId = cart.GPRCards.Where(x => x.ItemType == Helper.FundType.Debit.ToString() && x.Status != TransactionStatus.Committed.ToString("D")).Select(x => x.TransactionId).FirstOrDefault();
                        if (withdrawTransactionId != 0 && shoppingCartDetail.WithdrawFromCard != cart.GPRCards.Where(x => x.TransactionId == withdrawTransactionId).FirstOrDefault().LoadAmount)
                        {
                            alloyResponse = alloyService.UpdateFundAmount(withdrawTransactionId, funds.Amount, ZeoClient.HelperFundType.Debit, context);
                            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                        }
                        else if ((withdrawTransactionId == 0) && shoppingCartDetail.WithdrawFromCard != 0)
                        {
                            alloyResponse = alloyService.WithdrawFunds(funds, context);
                            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                        }
                        ////////////////////////////////////////////
                    }
                    else if (shoppingCartDetail.LoadToCard > 0)
                    {
                        ZeoClient.Funds funds = new ZeoClient.Funds()
                        {
                            Amount = Math.Abs(shoppingCartDetail.LoadToCard),
                            Fee = loadFee
                        };

                        long loadTransactionId = 0;

                        if (cart.GPRCards != null)
                            loadTransactionId = cart.GPRCards.Where(x => x.ItemType == ZeoClient.HelperFundType.Credit.ToString() && x.Status != TransactionStatus.Committed.ToString("D")).Select(x => x.TransactionId).FirstOrDefault();

                        if (loadTransactionId != 0 && shoppingCartDetail.LoadToCard != cart.GPRCards.Where(x => x.TransactionId == loadTransactionId).FirstOrDefault().LoadAmount)
                        {
                            alloyResponse = alloyService.UpdateFundAmount(loadTransactionId, funds.Amount, ZeoClient.HelperFundType.Credit, context);
                            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                        }
                        else if ((loadTransactionId == 0) && shoppingCartDetail.LoadToCard != 0)
                        {
                            alloyResponse = alloyService.LoadFunds(funds, context);
                            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountIdentifier"></param>
        /// <returns></returns>
        public static string getAcctLast4Digits(string accountIdentifier)
        {
            string lastfourdigits = string.Empty;
            if (!string.IsNullOrEmpty(accountIdentifier))
                lastfourdigits = accountIdentifier.Length >= 4 ? accountIdentifier.Substring(accountIdentifier.Length - 4) : accountIdentifier;

            return lastfourdigits;
        }

        
        public static string GetReceiptsType(ZeoClient.Receipts Receipts)
        {
            return string.Join("~~", Receipts.receiptType.ToArray());
        }

        #region MOPRINT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moneyOrder"></param>
        /// <returns></returns>
        public static string PrepareCheckForPrinting(List<string> checkLines)
        {
            string str = "";
            if (checkLines != null)
            {
                if (checkLines != null)
                {
                    checkLines = Common.FileUtility.ConvertCheckPrintImagesToBase64(checkLines.ToArray()).ToList<string>();
                    foreach (string eachLine in checkLines)
                    {
                        str += eachLine + '\t';
                    }
                    byte[] byteStr = System.Text.Encoding.UTF8.GetBytes(str);
                    String base64Str = Convert.ToBase64String(byteStr);
                    return base64Str;
                }
            }
            return string.Empty;
        }

        #endregion

        #endregion

        #region ParkTransaction
        public static void ParkShoppingCartTrx(long transactionId, ProductType productType, ZeoClient.ZeoContext context, long custSessionId = 0)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response response = null;
            try
            {
                long customerSessionId = custSessionId == 0 ? context.CustomerSessionId : custSessionId;

                response = alloyServiceClient.ParkShoppingCartTransaction(customerSessionId, transactionId, (int)productType, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
