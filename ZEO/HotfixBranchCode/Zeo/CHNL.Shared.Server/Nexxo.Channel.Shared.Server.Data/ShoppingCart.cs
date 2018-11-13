using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class ShoppingCart
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public long AlloyID { get; set; }
		[DataMember]
		public long CustomerSessionId { get; set; }
		[DataMember]
		public string AgentSessionId { get; set; }
		[DataMember]
		public string KioskId { get; set; }
		[DataMember]
		public String AppId { get; set; }
		[DataMember]
		public decimal CheckTotal { get; set; }
		[DataMember]
		public decimal BillTotal { get; set; }
		[DataMember]
		public List<Check> Checks { get; set; }
		[DataMember]
		public List<Bill> Bills { get; set; }
		[DataMember]
		public List<MoneyTransfer> MoneyTransfers { get; set; }
		[DataMember]
		public List<GprCard> GprCards { get; set; }
		[DataMember]
		public List<CartCash> Cash { get; set; }
		[DataMember]
		public decimal MoneyTransfeTotal { get; set; }
		[DataMember]
		public decimal GprCardTotal { get; set; }
		[DataMember]
		public decimal CashInTotal { get; set; }
		[DataMember]
		public decimal GenerateAmount { get; set; }
		[DataMember]
		public decimal GenerateFee { get; set; }
		[DataMember]
		public decimal GenerateTotal { get; set; }
		[DataMember]
		public decimal DepletingAmount { get; set; }
		[DataMember]
		public decimal DepletingFee { get; set; }
		[DataMember]
		public decimal DepletingTotal { get; set; }
		[DataMember]
		public decimal SubTotalFee { get; set; }
		[DataMember]
		public decimal TotalDueToCustomer { get; set; }

        [DataMember]
        public List<MoneyOrder> MoneyOrders { get; set; }
        [DataMember]
        public decimal MoneyOrderTotal { get; set; }
		//US1800 Referral promotions – Free check cashing to referrer and referee 
		[DataMember]
		public bool IsReferral { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			//str = string.Concat(str, "AlloyID = ", AlloyID, "\r\n");
			str = string.Concat(str, "AlloyID = ", AlloyID.ToString().Substring(0, 6) + "XXXXXX" + AlloyID.ToString().Substring(AlloyID.ToString().Length - 4, 4), "\r\n");
			str = string.Concat(str, "CustomerSessionId = ", CustomerSessionId, "\r\n");
			str = string.Concat(str, "AgentSessionId = ", AgentSessionId, "\r\n");
			str = string.Concat(str, "KioskId = ", KioskId, "\r\n");
			str = string.Concat(str, "AppId = ", AppId, "\r\n");
			str = string.Concat(str, "CheckTotal = ", CheckTotal, "\r\n");
			str = string.Concat(str, "BillTotal = ", BillTotal, "\r\n");
			if (Checks != null)
			{
				str = string.Concat(str, "Checks = ", "\r\n");
				foreach (Check check in Checks)
				{
					str = string.Concat(str, "Check = ", check, "\r\n");
				}
			}
			if (Bills != null)
			{
				str = string.Concat(str, "Bills = ", "\r\n");
				foreach (Bill bill in Bills)
				{
					str = string.Concat(str, "Bill = ", bill, "\r\n");
				} 
			}
			if (MoneyTransfers != null)
			{
				str = string.Concat(str, "MoneyTransfers = ", "\r\n");
				foreach (MoneyTransfer moneyTransfer in MoneyTransfers)
				{
					str = string.Concat(str, "MoneyTransfer = ", moneyTransfer, "\r\n");
				} 
			}
			if (GprCards != null)
			{
				str = string.Concat(str, "GprCards = ", "\r\n");
				foreach (GprCard gprCard in GprCards)
				{
					str = string.Concat(str, "Gpr Card = ", gprCard, "\r\n");
				} 
			}
            if (MoneyOrders != null)
            {
                str = string.Concat(str, "MoneyOrders  = ", "\r\n");
                foreach (MoneyOrder moneyOrder in MoneyOrders)
                {
                    str = string.Concat(str, "MoneyOrder = ", moneyOrder, "\r\n");
                }
            }
            str = string.Concat(str, " MoneyOrderTotal = ", MoneyOrderTotal, "\r\n");

			str = string.Concat(str, "Cash = ", Cash, "\r\n");
			str = string.Concat(str, "MoneyTransfeTotal = ", MoneyTransfeTotal, "\r\n");
			str = string.Concat(str, "GprCardTotal = ", GprCardTotal, "\r\n");
			str = string.Concat(str, "CashInTotal = ", CashInTotal, "\r\n");
			str = string.Concat(str, "GenerateAmount = ", GenerateAmount, "\r\n");
			str = string.Concat(str, "GenerateFee = ", GenerateFee, "\r\n");
			str = string.Concat(str, "GenerateTotal = ", GenerateTotal, "\r\n");
			str = string.Concat(str, "DepletingAmount = ", DepletingAmount, "\r\n");
			str = string.Concat(str, "DepletingFee = ", DepletingFee, "\r\n");
			str = string.Concat(str, "DepletingTotal = ", DepletingTotal, "\r\n");
			str = string.Concat(str, "SubTotalFee = ", SubTotalFee, "\r\n");
			str = string.Concat(str, "TotalDueToCustomer = ", TotalDueToCustomer, "\r\n");
			return str;
		}
	}
}
