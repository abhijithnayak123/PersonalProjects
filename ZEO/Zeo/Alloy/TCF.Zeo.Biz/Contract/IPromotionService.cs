using System;
using System.Collections.Generic;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Common.Util;
using static TCF.Zeo.Common.Util.Helper;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
    public interface IPromotionService
    {
        Promotion GetPromotion(long promotionId, CommonData.ZeoContext context);

        bool CreateAndUpdatePromotion(Promotion promotion, CommonData.ZeoContext context);

        List<PromotionDetail> GetPromotions(PromotionSearchCriteria promoCriteria, CommonData.ZeoContext context);

        bool ValidatePromoName(string promotionName, long promotionId, CommonData.ZeoContext context);

        int UpdatePromotionStatus(long promotionId, Helper.PromotionStatus status, CommonData.ZeoContext context);

        long SavePromoDetails(PromotionDetail promoDetails, CommonData.ZeoContext context);

        long SavePromoProvision(Provision provision, CommonData.ZeoContext context);

        long SavePromoQualifier(Qualifier qualifier, CommonData.ZeoContext context);

        bool DeletePromoProvision(long provisionId, CommonData.ZeoContext context);

        bool DeletePromoQualifier(long qualifierId, CommonData.ZeoContext context);

        long ValidateProviderPromotion(TransactionType transactionType, decimal amount, CommonData.ZeoContext context);

        List<Qualifier> AddUpdateQualifiers(List<Qualifier> qualifiers, long promotionId, DateTime startDate, CommonData.ZeoContext context);

        List<Provision> AddUpdateProvisions(List<Provision> provisions, long promotionId, CommonData.ZeoContext context);
    }
}
