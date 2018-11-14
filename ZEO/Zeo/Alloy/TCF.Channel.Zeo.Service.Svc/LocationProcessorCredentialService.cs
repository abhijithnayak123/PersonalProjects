using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Contract;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : ILocationProcessorCredentialsService
    {
        public ILocationProcessorCredentialService LocationProcessCredential;


        public Response GetLocationProcessorCredentials(long locationId, ZeoContext context)
        {
            return serviceEngine.GetLocationProcessorCredentials(locationId, context);
        }

        public Response SaveLocationProcessorCredential(LocationProcessorCredentials processorCredentials, ZeoContext context)
        {
            return serviceEngine.SaveLocationProcessorCredential(processorCredentials, context);
        }
    }
}
