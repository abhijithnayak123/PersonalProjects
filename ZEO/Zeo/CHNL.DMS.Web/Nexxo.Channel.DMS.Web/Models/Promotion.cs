using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCF.Zeo.Common.Util;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Models
{
    public class Promotion : BaseModel
    {
        public Promotion()
        {                                                             
            ProviderIds = new List<SelectListItem>()                          
            {                                                                 
                new SelectListItem(){ Text = "Select", Value = ""},
            };                                                                                                                            
        }

        public long PromotionId { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "PromotionNameRequired")]
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PromotionName")]
        public string PromotionName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PromoDescription")]
        public string PromoDescription { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "StartDateRequired")]
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "StartDate")]
        public string StartDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "EndDateRequired")]
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "EndDate")]
        public string EndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Nexxo), ErrorMessageResourceName = "ProductTypeRequired")]
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProductType")]
        public string ProductType { get; set; }

        public string Provider { get; set; }

        public string ProviderId { get; set; }

        public string Priority { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IsSystemApplied")]
        public bool IsSystemApplied { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IsOverridable")]
        public bool IsOverridable { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PromoStatus")]
        public string PromotionStatus { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IsPrintable")]
        public bool IsPrintable { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IsNextCustomerSession")]
        public bool IsNextCustomerSession { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "FreeTxnCount")]
        public int? FreeTxnCount { get; set; }

        public bool Stackable { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "IsPromotionHidden")]
        public bool IsPromotionHidden { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }

        public IEnumerable<SelectListItem> ProviderIds { get; set; }
    }
}