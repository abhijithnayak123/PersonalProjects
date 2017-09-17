using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
   //US1421 changes
    public class CheckFrankingDetailsViewModel
    {
        public string MICR { get; set; }
        public string Amount { get; set; }
        public string FrankData { get; set; }
        public string DisplayMsg { get; set; }
        public int chkSlno { get; set; }
		public bool IsCheckFrank { get; set; }
		public string TransactionID { get; set; }
    }
}