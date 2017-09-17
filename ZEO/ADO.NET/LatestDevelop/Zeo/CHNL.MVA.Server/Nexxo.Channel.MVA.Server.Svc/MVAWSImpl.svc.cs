using System.Collections.Generic;
using Spring.Context;
using MGI.Channel.MVA.Server.Contract;

namespace MGI.Channel.MVA.Server.Svc
{
    public partial class MVAWSImpl : IMVAService
    {
		private static string MVA_ENGINE = "MVAEngine";

		IMVAService MVAEngine { get; set; }
        public MVAWSImpl()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            MVAEngine = (IMVAService)ctx.GetObject(MVA_ENGINE);
            SetSelf(MVAEngine);
		}

        public void SetSelf(IMVAService dts)
        {
            MVAEngine.SetSelf(dts);
        }

		public MGI.Common.Util.MGIContext GetPartnerContext(string channelPartnerName)
        {
            return MVAEngine.GetPartnerContext(channelPartnerName);
        }

        public MGI.Common.Util.MGIContext GetCustomerContext(long customerSessionId)
        {
            return MVAEngine.GetCustomerContext(customerSessionId);
        }
    }
}
