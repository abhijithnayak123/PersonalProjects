using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.IO;

namespace Peripheral.Service.CustomAction
{
    public class FileActivites
    {
        public bool ConfigFile(Session session)
        {
            bool configFileCheck = false;
            Log.Writer(PeripheralConfig.INSTALL, "Begin::ConfigFile::PrerequisiteCheck()::");
            if (FileCheck(session))
            {
                Log.Writer(PeripheralConfig.INSTALL, "End::ConfigFile::PrerequisiteCheck()::");
                Log.Writer(PeripheralConfig.INSTALL, "Begin::ReadFile::PrerequisiteCheck()::");
                if (ReadInputFile(session))
                {
                    Log.Writer(PeripheralConfig.INSTALL, "End::ReadFile::PrerequisiteCheck()::");
                    configFileCheck = true;
                }
            }
            return configFileCheck;
        }

        private bool FileCheck(Session session)
        {
            bool fileCheck = false;

            if (InputFileExists(session))
            {
                fileCheck = true;
            }
            else
            {
                //No inputfiles
                Log.WriteEvent("Missing configuration file.");
                fileCheck = false;
            }
            return fileCheck;
        }

        private bool ReadInputFile(Session session)
        {
            bool readFile = false;
            Log.Writer(PeripheralConfig.INSTALL, "Starting ReadInputFile()::");
            if (ReadFile(session))
            {
                Log.Writer(PeripheralConfig.INSTALL, "Ended ReadInputFile()::");
                readFile = true;
            }
            else
            {
                Log.Writer(PeripheralConfig.ERROR, "Failed: ReadInputFile()::");
                readFile = false;
            }
            return readFile;
        }

        private bool InputFileExists(Session session)
        {
            bool fileCheck = false;
            string fileInputMSI = String.Empty;
            try
            {
                fileInputMSI = session["SERVERPATH"];
            }
            catch (Exception e)
            {
                fileInputMSI = String.Empty;
            }

            if (fileInputMSI.Length > 0)
            {
                fileCheck = true;
            }
            else
            {
                if (FilePathCheck(session))
                {
                    fileCheck = true;
                }
            }

            return fileCheck;
        }        
        
        /// <summary>
        /// Read File
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private bool ReadFile(Session session)
        {
            bool readfile = false;
            try
            {
                if (!string.IsNullOrEmpty(session["SERVERPATH"]))
                {
                    int counter = 0;
                    string line;
                    // Read the file
                    System.IO.StreamReader files = new System.IO.StreamReader(session["SERVERPATH"]);
                    Log.Writer(PeripheralConfig.INSTALL, "Starts Reading File Lines from ServerPath:: " + session["SERVERPATH"]);
                    while ((line = files.ReadLine()) != null)
                    {
                        // split the line into a string array on space separator
                        string[] splitLine = line.ToString().Split(new char[] { ' ' }, 2);

                        // if split isnt null and has a positive length
                        if (splitLine != null && splitLine.Length > 0)
                        {
                            switch (splitLine[0].ToString())
                            {
                                case PeripheralConfig.CLIENT:
                                    FileConfig.ChannelPartnerID = splitLine[1].ToString();
                                    Log.Writer(PeripheralConfig.INSTALL, "Fetched ClientID: " + FileConfig.ChannelPartnerID);
                                    break;
                                case PeripheralConfig.INSTALLLOGFOLDER:
                                    FileConfig.InstallLogFolder = splitLine[1].ToString();
                                    Log.Writer(PeripheralConfig.INSTALL, "Fetched InstallLogFolder: " + FileConfig.InstallLogFolder);
                                    break;
                                case PeripheralConfig.ERRORLOGFOLDER:
                                    FileConfig.InstallErrorFolder = splitLine[1].ToString();
                                    Log.Writer(PeripheralConfig.INSTALL, "Fetched InstallErrorFolder: " + FileConfig.InstallErrorFolder);
                                    break;
                                case PeripheralConfig.SERVICEURL:
                                    FileConfig.ServiceURL = splitLine[1].ToString();
                                    Log.Writer(PeripheralConfig.INSTALL, "Fetched ServiceURL: " + FileConfig.ServiceURL);
                                    break;
                            }
                        }
                        counter++;
                    }
                    files.Close();
                    Log.Writer(PeripheralConfig.INSTALL, "Reading File Lines Ends");
                    readfile = true;
                }
            }
            catch (Exception ex)
            {
                Log.Writer(PeripheralConfig.ERROR, "Reading File Exception: " + ex.Message);
            }
            return readfile;
        }

        /// <summary>
        /// File Path Check
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private bool FilePathCheck(Session session)
        {
            string currentMSIPath = session["SourceDir"] + "\\" + PeripheralConfig.FILENAME;
            string winTempPath = System.Environment.GetEnvironmentVariable("WINDIR") + "\\Temp\\" + PeripheralConfig.FILENAME;
            string winSystemPath = System.Environment.SystemDirectory + "\\" + PeripheralConfig.FILENAME;

            bool filepathExist = false;
            if (File.Exists(currentMSIPath))
            {
                session["SERVERPATH"] = currentMSIPath;
                filepathExist = true;
                Log.Writer("Install", "FilePath:CurrentMSIPath:: " + currentMSIPath);
            }
            else if (File.Exists(winTempPath))
            {
                if (FileExists(winTempPath))
                {
                    session["SERVERPATH"] = winTempPath;
                    filepathExist = true;
                    Log.Writer("Install", "FilePath:WinTempPath:: " + winTempPath);
                }
            }
            else if (File.Exists(winSystemPath))
            {
                if (FileExists(winSystemPath))
                {
                    session["SERVERPATH"] = winSystemPath;
                    filepathExist = true;
                    Log.Writer("Install", "FilePath:WinSystemPath:: " + winSystemPath);
                }
            }
            else
            {
                Log.Writer(PeripheralConfig.ERROR, "Failed: " + System.Environment.SystemDirectory + " \\ " + PeripheralConfig.FILENAME + ", file not found or access is denied");
                filepathExist = false;
            }
            return filepathExist;
        }

        private static bool FileExists(string filePath)
        {
            Log.Writer(PeripheralConfig.INSTALL, "Start::FileExists() :" + filePath);
            // Get file info.
            FileInfo info = new FileInfo(filePath);
            bool exists = info.Exists;
            if (exists)
            {
                //Log file path available
                Log.Writer(PeripheralConfig.INSTALL, "FileExists()-FilePath: " + filePath + "available");
            }
            else
            {
                //Log file path not available
                Log.Writer(PeripheralConfig.ERROR, "FilePath: " + filePath + "file path is not available / access denied");
            }
            Log.Writer(PeripheralConfig.INSTALL, "End::FileExists()");
            return exists;
        }
    }
}
