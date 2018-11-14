using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Web.Models
{
    public class Qualifier : BaseModel
    {
        public Qualifier()
        {
            TrxStates = new List<SelectListItem>() {
                new SelectListItem() { Text = "Completed", Value = "4" },
                new SelectListItem() { Text = "In Cart", Value = "1-2-12-4" },
                new SelectListItem() { Text = "Canceled, Declined, Parked, Failed", Value = "6-8-5-1-2-12-4" }
            };
        }
        public long QualifierId { get; set; }

        public long PromotionId { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PromotionName")]
        public string PromotionName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "QualifierProduct")]
        public string QualifierProduct { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TransactionEndDate")]
        public string TransactionEndDate { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "TransactionAmount")]
        [Range(typeof(Decimal), "0.01", "79228162514264337593543950335", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillAmountCashRegularExpression")]
        public decimal? TransactionAmount { get; set; }

        public string TransactionAmountWithCurrency { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MinimumTrxCount")]
        public int? MinimumTrxCount { get; set; }

        public string TrxState { get; set; }

        public string SelectedTxnStates { get; set; }

        public bool IsPaidFee { get; set; }

        public int RowId { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }

        public IEnumerable<SelectListItem> TrxStates { get; set; }
    }
}