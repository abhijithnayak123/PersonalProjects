using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class Field
	{
		[DataMember]
		public string Label { get; set; }
		[DataMember]
		public string DataType { get; set; }
		[DataMember]
		public string TagName { get; set; }
		[DataMember]
		public long MaxLength { get; set; }
		[DataMember]
		public string ValidationMessage { get; set; }
		[DataMember]
		public string RegularExpression { get; set; }
		[DataMember]
		public bool IsDynamic { get; set; }
		[DataMember]
		public bool IsMandatory { get; set; }
		[DataMember]
		public Dictionary<string, string> Values { get; set; }

		override public string ToString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Label = {0} \r\n", Label);
			stringBuilder.AppendFormat("DataType = {0} \r\n", DataType);
			stringBuilder.AppendFormat("TagName = {0} \r\n", TagName);
			stringBuilder.AppendFormat("MaxLength = {0} \r\n", MaxLength);
			stringBuilder.AppendFormat("ValidationMessage =	{0} \r\n", ValidationMessage);
			stringBuilder.AppendFormat("Mask = {0} \r\n", RegularExpression);
			stringBuilder.AppendFormat("IsDynamic = {0} \r\n", IsDynamic);
			stringBuilder.AppendFormat("IsMandatory = {0} \r\n", IsMandatory);

			if (Values != null)
			{
				foreach (KeyValuePair<string, string> value in Values)
				{
					stringBuilder.AppendFormat("{0} = {1} \r\n", value.Key, value.Value);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
