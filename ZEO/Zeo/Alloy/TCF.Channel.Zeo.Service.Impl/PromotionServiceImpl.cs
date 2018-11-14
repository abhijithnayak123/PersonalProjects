using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using CommonData = TCF.Zeo.Common.Data;
using BizPromotion = TCF.Zeo.Biz;
using System;
using TCF.Zeo.Common.Util;
using System.Collections.Generic;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IPromotionService
    {
        public BizPromotion.Contract.IPromotionService PromotionService { get; set; }
        public Response CreateAndUpdatePromotion(Promotion promotion, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);
            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.CreateAndUpdatePromotion(promotion, commonContext);

            return response;
        }

        public Response GetPromotion(long promotionId, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);
            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.GetPromotion(promotionId, commonContext);

            return response;
        }

        public Response GetPromotions(PromotionSearchCriteria promoCriteria, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);
            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.GetPromotions(promoCriteria, commonContext);

            return response;
        }

        public Response ValidatePromoName(string promotionName, long promotionId, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);
            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.ValidatePromoName(promotionName, promotionId, commonContext);

            return response;
        }

        public Response UpdatePromotionStatus(long promotionId, Helper.PromotionStatus status, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);
            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.UpdatePromotionStatus(promotionId, status, commonContext);

            return response;
        }

        public Response SavePromoDetails(PromotionDetail promoDetails, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);
            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.SavePromoDetails(promoDetails, commonContext);

            return response;
        }

        public Response SavePromoProvision(Provision provision, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);

            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.SavePromoProvision(provision, commonContext);

            return response;
        }

        public Response SavePromoQualifier(Qualifier qualifier, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);

            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.SavePromoQualifier(qualifier, commonContext);

            return response;
        }

        public Response DeletePromoProvision(long provisionId, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);

            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.DeletePromoProvision(provisionId, commonContext);

            return response;
        }

        public Response DeletePromoQualifier(long qualifierId, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);

            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.DeletePromoQualifier(qualifierId, commonContext);

            return response;
        }

        public Response ValidateProviderPromotion(Helper.TransactionType transactionType, decimal amount, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);

            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.ValidateProviderPromotion(transactionType, amount, commonContext);

            return response;
        }

        public Response AddUpdateQualifiers(List<Qualifier> qualifiers, long promotionId, DateTime startDate, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);

            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.AddUpdateQualifiers(qualifiers, promotionId, startDate, commonContext);

            return response;
        }

        public Response AddUpdateProvisions(List<Provision> provisions, long promotionId, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);

            Response response = new Response();

            PromotionService = new BizPromotion.Impl.PromotionServiceImpl();

            response.Result = PromotionService.AddUpdateProvisions(provisions, promotionId, commonContext);

            return response;
        }
    }
}
