using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using System.Collections.Generic;

namespace MGI.Channel.DMS.Server.Contract
{
    [ServiceContract]
    public interface IMoneyOrderService
    {
        /// <summary>
        /// Used for fetching details of fee and applicable discounts
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="moneyOrder">Information related to amount and promotions</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Fee and discount details</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		Response GetMoneyOrderFee(long customerSessionId, MoneyOrderData moneyOrder, MGIContext mgiContext);

        /// <summary>
        /// Used for staging a money order transaction
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="moneyOrderPurchase">Information related to amount, fee and promotions</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Money order transaction details</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
        Response PurchaseMoneyOrder(long customerSessionId, MoneyOrderPurchase moneyOrderPurchase, MGIContext mgiContext);

        /// <summary>
        /// Cancel the money order transaction and remove from the shopping cart
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="moneyOrderId">Transaction Id</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
        Response CancelMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

        /// <summary>
        /// Used for updating the money order transaction
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="moneyOrderTransaction">Details of money order transaction</param>
        /// <param name="moneyOrderId">Transaction Id</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		Response UpdateMoneyOrder(long customerSessionId, MoneyOrderTransaction moneyOrderTransaction, long moneyOrderId, MGIContext mgiContext);

        /// <summary>
        /// Updates the money order status in CXE and partner
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="moneyOrderId">Transaction Id</param>
        /// <param name="newMoneyOrderStatus">Status of transaction(enum)</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		Response UpdateMoneyOrderStatus(long customerSessionId, long moneyOrderId, int newMoneyOrderStatus, MGIContext mgiContext);

        /// <summary>
        /// Used for fetching the money order transaction details
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="moneyOrderId">Transaction Id</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Details of money order transaction</returns>
        [FaultContract(typeof (NexxoSOAPFault))]
        [OperationContract]
		Response GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

        /// <summary>
        /// Used for printing the check template in money order transaction
        /// </summary>
        /// <param name="moneyOrderId">Transaction Id</param>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Provides information about the position of details to be printed</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GenerateCheckPrintForMoneyOrder(long moneyOrderId, long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// Used for printing the check template during money order diagnostics
        /// </summary>
        /// <param name="agentSessionId">Unique Id allocated at the time of AgentSession</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Provides information about the position of details to be printed</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GenerateMoneyOrderDiagnostics(long agentSessionId, MGIContext mgiContext);
    }
}
