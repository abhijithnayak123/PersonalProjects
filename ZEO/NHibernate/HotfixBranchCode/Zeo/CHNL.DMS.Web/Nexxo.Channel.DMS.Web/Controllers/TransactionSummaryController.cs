using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Server.Data;
using System.Collections;
using MGI.Channel.DMS.Web.Common;
using System.ServiceModel;
using MGI.Channel.DMS.Web.ServiceClient;
using System.Reflection;
using System.Linq.Expressions;
using System.Globalization;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Web.Controllers
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
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			long agentSessionId = GetAgentSessionId();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop desktop = new Desktop();
			ShoppingCart cart;

			try
			{
				cart = desktop.ShoppingCart(customerSession.CustomerSessionId);

				if (cart.Checks.Count(c => c.Id == id) > 0)
				{
					try
					{
						// Display Check Details in pop-up    
						mgiContext = GetCheckLogin(Convert.ToInt64(customerSession.CustomerSessionId));
						Check checkStatus = desktop.GetCheckStatus(customerSession.CustomerSessionId, id.ToString(), mgiContext);
						//This call is to get the image
						CheckTransactionDetails checkTransactionDetails = desktop.GetCheckTransaction(agentSessionId, long.Parse(customerSession.CustomerSessionId), Convert.ToInt64(id), mgiContext);

						MGI.Channel.DMS.Web.Models.CheckTransaction checkTransaction = new MGI.Channel.DMS.Web.Models.CheckTransaction();
						checkTransaction.AmountCredited = checkStatus.Amount;
						checkTransaction.CheckCashingFee = checkStatus.Fee;
						//AL-657 - 5.x-Regression-For failed check, Net to Customer should be Zero.
						checkTransaction.NetAmount = (status == Constants.STATUS_FAILED) ? 0 : (checkStatus.Amount - checkStatus.Fee);
						checkTransaction.CheckFrontImage = Convert.ToBase64String(checkTransactionDetails.ImageFront);
						checkTransaction.CheckId = checkStatus.Id;

						if (checkStatus.StatusDescription != null)
						{
							checkTransaction.CheckStatus = checkStatus.Status + " [" + checkStatus.StatusDescription + "]";
						}
						else
						{
							checkTransaction.CheckStatus = checkStatus.Status;
						}

						checkTransaction.StatusDescription = (status == Constants.STATUS_FAILED) ? checkStatus.StatusDescription : null;

						return PartialView("_TransSummary", checkTransaction);
					}
					catch
					{
						MGI.Channel.DMS.Web.Models.CheckTransaction checkTransaction = new Models.CheckTransaction();
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
				else if (cart.MoneyTransfers.Count(c => c.Id == id) > 0)
				{
					// Change this whole code to display pop-up for Money Transfer
					try
					{
						MoneyTransfer moneytransfer = cart.MoneyTransfers.Find(c => c.Id == id);
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
						decimal transferTax = Convert.ToDecimal(NexxoUtil.GetDecimalDictionaryValueIfExists(moneytransfer.MetaData, "TransferTax"));
						moneyTransfer.TotalAmount = moneytransfer.Amount + transferTax + moneytransfer.Fee;
						moneyTransfer.ExchangeRate = moneytransfer.ExchangeRate;
						moneyTransfer.OtherFees = moneytransfer.OtherFee;
						//moneyTransfer.OtherTaxes = moneytransfer.OtherTax;
						moneyTransfer.TotalToRecipient = moneytransfer.Amount;
						moneyTransfer.StatusDescription = (status == Constants.STATUS_FAILED) ? cart.MoneyTransfers.Find(c => c.Id == id).StatusDescription : null;

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
						moneyTransfer.TotalToRecipient = cart.MoneyTransfers.Find(c => c.Id == id).MoneyTransferTotal * moneyTransfer.ExchangeRate;

						return PartialView("_MoneyTransferDetails", moneyTransfer);
					}
				}
				else if (cart.GprCards.Count(c => c.Id == id) > 0)
				{
					// Display GPRCard Details in pop--up
					try
					{
						PrePaidCard prepaidCard = new PrePaidCard();

						GprCard gprCard = cart.GprCards.Where(c => c.Id == id).FirstOrDefault();

						prepaidCard.Name = customerSession.Customer.PersonalInformation.FName + " " + customerSession.Customer.PersonalInformation.LName;
						prepaidCard.CardNumber = gprCard.CardNumber == null ? string.Empty : "**** **** **** " + gprCard.CardNumber; //gprCard.CardNumber;
						prepaidCard.AccountNumber = gprCard.AccountNumber == null ? string.Empty : gprCard.AccountNumber; //customerSession.Customer.Profile.AccountNumber;
						prepaidCard.StatusDescription = (status == Constants.STATUS_FAILED) ? gprCard.StatusDescription : null;
						if (gprCard.ItemType == Constants.PREPAID_CARD_ACTIVATE)
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
						else if (gprCard.ItemType == Constants.PREPAID_CARD_LOAD)
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
						else if (gprCard.ItemType == Constants.PREPAID_CARD_WITHDRAW)
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
					MoneyOrder moneyOrder = cart.MoneyOrders.Find(c => c.Id == id);
					Models.MoneyOrderSetup moneyOrderSetup = new Models.MoneyOrderSetup();
					if (moneyOrder != null)
					{
						moneyOrderSetup.Amount = moneyOrder.Amount;
						moneyOrderSetup.Fee = moneyOrder.Fee;
						moneyOrderSetup.Total = moneyOrder.Amount + moneyOrder.Fee;
						moneyOrderSetup.StatusDescription = (status == Constants.STATUS_AUTHORIZED) ? "Approved" :
							(status == Constants.STATUS_COMMITTED) ? "Committed" :
							(status == Constants.STATUS_CANCELED) ? "Removed" :
							(status == Constants.STATUS_FAILED) ? "Failed" : "Initiated";
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
				ViewBag.ExceptionMessage = ex.Message;
			}

			return RedirectToAction("ProductInformation", "Product");

		}
	}
}