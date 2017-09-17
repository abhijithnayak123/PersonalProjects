using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
	public class MoneyGramSendMoneyConfirm : BaseModel
	{

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyCouponPromoCode")]
		public string CouponPromoCode { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverFirstName { get; set; }
		
		public string PickupState { get; set; }

		public string PickupCountry { get; set; }

		public decimal TransferAmount { get; set; }
		public decimal Amount { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyDestinationAmount")]
		public decimal DestinationAmount { get; set; }

		public decimal TransferFee { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyOriginalFee")]
		public decimal OriginalFee { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyDeliveryMethod")]
		public string DeliveryMethod { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyTotalToRecipient")]
		public decimal TotalToRecipient { get; set; }

		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SendMoneyPromoDiscount")]
		public decimal PromoDiscount { get; set; }

        public bool IsDomesticTransfer { get; set; }

        [Required]
		[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ConsumerProtectionMessageMoneyGram")]
        public bool ConsumerProtectionMessage { get; set; }

        [Required]
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ProvidedTermsandConditionsMessageMoneyGram")]
        public bool ProvidedTermsandConditionsMessage { get; set; }

        [Required]
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DoddFrankTermsandConditionsMessageMoneyGram")]
        public bool DoddFrankDisclosure { get; set; }

        public long TransactionId { get; set; }

		
		public string ReceiverAddress { get; set; }
		public string ReceiverCity { get; set; }
		public string ReceiverState { get; set; }
		public string ReceiveAgent { get; set; }

	}
}
