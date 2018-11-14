using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IFeatureService
    {
        public Response GetFeatures(ZeoContext context)
        {
            return serviceEngine.GetFeatures(context);
        }

        public Response UpdateFeatures(List<FeatureDetails> features, ZeoContext context)
        {
            return serviceEngine.UpdateFeatures(features, context);
        }
    }
}