using System;
using System.Collections.Generic;
using System.ServiceModel;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Common.Util;
using static TCF.Zeo.Common.Util.Helper;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IPromotionService
    {
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetPromotion(long  promotionId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetPromotions(PromotionSearchCriteria promoCriteria, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response CreateAndUpdatePromotion(Promotion promotion, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response ValidatePromoName(string promotionName, long promotionId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response UpdatePromotionStatus(long promotionId, Helper.PromotionStatus status, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response SavePromoDetails(PromotionDetail promoDetails, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response SavePromoProvision(Provision provision, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response SavePromoQualifier(Qualifier qualifier, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response DeletePromoProvision(long provisionId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response DeletePromoQualifier(long qualifierId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response ValidateProviderPromotion(TransactionType transactionType, decimal amount, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response AddUpdateQualifiers(List<Qualifier> qualifiers, long promotionId, DateTime startDate, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response AddUpdateProvisions(List<Provision> provisions, long promotionId, ZeoContext context);


    }
}
