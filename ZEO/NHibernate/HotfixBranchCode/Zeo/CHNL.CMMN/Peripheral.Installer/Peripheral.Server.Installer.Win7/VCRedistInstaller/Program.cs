using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace VCRedistInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            //Will Install VC++ 2008 Redist, once the installer exits
            String currentDir = String.Empty;
            if (args.Length > 0)
                currentDir = args[0];
            else
                currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
            while (WinGetHandle("Peripheral Service Installer Setup") != IntPtr.Zero)
                Thread.Sleep(1000);

            StartProcess(currentDir);
        }

        public static void StartProcess(string currentDir)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "\"" + currentDir + @"\vcredist_x86.exe" + "\"";
                Console.WriteLine("Executing " + startInfo.FileName);
                startInfo.UseShellExecute = true;
                //startInfo.Verb = "runas";
                //startInfo.Arguments = "/q";
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        public static IntPtr WinGetHandle(string wName)
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
                if (pList.MainWindowTitle.Contains(wName))
                    hWnd = pList.MainWindowHandle;
            return hWnd;
        }
    }
}
