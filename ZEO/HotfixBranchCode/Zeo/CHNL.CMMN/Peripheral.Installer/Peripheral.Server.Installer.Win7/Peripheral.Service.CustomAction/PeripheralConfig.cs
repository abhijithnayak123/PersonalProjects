namespace Peripheral.Service.CustomAction
{
	public class PeripheralConfig
	{
		public const string TCPPORT_CHECK_MESSAGE            = "Open TCP port for Nexxo Peripheral Server (default is TCP port 18731)";
		public const string SERVICE_CONNECTIVITY             = "Nexxo DMS Server connectivity";
		public const string WINDOWSINSTALLER_CHECK_MESSAGE   = "Microsoft Windows Installer 3.1 or later";
		public const string OS_CHECK_MESSAGE                 = "Supported operating system";
        public const string OS_WIN7_MESSAGE                  = "Windows 7 (32 or 64 bit)";
        public const string OS_WIN7_32_MESSAGE               = "Windows 7 (32 bit)";
        public const string OS_WIN7_64_MESSAGE               = "Windows 7 (64 bit)";
		public const string FRAMEWORK_CHECK_MESSAGE          = "Microsoft .NET 4.0 Extended";
		public const string SYTEMMEMORY_CHECK_MESSAGE        = "2 GB of system memory (RAM)";
		public const string LOCALDISKSPACE_CHECK_MESSAGE     = "250MB free local disk space";
		public const string EPSON_CHECK_MESSAGE              = "Multi-function device with printer";
		public const int PORT_NUMBER						 = 18731;
		public const int HTTPS_PORT_NUMBER					 = 18732;
        public const string EPSON_ROLL_Printer_MESSAGE       = "EPSON ROLL Printer Driver";
        public const string PERIPHERAL_SERVICE_URL           = "https://nps.nexxofinancial.com:{0}/Peripheral/";
        public const string INSTALL                          = "Install";
        public const string UNINSTALL                        = "UnInstall";
        public const string ERROR                            = "Error";
        public const string INSTALLLOG                       = "Install.log";
        public const string ERRORLOG                         = "Error.log";
        public const string UNINSTALLLOG                     = "UnInstall.log";
        public const string UNINSTALLREGNXO                  = "unregnxo.bat";
        public const string FILENAME                         = "PSConfig.txt";
        public const string CLIENT                           = "ClientID";
        public const string INSTALLLOGFOLDER                 = "InstallLogFolder";
        public const string ERRORLOGFOLDER                   = "InstallErrorFolder";
        public const string SERVICEURL                       = "ServiceURL";        
	    public const string PRODUCTVERSION                   = "3.4.4.0";
    }
}
