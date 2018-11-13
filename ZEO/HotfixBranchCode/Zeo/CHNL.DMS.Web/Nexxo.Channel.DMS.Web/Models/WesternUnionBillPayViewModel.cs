using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    /// <summary>
    /// This class performs a WesternUnion BillPayment View model.
    /// </summary>
    public class WesternUnionBillPayViewModel : BillPaymentViewModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayeeCouponPromoCode")]
        [CouponPromoAlias("CouponPromoCode")]
        public string CouponPromoCode { set; get; }

        /// <summary>
        /// Gets or sets the AccountNumber
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "AccountNumber")]
        public string AccountNumber { set; get; }

        /// <summary>
        ///  Gets or sets the ConfirmAccountNumber
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "ConfirmAccountNumber")]
        [Compare("AccountNumber", ErrorMessage = "Re-enter account number")]
        public string ConfirmAccountNumber { set; get; }

        /// <summary>
        ///  Gets or sets the BillerLocation
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillerLocation")]
        public string BillerLocation { set; get; }
        public string BillerLocationName { get; set; }
        public IEnumerable<SelectListItem> LLocations { get; set; }


        /// <summary>
        ///  Gets or sets the DeliveryMethod
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "DeliveryMethod")]
        [Required(ErrorMessage = "Please select valid Delivery Method.")]
        public string BillerDeliveryMethod { set; get; }
        public string SelectedDeliveryMethod { set; get; }
        public IEnumerable<SelectListItem> LDeliveryMethods { get; set; }

        public string AccountAuthDescription { get; set; }

        public string AccountAuthMask { get; set; }
        
        public string SessionCookie { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayeeAttention")]
        public string Attention { set; get; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayeeReference")]
        public string Reference { set; get; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayeeAccountHolderName")]
        public string AccountHolder { set; get; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayeeAvailableBalance")]
        public string AvailableBalance { set; get; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayeeDateOfBirth")]
        public string DateOfBirth { set; get; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayConsumerProtectionMessage")]
        public bool ConsumerProtectionMessage { get; set; }

		public string BillPayTransactionId { get; set; }
    }
}