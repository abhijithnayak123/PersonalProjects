using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{
    public partial class DesktopWSImpl : IMoneyOrderService
    {

        public TransactionFee GetMoneyOrderFee(long customerSessionId, MoneyOrderData moneyOrder, MGIContext mgiContext)
        {
            return DesktopEngine.GetMoneyOrderFee(customerSessionId, moneyOrder, mgiContext);
        }

        public MoneyOrder PurchaseMoneyOrder(long customerSessionId, MoneyOrderPurchase moneyOrderPurchase, MGIContext mgiContext)
        {
            return DesktopEngine.PurchaseMoneyOrder(customerSessionId, moneyOrderPurchase, mgiContext);
        }

        public void CancelMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
        {
            DesktopEngine.CancelMoneyOrder(customerSessionId, moneyOrderId, mgiContext);
        }

        public void UpdateMoneyOrder(long customerSessionId, MoneyOrderTransaction moneyOrderTransaction, long moneyOrderId, MGIContext mgiContext)
        {
			DesktopEngine.UpdateMoneyOrder(customerSessionId, moneyOrderTransaction, moneyOrderId, mgiContext);
        }

        public void UpdateMoneyOrderStatus(long customerSessionId, long moneyOrderId, int newMoneyOrderStatus, MGIContext mgiContext)
        {
            DesktopEngine.UpdateMoneyOrderStatus(customerSessionId, moneyOrderId, newMoneyOrderStatus, mgiContext);
        }

        public MoneyOrder GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
        {
            return DesktopEngine.GetMoneyOrderStage(customerSessionId, moneyOrderId, mgiContext);
        }

        public CheckPrint GenerateCheckPrintForMoneyOrder(long moneyOrderId, long customerSessionId, MGIContext mgiContext)
        {
            return DesktopEngine.GenerateCheckPrintForMoneyOrder(moneyOrderId, customerSessionId, mgiContext);
        }

        public CheckPrint GenerateMoneyOrderDiagnostics(long agentSessionId, MGIContext mgiContext)
		{
            return DesktopEngine.GenerateMoneyOrderDiagnostics(agentSessionId, mgiContext);
		}
	}
}