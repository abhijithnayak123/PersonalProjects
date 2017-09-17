using AutoMapper;
using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using commonData = TCF.Zeo.Common.Data;
using core = TCF.Zeo.Core;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Common.Util;
using BizCommon = TCF.Zeo.Biz.Common;

namespace TCF.Zeo.Biz.Impl
{
    public class TransactionHistoryServiceImpl : Contract.ITransactionHistoryService
    {
        IMapper mapper;
        private ITransactionHistoryService trxService { get; set; }

        public TransactionHistoryServiceImpl()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<core.Data.TransactionHistory, TransactionHistory>();
                cfg.CreateMap<TransactionHistorySearchCriteria, core.Data.TransactionSearchCriteria>();
            });
            mapper = config.CreateMapper();
        }

        #region Public Region

        public List<TransactionHistory> GetCustomerTransactionHistory(TransactionHistorySearchCriteria criteria, commonData.ZeoContext context)
        {
            try
            {
                trxService = new core.Impl.TransactionHistoryServiceImpl();
                core.Data.TransactionSearchCriteria coreCriteria = mapper.Map<core.Data.TransactionSearchCriteria>(criteria);

                List<core.Data.TransactionHistory> coreHistory = trxService.GetCustomerTransactionHistory(coreCriteria, context);

                List<TransactionHistory> trxHistory = mapper.Map<List<TransactionHistory>>(coreHistory);

                return trxHistory;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCommon.Data.Exceptions.TransactionHistoryException(BizCommon.Data.Exceptions.TransactionHistoryException.GET_CUSTOMER_TRANSACTION_HISTORY_FAILED, ex);
            }
        }

        public List<TransactionHistory> GetAgentTransactionHistory(TransactionHistorySearchCriteria criteria, commonData.ZeoContext context)
        {
            try
            {
                trxService = new core.Impl.TransactionHistoryServiceImpl();
                core.Data.TransactionSearchCriteria coreCriteria = mapper.Map<core.Data.TransactionSearchCriteria>(criteria);

                List<core.Data.TransactionHistory> coreHistory = trxService.GetAgentTransactionHistory(coreCriteria, context);

                List<TransactionHistory> trxHistory = mapper.Map<List<TransactionHistory>>(coreHistory);

                return trxHistory;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCommon.Data.Exceptions.TransactionHistoryException(BizCommon.Data.Exceptions.TransactionHistoryException.GET_AGENT_TRANSACTION_HISTORY_FAILED, ex);
            }
        }

        public List<string> GetCustomerTransactionLocation(DateTime dateRange, commonData.ZeoContext context)
        {
            try
            {
                trxService = new core.Impl.TransactionHistoryServiceImpl();
                return trxService.GetCustomerTransactionLocation(dateRange, mapper.Map<commonData.ZeoContext>(context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCommon.Data.Exceptions.TransactionHistoryException(BizCommon.Data.Exceptions.TransactionHistoryException.GET_TRANSACTION_LOCATION_FAILED, ex);
            }
        }
        public long GetCustomerSessionId(long transactionId, int transactionTypeValue, commonData.ZeoContext context)
        {
            long customerSessionId = 0;
            try
            {
                customerSessionId = trxService.GetCustomerSessionId(transactionId,transactionTypeValue, context);
                return customerSessionId;
            }

            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCommon.Data.Exceptions.TransactionHistoryException(BizCommon.Data.Exceptions.TransactionHistoryException.GET_CUSTOMER_TRANSACTION_HISTORY_FAILED, ex);
            }
        }
        #endregion
    }
}
