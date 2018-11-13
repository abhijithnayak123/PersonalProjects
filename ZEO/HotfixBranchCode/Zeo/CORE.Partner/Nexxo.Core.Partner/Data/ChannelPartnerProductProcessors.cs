using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class ChannelPartnerProductProcessors
    {
        public virtual int Id { get; set; }

        public virtual int ChannelPartnerProductTypeId { get; set; }

        public virtual string Processor { get; set; }
        
        public virtual DateTime DTServerCreate { get; set; }

        public virtual Nullable<DateTime> DTServerLastModified { get; set; }
    }
}
