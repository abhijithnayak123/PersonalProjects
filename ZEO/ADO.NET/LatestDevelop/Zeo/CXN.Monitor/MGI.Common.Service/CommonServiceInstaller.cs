using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace MGI.Common.Service
{
	[RunInstaller(true)]
	public partial class CommonServiceInstaller : System.Configuration.Install.Installer
	{
		public CommonServiceInstaller()
		{
			InitializeComponent();
			var config = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);
			string environmentName = "";

			if (config.AppSettings.Settings["ServiceNameInEnvironment"] != null)
			{
				environmentName = config.AppSettings.Settings["ServiceNameInEnvironment"].Value;
			}

			if (!string.IsNullOrWhiteSpace(environmentName))
			{
				this.AlloyServiceInstaller.DisplayName = string.Format("{0} - {1}", this.AlloyServiceInstaller.DisplayName, environmentName);
			}

		}
	}
}
