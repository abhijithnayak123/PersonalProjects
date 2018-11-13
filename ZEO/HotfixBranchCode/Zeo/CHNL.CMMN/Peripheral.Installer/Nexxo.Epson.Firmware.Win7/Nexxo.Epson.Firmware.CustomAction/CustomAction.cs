using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Diagnostics;

namespace Nexxo.Epson.Firmware.CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult EpsonFirmwareUpdate(Session session)
        {
            //System.Diagnostics.Debugger.Launch();
            WriteEvent("Updating firmware on device");
            EpsonFirmwareActivities epsonFirmwareActivities = new EpsonFirmwareActivities();
            epsonFirmwareActivities.LogDelete();
            if (epsonFirmwareActivities.FileCheck(session))
            {

                string installDirectory = session["INSTALLLOCATION"];
                installDirectory = installDirectory.TrimEnd('\\');
                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "Installation Directory " + " - " + installDirectory);
                var processStartInfo = new ProcessStartInfo();

                processStartInfo.FileName = "\"" + installDirectory + "\\EpsonUpdate.bat\"";
                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.CreateNoWindow = true;

                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "ProcessStartInfo::Starting - " + processStartInfo.FileName);
                WriteEvent("Executing firmware update on device");
                Process proc = Process.Start(processStartInfo);
                WriteEvent("Execution of firmware update on device completed");
                proc.WaitForExit();
                proc.Close();

                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "FileCopy::Checking Path::" + session["INSTALLLOGFOLDER"]);
                Log.FileCopy(session["INSTALLLOGFOLDER"], EpsonFirmwareConfig.EPSONFIRMWAREUPDATE);
            }
            else
            {
                WriteEvent("Failed to locate Config file NoxxConfig.txt");
            }

            return ActionResult.Failure;
        }

        public static void WriteEvent(string str)
        {
            if (!EventLog.SourceExists("Nexxo Epson Firmware Update"))
                EventLog.CreateEventSource("Nexxo Epson Firmware Update", "Application");
            EventLog.WriteEntry("Nexxo Epson Firmware Update", str);
        }
    }
}
