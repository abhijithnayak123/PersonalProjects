using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class CheckAmountStatus : CheckDetails
    {
        public string Source { get; set; }
        public string CheckSubmitted { get; set; }
    }
}