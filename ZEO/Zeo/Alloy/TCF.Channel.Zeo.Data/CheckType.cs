using System.Runtime.Serialization;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Channel.Zeo.Data
{
	public class CheckType
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }
        [DataMember]
        public ProviderId ProductProviderCode { get; set; }

        override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "Name = ", Name, "\r\n");
            str = string.Concat(str, "ProductProviderCode = ", (int)ProductProviderCode, "\r\n");
            return str;
		}
	}
}
