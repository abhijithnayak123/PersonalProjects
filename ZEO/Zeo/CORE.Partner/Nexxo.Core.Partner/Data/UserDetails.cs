using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class UserDetails : NexxoModel
    {
        public virtual System.Guid Rowguid { get; set; }
        public virtual string UserName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
        public virtual bool IsEnabled { get; set; }
        public virtual System.Nullable<int> ManagerId { get; set; }        
        public virtual long LocationId { get; set; }
        public virtual int UserRoleId { get; set; }
        public virtual int UserStatusId { get; set; }
        public virtual long ChannelPartnerId { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Email { get; set; }
        public virtual string Notes { get; set; }
        public virtual DateTime? DtLastlogin { get; set; }
		public virtual string UserStatus { get; set; }
		public virtual string LocationName { get; set; }
        public virtual string ClientAgentIdentifier { get; set; }
    }          
}
    