using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Compliance.Data
{
	public class ComplianceProgram : NexxoModel
	{
		public virtual string Name { get; set; }
		public virtual bool RunOFAC { get; set; }

		public virtual ICollection<LimitType> LimitTypes { get; set; }

		public override string ToString()
		{
			string str = string.Empty;
			str = string.Concat("\r\n");
			str = string.Concat(str, "Name = ", Name, "\r\n");
			str = string.Concat(str, "RunOFAC = ", RunOFAC, "\r\n");
			str = string.Concat(str, "LimitTypes :", "\r\n");
			str = string.Concat(str, LimitTypes.ToString());
			return str;
		}
	}
}
