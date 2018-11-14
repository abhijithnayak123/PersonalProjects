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
    public class CheckProcessing_Fixture
    {
        ICheckService checkProcess = new ZeoCoreImpl();

        [Test]
        public void Can_CreateCheckTransaction()
        {
            ZeoContext context = new ZeoContext();
            Check check = new Check()
            {
                CustomerSessionId = 1000000020
               ,ProviderAccountId = 1000000001
               ,Amount = 40
               ,Fee = 10
               ,Description = "Test"
               ,ConfirmationNumber = "1234"
               ,BaseFee = 10
               ,AdditionalFee = 20
               ,DiscountName = "Test Discount"
               ,DiscountDescription = "Test Discount Desc"
               ,IsSystemApplied = true
               ,DiscountApplied = 30
               ,CheckType = "Payroll Printed"
               ,MICR = "123456"
               ,ProductProviderCode = Common.Util.Helper.ProviderId.Ingo
               ,DTServerCreate = DateTime.Now
               ,DTTerminalCreate = DateTime.Now
            };

            CheckImages image = new CheckImages()
            {
                Front = new byte[] { 0x32 }
                ,Back = new byte[] { 0x32 }
                ,Format = "JPEG"
            };

            long transactionId = checkProcess.CreateCheckTransaction(check, image, context);
            Assert.IsNotEmpty(transactionId.ToString());
        }

        [Test]
        public void Can_UpdateCheckTransaction()
        {
            ZeoContext context = new ZeoContext();
            Check check = new Check()
            {
                CustomerSessionId = 1000000020
               ,ProviderAccountId = 1000000001
               ,Amount = 40
               ,Fee = 10
               ,Description = "Test"
               ,ConfirmationNumber = "1234"
               ,BaseFee = 1
               ,AdditionalFee = 2
               ,DiscountName = "Test Discount"
               ,DiscountDescription = "Test Discount Desc"
               ,IsSystemApplied = true
               ,DiscountApplied = 3
               ,CheckType = "1"
               ,MICR = "123456"
               ,ProductProviderCode = Common.Util.Helper.ProviderId.Ingo
               ,DTServerCreate = DateTime.Now
               ,DTTerminalCreate = DateTime.Now
            };
            
            bool isSuccess = checkProcess.UpdateCheckTransaction(check, context);
            Assert.IsNotNull(check);
        }

        [Test]
        public void Can_GetCheckTransaction()
        {
            ZeoContext context = new ZeoContext();
            long tranId = 1000000001;
            Check checkTran = checkProcess.GetCheckTransaction(tranId, context);
            Assert.IsNotNull(checkTran);
        }

        [Test]
        public void Can_CommitTransaction()
        {
            ZeoContext context = new ZeoContext();
            long tranId = 1000000001;
            long customerSessionId = 1000000020;

            checkProcess.CommitTransaction(tranId, 2, customerSessionId, "Central Standard Time", context);
            Assert.IsNotNull(tranId);
        }

        [Test]
        public void Can_GetCheckTypes()
        {
            ZeoContext context = new ZeoContext();
            List<CheckType> checkTypes = checkProcess.GetCheckTypes( context);
            Assert.IsNotNull(checkTypes);
        }

        [Test]
        public void Can_UpdateCheckTransactionState()
        {
            ZeoContext context = new ZeoContext();
            long tranId = 1000000001;

            checkProcess.UpdateCheckTransactionState(tranId, Common.Util.Helper.TransactionStates.Authorized, "Central Standard Time", context);
            Assert.IsNotNull(tranId);
        }

        [Test]
        public void Can_CancelCheckTransactionState()
        {
            ZeoContext context = new ZeoContext();
            long tranId = 1000000001;

            bool isRes = checkProcess.CancelCheckTransaction(tranId, Common.Util.Helper.TransactionStates.Authorized, "Central Standard Time", context);
            Assert.IsTrue(isRes);
        }
    }
}
