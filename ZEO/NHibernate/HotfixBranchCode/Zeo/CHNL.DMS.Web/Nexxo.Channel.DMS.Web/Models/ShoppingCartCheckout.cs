using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MGI.Channel.DMS.Web.ServiceClient;
using System.ComponentModel.DataAnnotations;

namespace MGI.Channel.DMS.Web.Models
{
    public class ShoppingCartCheckout : BaseModel
    {
        public List<ShoppingCartItemModel> CartItems { get; set; }

        public decimal GenerateAmount { get; set; }
        public decimal GenerateFee { get; set; }
        public decimal GenerateTotal { get; set; }
                
        public decimal DepletingAmount { get; set; }
        public decimal DepletingFee { get; set; }
        public decimal DepletingTotal { get; set; }

        public decimal SubTotalFee { get; set; }

        public decimal TotalDueToCustomer { get; set; }

        public decimal LoadToCard { get; set; }

        public decimal NetDueToCustomer { get; set; }
        [RegularExpression(@"^[0-9]*(\.[0-9]{2})?$", ErrorMessage = "Please enter valid amount.")]
        [Range(typeof(Decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Please enter valid amount.")]
        public decimal? CashCollected { get; set; }
    }
}