using System;
using System.Collections.Generic;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Contract;
using CoreContract = TCF.Zeo.Core.Contract;
using BizCommonContract = TCF.Zeo.Biz.Common.Contract;
using BizCommonImpl = TCF.Zeo.Biz.Common.Impl;
using CoreImpl = TCF.Zeo.Core.Impl;
using CoreData = TCF.Zeo.Core.Data;
using AutoMapper;
using CommonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Biz.Impl
{
    public class PromotionServiceImpl : IPromotionService
    {
        public IMapper Mapper { get; set; }
        public CoreContract.IPromotionService CorePromotionService { get; set; }

        public BizCommonContract.IFeeService FeeService { get; set; }

        public PromotionServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoreData.PromotionDetail, PromotionDetail>();
                cfg.CreateMap<CoreData.Promotion, Promotion>();
                cfg.CreateMap<CoreData.Qualifier, Qualifier>();
                cfg.CreateMap<CoreData.Provision, Provision>();
                cfg.CreateMap<CoreData.PromotionTypes, PromotionTypes>();
                cfg.CreateMap<CoreData.PromotionSearchCriteria, PromotionSearchCriteria>();
                cfg.CreateMap<CoreData.Product, MasterData>();
                cfg.CreateMap<CoreData.Provider, MasterData>();
                cfg.CreateMap<CoreData.PromotionDetail, PromotionDetail>().ReverseMap();
                cfg.CreateMap<CoreData.Promotion, Promotion>().ReverseMap();
                cfg.CreateMap<CoreData.Qualifier, Qualifier>().ReverseMap();
                cfg.CreateMap<CoreData.Provision, Provision>().ReverseMap();
                cfg.CreateMap<CoreData.PromotionTypes, PromotionTypes>().ReverseMap();
                cfg.CreateMap<CoreData.PromotionSearchCriteria, PromotionSearchCriteria>().ReverseMap();
                cfg.CreateMap<CoreData.Product, MasterData>().ReverseMap();
                cfg.CreateMap<CoreData.Provider, MasterData>().ReverseMap();
            });

            Mapper = config.CreateMapper();
        }

        public bool CreateAndUpdatePromotion(Promotion promotion, CommonData.ZeoContext context)
        {
            try
            {
                CoreData.Promotion corePromotion = Mapper.Map<CoreData.Promotion>(promotion);

                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    return CorePromotionService.CreateAndUpdatePromotion(corePromotion, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.CREATE_OR_UPDATE_PROMOTION, ex);
            }
        }

        public Promotion GetPromotion(long promotionId, CommonData.ZeoContext context)
        {
            try
            {
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    CoreData.Promotion corePromotion = CorePromotionService.GetPromotion(promotionId, context);

                    return Mapper.Map<Promotion>(corePromotion);   
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.GET_PROMOTION_BY_ID, ex);
            }
        }

        public List<PromotionDetail> GetPromotions(PromotionSearchCriteria promoCriteria, CommonData.ZeoContext context)
        {
            try
            {
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    CoreData.PromotionSearchCriteria corePromotionCriteria = Mapper.Map<CoreData.PromotionSearchCriteria>(promoCriteria);

                    List<CoreData.PromotionDetail> corePromoDetails = CorePromotionService.GetPromotions(corePromotionCriteria, context);

                    return Mapper.Map<List<PromotionDetail>>(corePromoDetails);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.GET_PROMOTIONS, ex);
            }
        }

        public bool ValidatePromoName(string promotionName, long promotionId, CommonData.ZeoContext context)
        {
            try
            {
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    return CorePromotionService.ValidatePromoName(promotionName, promotionId, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.VALIDATE_PROMO_NAME, ex);
            }
        }

        public int UpdatePromotionStatus(long promotionId, Helper.PromotionStatus status, CommonData.ZeoContext context)
        {
            try
            {
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    return CorePromotionService.UpdatePromotionStatus(promotionId, status, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.UPDATE_PROMOTION_STATUS, ex);
            }
        }

        public long SavePromoDetails(PromotionDetail promoDetails, CommonData.ZeoContext context)
        {
            long promoId;
            try
            {
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    promoDetails.EndDate = promoDetails.EndDate == default(DateTime) ? null : promoDetails.EndDate;
                    promoDetails.StartDate = promoDetails.StartDate == default(DateTime) ? null : promoDetails.StartDate;
                    promoId = CorePromotionService.SavePromoDetails(Mapper.Map<CoreData.PromotionDetail>(promoDetails), context);
                }

                return promoId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.SAVE_PROMO_DETAILS_FAILED, ex);
            }
        }

        public long SavePromoProvision(Provision provision, CommonData.ZeoContext context)
        {
            long provisionId;
            try
            {
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    provisionId = CorePromotionService.SavePromoProvision(Mapper.Map<CoreData.Provision>(provision), context);
                }

                return provisionId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.SAVE_PROMO_PROVISION_FAILED, ex);
            }
        }

        public long SavePromoQualifier(Qualifier qualifier, CommonData.ZeoContext context)
        {
            long qualifierId;
            try
            {
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    qualifierId = CorePromotionService.SavePromoQualifier(Mapper.Map<CoreData.Qualifier>(qualifier), context);
                }

                return qualifierId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.SAVE_PROMO_QUALIFIER_FAILED, ex);
            }
        }

        public bool DeletePromoProvision(long provisionId, CommonData.ZeoContext context)
        {
            bool isDeleted;
            try
            {
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    isDeleted = CorePromotionService.DeletePromoProvision(provisionId, context);
                }

                return isDeleted;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.DELETE_PROMO_PROVISION_FAILED, ex);
            }
        }

        public bool DeletePromoQualifier(long qualifierId, CommonData.ZeoContext context)
        {
            bool isDeleted;
            try
            {
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    isDeleted = CorePromotionService.DeletePromoQualifier(qualifierId, context);
                }

                return isDeleted;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.DELETE_PROMO_QUALIFIER_FAILED, ex);
            }
        }

        /// <summary>
        /// Validating the Promo code.
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="amount"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public long ValidateProviderPromotion(TransactionType transactionType, decimal amount, CommonData.ZeoContext context)
        {
            FeeService = new BizCommonImpl.FeeServiceImpl();
            
            return FeeService.ValidateProviderPromotion(transactionType, amount, context);
        }

        public List<Qualifier> AddUpdateQualifiers(List<Qualifier> qualifiers, long promotionId, DateTime startDate, CommonData.ZeoContext context)
        {
            try
            {
                List<CoreData.Qualifier> coreQualifier = new List<CoreData.Qualifier>();
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    coreQualifier = CorePromotionService.AddUpdateQualifiers(Mapper.Map<List<CoreData.Qualifier>>(qualifiers), promotionId, startDate, context);
                }

                return Mapper.Map<List<Qualifier>>(coreQualifier);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.ADD_UPDATE_QUALIFIERS_FAILED, ex);
            }
        }

        public List<Provision> AddUpdateProvisions(List<Provision> provisions, long promotionId, CommonData.ZeoContext context)
        {
            try
            {
                List<CoreData.Provision> coreProvision = new List<CoreData.Provision>();
                using (CorePromotionService = new CoreImpl.ZeoCoreImpl())
                {
                    coreProvision = CorePromotionService.AddUpdateProvisions(Mapper.Map<List<CoreData.Provision>>(provisions), promotionId, context);
                }

                return Mapper.Map<List<Provision>>(coreProvision);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new PromotionException(PromotionException.ADD_UPDATE_PROVISIONS_FAILED, ex);
            }
        }
    }
}
