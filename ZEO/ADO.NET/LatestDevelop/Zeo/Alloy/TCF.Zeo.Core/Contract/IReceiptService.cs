using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface IReceiptService
    {
        BillpayReceiptData GetBillpayReceiptData(long transactionId, string transactionType, int provider, bool isReprint, ZeoContext context);

        ProcessCheckReceiptData GetCheckReceiptData(long transactionId, ZeoContext context);

        FundReceiptData GetFundReceiptData(long transactionId, ZeoContext context);

        MoneyTransferReceiptData GetMoneyTransferReceiptData(long transactionId, string transactionType, int provider, ZeoContext context, bool? isReprint=null );

        CouponReceiptData GetCouponReceiptData(long transactionId, ZeoContext context);

        MoneyOrderReceiptData GetMoneyOrderReceiptData(long transactionId, ZeoContext context);

        ShoppingCartSummeryReceiptData GetShoppingCartReceiptData(long customerSessionId, ZeoContext context);

        CashDrawerReceiptData GetCashDrawerReceiptData(long agentId, long locationId, ZeoContext context);

        String GetReceiptTemplate(List<string> receiptNames);

        String GetStringReceiptTemplate(List<string> receiptNames);
    }
}
