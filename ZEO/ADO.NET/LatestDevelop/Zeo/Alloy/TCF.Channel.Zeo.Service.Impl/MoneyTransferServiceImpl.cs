using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.MoneyTransfer.Impl;
using TCF.Zeo.Biz.MoneyTransfer.Contract;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Impl;
using commonData = TCF.Zeo.Common.Data;
namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IMoneyTransferService
    {
        public IMoneyTransferEngine BizmoneytrService { get; set; }

        public Response AddReceiver(Receiver receiver, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.AddReceiver(receiver, commonContext);
            return response;
        }

        public Response DeleteFavoriteReceiver(long receiverId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.DeleteFavoriteReceiver(receiverId, commonContext);
            return response;
        }

        public Response EditReceiver(Receiver receiver, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.UpdateReceiver(receiver, commonContext);
            return response;
        }

        public Response GetCurrencyCodeList(string countryCode, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetCurrencyCodeList(countryCode, commonContext);
            return response;
        }

        public Response GetDeliveryServices(DeliveryServiceRequest request, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetDeliveryServices(request, commonContext);
            return response;
        }

        public Response GetFeeMoneyTransfer(FeeRequest feeRequest, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();
                BizmoneytrService = new MoneyTransferEngineImpl();
                response.Result = BizmoneytrService.GetFee(feeRequest, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GetFrequentReceivers(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetFrequentReceivers(commonContext);
            return response;
        }

        public Response GetReceiver(long receiverId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetReceiver(receiverId, commonContext);
            return response;
        }

        public Response GetXfrCities(string stateCode, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetXfrCities(stateCode, commonContext);
            return response;
        }

        public Response GetXfrCountries(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetXfrCountries(commonContext);
            return response;
        }

        public Response GetXfrStates(string countryCode, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetXfrStates(countryCode, commonContext);
            return response;
        }

        public Response Validate(ValidateRequest validateRequest, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                BizmoneytrService = new MoneyTransferEngineImpl();

                ValidateResponse validateResponse = BizmoneytrService.Validate(validateRequest, commonContext);

                response.Result = validateResponse;

                if (!validateResponse.HasLPMTError)
                {
                    BIZShoppingCartService = new ShoppingCartServiceImpl();
                    BIZShoppingCartService.AddShoppingCartTransaction(validateResponse.TransactionId, (int)Helper.Product.MoneyTransfer, commonContext);

                }
                scope.Complete();

                return response;
            }
        }

        public Response WUCardEnrollment(ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                BizmoneytrService = new MoneyTransferEngineImpl();
                response.Result = BizmoneytrService.WUCardEnrollment(commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response AddPastReceivers(string cardNumber, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                BizmoneytrService = new MoneyTransferEngineImpl();
                BizmoneytrService.AddPastReceivers(cardNumber, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response WUCardLookup(CardLookupDetails lookupDetails, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            lookupDetails = BizmoneytrService.WUCardLookup(lookupDetails, commonContext);

            List<WUCustomerGoldCardResult> goldCardResult = new List<WUCustomerGoldCardResult>();

            WUCustomerGoldCardResult carddetail = null;

            foreach (Account account in lookupDetails.Sender)
            {
                carddetail = new WUCustomerGoldCardResult()
                {
                    Address = account.Address,
                    FullName = string.Format("{0} {1}", account.FirstName, account.LastName),
                    ZipCode = account.PostalCode,
                    WUGoldCardNumber = account.LoyaltyCardNumber,
                    PhoneNumber = account.MobilePhone
                };
                goldCardResult.Add(carddetail);
            }

            response.Result = goldCardResult;

            return response;

        }

        public Response UpdateWUAccount(string WUGoldCardNumber, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.UpdateAccount(WUGoldCardNumber, commonContext);
            return response;

        }

        public Response CancelXfer(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            BizmoneytrService.Cancel(transactionId, commonContext);
            return response;

        }

        public Response WUGetAgentBannerMessage(long agentSessionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetBannerMsgs(agentSessionId, commonContext);
            return response;
        }

        public Response GetCurrencyCode(string countryCode, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetCurrencyCode(countryCode, commonContext);
            return response;

        }

        public Response Search(SearchRequest request, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                BizmoneytrService = new MoneyTransferEngineImpl();
                response.Result = BizmoneytrService.Search(request, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response StageModify(ModifyRequest modifySendMoney, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                BizmoneytrService = new MoneyTransferEngineImpl();
                response.Result = BizmoneytrService.StageModify(modifySendMoney, commonContext);

                scope.Complete();

                return response;
            }

        }

        public Response GetRefundReasons(ReasonRequest request, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();

            response.Result = BizmoneytrService.GetRefundReasons(request, commonContext);

            return response;

        }

        public Response GetTransaction(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetTransaction(transactionId, commonContext);

            return response;

        }

        public Response GetStatus(string mtcn, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.GetStatus(mtcn, commonContext);
            return response;

        }

        public Response IsSWBStateXfer(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            response.Result = BizmoneytrService.IsSWBStateXfer(commonContext);
            return response;

        }

        public Response AuthorizeModifySendMoney(ModifyRequest modifySendMoney, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                BizmoneytrService = new MoneyTransferEngineImpl();

                BizmoneytrService.AuthorizeModify(modifySendMoney, commonContext);

                BIZShoppingCartService = new ShoppingCartServiceImpl();
                BIZShoppingCartService.AddShoppingCartTransaction(modifySendMoney.CancelTransactionId, (int)Helper.Product.MoneyTransfer, commonContext);
                BIZShoppingCartService.AddShoppingCartTransaction(modifySendMoney.ModifyTransactionId, (int)Helper.Product.MoneyTransfer, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response SendMoneyRefund(SendMoneyRefundRequest moneyTransferRefund, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response responseResult = new Response() { Result = string.Empty };
                BizmoneytrService = new MoneyTransferEngineImpl();
                BIZShoppingCartService = new ShoppingCartServiceImpl();
                SearchRequest request = new SearchRequest()
                {
                    ConfirmationNumber = moneyTransferRefund.ConfirmationNumber,
                    SearchRequestType = Helper.SearchRequestType.RefundWithStage,
                    TransactionId = long.Parse(moneyTransferRefund.TransactionId)
                };
                SearchResponse response = BizmoneytrService.Search(request, commonContext);
                if (response.RefundStatus.ToUpper() == Helper.RefundStatus.F.ToString() || response.RefundStatus.ToUpper() == Helper.RefundStatus.N.ToString())
                {
                    SendMoneyRefundRequest bizRefundSendMoney = new SendMoneyRefundRequest()
                    {
                        TransactionId = response.TransactionId,
                        CategoryCode = moneyTransferRefund.CategoryCode,
                        CategoryDescription = moneyTransferRefund.CategoryDescription,
                        Reason = moneyTransferRefund.Reason,
                        ConfirmationNumber = moneyTransferRefund.ConfirmationNumber,
                        RefundStatus = moneyTransferRefund.RefundStatus,
                        CancelTransactionId = response.CancelTransactionId,
                        RefundTransactionId = response.RefundTransactionId,
                        Comments = moneyTransferRefund.Reason
                    };

                    responseResult.Result = BizmoneytrService.Refund(bizRefundSendMoney, commonContext);
                    BIZShoppingCartService.AddShoppingCartTransaction(Convert.ToInt64(response.TransactionId), (int)Helper.Product.MoneyTransfer, commonContext);
                }

                scope.Complete();

                return responseResult;
            }
        }

        public Response ReceiveMoneySearch(ReceiveMoneyRequest receiveMoneyRequest, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                BizmoneytrService = new MoneyTransferEngineImpl();
                response.Result = BizmoneytrService.GetReceiveMoney(receiveMoneyRequest, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response CancelSendMoneyModify(long modifyTransactionId, long cancelTransactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            BizmoneytrService = new MoneyTransferEngineImpl();
            BizmoneytrService.CancelSendMoneyModify(modifyTransactionId, cancelTransactionId, commonContext);
            return response;

        }
    }
}
