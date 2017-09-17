using TCF.Zeo.Common.Data;
using NUnit.Framework;
using TCF.Zeo.Cxn.Fund.Visa.Impl;
using TCF.Zeo.Cxn.Fund.Contract;
using TCF.Zeo.Cxn.Fund.Data;

namespace TCF.Zeo.Cxn.Fund.Visa.Test
{
    [TestFixture]
    public class FundService_Fixture
    {
        public ZeoContext MgiContext { get; set; }
        IFundProcessor _fundProcessor = new Gateway();
        [Test]
        public void Can_GetVisaShippingFee()
        {
            IFundProcessor _fundProcessor = new Gateway();
            ZeoContext context = GetContext();
            CardMaintenanceInfo cardMaintenanceInfo = new CardMaintenanceInfo();
            cardMaintenanceInfo.ShippingType = "Express Shipping";
            double shippingFee = _fundProcessor.GetShippingFee(cardMaintenanceInfo, context);
            Assert.IsNotNullOrEmpty(shippingFee.ToString());
        }
        [Test]
        public void Can_GetFundFee()
        {
            ZeoContext context = GetContext();
            CardMaintenanceInfo cardinfo = new CardMaintenanceInfo();
            cardinfo.CardStatus = "5";
            double fee = _fundProcessor.GetFundFee(cardinfo, context);
            Assert.IsNotNull(fee);
        }

        [Test]
        public void Can_Cancel()
        {
            ZeoContext context = GetContext();
            _fundProcessor.Cancel(context);
        }

        [Test]
        public void Can_Register()
        {
            IFundProcessor _fundProcessor = new Gateway();
            ZeoContext context = GetContext();
            CardAccount cardAccount = new CardAccount();
            cardAccount.ExpirationDate = "03/2018";
            cardAccount.PseudoDDA = "39900000000048092";
            cardAccount.CardNumber = "4855078900078944";
            long account = _fundProcessor.Register(cardAccount, context);
            Assert.IsNotNullOrEmpty(account.ToString());
        }


        [Test]
        public void can_Activate()
        {
            ZeoContext context = GetContext();
            FundRequest fundRequest = new FundRequest();
            fundRequest.RequestType = "Credit";
            long activate = _fundProcessor.Activate(fundRequest, context);
            Assert.IsNotNullOrEmpty(activate.ToString());
        }

        [Test]
        public void Can_Commit()
        {
            ZeoContext context = GetContext();
            string cardnumber = "";
            long transactionId = 100000000;
            CustomerInfo customer = new CustomerInfo();
            _fundProcessor.Commit(transactionId, customer, context, cardnumber);
        }


        [Test]
        public void Can_CloseAccount()
        {
            IFundProcessor _fundProcessor = new Gateway();
            ZeoContext context = GetContext();
            bool closeAccount = _fundProcessor.CloseAccount(context);
            Assert.IsTrue(closeAccount);
        }


        private ZeoContext GetContext()
        {
            ZeoContext mgiContext = new ZeoContext()
            {
                CustomerId = 1000000000000010,
                ChannelPartnerId = 34,
                CheckUserName = "INGO",
                URL = "https://proxy.ic.local/ingo/webservice/",
                IngoBranchId = 12345,
                CompanyToken = "simulator",
                EmployeeId = 12345,
                AgentId = 123344455,
                ProviderId = 34,
                CustomerSessionId = 1000000002
            };
            return mgiContext;
        }

        [Test]
        public void Can_GetCardNumber()
        {
            _fundProcessor = new Gateway();
            ZeoContext context = GetContext();
            string cardNumber = _fundProcessor.GetPrepaidCardNumber(context);
            Assert.IsNotEmpty(cardNumber);
        }
    }
}
