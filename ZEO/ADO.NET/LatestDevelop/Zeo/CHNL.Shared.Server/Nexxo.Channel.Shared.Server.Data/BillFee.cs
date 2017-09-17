using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class BillFee
	{
		[DataMember]
		public long TransactionId { get; set; }
		[DataMember]
		public string SessionCookie { get; set; }
		[DataMember]
		public string AccountHolderName { get; set; }
		[DataMember]
		public string AvailableBalance { get; set; }
		[DataMember]
		public List<DeliveryMethod> DeliveryMethods { get; set; }
		[DataMember]
		public string CityCode { get; set; }
        
		override public string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("{0} = \r\n", TransactionId);
			stringBuilder.AppendFormat("{0} = \r\n", SessionCookie);
			stringBuilder.AppendFormat("{0} = \r\n", AccountHolderName);
			stringBuilder.AppendFormat("{0} = \r\n", AvailableBalance);
			stringBuilder.AppendFormat("{0} = \r\n", CityCode);

			if (DeliveryMethods != null)
			{
				for (int index = 0; index < DeliveryMethods.Count; index++)
				{
                    stringBuilder.AppendFormat("Code:{0} Text:{1} Fee:{2} Tax : {3} \r\n", DeliveryMethods[index].Code, DeliveryMethods[index].Text, DeliveryMethods[index].FeeAmount, DeliveryMethods[index].Tax);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
