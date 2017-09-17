using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class UserLocation
    {
		public virtual Guid AgentLocationPK { get; set; }
        public virtual int AgentId { get; set; }
        public virtual long LocationId { get; set; } 
		public virtual Agent AgentPK { get; set; }
		public virtual Location LocationPK { get; set; }
    }
}
