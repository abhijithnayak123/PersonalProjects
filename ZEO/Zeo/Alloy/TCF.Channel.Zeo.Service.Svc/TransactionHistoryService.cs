using TCF.Channel.Zeo.Service.Contract;
using System;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : ITransactionService
    {
        public Response GetAgentTransactions(TransactionHistorySearchCriteria criteria,ZeoContext context)
        {
            return serviceEngine.GetAgentTransactions(criteria, context);
        }

        public Response GetCustomerTransactionLocations(DateTime dateRange, ZeoContext context)
        {
            return serviceEngine.GetCustomerTransactionLocations(dateRange, context);
        }

        public Response GetCustomerTransactions(TransactionHistorySearchCriteria criteria, ZeoContext context)
        {
            return serviceEngine.GetCustomerTransactions(criteria, context);
        }

        public Response GetMoneyOrderTransaction(long transactionId, ZeoContext context)
        {
            return serviceEngine.GetMoneyOrderTransaction(transactionId, context);
        }

        public Response GetCustomerSessionId(long transactionId,int transactionTypeValue, ZeoContext context)
        {
            return serviceEngine.GetCustomerSessionId(transactionId, transactionTypeValue, context);
        }
    }
}