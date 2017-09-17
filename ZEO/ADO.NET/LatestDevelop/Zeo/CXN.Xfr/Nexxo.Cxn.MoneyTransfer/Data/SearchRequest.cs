using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
    public class SearchRequest
    {        
        public string ConfirmationNumber { set; get; }        
        public string ReasonDesc { get; set; }
        public string ReasonCode { get; set; }
        public string Comments { get; set; }
		public string RefundStatus { get; set; }
        public string ReferenceNumber { get; set; }
		public SearchRequestType SearchRequestType { get; set; }
		public long TransactionId { get; set; }	
	}

	public enum SearchRequestType
	{
		Modify,
		Refund,
		RefundWithStage,
        LookUp
	}

    public class SMRTransactionTypes
    {
        public enum SearchRefundFlags
        {
            REFUND,
            CANCEL_SEND,
            CANCEL_PAID
        }
    }    
}
