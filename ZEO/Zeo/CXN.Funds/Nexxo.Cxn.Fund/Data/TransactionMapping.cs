using System;

namespace MGI.Cxn.Fund.Data
{
    public class TransactionMapping
    {
        public long Id { get; set; }       
        public long FundId { get; set; }     
        public string ProcessorReferenceId { get; set; }    
        public int ProcessorId { get; set; }
        public long? PrimaryAccountNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public long? SSN { get; set; }
        public string GovernmentID { get; set; }
		public string GovernmentId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string HomePhoneNumber { get; set; }
        public string TranType { get; set; }
        public decimal? TransactionAmount { get; set; }
        public string CardAcceptorIdCode { get; set; }
        public string CardAcceptorTerminalID { get; set; }
        public int? CardAcceptorBusinessCode { get; set; }
        public string TransactionDescription { get; set; }
        public string MessageTypeIdentifier { get; set; }
        public string TransactionCurrencyCode { get; set; }
        public DateTime? DTLocalTransaction { get; set; }
        public DateTime? DTTransmission { get; set; }
        public int? CreditPlanMaster { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionID { get; set; }
        public decimal? CardBalance { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
		public string CardStatus { get; set; }
		public string ActivationRequired { get; set; }
	}
}
