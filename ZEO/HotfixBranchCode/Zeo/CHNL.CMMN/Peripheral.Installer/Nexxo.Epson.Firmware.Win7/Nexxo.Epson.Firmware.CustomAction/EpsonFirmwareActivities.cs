using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Deployment.WindowsInstaller;

namespace Nexxo.Epson.Firmware.CustomAction
{
    public class EpsonFirmwareActivities
    {
        private bool FilePathCheck(Session session)
        {
            bool fileCheck = false;
            string currentPath = session["SourceDir"] + "\\" + EpsonFirmwareConfig.FILENAME;
            string winDirPath = System.Environment.GetEnvironmentVariable("WINDIR") + "\\Temp\\" + EpsonFirmwareConfig.FILENAME;
            string systemPath = System.Environment.SystemDirectory + EpsonFirmwareConfig.FILENAME;

            if (File.Exists(currentPath))
            {
                session["SERVERPATH"] = currentPath;
                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "FilePath:CurrentPath:: " + currentPath);
                fileCheck = true;
            }
            else if (File.Exists(winDirPath))
            {
                session["SERVERPATH"] = winDirPath;
                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "FilePath:WinDirPath:: " + winDirPath);
                fileCheck = true;
            }
            else if (File.Exists(systemPath))
            {
                session["SERVERPATH"] = systemPath;
                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "FilePath:SystemPath:: " + systemPath);
                fileCheck = true;
            }
            else
            {
                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "NexxoConfig.txt File not found in current path, windows temp or system32");
            }

            return fileCheck;
        }

        public bool FileCheck(Session session)
        {
            bool fileCheck = false;

            if (FilePathCheck(session))
            {
                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "NexxoConfig.txt File found " + " - " + session["SERVERPATH"]);

                if (FileMode.Read(session))
                {
                    fileCheck = true;
                }
            }
            return fileCheck;
        }

        public void LogDelete()
        {
            Log.Delete(Path.GetTempPath() + System.Net.Dns.GetHostName() + "-" + EpsonFirmwareConfig.EPSONFIRMWAREUPDATE + ".log");
        }
    }
}
