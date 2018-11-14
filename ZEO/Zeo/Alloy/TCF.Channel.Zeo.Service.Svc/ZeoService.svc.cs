using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TCF.Channel.Zeo.Data;

#region External References
using Spring.Context;
using Spring.Context.Support;
#endregion

#region Zeo References
using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Service.Impl;
#endregion

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IZeoService
    {
        IZeoService serviceEngine { get; set; }
        public ZeoService() 
        {
            serviceEngine = new ZeoServiceImpl();
            IApplicationContext ctx = ContextRegistry.GetContext();
            serviceEngine = (IZeoService)ctx.GetObject("serviceEngine");
        }
    }
}
