namespace MGI.Common.Service
{
	partial class CommonServiceInstaller
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.AlloyServiceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
			this.AlloyServiceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// AlloyServiceProcessInstaller1
			// 
			this.AlloyServiceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.NetworkService;
			this.AlloyServiceProcessInstaller1.Password = null;
			this.AlloyServiceProcessInstaller1.Username = null;
			// 
			// AlloyServiceInstaller
			// 
			this.AlloyServiceInstaller.Description = "Alloy Archieve Windows Service";
			this.AlloyServiceInstaller.DisplayName = "Alloy Service";
			this.AlloyServiceInstaller.ServiceName = "Alloy Service";
			this.AlloyServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AlloyServiceProcessInstaller1,
            this.AlloyServiceInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller AlloyServiceProcessInstaller1;
		private System.ServiceProcess.ServiceInstaller AlloyServiceInstaller;
	}
}