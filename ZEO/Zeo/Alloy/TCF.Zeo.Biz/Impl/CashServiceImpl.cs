using TCF.Zeo.Biz.Contract;
using TCF.Channel.Zeo.Data;
using Core = TCF.Zeo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;

namespace TCF.Zeo.Biz.Impl
{
    public class CashServiceImpl : ICashService
    {
        IMapper mapper;
        public CashServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Core.Data.CashTransaction, CashTransaction>();
            });

            mapper = config.CreateMapper();
        }
        Core.Contract.ICashService CashService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool CashIn(decimal amount, commonData.ZeoContext context)
        {
            try
            {
                CashService = new Core.Impl.CashServiceImpl();
                return CashService.CashIn(context.CustomerSessionId, amount, context.TimeZone, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CashServiceException(CashServiceException.CASHIN_FAILED, ex);
            }
        }

        public bool UpdateOrCancelCashIn(decimal amount, commonData.ZeoContext context)
        {
            try
            {
                CashService = new Core.Impl.CashServiceImpl();
                return CashService.UpdateOrCancelCash(context.CustomerSessionId, amount, context.TimeZone, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CashServiceException(CashServiceException.UPDATE_OR_CANCEL_CASHIN_FAILED, ex);
            }
        }

        public CashTransaction GetCashTransaction(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                CashService = new Core.Impl.CashServiceImpl();

                return mapper.Map<CashTransaction>(CashService.GetCashTransaction(transactionId, context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CashServiceException(CashServiceException.GET_CASHTRANSACTION_FAILED, ex);
            }
        }

        public bool RemoveCashIn(commonData.ZeoContext context)
        {
            try
            {
                CashService = new Core.Impl.CashServiceImpl();

                return CashService.RemoveCashIn(context.CustomerSessionId, context.TimeZone, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CashServiceException(CashServiceException.REMOVE_CASHIN_FAILED, ex);
            }

        }

    }
}
