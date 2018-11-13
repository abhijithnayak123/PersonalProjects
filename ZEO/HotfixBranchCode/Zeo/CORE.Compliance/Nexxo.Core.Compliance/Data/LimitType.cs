using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Compliance.Data
{
	public class LimitType : NexxoModel
	{
		public virtual ComplianceProgram ComplianceProgram { get; set; }

		public virtual string Name { get; set; }

		public virtual ICollection<Limit> Limits { get; set; }

		public override string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str,"Name = ", Name, "\r\n");
			str = string.Concat(str, "Limits : ", "\r\n");
			str = string.Concat(str, Limits.ToString(), "\r\n");
			return str;
		}
	}
}
