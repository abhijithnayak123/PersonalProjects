using System;
using System.Collections.Generic;
using System.Linq;
using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.MVA.Server.Impl
{
	public partial class MVAEngine : IShoppingCartService
	{
		#region IShoppingCartService Impl


		public SharedData.Receipts Checkout(long customerSessionId)
		{
			MGIContext mgiContext = Self.GetCustomerContext(customerSessionId);

			SharedData.ShoppingCart cart = ConsumerEngine.ShoppingCart(customerSessionId, mgiContext);

			//Withdraw Funds for cart from card and add fund trx
			string cardNumber = String.Empty;//Withdraw(customerSessionId, cart, context);

			//Checkout
			SharedData.ShoppingCartCheckoutStatus cartStatus = ConsumerEngine.Checkout(customerSessionId, cardNumber, SharedData.ShoppingCartCheckoutStatus.InitialCheckout, mgiContext);

			Receipts receipts = null;
			if (cartStatus == SharedData.ShoppingCartCheckoutStatus.Completed)
			{
				//receipts = ConsumerEngine.GenerateReceiptsForShoppingCart( customerSessionId, cart.Id , context );

				ConsumerEngine.CloseShoppingCart(customerSessionId, mgiContext);
			}
			return receipts;
		}

		public void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId)
		{
			ConsumerEngine.RemoveMoneyTransfer(customerSessionId, moneyTransferId, Self.GetCustomerContext(customerSessionId));
		}

		public void RemoveBillPay(long customerSessionId, long billPayId)
		{
			ConsumerEngine.RemoveBillPay(customerSessionId, billPayId, Self.GetCustomerContext(customerSessionId));
		}

		#endregion

		#region Private methods

		private string Withdraw(long customerSessionId, SharedData.ShoppingCart cart, MGIContext mgiContext)
		{
			decimal withdrawAmount = 0m;
			decimal withdrawFee = 0m;

			withdrawAmount = GetWithdrawAmount(cart);
			withdrawFee = ConsumerEngine.GetFundsFee(customerSessionId, withdrawAmount, SharedData.FundType.Debit, mgiContext).NetFee;

			SharedData.FundsProcessorAccount fundAccount = ConsumerEngine.LookupFundsAccount(customerSessionId, mgiContext);

			if (fundAccount.CardBalance < (Math.Abs(withdrawAmount) + withdrawFee))
				throw new Exception("Withdraw Amount and related fee should be less than available balance.");

			SharedData.Funds funds = new SharedData.Funds()
			{
				Amount = Math.Abs(withdrawAmount),
				Fee = withdrawFee
			};

			//Withdraw Funds from card  
			ConsumerEngine.WithdrawFunds(customerSessionId, funds, mgiContext);

			return fundAccount.CardNumber;
		}

		private decimal GetWithdrawAmount(SharedData.ShoppingCart cart)
		{
			decimal TrxAmount = 0m;
			decimal TrxFee = 0m;

			//Get Send money Trx Amount
			TrxAmount += cart.MoneyTransfers != null ? cart.MoneyTransfers.Where(moneyTransfer => moneyTransfer.TransferType == (int)SharedData.TransferType.SendMoney).Sum(d => d.Amount) : TrxAmount;
			TrxFee += cart.MoneyTransfers != null ? cart.MoneyTransfers.Where(moneyTransfer => moneyTransfer.TransferType == (int)SharedData.TransferType.SendMoney).Sum(d => d.Fee) : TrxAmount;

			return TrxAmount + TrxFee;
		}


		#endregion



	}
}
