using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IFeatureService
    {
        [OperationContract]
        [FaultContract(typeof(commonData.ZeoSoapFault))]
        Response GetFeatures(ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(commonData.ZeoSoapFault))]
        Response UpdateFeatures(List<FeatureDetails> features, ZeoContext context);
    }
}
