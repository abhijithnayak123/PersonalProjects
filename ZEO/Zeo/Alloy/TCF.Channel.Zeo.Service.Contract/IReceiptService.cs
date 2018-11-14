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
    public interface IReceiptService
    {
        [OperationContract]
        Response GetBillpayReceipt(long transactionId, bool isReprint,ZeoContext context);

        [OperationContract]
        Response GetCheckReceipt(long transactionId, bool isReprint, ZeoContext context);

        [OperationContract]
        Response GetCouponReceipt(long customerSessionId, ZeoContext context);

        [OperationContract]
        Response GetMoneyTransferReceipt(long transactionId, bool isReprint, ZeoContext context);

        [OperationContract]
        Response GetDoddFranckRecipt(long transactionId, bool isReprint, ZeoContext context);

        [OperationContract]
        Response GetMoneyOrderReceipt(long transactionId, bool isReprint, ZeoContext context);

        [OperationContract]
        Response GetFundReceipt(long transactionId, bool isReprint, ZeoContext context);

        [OperationContract]
        Response GetSummaryReceipt(long customerSessionId, ZeoContext context);

        [OperationContract]
        Response GetCashDrawerReceipt(long agentId, long locationId, ZeoContext context);
    }
}
