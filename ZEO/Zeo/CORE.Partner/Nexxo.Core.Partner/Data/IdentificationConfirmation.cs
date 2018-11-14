using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class IdentificationConfirmation
    {
        public virtual Guid rowguid { get; set; }
        public virtual long Id { get; set; }
        public virtual long AgentID { get; set; }
        public virtual long CustomerSessionID { get; set; }
        public virtual bool ConfirmStatus { get; set; }
        public virtual DateTime DateIdentified { get; set; }
        public virtual DateTime DTServerCreate { get; set; }
        public virtual DateTime DTServerLastModified { get; set; }
    }
}
