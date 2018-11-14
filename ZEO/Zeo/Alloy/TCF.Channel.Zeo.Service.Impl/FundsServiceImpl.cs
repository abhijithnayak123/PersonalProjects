using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using TCF.Zeo.Biz.Fund.Contract;
using TCF.Zeo.Biz.Fund.Impl;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;
using AutoMapper;
using TCF.Zeo.Biz.Impl;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IFundsProcessorService
    {
        public IFundsEngine FundEngine;

        public Response ActivateGPRCard(Funds funds, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                FundEngine = new FundsEngineImpl();

                BIZShoppingCartService = new ShoppingCartServiceImpl();
                long trxId = FundEngine.Activate(funds, commonContext);

                if (funds.TransactionId == 0)
                    BIZShoppingCartService.AddShoppingCartTransaction(trxId, (int)Helper.Product.Fund, commonContext);

                response.Result = trxId;

                scope.Complete();

                return response;
            }
               
        }

        public Response Add(FundsAccount fundsAccount, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.Add(fundsAccount, commonContext);
            return response;
        }

        public Response AssociateCard(FundsAccount fundsAccount, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.AssociateCard(fundsAccount, commonContext);
            return response;
        }

        public Response CloseAccount(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.CloseAccount(commonContext);
            return response;
        }

        public Response GetBalance(ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                FundEngine = new FundsEngineImpl();
                response.Result = FundEngine.GetBalance(commonContext);

				scope.Complete();

                return response;
            }
        }

        public Response GetCardTransactionHistory(TransactionHistoryRequest request, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.GetTransactionHistory(request, commonContext);
            return response;
        }
        public Response GetFee(decimal amount, Helper.FundType fundsType, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.GetFee(amount, fundsType, commonContext);
            return response;
        }

        public Response GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.GetFundFee(cardMaintenanceInfo, commonContext);
            return response;
        }

        public Response GetMinimumLoadAmount(bool initialLoad, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.GetMinimumLoadAmount(initialLoad, commonContext);
            return response;
        }

        public Response GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.GetShippingFee(cardMaintenanceInfo, commonContext);
            return response;
        }

        public Response GetShippingTypes(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.GetShippingTypes(commonContext);
            return response;
        }

        public Response IssueAddOnCard(Funds funds, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                FundEngine = new FundsEngineImpl();
                response.Result = FundEngine.IssueAddOnCard(funds, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response Load(Funds funds, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                FundEngine = new FundsEngineImpl();
                BIZShoppingCartService = new ShoppingCartServiceImpl();
                long trxId = FundEngine.Load(funds, commonContext);
                BIZShoppingCartService.AddShoppingCartTransaction(trxId, (int)Helper.Product.Fund, commonContext);
                response.Result = trxId;

                scope.Complete();

                return response;
            }
        }

        public Response LookupForPAN(ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                FundEngine = new FundsEngineImpl();
                response.Result = FundEngine.GetAccount(commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response ReplaceCard(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.ReplaceCard(cardMaintenanceInfo, commonContext);
            return response;
        }

        public Response UpdateCardStatus(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.UpdateCardStatus(cardMaintenanceInfo, commonContext);
            return response;
        }

        public Response UpdateFundAmount(long cxnFundTrxId, decimal amount, Helper.FundType fundType, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                FundEngine = new FundsEngineImpl();
                response.Result = FundEngine.UpdateAmount(cxnFundTrxId, amount, fundType, string.Empty, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response Withdraw(Funds funds, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                FundEngine = new FundsEngineImpl();
                BIZShoppingCartService = new ShoppingCartServiceImpl();
                long trxId = FundEngine.Withdraw(funds, commonContext);
                BIZShoppingCartService.AddShoppingCartTransaction(trxId, (int)Helper.Product.Fund, commonContext);
                response.Result = trxId;

                scope.Complete();

                return response;
            }
             
        }

        public Response GetPrepaidActions(string cardStatus, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            switch (cardStatus)
            {
                case "active":
                    items.Add(new KeyValuePair<string, string>("Report Lost & Replace", "5"));
                    items.Add(new KeyValuePair<string, string>("Report Stolen & Replace", "6"));
                    items.Add(new KeyValuePair<string, string>("Suspend Card(Do not replace)", "3"));
                    items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                    break;
                case "suspended":
                    items.Add(new KeyValuePair<string, string>("Activate Card", "0"));
                    items.Add(new KeyValuePair<string, string>("Report Lost & Replace", "5"));
                    items.Add(new KeyValuePair<string, string>("Report Stolen & Replace", "6"));
                    items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                    break;
                case "cardissued":
                    items.Add(new KeyValuePair<string, string>("Report Lost & Replace", "5"));
                    items.Add(new KeyValuePair<string, string>("Report Stolen & Replace", "6"));
                    items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                    break;
                case "lostcard":
                    items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                    break;
                case "stolencard":
                    items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
                    break;
            }

            response.Result = items;
            return response;
        }

        public Response GetFundTransaction(long transactionId, bool isEditTransaction, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.Get(transactionId, isEditTransaction, commonContext);
            return response;
        }

        public Response UpdateGPRAccount(FundsAccount fundsAccount, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                FundEngine = new FundsEngineImpl();
                long trxId = FundEngine.UpdateAccount(fundsAccount, commonContext);
                response.Result = trxId;
                scope.Complete();

                return response;
            }
        }
    }
}
