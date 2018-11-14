using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Cxn.BillPay.WU.Impl;
using TCF.Zeo.Cxn.BillPay.WU.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.BillPay.Data;
using TCF.Zeo.Cxn.WU.Common.Data;

namespace TCF.Zeo.Cxn.BillPay.WU.Test
{
    [TestFixture]
    public class BillPayService_Fixture
    {
        IBillPayService bp = new ZeoCoreImpl();

        [Test]
        public void Can_Get_Billers_By_Term()
        {
            ZeoContext context = GetContext();
            int channelPartnerId = 34;
            List<string> products = bp.GetBillers("regi", channelPartnerId, context);
            Assert.IsTrue(products.Count >= 0);
        }

        [Test]
        public void Can_Create_BillPay_Transaction_Test()
        {
            ZeoContext context = GetContext();

            TCF.Zeo.Core.Data.BillPay billpay = new TCF.Zeo.Core.Data.BillPay()
            {
                AccountNumber = "1234561830",
                Amount = 50,
                BillerNameOrCode = "REGIONAL ACCEPTANCE",
                CustomerSessionId = 1000000000,
                Fee = 0,
                ProviderAccountId = 1000587784,
                ProviderId = 401,
                State = 1
            };

            long transactionId = new ZeoCoreImpl().CreateBillPayTransaction(billpay,context);
            Assert.IsNotNullOrEmpty(transactionId.ToString());
        }

        [Test]
        public void Can_Update_BillPay_Transaction_Test()
        {
            ZeoContext context = GetContext();

            bool caughtException = false;
            try
            {
                TCF.Zeo.Core.Data.BillPay billpay = new TCF.Zeo.Core.Data.BillPay()
                {
                    TransactionId = 1000054111,
                    AccountNumber = "1234561830",
                    Amount = 50,
                    BillerNameOrCode = "REGIONAL ACCEPTANCE",
                    CustomerSessionId = 1000000000,
                    ConfirmationNumber = "54874548787"
                };

                new ZeoCoreImpl().UpdateBillPayTransaction(billpay, 34,context);
            }
            catch (Exception e)
            {
                caughtException = true;
            }
            //Assert.IsNotNullOrEmpty(result.ToString());
            Assert.AreNotEqual(true, caughtException);
        }

        [Test]
        public void Can_Update_BillPay_TransactionState_Test()
        {
            ZeoContext context = GetContext();

            bool caughtException = false;
            try
            {
                long transactionId = 100000001;
                int newState = 2;
                string timezone = " ";

                new ZeoCoreImpl().UpdateBillPayTransactionState(transactionId, newState, timezone,context);
            }
            catch (Exception)
            {
                caughtException = true;
            }
            Assert.AreNotEqual(true, caughtException);
        }

        [Test]
        public void Can_Update_Preferred_ProductsAndState_Test()
        {
            ZeoContext context = GetContext();

            bool caughtException = false;
            try
            {
                long transactionId = 100000001;
                int newState = 2;
                string timezone = " ";

                new ZeoCoreImpl().UpdatePreferredProductsAndState(transactionId, newState, timezone,context);
            }
            catch (Exception e)
            {
                caughtException = true;
            }
            Assert.AreNotEqual(true, caughtException);

        }

        [Test]
        public void Can_Delete_FavoriteBiller_Test()
        {
            ZeoContext context = GetContext();

            long customerId = 1000000000000020;
            long ProductId = 100038120;
            string timeZone = "";
            List<FavouriteBiller> Products = new ZeoCoreImpl().DeleteFavouriteBiller(ProductId, customerId, timeZone,context);
            Assert.NotNull(Products);
        }

        [Test]
        public void Can_Get_Biller_Info_Test()
        {
            ZeoContext context = GetContext();
            string billerName = "REGIONAL ACCEPTANCE";
            long customerId = 542215454545;
            int channelPartnerId = 34;
            FavouriteBiller biller = new ZeoCoreImpl().GetBillerDetails(billerName, customerId, channelPartnerId,context);
            Assert.NotNull(biller);
        }

        [Test]
        public void Can_Get_WUBillPay_Account_Test()
        {

            long customerSessionId = 1000000000;
            string timeZone = "";
            long billPayAccountId = new ProcessorDAL().GetWUBillPayAccount(customerSessionId, timeZone);
            Assert.AreEqual(true, billPayAccountId > 0);
        }

        [Test]
        public void Can_Get_TransactionById_Test()
        {
            long transactionId = 1000000000;
            BillPayTransaction transaction = new ProcessorDAL().GetTransactionById(transactionId);
            Assert.NotNull(transaction);
        }

        [Test]
        public void Can_Create_Or_Update_BillPay_WUTransaction_Test()
        {
            ZeoContext context = new ZeoContext();

            string timeZone = "";
            WUTransaction trx = new WUTransaction()
            {
                WUAccountId = 1000000000,
                ProviderId = 401,
                ChannelType = "",
                ChannelName = "",
                ChannelVersion = "",
                SenderFirstName = "Pushkal",
                SenderLastname = "",
                SenderAddressLine1 = "",
                SenderCity = "Hyd",
                SenderState = "",
                SenderPostalCode = "",
                SenderAddressLine2 = "",
                SenderStreet = "",
                WesternUnionCardNumber = "548751212121",
                SenderDateOfBirth = "",
                BillerName = "REGIONAL ACCEPTANCE",
                CustomerAccountNumber = "",
                ForeignRemoteSystemCounterId = "",
                ForeignRemoteSystemIdentifier = "",
                ForeignRemoteSystemReferenceNo = "",
                Id= 100000001,
                DTServerCreate=DateTime.Now,
                DTTerminalCreate= Helper.GetTimeZoneTime(timeZone)

            };
            long id = new ProcessorDAL().CreateOrUpdateBillPayWUTransaction(trx);
            Assert.IsTrue(id > 0);
        }
        [Test]
        public void Can_Update_WUBP_Transaction_Test()
        {
            ZeoContext context = new ZeoContext();

            bool isException = false;
            try
            {
                WUTransaction trx = new WUTransaction()
                {
                    Id = 100000001,
                    SenderCountryCode = "",
                    SenderCurrencyCode = "",
                    LevelCode = "",
                    SenderEmail = "",
                    SenderContactPhone = "",
                    BillerCityCode = "",
                    CountryCode = "",
                    CurrencyCode = "",
                    FinancialsOriginatorsPrincipalAmount = 111,
                    FinancialsDestinationPrincipalAmount = 111,
                    FinancialsGrossTotalAmount = 111,
                    FinancialsFee = 111,
                    //FinancialsTotal                                = "",
                    FinancialsDiscountedCharges = 2,
                    FinancialsUndiscountedCharges = 5,
                    FinancialsTotalDiscount = 78,
                    FinancialsPlusChargesAmount = 67,
                    PaymentDetailsRecordingCountryCode = "",
                    PaymentDetailsRecordingCountryCurrency = "",
                    PaymentDetailsDestinationCountryCode = "",
                    PaymentDetailsDestinationCountryCurrency = "",
                    PaymentDetailsOriginatingCountryCode = "",
                    PaymentDetailsOriginatingCountryCurrency = "",
                    PaymentDetailsOriginatingCity = "",
                    PaymentDetailsOriginatingState = "",
                    PaymentDetailsTransactionType = "",
                    PaymentDetailsPaymentType = "",
                    PaymentDetailsExchangeRate = 0,
                    PaymentDetailsFixOnSend = "",
                    PaymentDetailsReceiptOptOut = "",
                    PaymentDetailsAuthStatus = "",
                    FillingDate = "",
                    FillingTime = "",
                    Mtcn = "",
                    NewMTCN = "",
                    DfFieldsDeliveryServiceName = "",
                    DfFieldsTransactionFlag = "",
                    DfFieldsPdsrequiredflag = "",
                    DeliveryCode = "",
                    FusionScreen = "",
                    ConvSessionCookie = "",
                    InstantNotificationAddlServiceCharges = "",
                    InstantNotificationAddlServiceLength = 46,
                    PromotionsCouponsPromotions = "",
                    PromotionsPromoCodeDescription = "",
                    PromotionsPromoSequenceNo = "",
                    PromotionsPromoMessage = "",
                    PromotionsPromoName = "",
                    PromotionsPromoDiscountAmount = 25,
                    PromotionsPromotionError = "",
                    PromotionsSenderPromoCode = "",
                    SenderComplianceDetailsTemplateID = "",
                    SenderComplianceDetailsIdDetailsIdType = "",
                    SenderComplianceDetailsIdDetailsIdNumber = "",
                    SenderComplianceDetailsIdDetailsIdPlaceOfIssue = "",
                    SenderComplianceDetailsIdDetailsIdCountryOfIssue = "",
                    SenderComplianceDetailsSecondIDIdType = "",
                    SenderComplianceDetailsSecondIDIdNumber = "",
                    SenderComplianceDetailsSecondIDIdCountryOfIssue = "",
                    SenderComplianceDetailsDateOfBirth = "",
                    SenderComplianceDetailsOccupation = "",
                    SenderComplianceDetailsAckFlag = "",
                    SenderComplianceDetailsIActOnMyBehalf = "",
                    SenderComplianceDetailsCurrentAddressAddrLine1 = "",
                    SenderComplianceDetailsCurrentAddressAddrLine2 = "",
                    SenderComplianceDetailsCurrentAddressCity = "",
                    SenderComplianceDetailsCurrentAddressStateCode = "",
                    SenderComplianceDetailsCurrentAddressPostalCode = "",
                    SenderComplianceDetailsContactPhone = "",
                    SenderComplianceDetailsComplianceDataBuffer = "",
                    QPCompanyDepartment = "Department"
                };
                new ProcessorDAL().UpdateWUBillPayTransaction(trx);

            }
            catch (Exception ex)
            {
                isException = true;
            }
            Assert.AreEqual(isException, false);

        }
        [Test]
        public void Can_Get_WU_BillPay_Request_Test()
        {
            long transactionId = 100000001;
            long customerSessionId = 1000000000;
            BillPaymentRequest request = new ProcessorDAL().GetWUBillPayRequest(customerSessionId,transactionId);
            Assert.IsNotNull(request);
        }


        [Test]
        public void Can_Commit_Test()
        {

            long transactionId = 100000001;
            ZeoContext context = GetContext();
            WesternUnionGateway westernUnionGateway = new WesternUnionGateway();
            BillPayTransaction billPayTransaction = westernUnionGateway.Commit(transactionId, context);
            Assert.That(billPayTransaction, Is.AtLeast(1));
        }

        [Test]
        public void Can_Get_BillPay_AccountId_Test()
        {
            ZeoContext context = GetContext();
            WesternUnionGateway westernUnionGateway = new WesternUnionGateway();
            long billpayAccountId = westernUnionGateway.GetBillPayAccountId(context.CustomerSessionId, context.TimeZone, context);
            Assert.NotNull(billpayAccountId);
        }

        //[Test]
        //public void Can_Get_Fee_Test()
        //{
        //    string billerName = "REGIONAL ACCEPTANCE";
        //    string accountNumber = "1234561830";
        //    decimal amount = 34;
        //    Location location = new Location();
        //    AlloyContext mgiContext = GetContext();
        //    WesternUnionGateway westernUnionGateway = new WesternUnionGateway();
        //    Fee fee = westernUnionGateway.GetFee(billerName, accountNumber, amount, location, mgiContext);
        //    Assert.NotNull(fee);
        //}

        //[Test]
        //public void Can_Get_Locations_Test()
        //{
        //    string billerName = "REGIONAL ACCEPTANCE";
        //    string accountNumber = "1234561830";
        //    decimal amount = 34;
        //    AlloyContext mgiContext = GetContext();
        //    WesternUnionGateway westernUnionGateway = new WesternUnionGateway();
        //    List<Location> locations = westernUnionGateway.GetLocations(billerName, accountNumber, amount, mgiContext);
        //    Assert.NotNull(locations);
        //}
        [Test]
        public void Can_Validate_Test()
        {
            string billerName = "REGIONAL ACCEPTANCE";
            string accountNumber = "1234561830";
            decimal amount = 34;
            BillPayment billPayment = new BillPayment()
            {
                BillerName= billerName,
                AccountNumber= accountNumber,
                PaymentAmount=amount,
                MetaData= GetMetaData()
            };
            ZeoContext context = GetContext();
            WesternUnionGateway westernUnionGateway = new WesternUnionGateway();
            BillPayValidateResponse billPayValidateResponse = westernUnionGateway.Validate(1000006, billPayment, context);
            Assert.NotNull(billPayValidateResponse);
        }

        [Test]
        public void Can_Get_ProviderAttributes_Test()
        {
            ZeoContext context = GetContext();
            string billerName = "REGIONAL ACCEPTANCE";
            string locationName = "KINGFISH FL";
            WesternUnionGateway westernUnionGateway = new WesternUnionGateway();
            List<Field> field = westernUnionGateway.GetProviderAttributes(billerName, locationName, context);
            Assert.NotNull(field);
        }

        [Test]
        public void Can_BillerInfo_Test()
        {
            ZeoContext context = GetContext();
            string billerName = "REGIONAL ACCEPTANCE";
            WesternUnionGateway westernUnionGateway = new WesternUnionGateway();
            BillerInfo billerInfo = westernUnionGateway.GetBillerInfo(billerName, context);
            Assert.NotNull(billerInfo);
        }

        [Test]
        public void Can_Get_CardInfo_Test()
        {
            string cardNumber = "194034567";

            ZeoContext context = GetContext();

            long customerSessionId = 1000000019;
            context.CustomerSessionId = customerSessionId;
            context.CustomerId = 1000000000000010;
            //context.WUCounterId = "990000402";
            context.ChannelPartnerId = 34;
            context.AgentId = 300000;
            context.AgentFirstName = "System";
            context.AgentLastName = "Administrator";
            context.TimeZone = TimeZone.CurrentTimeZone.StandardName;
            
            WesternUnionGateway westernUnionGateway = new WesternUnionGateway();
            var CardInfo = westernUnionGateway.GetCardInfo(cardNumber, context);
            Assert.NotNull(CardInfo);
        }
        private ZeoContext GetContext()
        {
            ZeoContext context = new ZeoContext()
            {
                ChannelPartnerId = 34,
                CustomerSessionId = 1000000000,
                ProviderId = 34,
                WUCounterId = "990000403",
                ProductType = "billpay",
                Context = GetMetaData(),
                RequestType = Helper.RequestType.Hold.ToString()
            };
            return context;
        }

        private Dictionary<string, object> GetMetaData()
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>()
            {
                { "Location", "" } ,
                { "SessionCookie", "" } ,
                { "AailableBalance", "" } ,
                { "AccountHolder", "" } ,
                { "Attention", "" } ,
                { "Reference", "" },
                { "DateOfBirth", "" } ,
                { "DeliveryCode", "000" }
            };
            return metadata;
        }

    }
}
