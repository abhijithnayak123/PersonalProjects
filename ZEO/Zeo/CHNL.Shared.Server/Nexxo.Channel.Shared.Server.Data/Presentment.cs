using System.Runtime.Serialization;
namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class Presentment
	{
		[DataMember]
		public int ProcessorID { get; set; }
		[DataMember]
		public long ProductID { get; set; }
		[DataMember]
		public string BillerName { get; set; }
		[DataMember]
		public System.Nullable<decimal> Fee { get; set; }
		[DataMember]
		public System.Nullable<decimal> MinimumLoad { get; set; }
		[DataMember]
		public System.Nullable<decimal> MaximumLoad { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "ProcessorID = ", ProcessorID, "\r\n");
			str = string.Concat(str, "ProductID = ", ProductID, "\r\n");
			str = string.Concat(str, "BillerName = ", BillerName, "\r\n");
			str = string.Concat(str, "Fee = ", Fee, "\r\n");
			str = string.Concat(str, "MinimumLoad = ", MinimumLoad, "\r\n");
			str = string.Concat(str, "MaximumLoad = ", MaximumLoad, "\r\n");
			return str;
		}
	}
}
