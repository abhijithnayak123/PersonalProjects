using System;
using TCF.Channel.Zeo.Service.Contract;
using System.Collections.Generic;
using TCF.Channel.Zeo.Data;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;
using BizReceipt = TCF.Zeo.Biz.Receipt;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IReceiptService
    {
        TCF.Zeo.Biz.Receipt.Contract.IReceiptService bizReceiptService = new TCF.Zeo.Biz.Receipt.Impl.WUReceiptServiceImpl();
        public Response GetBillpayReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizReceipt.Data.Receipt receipt = bizReceiptService.GetBillPayReceipt(transactionId, isReprint, commonContext);

            Data.Receipt receiptResult = mapper.Map<Data.Receipt>(receipt);

            response.Result = receiptResult;
            return response;
        }

        public Response GetDoddFranckRecipt(long transactionId, bool isReprint, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizReceipt.Data.Receipt receipt = bizReceiptService.GetDoddFrankReceipt(transactionId, commonContext);

            Data.Receipt receiptResult = mapper.Map<Data.Receipt>(receipt);

            response.Result = receiptResult;

            return response;
        }

        public Response GetMoneyOrderReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizReceipt.Data.Receipt receipt = bizReceiptService.GetMoneyOrderReceipt(transactionId, isReprint, commonContext);

            Data.Receipt receiptResult = mapper.Map<Data.Receipt>(receipt);

            response.Result = receiptResult;

            return response;
        }

        public Response GetFundReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizReceipt.Data.Receipt receipt = bizReceiptService.GetFundsReceipt(transactionId, isReprint, commonContext);

            Data.Receipt receiptResult = mapper.Map<Data.Receipt>(receipt);

            response.Result = receiptResult;

            return response;
        }

        public Response GetMoneyTransferReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();


            BizReceipt.Data.Receipt receipt = bizReceiptService.GetMoneyTransferReceipt(transactionId, isReprint, commonContext);

            Data.Receipt receiptResult = mapper.Map<Data.Receipt>(receipt);

            response.Result = receiptResult;

            return response;
        }

        public Response GetCheckReceipt(long transactionId, bool isReprint, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();


            BizReceipt.Data.Receipt receipt = bizReceiptService.GetCheckReceipt(transactionId, isReprint, commonContext);

            Data.Receipt receiptResult = mapper.Map<Data.Receipt>(receipt);

            response.Result = receiptResult;

            return response;
        }

        public Response GetCouponReceipt(long customerSessionId, Data.ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizReceipt.Data.Receipt receipt = bizReceiptService.GetCouponReceipt(customerSessionId, commonContext);

            Data.Receipt receiptResult = mapper.Map<Data.Receipt>(receipt);

            response.Result = receiptResult;

            return response;
        }

        public Response GetSummaryReceipt(long customerSessionId, Data.ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizReceipt.Data.Receipt receipt = bizReceiptService.GetSummaryReceipt(customerSessionId, commonContext);
            Data.Receipt receiptResult = mapper.Map<Data.Receipt>(receipt);
            response.Result = receiptResult;

            return response;
        }

        public Response GetCashDrawerReceipt(long agentId, long locationId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizReceipt.Data.CashDrawerReceipt receipt = bizReceiptService.GetCashDrawerReceipt(agentId, locationId, commonContext);
            Data.CashDrawerReceipt receiptResult = mapper.Map<Data.CashDrawerReceipt>(receipt);
            response.Result = receiptResult;

            return response;
        }

    }
}
