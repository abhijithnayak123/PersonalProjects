using MGI.Common.Util;
using MGI.Cxn.Partner.TCF.Contract;
//using MGI.Cxn.Partner.TCF.RCIFService;
using MGI.Cxn.Partner.TCF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MGI.Cxn.Partner.TCF.Impl
{
	public class Gateway : IFlushProcessor
	{
		//ZeoCustomerService client = new ZeoCustomerService();

		public IO IO { private get; set; }

		public void PreFlush(CustomerTransactionDetails cart, MGIContext mgiContext)
		{
			IO.PreFlush(cart, mgiContext);
		}

		public void PostFlush(CustomerTransactionDetails cart, MGIContext mgiContext)
		{
			IO.PostFlush(cart, mgiContext);
		}

	}
}
