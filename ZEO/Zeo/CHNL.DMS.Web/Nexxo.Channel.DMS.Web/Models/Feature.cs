using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Models
{
    public class Feature
    {
        public string FeatureName { get; set; }
        public int FeatureId {get; set;}
        public bool IsEnabled { get; set; }
    }
}