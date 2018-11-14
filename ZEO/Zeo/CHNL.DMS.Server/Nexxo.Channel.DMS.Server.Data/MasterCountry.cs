using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class MasterCountry
	{
		[DataMember]
		public virtual string Name { get; set; }
		[DataMember]
		public virtual string Abbr2 { get; set; }
		[DataMember]
		public virtual string Abbr3 { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Name = ", Name, "\r\n");
			str = string.Concat(str, "Abbr2 = ", Abbr2, "\r\n");
			str = string.Concat(str, "Abbr3 = ", Abbr3, "\r\n");			

			return str;
		}
	}
}
