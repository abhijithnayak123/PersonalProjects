using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public enum PTNRCheckStatus
	{
		Unknown = 0,
		Pending = 1,
		Approved = 2,
		Declined = 10,
		Cashed = 11,
		Canceled = 12,
		Failed = 13
	}
}
