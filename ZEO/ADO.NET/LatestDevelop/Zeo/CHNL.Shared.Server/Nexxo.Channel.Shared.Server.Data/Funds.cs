using System.Runtime.Serialization;
namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class Funds
    {
        public Funds() { }
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public decimal Fee { get; set; }
		[DataMember]
		public string PromoCode { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Amount = ", Amount, "\r\n");
			str = string.Concat(str, "Fee = ", Fee, "\r\n");
			str = string.Concat(str, "Fee = ", PromoCode, "\r\n");
			return str;
		}
	}
}
