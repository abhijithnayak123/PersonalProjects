using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.Fund.FirstView.Data;

namespace MGI.Cxn.Fund.FirstView.Contract
{
	public interface IFirstViewProcessor
	{
        FirstViewTransaction GetTransactionMapping(long fundId);
	}
}
