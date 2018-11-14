using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface IShoppingCartService
    {
        bool RemoveShoppingCartTransaction(long transactionId, int productId, ZeoContext context);

        bool AddShoppingCartTransaction(long customerSessionnId, long transactionId, long productId, string timeZone, ZeoContext context);

        bool ParkShoppingCartTransaction(long customerSessionnId, long transactionId, long productId, string timeZone, ZeoContext context);

        ShoppingCart GetShoppingCart(long customerSessionId, ZeoContext context);

		ShoppingCart GetShoppingCartById(long cartId, ZeoContext context);
        ShoppingCartCheckOut GetShoppingCartCheckOutDetails(long customerSessionId, long channelPartnerId, bool isReferral, ZeoContext context);

        void CashOutAndUpdateReferral(long customerSessionId, decimal cashToCustomer, int cartStatus, long cartId, string timeZone, bool isReferral, ZeoContext context);

        void CashOutAndUpdateCartStatus(long cartId, decimal amount, long customerSessionId, int cartStatus, string timeZone, ZeoContext context);

        bool IsShoppingCartEmpty(long customerSessionId, ZeoContext context);

        bool CanCloseCustomerSession(long customerSessionId, string timeZone, ZeoContext context);

        List<ParkedTransaction> GetAllParkedTransaction(ZeoContext context);

        List<long> GetResubmitTransactions(int productId, long customerSessionId, ZeoContext context);

        ShoppingCart GetShoppingCartForReceipts(ZeoContext context);
    }
}
