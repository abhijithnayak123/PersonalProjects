using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
	public class Message
	{
		public Guid rowguid { get; set; }
		public long Id { get; set; }
		public string MessageKey { get; set; }
		public MGI.Common.Util.NexxoUtil.Language Language { get; set; }
		public string Content { get; set; }
		public string AddlDetails { get; set; }
		public string Processor { get; set; }
		public long Partner { get; set; } 
		public int ErrorType { get; set; }

		public DateTime DTServerCreate { get; set; }
		public Nullable<DateTime> DTServerLastModified { get; set; }
	}
}
