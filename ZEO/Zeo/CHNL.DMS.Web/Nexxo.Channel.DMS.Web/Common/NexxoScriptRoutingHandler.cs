using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace TCF.Channel.Zeo.Web.Common
{
    public class NexxoScriptRoutingHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NexxoScriptTranslator(requestContext);
        }
    }
}