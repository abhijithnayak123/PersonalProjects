using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class ParkTransaction : BaseModel
    {
        public long TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string IconName { get; set; }
        public decimal Amount { get; set; }
        public string Detail { get; set; }
        public string InfoMessage { get; set; }
        public bool IsMobileEnabled { get; set; }
        public string MobileNumber { get; set; }
        public string TransactionStatus { get; set; }
		public string CheckMOPromotionAlertMessage { get; set; }
    }
}