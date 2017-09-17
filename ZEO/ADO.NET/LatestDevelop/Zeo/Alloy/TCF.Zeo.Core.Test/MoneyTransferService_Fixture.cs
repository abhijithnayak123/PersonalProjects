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
    public class MoneyTransferService_Fixture
    {
        ZeoContext alloycontext = new ZeoContext();
        IMoneyTransferService moneyTransfer = new ZeoCoreImpl();

        [Test]
        public void Can_CreateMoneyTransferTransaction()
        {
            MoneyTransfer moneyTran = new MoneyTransfer()
            {
                CustomerSessionId = 1000000020
               ,
                ProviderAccountId = 1000000008
               ,
                Amount = 40
               ,
                Fee = 10
               ,
                Description = "Test"
               ,
                ConfirmationNumber = "1234"
               ,
                RecipientId = 1000000019
               ,
                ExchangeRate = 12
               ,
                MoneyTransferType = Common.Util.Helper.MoneyTransferType.Send
               ,
                TransactionSubType = Common.Util.Helper.TransactionSubType.Modify
               ,
                OriginalTransactionID = 1000000001
               ,
                ProviderId = 401
               ,
                Destination = "Test Destination"
               ,
                State = Common.Util.Helper.TransactionStates.Authorized
               ,
                DTServerCreate = DateTime.Now
               ,
                DTTerminalCreate = DateTime.Now
            };

            long transactionId = moneyTransfer.CreateTransaction(moneyTran, alloycontext);
            Assert.IsNotEmpty(transactionId.ToString());
        }

        [Test]
        public void Can_UpdateMoneyTransferTransaction()
        {
            MoneyTransfer moneyTran = new MoneyTransfer()
            {
                Id = 1000000001
               ,
                State = Common.Util.Helper.TransactionStates.Authorized
               ,
                DTServerLastModified = DateTime.Now
               ,
                DTTerminalLastModified = DateTime.Now
            };

            moneyTransfer.UpdateTransaction(moneyTran, alloycontext);
            Assert.IsTrue(moneyTran != null);
        }

        [Test]
        public void Can_GetMoneyTransferTransaction()
        {
            ZeoContext alloycontext = new ZeoContext();
            long tranId = 1000000001;
            var mtTran = moneyTransfer.GetTransaction(tranId, alloycontext);
            Assert.IsNotNull(mtTran);
        }

        [Test]
        public void Can_UpdateTransactionState()
        {
            long tranId = 1000000001;
            moneyTransfer.UpdateTransactionState(tranId, 2, DateTime.Now, DateTime.Now, alloycontext);
            Assert.IsNotNull(tranId);
        }

        [Test]
        public void Can_UpdateTransactionStates()
        {
            ZeoContext alloycontext = new ZeoContext();
            long tranId = 1000000001;
            long mtranId = 1000000002;
            DateTime dtServerDate = DateTime.Now;
            DateTime dtTerminalDate = DateTime.Now;

            moneyTransfer.UpdateTransactionStates(mtranId, 2, dtServerDate, dtTerminalDate, alloycontext);
            Assert.IsNotNull(mtranId);
        }

        [Test]
        public void Can_AddModifyandRefundTransactions()
        {
            ZeoContext alloycontext = new ZeoContext();
            MoneyTransfer moneyTran = new MoneyTransfer()
            {
                Id = 1000000001
                ,
                CustomerSessionId = 1000000020
                ,
                RecipientId = 1000000019
                ,
                State = Common.Util.Helper.TransactionStates.Authorized
                ,
                TransactionSubType = Common.Util.Helper.TransactionSubType.Modify
                ,
                OriginalTransactionID = 1000000001
                ,
                DTServerCreate = DateTime.Now
                ,
                DTTerminalCreate = DateTime.Now
            };

            moneyTransfer.AddModifyandRefundTransactions(moneyTran, alloycontext);
            Assert.IsNotNull(moneyTran);
        }
    }
}
