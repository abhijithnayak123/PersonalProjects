using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using MGI.Common.DataAccess.Data;
using MGI.Common.Util;

namespace MGI.Core.Compliance.Data
{
	public class Limit : NexxoModel
	{
		
		public virtual LimitType LimitType { get; set; }

		public virtual Nullable<decimal> PerTransactionMaximum { get; set; }
		public virtual Nullable<decimal> PerTransactionMinimum { get; set; }
		public virtual string RollingLimits { get; set; }
		public virtual Dictionary<int, decimal> NDaysLimit { get { return GetLimits(); } }

		public virtual Dictionary<int, decimal> GetLimits()
		{
			Dictionary<int, decimal> _nDaysLimits = new Dictionary<int, decimal>();
			if (!String.IsNullOrEmpty(RollingLimits))
			{
				_nDaysLimits = RollingLimits.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
											.Select(part => part.Split(':')).ToList()
											.ToDictionary(split => Int32.Parse(split[0]), split => decimal.Parse(split[1]));
			}

			return _nDaysLimits;
		}

		public override string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "PerTransactionMaximum = ", PerTransactionMaximum, "\r\n");
			str = string.Concat(str, "PerTransactionMinimum = ", PerTransactionMinimum, "\r\n");
			str = string.Concat(str, "RollingLimits = ", RollingLimits, "\r\n");
			return str;
		}
	}
}
