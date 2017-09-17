using System;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IReceiptService
    {
        public Response GetBillpayReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            return serviceEngine.GetBillpayReceipt(transactionId, isReprint, context);
        }

        public Response GetCheckReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            return serviceEngine.GetCheckReceipt(transactionId, isReprint, context);
        }

        public Response GetCouponReceipt(long customerSessionId, ZeoContext context)
        {
            return serviceEngine.GetCouponReceipt(customerSessionId, context);
        }

        public Response GetDoddFranckRecipt(long transactionId, bool isReprint, ZeoContext context)
        {
            return serviceEngine.GetDoddFranckRecipt(transactionId, isReprint, context);
        }

        public Response GetFundReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            return serviceEngine.GetFundReceipt(transactionId, isReprint, context);
        }

        public Response GetMoneyOrderReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            return serviceEngine.GetMoneyOrderReceipt(transactionId, isReprint, context);
        }

        public Response GetMoneyTransferReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            return serviceEngine.GetMoneyTransferReceipt(transactionId, isReprint, context);
        }
        public Response GetSummaryReceipt(long customerSessionId, ZeoContext context)
        {
            return serviceEngine.GetSummaryReceipt(customerSessionId, context);
        }

        public Response GetCashDrawerReceipt(long agentId, long locationId, ZeoContext context)
        {
            return serviceEngine.GetCashDrawerReceipt(agentId, locationId, context);
        }
    }
}
