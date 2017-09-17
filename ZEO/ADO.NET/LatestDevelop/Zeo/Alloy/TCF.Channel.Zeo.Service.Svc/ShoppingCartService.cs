using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IShoppingCartService
    {

        public Response GetShoppingCart(long customerSessionId,ZeoContext context)
        {
            return serviceEngine.GetShoppingCart(customerSessionId, context);
        }

        public Response ParkShoppingCartTransaction(long transactionId, int productId, ZeoContext context)
        {
            return serviceEngine.ParkShoppingCartTransaction(transactionId, productId, context);
        }

        public Response RemoveCheck(long transactionId, ZeoContext context)
        {
            return serviceEngine.RemoveCheck(transactionId, context);
        }

        public Response RemoveBillPay(long transactionId, ZeoContext context)
        {
            return serviceEngine.RemoveBillPay(transactionId, context);
        }

        public Response RemoveMoneyTransfer(long transactionId, int productType, ZeoContext context)
        {
            return serviceEngine.RemoveMoneyTransfer(transactionId, productType, context);
        }

        public Response RemoveFund(long transactionId, bool hasFundsAccount, ZeoContext context)
        {
            return serviceEngine.RemoveFund(transactionId, hasFundsAccount, context);
        }

        public Response RemoveMoneyOrder(long transactionId, ZeoContext context)
        {
            return serviceEngine.RemoveMoneyOrder(transactionId, context);
        }

        public Response RemoveCashIn(ZeoContext context)
        {
            return serviceEngine.RemoveCashIn(context);
        }


        public Response ShoppingCartCheckout(decimal cashToCustomer, Helper.ShoppingCartCheckoutStatus status, ZeoContext context)
        {
            return serviceEngine.ShoppingCartCheckout(cashToCustomer, status, context);
        }

        public Response PostFlush(decimal cardBalance, ZeoContext context)
        {
            return serviceEngine.PostFlush(cardBalance, context);
        }

        public Response GetPrepaidCardNumber(ZeoContext context)
        {
            return serviceEngine.GetPrepaidCardNumber(context);
        }

        public Response IsShoppingCartEmpty(Data.ZeoContext context)
        {
            return serviceEngine.IsShoppingCartEmpty(context);
        }

        public Response CanCloseCustomerSession(Data.ZeoContext context)
        {
            return serviceEngine.CanCloseCustomerSession(context);
        }
		
		 public Response GetAllParkedTransaction(ZeoContext context)
        {
            return serviceEngine.GetAllParkedTransaction(context);
        }
		
        public Response GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, ZeoContext context)
        {
            return serviceEngine.GenerateReceiptsForShoppingCart(customerSessionId, shoppingCartId, context);
        }


    }
}