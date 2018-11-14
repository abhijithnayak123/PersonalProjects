using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data;

namespace TCF.Zeo.Core.Contract
{
    public interface IPromotionService : IDisposable
    {
        Promotion GetPromotion(long promotionId, ZeoContext context);

        bool CreateAndUpdatePromotion(Promotion promotion, ZeoContext context);

        List<PromotionDetail> GetPromotions(PromotionSearchCriteria promotionSearchCriteria, ZeoContext context);

        bool ValidatePromoName(string promotionName, long promotionId, ZeoContext context);

        int UpdatePromotionStatus(long promotionId, Helper.PromotionStatus status, ZeoContext context);

        long SavePromoDetails(PromotionDetail promoDetails, ZeoContext context);

        long SavePromoProvision(Provision provision, ZeoContext context);

        long SavePromoQualifier(Qualifier qualifier, ZeoContext context);

        bool DeletePromoProvision(long provisionId, ZeoContext context);

        bool DeletePromoQualifier(long qualifierId, ZeoContext context);

        List<Qualifier> AddUpdateQualifiers(List<Qualifier> qualifiers, long promotionId, DateTime startDate, ZeoContext context);

        List<Provision> AddUpdateProvisions(List<Provision> provisions, long promotionId, ZeoContext context);
    }
}
