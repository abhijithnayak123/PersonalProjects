using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TCF.Channel.Zeo.Web.Models
{
    public class CheckDeclinedReasons : CheckDetails
    {
        public string CheckDeclinedReason { get; set; }
        //    public string CheckId { get; set; }
        public string CheckDeclinedReasonDetails { get; set; }
        public bool isRepresentable { get; set; }
        public string Source { get; set; }
        public string CheckStatus { get; set; }



        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CheckEstablishmentFee")]
        public string CheckEstablishmentFee { get; set; }

        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CheckDate")]
        [Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "InvalidDate")]
        [DateTime(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "InvalidDate")]
        [DataType(DataType.Date)]
        public string CheckDate { get; set; }

        public decimal NetAmount { get; set; }

        [Required(ErrorMessageResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), ErrorMessageResourceName = "PickCheckCategory")]
        public string CheckType { get; set; }

        public string CheckTypeName { get; set; }

        public IEnumerable<SelectListItem> LCheckTypes { get; set; }

        public bool PrintReceiptOnDecline { get; set; }

        //AL-3032 Changes
        [Display(ResourceType = typeof(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo), Name = "CheckPromotionDetails")]
        public string CheckPromotionDetails { get; set; }
    }
}