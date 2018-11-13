using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Check.Data
{
	public enum CheckType
	{
        Cashier = 1,
        GovtUSTreasury = 2,
        GovtUSOther = 3,
        MoneyOrder = 5,
        PayrollHandwritten = 6,
        PayrollPrinted = 7,
        TwoParty = 10,
        LoanRAL = 14
	}
}
