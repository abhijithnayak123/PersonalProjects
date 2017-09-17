using MGI.Common.Util;
using MGI.Cxn.MoneyTransfer.MG.AgentConnectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.MG.Contract
{
    public interface IIO
    {
        DetailLookupResponse DetailLookupRequest(DetailLookupRequest detailLookupRequest);

		SendValidationResponse SendValidation(SendValidationRequest validationRequest, MGIContext mgiContext);

		CommitTransactionResponse SendCommit(CommitTransactionRequest commitRequest, MGIContext mgiContext);

		SendReversalResponse SendReversal(SendReversalRequest sendReversalRequest, MGIContext mgiContext);
        
		AmendTransactionResponse AmendTransaction(AmendTransactionRequest amendTransactionRequest);		

        /// <summary>
        /// This method is used for getting reference number
        /// </summary>
        /// <param name="referenceNumberRequest">contains request reference number details</param>
        /// <returns>reference number</returns>
        ReferenceNumberResponse RequestReferenceNumber(ReferenceNumberRequest referenceNumberRequest);

        /// <summary>
        /// This method is used for receive validation
        /// </summary>
        /// <param name="receiveValidationRequest">contains receive validation request information</param>
        /// <returns>receive validation response</returns>
        ReceiveValidationResponse ReceiveValidation(ReceiveValidationRequest receiveValidationRequest);

        /// <summary>
        /// This method is used for receive commit
        /// </summary>
        /// <param name="commitRequest">contains commit transaction request information</param>
        /// <param name="context"></param>
        /// <returns>commit transaction response</returns>
		CommitTransactionResponse ReceiveCommit(CommitTransactionRequest commitRequest, MGIContext mgiContext);
    }
}
