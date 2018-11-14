using TCF.Zeo.Biz.Receipt.Contract;
using TCF.Zeo.Biz.Receipt.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Test
{
    [TestFixture]
    public class Receipt_Fixturet
    {
        IReceiptService receiptService = new WUReceiptServiceImpl();
        [TestCase]
        public void Can_GetCheckReceipt()
        {
            receiptService.GetCheckReceipt(1000000000, false, null);
        }

        [TestCase]
        public void Can_GetBillpayReceipt()
        {
            receiptService.GetBillPayReceipt(1000000000, false, null);
        }

        [TestCase]
        public void Can_GetMoneyTransferReceipt()
        {
            receiptService.GetMoneyTransferReceipt(1000000000, false, null);
        }
        
        [TestCase]
        public void Can_GetMoneyOrderReceipt()
        {
            receiptService.GetMoneyOrderReceipt(1000000000, false, null);
        }

        [TestCase]
        public void Can_GetDoddFrankReceipt()
        {
            receiptService.GetDoddFrankReceipt(1000000000, null);
        }

        [TestCase]
        public void Can_GetCouponReceipt()
        {
            receiptService.GetCouponReceipt(1000000000, null);
        }
    }
}
