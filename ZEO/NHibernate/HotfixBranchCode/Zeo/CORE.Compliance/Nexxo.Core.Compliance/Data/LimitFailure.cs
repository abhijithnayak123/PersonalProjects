using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Compliance.Data
{
	public class LimitFailure : NexxoModel
	{
		public virtual long CustomerSessionId { get; set; }
		public virtual int TransactionType { get; set; }
		public virtual decimal TransactionAmount { get; set; }
		public virtual decimal LimitAmount { get; set; }
		public virtual string ComplianceProgramName { get; set; }
		public virtual ComplianceProgram ComplianceProgram { get; set; }
	}
}
