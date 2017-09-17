using System;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
using WebCommon = TCF.Channel.Zeo.Web.Common;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class TransactionSummaryController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult DisplayDetails(string id, string status)
        {
            ZeoClient.ShoppingCart cart;
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            try
            {
                ZeoClient.Response cartResponse = alloyServiceClient.GetShoppingCart(context.CustomerSessionId, context);
                if (WebHelper.VerifyException(cartResponse)) throw new ZeoWebException(cartResponse.Error.Details);
                cart = cartResponse.Result as ZeoClient.ShoppingCart;

                if (cart.Checks.Count(c => c.Id == long.Parse(id)) > 0)
                {
                    try
                    {
                        // Display Check Details in pop-up    
                        context = GetCheckLogin();                      

                        ZeoClient.Response getCheckStatusResponse = alloyServiceClient.GetCheckStatus(Convert.ToInt64(id), context);
                        if (WebHelper.VerifyException(cartResponse)) throw new ZeoWebException(getCheckStatusResponse.Error.Details);
                        ZeoClient.Check checkStatus = getCheckStatusResponse.Result as ZeoClient.Check;
                        //This call is to get the image
                        ZeoClient.Response getTransactionResponse = alloyServiceClient.GetCheckTranasactionDetails(Convert.ToInt64(id), context);
                        if (WebHelper.VerifyException(getTransactionResponse)) throw new ZeoWebException(getTransactionResponse.Error.Details);
                        ZeoClient.CheckTransactionDetails checkTransactionDetails = getTransactionResponse.Result as ZeoClient.CheckTransactionDetails;

                        CheckTransaction checkTransaction = new CheckTransaction();
                        checkTransaction.AmountCredited = checkStatus.Amount;
                        checkTransaction.CheckCashingFee = checkStatus.BaseFee;
                        //AL-657 - 5.x-Regression-For failed check, Net to Customer should be Zero.
                        checkTransaction.NetAmount = (status == WebCommon.TransactionStatus.Failed.ToString("D")) ? (checkStatus.Amount - checkStatus.BaseFee) : 0;
                        checkTransaction.CheckFrontImage = Convert.ToBase64String(checkTransactionDetails.ImageFront);
                        checkTransaction.CheckId = checkStatus.Id.ToString();

                        if (!string.IsNullOrWhiteSpace(checkStatus.StatusDescription))
                        {
                            checkTransaction.CheckStatus = checkStatus.Status + " [" + checkStatus.StatusDescription + "]";
                        }
                        else
                        {
                            checkTransaction.CheckStatus = checkStatus.Status;
                        }

                        checkTransaction.StatusDescription = (status == WebCommon.TransactionStatus.Failed.ToString("D")) ? checkStatus.StatusDescription : null;

                        return PartialView("_TransSummary", checkTransaction);
                    }
                    catch
                    {
                        CheckTransaction checkTransaction = new Models.CheckTransaction();
                        checkTransaction.AmountCredited = 0;
                        checkTransaction.CheckCashingFee = 0;
                        checkTransaction.NetAmount = 0;
                        checkTransaction.CheckFrontImage = null;
                        return PartialView("_TransSummary", checkTransaction);
                    }
                }
                //Todo:Provider specific logic need to be implemented to show popup
                //else if (cart.Bills.Any())
                //{
                //    // Display Bill Details in pop--up
                //    Bill bill = cart.Bills.Find(c => c.BillId == id);
                //    Models.BillPayment billPayment = new Models.BillPayment();
                //    if (bill != null)
                //    {
                //        billPayment.SelectBillPayee = bill.BillerName;//.Substring(startIndex, length);
                //        billPayment.BillPaymentFee = bill.Fee;
                //        billPayment.BillAmount = bill.Amount;
                //        billPayment.AccountNumber = bill.AccountNumber;
                //        billPayment.ConfirmAccountNumber = bill.AccountNumber;
                //        billPayment.StatusDescription = (status == Constants.STATUS_FAILED) ? bill.StatusDescription : null;
                //    }
                //    else
                //    {
                //        billPayment.SelectBillPayee = "";
                //        billPayment.BillPaymentFee = 0;
                //        billPayment.BillAmount = 0;
                //        billPayment.AccountNumber = "";
                //        billPayment.ConfirmAccountNumber = "";
                //    }
                //    return PartialView("_BillPaymentDetails", billPayment);
                //}
                else if (cart.MoneyTransfers.Count(c => c.Id == long.Parse(id)) > 0)
                {
                    // Change this whole code to display pop-up for Money Transfer
                    try
                    {
                        ZeoClient.MoneyTransfer moneytransfer = cart.MoneyTransfers.Find(c => c.Id == long.Parse(id));
                        //Receiver receiver = desktop.GetReceiverDetails(moneytransfer.ReceiverId.ToString());
                        SendMoney moneyTransfer = new SendMoney();
                        moneyTransfer.CountryCode = moneytransfer.DestinationCountry;
                        moneyTransfer.ReceiverName = moneytransfer.ReceiverFirstName + " " + moneytransfer.ReceiverLastName;
                        moneyTransfer.PickUpLocation = moneytransfer.PickupLocation;
                        moneyTransfer.PickUpMethod = moneytransfer.PickupMethod;
                        moneyTransfer.PickUpOptions = moneytransfer.PickupOptions;
                        moneyTransfer.TransferAmount = moneytransfer.Amount;
                        moneyTransfer.TransferFee = moneytransfer.Fee;
                        //moneyTransfer.TransferTax = moneytransfer.TransferTax;
                        decimal transferTax = Convert.ToDecimal(Helper.GetDecimalDictionaryValueIfExists(moneytransfer.MetaData, "TransferTax"));
                        moneyTransfer.TotalAmount = moneytransfer.Amount + transferTax + moneytransfer.Fee;
                        moneyTransfer.ExchangeRate = moneytransfer.ExchangeRate;
                        moneyTransfer.OtherFees = moneytransfer.OtherFee;
                        //moneyTransfer.OtherTaxes = moneytransfer.OtherTax;
                        moneyTransfer.TotalToRecipient = moneytransfer.Amount;
                        moneyTransfer.StatusDescription = (status == WebCommon.TransactionStatus.Failed.ToString("D")) ? cart.MoneyTransfers.Find(c => c.Id == long.Parse(id)).StatusDescription : null;

                        return PartialView("_MoneyTransferDetails", moneyTransfer);
                    }
                    catch
                    {
                        SendMoney moneyTransfer = new SendMoney();
                        moneyTransfer.ReceiverName = "";
                        moneyTransfer.PickUpLocation = "";
                        moneyTransfer.PickUpMethod = "";
                        moneyTransfer.PickUpOptions = "";
                        moneyTransfer.TransferAmount = 0;
                        moneyTransfer.TransferFee = 0;
                        //moneyTransfer.TransferTax = 0;
                        moneyTransfer.TotalAmount = 0;
                        moneyTransfer.ExchangeRate = 1;
                        moneyTransfer.TransferAmount = 0;
                        moneyTransfer.OtherFees = 0;
                        //moneyTransfer.OtherTaxes = 0;
                        moneyTransfer.TotalToRecipient = cart.MoneyTransfers.Find(c => c.Id == long.Parse(id)).MoneyTransferTotal * moneyTransfer.ExchangeRate;

                        return PartialView("_MoneyTransferDetails", moneyTransfer);
                    }
                }
                else if (cart.GPRCards.Count(c => c.Id == id) > 0)
                {
                    // Display GPRCard Details in pop--up
                    try
                    {
                        PrePaidCard prepaidCard = new PrePaidCard();
                        BaseModel baseModel = new BaseModel();
                        ZeoClient.GPRCard gprCard = cart.GPRCards.Where(c => c.Id == id).FirstOrDefault();

                        prepaidCard.Name = baseModel.CustomerSession.Customer.FirstName + " " + baseModel.CustomerSession.Customer.LastName;
                        prepaidCard.CardNumber = gprCard.CardNumber == null ? string.Empty : "**** **** **** " + gprCard.CardNumber; //gprCard.CardNumber;
                        prepaidCard.AccountNumber = gprCard.AccountNumber == null ? string.Empty : gprCard.AccountNumber; //customerSession.Customer.Profile.AccountNumber;
                        prepaidCard.StatusDescription = (status == WebCommon.TransactionStatus.Failed.ToString("D")) ? gprCard.StatusDescription : null;
                        if (gprCard.ItemType == ZeoClient.HelperFundType.Activation.ToString())
                        {
                            prepaidCard.TransactionType = "Activation";
                            prepaidCard.ActivationFee = gprCard.ActivationFee;
                            prepaidCard.LoadBalanceImpact = 0;
                            prepaidCard.LoadAmount = 0;
                            prepaidCard.LoadFee = 0;
                            prepaidCard.WithdrawAmount = 0;
                            prepaidCard.WithdrawFee = 0;
                            prepaidCard.WithdrawBalanceImpact = 0;
                        }
                        else if (gprCard.ItemType == ZeoClient.HelperFundType.Credit.ToString())
                        {
                            prepaidCard.TransactionType = "Load";
                            prepaidCard.ActivationFee = 0;

                            prepaidCard.LoadAmount = gprCard.LoadAmount;
                            prepaidCard.LoadFee = gprCard.LoadFee;
                            prepaidCard.WithdrawAmount = 0;
                            prepaidCard.WithdrawFee = 0;
                            prepaidCard.WithdrawBalanceImpact = 0;
                            prepaidCard.LoadBalanceImpact = prepaidCard.LoadAmount - prepaidCard.LoadFee;
                        }
                        else if (gprCard.ItemType == ZeoClient.HelperFundType.Debit.ToString())
                        {
                            prepaidCard.TransactionType = "Withdraw";
                            prepaidCard.ActivationFee = 0;
                            prepaidCard.LoadBalanceImpact = 0;
                            prepaidCard.LoadAmount = 0;
                            prepaidCard.LoadFee = 0;
                            prepaidCard.WithdrawAmount = gprCard.WithdrawAmount;
                            prepaidCard.WithdrawFee = gprCard.WithdrawFee;
                            prepaidCard.LoadBalanceImpact = 0;
                            prepaidCard.WithdrawBalanceImpact = prepaidCard.WithdrawAmount + prepaidCard.WithdrawFee;
                        }
                        return PartialView("_PrepaidCardDetailsPopup", prepaidCard);
                    }
                    catch
                    {
                        PrePaidCard prepaidCard = new PrePaidCard();
                        prepaidCard.Name = "";
                        prepaidCard.CardNumber = "";
                        prepaidCard.AccountNumber = "";
                        prepaidCard.TransactionType = "";
                        prepaidCard.ActivationFee = 0;
                        prepaidCard.LoadBalanceImpact = 0;
                        prepaidCard.LoadAmount = 0;
                        prepaidCard.LoadFee = 0;
                        prepaidCard.WithdrawAmount = 0;
                        prepaidCard.WithdrawFee = 0;
                        prepaidCard.StatusDescription = "";
                        return PartialView("_PrepaidCardDetailsPopup", prepaidCard);
                    }
                }
                else if (cart.MoneyOrders.Any())
                {
                    ZeoClient.MoneyOrder moneyOrder = cart.MoneyOrders.Find(c => c.Id == long.Parse(id));
                    Models.MoneyOrderSetup moneyOrderSetup = new Models.MoneyOrderSetup();
                    if (moneyOrder != null)
                    {
                        moneyOrderSetup.Amount = moneyOrder.Amount;
                        moneyOrderSetup.Fee = moneyOrder.Fee;
                        moneyOrderSetup.Total = moneyOrder.Amount + moneyOrder.Fee;
                        moneyOrderSetup.StatusDescription = (status == WebCommon.TransactionStatus.Authorized.ToString("D")) ? "Approved" :
                            (status == WebCommon.TransactionStatus.Committed.ToString("D")) ? "Committed" :
                            (status == WebCommon.TransactionStatus.Cancelled.ToString("D")) ? "Removed" :
                            (status == WebCommon.TransactionStatus.Failed.ToString("D")) ? "Failed" : "Initiated";
                    }
                    else
                    {
                        moneyOrderSetup.Amount = 0;
                        moneyOrderSetup.Fee = 0;
                        moneyOrderSetup.Total = 0;
                    }
                    return PartialView("_MoneyOrderDetailsPopup", moneyOrderSetup);
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }

            return RedirectToAction("ProductInformation", "Product");

        }
    }
}