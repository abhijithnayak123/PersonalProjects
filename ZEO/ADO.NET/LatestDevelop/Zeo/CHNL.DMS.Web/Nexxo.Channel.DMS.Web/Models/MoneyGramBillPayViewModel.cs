﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TCF.Channel.Zeo.Web.Models
{
    /// <summary>
    /// This class performs a MoneyGram BillPayment View model.
    /// </summary>
    public class MoneyGramBillPayViewModel : BillPaymentViewModel
    {
        /// <summary>
        /// Gets or sets the AccountNumber
        /// </summary>
        [Required(ErrorMessage = "Please enter valid account number.")]
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "AccountNumber")]
        [RegularExpression(@"^[-a-zA-Z0-9]*", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "AccountNumberSpecialChar")]
        [StringLength(16, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillPayAccountNumberStringLength")]
        public string AccountNumber { set; get; }

        /// <summary>
        ///  Gets or sets the ConfirmAccountNumber
        /// </summary>
        [Required(ErrorMessage = "Please enter valid account number.")]
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ConfirmAccountNumber")]
        [System.ComponentModel.DataAnnotations.Compare("AccountNumber", ErrorMessage = "Re-enter account number")]
        [RegularExpression(@"^[-a-zA-Z0-9]*", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "AccountNumberSpecialChar")]
        [StringLength(16, ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillPayAccountNumberStringLength")]
        public string ConfirmAccountNumber { set; get; }

        public string AccountAuthDescription { get; set; }

        public string AccountAuthMask { get; set; }

        public string SessionCookie { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DeliveryOption")]
        public string DeliveryOption { get; set; }

        public List<Fields> FieldList { get; set; }

        public string ReceiveCountry { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "BillPayAmount")]
        [Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillAmountRequired")]
        public decimal BillAmount1 { set; get; }

        public string BillerNotes { get; set; }
        public string BillerDenominations { get; set; }
        public string BillerCode { get; set; }
        public List<FieldViewModel> DynamicFields { get; set; }

    }
    public class Fields
    {
        public string Name { set; get; }
        public string Value { set; get; }
    }
}