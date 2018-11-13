using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using System.Collections.Generic;
using System;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface ICheckCashingService
	{
        /// <summary>
        /// This method to submit the check information to processor.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="check">This parameter contains check information</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check transaction details</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
        Check SubmitCheck(long customerSessionId, CheckSubmission check, MGIContext mgiContext);

        /// <summary>
        /// This method to cancel the check information to processor.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="check">This parameter contains check information</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check transaction details</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
        void CancelCheck(long customerSessionId, string check, MGIContext mgiContext);

        /// <summary>
        /// This method to get updated check status from processor by checkId.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check transaction details</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		Check GetCheckStatus(long customerSessionId, string checkId, MGIContext mgiContext);
       
        /// <summary>
        /// This method to check can submit check after check status is declined.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="check">This parameter contains check information</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Transaction status</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		bool CanResubmit(long customerSessionId, string checkId, MGIContext mgiContext);		

        /// <summary>
        /// This method to get all check types.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List of check types</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		List<string> GetCheckTypes(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get check transaction fee.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkSubmit">This parameter contains check information</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Transaction Fee Details</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		TransactionFee GetCheckFee(long customerSessionId, CheckSubmission checkSubmit, MGIContext mgiContext);

        /// <summary>
        /// This method to get check transaction details by check id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="check">This parameter contains check information</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check Transaction Details</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		CheckTransactionDetails GetCheckTranasactionDetails(long agentSessionId, long customerSessionId, string checkId, MGIContext mgiContext);

        /// <summary>
        /// This method to get check franking data by transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check francking data</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		string GetCheckFrankingData(long customerSessionId, long transactionId, MGIContext mgiContext);
		
        /// <summary>
        /// This method to get processor details.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Check Processor Information</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		CheckProcessorInfo GetCheckProcessorInfo(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to update check transaction franked to true by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		void UpdateTransactionFranked(long customerSessionId, long transactionId, MGIContext mgiContext);

		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		CheckLogin GetCheckSession(long customerSessionId, MGIContext mgiContext);
	}
}
