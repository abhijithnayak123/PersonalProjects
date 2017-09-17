using System;
using System.ServiceModel;


namespace MGI.Channel.DMS.Server.Contract
{
	/// <summary>
	/// The Desktop service for all desktop related functions.
	/// This is the entry point for all Desktop ops.
	/// </summary>
	[ServiceContract]
	public interface IDesktopService : ICustomerService, IAgentService, IDataStructuresService, IChannelPartnerService, ICheckCashingService,
        ICashService, IReportService, IShoppingCartService, IBillPayService, IMoneyTransferService, IMoneyTransferSetupService, IFundsProcessorService,
        IUserService, ILocationService, INpsTerminalService, ITerminalService, ITransactionHistoryService, IReceiptsService, IMoneyOrderService, IMessageCenterService, ILocationCounterIdService, ILocationProcessorCredentialsService
    {
	}
}
//, INLogLoggerService

