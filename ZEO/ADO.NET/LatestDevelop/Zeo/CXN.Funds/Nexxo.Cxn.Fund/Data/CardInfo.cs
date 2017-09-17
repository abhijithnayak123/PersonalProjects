using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.Data
{
	public class CardInfo
	{
		public decimal Balance { get; set; }
		public string CardStatus { get; set; }
		public DateTime? ClosureDate { get; set; }
		public Dictionary<string, object> MetaData { get; set; }
	}
}
