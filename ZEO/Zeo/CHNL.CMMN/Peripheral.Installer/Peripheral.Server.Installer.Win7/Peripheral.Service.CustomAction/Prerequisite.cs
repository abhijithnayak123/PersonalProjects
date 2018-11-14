using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Net;
using System.Management;
using System.Diagnostics;
using com.epson.bank.driver;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Peripheral.Service.CustomAction
{
	public static class Prerequisite
	{        
        public static string GetRegistryValue()
		{
			string toReturn = null;
			string keyToOpen = @"HKEY_LOCAL_MACHINE\SOFTWARE\Nexxo\Nexxo Peripheral Service\PeripheralName";
			try
			{
				RegistryKey hive = Registry.LocalMachine;
				string hiveSTR = keyToOpen.Substring(0, keyToOpen.IndexOf("\\"));
				if (hiveSTR == "HKEY_CURRENT_USER") hive = Registry.CurrentUser;
				if (hiveSTR == "HKEY_LOCAL_MACHINE") hive = Registry.LocalMachine;
				keyToOpen = keyToOpen.Substring(keyToOpen.IndexOf("\\") + 1, keyToOpen.Length - keyToOpen.IndexOf("\\") - 1);
				string valueToOpen = keyToOpen.Substring(keyToOpen.LastIndexOf("\\") + 1, keyToOpen.Length - keyToOpen.LastIndexOf("\\") - 1);
				keyToOpen = keyToOpen.Substring(0, keyToOpen.LastIndexOf("\\"));
				RegistryKey key = hive.OpenSubKey(keyToOpen);
				toReturn = (string)key.GetValue(valueToOpen);
			}
			catch (Exception ex)
			{
                Log.Writer(PeripheralConfig.ERROR, "Get Registry exception" + ex.Message);
			}
			return toReturn;
		}

		public static string GetIPv6()
		{
			string hostName = System.Net.Dns.GetHostName();
			IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(hostName);
			IPAddress[] addr = ipEntry.AddressList;
			return addr[addr.Length - 1].ToString();
		}

		public static string GetIP()
		{
			string Ipv4address = string.Empty;
			try
			{

				foreach (var addr in Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList)
				{
					//AddressFamily.InterNetwork for IPv4 and AddressFamily.InterNetworkV6 for IPv6
					if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
					{
						Ipv4address = addr.ToString();
						break;
					}
				}
			}
			catch (Exception ex)
			{
                Log.Writer(PeripheralConfig.ERROR, "Get Ip exception" + ex.Message);
			}
			return Ipv4address;
		}

		public static bool GetWinInstaller()
		{
			bool winInstaller;
			try
			{
             	string winPath = System.Environment.GetEnvironmentVariable("WINDIR");
				winPath = winPath + "\\System32\\msi.dll";
				Version winInstalledVersion = new Version(System.Diagnostics.FileVersionInfo.GetVersionInfo(@winPath).ProductVersion);
				Version winRequiredVersion = new Version("3.1");
				winInstaller = winInstalledVersion >= winRequiredVersion;
			}
			catch (Exception ex)
			{
                Log.Writer(PeripheralConfig.ERROR, "GetWinInstaller Execption :" + ex.Message);
				winInstaller = false;
			}
			return winInstaller;
		}

		public static bool GetEpson()
		{
            bool isEspnAvailable = false;
			try
			{
				MFDevice epsonDevice = new MFDevice();
				epsonDevice.OpenMonPrinter(OpenType.TYPE_PRINTER, "TM-S9000U");
				ASB epsonResponse;
				epsonDevice.GetRealStatus(out epsonResponse);
				isEspnAvailable = (epsonResponse == ASB.ASB_NO_RESPONSE) ? false : true;
			}
			catch (Exception ex)
			{
                Log.Writer(PeripheralConfig.ERROR, "GetEpson Execption: Unable to communicate: " + ex.Message);
			}
			return isEspnAvailable;
		}

		public static bool GetSystemMemory()
		{
			double GBConversionFactor = Convert.ToDouble(1024 * 1024 * 1024);
			Microsoft.VisualBasic.Devices.ComputerInfo computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
			double totalMemoryInBytes = Convert.ToDouble(computerInfo.TotalPhysicalMemory);
			bool ramCheck = Math.Ceiling(totalMemoryInBytes / GBConversionFactor) >= 2 ? true : false;
			return ramCheck;
		}

		public static bool GetOSVersion()
		{
			int majorVersion = Environment.OSVersion.Version.Major;
            int minorVersion = Environment.OSVersion.Version.Minor;

            //OS should be Windows 7 & above for PS to install.
            if (!(majorVersion == 6 && minorVersion == 0)
                && majorVersion >= 6 && minorVersion >= 0) 
            {
                Log.Writer(PeripheralConfig.INSTALL, "Operating System is Windows 7 & above");
                return true;
            }
            else
            {
                Log.Writer(PeripheralConfig.INSTALL, "Operating System should be Windows and need to be Upgraded to Windows 7 & above");
                return false;
            }
		}

		public static bool IsProcessOpen(string applicationName)
		{
			foreach (Process clsProcess in Process.GetProcesses())
			{
				if (clsProcess.ProcessName.Contains(applicationName))
				{
					//if the process is found to be running then
					return true;
				}
			}
			return false;
		}

        public static bool IsEsponRollerInstalled()
        {     
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                if (printer.Equals("EPSON TM-S9000 Roll Paper"))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckPath(string Name)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + Name);
            if (key != null)
            {
                key.Close();
            }
            return key != null;
        }

       //Moved from PeripheralService 
       public static void RegisterNXO(string currentDir)
       {
           try
           {
               Log.Writer(PeripheralConfig.INSTALL, "RegisterNXO Step 1");
               RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes", RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.ChangePermissions | RegistryRights.ReadKey);//Get the registry key desired with ChangePermissions Rights.
               System.Security.AccessControl.RegistrySecurity rs = new RegistrySecurity();
               rs.AddAccessRule(new RegistryAccessRule("Administrator", RegistryRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));//Create access rule giving full control to the Administrator user.
               rk.SetAccessControl(rs); //Apply the new access rule to this Registry Key.

               Log.Writer(PeripheralConfig.INSTALL, "RegisterNXO Step 2");
               //Check if registry entry for .nxo association with openoffice print is present else create one
               RegistryKey regKeyCommand = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Print.nxo\shell\print\command", RegistryKeyPermissionCheck.ReadWriteSubTree);

               Log.Writer(PeripheralConfig.INSTALL, "RegisterNXO Step 3");
               if (regKeyCommand == null)
                   regKeyCommand = rk.CreateSubKey(@"Print.nxo\shell\print\command");
               //String currentDir = System.AppDomain.CurrentDomain.BaseDirectory;
               
               Log.Writer(PeripheralConfig.INSTALL, "RegisterNXO Step 4");
               String openOffice = "\"" + currentDir + @"\OpenOffice\program\soffice.exe" + "\" -p \"%1\"";
               if ((String)regKeyCommand.GetValue(null) != openOffice)
               {
                   Log.Writer(PeripheralConfig.INSTALL, "RegisterNXO Step 5");
                   regKeyCommand.SetValue(null, openOffice);
               }
               Log.Writer(PeripheralConfig.INSTALL, "RegisterNXO Step 6");
               RegistryKey regKeyNXO = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\.nxo\OpenWithProgIDs", RegistryKeyPermissionCheck.ReadWriteSubTree);
               if (regKeyNXO == null)
               {
                   Log.Writer(PeripheralConfig.INSTALL, "RegisterNXO Step 7");
                   regKeyNXO = rk.CreateSubKey(@".nxo\OpenWithProgIDs");
               }

               Log.Writer(PeripheralConfig.INSTALL, "RegisterNXO Step 8");
               if ((String)regKeyNXO.GetValue(null) != "Print.nxo")
               {
                   Log.Writer(PeripheralConfig.INSTALL, "RegisterNXO Step 9");
                   regKeyNXO.SetValue("Print.nxo", "");
               }

               RegistryKey regKeyNXOMid = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\.nxo", RegistryKeyPermissionCheck.ReadWriteSubTree);
               if ((String)regKeyNXOMid.GetValue(null) != "Print.nxo")
                   regKeyNXOMid.SetValue(null, "Print.nxo");
           }
           catch (Exception e)
           {
               Log.Writer(PeripheralConfig.ERROR, "RegisterNXO Execption: - " + e.Message);
           }
       }
	}
}