using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class Product : NexxoModel
    {
        public virtual string Name { get; set; }
    }
}
