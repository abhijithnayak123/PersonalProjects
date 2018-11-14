using System.Collections.Generic;
using AutoMapper;
using MGI.Biz.Partner.Contract;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using Spring.Transaction.Interceptor;


namespace MGI.Channel.DMS.Server.Impl
{
    public partial class DesktopEngine : Server.Contract.ILocationCounterIdService
    {
        public MGI.Biz.Partner.Contract.ILocationCounterIdService LocationCounterIdService { private get; set; }

       
        [Transaction()]
		public Response UpdateCounterId(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Response response = new Response();

			response.Result = LocationCounterIdService.UpdateCounterId(customerSessionId, context);
			return response;
		}
	}
}
