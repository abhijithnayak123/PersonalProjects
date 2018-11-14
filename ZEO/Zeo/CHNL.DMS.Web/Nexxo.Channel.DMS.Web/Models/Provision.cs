using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCF.Zeo.Common.Util;
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

namespace TCF.Channel.Zeo.Web.Models
{
    public class Provision : BaseModel
    {
        public Provision()
        {
            DiscountTypes = new List<SelectListItem>() {
                new SelectListItem() { Text = "Select", Value = "" },
                new SelectListItem() { Text = "Percentage", Value = "1" },
                new SelectListItem() { Text = "Flat Rate", Value = "2" },
                new SelectListItem() { Text = "Fixed Fee", Value = "3" }
            };
        }
        public long ProvisionId { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PromotionName")]
        public string PromotionName { get; set; }

        public long PromotionId { get; set; }

        public string Location { get; set; }

        public string SelectedLocations { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CheckType")]
        public string CheckType { get; set; }

        public string SelectedCheckTypes { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MinAmount")]
        [Range(typeof(Decimal), "0", "79228162514264337593543950335", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillAmountCashRegularExpression")]
        public decimal? MinAmount { get; set; }

        public string MinAmountWithCurrency { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "MaxAmount")]
        [Range(typeof(Decimal), "0", "79228162514264337593543950335", ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "BillAmountCashRegularExpression")]
        public decimal? MaxAmount { get; set; }

        public string MaxAmountWithCurrency { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "ValueRequired")]
        public string Value { get; set; }

        public string DiscountValueWithSymbol { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "DiscountType")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "DiscountTypeRequired")]
        public string DiscountType { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "Group1")]
        public string GroupName { get; set; }

        public string SelectedGroupNames { get; set; }

        public int RowId { get; set; }

        public IEnumerable<SelectListItem> CheckTypes { get; set; }

        public List<ZeoClient.MasterData> Locations { get; set; }

        public IEnumerable<SelectListItem> LGroup { get; set; }

        public IEnumerable<SelectListItem> DiscountTypes { get; set; }
    }
}