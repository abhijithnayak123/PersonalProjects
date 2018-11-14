using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Biz.Check.Contract;
using TCF.Zeo.Biz.Check.Impl;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Impl;
using AutoMapper;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : ICheckService
    {

        public ICPService CPService;
        public Response GetCheckTypes(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            CPService = new CPServiceImpl();
            response.Result = CPService.GetCheckTypes(commonContext);

            return response;
        }

        public Response GetCheckSession(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            CPService = new CPServiceImpl();
            response.Result = CPService.GetChexarSessions(commonContext);

            return response;
        }

        public Response CancelCheck(long transactionId, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();

                CPService = new CPServiceImpl();
                response.Result = CPService.Cancel(transactionId, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GetCheckFee(CheckSubmission checkSubmit, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();

                CPService = new CPServiceImpl();
                response.Result = CPService.GetFee(checkSubmit, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GetCheckFrankingData(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            CPService = new CPServiceImpl();
            response.Result = CPService.GetCheckFrankingData(transactionId, commonContext);

            return response;
        }

        public Response GetCheckProcessorInfo(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            CPService = new CPServiceImpl();
            response.Result = CPService.GetCheckProcessorInfo(commonContext);

            return response;
        }

        public Response GetCheckStatus(long transactionId, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();

                CPService = new CPServiceImpl();
                bool includeImage = false;
                response.Result = CPService.GetStatus(transactionId, includeImage, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GetCheckTranasactionDetails(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            CPService = new CPServiceImpl();
            response.Result = CPService.GetTransaction(transactionId, commonContext);

            return response;
        }

        public Response SubmitCheck(CheckSubmission checkSubmission, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();

                BIZShoppingCartService = new ShoppingCartServiceImpl();
                CPService = new CPServiceImpl();

                Check check = CPService.Submit(checkSubmission, commonContext);

                if (check.Status != Helper.CheckStatus.Declined.ToString())
                    BIZShoppingCartService.AddShoppingCartTransaction(check.Id, (long)Helper.Product.ProcessCheck, commonContext);

                scope.Complete();

                response.Result = check;

                return response;
            }
        }

        public Response UpdateCheckTransactionFranked(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            CPService = new CPServiceImpl();
            CPService.UpdateCheckTransactionFranked(transactionId, commonContext);

            return response;
        }

        public Response GetFeeBasedOnPromoCode(CheckSubmission checkSubmit, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();

            CPService = new CPServiceImpl();
            response.Result = CPService.GetFeeBasedOnPromocode(checkSubmit, commonContext);

            return response;
        }


        public Response GetCheckProvider(MICRDetails micrDetails, ZeoContext context)
        {
            Response response = new Response();

            CPService = new CPServiceImpl();

            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            response.Result = CPService.GetCheckProvider(micrDetails, commonContext);

            return response;
        }
    }
}
