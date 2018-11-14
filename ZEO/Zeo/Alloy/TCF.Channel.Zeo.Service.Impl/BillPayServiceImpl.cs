using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Common.Util;
using BizBillPay = TCF.Zeo.Biz.BillPay.Contract;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Biz.BillPay.Impl;
using TCF.Zeo.Biz.Impl;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IBillPayService
    {

        #region Dependancies
        public BizBillPay.IBillPayService BillPayService;
        #endregion

        #region IBillPayService Methods

        #region Bill Pay transaction Methods

        public Response GetLocations(long transactionId, string billerName, string accountNumber, decimal amount, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                BillPayService = new BillPayServiceImpl();
                response.Result = BillPayService.GetLocations(transactionId, billerName, accountNumber, amount, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GetBillPayFee(long transactionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation billerlocation, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                BillPayService = new BillPayServiceImpl();
                response.Result = BillPayService.GetFee(transactionId, billerNameOrCode, accountNumber, amount, billerlocation, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GetProviderAttributes(string billerNameOrCode, string location, ZeoContext context)
        {

            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            BillPayService = new BillPayServiceImpl();
            response.Result = BillPayService.GetProviderAttributes(billerNameOrCode, location, commonContext);

            return response;
        }

        public Response StageBillPayment(long transactionId, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();

                BillPayService = new BillPayServiceImpl();

                BIZShoppingCartService = new ShoppingCartServiceImpl();

                BillPayService.Submit(transactionId, commonContext);

                BIZShoppingCartService.AddShoppingCartTransaction(transactionId, (long)Helper.Product.BillPayment, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response ValidateBillPayment(long transactionId, BillPayment billPayment, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();

                BillPayService = new BillPayServiceImpl();
                response.Result = BillPayService.Validate(transactionId, billPayment, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response CancelBillPayment(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            BillPayService = new BillPayServiceImpl();
            BillPayService.UpdateTransactionState(transactionId, (int)Helper.TransactionStates.Canceled, commonContext);

            return response;
        }


        #endregion

        #region Biller's Methods

        public Response GetBillerInfo(string billerNameOrCode, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            BillPayService = new BillPayServiceImpl();
            response.Result = BillPayService.GetBillerInfo(billerNameOrCode, commonContext);
            return response;
        }

        public Response GetBillers(string searchTerm, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            BillPayService = new BillPayServiceImpl();
            response.Result = BillPayService.GetBillers(searchTerm, Convert.ToInt16(context.ChannelPartnerId), commonContext);
            return response;
        }

        public Response GetBillerDetails(string billerNameOrCode, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            BillPayService = new BillPayServiceImpl();
            response.Result = BillPayService.GetBillerDetails(billerNameOrCode, commonContext);
            return response;
        }

        public Response GetFrequentBillers(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            BillPayService = new BillPayServiceImpl();
            response.Result = BillPayService.GetFrequentBillers(commonContext);
            return response;
        }

        public Response AddPastBillers(string cardNumber, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                BillPayService = new BillPayServiceImpl();
                response.Result = BillPayService.AddPastBillers(cardNumber, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response DeleteFavoriteBiller(long billerId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            BillPayService = new BillPayServiceImpl();
            response.Result = BillPayService.DeleteFavouriteBiller(billerId, commonContext);
            return response;
        }

        #endregion


        #endregion
        public Response GetCardInfo(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            BillPayService = new BillPayServiceImpl();
            var cardInfo = BillPayService.GetCardInfo(commonContext);
            response.Result = cardInfo;
            return response;
        }

        public Response GetBillPayTransaction(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            BillPayService = new BillPayServiceImpl();
            BillPayTransaction trx = BillPayService.GetTransaction(transactionId, commonContext);
            response.Result = trx;
            return response;
        }
    }
}
