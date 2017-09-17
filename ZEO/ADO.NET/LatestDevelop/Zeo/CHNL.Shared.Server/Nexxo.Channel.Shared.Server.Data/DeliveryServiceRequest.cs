using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class DeliveryServiceRequest
	{
		[DataMember]
		public DeliveryServiceType Type { get; set; }
		[DataMember]
		public string CountryCode { get; set; }
		[DataMember]
		public string CountryCurrency { get; set; }
		[DataMember]
		public Dictionary<string, object> MetaData { get; set; }

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("DeliveryServiceType = {0} \r\n", Type);
			builder.AppendFormat("CountryCode = {0} \r\n", CountryCode);
			builder.AppendFormat("CountryCurrency = {0} \r\n", CountryCurrency);

			if (MetaData != null)
			{
				foreach (KeyValuePair<string, object> data in MetaData)
				{
					builder.AppendFormat("{0} = {1} \r\n", data.Key, data.Value);
				}
			}

			return builder.ToString();
		}

	}
}
