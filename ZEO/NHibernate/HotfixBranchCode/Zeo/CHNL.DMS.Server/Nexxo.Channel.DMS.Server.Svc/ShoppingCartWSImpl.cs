using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;
using System;
using System.Collections.Generic;


namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IShoppingCartService
	{

        public ShoppingCart ShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.ShoppingCart(customerSessionId, mgiContext);
		}

		public void SetSelf(IDesktopService dts)
		{
			DesktopEngine.SetSelf(dts);
		}

        public List<Check> Cheques(string customerSessionId, string shoppingCartId, MGIContext mgiContext)
		{
			//return DesktopEngine.Cheques(customerSessionId, shoppingCartId);
			throw new NotImplementedException();
		}

        public void AddCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
            DesktopEngine.AddCheck(customerSessionId, checkId, mgiContext);
		}

        public void AddBillPay(long customerSessionId, long billId, MGIContext mgiContext)
		{
            DesktopEngine.AddBillPay(customerSessionId, billId, mgiContext);
		}

        public void AddMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext)
		{
            DesktopEngine.AddMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
		}

        public void AddMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
            DesktopEngine.AddMoneyOrder(customerSessionId, moneyOrderId, mgiContext);
		}

        public void AddFunds(long customerSessionId, long fundId, MGIContext mgiContext)
		{
            DesktopEngine.AddFunds(customerSessionId, fundId, mgiContext);
		}

        public bool RemoveCheck(long customerSessionId, long checkId, bool isParkedTransaction, MGIContext mgiContext)
		{
           return DesktopEngine.RemoveCheck(customerSessionId, checkId, isParkedTransaction, mgiContext);
		}

        public void RemoveCheckFromCart(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			DesktopEngine.RemoveCheckFromCart(customerSessionId, checkId, mgiContext);
		}

        public void RemoveFunds(long customerSessionId, long fundsId, bool isParkedTransaction, MGIContext mgiContext)
		{
            DesktopEngine.RemoveFunds(customerSessionId, fundsId, isParkedTransaction, mgiContext);
		}

        public void RemoveBillPay(long customerSessionId, long billPayId, bool isParkedTransaction, MGIContext mgiContext)
		{
            DesktopEngine.RemoveBillPay(customerSessionId, billPayId, isParkedTransaction, mgiContext);
		}

        public void RemoveMoneyOrder(long customerSessionId, long moneyOrderId, bool isParkedTransaction, MGIContext mgiContext)
		{
            DesktopEngine.RemoveMoneyOrder(customerSessionId, moneyOrderId, isParkedTransaction, mgiContext);
		}

        public void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, bool isParkedTransaction, MGIContext mgiContext)
		{
            DesktopEngine.RemoveMoneyTransfer(customerSessionId, moneyTransferId, isParkedTransaction, mgiContext);
		}
		public void RemoveCashIn(long customerSessionId, long cashIn, MGIContext mgiContext)
		{
			DesktopEngine.RemoveCashIn(customerSessionId,cashIn,mgiContext);
		}
		public ShoppingCartCheckoutStatus Checkout(long customerSessionId, decimal cashToCustomer, string cardNumber, ShoppingCartCheckoutStatus shoppingCartstatus, MGIContext mgiContext)
		{
			return DesktopEngine.Checkout(customerSessionId, cashToCustomer, cardNumber, shoppingCartstatus,mgiContext);
		}
        public void AddCash(long customerSessionId, long cashTxnId, MGIContext mgiContext)
		{
            DesktopEngine.AddCash(customerSessionId, cashTxnId, mgiContext);
		}
        public Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext)
		{
            return DesktopEngine.GenerateReceiptsForShoppingCart(customerSessionId, shoppingCartId, mgiContext);
		}

        public void CloseShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
            DesktopEngine.CloseShoppingCart(customerSessionId, mgiContext);
		}

		public void ParkCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
			DesktopEngine.ParkCheck(customerSessionId, checkId, mgiContext);
            DesktopEngine.ReSubmitCheck(customerSessionId, checkId, mgiContext);
		}

        public void ReSubmitCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
            DesktopEngine.ReSubmitCheck(customerSessionId, checkId, mgiContext);
		}

        public void ParkFunds(long customerSessionId, long fundsId, MGIContext mgiContext)
		{
            DesktopEngine.ParkFunds(customerSessionId, fundsId, mgiContext);
		}

        public void ParkBillPay(long customerSessionId, long billPayId, MGIContext mgiContext)
		{
            DesktopEngine.ParkBillPay(customerSessionId, billPayId, mgiContext);
		}

        public void ParkMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext)
		{
			DesktopEngine.ParkMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
		}

        public void ParkMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
            DesktopEngine.ParkMoneyOrder(customerSessionId, moneyOrderId, mgiContext);
            DesktopEngine.ReSubmitMO(customerSessionId, moneyOrderId, mgiContext);
		}

        public void ReSubmitMO(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
            DesktopEngine.ReSubmitCheck(customerSessionId, moneyOrderId, mgiContext);
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added new method to Get IsReferralApplicable for the ChannelPartner
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="mgiContext"></param>
		/// <returns></returns>
		public bool IsReferralApplicable(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.IsReferralApplicable(customerSessionId, mgiContext);
		}


		/// <summary>
		/// Author : Abhijith
		/// Description : 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="mgiContext"></param>
		public void PostFlush(long customerSessionId, MGIContext mgiContext)
		{
			 DesktopEngine.PostFlush(customerSessionId, mgiContext);
		}

        public List<ParkedTransaction> GetAllParkedShoppingCartTransactions()
        {
            return DesktopEngine.GetAllParkedShoppingCartTransactions();
        }
    }
}