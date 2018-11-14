using System;
using System.Threading;
using System.Linq;
using Microsoft.Deployment.WindowsInstaller;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace Peripheral.Service.CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult PrerequisiteCheck(Session session)
        {
            //System.Diagnostics.Debugger.Launch();
            Log.WriteEvent("Starting prerequisite checks");
            ActionResult InstallStatus = ActionResult.Failure;

            CustomActivites customActivites = new CustomActivites();
            Log.Writer(PeripheralConfig.INSTALL, "Begin::DeleteFile::PrerequisiteCheck()::");
            customActivites.DeleteFile();
            Log.Writer(PeripheralConfig.INSTALL, "End::DeleteFile::PrerequisiteCheck()::");

            Log.Writer(PeripheralConfig.INSTALL, "Begin::GetFile::PrerequisiteCheck()::");
            if (customActivites.GetFile(session))
            {
                Log.Writer(PeripheralConfig.INSTALL, "End::GetFile::PrerequisiteCheck()::");               
                Log.Writer(PeripheralConfig.INSTALL, "Begin::CheckPrerequistes()::");
                if (customActivites.CheckPrerequistes())
                {
                    Log.WriteEvent("Prerequisite checks completed");
                    Log.Writer(PeripheralConfig.INSTALL, "End::CheckPrerequistes()::Sucess");

                    Log.Writer(PeripheralConfig.INSTALL, "Begin::SaveNPSDetails::PrerequisiteCheck()::");
                    customActivites.SaveNPSDetails();
                    Log.Writer(PeripheralConfig.INSTALL, "End::SaveNPSDetails::PrerequisiteCheck()::");

                    Log.Writer(PeripheralConfig.INSTALL, "Begin::BatchExecuteDetails::PrerequisiteCheck()::");
                    customActivites.BatchExecuteDetails(session);
                    Log.Writer(PeripheralConfig.INSTALL, "End::BatchExecuteDetails::PrerequisiteCheck()::");
                    Log.WriteEvent("Peripheral Service would be installed");
                    InstallStatus = ActionResult.Success;
                }                
            }

            try
            {
                switch (InstallStatus)
                {
                    case ActionResult.Failure:
                        Log.Writer(PeripheralConfig.ERROR, "End::Prerequistes::PrerequisiteCheck()::Faliure");
                        Log.FileCopy(FileConfig.InstallLogFolder, FileConfig.InstallErrorFolder, PeripheralConfig.ERROR);
                        break;
                    case ActionResult.Success:
                        Log.FileCopy(FileConfig.InstallLogFolder, FileConfig.InstallErrorFolder, PeripheralConfig.INSTALL);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.WriteEvent("Copying faliure in  Install log ::" + ex.Message);
            }

            return InstallStatus;
        }

        [CustomAction]
        public static ActionResult UnInstallNXO(Session session)
        {
            //System.Diagnostics.Debugger.Launch();
            Log.WriteEvent("Uninstalling Peripheral Service");
            ActionResult unInstallStatus = ActionResult.Failure;
            CustomActivites customActivites = new CustomActivites();
            
            string installDirectory = session["INSTALLLOCATION"];
            Log.Writer(PeripheralConfig.UNINSTALL, "UnInstallNXO()::Begin " + installDirectory);
            try
            {
                var processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = "\"" + installDirectory + "\\" + PeripheralConfig.UNINSTALLREGNXO + "\"";
                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.CreateNoWindow = true;
                Process proc = Process.Start(processStartInfo);
                Log.Writer(PeripheralConfig.UNINSTALL, "UnInstallNXO()::End");
                unInstallStatus = ActionResult.Success;
            }
            catch (Exception ex)
            {
                Log.WriteEvent("Uninstalling Zeo Peripheral Service Failed");
                Log.Writer(PeripheralConfig.UNINSTALL, "UnInstallNXO()::Failed: Exception" + ex.Message);
            }

            if (customActivites.GetFile(session))
            {
                //reading file
                Log.FileCopy(FileConfig.InstallLogFolder, FileConfig.InstallErrorFolder, PeripheralConfig.UNINSTALL);
            }
            return unInstallStatus;
        }        
    }
}