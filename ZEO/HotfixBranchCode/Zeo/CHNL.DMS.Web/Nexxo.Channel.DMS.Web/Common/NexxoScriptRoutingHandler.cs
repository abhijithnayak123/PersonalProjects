using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MGI.Channel.DMS.Web.Common
{
    public class NexxoScriptRoutingHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NexxoScriptTranslator(requestContext);
        }
    }
}