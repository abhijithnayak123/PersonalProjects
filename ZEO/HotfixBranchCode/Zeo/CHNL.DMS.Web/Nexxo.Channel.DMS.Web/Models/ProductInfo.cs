using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class ProductInfo :  BaseModel
    {
        public string CashaCheckDisabled { get; set; }
        public decimal CheckLimit { get; set; }
    }
}