using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface ILocationProcessorCredentialService: IDisposable
    {
        bool SaveLocationProcessorCredentials(LocationProcessorCredentials locationProcessorCredenntial,string timeZone, ZeoContext context);

        List<LocationProcessorCredentials> GetLocationProcessorCredentials(long locationId, ZeoContext context);

    }
}
