using MGI.Channel.DMS.Server.Data;
using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using SharedData = MGI.Channel.Shared.Server.Data;
namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface ITransactionHistoryService
	{
        /// <summary>
        /// This method to get customer transaction history.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="customerId">This is unique identifier for customer</param>
        /// <param name="transactionType">the transaction type [Send Money, Money Order, Check Processing, BillPay,]</param>
        /// <param name="location">the location name</param>
        /// <param name="dateRange">this parameter to get transaction history with in date range</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Customer Transaction Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Response GetTransactionHistory(long customerSessionId, long customerId, string transactionType, string location, DateTime dateRange, MGIContext mgiContext);

        /// <summary>
        /// This method to get agent transaction history.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="agentId">This is unique identifier for agent</param>
        /// <param name="transactionType">the transaction type [Send Money, Money Order, Check Processing, BillPay,]</param>
        /// <param name="location">the location name</param>
        /// <param name="showAll">This parameter show all transaction in all status, by default only committed transaction will be shown</param>
        /// <param name="transactionId">This is unique identifier for Transaction</param>
        /// <param name="duration">This parameter gives the AgentTransactionHistoryDuration </param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Agent Transaction Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Response GetAgentTransactionHistory(long agentSessionId, long? agentId, string transactionType, string location, bool showAll, long transactionId, int duration, MGIContext mgiContext);

        /// <summary>
        /// This Method to Get fund transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Fund Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Fund Transaction Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Response GetFundTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This Method to Get check transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Check Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check Transaction Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Response GetCheckTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This Method to Get money transfer transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Money Transfer Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Money Transfer Transaction Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Response GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This Method to Get money order transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Money Order Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Money Order Transaction Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Response GetMoneyOrderTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This Method to Get billpay transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Billpay Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Billpay Transaction Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Response GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This Method to Get cash transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Cash Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Cash Transaction Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Response GetCashTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);
	}
}
