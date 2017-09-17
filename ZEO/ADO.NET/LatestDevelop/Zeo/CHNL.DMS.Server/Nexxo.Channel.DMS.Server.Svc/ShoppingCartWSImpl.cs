using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IShoppingCartService
	{

        public Response ShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                return DesktopEngine.ShoppingCart(customerSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

		public void SetSelf(IDesktopService dts)
		{
			DesktopEngine.SetSelf(dts);
		}

        public Response AddCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.AddCheck(customerSessionId, checkId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response AddBillPay(long customerSessionId, long billId, MGIContext mgiContext)
		{
            Response response = new Response();
            try 
            {
                DesktopEngine.AddBillPay(customerSessionId, billId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response AddMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.AddMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response AddMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.AddMoneyOrder(customerSessionId, moneyOrderId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response AddFunds(long customerSessionId, long fundId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.AddFunds(customerSessionId, fundId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response RemoveCheck(long customerSessionId, long checkId, bool isParkedTransaction, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.RemoveCheck(customerSessionId, checkId, isParkedTransaction, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response RemoveCheckFromCart(long customerSessionId, long checkId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.RemoveCheckFromCart(customerSessionId, checkId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response RemoveFunds(long customerSessionId, long fundsId, bool isParkedTransaction, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.RemoveFunds(customerSessionId, fundsId, isParkedTransaction, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response RemoveBillPay(long customerSessionId, long billPayId, bool isParkedTransaction, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.RemoveBillPay(customerSessionId, billPayId, isParkedTransaction, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response RemoveMoneyOrder(long customerSessionId, long moneyOrderId, bool isParkedTransaction, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.RemoveMoneyOrder(customerSessionId, moneyOrderId, isParkedTransaction, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, bool isParkedTransaction, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.RemoveMoneyTransfer(customerSessionId, moneyTransferId, isParkedTransaction, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}
		
        public Response RemoveCashIn(long customerSessionId, long cashIn, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.RemoveCashIn(customerSessionId, cashIn, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}
		
        public Response Checkout(long customerSessionId, decimal cashToCustomer, string cardNumber, ShoppingCartCheckoutStatus shoppingCartstatus, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.Checkout(customerSessionId, cashToCustomer, cardNumber, shoppingCartstatus, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}
        
        public Response AddCash(long customerSessionId, long cashTxnId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.AddCash(customerSessionId, cashTxnId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}
        
        public Response GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GenerateReceiptsForShoppingCart(customerSessionId, shoppingCartId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response CloseShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.CloseShoppingCart(customerSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response ParkCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.ParkCheck(customerSessionId, checkId, mgiContext);
                DesktopEngine.ReSubmitCheck(customerSessionId, checkId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response ReSubmitCheck(long customerSessionId, long checkId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.ReSubmitCheck(customerSessionId, checkId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response ParkFunds(long customerSessionId, long fundsId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.ParkFunds(customerSessionId, fundsId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response ParkBillPay(long customerSessionId, long billPayId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.ParkBillPay(customerSessionId, billPayId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response ParkMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.ParkMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response ParkMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.ParkMoneyOrder(customerSessionId, moneyOrderId, mgiContext);
                DesktopEngine.ReSubmitMO(customerSessionId, moneyOrderId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response ReSubmitMO(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.ReSubmitCheck(customerSessionId, moneyOrderId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added new method to Get IsReferralApplicable for the ChannelPartner
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="mgiContext"></param>
		/// <returns></returns>
		public Response IsReferralApplicable(long customerSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.IsReferralApplicable(customerSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="mgiContext"></param>
		public Response PostFlush(long customerSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.PostFlush(customerSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
		}

        public Response GetAllParkedShoppingCartTransactions()
        {
            Response response = new Response();
            try
            {
                return DesktopEngine.GetAllParkedShoppingCartTransactions();
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
                response.Error = PrepareError(ex);
            }
            return response;
        }
    }
}