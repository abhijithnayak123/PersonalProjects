using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
    public class MoneyGramBillPayReview : BaseModel
    {
        public long BillpayTransactionId { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillerName")]
        public string BillerName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillerLocation")]       
        public string BillerLocationName { get; set; }

        //public string BillerLocationId { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress1 { get; set; }
        public string SenderAddress2 { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderEmail { get; set; }
        //public string CouponPromoCode { set; get; }
        public string BillPaymentAccount { get; set; }
        public string BillPaymentDeliveryMethod { get; set; }
        public string BillPaymentDeliveryOption { get; set; }
        //public string BillPaymentDeliveryMethodCode { get; set; }
        public decimal BillPaymentFee { get; set; }
        public decimal BillpaymentAmount { get; set; }
        //public decimal DiscountedFee { get; set; }
        //public decimal UnDiscountedFee { get; set; }
        
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayConsumerProtectionMessage")]
        public bool ConsumerProtectionMessage { get; set; }

    }
}