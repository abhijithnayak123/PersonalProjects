using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using commonData = TCF.Zeo.Common.Data;
using System.ServiceModel;
using TCF.Zeo.Biz.Impl;
using TCF.Zeo.Biz.Check.Impl;
using TCF.Zeo.Biz.BillPay.Impl;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.MoneyTransfer.Impl;
using TCF.Zeo.Biz.Fund.Impl;
using BizMoneyOrder = TCF.Zeo.Biz.MoneyOrder;
using BizMoneyTransfer = TCF.Zeo.Biz.MoneyTransfer;
using BizContract = TCF.Zeo.Biz;
using TCF.Zeo.Biz.Customer.Impl;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IShoppingCartService
    {
        BizContract.Contract.IShoppingCartService BIZShoppingCartService;
        BizContract.BillPay.Contract.IBillPayService BPService;
        BizContract.Check.Contract.ICPService CheckService;
        BizMoneyTransfer.Contract.IMoneyTransferEngine MoneyTransferEngine;
        BizMoneyOrder.Contract.IMoneyOrderServices MoneyOrderService;
        BizContract.Contract.ICashService CashServiceEngine;
        BizContract.Customer.Contract.IFlushService flushService;

        public Response GetShoppingCart(long customerSessionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BIZShoppingCartService = new ShoppingCartServiceImpl();

            response.Result = BIZShoppingCartService.GetShoppingCart(customerSessionId, commonContext);

            return response;
        }

        public Response ParkShoppingCartTransaction(long customerSessionId, long transactionId, int productId, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                productId = productId.ToProductIdEnum();

                BIZShoppingCartService = new ShoppingCartServiceImpl();

                response.Result = BIZShoppingCartService.ParkShoppingCartTransaction(customerSessionId, transactionId, productId, commonContext);

                if (productId == (int)Helper.Product.MoneyOrder || productId == (int)Helper.Product.ProcessCheck)
                {
                    ResubmitTransactions(transactionId, productId, context);
                }

                scope.Complete();

                return response;
            }
        }

        public Response RemoveCheck(long transactionId, ZeoContext context)
        {
            //TODO: we need to think about this Scope
            //using (var scope = TransactionHandler.CreateTransactionScope())
            //{
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            BIZShoppingCartService = new ShoppingCartServiceImpl();
            Response response = new Response();

            CheckService = new CPServiceImpl();

            bool isCancelled = CheckService.Cancel(transactionId, commonContext);

            if (isCancelled)
                response.Result = BIZShoppingCartService.RemoveShoppingCartTransaction(transactionId, (int)Helper.Product.ProcessCheck, commonContext);

            ShoppingCart shoppingCart = BIZShoppingCartService.GetShoppingCart(context.CustomerSessionId, commonContext);

            List<Check> availableChecks = shoppingCart.Checks.Where(m => m.Id > transactionId).ToList();

            for (int i = 0; i < availableChecks.Count; i++)
            {
                CheckService.Resubmit(availableChecks[i].Id, commonContext);
            }

            //   scope.Complete();

            return response;
            // }
        }

        public Response RemoveBillPay(long transactionId, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                BIZShoppingCartService = new ShoppingCartServiceImpl();
                BPService = new BillPayServiceImpl();

                try
                {
                    BPService.Cancel(transactionId, commonContext);
                    response.Result = BIZShoppingCartService.RemoveShoppingCartTransaction(transactionId, (int)Helper.Product.BillPayment, commonContext);
                    scope.Complete();
                }
                catch (Exception)
                {
                    BPService.UpdateTransactionState(transactionId, (int)Helper.TransactionStates.Canceled, commonContext);
                    response.Result = BIZShoppingCartService.RemoveShoppingCartTransaction(transactionId, (int)Helper.Product.BillPayment, commonContext);
                    scope.Complete();
                }

                return response;
            }
        }

        public Response RemoveMoneyTransfer(long transactionId, int productType, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();
                BIZShoppingCartService = new ShoppingCartServiceImpl();
                MoneyTransferEngine = new MoneyTransferEngineImpl();

                MoneyTransferEngine.CancelSendMoney(transactionId, commonContext);

                response.Result = BIZShoppingCartService.RemoveShoppingCartTransaction(transactionId, (int)Helper.Product.MoneyTransfer, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response RemoveFund(long transactionId, bool hasFundsAccount, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();
                BIZShoppingCartService = new ShoppingCartServiceImpl();
                FundEngine = new FundsEngineImpl();
                FundEngine.Cancel(transactionId, hasFundsAccount, commonContext);
                response.Result = BIZShoppingCartService.RemoveShoppingCartTransaction(transactionId, (int)Helper.Product.Fund, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response RemoveMoneyOrder(long transactionId, ZeoContext context)
        {
            //TODO: we need to think about this Scope
            //using (var scope = TransactionHandler.CreateTransactionScope())
            //{
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BIZShoppingCartService = new ShoppingCartServiceImpl();
            MoneyOrderService = new BizMoneyOrder.Impl.MoneyOrderServicesImpl();
            MoneyOrderService.UpdateMoneyOrderStatus(transactionId, (int)Helper.TransactionStates.Canceled, commonContext);

            response.Result = BIZShoppingCartService.RemoveShoppingCartTransaction(transactionId, (int)Helper.Product.MoneyOrder, commonContext);

            ShoppingCart shoppingCart = BIZShoppingCartService.GetShoppingCart(context.CustomerSessionId, commonContext);

            List<MoneyOrder> availableMoneyOrders = shoppingCart.MoneyOrders.Where(m => m.Id > transactionId).ToList();

            for (int i = 0; i < availableMoneyOrders.Count; i++)
            {
                MoneyOrderService.Resubmit(availableMoneyOrders[i].Id, commonContext);
            }

            // scope.Complete();

            return response;
            //}
        }

        public Response RemoveCashIn(ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();
                CashServiceEngine = new CashServiceImpl();
                BIZShoppingCartService = new ShoppingCartServiceImpl();

                response.Result = CashServiceEngine.RemoveCashIn(commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response ShoppingCartCheckout(decimal cashToCustomer, Helper.ShoppingCartCheckoutStatus status, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();
                BIZShoppingCartService = new ShoppingCartServiceImpl();
                response.Result = BIZShoppingCartService.ShoppingCartCheckout(cashToCustomer, status, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response PostFlush(decimal cardBalance, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            flushService = new FlushServiceImpl();
            flushService.PostFlush(cardBalance, commonContext);
            return response;
        }

        public Response FinalCommit(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            flushService = new FlushServiceImpl();
            flushService.FinalCommit(commonContext);
            return response;
        }

        public Response GetPrepaidCardNumber(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            FundEngine = new FundsEngineImpl();
            response.Result = FundEngine.GetPrepaidCardNumber(commonContext);
            return response;
        }

        public Response IsShoppingCartEmpty(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BIZShoppingCartService = new ShoppingCartServiceImpl();
            response.Result = BIZShoppingCartService.IsShoppingCartEmpty(commonContext);
            return response;

        }

        public Response CanCloseCustomerSession(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BIZShoppingCartService = new ShoppingCartServiceImpl();
            response.Result = BIZShoppingCartService.CanCloseCustomerSession(commonContext);
            return response;

        }

        public Response GetAllParkedTransaction(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BIZShoppingCartService = new ShoppingCartServiceImpl();
            response.Result = BIZShoppingCartService.GetAllParkedTransaction(commonContext);
            return response;
        }

        public Response GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            BIZShoppingCartService = new ShoppingCartServiceImpl();
            response.Result = BIZShoppingCartService.GenerateReceiptsForShoppingCart(customerSessionId, shoppingCartId, commonContext);
            return response;
        }

        private void ResubmitTransactions(long transactionId, int productId, ZeoContext context)
        {
            //TODO: we need to think about this Scope
            //using (var scope = TransactionHandler.CreateTransactionScope())
            //{
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            List<long> availableTransactions = BIZShoppingCartService.GetResubmitTransactions(productId, context.CustomerSessionId, commonContext);

            availableTransactions.RemoveAll(i => i < transactionId);

            if (productId == (int)Helper.Product.MoneyOrder)
            {
                MoneyOrderService = new BizMoneyOrder.Impl.MoneyOrderServicesImpl();

                for (int i = 0; i < availableTransactions.Count; i++)
                {
                    MoneyOrderService.Resubmit(availableTransactions[i], commonContext);
                }

                commonContext.IsParked = true;

                MoneyOrderService.Resubmit(transactionId, commonContext);
            }

            if (productId == (int)Helper.Product.ProcessCheck)
            {
                CheckService = new CPServiceImpl();

                for (int i = 0; i < availableTransactions.Count; i++)
                {
                    CheckService.Resubmit(availableTransactions[i], commonContext);
                }

                commonContext.IsParked = true;

                CheckService.Resubmit(transactionId, commonContext);
            }

            //  scope.Complete();
            //}
        }

    }
}
