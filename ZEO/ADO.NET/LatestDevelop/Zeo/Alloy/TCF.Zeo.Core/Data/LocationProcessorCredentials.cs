using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Data
{
    public class LocationProcessorCredentials : ZeoModel
    {
        public virtual long LocationId { get; set; }

        public virtual long ProviderId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual string Identifier { get; set; }
		
        public virtual string Identifier2 { get; set; }
    }
}
