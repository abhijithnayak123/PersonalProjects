using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
    /// <summary>
    /// This class performs a MoneyOrderDetails model.
    /// </summary>
    public class MoneyOrderDetails : MoneyOrderImage
    {
        /// <summary>
        /// Gets or sets the Amount
        /// </summary>
        public decimal Amount { get; set; }
    }
}