using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
    public class MoneyGramReceiveMoneyViewModel : BaseModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MGramRMReferenceNumber")]
        [StringLength(8, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MGramRMReferenceNumberStringLength", MinimumLength = 8)]
        [Required(ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "MGramRMReferenceNumberRequired")]
        public string ReferenceNumber { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneyReceiverName")]
        public string ReceiverName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneySenderName")]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneyTestQuestion")]
        public string TestQuestion { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneyTestAnswer")]
        public string TestAnswer { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneyReceiveAmount")]
        public string ReceiveAmount { get; set; }

        public long TransactionId { get; set; }

        public string ReceiveAmountWithCurrency { get; set; }

        public string ReceiveCurrency { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "TransactionOriginatingState")]
        public string SenderStateCode { get; set; }

        public List<FieldViewModel> DynamicFields { get; set; }
    }
}