using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Deployment.WindowsInstaller;

namespace Nexxo.Wrapper.Epson.CustomAction
{
    public class EpsonActivities
    {
        private bool FilePathCheck(Session session)
        {
            bool fileCheck = false;
            string currentPath = session["SourceDir"] + "\\" + EpsonConfig.FILENAME;
            string winDirPath = System.Environment.GetEnvironmentVariable("WINDIR") + "\\Temp\\" + EpsonConfig.FILENAME;
            string systemPath = System.Environment.SystemDirectory + EpsonConfig.FILENAME;
            
            if (File.Exists(currentPath))
            {
                session["SERVERPATH"] = currentPath;
                Log.Writer(EpsonConfig.EPSONINSTALL, "FilePath:CurrentPath:: " + currentPath);
                fileCheck = true;
            }
            else if (File.Exists(winDirPath))
            {
                session["SERVERPATH"] = winDirPath;
                Log.Writer(EpsonConfig.EPSONINSTALL, "FilePath:WinDirPath:: " + winDirPath);
                fileCheck = true;
            }
            else if (File.Exists(systemPath))
            {
                session["SERVERPATH"] = systemPath;
                Log.Writer(EpsonConfig.EPSONINSTALL, "FilePath:SystemPath:: " + systemPath);
                fileCheck = true;
            }
            else
            {
                Log.Writer(EpsonConfig.EPSONINSTALL, "NexxoConfig.txt File not found in current path, windows temp or system32");
            }

            return fileCheck; 
        }

        public bool FileCheck(Session session)
        {
            bool fileCheck = false;
            
            if(FilePathCheck(session))
            {
                Log.Writer(EpsonConfig.EPSONINSTALL, "NexxoConfig.txt File found " + " - " + session["SERVERPATH"]);

                if (FileMode.Read(session))
                {
                    fileCheck = true;
                }
            }
            return fileCheck;
        }

        public void LogDelete()
        {
            Log.Delete(Path.GetTempPath() + System.Net.Dns.GetHostName() + "-" + EpsonConfig.EPSONINSTALL);
            Log.Delete(Path.GetTempPath() + System.Net.Dns.GetHostName() + "-" + EpsonConfig.EPSONUNINSTALL);
 
        }
    }
}
