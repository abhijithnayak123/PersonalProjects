using System;
using System.ServiceModel;
using System.Collections.Generic;

using MGI.Common.Sys;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface ICashService
	{
		/// <summary>
		/// Stages the cash-in transaction.
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="amount">Transaction amount</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Cash In Transaction ID</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
        long CashIn(long customerSessionId, decimal amount, MGIContext mgiContext);

		

	    ///AL-2729 user story for updating cash in transaction
		/// <summary>
		/// Used for updating the cash in transaction
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="amount">Amount to be updated</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Cash In Transaction ID</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		long UpdateCash(long customerSessionId, long trxId, decimal amount,  MGIContext mgiContext);
	}
}
