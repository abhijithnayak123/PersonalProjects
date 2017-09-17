using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Data
{
    public class WUCountryCurrency : ZeoModel
    {
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CountryName { get; set; }
        public string CountryNumCode { get; set; }
        public string CurrencyNumCode { get; set; }
        public string CurrencyName { get; set; }
    }
}
