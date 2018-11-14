using TCF.Channel.Zeo.Service.Contract;
using Biz = TCF.Zeo.Biz;
using TCF.Channel.Zeo.Data;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;
using System;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : ICashService
    {
        Biz.Contract.ICashService CashService;
        public Response CashIn(decimal amount, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();
                CashService = new Biz.Impl.CashServiceImpl();

                response.Result = CashService.CashIn(amount, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GetCashTransaction(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CashService = new Biz.Impl.CashServiceImpl();

            response.Result = CashService.GetCashTransaction(transactionId, commonContext);
            return response;
        }

        public Response UpdateOrCancelCashIn(decimal amount, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();
                CashService = new Biz.Impl.CashServiceImpl();

                response.Result = CashService.UpdateOrCancelCashIn(amount, commonContext);

                scope.Complete();

                return response;
            }
        }
    }
}
