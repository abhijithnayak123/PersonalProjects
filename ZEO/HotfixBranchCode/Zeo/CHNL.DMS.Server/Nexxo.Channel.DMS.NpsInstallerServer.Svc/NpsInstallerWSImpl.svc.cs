using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MGI.Channel.DMS.NpsInstallerServer.Contract;
using Spring.Context;

namespace MGI.Channel.DMS.NpsInstallerServer.Svc
{
	public partial class NpsInstallerWSImpl : INpsInstallerService
	{
		private static string NPSINSTALLER_ENGINE = "NpsInstallerEngine";

		INpsInstallerService NpsInstallerEngine { get; set; }
		public NpsInstallerWSImpl()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			NpsInstallerEngine = (INpsInstallerService)ctx.GetObject(NPSINSTALLER_ENGINE);
		}
	}
}
