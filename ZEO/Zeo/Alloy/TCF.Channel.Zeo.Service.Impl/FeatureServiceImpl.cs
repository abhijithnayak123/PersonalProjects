using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using CommonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Biz;
using BizFeature = TCF.Zeo.Biz;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IFeatureService
    {
        public BizFeature.Contract.IFeatureService FeatureService { get; set; }
        public Response GetFeatures(ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);
            Response response = new Response();

            FeatureService = new BizFeature.Impl.FeatureServiceImpl();

            response.Result = FeatureService.GetFeatures(commonContext);

            return response;
        }

        public Response UpdateFeatures(List<FeatureDetails> features, ZeoContext context)
        {
            CommonData.ZeoContext commonContext = mapper.Map<CommonData.ZeoContext>(context);
            Response response = new Response();

            FeatureService = new BizFeature.Impl.FeatureServiceImpl();

            response.Result = FeatureService.UpdateFeatures(features, commonContext);

            return response;
        }
    }
}
