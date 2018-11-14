using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Check.Data
{
	public class CheckTrx
	{
		public long Id;
		public decimal Amount;
		public string CheckNumber;
		public decimal ReturnAmount;
		public decimal ReturnFee;
		public string WaitTime;
		public CheckStatus Status;
		public CheckType SubmitType;
		public CheckType ReturnType;
		public string DeclineCode;
		public string DeclineMessage;
		public string ConfirmationNumber;
		public int TicketId;
		public bool IsCheckFranked;
		public Dictionary<string, object> MetaData { get; set; }
	}
}
