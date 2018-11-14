using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public enum AuthenticationStatus
	{
		Authenticated = 1,
		TempPassword = 2,
		PasswordExpired = 3,
		Failed = 4,
		LockedOut = 5,
		Disabled = 6
	}
}
