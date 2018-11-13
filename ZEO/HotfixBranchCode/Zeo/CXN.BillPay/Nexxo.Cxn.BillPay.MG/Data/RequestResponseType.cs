using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.BillPay.MG.Data
{
    public enum RequestResponseType : int
    {
        FeeRequest = 1,
        FeeResponse,
        ValidationRequest,
        ValidationResponse,
        CommitRequest,
        CommitResponse
    }
}
