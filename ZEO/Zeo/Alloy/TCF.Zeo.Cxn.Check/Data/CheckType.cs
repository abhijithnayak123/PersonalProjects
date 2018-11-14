using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Cxn.Check.Data
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
        LoanRAL = 14,
        OnUsOCMO = 15,
        OnUsTrue = 16,
        OnUsOther = 17,
        OnUSCashier = 18,
        OnUSTreasury = 19,
        OnUSGovernment = 20,
        OnUSMoneyOrder = 21,
        OnUSHandwrittenPayroll = 22,
        OnUSPrintedPayroll = 23,
        OnUSTwoParty = 24,
        OnUSLoanRAL = 25
    }
}
