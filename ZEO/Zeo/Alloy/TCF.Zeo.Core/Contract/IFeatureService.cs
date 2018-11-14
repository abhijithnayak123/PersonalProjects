using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;

namespace TCF.Zeo.Core.Contract
{
    public interface IFeatureService : IDisposable
    {
        List<FeatureDetails> GetFeatures(ZeoContext context);

        bool UpdateFeatures(List<FeatureDetails> features, ZeoContext context);
    }
}
