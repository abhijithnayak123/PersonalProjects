using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedData = MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Models
{
    /// <summary>
    /// This class performs a BillPayment View model.
    /// </summary>
    public class BillPaymentViewModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the SelectBillPayee
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "SelectBillPayee")]
        [RegularExpression("^((?![<>]).)*$", ErrorMessage = "Bill payee should not contain angular brackets")]
        public string BillerName { set; get; }

        public long BillerId { get; set; }

        /// <summary>
        ///  Gets or sets the EnterBillAmount
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayAmount")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillAmountRequired")]
        //[RegularExpression(@"^[0-9]*(\.[0-9]{2})?$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillAmountCashRegularExpression")]
        [Range(typeof(Decimal), "0.01", "79228162514264337593543950335", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillAmountRegularExpression")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal BillAmount { set; get; }

        /// <summary>
        ///  Gets or sets the BillPaymentFee
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPaymentFee")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal BillPaymentFee { set; get; }

        public List<SharedData.Product> FrequentBillPayees { get; set; }

        public string ProviderName { set; get; }
    }
}