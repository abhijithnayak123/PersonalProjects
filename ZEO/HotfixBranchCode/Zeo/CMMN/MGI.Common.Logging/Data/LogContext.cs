using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Common.Logging.Data
{
    public class LogContext : IPrincipal
    {
        public long AgentSessionId { get; set; }
        public long CustomerSessionId { get; set; }
        public string CommonFileName { get; set; }
        public string FileName {
            get
            {
                string fileName = CustomerSessionId == 0 ? AgentSessionId == 0 ? CommonFileName : "AgentSession." + AgentSessionId : "CustomerSession." + CustomerSessionId;
                return fileName;
            }
        }
        public string ApplicationName { get; set; }
        public string Version { get; set; }
        public string LogFolderPath { get; set; }

        public IIdentity Identity
        {
			get { return null; }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
