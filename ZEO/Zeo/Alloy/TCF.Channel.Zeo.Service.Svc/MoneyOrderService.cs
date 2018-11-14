using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;
using ZeoData = TCF.Channel.Zeo.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IMoneyOrderService
    {
        public Response Commit(long transactionId, ZeoContext context)
        {
            return serviceEngine.Commit(transactionId, context);
        }

        public Response GenerateCheckPrintForMoneyOrder(long transactionId, ZeoData.ZeoContext context)
        {
            return serviceEngine.GenerateCheckPrintForMoneyOrder(transactionId, context);
        }

        public Response GenerateMoneyOrderDiagnostics(ZeoData.ZeoContext context)
        {
            return serviceEngine.GenerateMoneyOrderDiagnostics(context);
        }

        public Response GetFeeBOnPromoCode(decimal amount, string promoCode, ZeoContext context)
        {
            return serviceEngine.GetFeeBOnPromoCode(amount, promoCode, context);
        }

        public Response GetMoneyOrderFee(decimal amount, ZeoData.ZeoContext context)
        {
            return serviceEngine.GetMoneyOrderFee(amount, context);
        }

        public Response PurchaseMoneyOrder(MoneyOrderPurchase moneyOrderPurchase, ZeoData.ZeoContext context)
        {
            return serviceEngine.PurchaseMoneyOrder(moneyOrderPurchase, context);
        }

        public Response UpdateMoneyOrder(MoneyOrder moneyOrderTransaction, ZeoData.ZeoContext context)
        {
            return serviceEngine.UpdateMoneyOrder(moneyOrderTransaction, context);
        }

        public Response UpdateMoneyOrderStatus(long transactionId, int newMoneyOrderStatus, ZeoData.ZeoContext context)
        {
            return serviceEngine.UpdateMoneyOrderStatus(transactionId, newMoneyOrderStatus, context);
        }
    }
}