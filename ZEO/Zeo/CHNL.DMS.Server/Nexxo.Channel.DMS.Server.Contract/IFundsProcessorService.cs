// -----------------------------------------------------------------------
// <copyright file="IFundsProcessorService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Channel.DMS.Server.Contract
{
	using System.ServiceModel;
	using System.Collections.Generic;

	// Nexxo deps
	using MGI.Common.Sys;
	using MGI.Channel.Shared.Server.Data;
	using MGI.Channel.DMS.Server.Data;
	using System;

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
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Unique Id associated with account</returns>
		[OperationContract(Name = "AddFundsAccount")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response Add(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext);

		/// <summary>
		/// Used for validating the card number and account associated with it
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="cardNumber">GPR card has a unique number used for transactions</param>
		/// <param name="pin">This parameter is not being used and should be removed</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Updates the status of card</returns>
		[OperationContract(Name = "AuthenticateCard")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response AuthenticateCard(long customerSessionId, string cardNumber, string pin, MGIContext mgiContext);

		/// <summary>
		/// Stages the withdraw transaction
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="funds">Contains information related to fund transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Transaction Id used for making the withdraw transaction</returns>
		[OperationContract(Name = "WithdrawFunds")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response Withdraw(long customerSessionId, Funds funds, MGIContext mgiContext);

		/// <summary>
		/// Stages the load transaction
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="funds">Contains information related to fund transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Transaction Id used for making the load transaction</returns>
		[OperationContract(Name = "LoadFunds")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response Load(long customerSessionId, Funds funds, MGIContext mgiContext);

		/// <summary>
		/// Used for staging an activation of a card
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="funds">Contains information related to fund transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Unique Id associated with fund transaction</returns>
		[OperationContract(Name = "ActivateGPRCard")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response ActivateGPRCard(long customerSessionId, Funds funds, MGIContext mgiContext);

		/// <summary>
		/// Used for getting the balance amount associated with the GPR card
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Balance amount present in the GPR card</returns>
		[OperationContract(Name = "GetFundsBalance")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetBalance(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// Used for fetching the card customer details  
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Account details of the card customer</returns>
		[OperationContract(Name = "LookupForPANFund")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response LookupForPAN(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// Used for fetching details of fee and applicable discounts
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="amount">Load/Withdraw amount in fund transaction</param>
		/// <param name="fundsType">Type of fund transaction</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Fee and discount details</returns>
		[OperationContract(Name = "GetFeeForFunds")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext context);

		/// <summary>
		/// Used for updating the amount during a transaction
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="cxeFundTrxId">Unique Id associated with fund transaction</param>
		/// <param name="amount">Load/Withdraw amount in fund transaction</param>
		/// <param name="fundType">Type of fund transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Unique transactionId associated with each transaction</returns>
		[OperationContract(Name = "UpdateFundAmount")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response UpdateFundAmount(long customerSessionId, long cxeFundTrxId, decimal amount, FundType fundType, MGIContext mgiContext);

		/// <summary>
		/// Used for getting the minimum amount required during activation and load
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="initialLoad">Flag to determine the load is during activation or transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Minimum amount required for a particular transaction</returns>
		[OperationContract(Name = "GetMinimumLoadAmount")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetMinimumLoadAmount(long customerSessionId, bool initialLoad, MGIContext mgiContext);

		[OperationContract(Name = "GetCardTransactionHistory")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetCardTransactionHistory(long customerSessionId, TransactionHistoryRequest request, MGIContext mgiContext);

		[OperationContract(Name = "CloseAccount")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response CloseAccount(long customerSessionId, MGIContext mgiContext);

		[OperationContract(Name = "UpdateCardStatus")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response UpdateCardStatus(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		[OperationContract(Name = "ReplaceCard")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response ReplaceCard(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		[OperationContract(Name = "GetShippingTypes")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetShippingTypes(long customerSessionId, MGIContext mgiContext);

		[OperationContract(Name = "GetShippingFee")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetShippingFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		/// <summary>
		/// Method to associate card with alloy customer
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		///<param name="fundsType">Type of fund transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns></returns>
		[OperationContract(Name = "AssociateCard")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response AssociateCard(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext);

		[OperationContract(Name = "GetFundFee")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetFundFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		/// <summary>
		/// Method to Issue AddOn Card
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="funds">Contains information related to fund transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Unique Id associated with fund transaction</returns>
		[OperationContract(Name = "IssueAddOnCard")]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response IssueAddOnCard(long customerSessionId, Funds funds, MGIContext mgiContext);
	}
}
