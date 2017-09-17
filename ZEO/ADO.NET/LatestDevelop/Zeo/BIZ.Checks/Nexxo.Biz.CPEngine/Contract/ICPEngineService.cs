using System;
using System.Collections.Generic;

using MGI.Biz.Common.Data;
using MGI.Biz.CPEngine.Data;
using MGI.Common.Util;

namespace MGI.Biz.CPEngine.Contract
{
	public interface ICPEngineService
	{
        /// <summary>
        /// This method to submit the check information to processor.
        /// </summary>
        /// <param name="check">This parameter contains check information</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check transaction details</returns>
		Check Submit(CheckSubmission check, long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get updated check transaction status.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
        /// <param name="includeImage"></param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns></returns>
        Check GetStatus(long customerSessionId, string checkId, bool includeImage, MGIContext mgiContext);

        /// <summary>
        /// This method to cancel the check transaction by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        bool Cancel(long customerSessionId, string checkId, MGIContext mgiContext);

        /// <summary>
        /// This method to check can submit check after check status is declined.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Transaction Status</returns>
        bool CanResubmit(long customerSessionId, string checkId, MGIContext mgiContext);

        /// <summary>
        /// This method to submit the park check transaction by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Transaction Status</returns>
		bool Resubmit(long customerSessionId, long checkId, MGIContext mgiContext);

        /// <summary>
        /// This method to commit check transaction by check transaction id.
        /// </summary>
        /// <param name="checkId">This is unique identifier for check transaction</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        void Commit(string CheckId, long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get all check type.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Check Types</returns>
		List<string> GetCheckTypes(long customerSessionId, MGIContext mgiContext);

		//US1799 Changed contract to class object to add more properties -PromotionCode,IsSystemApplied
        /// <summary>
        /// This method to get Check Transaction fee.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="check"></param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Transaction Fee Details</returns>
        TransactionFee GetFee(long customerSessionId, CheckSubmission check, MGIContext mgiContext);

        /// <summary>
        /// This method to get check transaction details by check transaction id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check Transaction Details</returns>
        CheckTransaction GetTransaction(long agentSessionId, long customerSessionId, string CheckId, MGIContext mgiContext);

        /// <summary>
        /// This method to get check franking data by transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check francking data</returns>
        string GetCheckFrankingData(long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get processor details.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check Processor Information</returns>
        CheckProcessorInfo GetCheckProcessorInfo(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to update check transaction franked to true by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		void UpdateTransactionFranked(long customerSessionId, long transactionId, MGIContext mgiContext);

		void UpdateStatusOnRemoval(long customerSessionId, long checkId);

		ChexarLogin GetChexarSessions(MGIContext mgiContext);
	}
}
