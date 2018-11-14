using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Cxn.Partner.TCF.Data
{
	public class CustomerTransactionDetails
	{
		public CustomerTransactionDetails()
		{
			Transactions = new List<Transaction>();
		}

		public Customer Customer { get; set; }
		public List<Transaction> Transactions { get; set; }

	}
}
