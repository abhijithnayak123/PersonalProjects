using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using TCF.Zeo.Cxn.MoneyTransfer.WU.ReceiveMoneyPay;
using TCF.Zeo.Cxn.MoneyTransfer.WU.ReceiveMoneySearch;
using TCF.Zeo.Cxn.MoneyTransfer.WU.Search;
using TCF.Zeo.Cxn.MoneyTransfer.WU.SendMoneyPayStatus;
using TCF.Zeo.Cxn.MoneyTransfer.WU.SendMoneyRefund;
using TCF.Zeo.Cxn.MoneyTransfer.WU.SendMoneyStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Cxn.MoneyTransfer.Data;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Contract
{
    public interface IIO
    {
        List<DeliveryService> GetDeliveryServices(DeliveryServiceRequest request,
            string state, string stateCode, string city, string deliveryService, ZeoContext context);

        receivemoneysearchreply SearchReceiveMoney(receivemoneysearchrequest receiveMoneySearchRequest, ZeoContext context);

        receivemoneypayreply ReceiveMoneyPay(receivemoneypayrequest receiveMoneyPayRequest, ZeoContext context);

        sendmoneystorereply SendMoneyStore(sendmoneystorerequest sendMoneyStoreRequest, ZeoContext context, out bool hasLPMTError);

        paystatusinquiryreply GetPayStatus(paystatusinquiryrequestdata searchRequest, ZeoContext context);

        ModifySendMoney.modifysendmoneyreply Modify(ModifySendMoney.modifysendmoneyrequest modifySendMoneyRequest, ZeoContext context);

        ModifySendMoneySearch.modifysendmoneysearchreply ModifySearch(ModifySendMoneySearch.modifysendmoneysearchrequest request, ZeoContext context);

        searchreply Search(searchrequest searchRequest, ZeoContext context);

        refundreply Refund(refundrequest refundRequest, ZeoContext context);

        List<MoneyTransfer.Data.Reason> GetRefundReasons(MoneyTransfer.Data.ReasonRequest request, ZeoContext context);

        FeeInquiry.feeinquiryreply FeeInquiry(FeeInquiry.feeinquiryrequest feeInquiryRequest, ZeoContext context);

        SendMoneyValidation.sendmoneyvalidationreply SendMoneyValidate(SendMoneyValidation.sendmoneyvalidationrequest validationRequest, ZeoContext context);
    }
}
