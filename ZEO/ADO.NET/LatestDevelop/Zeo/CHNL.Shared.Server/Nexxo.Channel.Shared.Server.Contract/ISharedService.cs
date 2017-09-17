using System.Collections.Generic;
using System.ServiceModel;

namespace MGI.Channel.Shared.Server.Contract
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface ISharedService : IMoneyTransferSetupService, IMoneyTransferService, ICustomerService,
        IBillPayService, IFundsProcessorService, ICheckCashingService, IMoneyOrderService, ICashService, IShoppingCartService, 
        ITransactionHistoryService
    {
        void SetSelf(ISharedService dts);
    }
}
