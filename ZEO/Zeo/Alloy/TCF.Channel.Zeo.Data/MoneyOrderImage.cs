using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Data
{
    public class MoneyOrderImage : ZeoModel
    {
        public long ImageID { get; set; }
        public long TransactionID { get; set; }
        public byte[] CheckFrontImage { get; set; }
        public byte[] CheckBackImage { get; set; }
    }
}
