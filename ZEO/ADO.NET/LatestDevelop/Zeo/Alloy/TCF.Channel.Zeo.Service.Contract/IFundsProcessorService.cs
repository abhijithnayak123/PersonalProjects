// -----------------------------------------------------------------------
// <copyright file="IFundsProcessorService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TCF.Channel.Zeo.Service.Contract
{
    using TCF.Zeo.Common.Data;
    using System.ServiceModel;
    using TCF.Channel.Zeo.Data;
    using TCF.Zeo.Common.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ServiceContract]
    public interface IFundsProcessorService
    {
        /// <summary>
        /// Used for adding a GPR account to the customer
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="fundsAccount">Contains information related to card</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Unique Id associated with account</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "AddFundsAccount")]
        Response Add(FundsAccount fundsAccount, Data.ZeoContext context);

        /// <summary>
        /// Stages the withdraw transaction
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making the withdraw transaction</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "WithdrawFunds")]
        Response Withdraw(Funds funds, Data.ZeoContext context);

        /// <summary>
        /// Stages the load transaction
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making the load transaction</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "LoadFunds")]
        Response Load(Funds funds, Data.ZeoContext context);

        /// <summary>
        /// Used for staging an activation of a card
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Unique Id associated with fund transaction</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "ActivateGPRCard")]
        Response ActivateGPRCard(Funds funds, Data.ZeoContext context);

        /// <summary>
        /// Used for getting the balance amount associated with the GPR card
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Balance amount present in the GPR card</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "GetCardBalance")]
        Response GetBalance(Data.ZeoContext context);

        /// <summary>
        /// Used for fetching the card customer details  
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Account details of the card customer</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "LookupForPANFund")]
        Response LookupForPAN(Data.ZeoContext context);

        /// <summary>
        /// Used for updating the amount during a transaction
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="cxeFundTrxId">Unique Id associated with fund transaction</param>
        /// <param name="amount">Load/Withdraw amount in fund transaction</param>
        /// <param name="fundType">Type of fund transaction</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Unique transactionId associated with each transaction</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "UpdateFundAmount")]
        Response UpdateFundAmount(long cxnFundTrxId, decimal amount, Helper.FundType fundType, Data.ZeoContext context);

        /// <summary>
        /// Used for getting the minimum amount required during activation and load
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="initialLoad">Flag to determine the load is during activation or transaction</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Minimum amount required for a particular transaction</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "GetMinimumLoadAmount")]
        Response GetMinimumLoadAmount(bool initialLoad, Data.ZeoContext context);

        /// <summary>
        /// Method to return card transaction's history
        /// </summary>
        /// <param name="request">Search Criteria</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "GetCardTransactionHistory")]
        Response GetCardTransactionHistory(TransactionHistoryRequest request, Data.ZeoContext context);

        /// <summary>
        /// Method to close the existing card account at Provider
        /// </summary>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "CloseAccount")]
        Response CloseAccount(Data.ZeoContext context);

        /// <summary>
        /// Method to update the card status with provider
        /// </summary>
        /// <param name="cardMaintenanceInfo">Card related information</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "UpdateCardStatus")]
        Response UpdateCardStatus(CardMaintenanceInfo cardMaintenanceInfo, Data.ZeoContext context);

        /// <summary>
        /// Method to request the card replace to the provider
        /// </summary>
        /// <param name="cardMaintenanceInfo">card realted information</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "ReplaceCard")]
        Response ReplaceCard(CardMaintenanceInfo cardMaintenanceInfo, Data.ZeoContext context);

        /// <summary>
        /// Method to return the available shipping types for card replace
        /// </summary>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "GetShippingTypes")]
        Response GetShippingTypes(Data.ZeoContext context);

        /// <summary>
        /// Method to return the shipping fee for each shipping type
        /// </summary>
        /// <param name="cardMaintenanceInfo"></param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns></returns>
        [OperationContract(Name = "GetShippingFee")]
        Response GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, Data.ZeoContext context);

        /// <summary>
        /// Method to associate card with alloy customer
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        ///<param name="fundsType">Type of fund transaction</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "AssociateCard")]
        Response AssociateCard(FundsAccount fundsAccount, Data.ZeoContext context);

        /// <summary>
        /// Method to return the replacement fee 
        /// </summary>
        /// <param name="cardMaintenanceInfo">card realted information</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "GetFundFee")]
        Response GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, Data.ZeoContext context);

        /// <summary>
        /// Method to Issue AddOn Card
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Unique Id associated with fund transaction</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract(Name = "IssueAddOnCard")]
        Response IssueAddOnCard(Funds funds, Data.ZeoContext context);

        /// <summary>
        /// Method to Get Prepaid Actions
        /// </summary>
        /// <param name="cardStatus">Contains information related to card status</param>
        /// <returns>Unique Id associated with fund transaction</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response GetPrepaidActions(string cardStatus, Data.ZeoContext context);

        /// <summary>
        /// Method to return the fund transaction
        /// </summary>
        /// <param name="transactionId">transaction uniqueId</param>
        /// <param name="context">This is the common parameter used to pass supplimental information</param>
        /// <returns>Fund transaction</returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response GetFundTransaction(long transactionId, bool isEditTransaction, Data.ZeoContext context);


		//ALM-5234,ALM-5235
        /// <summary>
        /// Updates the funds account to old values when removing the activate transaction from Shopping cart for the closed customers.
        /// </summary>
        /// <param name="fundsAccount">Funds Account</param>
        /// <param name="context"></param>
        /// <returns></returns>
        [FaultContract(typeof(ZeoSoapFault))]
        [OperationContract]
        Response UpdateGPRAccount(FundsAccount fundsAccount, Data.ZeoContext context);
    }
}
