using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;

#region Zeo References
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using TCF.Zeo.Biz.MoneyOrder.Impl;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.MoneyOrder.Contract;
using System.ServiceModel;
using TCF.Zeo.Biz.Impl;
#endregion

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IMoneyOrderService
    {
        IMoneyOrderServices moneyOrderService;

        public Response Commit(long transactionId, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                moneyOrderService = new MoneyOrderServicesImpl();
                response.Result = moneyOrderService.Commit(transactionId, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GenerateCheckPrintForMoneyOrder(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            moneyOrderService = new MoneyOrderServicesImpl();
            MoneyOrderCheckPrint checkPrint = new MoneyOrderCheckPrint();
            checkPrint.Lines = moneyOrderService.GetMoneyOrderCheck(transactionId, commonContext).Lines;
            response.Result = checkPrint;
            return response;
        }

        public Response GenerateMoneyOrderDiagnostics(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            moneyOrderService = new MoneyOrderServicesImpl();
            response.Result = moneyOrderService.GetMoneyOrderDiagnostics(commonContext);
            return response;
        }

        public Response GetMoneyOrderFee(decimal amount, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            moneyOrderService = new MoneyOrderServicesImpl();
            response.Result = moneyOrderService.GetMoneyOrderFee(amount, commonContext);
            return response;
        }

        public Response GetMoneyOrderTransaction(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            moneyOrderService = new MoneyOrderServicesImpl();
            response.Result = moneyOrderService.GetMoneyOrderTransaction(transactionId, commonContext);
            return response;
        }

        public Response PurchaseMoneyOrder(MoneyOrderPurchase moneyOrderPurchase, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                moneyOrderService = new MoneyOrderServicesImpl();

                BIZShoppingCartService = new ShoppingCartServiceImpl();

                MoneyOrder moneyOrder = moneyOrderService.Add(moneyOrderPurchase, commonContext);

                BIZShoppingCartService.AddShoppingCartTransaction(moneyOrder.Id, (int)Helper.Product.MoneyOrder, commonContext);

                response.Result = moneyOrder;

                scope.Complete();

                return response;
            }
        }

        public Response UpdateMoneyOrder(MoneyOrder moneyOrderTransaction, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                moneyOrderService = new MoneyOrderServicesImpl();

                response.Result = moneyOrderService.UpdateMoneyOrder(moneyOrderTransaction, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response UpdateMoneyOrderStatus(long transactionId, int newMoneyOrderStatus, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                moneyOrderService = new MoneyOrderServicesImpl();

                response.Result = moneyOrderService.UpdateMoneyOrderStatus(transactionId, newMoneyOrderStatus, commonContext);

                scope.Complete();

                return response;
            }
        }
    }
}
