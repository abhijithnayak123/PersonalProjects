using AlloyCommon = TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.MoneyTransfer.Contract;
using AlloyImpl = TCF.Zeo.Cxn.MoneyTransfer.WU.Impl;
using TCF.Zeo.Cxn.MoneyTransfer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlloyService = TCF.Zeo.Cxn.MoneyTransfer.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Cxn.MoneyTransfer.WU.Search;
using TCF.Zeo.Cxn.MoneyTransfer.WU.Contract;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Test
{
    [TestFixture]
    public class MoneyTransfer_Fixture
    {
        IMoneyTransferService MoneyTransferService = new AlloyImpl.WUGateway();
        public AlloyCommon.ZeoContext context;
        public IIO WUIO { private get; set; }

        [Test]
        public void GetCountriesTest()
        {

            List<AlloyService.MasterData> Countries = MoneyTransferService.GetXfrCountries();
            Assert.IsTrue(Countries.Count > 0);
        }
        [TestCase("US")]
        [TestCase("IN")]
        [TestCase("MX")]
        public void GetStatesTest(string countryCode)
        {
            //string countryCode = "US";
            List<AlloyService.MasterData> Countries = MoneyTransferService.GetXfrStates(countryCode);
            Assert.IsTrue(Countries.Count > 0);
        }

        [TestCase]
        public void GetCitiesTest()
        {
            string stateCode = "MEX";
            List<AlloyService.MasterData> Countries = MoneyTransferService.GetXfrCities(stateCode);
            Assert.IsTrue(Countries.Count > 0);
        }

        //[TestCase]
        //public void SaveReceiverTest()
        //{

        //    AlloyService.Receiver receiver = new AlloyService.Receiver()
        //    {
        //        DeliveryMethod = "Money",
        //        DeliveryOption = "SpeedMoney",
        //        FirstName = "Ashok",
        //        LastName = "LastName",
        //        PickupCity = "Hyderabad",
        //        PickupCountry = "India",
        //        PickupState_Province = "Andhra Pradesh",
        //        SecondLastName = "Gandamaneni",
        //        State_Province = "Andhra Pradesh",
        //        Status = "Active"

        //    };
        //    var saveReceiver = MoneyTransferService.SaveReceiver(receiver, context);
        //    Assert.That(saveReceiver, Is.Not.Null);
        //}
        [Test]
        public void GetReceiverTest()
        {
            long customerSessionId = 1000000006;
            var getReceiver = MoneyTransferService.GetReceiver(customerSessionId);
            Assert.That(getReceiver, Is.Not.Null);
        }
        [TestCase]
        public void GetActiveReceiverTest()
        {
            long Id = 1000000000;
            var getActiveReceiver = MoneyTransferService.GetReceiver(Id);
            Assert.That(getActiveReceiver, Is.Not.Null);
        }
        [Test]
        public void GetFrequentreceiversTest()
        {
            long customerId = 1000000000000070;
            var getFrequentReceivers = MoneyTransferService.GetFrequentReceivers(customerId);
            Assert.IsTrue(getFrequentReceivers.Count > 0);
        }

        //[Test]
        //public void GetReceiversTest()
        //{
        //    long customerSessionId = 1000000006;
        //    string FullName = "Ashok LastName";
        //    var getReceiver = MoneyTransferService.GetReceivers(customerSessionId, FullName);
        //    Assert.That(getReceiver.Count > 0);
        //}
        //[TestCase]
        //public void UpdateWUAccountTest()
        //{
        //    AlloyService.WUAccount WUAccount = new AlloyService.WUAccount()
        //    {
        //        CustomerId = 1000000006,
        //        NameType = "",
        //        CustomerSessionId = 1000000006,
        //        PreferredCustomerAccountNumber = "",
        //        PreferredCustomerLevelCode = ""
        //    };
        //    var updateWUAccount = MoneyTransferService.UpdateWUAccount(WUAccount);
        //    Assert.That(updateWUAccount, Is.Not.Null);
        //}
        [Test]
        public void Can_CreateAccount()
        {
            AlloyService.WUAccount WUAccount = new AlloyService.WUAccount()
            {
                CustomerId = 1000000000000010,
                NameType = "D",
                CustomerSessionId = 1000000070,
                PreferredCustomerAccountNumber = "196659595",
                PreferredCustomerLevelCode = "",
                DTServerCreate = DateTime.Now,
                DTTerminalCreate = DateTime.Now

            };
            var createWUAccount = MoneyTransferService.AddAccount(new MoneyTransfer.Data.Account(), context);
            Assert.That(createWUAccount, Is.Not.Null);
        }

        [Test]
        public void Can_AddReceiver()
        {
            AlloyService.Receiver receiver = new AlloyService.Receiver()
            {
                Id = 1000000006,
                DeliveryMethod = "Money",
                DeliveryOption = null,
                FirstName = "phil",
                LastName = "chang",
                PickupCity = null,
                PickupCountry = "AI",
                PickupState_Province = null,
                SecondLastName = null,
                State_Province = null,
                Status = "Active",
                DTServerCreate = DateTime.Now,
                DTServerLastModified = DateTime.Now,
                DTTerminalCreate = DateTime.Now,
                DTTerminalLastModified = DateTime.Now

            };
            var addReceiver = MoneyTransferService.AddReceiver(receiver);
            Assert.That(addReceiver, Is.Not.Null);
        }
        [Test]
        public void WUCardEnrollment()
        {
            AlloyService.Account account = new AlloyService.Account()
            {
                Address = "Near KIMS College, BSK 3rd Stage",
                City = "Bangalore",
                ContactPhone = "9292939392",
                Email = "t@t.com",
                FirstName = "Krishnapratap",
                LastName = "Vedula",
                MobilePhone = "9388383848",
                PostalCode = "94010",
                LoyaltyCardNumber = "500584404",
                LevelCode = "1",
                State = "CA",
                SmsNotificationFlag = "True"
            };
            context = new AlloyCommon.ZeoContext();
            context.CustomerSessionId = 1000000006;
            AlloyService.PaymentDetails paymentDetails = new AlloyService.PaymentDetails();
            paymentDetails.TransactionId = 1000000175;

            var wuCardEnrollment = MoneyTransferService.WUCardEnrollment(context);
            Assert.That(wuCardEnrollment, Is.Not.Null);
        }


        [Test]
        public void UpdateReceiverTest()
        {
            AlloyService.Receiver receiver = new AlloyService.Receiver()
            {
                Id = 1000000021,
                DeliveryMethod = "Money",
                DeliveryOption = null,
                FirstName = "sunil",
                LastName = "sahu",
                PickupCity = null,
                PickupCountry = "AR",
                PickupState_Province = null,
                SecondLastName = null,
                State_Province = null,
                Status = "Active",
                DTServerCreate = DateTime.Now,
                DTServerLastModified = DateTime.Now,
                DTTerminalCreate = DateTime.Now,
                DTTerminalLastModified = DateTime.Now

            };
            var updateReceiver = MoneyTransferService.UpdateReceiver(receiver);
            Assert.That(updateReceiver, Is.Not.Null);
        }

        //[Test]
        //public void DeleteReceiverTest()
        //{

        //    long receiverId = 1000000006;
        //    string status = "Active";
        //    //string actualValue = "InActive";
        //    MoneyTransferService.DeleteReceiver(receiverId, status);
        //}
        //[Test]
        //public void GetWUAccountTest()
        //{
        //    long customerSessionId = 1000000006;
        //    var GetWUAccount = MoneyTransferService.GetWUAccount(customerSessionId);
        //    Assert.That(GetWUAccount, Is.Not.Null);
        //}

        [Test]
        public void GetDeliveryServiceTest()
        {
            string state = "California";
            string stateCode = "CA";
            string city = "California";
            string deliveryService = "MoneyinMinutes";
            Dictionary<string, object> Metadata = new Dictionary<string, object>();
            Metadata.Add("State", state);
            Metadata.Add("StateCode", stateCode);
            Metadata.Add("City", city);
            Metadata.Add("DeliveryService", deliveryService);
            context = new AlloyCommon.ZeoContext();
            context.ChannelPartnerId = 34;
            context.WUCounterId = "990000402";
            AlloyService.DeliveryServiceRequest request = new AlloyService.DeliveryServiceRequest()
            {
                CountryCode = "US",
                CountryCurrency = "USD",
                Type = Helper.DeliveryServiceType.Option,
                MetaData= Metadata,

            };
            
            var deliveryServices = new List<AlloyService.DeliveryService>();
           
            var getDeliveryServices = MoneyTransferService.GetDeliveryServices(request, context);
            Assert.That(getDeliveryServices, Is.Not.Null);
        }
        [Test]
        public void GetFeetest()
        {
            AlloyService.DeliveryService ds = new AlloyService.DeliveryService();
            ds.Code = "000";
            ds.Name = "MONEY IN MINUTES";

            AlloyService.FeeRequest feeRequest = new AlloyService.FeeRequest()
            {
                AccountId = 1000000183,
                ReceiverId = 1000000000,

                FeeRequestType = FeeRequestType.AmountIncludingFee,
                Amount = 150.0M,
                // DeliveryService = ds,
                IsDomesticTransfer = true,
                ReceiveAmount = 150.0M,
                PersonalMessage = "This is a test",
                ReceiveCountryCode = "IN",
                ReceiveCountryCurrency = "INR",
                ReceiverFirstName = "A",
                ReceiverLastName = "B",
                MetaData = new Dictionary<string, object>()
            };

            feeRequest.MetaData.Add("StateName", "CALIFORNIA");
            AlloyCommon.ZeoContext context = new AlloyCommon.ZeoContext()
            {
                TimeZone = TimeZone.CurrentTimeZone.StandardName,
                WUCounterId = "9900004",
                ProviderId = 301,
                SMTrxType = RequestType.Hold.ToString(),
                ChannelPartnerId = 33,
            };
            var getFee = MoneyTransferService.GetFee(feeRequest, context);
            Assert.That(getFee, Is.Not.Null);
        }

        [Test]
        public void Validate()
        {
            AlloyService.ValidateRequest validateRequest = new AlloyService.ValidateRequest();
            validateRequest.TransactionId = 1000000000;
            long customerSessionId = 1000000006;
         AlloyCommon.ZeoContext   context = new AlloyCommon.ZeoContext()
            {
                TimeZone = TimeZone.CurrentTimeZone.StandardName,
                WUCounterId = "9900004",
                ProviderId = 301,
                SMTrxType = RequestType.Hold.ToString(),
                ChannelPartnerId = 33,
            };
            var validate = MoneyTransferService.Validate(validateRequest, context);
            Assert.That(validate, Is.Not.Null);
        }
        [Test]
        public void Search()
        {
            AlloyCommon.ZeoContext context = new AlloyCommon.ZeoContext()
            {
                TimeZone = TimeZone.CurrentTimeZone.StandardName,
                WUCounterId = "9900004",
                ProviderId = 301,
                SMTrxType = RequestType.Hold.ToString(),
                ChannelPartnerId = 33,
            };
            TCF.Zeo.Cxn.MoneyTransfer.Data.SearchRequest searchRequest = new AlloyService.SearchRequest();
            searchRequest.SearchRequestType = SearchRequestType.Modify;
            searchRequest.TransactionId = 1000000073;
            searchRequest.ConfirmationNumber = "4750677830";

            var search = MoneyTransferService.Search(searchRequest, context);
            Assert.That(search, Is.Not.Null);
        }
        [Test]
        public void GetStatus()
        {
            string confirmationNumber = "3290868661";
            var getStatus = MoneyTransferService.GetStatus(confirmationNumber, context);
            Assert.That(getStatus, Is.Not.Null);
        }
        [Test]
        public void GetRefundReasons()
        {
            AlloyService.ReasonRequest request = new AlloyService.ReasonRequest();
            request.TransactionType = "ReceiveMoney";
            var getRefundReasons = MoneyTransferService.GetRefundReasons(request, context);
            Assert.That(getRefundReasons, Is.Not.Null);
        }
        [Test]
        public void Modify()
        {
            long transactionId = 1000000005;
            AlloyCommon.ZeoContext context = new AlloyCommon.ZeoContext()
            {
                TimeZone = TimeZone.CurrentTimeZone.StandardName,
                WUCounterId = "9900004",
                ProviderId = 301,
                SMTrxType = RequestType.Hold.ToString(),
                ChannelPartnerId = 33,
            };
            context.CustomerSessionId = 1000000004;
            context.CustomerId = 1000000000000010;
            context.WUCounterId = "990000402";
            context.ChannelPartnerId = 34;
            context.AgentId = 300000;
            context.AgentFirstName = "System";
            context.AgentLastName = "Administrator";
            MoneyTransferService.Modify(transactionId, context);
        }
        [Test]
        public void Refund() //
        {
            TCF.Zeo.Cxn.MoneyTransfer.Data.RefundRequest refundRequest = new AlloyService.RefundRequest();
            refundRequest.TransactionId = 1000000022;
            refundRequest.ReasonCode = "W9203";
            refundRequest.ReasonDesc = "RFD - Wrong Information";
            refundRequest.RefundStatus = "N";
            refundRequest.Comments = "Test";
            refundRequest.TransactionId = 1000000065;
            refundRequest.ReferenceNumber = null;
            AlloyCommon.ZeoContext context = new AlloyCommon.ZeoContext()
            {
                TimeZone = TimeZone.CurrentTimeZone.StandardName,
                WUCounterId = "9900004",
                ProviderId = 301,
                SMTrxType = RequestType.Hold.ToString(),
                ChannelPartnerId = 33,
                CustomerSessionId= 1000000097
            };
            var refund = MoneyTransferService.Refund(refundRequest, context);
            Assert.That(refund, Is.Not.Null);
        }
        [Test]
        public void StageModify()
        {
            AlloyService.ModifyRequest modifyRequest = new AlloyService.ModifyRequest();
            context = new AlloyCommon.ZeoContext();
            long customerSessionId = 1000000038;
            context.CustomerSessionId = customerSessionId;
            context.CustomerId = 1000000000000020;
            context.WUCounterId = "990000402";
            context.ChannelPartnerId = 34;
            context.AgentId = 300000;
            context.AgentFirstName = "System";
            context.AgentLastName = "Administrator";
            context.TimeZone = TimeZone.CurrentTimeZone.StandardName;
            modifyRequest.TransactionId = 1000000009;
            modifyRequest.FirstName = "kiranmaie";
            modifyRequest.LastName = "munagapati";
            modifyRequest.SecondLastName = "SecondLastName";
            modifyRequest.ConfirmationNumber = "6300474926";
            modifyRequest.TestAnswer = "TestAnswer";
            modifyRequest.TestQuestion = "TestQuestion";
            modifyRequest.TestQuestionAvailable = "TestQuestionAvailable";

            var stageModify = MoneyTransferService.StageModify(modifyRequest, context);
            Assert.That(stageModify, Is.Not.Null);
        }
        [Test]
        public void UpdateAccountWithCardNumberTest()
        {
            long customerSessionId = 1000000006;

            AlloyService.Account account = new AlloyService.Account
            {
                LoyaltyCardNumber = "",
                DTServerLastModified = DateTime.Now,
                DTTerminalLastModified = DateTime.Now

            };
            MoneyTransferService.UpdateAccountWithCardNumber(customerSessionId, account);

        }
        [Test]
        public void Commit()
        {
            long transactionId = 1000001244;
            var commit = MoneyTransferService.Commit(transactionId, context);
            Assert.That(commit, Is.Not.Null);
        }

        [Test]
        public void ImportReceiver()
        {
            context = new AlloyCommon.ZeoContext();

            long customerSessionId = 1000000010;
            context.CustomerSessionId = customerSessionId;
            context.CustomerId = 1000000000000010;
            context.WUCounterId = "990000402";
            context.ChannelPartnerId = 34;
            context.AgentId = 300000;
            context.AgentFirstName = "System";
            context.AgentLastName = "Administrator";
            context.TimeZone = TimeZone.CurrentTimeZone.StandardName; 

            string cardNumber = "100205012";
            var isSuccess = MoneyTransferService.GetPastReceivers(customerSessionId, cardNumber, context);
            Assert.IsTrue(isSuccess);
        }

        [Test]
        public void WUCardLookup()
        {
            TCF.Zeo.Cxn.MoneyTransfer.Data.CardLookupDetails cardDetails =
                new MoneyTransfer.Data.CardLookupDetails()
                {
                    FirstName = "ANGELA",
                    LastName = "CHEN"
                };

            context = new AlloyCommon.ZeoContext();

            long customerSessionId = 1000000010;
            context.CustomerSessionId = customerSessionId;
            context.CustomerId = 1000000000000010;
            context.WUCounterId = "990000402";
            context.ChannelPartnerId = 34;
            context.AgentId = 300000;
            context.AgentFirstName = "System";
            context.AgentLastName = "Administrator";
            context.TimeZone = TimeZone.CurrentTimeZone.StandardName;

            var detail = MoneyTransferService.WUCardLookup(cardDetails, context);
            Assert.IsNotNull(detail);
        }

        [Test]
        public void UseGoldcard()
        {
            string WUCardNumber = "500585707";

            context = new AlloyCommon.ZeoContext();

            long customerSessionId = 1000000010;
            context.CustomerSessionId = customerSessionId;
            context.CustomerId = 1000000000000010;
            context.WUCounterId = "990000402";
            context.ChannelPartnerId = 34;
            context.AgentId = 300000;
            context.AgentFirstName = "System";
            context.AgentLastName = "Administrator";
            context.TimeZone = TimeZone.CurrentTimeZone.StandardName;

            var detail = MoneyTransferService.UseGoldcard(WUCardNumber, context);
            Assert.IsNotNull(detail);
        }
		[Test]
        public void can_Get_Banner_Msgs()
        {
           
            context = new AlloyCommon.ZeoContext();
            context.ChannelPartnerId = 34;
            context.WUCounterId = "990000403";
            var deliveryServices = new List<AlloyService.DeliveryService>();

            var getDeliveryServices = MoneyTransferService.GetBannerMsgs(context);
            Assert.That(getDeliveryServices, Is.Not.Null);
        }
    }
}
