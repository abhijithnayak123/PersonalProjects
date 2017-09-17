using TCF.Channel.Zeo.Data;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.MoneyOrder.Contract
{
    public interface IMoneyOrderServices
    {
        /// <summary>
        /// To get the Money Order fee
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        TransactionFee GetMoneyOrderFee(decimal amount, CommonData.ZeoContext context);

        /// <summary>
        /// To add the transaction to the Shopping Cart
        /// </summary>
        /// <param name="moneyOrderPurchase"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        TCF.Channel.Zeo.Data.MoneyOrder Add(MoneyOrderPurchase moneyOrderPurchase, CommonData.ZeoContext context);

        /// <summary>
        /// To commit the transaction from the Shopping Cart
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool Commit(long transactionId, CommonData.ZeoContext context);

        /// <summary>
        /// To update the Money Order transaction
        /// </summary>
        /// <param name="moneyOrder"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool UpdateMoneyOrder(TCF.Channel.Zeo.Data.MoneyOrder moneyOrder, CommonData.ZeoContext context);

        /// <summary>
        /// To update the MoneyOrder status
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="state"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool UpdateMoneyOrderStatus(long transactionId, int state, CommonData.ZeoContext context);

        /// <summary>
        /// To get the check print from the Money Order
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        MoneyOrderCheckPrint GetMoneyOrderCheck(long transactionId, CommonData.ZeoContext context);

        /// <summary>
        /// To resubmit the transactions after a transaction is parked
        /// </summary>
        /// <param name="moneyOrderId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool Resubmit(long moneyOrderId, CommonData.ZeoContext context);

        /// <summary>
        /// TO get the Money Order diagnostics
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        MoneyOrderCheckPrint GetMoneyOrderDiagnostics(CommonData.ZeoContext context);

        /// <summary>
        /// Get the MoneyOrder Transaction
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        TCF.Channel.Zeo.Data.MoneyOrder GetMoneyOrderTransaction(long transactionId, CommonData.ZeoContext context);
    }
}
