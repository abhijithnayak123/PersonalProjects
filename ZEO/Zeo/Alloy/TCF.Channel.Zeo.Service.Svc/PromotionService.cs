using System;
using System.Collections.Generic;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IPromotionService
    {
        public Response CreateAndUpdatePromotion(Promotion promotion, ZeoContext context)
        {
            return serviceEngine.CreateAndUpdatePromotion(promotion, context);
        }

        public Response GetPromotion(long promotionId, ZeoContext context)
        {
            return serviceEngine.GetPromotion(promotionId, context);
        }

        public Response GetPromotions(PromotionSearchCriteria promoCriteria, ZeoContext context)
        {
            return serviceEngine.GetPromotions(promoCriteria, context);
        }

        public Response ValidatePromoName(string promotionName, long promotionId, ZeoContext context)
        {
            return serviceEngine.ValidatePromoName(promotionName, promotionId, context);
        }

        public Response UpdatePromotionStatus(long promotionId, Helper.PromotionStatus status, ZeoContext context)
        {
            return serviceEngine.UpdatePromotionStatus(promotionId, status, context);
        }

        public Response SavePromoDetails(PromotionDetail promoDetails, ZeoContext context)
        {
            return serviceEngine.SavePromoDetails(promoDetails, context);
        }

        public Response SavePromoProvision(Provision provision, ZeoContext context)
        {
            return serviceEngine.SavePromoProvision(provision, context);
        }

        public Response SavePromoQualifier(Qualifier qualifier, ZeoContext context)
        {
            return serviceEngine.SavePromoQualifier(qualifier, context);
        }

        public Response DeletePromoProvision(long provisionId, ZeoContext context)
        {
            return serviceEngine.DeletePromoProvision(provisionId, context);
        }

        public Response DeletePromoQualifier(long qualifierId, ZeoContext context)
        {
            return serviceEngine.DeletePromoQualifier(qualifierId, context);
        }

        public Response ValidateProviderPromotion(Helper.TransactionType transactionType, decimal amount, ZeoContext context)
        {
            return serviceEngine.ValidateProviderPromotion(transactionType, amount, context);
        }

        public Response AddUpdateQualifiers(List<Qualifier> qualifiers, long promotionId, DateTime startDate, ZeoContext context)
        {
            return serviceEngine.AddUpdateQualifiers(qualifiers, promotionId, startDate, context);
        }

        public Response AddUpdateProvisions(List<Provision> provisions, long promotionId, ZeoContext context)
        {
            return serviceEngine.AddUpdateProvisions(provisions, promotionId, context);
        }
    }
}