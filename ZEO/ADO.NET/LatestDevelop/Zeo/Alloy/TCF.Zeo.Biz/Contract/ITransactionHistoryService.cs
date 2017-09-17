using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
    public interface ITransactionHistoryService
    {
        /// <summary>
        /// To get Customer's previous transaction history
        /// </summary>
        /// <param name="criteria">Search filter's</param>
        /// <returns></returns>
        List<TransactionHistory> GetCustomerTransactionHistory(TransactionHistorySearchCriteria criteria, commonData.ZeoContext context);

        /// <summary>
        /// To get agent's transaction history
        /// </summary>
        /// <param name="criteria">Search filter's</param>
        /// <returns></returns>
        List<TransactionHistory> GetAgentTransactionHistory(TransactionHistorySearchCriteria criteria, commonData.ZeoContext context);

        /// <summary>
        /// To fetch all the locations from which customer has performed transactions 
        /// </summary>
        /// <param name="dateRange">date period</param>
        /// <param name="context"></param>
        /// <returns>collection of locations</returns>
        List<string> GetCustomerTransactionLocation(DateTime dateRange, commonData.ZeoContext context);


        /// <summary>
        /// This Will Get the CustomerSessionId based on TransactionId
        /// </summary>
        /// <param name="transactionId"> transactionid for the perticular Customer Transaction</param>
        /// <param name="transactionTypeValue">Ttansaction Type</param>
        /// <param name="context"></param>
        /// <returns></returns>
        long GetCustomerSessionId(long transactionId,int transactionTypeValue, commonData.ZeoContext context);

    }
}
