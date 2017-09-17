using MGI.Common.Monitor.DMSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Common.Monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.WriteLog("MGI common monitor started.");
            DeleteParkedShoppingCart();
            Logger.WriteLog("MGI common monitor completed.");
        }

        public static bool DeleteParkedShoppingCart()
        {
            bool isCompleted = false;
            DesktopServiceClient client = new DesktopServiceClient();
            Response response = client.GetAllParkedShoppingCartTransactions();
            List<ParkedTransaction> trxs = response.Result as List<ParkedTransaction>;
            Logger.WriteLog("Parked shoppingcart transactions : " + trxs.Count());
            foreach (var trx in trxs)
            {
                try
                {
                    switch (trx.TransactionType)
                    {
                        case "BillPay":
                            Logger.WriteLog("Parked transaction id: " + trx.TransactionId + ", customer session: " + trx.CustomerSessionId + ", transaction type: " + trx.TransactionType);
                            client.RemoveBillPay(trx.CustomerSessionId, trx.TransactionId, true, null);
                            break;
                        case "MoneyTransfer":
                            Logger.WriteLog("Parked transaction id: " + trx.TransactionId + ", customer session: " + trx.CustomerSessionId + ", transaction type: " + trx.TransactionType);
                            client.RemoveMoneyTransfer(trx.CustomerSessionId, trx.TransactionId, true, null);
                            break;
                        case "Check":
                            Logger.WriteLog("Parked transaction id: " + trx.TransactionId + ", customer session: " + trx.CustomerSessionId + ", transaction type: " + trx.TransactionType);
                            client.RemoveCheck(trx.CustomerSessionId, trx.TransactionId, true, null);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Logger.WriteLog("Exception : " + ex.ToString());
                }
            }
            isCompleted = true;
            return isCompleted;
        }
    }
}
