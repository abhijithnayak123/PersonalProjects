using System;

namespace Reporting.X9
{
	class Item
	{
		public Guid ItemId;
		public DateTime BusinessDate;
		public string BranchID;
		public string BankID;
		public string AgentID;
		public Int64 NexxoPAN;
		public decimal Amount;
		public byte[] FrontImage;
		public byte[] RearImage;
	}
}
