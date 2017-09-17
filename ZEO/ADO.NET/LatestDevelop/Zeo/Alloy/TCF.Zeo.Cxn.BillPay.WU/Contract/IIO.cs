using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.BillPay.Data;
using TCF.Zeo.Cxn.BillPay.WU.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.BillPay.WU.Contract
{
    public interface IIO
    {
        List<Location> GetLocations(string billerName, string accountNumber, decimal amount, BillPaymentRequest billPaymentRequest, ZeoContext context);

        Fee GetDeliveryMethods(long wuTrxId, string billerName, string accountNumber, decimal amount, Location location, BillPaymentRequest billPaymentRequest, ZeoContext context);

        WUTransaction ValidatePayment(long wuTrxId, BillPaymentRequest billPaymentRequest, ZeoContext context);

        WUTransaction MakePayment(BillPayTransactionRequest trx, ZeoContext context);

        List<Field> GetBillerFields(string billerName, string locationName, ZeoContext context);

        string GetBillerMessage(string billerName, ZeoContext context);
    }
}
