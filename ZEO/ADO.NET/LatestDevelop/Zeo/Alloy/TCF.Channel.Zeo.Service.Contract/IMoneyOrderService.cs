using System.ServiceModel;
using System.Collections.Generic;
#region Zeo References
using TCF.Channel.Zeo.Data;
using ZeoData = TCF.Channel.Zeo.Data;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Service.Contract
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
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response GetMoneyOrderFee(decimal amount, ZeoData.ZeoContext context);

        /// <summary>
        /// Used for staging a money order transaction
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="moneyOrderPurchase">Information related to amount, fee and promotions</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Money order transaction details</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response PurchaseMoneyOrder(MoneyOrderPurchase moneyOrderPurchase, ZeoData.ZeoContext context);

        /// <summary>
        /// Used for updating the money order transaction
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="moneyOrderTransaction">Details of money order transaction</param>
        /// <param name="moneyOrderId">Transaction Id</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response UpdateMoneyOrder(MoneyOrder moneyOrderTransaction, ZeoData.ZeoContext context);

        /// <summary>
        /// Updates the money order status in CXE and partner
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="moneyOrderId">Transaction Id</param>
        /// <param name="newMoneyOrderStatus">Status of transaction(enum)</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response UpdateMoneyOrderStatus(long transactionId, int newMoneyOrderStatus, ZeoData.ZeoContext context);

        ///// <summary>
        ///// Used for fetching the money order transaction details
        ///// </summary>
        ///// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        ///// <param name="moneyOrderId">Transaction Id</param>
        ///// <param name="context">This is the common class parameter used to pass supplimental information</param>
        ///// <returns>Details of money order transaction</returns>
        //[FaultContract(typeof(AlloySoapFault))]
        //[OperationContract]
        //Response GetMoneyOrderStage(long moneyOrderId, ZeoData.AlloyContext context);

        /// <summary>
        /// Used for printing the check template in money order transaction
        /// </summary>
        /// <param name="moneyOrderId">Transaction Id</param>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Provides information about the position of details to be printed</returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GenerateCheckPrintForMoneyOrder(long transactionId, ZeoData.ZeoContext context);

        /// <summary>
        /// Used for printing the check template during money order diagnostics
        /// </summary>
        /// <param name="agentSessionId">Unique Id allocated at the time of AgentSession</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Provides information about the position of details to be printed</returns>
		[OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GenerateMoneyOrderDiagnostics(ZeoData.ZeoContext context);

        /// <summary>
        /// Used to commit the MO transaction from shopping Cart
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response Commit(long transactionId, ZeoData.ZeoContext context);
		
		 /// <summary>
        /// To get money order transaction
        /// </summary>
        /// <param name="transactionId">unique transaction id</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>To return moneyorder transaction details</returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetMoneyOrderTransaction(long transactionId, ZeoData.ZeoContext context);


    }
}
