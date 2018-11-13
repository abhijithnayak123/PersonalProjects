using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class UserRole
    {
        public virtual int Id { get; set; }
        public virtual string role { get; set; }
		public virtual ICollection<UserRolePermission> RolesPermissions { get; set; }
    }
}
