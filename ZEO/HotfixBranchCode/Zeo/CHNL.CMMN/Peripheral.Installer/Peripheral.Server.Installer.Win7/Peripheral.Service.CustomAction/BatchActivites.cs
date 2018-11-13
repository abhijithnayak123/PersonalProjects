using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Diagnostics;

namespace Peripheral.Service.CustomAction
{
    public class BatchActivites
    {
        public void BatchExecute(Session session)
        {
            Log.Writer(PeripheralConfig.INSTALL, "Starting BindingPermission()::");
            BindingPermission(session);
            Log.Writer(PeripheralConfig.INSTALL, "Ended BindingPermission()::");

            Log.Writer(PeripheralConfig.INSTALL, "Starting BindingHttpcfg()::");
            BindingHttpcfg(session);
            Log.Writer(PeripheralConfig.INSTALL, "Ended BindingHttpcfg()::");
        }

        /// <summary>
        /// BindingPermission
        /// </summary>
        /// <param name="session"></param>
        private static void BindingPermission(Session session)
        {
            Log.Writer(PeripheralConfig.INSTALL, "BindingPermission()::Begin ");
            try
            {
                var processStartInfo = new ProcessStartInfo();
                string installDirectory = session["INSTALLLOCATION"];
                installDirectory = installDirectory.TrimEnd('\\');
                processStartInfo.FileName = "\"" + installDirectory + "\\" + "PeripheralPerm.bat\"";
                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.CreateNoWindow = true;
                Process proc = Process.Start(processStartInfo);
                proc.WaitForExit();
                proc.Close();
                Log.Writer(PeripheralConfig.INSTALL, "BindingPermission()::End ");
            }
            catch (Exception ex)
            {
                Log.Writer(PeripheralConfig.ERROR, "BindingPermission(): Exception " + ex.Message);
            }
        }

        /// <summary>
        /// BindingHttpcfg
        /// </summary>
        /// <param name="session"></param>
        private static void BindingHttpcfg(Session session)
        {
            Log.Writer(PeripheralConfig.INSTALL, "BindingHttpcfg()::Begin ");
            try
            {
                string installDirectory = session["INSTALLLOCATION"];
                installDirectory = installDirectory.TrimEnd('\\');
                string checkFile1 = installDirectory + "\\" + "configuresys.bat";
                string checkFile2 = installDirectory + "\\" + "registernxo.reg";
                if (System.IO.File.Exists(checkFile1) && System.IO.File.Exists(checkFile2))
                {
                    ReplaceLocation("registernxo.reg", installDirectory);
                }
                else
                {
                    Log.Writer(PeripheralConfig.INSTALL, "Binding of HTTP CFG - Could not fild batch file or registry file");
                    return;
                }
                var processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = "\"" + installDirectory + "\\" + "configuresys.bat\"";
                Log.Writer(PeripheralConfig.INSTALL, "File Name Httpcfg: *" + processStartInfo.FileName + "*");
                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.CreateNoWindow = true;

                Log.Writer(PeripheralConfig.INSTALL, "Binding of HTTP CFG Process Start");
                Process proc = Process.Start(processStartInfo);
                //proc.WaitForExit();
                //proc.Close();
                Log.Writer(PeripheralConfig.INSTALL, "BindingHttpcfg()::End");
            }
            catch (Exception ex)
            {
                Log.Writer(PeripheralConfig.ERROR, "BindingHttpcfg(): Exception" + ex.Message);
            }
        }

        /// <summary>
        /// ExecuteHttpcfg
        /// </summary>
        /// <param name="session"></param>
        private static void ExecuteHttpcfg(Session session)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo();
                string installDirectory = session["INSTALLLOCATION"];
                installDirectory = installDirectory.TrimEnd('\\');
                processStartInfo.FileName = "\"" + installDirectory + "\\" + "httpcfgexec.bat\" ";
                Log.Writer(PeripheralConfig.INSTALL, "File Name Httpcfg: " + processStartInfo.FileName);
                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = false;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.Arguments = "\"" + installDirectory + "\\" + "httpcfg.exe" + "\"";
                Process proc = Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                Log.Writer(PeripheralConfig.ERROR, "Failed ExecuteHttpcfg: Exception" + ex.Message);
            }
        }

        /// <summary>
        /// ReplaceLocation
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="installLocation"></param>
        private static void ReplaceLocation(string fileName, string installLocation)
        {
            Log.Writer(PeripheralConfig.INSTALL, "ReplaceLocation()" + fileName + ":" + installLocation);
            try
            {
                string file = installLocation + "\\" + fileName;
                string text = System.IO.File.ReadAllText(file);
                installLocation = installLocation.Replace("\\", "\\\\");
                text = text.Replace("[INSTALL_LOCATION]", installLocation);
                System.IO.File.WriteAllText(file, text);
            }
            catch (Exception ex)
            {
                Log.Writer(PeripheralConfig.ERROR, "Failed to replace location: " + ex.Message);
            }
        }

        /// <summary>
        /// SetPermission
        /// </summary>
        /// <param name="file"></param>
        /// <param name="installDirectory"></param>
        private static void SetPermission(string file, string installDirectory)
        {
            Log.Writer(PeripheralConfig.INSTALL, "SetPermission()::Begin " + file + ":" + installDirectory);
            try
            {
                var processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = "\"" + installDirectory + "\\" + "configuresys.bat\"";
                string checkFile1 = installDirectory + "\\" + "configuresys.bat";
                string checkFile2 = installDirectory + "\\" + "registernxo.reg";
                if (System.IO.File.Exists(checkFile1) && System.IO.File.Exists(checkFile2))
                {
                    ReplaceLocation("registernxo.reg", installDirectory);
                }
                else
                {
                    return;
                }
                Log.Writer(PeripheralConfig.INSTALL, "File Name Httpcfg: *" + processStartInfo.FileName + "*");
                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.CreateNoWindow = true;
                Process proc = Process.Start(processStartInfo);
                Log.Writer(PeripheralConfig.INSTALL, "SetPermission()::End");
            }
            catch (Exception ex)
            {
                Log.Writer(PeripheralConfig.ERROR, "Failed: " + "ExecuteHttpcfg: Exception" + ex.Message);
            }
        }

        /// <summary>
        /// Install VcRedist
        /// </summary>
        /// <param name="session"></param>
        private static void InstallVcRedist(Session session)
        {
            string installDirectory = session["INSTALLLOCATION"];
            installDirectory = installDirectory.TrimEnd('\\');
            //Prerequisite.RegisterNXO(installDirectory);
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "VCRedistInstaller.exe";
            //processStartInfo.Verb = "runas";
            //processStartInfo.UseShellExecute = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.Arguments = "\"" + installDirectory + "\"";
            Process proc = Process.Start(processStartInfo);
        }    
    
    }
}
