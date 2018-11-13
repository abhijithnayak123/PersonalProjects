using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BizTransaction = MGI.Biz.Partner.Data.Transactions;

namespace MGI.Biz.Partner.Data
{
	[DataContract]
	public class ShoppingCart
	{
		[DataMember]
		public long Id;
		[DataMember]
		public List<BizTransaction.Check> Checks;
		[DataMember]
		public List<BizTransaction.Funds> Funds;
		[DataMember]
		public List<BizTransaction.BillPay> BillPays;
		[DataMember]
		public List<BizTransaction.MoneyOrder> MoneyOrders;
		[DataMember]
		public List<BizTransaction.MoneyTransfer> MoneyTransfers;
        [DataMember]
		public List<BizTransaction.Cash> CashInTransactions;
		[DataMember]
		public decimal CheckTotal;
		[DataMember]
		public decimal FundsTotal;
		[DataMember]
		public decimal BillTotal;
		[DataMember]
		public decimal MoneyOrderTotal;
		[DataMember]
		public decimal MoneyTransferTotal;
        [DataMember]
        public decimal CashTotal;

		[DataMember]
		public decimal GenerateAmount;
		[DataMember]
		public decimal GenerateFee;
		[DataMember]
		public decimal GenerateTotal;
		[DataMember]
		public decimal DepletingAmount;
		[DataMember]
		public decimal DepletingFee;
		[DataMember]
		public decimal DepletingTotal;
		[DataMember]
		public decimal SubTotalFee;
		[DataMember]
		public decimal TotalDueToCustomer;
        [DataMember]
        public bool IsReferral;        

		public ShoppingCart( long id )
		{
			Id = id;
			Checks = new List<BizTransaction.Check>();
			Funds = new List<BizTransaction.Funds>();
			BillPays = new List<BizTransaction.BillPay>();
			MoneyOrders = new List<BizTransaction.MoneyOrder>();
			MoneyTransfers = new List<BizTransaction.MoneyTransfer>();
			CashInTransactions = new List<BizTransaction.Cash>();
		}
	}
}
