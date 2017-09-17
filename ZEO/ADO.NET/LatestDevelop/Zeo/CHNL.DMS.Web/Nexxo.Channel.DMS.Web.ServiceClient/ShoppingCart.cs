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
        public Response ShoppingCart(string customerSessionId)
        {
			Response response = DesktopService.ShoppingCart(long.Parse(customerSessionId), new MGIContext());
			return response;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="check"></param>
        public Response RemoveCheque(long customerSessionId, long TxnId, MGIContext mgiContext, bool isParkedTransaction = false)
        {          
            Response response = DesktopService.RemoveCheck(customerSessionId, TxnId, isParkedTransaction, mgiContext);
			return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="bill"></param>
        public Response RemoveBill(long customerSessionId, long TxnId, bool isParkedTransaction = false)
        {
			Response response =	DesktopService.RemoveBillPay(customerSessionId, TxnId, isParkedTransaction, new MGIContext());
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="moneyTransfer"></param>
        public Response RemoveMoneyTransfer(long customerSessionId, long TxnId, bool isParkedTransaction = false)
        {
			Response response =	DesktopService.RemoveMoneyTransfer(customerSessionId, TxnId, isParkedTransaction, new MGIContext());
			return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="gprCard"></param>
        public Response RemoveFunds(long customerSessionId, long TxnId, bool isParkedTransaction = false)
        {
            Response response = DesktopService.RemoveFunds(customerSessionId, TxnId, isParkedTransaction, new MGIContext());
			return response;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="gprCard"></param>
		public Response RemoveCashIn(long customerSessionId, long TxnId)
		{
			Response response =	DesktopService.RemoveCashIn(customerSessionId, TxnId, new MGIContext());
			return response;
		}

        public Response Checkout(string customerSessionId, decimal cashToCustomer, string cardNumber, SharedData.ShoppingCartCheckoutStatus shoppingCartStatus, MGIContext mgiContext)
        {
            Response response = DesktopService.Checkout(long.Parse(customerSessionId), cashToCustomer, cardNumber,shoppingCartStatus, mgiContext);
			return response;
        }
        
        public Response RemoveMoneyOrder(long customerSessionId, long TxnId)
        {
			Response response =	DesktopService.CancelMoneyOrder(customerSessionId, TxnId, new MGIContext());
			return response;
        }

        public Response ParkCheck(long customerSessionId, long checkId)
        {
			Response response =	DesktopService.ParkCheck(customerSessionId, checkId, new MGIContext());
			return response;
        }

        public Response ParkFunds(long customerSessionId, long fundsId)
        {
			Response response = DesktopService.ParkFunds(customerSessionId, fundsId, new MGIContext());
			return response;
        }

        public Response ParkBillPay(long customerSessionId, long billPayId)
        {
			Response response =	DesktopService.ParkBillPay(customerSessionId, billPayId, new MGIContext());
			return response;
        }

        public Response ParkMoneyTransfer(long customerSessionId, long moneyTransferId)
        {
			Response response =	DesktopService.ParkMoneyTransfer(customerSessionId, moneyTransferId, new MGIContext());
			return response;
        }

        public Response ParkMoneyOrder(long customerSessionId, long moneyOrderId)
        {
			Response response =	DesktopService.ParkMoneyOrder(customerSessionId, moneyOrderId, new MGIContext());
			return response;
        }

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Adding new method to get IsReferral Applicable for the channelpartner
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public Response IsReferralApplicable(long customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.IsReferralApplicable(customerSessionId, mgiContext);
			return response;
		}
    }
}
