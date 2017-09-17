using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class Permission : NexxoModel
    {				
        public virtual string PermissionName { get; set; }
		public virtual ICollection<UserRolePermission> PermissionsRoles { get; set; }
    }
}
