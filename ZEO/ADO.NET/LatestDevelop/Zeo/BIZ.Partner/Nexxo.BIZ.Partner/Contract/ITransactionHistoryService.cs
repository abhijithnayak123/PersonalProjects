using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Data.Transactions;
using MGI.Common.Util;
using System;
using System.Collections.Generic;

namespace MGI.Biz.Partner.Contract
{
	public interface ITransactionHistoryService
	{
        /// <summary>
        /// This method to fetch customer transaction history.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="customerId">This is unique identifier for customer</param>
        /// <param name="transactionType">the transaction type [Send Money, Money Order, Check Processing, BillPay,]</param>
        /// <param name="location">the location name</param>
        /// <param name="dateRange">this parameter to get transaction history with in date range</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Customer Transaction Details</returns>
        List<TransactionHistory> GetTransactionHistory(long customerSessionId, long customerId, string transactionType, string location, DateTime dateRange, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch agent transaction history.
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
        List<TransactionHistory> GetTransactionHistory(long agentSessionId, long? agentId, string transactionType, string location, bool showAll, long transactionId, int duration, MGIContext mgiContext);

        /// <summary>
        /// This Method to fetch fund transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Fund Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Fund Transaction Details</returns>
        FundTransaction GetFundTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This Method to fetch check transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Check Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check Transaction Details</returns>
        CheckTransaction GetCheckTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This Method to fetch money transfer transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Money Transfer Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Money Transfer Transaction Details</returns>
        MoneyTransferTransaction GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This Method to fetch money order transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Money Order Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Money Order Transaction Details</returns>
        MoneyOrderTransaction GetMoneyOrderTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This Method to fetch billpay transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Billpay Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Billpay Transaction Details</returns>
        BillPayTransaction GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This method is used for getting past transactions
        /// </summary>
        /// <param name="agentSessionId">unique identifier for agent session</param>
        /// <param name="customerSessionId">unique identifier for customer session</param>
        /// <param name="customerId">unique identifier for customer</param>
        /// <param name="transactionType">transaction type</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>list of past transactions</returns>
        List<PastTransaction> GetPastTransactions(long agentSessionId, long customerSessionId, long customerId, string transactionType, MGIContext mgiContext);

        /// <summary>
        /// This Method to fetch cash transaction details based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Cash Transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Cash Transaction Details</returns>
        CashTransaction GetCashTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);
	}
}
