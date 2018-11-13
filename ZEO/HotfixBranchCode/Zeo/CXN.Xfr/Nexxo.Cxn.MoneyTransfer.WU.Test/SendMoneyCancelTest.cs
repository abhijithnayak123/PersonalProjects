using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Testing.NUnit;
//using MGI.Cxn.MoneyTransfer.WU.Impl.ReceiveMoneySearch;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using NHibernate;
using NHibernate.Context;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.WU.Common.Data;
using MGI.Cxn.MoneyTransfer.Contract;

namespace MGI.Cxn.MoneyTransfer.WU.Test
{
	class SendMoneyCancelTest : AbstractTransactionalSpringContextTests
	{
		public IWUMoneyTransferIO CXNMoneyTransferIOSetup { get; set; }
		public IMoneyTransfer CXNMoneyTransfer { get; set; }
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.MoneyTransfer.WU.Test/MGI.Cxn.MoneyTransfer.WU.Test/CXNTestSpring.xml" }; }
		}

		[Test]
		public void Can_Get_PayStatus()
		{
			PaymentSearchRequest paymentsearchrequest = new PaymentSearchRequest();
			paymentsearchrequest.mtcn = "3680362863";
			long transactionId = 1000000187;
			Dictionary<string, object> context = new Dictionary<string, object>();
			context.Add("ChannelPartnerId", 33);
			DasInquiryRequest dasinquiryrequest = new DasInquiryRequest();
			dasinquiryrequest.QueryFilter1 = "en";
			dasinquiryrequest.QueryFilter2 = "RCAN";
			CXNMoneyTransferIOSetup.GetDasReasonList(context, dasinquiryrequest);
		}

		[Test]
		public void Can_Search()
		{
            SearchRequest searchRefundrequest = new SearchRequest();
            searchRefundrequest.Mtcn = "6080378209"; //"3490335290";//"7940377942";
            long transacionID = 1000002413;
            string refundCancelflag;
            searchRefundrequest.sendmoneytransactiontype = SendMoneyTransactionType.agentcsc_flags.REFUND;

			Dictionary<string, object> context = new Dictionary<string, object>();
			context.Add("ChannelPartnerId", 33);
            context.Add("TimeZone", "Central Mountain Time");
            context.Add("ProviderID", 301);
            //CXNMoneyTransferIOSetup.Search(Searchrequest, context);
            
            long newTranID = CXNMoneyTransfer.SearchSendMoneyRefund(transacionID, searchRefundrequest, out refundCancelflag, context);
            SetComplete();
            Assert.IsTrue(newTranID > 0);
            
		}
		[Test]
		public void Can_Refund_SendMoney()
		{
			RefundRequest RefundRequest = new Data.RefundRequest();
			RefundRequest.sender = new Cxn.WU.Common.Data.Sender();
			RefundRequest.sender.NameType = WUEnums.name_type.D;
			RefundRequest.sender.FirstName = "KARUNAKARAN";
			RefundRequest.sender.LastName = "PANNIRSELVAM";
			RefundRequest.sender.AddressCity = "BURLINGAME";
			RefundRequest.sender.AddressState = "CA";
			RefundRequest.sender.AddressStateZip = "94010";
			RefundRequest.sender.AddressStreet = "111 ANZA BLVD";
			RefundRequest.sender.CountryCode = "US";
			RefundRequest.sender.CurrencyCode = "USD";
			RefundRequest.sender.ContactPhone = "6505803551";
			RefundRequest.receiver = new Cxn.WU.Common.Data.Receiver();
			RefundRequest.receiver.NameType = WUEnums.name_type.D;
			RefundRequest.receiver.FirstName = "RAMAA";
			RefundRequest.receiver.LastName = "SHRINIVASAN";
			RefundRequest.receiver.CountryCode = "US";
			RefundRequest.receiver.CurrencyCode = "USD";
			RefundRequest.receiver.ContactPhone = "9945367626";
			RefundRequest.paymentdetails = new Cxn.WU.Common.Data.PaymentDetails();
			RefundRequest.paymentdetails.expectedPayoutLoc_StateCode = "CA";
			RefundRequest.paymentdetails.recording_country_currency = new CountryCurrencyInfo();
			RefundRequest.paymentdetails.recording_country_currency.country_code = "US";
			RefundRequest.paymentdetails.recording_country_currency.currency_code = "USD";
			RefundRequest.paymentdetails.destination_country_currency = new CountryCurrencyInfo();
			RefundRequest.paymentdetails.destination_country_currency.country_code = "US";
			RefundRequest.paymentdetails.destination_country_currency.currency_code = "USD";
			RefundRequest.paymentdetails.originating_country_currency = new CountryCurrencyInfo();
			RefundRequest.paymentdetails.originating_country_currency.country_code = "US";
			RefundRequest.paymentdetails.originating_country_currency.currency_code = "USD";
			RefundRequest.paymentdetails.transaction_type = WUEnums.Transaction_type.WMN;
			RefundRequest.paymentdetails.Exchange_Rate = 1.0000000;
			RefundRequest.paymentdetails.duplicate_detection_flag = "D";
			RefundRequest.financials = new Financials();
			RefundRequest.financials.originators_principal_amount = 22200;
			RefundRequest.financials.destination_principal_amount = 22200;
			RefundRequest.financials.destination_principal_amountSpecified = true;
			RefundRequest.financials.gross_total_amount = 24600;
			RefundRequest.financials.pay_amount = 0;
			RefundRequest.financials.principal_amount = 22200;
			RefundRequest.financials.charges = 2400;
            RefundRequest.mtcn = "7940377942";
            RefundRequest.newmtcn = "1405081130583698";
			RefundRequest.EncompassReasonCode = "RCM";
			RefundRequest.Comments = "Refund Money Transfer Test";
            //RefundRequest.money_transfer_key = "2056491649";
            //RefundRequest.EncompassReasonCode = "RCM";
			Dictionary<string, object> context = new Dictionary<string, object>();
			context.Add("ChannelPartnerId", 33);
            context.Add("TimeZone", "Central Mountain Time");

            CXNMoneyTransfer.SendMoneyRefund(1000000867, RefundRequest, context);
            //CXNMoneyTransfer.SendMoneyRefund(1000002336, RefundRequest, context);
			//CXNMoneyTransferIOSetup.SendMoneyRefund("RCM", "Money not available by date on receipt", "5320495588", context);
		}

        [Test]
        public void RefundSendMoneyTest()
        {
            RefundRequest RefundRequest = new Data.RefundRequest();
            
            RefundRequest.EncompassReasonCode = "RCM";
            RefundRequest.Comments = "Refund Money Transfer Test";
            RefundRequest.Mtcn = "6080378209";
            
            Dictionary<string, object> context = new Dictionary<string, object>();
            context.Add("ChannelPartnerId", 33);
            context.Add("TimeZone", "Central Mountain Time");
            CXNMoneyTransfer.SendMoneyRefund(1000002416, RefundRequest, context);

        }
	}
}
