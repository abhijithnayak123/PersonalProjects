using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Contract
{
    public interface IBillPayService : IDisposable
    {
        long CreateBillPayTransaction(BillPay billPay, ZeoContext context);

        void UpdateBillPayTransaction(BillPay billPay, long channelPartnerId, ZeoContext context);

        void UpdateBillPayTransactionState(long transactionId, int state, string timeZone, ZeoContext context);

        void UpdatePreferredProductsAndState(long transactionId, int state, string timeZone, ZeoContext context);

        void UpdateBillPayTransactionFee(long transactionid, int state, decimal fee, decimal amount, string confirmationNumber, string timeZone, ZeoContext context);

        List<string> GetBillers(string term, int channelPartnerId, ZeoContext context);

        List<FavouriteBiller> GetFrequentBillers(long customerId, ZeoContext context);

        FavouriteBiller GetBillerDetails(string billerName, long customerId, int channelPartnerId, ZeoContext context);

        List<FavouriteBiller> DeleteFavouriteBiller(long productId, long customerId, string timeZone, ZeoContext context);

        long GetBillPayCxnTransactionId(long transactionId);

        void UpdateTransactionwithCXNID(long wuTrxId, BillPay billPay, ZeoContext context);
    }
}
