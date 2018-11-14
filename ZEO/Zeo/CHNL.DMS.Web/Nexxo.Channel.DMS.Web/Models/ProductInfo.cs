using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class ProductInfo :  BaseModel
    {
        public string CashaCheckDisabled { get; set; }
        public decimal CheckLimit { get; set; }
    }
}