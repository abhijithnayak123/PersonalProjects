using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Models
{
    public class CashACheck : CheckDetails 
    {
        public Guid UniqueId { get; set; }
        public string ckecksToAttendMsg { get; set; }
		public string  Processor { get; set; }
    }
}