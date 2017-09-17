using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Contract
{
    public interface IMoneyOrderService : IDisposable
    {
        /// <summary>
        /// To create a transaction in the ttxn_MoneyOrder table
        /// </summary>
        /// <param name="moneyOrderTransaction"></param>
        /// <returns></returns>
        long CreateMoneyOrderTransaction(MoneyOrder moneyOrderTransaction, ZeoContext context);

        /// <summary>
        /// To get the transaction based on the transactionId
        /// </summary>
        /// <param name="moneyOrderId"></param>
        /// <returns></returns>
        MoneyOrder GetMoneyOrderTransactionById(long moneyOrderId, ZeoContext context);

        /// <summary>
        /// To update the MoneyOrder state
        /// </summary>
        /// <param name="moneyOrderId"></param>
        /// <param name="state"></param>
        /// <param name="description"></param>
        /// <param name="dtTerminalModified"></param>
        /// <returns></returns>
        bool UpdateMoneyOrderState(long moneyOrderId, long customerId, int state, DateTime dtTerminalModified, ZeoContext context);

        /// <summary>
        /// To update the MoneyOrder transaction
        /// </summary>
        /// <param name="moneyOrderTransaction"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        bool UpdateMoneyOrderTransaction(MoneyOrder moneyOrderTransaction, MoneyOrderImage moneyOrderImage,bool AllowDuplicateMoneyOrder, string timeZone, ZeoContext context);


        /// <summary>
        /// To update the fee and discount related details in the database
        /// </summary>
        /// <param name="moneyOrder"></param>
        /// <returns></returns>
        bool UpdateMoneyOrderFee(Core.Data.MoneyOrder moneyOrder, ZeoContext context);
    }
}
