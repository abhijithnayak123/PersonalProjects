using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface IZeoContext
    {
        /// <summary>
        /// This method is used to return the AlloyContext when the agent session is initiated. 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <returns></returns>
        ZeoContext GetZeoContextForAgent(long agentSessionId, ZeoContext context);

        /// <summary>
        /// This method is used to return the AlloyContext when the customer session is initiated.
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        ZeoContext GetZeoContextForCustomer(long customerSessionId, ZeoContext context);
    }
}
