using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    public class BillPayTransactionDetailsViewModel : TransactionDetailsViewModel
    {
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayTrxHistoryAmount")]
        public decimal Amount { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayTrxHistoryPayee")]
        public string Payee { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayTrxHistoryAccountNumber")]
        public string AccountNumber { get; set; }
        
        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "TenantId")]
        public string TenantId { set; get; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "BillPayTrxHistoryMTCN")]
        public string MTCN { get; set; }

        [Display(ResourceType = typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo), Name = "MoneyTrasferTrxRefNo")]
        public string ReferenceNumber { get; set; }
    }
}