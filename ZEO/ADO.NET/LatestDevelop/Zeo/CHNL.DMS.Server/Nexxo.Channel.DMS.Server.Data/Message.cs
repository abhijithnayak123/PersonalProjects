using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class Message
	{
		[DataMember]
		public Guid rowguid { get; set; }
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public string MessageKey { get; set; }
		[DataMember]
		public MGI.Common.Util.NexxoUtil.Language Language { get; set; }
		[DataMember]
		public string Content { get; set; }
		[DataMember]
		public string AddlDetails { get; set; }
		[DataMember]
		public string Processor { get; set; }
		[DataMember]
		public long Partner { get; set; }
		[DataMember]
		public int ErrorType { get; set; }
		[DataMember]
		public DateTime DTServerCreate { get; set; }
		[DataMember]
		public Nullable<DateTime> DTServerLastModified { get; set; }
	}
}
