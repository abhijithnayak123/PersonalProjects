using TCF.Zeo.Biz.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;
using AutoMapper;
using CoreData = TCF.Zeo.Core;

namespace TCF.Zeo.Biz.Impl
{
    public class ZeoContextServiceImpl : IZeoContext
    {
        IMapper mapper;

        public ZeoContextServiceImpl()
        {

        }

        /// <summary>
        /// This method is used to return the AlloyContext when the agent session is initiated.
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public commonData.ZeoContext GetZeoContextForAgent(long agentSessionId, commonData.ZeoContext context)
        {
            CoreData.Impl.ZeoContextServiceImpl alloyContextService = new CoreData.Impl.ZeoContextServiceImpl();

            return alloyContextService.GetZeoContextForAgent(agentSessionId, context);
        }

        /// <summary>
        /// This method is used to return the AlloyContext when the customer session is initiated.
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        public commonData.ZeoContext GetZeoContextForCustomer(long customerSessionId, commonData.ZeoContext context)
        {
            CoreData.Impl.ZeoContextServiceImpl alloyContextService = new CoreData.Impl.ZeoContextServiceImpl();

            return alloyContextService.GetZeoContextForCustomer(customerSessionId, context);
        }
    }
}
