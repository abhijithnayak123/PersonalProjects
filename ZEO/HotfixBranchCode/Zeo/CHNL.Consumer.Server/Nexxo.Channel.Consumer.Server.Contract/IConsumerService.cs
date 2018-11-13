using System.Collections.Generic;
using System.ServiceModel;

namespace MGI.Channel.Consumer.Server.Contract
{
    /// <summary>
    /// The MVA service for all Mobile Virtual Agent related functions.
    /// This is the entry point for all MVA ops.
    /// </summary>
    [ServiceContract]
    public interface IConsumerService : ICustomerService, IMoneyTransferService, IFundsProcessorService, IShoppingCartService, IBillPayService, ITransactionHistoryService
    {
        void SetSelf(IConsumerService dts);
    }
}
