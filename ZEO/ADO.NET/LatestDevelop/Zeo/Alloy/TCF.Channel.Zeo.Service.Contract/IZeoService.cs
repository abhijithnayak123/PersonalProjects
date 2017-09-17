using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;


namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IZeoService : ICustomerService, IDataStructuresService, IChannelPartnerService, IZeoContext, ICheckService, 
        IMessageCenterService, IMoneyOrderService, IShoppingCartService, IFundsProcessorService, IMoneyTransferService, IBillPayService,ITransactionService,
        ILocationCounterIdService,ILocationProcessorCredentialsService,ILocationService, ICashService, IReceiptService,IAgentService,ITerminalService,INpsTerminalService, IMessageStoreService
    {

    }
}
