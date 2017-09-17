using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.Fund.FirstView.Data
{
    public class FirstViewIdTypes : NexxoModel
    {
        public virtual int NexxoIdTypeId { get; set; }
        public virtual string IdCode { get; set; }
        public virtual string Name { get; set; }
        public virtual string CountryCode { get; set; }
        public virtual string StateCode { get; set; }
    }
}
