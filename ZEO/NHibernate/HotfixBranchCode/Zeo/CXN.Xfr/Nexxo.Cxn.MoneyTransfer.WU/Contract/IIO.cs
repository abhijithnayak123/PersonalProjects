using MGI.Common.Util;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.WU.ReceiveMoneyPay;
using MGI.Cxn.MoneyTransfer.WU.ReceiveMoneySearch;
using MGI.Cxn.MoneyTransfer.WU.Search;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyPayStatus;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyRefund;
using MGI.Cxn.MoneyTransfer.WU.SendMoneyStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.WU.Impl
{
	public interface IIO
	{
		List<MoneyTransfer.Data.DeliveryService> GetDeliveryServices(MoneyTransfer.Data.DeliveryServiceRequest request,
			string state, string stateCode, string city, string deliveryService, MGIContext mgiContext);

		receivemoneysearchreply SearchReceiveMoney(receivemoneysearchrequest receiveMoneySearchRequest, MGIContext mgiContext);

		receivemoneypayreply ReceiveMoneyPay(receivemoneypayrequest receiveMoneyPayRequest, MGIContext mgiContext);

		sendmoneystorereply SendMoneyStore(sendmoneystorerequest sendMoneyStoreRequest, MGIContext mgiContext, out bool hasLPMTError);

		paystatusinquiryreply GetPayStatus(paystatusinquiryrequestdata searchrequest, MGIContext mgiContext);

		ModifySendMoney.modifysendmoneyreply Modify(ModifySendMoney.modifysendmoneyrequest modifySendMoneyRequest, MGIContext mgiContext);

		ModifySendMoneySearch.modifysendmoneysearchreply ModifySearch(ModifySendMoneySearch.modifysendmoneysearchrequest request, MGIContext mgiContext);

		searchreply Search(searchrequest searchRequest, MGIContext mgiContext);

		refundreply Refund(refundrequest refundRequest, MGIContext mgiContext);

		List<MoneyTransfer.Data.Reason> GetRefundReasons(MoneyTransfer.Data.ReasonRequest request, MGIContext mgiContext);

		FeeInquiry.feeinquiryreply FeeInquiry(FeeInquiry.feeinquiryrequest feeInquiryRequest, MGIContext mgiContext);

		SendMoneyValidation.sendmoneyvalidationreply SendMoneyValidate(SendMoneyValidation.sendmoneyvalidationrequest validationRequest, MGIContext mgiContext);

	}
}
