using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace Peripheral.Service.CustomAction
{
	public class PrerequisitesActions
	{
        /// <summary>
        /// Prerequiste Check
        /// </summary>
        /// <returns></returns>
        public bool Prerequistes()
        {
            PrerequisitesActions prerequisitesActions = new PrerequisitesActions();
            Log.Writer(PeripheralConfig.INSTALL, "Starting Checking:: Prerequistes()::");
            prerequisitesActions.PrerequisiteCheck();

            if (PreRequisiteStatus.IsWindowsEdition
                && PreRequisiteStatus.IsSystemMemoryAvailable
                && PreRequisiteStatus.IsDiskSpaceAvailable
                && PreRequisiteStatus.IsWinInstallerAvailable
                && PreRequisiteStatus.IsMultiFunctionDeviceAvailable)
            {              
                return true;
            }
            else
            {
                //Stop Installer
                Log.Writer(PeripheralConfig.ERROR, "Failed::Prerequistes()");
                return false;
            }
        }

        /// <summary>
        /// Prerequisite Checking
        /// </summary>
        private void PrerequisiteCheck()
		{
            Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::DiskSpace::Begin::");
            DiskSpaceAvailableCheck();
            Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::DiskSpace::End::");

            Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::Win Installer::Begin::");
            WinInstallerAvailableCheck();
            Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::Win Installer::End::");

            Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::System Memory::Begin::");
            SystemMemoryAvailableCheck();
            Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::System Memory::End::");

            Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::Operating System::Begin::");
            OperatingSystemCheck();
            Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::Operating System::End::");

            //Al-2507: The below change was done as the requirement i.e.As Product, update the MSI deployment package for PS to not require Drivers
            //Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::EPSON Roll Printer::Begin::");
            //EPSONRollPrinterCheck();
            //Log.Writer(PeripheralConfig.INSTALL, "PrerequisiteCheck()::EPSON Roll Printer::End::");

            PreRequisiteStatus.IsMultiFunctionDeviceAvailable = true;
		}
        
        /// <summary>
        /// Disk Space Available Check
        /// </summary>
		private void DiskSpaceAvailableCheck()
		{
			try
			{
				bool diskSpace = false;
				DriveInfo[] allDrives = DriveInfo.GetDrives();
                Log.Writer(PeripheralConfig.INSTALL, "Diskspace Checking Starts");
				foreach (DriveInfo driveinfo in allDrives)
				{
					if (driveinfo.IsReady == true)
					{
						if (driveinfo.AvailableFreeSpace > 262144000) //250MB 
						{
							diskSpace = true;
                         	break;
						}
						else
						{
							continue;
						}
					}
				}
				if (diskSpace)
				{
					PreRequisiteStatus.IsDiskSpaceAvailable = true;
				    Log.Writer(PeripheralConfig.INSTALL, "Success: " + PeripheralConfig.LOCALDISKSPACE_CHECK_MESSAGE);
				}
				else
				{
					PreRequisiteStatus.IsDiskSpaceAvailable = false;
				    Log.Writer(PeripheralConfig.ERROR, "Failed: " + PeripheralConfig.LOCALDISKSPACE_CHECK_MESSAGE);
				}
                Log.Writer(PeripheralConfig.INSTALL, "Diskspace Checking Ends");
			}
			catch (Exception ex)
			{
				PreRequisiteStatus.IsDiskSpaceAvailable = false;
                Log.Writer(PeripheralConfig.ERROR, "DiskSpace Exeception: " + ex.Message);
			}
		}

        /// <summary>
        /// Windows Installer Available Check
        /// </summary>
		private void WinInstallerAvailableCheck()
		{
			try
			{
                Log.Writer(PeripheralConfig.INSTALL, "Windows Installer Checking Starts");
				bool InstallerExists = Prerequisite.GetWinInstaller();
				if (InstallerExists)
				{
					PreRequisiteStatus.IsWinInstallerAvailable = true;
			        Log.Writer(PeripheralConfig.INSTALL, "Success: " + PeripheralConfig.WINDOWSINSTALLER_CHECK_MESSAGE);
				}
				else
				{
					PreRequisiteStatus.IsWinInstallerAvailable = false;
                    Log.Writer(PeripheralConfig.ERROR, "Falied: " + PeripheralConfig.WINDOWSINSTALLER_CHECK_MESSAGE);
				}
                Log.Writer(PeripheralConfig.INSTALL, "Windows Installer Checking Ends");
			}
			catch (Exception ex)
			{
				PreRequisiteStatus.IsWinInstallerAvailable = false;
                Log.Writer(PeripheralConfig.INSTALL, "WinInstallerAvailableCheck Execption: " + ex.Message);
			}
		}

        /// <summary>
        /// System Memory Available Check
        /// </summary>
		private void SystemMemoryAvailableCheck()
		{

			try
			{
                Log.Writer(PeripheralConfig.INSTALL, "System Memory Checking Starts");
				bool systemMemory = Prerequisite.GetSystemMemory();
				if (systemMemory)
				{
					PreRequisiteStatus.IsSystemMemoryAvailable = true;
                    Log.Writer(PeripheralConfig.INSTALL, "Success: " + PeripheralConfig.SYTEMMEMORY_CHECK_MESSAGE);
				}
				else
				{
					PreRequisiteStatus.IsSystemMemoryAvailable = false;
                    Log.Writer(PeripheralConfig.ERROR, "Failed: " + PeripheralConfig.SYTEMMEMORY_CHECK_MESSAGE);
				}
                Log.Writer(PeripheralConfig.INSTALL, "System Memory Checking Ends");
			}
			catch (Exception ex)
			{
				PreRequisiteStatus.IsDiskSpaceAvailable = false;
                Log.Writer(PeripheralConfig.INSTALL, "System Memory Exeception:: " + ex.Message);
			}
		}

        /// <summary>
        /// Operating System Check
        /// </summary>
		private void OperatingSystemCheck()
		{
			try
			{
                Log.Writer(PeripheralConfig.INSTALL, PeripheralConfig.OS_CHECK_MESSAGE + " " + "checking starts");
                string operatingSystem = Prerequisite.GetOSVersion();

                if (operatingSystem.Equals("Windows 7") || operatingSystem.Equals("Windows 8"))
				{
                    bool is64bitOS = OSBitVersion.Is64BitOperatingSystem();
					if (is64bitOS)
					{
						PreRequisiteStatus.IsWindowsEdition = true;
                        Log.Writer(PeripheralConfig.INSTALL, "Success: " + PeripheralConfig.OS_WIN7_64_MESSAGE);
					}
					else
					{
						PreRequisiteStatus.IsWindowsEdition = true;
                        Log.Writer(PeripheralConfig.INSTALL, "Success: " + PeripheralConfig.OS_WIN7_32_MESSAGE);
					}
				}
				else
				{
					PreRequisiteStatus.IsWindowsEdition = false;
                    Log.Writer(PeripheralConfig.ERROR, "Failed: " + PeripheralConfig.OS_CHECK_MESSAGE + " " + PeripheralConfig.OS_WIN7_MESSAGE);
				}
                Log.Writer(PeripheralConfig.INSTALL,  PeripheralConfig.OS_CHECK_MESSAGE + " " +"checking ends");
			}
			catch (Exception ex)
			{
				PreRequisiteStatus.IsDiskSpaceAvailable = false;
				Log.Writer(PeripheralConfig.ERROR, "System Memory Execption: " + ex.Message);
			}
		}

        /// <summary>
        /// Epson Roll Printer Check
        /// </summary>
        private void EPSONRollPrinterCheck()
        {
            try
            {
                Log.Writer(PeripheralConfig.INSTALL, "Epson Roll Printer Checking Starts");
                if (Prerequisite.IsEsponRollerInstalled())
                {
                    PreRequisiteStatus.IsEPSONRollPrinter = true;
                    Log.Writer(PeripheralConfig.INSTALL, "Success: " + PeripheralConfig.EPSON_ROLL_Printer_MESSAGE);
                }
                else
                {
                    PreRequisiteStatus.IsEPSONRollPrinter = false;
                    Log.Writer(PeripheralConfig.ERROR, "Failed: " + PeripheralConfig.EPSON_ROLL_Printer_MESSAGE);
                }
                Log.Writer(PeripheralConfig.INSTALL, "Epson Roll Printer Checking Ends");
            }
            catch (Exception ex)
            {
                PreRequisiteStatus.IsEPSONRollPrinter = false;
                Log.Writer(PeripheralConfig.ERROR, "EPSON ROLL PRINTER Execption: " + ex.Message);
            }
        }
	}
}