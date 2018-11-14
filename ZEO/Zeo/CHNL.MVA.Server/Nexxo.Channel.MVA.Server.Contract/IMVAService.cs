using MGI.Common.Util;
using System.Collections.Generic;
using System.ServiceModel;

namespace MGI.Channel.MVA.Server.Contract
{
    /// <summary>
    /// The MVA service for all Mobile Virtual Agent related functions.
    /// This is the entry point for all MVA ops.
    /// </summary>
    [ServiceContract]
    public interface IMVAService : ICustomerService, IMoneyTransferService, IBillPayService, IShoppingCartService, ITransactionHistoryService
    {
        void SetSelf(IMVAService dts);

        MGIContext GetPartnerContext(string channelPartnerName);

        MGIContext GetCustomerContext(long customerSessionId);
    }
}


















