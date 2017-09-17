using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
    public enum RequestResponseType : int
    {
        FeeRequest = 1,
        FeeResponse,
        ValidationRequest,
        ValidationResponse,
        CommitRequest,
        CommitResponse,
        ReferenceNumberRequest,
        ReferenceNumberResponse,
        ValidationRequestDuringCommit,
        ValidationResponseDuringCommit,
        Stage,
        AmendTransactionRequest,
        AmendTransactionResponse,
        ReversalRequest,
        ReversalResponse
    }
}
