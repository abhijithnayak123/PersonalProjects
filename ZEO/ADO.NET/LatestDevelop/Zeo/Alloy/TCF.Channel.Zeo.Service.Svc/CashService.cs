using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;
using System;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : ICashService
    {
        public Response CashIn(decimal amount, ZeoContext context)
        {
            return serviceEngine.CashIn(amount, context);
        }

        public Response GetCashTransaction(long transactionId, ZeoContext context)
        {
            return serviceEngine.GetCashTransaction(transactionId,context);
        }

        public Response UpdateOrCancelCashIn(decimal amount, ZeoContext context)
        {
            return serviceEngine.UpdateOrCancelCashIn(amount, context);
        }
    }
}