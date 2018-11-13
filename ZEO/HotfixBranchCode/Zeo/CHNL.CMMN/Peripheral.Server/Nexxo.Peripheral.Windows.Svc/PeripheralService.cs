using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using Microsoft.Win32;
//using MGI.Peripheral.Server.Data;

namespace MGI.Peripheral.Windows.Service
{
    partial class PeripheralService : ServiceBase
    {
        internal static ServiceHost myServiceHost = null; 

        public PeripheralService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (myServiceHost != null)
            {
                myServiceHost.Close();
            }

            myServiceHost = new ServiceHost(typeof(MGI.Peripheral.Server.Impl.PeripheralServiceImpl));

            myServiceHost.Open();

            new MGI.Peripheral.Server.Impl.PeripheralServiceImpl().PrinterDiagnostics("startup");

        }

        protected override void OnStop()
        {
            if (myServiceHost != null)
            {
                myServiceHost.Close();
                myServiceHost = null;
            }
        }
    }
}
