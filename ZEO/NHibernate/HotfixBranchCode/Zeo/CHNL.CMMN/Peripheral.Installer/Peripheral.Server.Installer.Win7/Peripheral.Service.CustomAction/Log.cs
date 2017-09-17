using System;
using System.IO;
using System.Diagnostics;

namespace Peripheral.Service.CustomAction
{
	public static class Log
	{
		static string tempPath = Path.GetTempPath();

        /// <summary>
        /// Log Writer
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="msg"></param>
        public static void Writer(string logType, string msg)
        {
            StreamWriter logWriter = null;
            try
            {
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                if (msg == null) return;
                string file = tempPath + System.Net.Dns.GetHostName() + "-" + PeripheralConfig.PRODUCTVERSION + "-" + logType + ".log";
                if (!File.Exists(@file))
                {
                    logWriter = new StreamWriter(@file);
                }
                else
                {
                    logWriter = File.AppendText(@file);
                }
                logWriter.WriteLine("{0} {1}", DateTime.Now.ToString("yyyyMMddhhmmss"), msg);
                logWriter.Flush();
            }
            catch { }
            finally
            {
                if (logWriter != null)
                {
                    logWriter.Close();
                }
            }
        }

        /// <summary>
        /// File Copy 
        /// </summary>
        /// <param name="targetInstallFolder"></param>
        /// <param name="targetErrorFolder"></param>
        /// <param name="LogType"></param>
        public static void FileCopy(string targetInstallFolder, string targetErrorFolder, string LogType)
        {
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            string destinationLogPath = targetInstallFolder + "\\" + System.Net.Dns.GetHostName()  + "_" + PeripheralConfig.PRODUCTVERSION + "_" + LogType + "_" + currentTime + ".log";
            string currentInstallLogPath = tempPath + System.Net.Dns.GetHostName() + "-" + PeripheralConfig.PRODUCTVERSION + "-" + LogType + ".log";
            Log.Writer(LogType, "SourceLog::Path:: " + currentInstallLogPath);


            switch(LogType)
            {
                case PeripheralConfig.INSTALL:
                    if (File.Exists(currentInstallLogPath))
                    {
                        Log.Writer(LogType, "DestInstallLog::FileCopyPath:: Target Install Log Path= " + destinationLogPath);
                        File.Copy(currentInstallLogPath, destinationLogPath, true);
                    }
                    break;
                case PeripheralConfig.ERROR:

                    if (File.Exists(currentInstallLogPath))
                    {
                        Log.Writer(LogType, "DestErrorLog::Path::Target Erorr Log Path= " + destinationLogPath);
                        File.Copy(currentInstallLogPath, destinationLogPath, true);
                    }
                    break;
                case PeripheralConfig.UNINSTALL:
                    if (File.Exists(currentInstallLogPath))
                    {
                        Log.Writer(LogType, "DestUninstallLog::Path::Target Erorr Log Path= " + destinationLogPath);
                        File.Copy(currentInstallLogPath, destinationLogPath, true);
                    }
                break;
            }

        }
        
        /// <summary>
        /// Delete Log
        /// </summary>
        /// <param name="logName"></param>
        public static void Delete(string logName)
        {
            if (File.Exists(logName))
            {
                File.Delete(logName);
            }
        }

        public static void WriteEvent(string str)
        {
            //if (!EventLog.SourceExists("Nexxo Peripheral Installer"))
            //    EventLog.CreateEventSource("Nexxo Peripheral Installer", "Application");
            //EventLog.WriteEntry("Nexxo Peripheral Installer", str);
        }
	}
}
