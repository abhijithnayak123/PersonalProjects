using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{

    [DataContract]
    public class XferFee
    {
        public XferFee()
        {
        }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal TransferFee { get; set; }
        [DataMember]
        public decimal TransferTax { get; set; }
        [DataMember]
        public decimal OtherFee { get; set; }
        [DataMember]
        public decimal OtherTax { get; set; }
        //[DataMember]
        //public string PromotionsCodeDescription { get; set; }
        [DataMember]
        public string TestQuestion { get; set; }
        [DataMember]
        public string TestAnswer { get; set; }
        [DataMember]
        public string TestQuestionOption { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal DestinationAmount { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public decimal PromotionDiscount { get; set; }
        [DataMember]
        public decimal PayAmount { get; set; }
		[DataMember]
		public bool IsFixedOnSend { get; set; }
		[DataMember]
		public string PromoCodeDescription { get; set; }
		[DataMember]
		public string PromoName { get; set; }
		[DataMember]
		public string PromoMessage { get; set; }
		[DataMember]
		public string PromotionError { get; set; }
        [DataMember]
        public string PromotionErrorCode { get; set; }
	[DataMember]
	public string ReferenceNo { get; set; }
	[DataMember]
	public string PersonalMessage { get; set; }
	[DataMember]
	public decimal MessageCharge { get; set; }


        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "Amount = ", Amount, "\r\n");
            str = string.Concat(str, "DestinationAmount = ", DestinationAmount, "\r\n");
            str = string.Concat(str, "ExchangeRate = ", ExchangeRate, "\r\n");
            str = string.Concat(str, "TransferFee = ", TransferFee, "\r\n");
            str = string.Concat(str, "TransferTax = ", TransferTax, "\r\n");
            str = string.Concat(str, "OtherFee = ", OtherFee, "\r\n");
            str = string.Concat(str, "OtherTax = ", OtherTax, "\r\n");
            //str = string.Concat(str, "PromotionsCodeDescription = ", PromotionsCodeDescription, "\r\n");
            str = string.Concat(str, "TestQuestion = ", TestQuestion, "\r\n");
            str = string.Concat(str, "TestAnswer = ", TestAnswer, "\r\n");
            str = string.Concat(str, "TestQuestionOption = ", TestQuestionOption, "\r\n");
			str = string.Concat(str, "PromoCodeDescription = ", PromoCodeDescription, "\r\n");
			str = string.Concat(str, "PromoName = ", PromoName, "\r\n");
			str = string.Concat(str, "PromoMessage = ", PromoMessage, "\r\n");
			str = string.Concat(str, "PromotionError = ", PromotionError, "\r\n");
            return str;
        }
    }
}
