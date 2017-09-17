using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class UserRolePermission : NexxoModel
    {
        public virtual Permission Permission { get; set; }
		public virtual UserRole Role { get; set; }
		public virtual ChannelPartner ChannelPartner { get; set; }
        public virtual bool IsEnabled { get; set; }			
    }
}
