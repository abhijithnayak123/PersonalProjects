using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class ValidateResponse
    {
        public long TransactionId { get; set; }
        public bool HasLPMTError { get; set; }
    }
}
