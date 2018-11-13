using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class Chat
    {
        public string CustomerName { get; set; }
        public string CheckStatus { get; set; }
        public string CheckAmount { get; set; }
        public string TicketNumber { get; set; }
        public long TransactionId { get; set; }
    }
}