
using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
    public class DeliveryServiceTransalation : NexxoModel
    {
        public virtual string EnglishName { get; set; }
        public virtual string Name { get; set; }
        public virtual string Language { get; set; }
    }
}
