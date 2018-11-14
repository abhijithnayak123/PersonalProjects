using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Fund.Data
{
    public enum CardStatus
    {
        Suspended = 3,
        Lost = 5,
        Stolen = 6,
        Closed = 9,
        ClosedForFraud = 10,
        ReplaceCard = 14
    }
}
