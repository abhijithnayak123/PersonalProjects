using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.WU.Common.Data;

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
    public class SearchRequest
    {
        public string database_key { set; get; }
        public string money_transfer_key { set; get; }
        public string mtcn { set; get; }
        public string paystatusdescription { get; set; }
	    public SendMoneyTransactionType.agentcsc_flags sendmoneytransactiontype { get; set; }
        public string ReasonDesc { get; set; }
        public string ReasonCode { get; set; }
        public string Comments { get; set; }
        public string ReferenceNo { get; set; }
	}

	public class SendMoneyTransactionType
	{
		public enum agentcsc_flags
		{
			
			REFUND,

			
			CANCEL_SEND,

			
			CANCEL_PAID
		}
	}    
}
