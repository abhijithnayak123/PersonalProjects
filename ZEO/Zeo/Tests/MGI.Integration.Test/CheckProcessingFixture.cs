using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using static TCF.Zeo.Common.Util.Helper;

namespace MGI.Integration.Test
{
    [TestFixture]
    public partial class AlloyIntegrationTestFixture
    {
        #region IngoTest Cases

        [TestCase("TCF")]
        public void DoCheckProcess(string channelPartnerName)
        {
            var result = DoCp(channelPartnerName);

            Assert.That(result, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void RemoveCheckProcess(string channelPartnerName)
        {
            bool isCheckRemoved = RemoveCP(channelPartnerName);

            Assert.That(isCheckRemoved, Is.True);
        }

        [TestCase("TCF")]
        public void ParkCheckProcess(string channelPartnerName)
        {
            bool isCheckParked = ParkCP(channelPartnerName);

            Assert.That(isCheckParked, Is.True);
        }

        [TestCase("TCF")]
        public void UnParkCheckProcess(string channelPartnerName)
        {
            HelperShoppingCartCheckoutStatus status = UnParkingCP(channelPartnerName);

            Assert.That(status, Is.EqualTo(HelperShoppingCartCheckoutStatus.Completed));
        }

        [TestCase("TCF")]
        public void ParkCheckProcessWithManualPromo(string channelParnerName)
        {
            bool isCheckParked = ParkCPWithManualPromo(channelParnerName);

            Assert.That(isCheckParked, Is.True);
        }

        [TestCase("TCF")]
        public void UnParkCheckProcessWithManualPromo(string channelPartnerName)
        {
            HelperShoppingCartCheckoutStatus status = UnParkingCP(channelPartnerName);

            Assert.That(status, Is.EqualTo(HelperShoppingCartCheckoutStatus.Completed));
        }

        #endregion

        #region Private Methods

        private HelperShoppingCartCheckoutStatus UnParkingCP(string channelPartnerName)
        {
            Response response;

            CustomerSession customersession = InitiateCustomerSession(channelPartnerName);

            response = client.GetZeoContextForCustomer(customersession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            response = client.ShoppingCartCheckout(0, HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);

            HelperShoppingCartCheckoutStatus status = (HelperShoppingCartCheckoutStatus)response.Result;

            return status;
        }

        private void PerformCheckProcess(long CustomerSessionId, ZeoContext context)
        {
            ProcesscheckTransaction(CustomerSessionId, "", context);
        }

        private void ProcesscheckTransaction(long customerSessionId, string promoManualCode, ZeoContext zeoContext)
        {
            Response response;

            ZeoContext context = new ZeoContext
            {
                TimeZone = "Eastern Standard Time",
                CompanyToken = "Simulator",
                URL = "http://beta.chexar.net/webservice/"

            };
            CheckData checkData = new CheckData();

            response = client.GetCheckTypes(context);

            var checkTypes = response.Result as List<CheckType>;

            var transactionFee = GetCheckFee(checkTypes, customerSessionId, promoManualCode, zeoContext);

            CheckSubmission checkSubmit = new CheckSubmission()
            {
                IsSystemApplied = transactionFee.IsSystemApplied,
                ImageFormat = "TIFF",
                FrontImageTIFF = Convert.FromBase64String(checkData.FrontImage_Tiff),
                FrontImage = Convert.FromBase64String(checkData.FrontImage),
                BackImageTIFF = Convert.FromBase64String(checkData.BackImage_Tiff),
                BackImage = Convert.FromBase64String(checkData.BackImage),
                MICR = checkData.MICR,
                AccountNumber = checkData.AccountNumber,
                RoutingNumber = checkData.RoutingNumber,
                CheckNumber = checkData.CheckNumber,
                MicrEntryType = (int)CheckEntryTypes.ScanWithImage,
                Fee = transactionFee.BaseFee,
                IssueDate = DateTime.Now,
                CheckType = checkTypes.Where(x => x.Name.ToLower() != "select").FirstOrDefault().Id.ToString(),
                Amount = GetRandomAmount()
            };

            response = client.SubmitCheck(checkSubmit, zeoContext);
        }

        private HelperShoppingCartCheckoutStatus DoCp(string channelPartnerName)
        {
            Response response;

            CustomerSession customersession = InitiateCustomerSession(channelPartnerName);

            response = client.GetZeoContextForCustomer(customersession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformCheckProcess(customersession.CustomerSessionId, zeoContext);

            response = client.ShoppingCartCheckout(0, HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);

            return (HelperShoppingCartCheckoutStatus)response.Result;
        }

        private TransactionFee GetCheckFee(List<CheckType> checkTypes, long customerSessionId, string promoCode, ZeoContext context)
        {
            CheckSubmission checkSubmit = new CheckSubmission();

            if (!string.IsNullOrEmpty(promoCode))
            {
                checkSubmit.PromoCode = promoCode;
                checkSubmit.IsSystemApplied = false;
            }
            else
            {
                checkSubmit.IsSystemApplied = true;
            }

            checkSubmit.CheckType = checkTypes.Where(x => x.Name.ToLower() != "select").FirstOrDefault().Id.ToString();

            checkSubmit.Amount = GetRandomAmount();

            Response getCheckFeeResponse = client.GetCheckFee(checkSubmit, zeoContext);

            TransactionFee CheckFee = getCheckFeeResponse.Result as TransactionFee;

            return CheckFee;
        }

        private bool RemoveCP(string channelPartnerName)
        {
            Response response;

            bool isCheckRemoved = false;

            CustomerSession customersession = InitiateCustomerSession(channelPartnerName);

            response = client.GetZeoContextForCustomer(customersession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformCheckProcess(customersession.CustomerSessionId, zeoContext);

            response = client.GetShoppingCart(customersession.CustomerSessionId, zeoContext);
            ShoppingCart cart = response.Result as ShoppingCart;

            if (cart.Checks.Count > 0)
            {
                long transactionId = cart.Checks.FirstOrDefault(x => x.Status == "2").Id;

                response = client.RemoveCheck(transactionId, zeoContext);

                isCheckRemoved = (bool)response.Result;
            }

            return isCheckRemoved;
        }

        private bool ParkCP(string channelPartnerName)
        {
            Response response;

            bool isCheckParked = false;

            CustomerSession customersession = InitiateCustomerSession(channelPartnerName);

            response = client.GetZeoContextForCustomer(customersession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformCheckProcess(customersession.CustomerSessionId, zeoContext);

            isCheckParked = ParkCheck(customersession.CustomerSessionId, zeoContext);

            return isCheckParked;
        }

        private bool ParkCPWithManualPromo(string channelPartnerName)
        {
            Response response;

            bool isCheckParked = false;

            CustomerSession customersession = InitiateCustomerSession(channelPartnerName);

            response = client.GetZeoContextForCustomer(customersession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformCheckProcessWithPromo(channelPartnerName, customersession.CustomerSessionId, zeoContext);

            isCheckParked = ParkCheck(customersession.CustomerSessionId, zeoContext);

            return isCheckParked;
        }

        private bool ParkCheck(long customerSessionId, ZeoContext zeoContext)
        {
            Response response;

            bool isCheckParked = false;

            response = client.GetShoppingCart(customerSessionId, zeoContext);

            ShoppingCart cart = response.Result as ShoppingCart;

            if (cart.Checks.Count > 0)
            {
                long transactionId = cart.Checks.FirstOrDefault(x => x.Status == "2").Id;

                response = client.ParkShoppingCartTransaction(transactionId, (int)ProductType.Checks, zeoContext);

                isCheckParked = (bool)response.Result;
            }

            return isCheckParked;
        }


        //TODO
        private void PerformCheckProcessWithPromo(string channelPartnerName, long customerSessionId, ZeoContext context)
        {
            string promoManualCode = null;

            if (channelPartnerName == "Synovus")
            {
                promoManualCode = "JewelEmpPayroll";
            }

            ProcesscheckTransaction(customerSessionId, promoManualCode, context);
        }

        #endregion
    }
}
