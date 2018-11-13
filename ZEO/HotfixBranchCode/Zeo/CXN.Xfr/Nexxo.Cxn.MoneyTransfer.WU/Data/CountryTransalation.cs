
using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
    public class CountryTransalation : NexxoModel
    {
        public virtual string CountryCode { get; set; }
        public virtual string Language { get; set; }
        public virtual string Name { get; set; }
    }
}
