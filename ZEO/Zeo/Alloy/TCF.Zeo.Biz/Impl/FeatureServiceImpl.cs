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
using TCF.Channel.Zeo.Data;
using System.Collections.Generic;
using System;

namespace TCF.Zeo.Biz.Impl
{
    public class FeatureServiceImpl : IFeatureService
    {
        public IMapper Mapper { get; set; }
        public CoreContract.IFeatureService CoreFeatureService { get; set; }

        public FeatureServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoreData.FeatureDetails, FeatureDetails>().ReverseMap();
            });

            Mapper = config.CreateMapper();
        }

        public List<FeatureDetails> GetFeatures(Zeo.Common.Data.ZeoContext context)
        {
            try
            {
                using (CoreFeatureService = new CoreImpl.ZeoCoreImpl())
                {
                    List<CoreData.FeatureDetails> coreFeatures = CoreFeatureService.GetFeatures(context);

                    return Mapper.Map<List<FeatureDetails>>(coreFeatures);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FeatureException(FeatureException.GET_FEATRUES_FAILED, ex);
            }
        }

        public bool UpdateFeatures(List<FeatureDetails> features, Zeo.Common.Data.ZeoContext context)
        {
            try
            {
                List<CoreData.FeatureDetails> corefeatureDetails = Mapper.Map<List<CoreData.FeatureDetails>>(features);

                using (CoreFeatureService = new CoreImpl.ZeoCoreImpl())
                {
                    return CoreFeatureService.UpdateFeatures(corefeatureDetails, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FeatureException(FeatureException.FEATURE_UPDATE_FAILED, ex);
            }
        }
    }
}
