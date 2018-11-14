using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
  public  class ShoppingCartSummeryReceiptData: BaseReceiptData
    {
        public int CheckCount { get; set; }
        public int MOCount { get; set; }
        public int BPCount { get; set; }
        public int SMCount { get; set; }
        public int RMCount { get; set; }
        public decimal CheckTotal { get; set; }
        public decimal GPRWithDraw { get; set; }
        public decimal FundsGeneratingTotal { get; set; }
        public decimal GPRLoad { get; set; }
        public decimal GPRActivate { get; set; }
        public decimal MoneyOrder { get; set; }
        public decimal FundsDepletingTotal { get; set; }
        public decimal GPRCompanion { get; set; }
        public decimal NetAmount { get; set; }
        public string TotalMsg { get; set; }
        public decimal CashCollected { get; set; }
        public decimal CashToCustomer { get; set; }
        public decimal Currency { get; set; }
        public string CardNumber { get; set; }
        public decimal MoneyTransferSend { get; set; }
        public decimal BillPay { get; set; }
        public decimal MoneyTransferReceive { get; set; }
        public decimal MoneyTransferModified { get; set; }
        public decimal MoneyTransferCancelled { get; set; }
        public decimal MoneyTransferRefund { get; set; }
        public string Gpr { get; set; }

    }
}
