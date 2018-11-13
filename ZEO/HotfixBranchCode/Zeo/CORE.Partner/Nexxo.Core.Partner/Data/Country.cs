using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
    public class Country:NexxoModel
    {
        public virtual System.Guid Id { get; set; }
        public virtual int Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Abbr2 { get; set; }
        public virtual string Abbr3 { get; set; }
        public virtual System.Nullable<decimal> XRate { get; set; }
        public virtual System.Nullable<System.DateTime> Dtrateasof { get; set; }
        public virtual string CurrencyName { get; set; }
        public virtual string CurrCdAlpha { get; set; }
        public virtual System.Nullable<int> CurrCdNum { get; set; }
        public virtual string Symbol { get; set; }
        public virtual System.Nullable<int> CallingCode { get; set; }
        public virtual System.Nullable<bool> IsCOB { get; set; }
        public virtual System.Nullable<bool> DebitAllowed { get; set; }
    }
}
