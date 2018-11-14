using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;

using MGI.Channel.DMS.Server.Data;
using System.ServiceModel;

namespace MGI.Channel.DMS.Server.Contract
{
    [ServiceContract]
    public interface IReceiptsService
    {
        /// <summary>
        /// This method to get check receipt data based on check transaction Id. 
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Return the list of Receipt Data</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetCheckReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);
        
        /// <summary>
        /// This method to get fund receipt data based on fund transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Return the list of Receipt Data</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetFundsReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);

        /// <summary>
        /// This method to get money transfer receipt data based on money transfer transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Return the list of Receipt Data</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);

        /// <summary>
        /// This method to get money order receipt data based on money order transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Return the list of Receipt Data</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetMoneyOrderReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);

        /// <summary>
        /// This method to get billpay receipt data based on billpay transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Return the list of Receipt Data</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);

        /// <summary>
        /// This method to get dodd frank receipt data based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Return the list of Receipt Data</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetDoddFrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetCheckDeclinedReceiptData(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        
 		/// <summary>
        /// This method to get summary receipt data based on shopping cart Id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="cartId">This is unique identifier for shopping cart.</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Return the list of Receipt Data</returns>
		[OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetSummaryReceipt(long customerSessionId, long cartId, MGIContext mgiContext);

        /// <summary>
        /// This method to get summary receipt data based on shopping cart Id for reprint.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="transactiontype">the transaction type [Send Money, Money Order, Check Processing, BillPay,]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Return the list of Receipt Data</returns>
        [OperationContract(Name = "GetSummaryReceiptForReprint")]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetSummaryReceipt(long agentSessionId, long customerSessionId, long transactionId, string transactiontype, MGIContext mgiContext);

        /// <summary>
        /// This method to get coupon code receipt data, when IsReferral is true.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Return the list of Receipt Data</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetCouponCodeReceipt(long customerSessionId, MGIContext mgiContext);
    }
}
