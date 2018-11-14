using TCF.Zeo.Common.Data;
using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface ITransactionService
    {
        /// <summary>
        /// Method to fetch all transaction done by customer
        /// </summary>
        /// <param name="criteria">transaction search criteria</param>
        /// <param name="context">This is a common object</param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response GetCustomerTransactions(TransactionHistorySearchCriteria criteria, Data.ZeoContext context);

        /// <summary>
        /// Method to fetch all transactions for agent
        /// </summary>
        /// <param name="criteria">transaction search criteria</param>
        /// <param name="context">This is a common object</param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response GetAgentTransactions(TransactionHistorySearchCriteria criteria, Data.ZeoContext context);

        /// <summary>
        /// To fetch all locations where transactions are performed
        /// </summary>
        /// <param name="dateRange">Date Period</param>
        /// <param name="contex"></param>
        /// <returns>Collection of String as Result</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response GetCustomerTransactionLocations(DateTime dateRange, Data.ZeoContext context);


        /// <summary>
        /// This Will Get the CustomerSessionId based on TransactionId
        /// </summary>
        /// <param name="transactionId"> transactionid for the perticular Customer Transaction</param>
        /// <param name="transactionTypeValue">Ttansaction Type</param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        Response GetCustomerSessionId(long transactionId, int transactionTypeValue, Data.ZeoContext context);

    }
}
