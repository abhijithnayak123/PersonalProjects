using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.CashEngine.Data;
using MGI.Common.Util;

namespace MGI.Biz.CashEngine.Contract
{
    public interface ICashEngine
    {
        /// <summary>
        /// Stages the cash-in transaction.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="amount">Transaction amount</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Cash In Transaction ID</returns>
		long CashIn(long customerSessionId, decimal amount, MGIContext mgiContext);

        /// <summary>
        /// Stage and commit the CashOut transaction
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="amount">Transaction amount</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
		long CashOut(long customerSessionId, decimal amount, MGIContext mgiContext);

        /// <summary>
        /// Commit the CashIn transaction.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="cxeTxnId">Cash In Transaction ID</param>       
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Cash In Transaction Status Committed</returns>
		int Commit(long customerSessionId, long cxeTxnId, MGIContext mgiContext);
        /// <summary>

		/// Cancel the CashIn transaction
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="cashInId"></param>
		/// <param name="mgiContext"></param>
		void Cancel(long customerSessionId, long cashInId, MGIContext mgiContext);

		/// AL-2729 user story
		/// <summary>
		/// Used for updating the CashIn transaction
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="amount">Amount to be updated</param>	
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Cash In Transaction ID</returns>
        long Update(long customerSessionId, long trxId, decimal amount, MGIContext mgiContext);
    }
}
