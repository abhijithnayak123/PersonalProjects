using System.ServiceModel;
using System.Collections.Generic;
  
// Nexxo deps
using MGI.Common.Sys;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.Consumer.Server.Contract
{
    public interface IFundsProcessorService
    {
        /// <summary>
        /// Gets Fee for a particular type of funds transaction
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="amount">Load/Withdraw amount in fund transaction</param>
        /// <param name="fundsType">Type of fund transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Fee and discount details</returns>
		TransactionFee GetFundsFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext mgiContext);

        /// <summary>
        /// Stages the withdraw transaction
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making withdraw transaction</returns>
		long WithdrawFunds(long customerSessionId, Funds funds, MGIContext mgiContext);

        /// <summary>
        /// Used for fetching the card customer details  
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Account details of the card customer</returns>
		FundsProcessorAccount LookupFundsAccount(long customerSessionId, MGIContext mgiContext);

    }
}