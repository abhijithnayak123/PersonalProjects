using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
    public interface IFeatureService
    {
        List<FeatureDetails> GetFeatures(commonData.ZeoContext context);

        bool UpdateFeatures(List<FeatureDetails> features, commonData.ZeoContext context);
    }
}
