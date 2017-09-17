using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peripheral.Service.CustomAction
{
    public static class FileConfig
    {
        public static string ChannelPartnerID { get; set; }
        public static string InstallLogFolder { get; set; }
        public static string InstallErrorFolder { get; set; }
        public static string ServiceURL { get; set; }
    }
}
