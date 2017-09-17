using System.Collections.Generic;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Contract
{
	public interface IShoppingCartService
    {
        bool RemoveCheck(long customerSessionId, long checkId, bool isParkedTransaction, MGIContext mgiContext);

        void RemoveFunds(long customerSessionId, long fundsId, bool isParkedTransaction, MGIContext mgiContext);

        void RemoveBillPay(long customerSessionId, long billPayId, bool isParkedTransaction, MGIContext mgiContext);

        void RemoveMoneyOrder(long customerSessionId, long moneyOrderId, bool isParkedTransaction, MGIContext mgiContext);

        void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, bool isParkedTransaction, MGIContext mgiContext);

		void RemoveCashIn(long customerSessionId, long cashInId, MGIContext mgiContext);

		ShoppingCart ShoppingCart(long customerSessionId, MGIContext mgiContext);

		ShoppingCartCheckoutStatus Checkout(long customerSessionId, decimal cashToCustomer,  string cardNumber, ShoppingCartCheckoutStatus shoppingCartstatus, MGIContext mgiContext);

		Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext);

		void CloseShoppingCart(long customerSessionId, MGIContext mgiContext);

		void ReSubmitCheck(long customerSessionId, long checkId, MGIContext mgiContext);

		void ReSubmitMO(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

		void PostFlush(long customerSessionId, MGIContext mgiContext);

		void RemoveCheckFromCart(long customerSessionId, long checkId, MGIContext mgiContext);

        List<ParkedTransaction> GetAllParkedShoppingCartTransactions();
    }
}
