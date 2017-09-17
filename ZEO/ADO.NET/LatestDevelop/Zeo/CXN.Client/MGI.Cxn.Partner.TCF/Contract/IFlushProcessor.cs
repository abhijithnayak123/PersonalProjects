using MGI.Common.Util;
using MGI.Cxn.Partner.TCF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Partner.TCF.Contract
{
	public interface IFlushProcessor
	{
		void PreFlush(CustomerTransactionDetails cart, MGIContext mgiContext);

		void PostFlush(CustomerTransactionDetails cart, MGIContext mgiContext);
	}
}
