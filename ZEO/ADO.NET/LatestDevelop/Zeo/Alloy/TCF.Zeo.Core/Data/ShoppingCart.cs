using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class ShoppingCart
    {

        public long Id { set; get; }

        public List<Check> Checks { set; get; }

        public List<Funds> Funds { set; get; }

        public List<BillPay> Bills { set; get; }

        public List<MoneyOrder> MoneyOrders { set; get; }

        public List<MoneyTransfer> MoneyTransfers { set; get; }

        public List<Cash> Cash { set; get; }

        public decimal CheckTotal { set; get; }

        public decimal FundsTotal { set; get; }

        public decimal BillTotal { set; get; }

        public decimal MoneyOrderTotal { set; get; }

        public decimal MoneyTransferTotal { set; get; }

        public decimal CashTotal { set; get; }

        public decimal GenerateAmount { set; get; }

        public decimal GenerateFee { set; get; }

        public decimal GenerateTotal { set; get; }

        public decimal DepletingAmount { set; get; }

        public decimal DepletingFee { set; get; }

        public decimal DepletingTotal { set; get; }

        public decimal SubTotalFee { set; get; }

        public decimal TotalDueToCustomer { set; get; }

        public bool IsReferral { set; get; }

        public bool IsCashOverCounter { set; get; }

        public bool IsReferralSectionEnabled { set; get; }

        public bool IsCheckFrank { set; get; }

        public ShoppingCart()
        {
            Checks = new List<Check>();
            Funds = new List<Funds>();
            Bills = new List<BillPay>();
            MoneyOrders = new List<MoneyOrder>();
            MoneyTransfers = new List<MoneyTransfer>();
            Cash = new List<Cash>();
        }
    }
}
