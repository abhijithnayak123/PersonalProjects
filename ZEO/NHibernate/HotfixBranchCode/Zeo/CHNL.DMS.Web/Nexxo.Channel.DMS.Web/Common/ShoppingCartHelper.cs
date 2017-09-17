using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Funds = MGI.Channel.Shared.Server.Data.Funds;
using FundType = MGI.Channel.Shared.Server.Data.FundType;
using Receipt = MGI.Channel.Shared.Server.Data.Receipt;
using SharedData = MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Common
{
	public static class ShoppingCartHelper
	{
		#region Shoppingcart Summary

		/// <summary>
		/// 
		/// </summary>
		/// <param name="shoppingCart"></param>
		/// <returns></returns>
		public static ShoppingCartSummary ShoppingCartSummary(ShoppingCart shoppingCart)
		{
			ShoppingCartSummary shoppingCartSummary = new ShoppingCartSummary();

			//Add transaction summary
			AddCheckSummary(shoppingCartSummary, shoppingCart.Checks);
			AddBillSummary(shoppingCartSummary, shoppingCart.Bills);
			AddMoneyTransferSummary(shoppingCartSummary, shoppingCart.MoneyTransfers);
			AddFundSummary(shoppingCartSummary, shoppingCart.GprCards);
			AddMoneyOrderSummary(shoppingCartSummary, shoppingCart.MoneyOrders);
			return shoppingCartSummary;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="shoppingCartSummary"></param>
		/// <param name="checks"></param>
		private static void AddCheckSummary(ShoppingCartSummary shoppingCartSummary, List<Check> checks)
		{
			//Approved check transactions summary
			shoppingCartSummary.CheckSummary(checks, Constants.STATUS_AUTHORIZED);
			//Pending check transactions summary
			shoppingCartSummary.CheckSummary(checks, Constants.STATUS_PENDING);
			//Failed check transactions summary. Do we need the failed transaction here???
			shoppingCartSummary.CheckSummary(checks, Constants.STATUS_FAILED);
			//Declined check transactions summary. Do we need the declined transaction here???
			shoppingCartSummary.CheckSummary(checks, Constants.STATUS_DECLINED);
			//Failed check transactions summary. Do we need the failed transaction here???
			shoppingCartSummary.CheckSummary(checks, Constants.STATUS_AUTHORIZATION_FAILED);
			//committed check
			shoppingCartSummary.CheckSummary(checks, Constants.STATUS_COMMITTED);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="shoppingCartSummary"></param>
		/// <param name="bills"></param>
		private static void AddBillSummary(ShoppingCartSummary shoppingCartSummary, List<Bill> bills)
		{
			//Validated Bill Pay transactions summary
			shoppingCartSummary.BillSummary(bills, Constants.STATUS_AUTHORIZED);
			//Failed Bill Pay transactions summary
			shoppingCartSummary.BillSummary(bills, Constants.STATUS_FAILED);
			//Succeeded Money Transfer transactions summary
			shoppingCartSummary.BillSummary(bills, Constants.STATUS_COMMITTED);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="shoppingCartSummary"></param>
		/// <param name="moneyTransfers"></param>
		private static void AddMoneyTransferSummary(ShoppingCartSummary shoppingCartSummary, List<MoneyTransfer> moneyTransfers)
		{
			//Validated Money transfer transactions summary
			shoppingCartSummary.MoneyTransferSummary(moneyTransfers, Constants.STATUS_AUTHORIZED);
			//Failed Money transfer transactions summary
			shoppingCartSummary.MoneyTransferSummary(moneyTransfers, Constants.STATUS_FAILED);
			//Succeeded Money transfer transactions summary
			shoppingCartSummary.MoneyTransferSummary(moneyTransfers, Constants.STATUS_COMMITTED);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="shoppingCartSummary"></param>
		/// <param name="funds"></param>
		private static void AddFundSummary(ShoppingCartSummary shoppingCartSummary, List<GprCard> funds)
		{
			// Validated Fund transactions summary
			shoppingCartSummary.FundsSummary(funds, Constants.STATUS_AUTHORIZED);
			// Failed Funds transactions summary
			shoppingCartSummary.FundsSummary(funds, Constants.STATUS_FAILED);
			//Succeeded Funds transactions summary
			shoppingCartSummary.FundsSummary(funds, Constants.STATUS_COMMITTED);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="shoppingCartSummary"></param>
		/// <param name="list"></param>
		private static void AddMoneyOrderSummary(ShoppingCartSummary shoppingCartSummary, List<MoneyOrder> moneyOrders)
		{
			//Pending MoneyOrder transactions summary
			shoppingCartSummary.MoneyOrderSummary(moneyOrders, Constants.STATUS_PENDING);
			//Approved MoneyOrder transactions summary
			shoppingCartSummary.MoneyOrderSummary(moneyOrders, Constants.STATUS_AUTHORIZED);
			//Processing MoneyOrder transactions summary
			shoppingCartSummary.MoneyOrderSummary(moneyOrders, Constants.STATUS_PROCESSING);
			//Succeeded MoneyOrder transactions summary
			shoppingCartSummary.MoneyOrderSummary(moneyOrders, Constants.STATUS_COMMITTED);
			//Cancelled MoneyOrder transactions summary
			shoppingCartSummary.MoneyOrderSummary(moneyOrders, Constants.STATUS_CANCELED);
		}
		#endregion Shoppingcart Summary.

		#region Shoppingcart Detail
		public static ShoppingCartDetail ShoppingCartDetailed(ShoppingCart shoppingCart)
		{
			ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();

			//Add transaction details.
			shoppingCartDetail.CheckDetail(shoppingCart.Checks);
			shoppingCartDetail.BillDetail(shoppingCart.Bills);
			shoppingCartDetail.MoneyTransferDetail(shoppingCart.MoneyTransfers);
			shoppingCartDetail.FundsDetail(shoppingCart.GprCards);
			shoppingCartDetail.CashDetail(shoppingCart.Cash);
			shoppingCartDetail.MoneyOrderDetail(shoppingCart.MoneyOrders);

			//Calculate Funds generated and depleted and other sub totals
			CalculateSubTotal(shoppingCartDetail);

			//Set the shopping cart Id if available
			shoppingCartDetail.Id = shoppingCart.Id;

			//AL-664 
			shoppingCartDetail.IsReferral = shoppingCart.IsReferral;

			//Finaly return the detailed shoppingcart
			return shoppingCartDetail;
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

			getConsolidatedAmounts(activenongprShoppingCartItems, FundType.Credit, out generatedtotal, out generatedAmount, out generatedFee);

			detailedShoppingCart.GeneratedTotal = generatedtotal;
			detailedShoppingCart.GeneratedAmount = generatedAmount;
			detailedShoppingCart.GeneratedFee = generatedFee;

			getConsolidatedAmounts(activenongprShoppingCartItems, FundType.Debit, out depletingtotal, out depletingAmount, out depletingFee);

			detailedShoppingCart.DepletedTotal = depletingtotal;
			detailedShoppingCart.DepletedAmount = depletingAmount;
			detailedShoppingCart.DepletedFee = depletingFee;


			detailedShoppingCart.LoadToCard = activegprShoppingCartItems.Where(x => x.TxnType == FundType.Credit.ToString()
				&& x.Product != "GPRActivation").Sum(x => x.Amount);

			//US1114 - Changes to set the value to withdraw from card. 		
			//detailedShoppingCart.LoadToCard = detailedShoppingCart.LoadToCard - activegprShoppingCartItems.Where(x => x.TxnType == FundType.Debit.ToString()).Sum(x => x.Amount);			
			detailedShoppingCart.WithdrawFromCard = activegprShoppingCartItems.Where(x => x.TxnType == FundType.Debit.ToString()).Sum(x => x.Amount);

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

		private static void getConsolidatedAmounts(List<ShoppingCartItem> activeShoppingCartItems, FundType fundType, out decimal netAmount, out decimal amount, out decimal fee)
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
		public static void UpdateFundsTransaction(ShoppingCartDetail shoppingCartDetail, ShoppingCart cart)
		{
			Desktop desktop = new Desktop();
			decimal loadAmount = 0m;
			decimal withdrawAmount = 0m;
			long customerSessionId;
			long loadTxnId;
			long withdrawTxnId;

			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			customerSessionId = long.Parse(((CustomerSession)HttpContext.Current.Session["CustomerSession"]).CustomerSessionId);

			//AL-2729 user story for updating the cash-in transaction
			if (shoppingCartDetail.CashCollected > 0 && HttpContext.Current.Session["ShoppingCartCheckOutStatus"].ToString() == ShoppingCartCheckoutStatus.CashCollected.ToString())
			{
				desktop.CashIn(customerSessionId, shoppingCartDetail.CashCollected, mgiContext);
			}
			else if (HttpContext.Current.Session["ShoppingCartCheckOutStatus"].ToString() != ShoppingCartCheckoutStatus.MOPrinting.ToString() && HttpContext.Current.Session["ShoppingCartCheckOutStatus"].ToString() != ShoppingCartCheckoutStatus.CashOverCounter.ToString())
			{
				List<CartCash> cashInTrans = new List<CartCash>();

				if (cart != null && cart.Cash != null)
					cashInTrans = cart.Cash.Where(x => x.CashType == CashTransactionType.CashIn.ToString()).ToList();

				if (cashInTrans.Count() > 0)
				{
					if (shoppingCartDetail.CashCollected == 0)
					{
						foreach (CartCash cashTran in cashInTrans)
						{
							desktop.UpdateCash(customerSessionId, long.Parse(cashTran.Id), shoppingCartDetail.CashCollected, mgiContext);
							desktop.CancelCashIn(customerSessionId, long.Parse(cashTran.Id));
						}
					}
					else if (shoppingCartDetail.CashCollected > 0)
					{
						if (cashInTrans.Count() > 1)
						{
							desktop.UpdateCash(customerSessionId, long.Parse(cashInTrans.LastOrDefault().Id), 0, mgiContext);
							desktop.CancelCashIn(customerSessionId, long.Parse(cashInTrans.LastOrDefault().Id));
						}
						desktop.UpdateCash(customerSessionId, long.Parse(cashInTrans.FirstOrDefault().Id), shoppingCartDetail.CashCollected, mgiContext);
					}
				}
				else if (shoppingCartDetail.CashCollected > 0)
					desktop.CashIn(customerSessionId, shoppingCartDetail.CashCollected, mgiContext);
			}

			if (shoppingCartDetail.CardHolder || cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_ACTIVATE).Select(x => x.TransactionId).Count() > 0)
			{
				customerSessionId = long.Parse(((CustomerSession)HttpContext.Current.Session["CustomerSession"]).CustomerSessionId);

				decimal loadFee = desktop.GetFundsFee(customerSessionId, shoppingCartDetail.LoadToCard, FundType.Credit, mgiContext).NetFee;

				decimal withdrawFee = desktop.GetFundsFee(customerSessionId, shoppingCartDetail.WithdrawFromCard, FundType.Debit, mgiContext).NetFee;

				//US1115 - TA3600 - Start
				if (shoppingCartDetail.LoadToCard == 0 && shoppingCartDetail.WithdrawFromCard == 0 && cart.GprCards != null)
				{
					loadAmount = cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_LOAD).Sum(y => y.LoadAmount);
					withdrawAmount = cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_WITHDRAW).Sum(y => y.WithdrawAmount);

					if (loadAmount > 0)
					{
						loadTxnId = cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_LOAD && x.Status != Constants.STATUS_COMMITTED).Select(x => x.TransactionId).FirstOrDefault();
						if (loadTxnId != 0)
							desktop.RemoveFunds(customerSessionId, loadTxnId);
					}
					else if (withdrawAmount > 0)
					{
						withdrawTxnId = cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_WITHDRAW && x.Status != Constants.STATUS_COMMITTED).Select(x => x.TransactionId).FirstOrDefault();
						if (withdrawTxnId != 0)
							desktop.RemoveFunds(customerSessionId, withdrawTxnId);
					}
				}//US1115 - TA3600 - End
				else
				{
					if (shoppingCartDetail.WithdrawFromCard > 0)
					{
						Funds funds = new Funds()
						{
							Amount = Math.Abs(shoppingCartDetail.WithdrawFromCard),
							Fee = withdrawFee
						};
						/////////// When tested for AL-514 in TCF while do not proceed message more than once coming withdraw happening twice,so added the below code ///////////////
						long withdrawTransactionId = 0;
						if (cart.GprCards != null)
							withdrawTransactionId = cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_WITHDRAW && x.Status != Constants.STATUS_COMMITTED).Select(x => x.TransactionId).FirstOrDefault();
						if (withdrawTransactionId != 0 && shoppingCartDetail.WithdrawFromCard != cart.GprCards.Where(x => x.TransactionId == withdrawTransactionId).FirstOrDefault().LoadAmount)
							desktop.UpdateFundAmount(customerSessionId, withdrawTransactionId, funds.Amount, FundType.Debit, mgiContext);
						else if ((withdrawTransactionId == 0) && shoppingCartDetail.WithdrawFromCard != 0)
							desktop.Withdraw(customerSessionId.ToString(), funds, mgiContext);
						////////////////////////////////////////////
					}
					else if (shoppingCartDetail.LoadToCard > 0)
					{
						Funds funds = new Funds()
						{
							Amount = Math.Abs(shoppingCartDetail.LoadToCard),
							Fee = loadFee
						};
						long loadTransactionId = 0;
						if (cart.GprCards != null)
							loadTransactionId = cart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_LOAD && x.Status != Constants.STATUS_COMMITTED).Select(x => x.TransactionId).FirstOrDefault();
						if (loadTransactionId != 0 && shoppingCartDetail.LoadToCard != cart.GprCards.Where(x => x.TransactionId == loadTransactionId).FirstOrDefault().LoadAmount)
							desktop.UpdateFundAmount(customerSessionId, loadTransactionId, funds.Amount, FundType.Credit, mgiContext);
						else if ((loadTransactionId == 0) && shoppingCartDetail.LoadToCard != 0)
							desktop.Load(customerSessionId.ToString(), funds, mgiContext);
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Receipts"></param>
		/// <returns></returns>
		public static string PrepareReceiptForPrinting(SharedData.Receipts Receipts)
		{
			ShoppingCartSuccess shoppingCartSuccess = new ShoppingCartSuccess();

			foreach (Receipt receipt in Receipts.receipts)
			{
				if (shoppingCartSuccess.Receipts == null)
					shoppingCartSuccess.Receipts = new List<string>();
				shoppingCartSuccess.Receipts.Add(receipt.Lines[0]);
			}
			if (shoppingCartSuccess.Receipts != null)
				return string.Join("~~", shoppingCartSuccess.Receipts.ToArray());
			else
				return string.Empty;
		}

		public static string GetReceiptsType(SharedData.Receipts Receipts)
		{
			return string.Join("~~", Receipts.receiptType.ToArray());
		}

		#region MOPRINT
		/// <summary>
		/// 
		/// </summary>
		/// <param name="moneyOrder"></param>
		/// <returns></returns>
		public static string PrepareCheckForPrinting(CheckPrint checkPrint)
		{
			ShoppingCartSuccess shoppingCartSuccess = new ShoppingCartSuccess();

			string str = "";
			if (checkPrint != null)
			{
				if (checkPrint.Lines != null)
				{
					checkPrint.Lines = Common.FileUtility.ConvertCheckPrintImagesToBase64(checkPrint.Lines.ToArray()).ToList<string>();
					foreach (string eachLine in checkPrint.Lines)
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
		// R we using this method , has to check. 
		public static void ParkShoppingCartTrx(string id, ProductType productType)
		{
			Desktop desktop = new Desktop();
			ShoppingCart shoppingCart = new ShoppingCart();
			if (HttpContext.Current.Session["CustomerSession"] == null)
				throw new Exception("Customer session not initiated");

			try
			{
				CustomerSession currentCustomer = (CustomerSession)HttpContext.Current.Session["CustomerSession"];

				long customerSessionId = Convert.ToInt64(currentCustomer.CustomerSessionId);
				switch (productType)
				{
					case ProductType.Checks:
						desktop.ParkCheck(customerSessionId, long.Parse(id));
						break;

					case ProductType.BillPay:
						desktop.ParkBillPay(customerSessionId, long.Parse(id));
						break;

					case ProductType.ReceiveMoney:
					case ProductType.SendMoney:
						desktop.ParkMoneyTransfer(customerSessionId, long.Parse(id));
						break;

					case ProductType.GPRActivation:
						//Deleting activation record should delete load record implicitly.
						shoppingCart = desktop.ShoppingCart(customerSessionId.ToString());
						desktop.ParkFunds(customerSessionId, long.Parse(id));
						if (shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_LOAD).Any())
						{
							string loadTxnId = shoppingCart.GprCards.Where(x => x.ItemType == Constants.PREPAID_CARD_LOAD).FirstOrDefault().Id;
							desktop.ParkFunds(customerSessionId, long.Parse(loadTxnId));
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
								desktop.ParkFunds(customerSessionId, long.Parse(activateTxnId));
							}
						}
						desktop.ParkFunds(customerSessionId, long.Parse(id));
						break;

					case ProductType.GPRWithdraw:
					case ProductType.AddOnCard:
						desktop.ParkFunds(customerSessionId, long.Parse(id));
						break;

					case ProductType.MoneyOrder:
						desktop.ParkMoneyOrder(customerSessionId, long.Parse(id));
						break;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion
	}
}