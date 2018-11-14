using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    public class CardDetails
    {
        public PaymentDetails paymentDetails { get; set; }
        public Account sender { get; set; }
        public string AccountNumber { get; set; }
        public string ForiegnSystemId { get; set; }
        public string ForiegnRefNum { get; set; }
        public string CounterId { get; set; }
    }
}
