using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
    public class State:NexxoModel
    {
        public virtual System.Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Abbr { get; set; }
        public virtual string CountryCode { get; set; }
        public virtual string Hasc1 { get; set; }
        public virtual string Region { get; set; }
        public virtual string Label { get; set; }
        public virtual string Fips { get; set; }
        public virtual System.Nullable<int> Xlicenseid { get; set; }
        public virtual string TimeZone { get; set; }
    }
}
