using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class DeliveryService
	{
		[DataMember]
		public string Code { get; set; }
		[DataMember]
		public string Name { get; set; }

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("Code = {0} \r\n", Code);
			builder.AppendFormat("Name = {0} \r\n", Name);

			return builder.ToString();
		}
	}
}
