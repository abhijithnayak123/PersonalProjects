using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Models
{
    public class SystemExceptionMessage : SystemMessage
    {
        public string CName { get; set; }
        public string AName { get; set; }
    }
}