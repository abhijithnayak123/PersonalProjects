using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public enum ChangePasswordStatus
	{
		PasswordSuccessfullyChanged = 1,
		OldPasswordMismatch = 2,
		PasswordRequirementsNotMet = 3
	}
}
