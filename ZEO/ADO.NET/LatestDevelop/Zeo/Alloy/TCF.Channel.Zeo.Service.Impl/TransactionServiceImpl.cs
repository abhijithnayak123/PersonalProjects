using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Contract;
using TCF.Zeo.Biz.Impl;
using System;
using commonData = TCF.Zeo.Common.Data;
using System.ServiceModel;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : ITransactionService
    {
        public ITransactionHistoryService tranService;

        public Response GetAgentTransactions(TransactionHistorySearchCriteria criteria, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            tranService = new TransactionHistoryServiceImpl();
            Response response = new Response();
            response.Result = tranService.GetAgentTransactionHistory(criteria, commonContext);
            return response;
        }

        public Response GetCustomerTransactionLocations(DateTime dateRange, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            tranService = new TransactionHistoryServiceImpl();
            Response response = new Response();
            response.Result = tranService.GetCustomerTransactionLocation(dateRange, commonContext);
            return response;
        }

        public Response GetCustomerTransactions(TransactionHistorySearchCriteria criteria, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            tranService = new TransactionHistoryServiceImpl();
            Response response = new Response();
            response.Result = tranService.GetCustomerTransactionHistory(criteria, commonContext);
            return response;
        }

        public Response GetCustomerSessionId(long transactionId, int transactionTypeValue, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            response.Result = tranService.GetCustomerSessionId(transactionId,transactionTypeValue, commonContext);
            return response;
        }
    }
}
