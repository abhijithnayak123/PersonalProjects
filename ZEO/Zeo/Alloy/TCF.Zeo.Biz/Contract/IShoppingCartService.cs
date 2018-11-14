using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Biz.Contract
{
    public interface IShoppingCartService
    {
        bool AddShoppingCartTransaction(long transactionId, long productId, commonData.ZeoContext context);

        bool ParkShoppingCartTransaction(long customerSessionId, long transactionId, long productId, commonData.ZeoContext context);

        bool RemoveShoppingCartTransaction(long transactionId, int productId, commonData.ZeoContext context);

        ShoppingCart GetShoppingCart(long customerSessionId, commonData.ZeoContext context);

        ShoppingCart GetShoppingCartById(long cartId, commonData.ZeoContext context);

        Helper.ShoppingCartCheckoutStatus ShoppingCartCheckout(decimal cashToCustomer, Helper.ShoppingCartCheckoutStatus cartCheckOutStatus, commonData.ZeoContext context);

        bool IsShoppingCartEmpty(commonData.ZeoContext context);

        bool CanCloseCustomerSession(commonData.ZeoContext context);

		List<ParkedTransaction> GetAllParkedTransaction(commonData.ZeoContext context);
		
		Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, commonData.ZeoContext context);

        List<long> GetResubmitTransactions(int productId, long customerSessionId, commonData.ZeoContext context);

    }
}
