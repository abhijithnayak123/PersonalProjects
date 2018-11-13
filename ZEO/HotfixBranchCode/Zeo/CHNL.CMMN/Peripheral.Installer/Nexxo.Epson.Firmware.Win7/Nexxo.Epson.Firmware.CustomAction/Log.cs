using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nexxo.Epson.Firmware.CustomAction
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

                string file = tempPath + System.Net.Dns.GetHostName() + "-" + logType + ".log";
                if (!File.Exists(@file))
                {
                    logWriter = new StreamWriter(@file);
                }
                else
                {
                    logWriter = File.AppendText(@file);
                }
                logWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), msg);
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
        /// Log File Copy
        /// </summary>
        /// <param name="targetInstallFolder"></param>
        public static void FileCopy(string targetInstallFolder, string LogType)
        {
            Log.Writer(LogType, "Log::FileCopy() Enter");
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            string destLog = targetInstallFolder + "\\" + System.Net.Dns.GetHostName() + "_" + LogType + "_" + currentTime + ".log";
            Log.Writer(LogType, "DestLog::FileCopyPath:: Target Path= " + destLog);

            string srcLogPath = tempPath + System.Net.Dns.GetHostName() + "-" + LogType + ".log";
            Log.Writer(LogType, "CurrentLog::FilePath:: Source Path = " + srcLogPath);

            if (File.Exists(srcLogPath))
            {
                File.Copy(srcLogPath, destLog, true);
            }
            else
            {
                Log.Writer(LogType, "Notfound::currentLogPath:::: " + destLog);
            }
            Log.Writer(LogType, "Log::FileCopy() Exit");
        }

        /// <summary>
        /// Log Delete
        /// </summary>
        /// <param name="logName"></param>
        public static void Delete(string logName)
        {
            if (File.Exists(logName))
            {
                File.Delete(logName);
            }
        }

    }
}
