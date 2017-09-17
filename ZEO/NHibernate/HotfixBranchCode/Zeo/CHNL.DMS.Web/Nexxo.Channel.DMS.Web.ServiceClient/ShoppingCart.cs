using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using MGI.Channel.DMS.Web.ServiceClient.DMSService;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Server.Data;
using NexxoSOAPFault = MGI.Common.Sys.NexxoSOAPFault;
using MGI.Common.Sys;
using SharedData = MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.ServiceClient
{
    /// <summary>
    /// This is partial implementation of Desktop class, ie methods related to shopping cart.
    /// </summary>
    public partial class Desktop
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        public SharedData.ShoppingCart ShoppingCart(string customerSessionId)
        {
            try
            {

				return DesktopService.ShoppingCart(long.Parse(customerSessionId), new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="check"></param>
        public bool RemoveCheque(long customerSessionId, long TxnId, MGIContext mgiContext, bool isParkedTransaction = false)
        {
            try
            {                
                return DesktopService.RemoveCheck(customerSessionId, TxnId, isParkedTransaction, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="bill"></param>
        public void RemoveBill(long customerSessionId, long TxnId, bool isParkedTransaction = false)
        {
            try
            {

				DesktopService.RemoveBillPay(customerSessionId, TxnId, isParkedTransaction, new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="moneyTransfer"></param>
        public void RemoveMoneyTransfer(long customerSessionId, long TxnId, bool isParkedTransaction = false)
        {
            try
            {

				DesktopService.RemoveMoneyTransfer(customerSessionId, TxnId, isParkedTransaction, new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="gprCard"></param>
        public void RemoveFunds(long customerSessionId, long TxnId, bool isParkedTransaction = false)
        {
            try
            {

				DesktopService.RemoveFunds(customerSessionId, TxnId, isParkedTransaction, new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="gprCard"></param>
		public void RemoveCashIn(long customerSessionId, long TxnId)
		{
			try
			{

				DesktopService.RemoveCashIn(customerSessionId, TxnId, new MGIContext());
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}


        public SharedData.ShoppingCartCheckoutStatus Checkout(string customerSessionId, decimal cashToCustomer, string cardNumber, SharedData.ShoppingCartCheckoutStatus shoppingCartStatus, MGIContext mgiContext)
        {
            try
            {
                return DesktopService.Checkout(long.Parse(customerSessionId), cashToCustomer, cardNumber,shoppingCartStatus, mgiContext);
            }
            catch (FaultException<MGI.Common.Sys.NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }
        public void RemoveMoneyOrder(long customerSessionId, long TxnId)
        {
            try
            {

				DesktopService.CancelMoneyOrder(customerSessionId, TxnId, new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void ParkCheck(long customerSessionId, long checkId)
        {
            try
            {

				DesktopService.ParkCheck(customerSessionId, checkId, new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void ParkFunds(long customerSessionId, long fundsId)
        {
            try
            {

				DesktopService.ParkFunds(customerSessionId, fundsId, new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void ParkBillPay(long customerSessionId, long billPayId)
        {
            try
            {

				DesktopService.ParkBillPay(customerSessionId, billPayId, new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void ParkMoneyTransfer(long customerSessionId, long moneyTransferId)
        {
            try
            {

				DesktopService.ParkMoneyTransfer(customerSessionId, moneyTransferId, new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

        public void ParkMoneyOrder(long customerSessionId, long moneyOrderId)
        {
            try
            {

				DesktopService.ParkMoneyOrder(customerSessionId, moneyOrderId, new MGIContext());
            }
            catch (FaultException<NexxoSOAPFault> nexxoFault)
            {
                throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
            }
        }

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Adding new method to get IsReferral Applicable for the channelpartner
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public bool IsReferralApplicable(long customerSessionId, MGIContext mgiContext)
		{
			try
			{
				return DesktopService.IsReferralApplicable(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}
    }
}
