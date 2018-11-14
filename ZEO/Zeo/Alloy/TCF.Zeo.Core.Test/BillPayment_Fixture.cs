using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class BillPayService_Fixture
    {
        IBillPayService bp = new ZeoCoreImpl();
        [Test]
        public void Can_Get_Billers_By_Term()
        {
            ZeoContext context = new ZeoContext();
            int channelPartnerId = 34;
            List<string> products = bp.GetBillers("regi", channelPartnerId, context);
            Assert.IsTrue(products.Count >= 0);
        }
        [Test]
        public void Can_Get_FrequentBillers_By_CustomerId()
        {
            ZeoContext context = new ZeoContext();

            long customerID = 1000000000000010;
            List<FavouriteBiller> masterCatalogData = bp.GetFrequentBillers(customerID, context);
            Assert.IsTrue(masterCatalogData.Count >= 0);
        }
        [Test]
        public void Can_Create_BillPay_Transaction_Test()
        {
            ZeoContext context = new ZeoContext();
            Data.BillPay billpay = new Data.BillPay()
            {
                AccountNumber = "1234561830",
                Amount = 50,
                BillerNameOrCode = "REGIONAL ACCEPTANCE",
                CustomerSessionId = 1000000000,
                DTServerCreate = DateTime.Now,
                DTTerminalCreate = DateTime.Now,
                Fee = 0,
                ProviderAccountId = 1000587784,
                ProviderId = 401,
                State = 1
            };

            long transactionId = new ZeoCoreImpl().CreateBillPayTransaction(billpay, context);
            Assert.IsNotNullOrEmpty(transactionId.ToString());
        }

        [Test]
        public void Can_Update_BillPay_Transaction_Test()
        {
            ZeoContext context = new ZeoContext();
            bool caughtException = false;
            try
            {
            Data.BillPay billpay = new Data.BillPay()
            {
                TransactionId = 100000001,
                AccountNumber = "1234561830",
                Amount = 70,
                BillerNameOrCode = "REGIONAL ACCEPTANCE",
                CustomerSessionId = 1000000000,
                DTServerLastModified = DateTime.Now,
                DTTerminalLastModified = DateTime.Now,
                ConfirmationNumber = "54874548787"
            };

            new ZeoCoreImpl().UpdateBillPayTransaction(billpay,34, context);
            }
            catch (Exception ex)
            {
                caughtException = true;
            }
            //Assert.IsNotNullOrEmpty(result.ToString());
            Assert.AreNotEqual(true, caughtException);
        }

        [Test]
        public void Can_Update_BillPay_TransactionState_Test()
        {
            ZeoContext context = new ZeoContext();
            bool caughtException = false;
            try
            {
                long transactionId = 100000001;
            int newState = 2;
            string timezone = " ";

            new ZeoCoreImpl().UpdateBillPayTransactionState(transactionId, newState, timezone, context);
            }
            catch (Exception e)
            {
                caughtException = true;
            }
            //Assert.IsNotNullOrEmpty(result.ToString());
            Assert.AreNotEqual(true, caughtException);
        }

        [Test]
        public void Can_Update_Preferred_ProductsAndState_Test()
        {
            ZeoContext context = new ZeoContext();
            bool caughtException = false;
            try
            {
                long transactionId = 100000001;
            int newState = 3;
            string timezone = " ";

            new ZeoCoreImpl().UpdatePreferredProductsAndState(transactionId, newState, timezone, context);
            }
            catch (Exception e)
            {
                caughtException = true;
            }
            //Assert.IsNotNullOrEmpty(result.ToString());
            Assert.AreNotEqual(true, caughtException);

        }

        [Test]
        public void Can_Update_BillPay_TransactionFee_Test()
        {
            ZeoContext context = new ZeoContext();
            bool caughtException = false;
            try
            {
                long transactionId = 100000001;
                int newState = 2;
                string timezone = " ";
                decimal fee = 60;
                decimal amount = 60;
                string confirmationNumber = "1234";

                new ZeoCoreImpl().UpdateBillPayTransactionFee(transactionId, newState,fee,amount,confirmationNumber, timezone, context);
            }
            catch (Exception e)
            {
                caughtException = true;
            }
            //Assert.IsNotNullOrEmpty(result.ToString());
            Assert.AreNotEqual(true, caughtException);

        }

        [Test]
        public void Can_Delete_Favorite_Biller_Test()
        {
            ZeoContext context = new ZeoContext();
            long customerId = 1000000000000010;
            long ProductId = 100026399;
            string timeZone = "";
            List<FavouriteBiller> Products = new ZeoCoreImpl().DeleteFavouriteBiller(ProductId,customerId,timeZone, context);
            Assert.NotNull(Products);
        }
		
        [Test]
        public void Can_Get_BillerInfo_Test()
        {
            ZeoContext context = new ZeoContext();
            string billerName = "REGIONAL ACCEPTANCE";
            long customerId = 1000000000000010;
            int channelPartnerId = 34;
            FavouriteBiller biller = new ZeoCoreImpl().GetBillerDetails(billerName, customerId, channelPartnerId, context);
            Assert.NotNull(biller);
        }
    }
}
