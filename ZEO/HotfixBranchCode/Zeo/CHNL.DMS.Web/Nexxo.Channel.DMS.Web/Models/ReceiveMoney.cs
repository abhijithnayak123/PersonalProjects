using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Web.Models
{
    public class ReceiveMoney : BaseModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneyWesternUnionMTCN")]
        [StringLength(12, ErrorMessageResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "WesternunionMTCNNumberRequired", MinimumLength = 10)]
        [Required(ErrorMessageResourceType=typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "ReceiveMoneyMTCN")]
        public string WesternUnionMTCN { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneyReceiverName")]
        public string ReceiverName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneySenderName")]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneyTestQuestion")]
        public string TestQuestion { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneyTestAnswer")]
        public string TestAnswer { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "RecieveMoneyTransferAmount")]
        public string TransferAmount { get; set; }

        public long TransactionId { get; set; }

        public string TransferAmountWithCurrency { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "TransactionOriginatingState")]
        public string SenderStateCode { get; set; }
    }
}