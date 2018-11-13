using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class DownloaderModel : BaseModel
    {
        public List<string> Installers { get; set; }
        public List<string> Upgrades { get; set; }
        public List<string> Documents { get; set; }
    }
}