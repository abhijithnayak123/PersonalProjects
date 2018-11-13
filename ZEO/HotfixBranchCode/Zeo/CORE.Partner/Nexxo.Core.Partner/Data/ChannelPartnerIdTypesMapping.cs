using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class ChannelPartnerIdTypesMapping : NexxoModel
    {
        public virtual ChannelPartner ChannelPartner { get; set; }
        public virtual NexxoIdType NexxoIdType { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
