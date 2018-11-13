using System.Collections.Generic;
using MGI.Cxn.Check.Data;
using MGI.Cxn.Common.Processor.Contract;
using MGI.Common.Util;



namespace MGI.Cxn.Check.Contract
{
	public interface ICheckProcessor : IProcessor
	{
        /// <summary>
        /// This method to submit the check information to processor for validation.
        /// </summary>
        /// <param name="trxId">This is unique identifier for check transaction CheckId </param>
        /// <param name="accountId">This is unique identifier for check account</param>
        /// <param name="check">This contents the check details</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>The check transaction status[Pending/Approval/Decline etc.] </returns>
		CheckStatus Submit(long trxId, long accountId, CheckInfo check, MGIContext mgiContext);
        
        /// <summary>
        /// This method to commit the check transaction to processor.
        /// </summary>
        /// <param name="trxId">This is unique identifier for check transaction CheckId</param>
        /// <param name="timezone">This is unique identifier for location timezone</param>
        /// <param name="context">This is the common class parameter used to pass supplemental information</param>
        void Commit(long trxId, string timezone, MGIContext mgiContext);
        
        /// <summary>
        /// This method to get submited check transaction details by check transaction ID.
        /// </summary>
        /// <param name="trxId">This is unique identifier for check transaction CheckId</param>
        /// <returns>Check Transaction Details</returns>
		CheckTrx Get(long trxId);
        
        /// <summary>
        /// This method to get check status from processor by check transaction id and location timezone.
        /// </summary>
        /// <param name="trxId">This is unique identifier for check transaction CheckId</param>
        /// <param name="timezone">This is unique identifier for location timezone</param>
        /// <param name="context">This is the common class parameter used to pass supplemental information</param>
        /// <returns>The check transaction status[Pending/Approval/Decline etc.] </returns>
        CheckStatus Status(long trxId, string timezone, MGIContext mgiContext);
        
        /// <summary>
        /// This method to cancel check transaction by check transaction id and location timezone.
        /// </summary>
        /// <param name="trxId">This is unique identifier for check transaction CheckId</param>
        /// <param name="timezone">This is unique identifier for location timezone</param>
        /// <param name="context">This is the common class parameter used to pass supplemental information</param>
        bool Cancel(long trxId, string timezone, MGIContext mgiContext);
        
        /// <summary>
        /// This method to get all pending check transaction.
        /// </summary>
        /// <returns>List of pending check transaction with details each check transaction</returns>
		List<CheckTrx> PendingChecks();
        
        /// <summary>
        /// This method used to register check account to processor.
        /// </summary>
        /// <param name="account">contains all details related check account(checkAccount Object)</param>
        /// <param name="context">This is the common class parameter used to pass supplemental information</param>
        /// <param name="timezone">This is unique identifier for location timezone id</param>
	long Register(CheckAccount account, MGIContext mgiContext,string timezone);
        
        /// <summary>
        /// This method to update check account.
        /// </summary>
        /// <param name="account">contains all details related check account(checkAccount Object)</param>
        /// <param name="context">This is the common class parameter used to pass supplemental information</param>
		void Update(CheckAccount account, MGIContext mgiContext);
        
        /// <summary>
        /// This method to update check transaction if the status change.
        /// </summary>
        /// <param name="checkTrx">contains check transaction details</param>
        /// <param name="context">This is the common class parameter used to pass supplemental information</param>
		void Update(Cxn.Check.Data.CheckTrx checkTrx, MGIContext mgiContext);
        
        /// <summary>
        /// This method to get check processor information by locationId [channelpartnerid-branchusername].
        /// </summary>
        /// <param name="locationId">This is unique identifier for location timezone id</param>
        /// <returns>Check Processor Information</returns>
		CheckProcessorInfo GetCheckProcessorInfo(string locationId);
        
        /// <summary>
        /// This method to update check transaction franked to true by check transaction id. [IsCheckFranked = true]
        /// </summary>
        /// <param name="trxId">This is unique identifier for check transaction CheckId</param>
		void UpdateTransactionFranked(long trxId);
        
        /// <summary>
        /// This method to Get check account details by check account id.
        /// </summary>
        /// <param name="accountId">This is unique identifier for check account</param>
        /// <returns>Check Account Details </returns>
		CheckAccount GetAccount(long accountId);

		MGI.Cxn.Check.Data.CheckLogin GetCheckSessions(MGIContext mgiContext);//TODO:change name, checkCred Getcheckcredintails

	}
}
