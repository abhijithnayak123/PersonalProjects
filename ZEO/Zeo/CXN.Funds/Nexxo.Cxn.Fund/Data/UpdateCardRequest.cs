using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.Data
{
	public class UpdateCardRequest
	{
		public long AliasId { get; set; }
		public string CardStatus { get; set; }
	}
}
