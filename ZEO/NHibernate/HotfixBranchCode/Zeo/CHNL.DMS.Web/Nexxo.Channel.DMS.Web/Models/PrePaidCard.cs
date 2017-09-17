using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
    public class PrePaidCard : BaseModel
    {

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidName")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the CardNumber
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidCardNumber")]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidCardNumberRequired")]
        //[RegularExpression(@"^(\d){4}(\s){1}(\d){4}(\s){1}(\d){4}(\s){1}(\d){4}$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidCardNumberRegularExpression")]
        //[StringLength(16, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidCardNumberStringLength")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the AccountNumber
        /// </summary>
        //[Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidAccountNumber")]
        //[RegularExpression(@"^\d*$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidAccountNumberRegularExpression")]
        //[RegularExpression(@"^(\d){4}-(\d){4}-(\d){2}$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidAccountNumberRegularExpression")]
        //[StringLength(10, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidAccountNumberStringLength")]
        public string AccountNumber { set; get; }

        /// <summary>
        ///  Gets or sets the ActivationFee
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidActivationFee")]
        //[Range(typeof(Decimal), "0.01", "79228162514264337593543950335", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidActivationValidRegex")]
        public decimal? ActivationFee { set; get; }

        /// <summary>
        /// Gets or sets the MinimumLoad
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidMinimumLoad")]
        public decimal? MinimumLoad { get; set; }

        /// <summary>
        ///  Gets or sets the LoadCard
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidLoadCard")]
        //[Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidLoadCardRequired")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{2})?$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidLoadCardRegex")]
        [Range(typeof(Decimal), "0.01", "79228162514264337593543950335", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidLoadCardValidRegex")]
        public decimal? LoadAmount { get; set; }

        /// <summary>
        ///  Gets or sets the LoadFee
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidLoadFee")]
        public decimal? LoadFee { get; set; }

        /// <summary>
        ///  Gets or sets the LoadBalanceImpact
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidBalanceImpact")]
        public decimal? LoadBalanceImpact { get; set; }

        /// <summary>
        ///  Gets or sets the WithdrawBalanceImpact
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidBalanceImpact")]
        public decimal? WithdrawBalanceImpact { get; set; }

        /// <summary>
        ///  Gets or sets the CashToCustomer
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidCashToCustomer")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{2})?$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidLoadCardRegex")]
        [Range(typeof(Decimal), "0.01", "79228162514264337593543950335", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidCashToCustomerValidRegex")]
        public decimal? CashToCustomer { get; set; }

        /// <summary>
        ///  Gets or sets the WithdrawFee
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidWithdrawFee")]
        public decimal? WithdrawFee { get; set; }

        /// <summary>
        ///  Gets or sets the WithdrawFee
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidCashToCustomer")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{2})?$", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidLoadCardRegex")]
        [Range(typeof(Decimal), "0.01", "79228162514264337593543950335", ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PrepaidCashToCustomerValidRegex")]
        public decimal? WithdrawAmount { get; set; }

        /// <summary>
        ///  Gets or sets the TransactionType
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidTransactionType")]
        public string TransactionType { get; set; }

        /// <summary>
        ///  Gets or sets the ScreenTitle
        /// </summary>
        public string ScreenTitle { get; set; }

        /// <summary>
        ///  Gets or sets the GPRCardExists flag
        /// </summary>
        public bool GPRCardExists { get; set; }

        /// <summary>
        ///  Gets or sets the MaxAvailableBalance
        /// </summary>
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "PrepaidMaxAvailableBalance")]
        public decimal? MaxAvailableBalance { get; set; }

        public string StatusDescription { get; set; }

        public string MaskedCardNumber { get; set; }

    }
}