using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
	public class CashCheckPrintRequest : PrintRequest
	{
		[DataMember]
		public string LogoImage { get; set; }
		[DataMember]
		public string KioskName { get; set; }
		[DataMember]
		public string ChannelPartnerName { get; set; }
		[DataMember]
		public string Location { get; set; }
		[DataMember]
		public string Title { get; set; }
		[DataMember]
		public string TransactionIDValue { get; set; }
		[DataMember]
		public string TransactionDateValue { get; set; }
		[DataMember]
		public string CheckDateValue { get; set; }
		[DataMember]
		public string IssuerValue { get; set; }
		[DataMember]
		public string CheckAmountValue { get; set; }
		[DataMember]
		public string AmountPaidValue { get; set; }
		[DataMember]
		public string BalanceValue { get; set; }
		[DataMember]
		public string CardHolderValue { get; set; }
		[DataMember]
		public string NexxoCardBalanceValue { get; set; }
		[DataMember]
		public string CardNoValue { get; set; }
		[DataMember]
		public string CustomerService { get; set; }
		[DataMember]
		public string TollFreeNo { get; set; }
		[DataMember]
		public string ReceiptNoValue { get; set; }
		[DataMember]
		public string TotalAmount { get; set; }
		[DataMember]
		public string FeeAmount { get; set; }

	}
}
