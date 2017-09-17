using System;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.BillPay.MG.Data
{
	public class Transaction : NexxoModel
	{
        //Common fields

        public virtual string AgentID { get; set; }
		public virtual string AgentSequence { get; set; }
		public virtual string Token { get; set; }
		public virtual string ApiVersion { get; set; }
		public virtual string ClientSoftwareVersion { get; set; }
        public virtual Int16 RequestResponseType { get; set; }
        public virtual Account Account { get; set; }
        public virtual string BillerName { get; set; }

		//FeeRequest fields
        
	    public virtual Int16 ProductVariant { get; set; }
	    public virtual string ReceiveCountry { get; set; }        
        public virtual string ReceiveCode { get; set; }               
        public virtual string ReceiveAgentID { get; set; }        
        public virtual string ReceiveCurrency { get; set; }        
        public virtual string SendCurrency { get; set; }
        public virtual string PromoCodeValuesPromoCode { get; set; }
        public virtual string AccountNumber { get; set; }	

        //FeeResponse fields	

        public virtual bool? DoCheckIn { get; set; }
        public virtual DateTime? TimeStamp { get; set; }
        public virtual int? Flags { get; set; }

	    public virtual decimal ValidReceiveAmount { get; set; }        
        public virtual string ValidReceiveCurrency { get; set; }        
        public virtual decimal ValidExchangeRate { get; set; }    
     
        public virtual decimal TotalAmount { get; set; }        
        public virtual bool ReceiveAmountAltered { get; set; }      
        public virtual bool RevisedInformationalFee { get; set; }         
        public virtual string DeliveryOptId { get; set; }        
        public virtual string DeliveryOptDisplayName { get; set; }  
        public virtual string ReceiveAgentName { get; set; }      
        public virtual string MgiTransactionSessionID { get; set; }        
        public virtual bool SendAmountAltered { get; set; }   

	    public virtual decimal SendAmount { get; set; }          
        public virtual decimal TotalSendFees { get; set; }          
        public virtual decimal TotalDiscountAmount { get; set; }         
        public virtual decimal TotalSendTaxes { get; set; }          
        public virtual decimal TotalAmountToCollect { get; set; }    

        public virtual decimal ReceiveAmount { get; set; }              
        public virtual bool ValidCurrencyIndicator { get; set; }         
        public virtual string PayoutCurrency { get; set; }        
        public virtual decimal TotalReceiveFees { get; set; }          
        public virtual decimal TotalReceiveTaxes { get; set; }         
        public virtual decimal TotalReceiveAmount { get; set; }           
        public virtual bool ReceiveFeesAreEstimated { get; set; }        
        public virtual bool ReceiveTaxesAreEstimated { get; set; }

        //BpValidation() fields
        
        public virtual string BillerAccountNumber { get; set; }
        public virtual string ValidateAccountNumber { get; set; }
        public virtual string SenderFirstName { get; set; }
        public virtual string SenderMiddleName { get; set; }
        public virtual string SenderLastName { get; set; }
        public virtual string SenderLastName2 { get; set; }
        public virtual string SenderAddress { get; set; }
        public virtual string SenderCity { get; set; }
        public virtual string SenderState { get; set; }
        public virtual string SenderZipCode { get; set; }
        public virtual string SenderCountry { get; set; }
        public virtual string SenderHomePhone { get; set; }
        public virtual string ReceiverFirstName { get; set; }
        public virtual string ReceiverMiddleName { get; set; }
        public virtual string ReceiverLastName { get; set; }
        public virtual string ReceiverLastName2 { get; set; }
        
        public virtual string MessageField1 { get; set; }
        public virtual string MessageField2 { get; set; }
        public virtual string SenderPhotoIdType { get; set; }
        public virtual string SenderPhotoIdNumber { get; set; }
        public virtual string SenderPhotoIdState { get; set; }
        public virtual string SenderPhotoIdCountry { get; set; }
        public virtual string SenderLegalIdType { get; set; }
        public virtual string SenderLegalIdNumber { get; set; }
        public virtual DateTime? SenderDOB { get; set; }
        public virtual string SenderOccupation { get; set; }

        public virtual string AccountNumberRetryCount { get; set; }
        public virtual string PurposeOfFund { get; set; }
        
        // BP Response fields

        public virtual decimal TotalSendAmount { get; set; }
        public virtual string ServiceOfferingID { get; set; }
        public virtual string MgiRewardsNumber { get; set; }
        public virtual string BillerWebsite { get; set; }
        public virtual string BillerPhone { get; set; }
        public virtual string BillerCutoffTime { get; set; }
	    public virtual string BillerAddress { get; set; }
        public virtual string BillerAddress2 { get; set; }
        public virtual string BillerAddress3 { get; set; }
        public virtual string BillerCity { get; set; }
        public virtual string BillerState { get; set; }
        public virtual string BillerZip { get; set; }
        public virtual bool PrintMGICustomerServiceNumber { get; set; }
        public virtual string AgentTransactionId { get; set; }
        public virtual bool ReadyForCommit { get; set; }
        public virtual decimal ProcessingFee { get; set; }
        public virtual bool InfoFeeIndicator { get; set; }
        public virtual decimal ExchangeRateApplied { get; set; }

        //// fields in commit response

        public virtual string ReferenceNumber { get; set; }
        public virtual string PartnerConfirmationNumber { get; set; }
        public virtual string PartnerName { get; set; }
        public virtual string FreePhoneCallPin { get; set; }
        public virtual string TollFreePhoneNumber { get; set; }
        public virtual string TextTranslation { get; set; }

        public virtual string CardExpirationMonth { get; set; }
        public virtual string CardExpirationYear { get; set; }

        public virtual bool IsValidateAccNumberRequired { get; set; }

        public virtual string ExpectedPostingTimeFrame { get; set; }
        public virtual string ExpectedPostingTimeFrameSecondary { get; set; }
        public virtual string CustomerTipTextTranslation { get; set; }
    }
}
	