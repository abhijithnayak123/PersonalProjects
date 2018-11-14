using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;

namespace Nexxo.Epson.Firmware.CustomAction
{
    public static class FileMode
    {
        public static bool Read(Session session)
        {
            bool readfile = false;
            string filePath = session["SERVERPATH"];
            try
            {
                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "Reading Server Config()::" + filePath);
                if (!string.IsNullOrEmpty(filePath))
                {
                    int counter = 0;
                    string line;
                    // Read the file
                    System.IO.StreamReader files = new System.IO.StreamReader(filePath);
                    Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "Reading File Lines Starting");
                    while ((line = files.ReadLine()) != null)
                    {
                        // split the line into a string array on space separator
                        string[] splitLine = line.ToString().Split(new char[] { ' ' }, 2);

                        // if split isnt null and has a positive length
                        if (splitLine != null && splitLine.Length > 0)
                        {
                            if (splitLine[0].ToString().Equals("InstallLogFolder"))
                            {
                                session["INSTALLLOGFOLDER"] = splitLine[1].ToString();
                                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "Fetched InstallLogFolder: " + session["INSTALLLOGFOLDER"]);
                                break;
                            }
                        }
                        counter++;
                    }
                    files.Close();
                    Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "Reading File Lines Ends");
                    readfile = true;
                }
            }
            catch (Exception ex)
            {
                Log.Writer(EpsonFirmwareConfig.EPSONFIRMWAREUPDATE, "ReadFile()::Exception " + filePath + " " + ex.Message);
            }
            return readfile;
        }
    }
}
