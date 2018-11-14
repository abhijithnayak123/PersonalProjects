using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Data
{
    public class FeatureDetails : ZeoModel
    {
        public int FeatureId { get; set; }

        public string FeatureName { get; set; }

        public bool IsEnabled { get; set; }
    }
}
