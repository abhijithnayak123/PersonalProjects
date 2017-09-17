using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
  public  class Receipt_Fixture
    {
        IReceiptService receiptService = new ReceiptServiceImpl();
        ZeoContext alloycontext = new ZeoContext();
        [Test]
        public void Can_Get_Billpay_Receipt_Data()
        {
            BillpayReceiptData billpayReceiptData = receiptService.GetBillpayReceiptData(100000006, Helper.TransactionType.BillPayment.ToString(), (int)Common.Util.Helper.ProviderId.WesternUnionBillPay, false, alloycontext);
            Assert.IsNotNull(billpayReceiptData);
        }
        [Test]
        public void Can_Get_Check_Receipt_Data()
        {
            ProcessCheckReceiptData processCheckReceiptData = receiptService.GetCheckReceiptData(1000000001, alloycontext);
            Assert.IsNotNull(processCheckReceiptData);
        }

        [Test]
        public void Can_Get_Money_Order_Receipt_Data()
        {
            MoneyOrderReceiptData moneyOrderReceiptData = receiptService.GetMoneyOrderReceiptData(100000001, alloycontext);
            Assert.IsNotNull(moneyOrderReceiptData);
        }

        [Test]
        public void Can_Get_Money_Transfer_Receipt_Data()
        {
            MoneyTransferReceiptData moneyTransferReceiptData = receiptService.GetMoneyTransferReceiptData(1000000001, Helper.TransactionType.MoneyTransfer.ToString(),(int)Common.Util.Helper.ProviderId.WesternUnion, alloycontext,false);
            Assert.IsNotNull(moneyTransferReceiptData);
        }

        [Test]
        public void Can_Get_Shopping_Cart_Receipt_Data()
        {
            ShoppingCartSummeryReceiptData summeryReceiptData = receiptService.GetShoppingCartReceiptData(1000000004, alloycontext);
            Assert.IsNotNull(summeryReceiptData);
        }

        [Test]
        public void Can_Get_Cash_Drawer_Receipt_Data()
        {
            CashDrawerReceiptData cashdrawerReceiptData = receiptService.GetCashDrawerReceiptData(500001, 1000000003, alloycontext);
            Assert.IsNotNull(cashdrawerReceiptData);
        }

    }
}
