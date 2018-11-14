using data = TCF.Zeo.Cxn.Fund.Data;
using NUnit.Framework;
using System.Collections.Generic;
using TCF.Zeo.Cxn.Fund.Data;
using TCF.Zeo.Cxn.Fund.Visa.Impl;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Cxn.Fund.Visa.Test
{
    [TestFixture]
    public class IOFixtures
    {
        IO VisaIO = new IO();
        ZeoContext context = new ZeoContext()
        {
            VisaLocationNodeId = 69685
        };
        #region Unit test cases
        [Test]
        public void Can_Diagnostics()
        {
            data.Credential credential = BuildCredential();
            VisaIO.Diagnostics(credential);
        }

        [Test]
        public void Can_GetCardInfoByProxyId()
        {
            List<string> proxys = new List<string>(){
                "0000000000080506056"
            };
            string proxyId = string.Empty;
            foreach (var proxy in proxys)
            {
                proxyId = proxy;
                data.Credential credential = BuildCredential();
                data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential, context);

                Assert.That(cardInfo.AliasId, Is.AtLeast(1));
                Assert.That(cardInfo.AliasId, Is.AtLeast(20754703));
            }
        }

        [Test]
        public void Can_GetPsedoDDAFromAliasId()
        {
            data.Credential credential = BuildCredential();
            string proxyId = "79621773";
            data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential, context);
            string pseudoDDA = VisaIO.GetPsedoDDAFromAliasId(cardInfo.AliasId, credential, context);

            Assert.That(pseudoDDA, Is.Not.Null);
            Assert.That(pseudoDDA, Is.EqualTo("39900000000023860"));
        }

        [Test]
        public void Can_GetBalance()
        {
            data.Credential credential = BuildCredential();
            string proxyId = "0000000000080481252";
            data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential, context);

            long amountToLoad = 30;
            data.LoadResponse loadResponse = VisaIO.Load(cardInfo.AliasId, amountToLoad, credential, context);

            data.CardBalanceInfo cardBalance = VisaIO.GetBalance(cardInfo.AliasId, credential, context);

            Assert.That(cardBalance, Is.Not.Null);
            Assert.That(cardBalance.Balance, Is.GreaterThan(0));
        }

        [Test]
        [ExpectedException(ExpectedMessage = "Limit violation detected")]
        public void Can_not_LoadToCard_When_Limit_Exceeds()
        {
            string proxyId = "79621773";
            data.Credential credential = BuildCredential();
            credential.VisaLocationNodeId = 69685;
            data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential, context);
            long amountToLoad = 5;

            data.LoadResponse loadResponse = VisaIO.Load(cardInfo.AliasId, amountToLoad, credential, context);

            Assert.That(loadResponse, Is.Not.Null);
            Assert.That(loadResponse.TransactionKey, Is.EqualTo(0.0));
        }

        [Test]
        public void Can_WithdrawToCard()
        {
            string proxyId = "79614558";
            data.Credential credential = BuildCredential();
            credential.VisaLocationNodeId = 69685;
            data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential, context);
            data.CardBalanceInfo bal = VisaIO.GetBalance(cardInfo.AliasId, credential, context);
            long amountToWithdraw = 30;
            LoadResponse loadResponse = VisaIO.Load(cardInfo.AliasId, amountToWithdraw, credential, context);
            VisaIO.Withdraw(cardInfo.AliasId, amountToWithdraw, credential, context);
            bal = VisaIO.GetBalance(cardInfo.AliasId, credential, context);

            Assert.That(true, Is.True);
        }


        [Test]
        public void TransactionHistory()
        {
            string proxyId = "79614558";
            data.Credential credential = BuildCredential();
            data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential, context);
            List<TransactionHistory> trx = VisaIO.GetTransactionHistory(new TransactionHistoryRequest() { AliasId = cardInfo.AliasId, DateRange = 60, TransactionStatus = Helper.TransactionStatus.Posted }, credential, context);

            Assert.That(trx, Is.Not.Null);
        }

        [Test]
        public void Can_CloseAccount()
        {
            string proxyId = "79620078";
            data.Credential credential = BuildCredential();
            data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential, context);

            Assert.That(cardInfo.Status, Is.Not.EqualTo("9"));

            long aliasId = 20754305;

            bool couldClose = VisaIO.CloseAccount(aliasId, credential, context);

            Assert.That(couldClose, Is.True);

            cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential, context);
            Assert.That(cardInfo.Status, Is.EqualTo("9"));

        }

        [Test]
        public void Can_UpdateCardStatus()
        {
            data.Credential credential = BuildCredential();

            long aliasId = 20753003;
            string cardStatus = "5";

            bool isStatusChanged = VisaIO.UpdateCardStatus(aliasId, cardStatus, credential, context);

            Assert.That(isStatusChanged, Is.True);

            string proxyId = "79613979";

            data.CardInfo cardInfo = VisaIO.GetCardInfoByProxyId(proxyId, credential, context);

            Assert.That(cardInfo.Status, Is.EqualTo("5"));

        }

        [Test]
        public void Can_GetCardInfoByCardNumber()
        {
            string cardNumber = "4756757000182512";
            data.Credential credential = BuildCredential();
            data.CardInfo cardInfo = VisaIO.GetCardInfoByCardNumber(cardNumber, credential, context);

            Assert.That(cardInfo.AliasId, Is.AtLeast(1));
            Assert.That(cardInfo.SSN, Is.Not.Empty);
            //Assert.That(cardInfo.AliasId, Is.AtLeast(20754703));
        }

        [Test]
        public void Can_GetCardHolderInfo()
        {
            long aliasId = 90000001966853;
            data.Credential credential = BuildCredential();
            data.CardInfo cardInfo = VisaIO.GetCardHolderInfo(aliasId, credential, context);

            Assert.That(cardInfo.AliasId, Is.AtLeast(1));
        }

        [Test]
        public void Can_GetAccountInfo()
        {
            ZeoContext context = new ZeoContext();
            long aliasId = 20754707;
            data.Credential credential = BuildCredential();
            VisaIO.GetAccountholderInfo(aliasId, credential, context);
        }

        #endregion

        #region Private Methods

        private data.Credential BuildCredential()
        {
            data.Credential credential = new data.Credential()
            {
                ServiceUrl = "https://proxy.ic.local/visa/websrv_prepaid/v16_10/prepaidservices",
                CertificateName = "TCF Nexxo Web Services (CTE WSI)",
                UserName = "prc1279.webserv",
                Password = "pKzWRV24r4",
                ClientNodeId = 12081,
                CardProgramNodeId = 250012,
                SubClientNodeId = -1,
                StockId = "127CS201",
                ChannelPartnerId = 34,
                VisaLocationNodeId = 69685
            };

            return credential;
        }

        #endregion
    }
}
