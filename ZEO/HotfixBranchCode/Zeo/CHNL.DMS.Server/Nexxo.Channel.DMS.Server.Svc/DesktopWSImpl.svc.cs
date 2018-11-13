using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using Spring.Context;
using Spring.Context.Support;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IDesktopService
	{
		private static string DESKTOP_ENGINE = "DesktopEngine";

		IDesktopService DesktopEngine { get; set; }
		public DesktopWSImpl()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			DesktopEngine = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);
            DesktopEngine.SetSelf(DesktopEngine);

		}
    }
}
