using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using commonData = TCF.Zeo.Common.Data;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Impl;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IZeoContext
    {
        /// <summary>
        /// This method is used to return the AlloyContext when the agent session is initiated.
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <returns></returns>
        public Response GetZeoContextForAgent(long agentSessionId, ZeoContext context)
        {
            ZeoContextServiceImpl bizZeoContextService = new ZeoContextServiceImpl();
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            context = mapper.Map<ZeoContext>(bizZeoContextService.GetZeoContextForAgent(agentSessionId, commonContext));
            Response result = new Response() { Result = context };
            return result;
        }

        /// <summary>
        /// This method is used to return the AlloyContext when the customer session is initiated.
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        public Response GetZeoContextForCustomer(long customerSessionId, ZeoContext context)
        {
            ZeoContextServiceImpl bizZeoContextService = new ZeoContextServiceImpl();
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            context = mapper.Map<ZeoContext>(bizZeoContextService.GetZeoContextForCustomer(customerSessionId, commonContext));
            Response result = new Response()
            {
                Result = context
            };
            return result;
        }
    }
}
