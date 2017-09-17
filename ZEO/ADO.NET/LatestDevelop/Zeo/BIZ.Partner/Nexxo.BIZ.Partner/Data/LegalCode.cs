using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public class LegalCode
	{
		public Guid rowguid { get; set; }
		public long Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
        //public DateTime DTCreate { get; set; }
        //public DateTime? DTLastMod { get; set; }
        //public DateTime? DTServerCreate { get; set; }
        //public DateTime? DTServerLastMod { get; set; }
	}
}
