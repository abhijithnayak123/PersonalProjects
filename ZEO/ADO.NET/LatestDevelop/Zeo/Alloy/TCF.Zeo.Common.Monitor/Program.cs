using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Common.Monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Logger.WriteLog("Zeo common monitor started.");
                DeleteParkedShoppingCart();
                Logger.WriteLog("Zeo common monitor completed.");
            }
            catch (Exception  ex)
            {
                Logger.WriteLog("Exception : " + ex.ToString());
            }
        }

        public static void DeleteParkedShoppingCart()
        {
            int MoneyTransfer = 1;
            ZeoService.ZeoServiceClient client = new ZeoService.ZeoServiceClient();
            ZeoService.ZeoContext context = new ZeoService.ZeoContext();
            ZeoService.Response response = client.GetAllParkedTransaction(context);
            List<ZeoService.ParkedTransaction> trxs = response.Result as List<ZeoService.ParkedTransaction>;

            Logger.WriteLog("Parked Shoppingcart Transactions : " + trxs.Count());

            List<ZeoService.CheckLogin> chexarSessions = new List<ZeoService.CheckLogin>();
            if (trxs != null && trxs.FindAll(x => x.ProductId == "ProcessCheck").Count > 0)
            {
                chexarSessions = GetChexarSessionForChannelPartners(trxs);
            }

            foreach (var trx in trxs)
            {
                try
                {
                    switch (trx.ProductId)
                    {
                        case "BillPayment":
                            Logger.WriteLog("Parked transaction id: " + trx.TransactionId + ", customer session: " + trx.CustomerSessionId + ", transaction type: " + trx.ProductId);
                            context = GetContext(trx);
                            client.RemoveBillPay(trx.TransactionId, context);
                            break;
                        case "MoneyTransfer":
                            Logger.WriteLog("Parked transaction id: " + trx.TransactionId + ", customer session: " + trx.CustomerSessionId + ", transaction type: " + trx.ProductId);
                            context = GetContext(trx);
                            // As receive money cannot be oarked we are hard coding product type to 1 - Money Transfer
                            client.RemoveMoneyTransfer(trx.TransactionId, MoneyTransfer, context);
                            break;
                        case "ProcessCheck":
                            Logger.WriteLog("Parked transaction id: " + trx.TransactionId + ", customer session: " + trx.CustomerSessionId + ", transaction type: " + trx.ProductId);
                            context = GetProviderAlloyContext(chexarSessions, trx);
                            client.RemoveCheck(trx.TransactionId, context);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("Exception : " + ex.ToString());
                }
            }
        }

        private static List<ZeoService.CheckLogin> GetChexarSessionForChannelPartners(List<ZeoService.ParkedTransaction> parkedTrxs)
        {
            ZeoService.ZeoServiceClient client = new ZeoService.ZeoServiceClient();
            // Get All the channel partners from the pending checks
            List<ZeoService.ParkedTransaction> channelPartnerCredintials =
                parkedTrxs.GroupBy(i => new { i.ChannelPartnerId, i.LocationID }).Select(grp => grp.First()).ToList();


            List<ZeoService.CheckLogin> chxrSessions = new List<ZeoService.CheckLogin>();
            ZeoService.ZeoContext context = new ZeoService.ZeoContext();

            foreach (ZeoService.ParkedTransaction item in channelPartnerCredintials)
            {
                context.ChannelPartnerId = item.ChannelPartnerId;
                context.CheckUserName = item.CheckUserName;
                context.CheckPassword = item.CheckPassword;
                context.TimeZone = item.TimeZone;
                context.ChannelPartnerName = item.ChannelPartnerName;

                ZeoService.Response response = client.GetCheckSession(context);
                ZeoService.CheckLogin checkLogin = response.Result as ZeoService.CheckLogin;
                if (checkLogin != null)
                {
                    checkLogin.LocationIdentifier = item.LocationID.ToString();
                    chxrSessions.Add(checkLogin);
                }
            }

            return chxrSessions;
        }

        private static ZeoService.ZeoContext GetProviderAlloyContext(List<ZeoService.CheckLogin> chxrSessions, ZeoService.ParkedTransaction parkedTrx)
        {
            ZeoService.CheckLogin chxrLogin = chxrSessions.Find(i => i.ChannelPartnerId == parkedTrx.ChannelPartnerId && i.LocationIdentifier == parkedTrx.LocationID.ToString());
            return new ZeoService.ZeoContext
            {
                URL = chxrLogin.URL,
                CompanyToken = chxrLogin.CompanyToken,
                IngoBranchId = chxrLogin.BranchId,
                EmployeeId = chxrLogin.EmployeeId,
                ChannelPartnerId = chxrLogin.ChannelPartnerId,
                CustomerSessionId = parkedTrx.CustomerSessionId,
                TimeZone = parkedTrx.TimeZone,
                ChannelPartnerName = parkedTrx.ChannelPartnerName,
                CustomerId = parkedTrx.CustomerId
            };
        }

        private static ZeoService.ZeoContext GetContext(ZeoService.ParkedTransaction trx)
        {
            return new ZeoService.ZeoContext
            {
                ChannelPartnerId = trx.ChannelPartnerId,
                CustomerSessionId = trx.CustomerSessionId,
                TimeZone = trx.TimeZone,
                ChannelPartnerName = trx.ChannelPartnerName,
                CustomerId = trx.CustomerId,
                AgentFirstName = trx.AgentFirstName,
                AgentLastName = trx.AgentLastName,
                AgentName = trx.AgentName,
                AgentId = trx.AgentId,
                WUCounterId = trx.WUCounterId,
            };
        }
    }
}
