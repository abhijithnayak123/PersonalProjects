using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Diagnostics;
using System.IO;

namespace Nexxo.Wrapper.Epson.CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult InstallEPSON(Session session)
        {
          //System.Diagnostics.Debugger.Launch();
            Log.WriteEvent("Epson installation is initiated");
            try
            {
                EpsonActivities epsonActivities = new EpsonActivities();
                epsonActivities.LogDelete();

                if (epsonActivities.FileCheck(session))
                {
                    string driverName = "EPSON TM-S9000 Driver Version 1.02 [32-bit]";
                    string installDirectory = session["INSTALLLOCATION"];
                    installDirectory = installDirectory.TrimEnd('\\');
                    Log.Writer(EpsonConfig.EPSONINSTALL, "Installation Directory " + " - " + installDirectory);
                    var processStartInfo = new ProcessStartInfo();

                    Log.Writer(EpsonConfig.EPSONINSTALL, "OSCheck():: Starting");
                    bool is64bitOS = OSVersion.Is64BitOperatingSystem();
                    Log.Writer(EpsonConfig.EPSONINSTALL, "OSCheck():: Ended");
                    if (is64bitOS)
                    {
                        Log.Writer(EpsonConfig.EPSONINSTALL, "OS::64Bit");
                        driverName = "EPSON TM-S9000 Driver Version 1.02 [32-bit/64-bit]";
                        if (InstalledApp.IsApplictionInstalled(driverName))
                        {
                            Log.WriteEvent("EPSON TM-S9000 Driver Version 1.02 [32-bit/64-bit] is already installed, Will uninstall and install");
                            Log.Writer(EpsonConfig.EPSONINSTALL, "EPSON TM-S9000 Driver Version 1.02 [32-bit/64-bit] is already installed, Will uninstall and install");
                            processStartInfo.FileName = "\"" + installDirectory + "\\" + EpsonConfig.INSTALLEPSONBAT64 + "\"";
                        }
                        else
                        {
                            Log.WriteEvent("EPSON TM-S9000 Driver Version 1.02 [32-bit/64-bit]is not installed, Will install");
                            Log.Writer(EpsonConfig.EPSONINSTALL, "EPSON TM-S9000 Driver Version 1.02[32-bit/64-bit] is not installed, Will  install");
                            processStartInfo.FileName = "\"" + installDirectory + "\\" + EpsonConfig.INSTALLEPSONBAT64ALT + "\"";
                        }
                    }
                    else
                    {
                        Log.Writer(EpsonConfig.EPSONINSTALL, "OS::32Bit");
                        if (InstalledApp.IsApplictionInstalled(driverName))
                        {
                            Log.WriteEvent("EPSON TM-S9000 Driver Version 1.02 [32-bit] is already installed, Will uninstall and install");
                            Log.Writer(EpsonConfig.EPSONINSTALL, "EPSON TM-S9000 Driver Version 1.02 [32-bit] is already installed, Will uninstall and install");
                            processStartInfo.FileName = "\"" + installDirectory + "\\" + EpsonConfig.INSTALLEPSONBAT32 + "\"";
                        }
                        else
                        {
                            Log.WriteEvent("EPSON TM-S9000 Driver Version 1.02[32-bit] is not installed, Will install");
                            Log.Writer(EpsonConfig.EPSONINSTALL, "EPSON TM-S9000 Driver Version 1.02[32-bit] is not installed, Will  install");
                            processStartInfo.FileName = "\"" + installDirectory + "\\" + EpsonConfig.INSTALLEPSONBAT32ALT + "\"";
                        }
                    }
                    processStartInfo.Verb = "runas";
                    processStartInfo.UseShellExecute = true;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    processStartInfo.CreateNoWindow = true;

                    Log.Writer(EpsonConfig.EPSONINSTALL, "ProcessStartInfo::Starting - " + processStartInfo.FileName);
                    Log.WriteEvent("Initiating Epson Driver Uninstallation and Installation Process");
                    Process proc = Process.Start(processStartInfo);
                    Log.WriteEvent("Batch Process started and not waiting for completion");
                    Log.Writer(EpsonConfig.EPSONINSTALL, "ProcessStartInfo::Not waiting for process to complete " + processStartInfo.FileName);

                    Log.Writer(EpsonConfig.EPSONINSTALL, "FileCopy::Checking Path::" + session["INSTALLLOGFOLDER"]);
                    try
                    {
                        Log.FileCopy(session["INSTALLLOGFOLDER"], EpsonConfig.EPSONINSTALL);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteEvent("Failed to copy log files to destination folder. Proceed with Install " + ex.Message);
                    }
                    Log.Writer(EpsonConfig.EPSONINSTALL, "Log FileCopy::Ended");
                }
                else
                {
                    Log.WriteEvent("Nexxo Configuration NexxoConfig.txt not found");
                }
            }
            catch (Exception ex)
            {
                Log.WriteEvent("Failed to install Epson Drivers " + ex.Message);
                Log.Writer(EpsonConfig.EPSONINSTALL, "Exception::Install Epson():: " + ex.Message);

            }
            return ActionResult.Success;
        }

        
        /// <summary>
        /// Calling bat file
        /// Copying Epson PDS driver
        /// to temp folder
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static void CopyEpson(Session session)
        {
            Log.Writer(EpsonConfig.EPSONUNINSTALL, "CopyEpson()::Begin");
            string installDirectory = session["INSTALLLOCATION"];
            Log.Writer(EpsonConfig.EPSONUNINSTALL, "CopyEpson()::Install Directory - " + installDirectory);
            try
            {
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "OSCheck():: Starting");
                bool is64bitOS = OSVersion.Is64BitOperatingSystem();
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "OSCheck():: Ended");

                var processStartInfo = new ProcessStartInfo();
                if (is64bitOS)
                {
                    Log.Writer(EpsonConfig.EPSONUNINSTALL, "OS::64Bit");
                    processStartInfo.FileName = "\"" + installDirectory + "InitUninstallEpson64.bat" + "\"";
                    Log.Writer(EpsonConfig.EPSONUNINSTALL, "processStartInfo.FileName:: " + processStartInfo.FileName);
                }
                else
                {
                    Log.Writer(EpsonConfig.EPSONUNINSTALL, "OS::32Bit");
                    processStartInfo.FileName = "\"" + installDirectory + "InitUninstallEpson32.bat" + "\"";
                    Log.Writer(EpsonConfig.EPSONUNINSTALL, "processStartInfo.FileName:: " + processStartInfo.FileName);
                }

                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.CreateNoWindow = true;
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "CopyEpson Insitiating Process for Epson removal");
                Process proc = Process.Start(processStartInfo);
                proc.WaitForExit();
                proc.Close();
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "CopyEpson()::End");
            }
            catch (Exception ex)
            {
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "CopyEpson()::Failed: Exception" + ex.Message);
            }
        }

        /// <summary>
        /// Calling bat file
        /// Uninstall Epson PDS driver
        /// from temp folder
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static void RemoveEpson(Session session)
        {
       
            string installDirectory = Path.GetTempPath(); //session["INSTALLLOCATION"];
            
            Log.Writer(EpsonConfig.EPSONUNINSTALL, "RemoveEpson()::Begin Temp Path" + installDirectory);
            try
            {
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "OSCheck():: Starting");
                bool is64bitOS = OSVersion.Is64BitOperatingSystem();
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "OSCheck():: Ended");
                //////////
                var processStartInfo = new ProcessStartInfo();
                if (is64bitOS)
                {
                    Log.Writer(EpsonConfig.EPSONUNINSTALL, "OS::64Bit");
                    processStartInfo.FileName = "\"" + installDirectory + EpsonConfig.UNINSTALLEPSONBAT64 + "\"";
                    Log.Writer(EpsonConfig.EPSONUNINSTALL, "processStartInfo.FileName:: " + processStartInfo.FileName);
                }
                else
                {
                    Log.Writer(EpsonConfig.EPSONUNINSTALL, "OS::32Bit");
                    
                    //string installDirectoryWIn32 = "C:\\Users\\ADMINI~1\\AppData\\Local\\Temp\\";

                    processStartInfo.FileName = "\"" + installDirectory + EpsonConfig.UNINSTALLEPSONBAT32 + "\"";
                    Log.Writer(EpsonConfig.EPSONUNINSTALL, "processStartInfo.FileName:: " + processStartInfo.FileName);
                }

                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.CreateNoWindow = true;
                Process proc = Process.Start(processStartInfo);
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "RemoveEpson()::End");      
            }
            catch (Exception ex)
            {
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "RemoveEpson()::Failed: Exception" + ex.Message);                
            }
        }

        [CustomAction]
        public static ActionResult UnInstallEPSON(Session session)
        {
            //System.Diagnostics.Debugger.Launch();
            Log.WriteEvent("Starting Uninstallation of Epson Drivers");
            ActionResult unInstallStatus = ActionResult.Failure;

            EpsonActivities epsonActivities = new EpsonActivities();
            Log.Writer(EpsonConfig.EPSONUNINSTALL, "UnInstallEPSON()::Begin ");
            try
            {
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "UnInstallEPSON:CopyEpson()::Call ");
                Log.WriteEvent("Starting Uninstallation of SDK Driver");
                CopyEpson(session);
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "UnInstallEPSON:CopyEpson()::Call End ");
                Log.WriteEvent("Starting Uninstallation of PDS Driver");
                RemoveEpson(session);
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "UnInstallEPSON()::End ");
                unInstallStatus = ActionResult.Success;
            }
            catch (Exception ex)
            {
                Log.WriteEvent("Failed to Uninstall Epson Drivers");
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "UnInstallEPSON()::Failed: Exception" + ex.Message);
                unInstallStatus = ActionResult.Failure;
            }

            if (epsonActivities.FileCheck(session))
            {
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "UnInstallEPSON:FileCopy - Installation Folder:" + session["INSTALLLOGFOLDER"]);
                Log.FileCopy(session["INSTALLLOGFOLDER"], EpsonConfig.EPSONUNINSTALL);
                Log.Writer(EpsonConfig.EPSONUNINSTALL, "UnInstallEPSON:FileCopy FileCopy::Call End");
            }
            return unInstallStatus;
        }
    }
}
