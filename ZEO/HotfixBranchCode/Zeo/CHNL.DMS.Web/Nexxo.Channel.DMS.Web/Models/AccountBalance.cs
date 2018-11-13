using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
    public class AccountBalance : CheckTransaction
    {
        [DisplayFormat(DataFormatString = "{0:.##}")]
        public decimal AccountBalanceAmount { get; set; }
    }
}