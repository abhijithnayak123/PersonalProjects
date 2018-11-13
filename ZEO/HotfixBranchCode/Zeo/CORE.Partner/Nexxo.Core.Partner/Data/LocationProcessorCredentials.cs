using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class LocationProcessorCredentials : NexxoModel
    {
        public virtual Location Location { get; set; }

        public virtual long ProviderId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual string Identifier { get; set; }
    }
}
