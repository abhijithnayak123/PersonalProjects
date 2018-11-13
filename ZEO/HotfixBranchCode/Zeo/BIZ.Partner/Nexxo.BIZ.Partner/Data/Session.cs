using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public class Session
	{
		public long Id { get; set; }
		public Guid rowguid { get; set; }

		public int AgentId { get; set; }
		public Terminal Terminal { get; set; }

        //public DateTime DTCreate { get; set; }
        //public DateTime? DTLastMod { get; set; }
	}
}
