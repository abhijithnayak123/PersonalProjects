using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Deployment.WindowsInstaller;

namespace Peripheral.Service.CustomAction
{
    public class CustomActivites
    {
        public bool GetFile(Session session)
        {
            FileActivites fileActivites = new FileActivites();
            return fileActivites.ConfigFile(session);
        }

        public void DeleteFile()
        {
            Log.Delete(Path.GetTempPath() + System.Net.Dns.GetHostName() + "-" + PeripheralConfig.PRODUCTVERSION + "-" + PeripheralConfig.INSTALLLOG);
            Log.Delete(Path.GetTempPath() + System.Net.Dns.GetHostName() + "-" + PeripheralConfig.PRODUCTVERSION + "-" + PeripheralConfig.ERRORLOG);
            Log.Delete(Path.GetTempPath() + System.Net.Dns.GetHostName() + "-" + PeripheralConfig.PRODUCTVERSION + "-" + PeripheralConfig.UNINSTALLLOG);
        }       

        public bool CheckPrerequistes()
        {
            PrerequisitesActions prerequisitesActions = new PrerequisitesActions();
            return prerequisitesActions.Prerequistes();
        }

        public void SaveNPSDetails()
        {
            NPSActivites npsActivites = new NPSActivites();
            npsActivites.CreateTerminal();
        }

        public void BatchExecuteDetails(Session session)
        {
          BatchActivites batchActivites = new BatchActivites();
          batchActivites.BatchExecute(session);
        }       

    }
}
