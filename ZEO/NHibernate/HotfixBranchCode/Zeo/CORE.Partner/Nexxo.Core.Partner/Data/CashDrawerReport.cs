using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
    public class CashDrawerReport
	{
        public virtual Guid rowguid { get; set; }
        public virtual int AgentId { get; set; }
        public virtual int CashType { get; set; }
        public virtual long LocationId { get; set; }
        public virtual string Location { get; set; }
        public virtual decimal Amount { get; set; }
	}
}
