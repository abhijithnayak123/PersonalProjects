using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : ICheckService
    {
        public Response GetCheckTypes(ZeoContext context)
        {

            return serviceEngine.GetCheckTypes(context);
        }

        public Response CancelCheck(long transactionId, ZeoContext context)
        {
            return serviceEngine.CancelCheck(transactionId, context);
        }

        public Response GetCheckFee(CheckSubmission checkSubmit, ZeoContext context)
        {
            return serviceEngine.GetCheckFee(checkSubmit, context);
        }

        public Response GetCheckFrankingData(long transactionId, ZeoContext context)
        {
            return serviceEngine.GetCheckFrankingData(transactionId, context);
        }

        public Response GetCheckProcessorInfo(ZeoContext context)
        {
            return serviceEngine.GetCheckProcessorInfo(context);
        }

        public Response GetCheckSession(ZeoContext context)
        {
            return serviceEngine.GetCheckSession(context);
        }

        public Response GetCheckStatus(long transactionId, ZeoContext context)
        {
            return serviceEngine.GetCheckStatus(transactionId, context);
        }

        public Response GetCheckTranasactionDetails(long transactionId, ZeoContext context)
        {
            return serviceEngine.GetCheckTranasactionDetails(transactionId, context);
        }

        public Response SubmitCheck(CheckSubmission check, ZeoContext context)
        {
            return serviceEngine.SubmitCheck(check, context);
        }

        public Response UpdateCheckTransactionFranked(long transactionId, ZeoContext context)
        {
            return serviceEngine.UpdateCheckTransactionFranked(transactionId, context);
        }

        public Response CanResubmit(long transactionId, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        public Response GetFeeBasedOnPromoCode(CheckSubmission checkSubmit, ZeoContext context)
        {
            return serviceEngine.GetFeeBasedOnPromoCode(checkSubmit, context);
        }

   	public Response GetCheckProvider(MICRDetails micrDetails, ZeoContext context)
        {
            return serviceEngine.GetCheckProvider(micrDetails, context);
        }
    }
}
