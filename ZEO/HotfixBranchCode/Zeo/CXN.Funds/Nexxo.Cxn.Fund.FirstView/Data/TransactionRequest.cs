using System;

namespace MGI.Cxn.Fund.FirstView.Data
{
	public class TransactionRequest : Request
	{
        public TransactionRequest()
        {
            deClrSvcMessageTypeIdentifier = "01";
            deClrSvcTransactionCurrencyCode = "840";
            deClrSvcDateTimeLocalTransaction = DateTime.Now;
            deClrSvcTransmissionDateTime = DateTime.Now;
        }

		public long ClientId { get; set; }

		public string deClrSvcMessageTypeIdentifier { get; set; }

		public long? deClrSvcCustNbr { get; set; }

		public string deClrSvcPrimaryAccountNumber { get; set; }

		public string deClrSvcTranType { get; set; }

		public decimal? deClrSvcTransactionAmount { get; set; }

		public DateTime? deClrSvcTransmissionDateTime { get; set; }

		public long? deClrSvcLineItemSeqNumber { get; set; }

		public int? deClrSvcInventoryCode { get; set; }

		public int? deClrSvcQuantity { get; set; }

		public decimal? deClrSvcUnitPrice { get; set; }

		public int? deClrSvcCreditPlanMaster { get; set; }

		public string deClrSvcTransactionDescription { get; set; }

		public string deClrSvcTransactionCurrencyCode { get; set; }

		public string deClrSvcSpecialMerchantIdentifier { get; set; }

		public string deClrSvcCardAcceptorIdCode { get; set; }

		public string deClrSvcCardAcceptorTerminalID { get; set; }

		public int? deClrSvcCardAcceptorBusinessCode { get; set; }

		public DateTime? deClrSvcDateTimeLocalTransaction { get; set; }

		public long? deClrSvcSystemTraceAuditNumber { get; set; }

		public long? deClrSvcRetrievalReferenceNumber { get; set; }

		public int? deClrSvcMerchantType { get; set; }

		public int? deClrSvcApprovalCode { get; set; }     
	}
}
