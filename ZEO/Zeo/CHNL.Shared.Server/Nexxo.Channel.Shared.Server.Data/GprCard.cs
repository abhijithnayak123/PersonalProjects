using System;
using System.Runtime.Serialization;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class GprCard
	{
		public GprCard() { }

		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string CardNumber { get; set; }
		[DataMember]
		public string AccountNumber { get; set; }
		[DataMember]
		public decimal ActivationFee { get; set; }
		[DataMember]
		public decimal InitialLoadAmount { get; set; }
		[DataMember]
		public decimal LoadAmount { get; set; }
		[DataMember]
		public decimal LoadFee { get; set; }
		[DataMember]
		public decimal WithdrawAmount { get; set; }
		[DataMember]
		public decimal WithdrawFee { get; set; }
		[DataMember]
		public string Status { get; set; }
		[DataMember]
		public string StatusDescription { get; set; }
		[DataMember]
		public string StatusMessage { get; set; }
		[DataMember]
		public string ItemType { get; set; }
		[DataMember]
		public long TransactionId { get; set; }

		[DataMember]
		public decimal BaseFee { get; set; }
		[DataMember]
		public decimal NetFee { get; set; }
		[DataMember]
		public decimal DiscountApplied { get; set; }
		[DataMember]
		public string DiscountName { get; set; }
		[DataMember]
		public string AddOnCustomerName { get; set; }

		override public string ToString()
		{
			string str = String.Empty;
			str = String.Concat(str, "Id = ", Id, "\r\n");
			str = String.Concat(str, "CardNumber = ", NexxoUtil.cardLastFour(CardNumber), "\r\n");
			str = String.Concat(str, "AccountNumber = ", AccountNumber, "\r\n");
			str = String.Concat(str, "ActivationFee = ", ActivationFee, "\r\n");
			str = String.Concat(str, "InitialLoadAmount = ", InitialLoadAmount, "\r\n");
			str = String.Concat(str, "LoadAmount = ", LoadAmount, "\r\n");
			str = String.Concat(str, "LoadFee = ", LoadFee, "\r\n");
			str = String.Concat(str, "WithdrawAmount = ", WithdrawAmount, "\r\n");
			str = String.Concat(str, "WithdrawFee = ", WithdrawFee, "\r\n");
			str = String.Concat(str, "Status = ", Status, "\r\n");
			str = String.Concat(str, "StatusDescription = ", StatusDescription, "\r\n");
			str = String.Concat(str, "StatusMessage = ", StatusMessage, "\r\n");
			str = String.Concat(str, "ItemType = ", ItemType, "\r\n");
			str = String.Concat(str, "TransactionId = ", TransactionId, "\r\n");
			str = String.Concat(str, "AddOnCustomerName = ", AddOnCustomerName, "\r\n");
			return str;
		}
	}
}