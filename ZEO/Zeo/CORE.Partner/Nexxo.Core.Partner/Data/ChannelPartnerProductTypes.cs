using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class ChannelPartnerProductTypes
    {
        public virtual int Id { get; set; }

        public virtual int ChannelPartnerId { get; set; }

        public virtual int ProductTypeId { get; set; }

        public virtual DateTime DTServerCreate { get; set; }

        public virtual Nullable<DateTime> DTServerLastModified { get; set; }
    }   
}
