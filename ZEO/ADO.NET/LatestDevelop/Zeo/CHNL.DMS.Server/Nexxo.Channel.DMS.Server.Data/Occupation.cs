using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class Occupation
	{
		[DataMember]
		public Guid rowguid { get; set; }
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public string Code { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public bool IsActive { get; set; }
        //[DataMember]
        //public DateTime DTCreate { get; set; }
        //[DataMember]
        //public DateTime? DTLastMod { get; set; }
		[DataMember]
		public DateTime? DTServerCreate { get; set; }
		[DataMember]
		public DateTime? DTServerLastMod { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "rowguid = ", rowguid, "\r\n");
			str = string.Concat(str, "Code = ", Code, "\r\n");
			str = string.Concat(str, "Name = ", Name, "\r\n");
            //str = string.Concat(str, "DTCreate = ", DTCreate, "\r\n");
            //str = string.Concat(str, "DTLastMod = ", DTLastMod, "\r\n");
			str = string.Concat(str, "DTServerCreate = ", DTServerCreate, "\r\n");
			str = string.Concat(str, "DTServerLastMod = ", DTServerLastMod, "\r\n");

			return str;
		}
	}
}
