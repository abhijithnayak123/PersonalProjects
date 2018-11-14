using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commonData = TCF.Zeo.Common.Data;
namespace TCF.Zeo.Biz.Contract
{
    public interface ILocationProcessorCredentialService
    {
        bool SaveLocationProcessorCredentials(LocationProcessorCredentials locProcessorCredential, commonData.ZeoContext context);

        List<LocationProcessorCredentials> GetLocationProcessorCredentials(long locationId, commonData.ZeoContext context);
    }
}
