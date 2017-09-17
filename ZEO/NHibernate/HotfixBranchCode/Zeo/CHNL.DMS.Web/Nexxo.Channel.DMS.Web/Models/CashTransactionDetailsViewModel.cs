using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class CashTransactionDetailsViewModel : TransactionDetailsViewModel
    {
        public decimal Amount { get; set; }
    }
}