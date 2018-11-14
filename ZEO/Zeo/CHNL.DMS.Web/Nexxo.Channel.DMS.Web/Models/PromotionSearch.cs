using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
    public class PromotionSearch : BaseModel
    {
        public PromotionSearch()
        {
            ProviderIds = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Select", Value = "", Selected = true},
            };
        }
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PromotionName")]
        public string PromotionName { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "PromoDescription")]
        public string PromoDescription { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "StartDate")]
        public string StartDate { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "EndDate")]
        public string EndDate { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ProductType")]
        public string ProductType { get; set; }

        public string Provider { get; set; }

        public string ProviderId { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "ShowExpired")]
        public bool ShowExpired { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }

        public IEnumerable<SelectListItem> ProviderIds { get; set; }
    }
}