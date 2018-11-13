using System.ServiceModel;
using System.Collections.Generic;
  
// Nexxo deps
using MGI.Common.Sys;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Contract
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IFundsProcessorService
    {

		long WithdrawFunds(long customerSessionId, Funds funds, MGIContext mgiContext);

		FundsProcessorAccount LookupFundsAccount(long customerSessionId, MGIContext mgiContext);

		TransactionFee GetFundsFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext mgiContext);

    }
}