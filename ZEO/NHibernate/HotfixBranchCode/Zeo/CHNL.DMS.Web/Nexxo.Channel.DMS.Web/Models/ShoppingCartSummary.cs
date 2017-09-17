using MGI.Channel.DMS.Web.Common;
using MGI.Channel.Shared.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SharedData = MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Web.Models
{
	public class ShoppingCartSummary
	{
		private List<ShoppingCartSummaryItem> _items = new List<ShoppingCartSummaryItem>();
		public List<ShoppingCartSummaryItem> Items
		{
			get
			{
				return _items;
			}
			set
			{
				value = _items;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="checks"></param>
		/// <param name="status"></param>
		public void CheckSummary(List<SharedData.Check> checks, string status)
		{
			if (checks != null && checks.Count(c => c.Status == status) > 0)
			{
				_items.Add(new ShoppingCartSummaryItem
				{
					Product = "Check Process",
					Status = status,
					TxnType = "c",
					TxnCount = checks.Count(c => c.Status == status),
					Amount = checks.Where(c => c.Status == status).Sum(s => (s.Amount)),
					Fee = checks.Where(c => c.Status == status).Sum(s => (s.Fee)),
					Total = checks.Where(c => c.Status == status).Sum(s => (s.Amount - s.Fee))
				});
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bills"></param>
		/// <param name="status"></param>
		public void BillSummary(List<SharedData.Bill> bills, string status)
		{
			if (bills != null && bills.Count(b => b.Status == status) > 0)
			{
				_items.Add(new ShoppingCartSummaryItem
				{
					Product = "Bill Process",
					Status = status,
					TxnType = "d",
					TxnCount = bills.Count(b => b.Status == status),
					Amount = bills.Where(b => b.Status == status).Sum(s => (s.Amount)),
					Fee = bills.Where(c => c.Status == status).Sum(s => (s.Fee)),
					Total = bills.Where(c => c.Status == status).Sum(s => (s.Amount + s.Fee))
				});
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moneyTransfer"></param>
		/// <param name="status"></param>
		public void MoneyTransferSummary(List<SharedData.MoneyTransfer> moneyTransfer, string status)
		{

			if (moneyTransfer != null && moneyTransfer.Count(b => b.Status == status) > 0)
			{
				if (moneyTransfer.Count(m => m.Status == status && m.TransferType == (int)SharedData.TransferType.SendMoney) > 0)
				{
					//We are adding 2 records for Send Money Modify, one for modify and another for Cancel with same amount ,
					//so we are not considering amount for calculations (since Modify+Cancel=0)
					decimal amount = moneyTransfer.Where(m => m.Status == status && m.TransferType == (int)SharedData.TransferType.SendMoney && string.IsNullOrEmpty(m.TransactionSubType)).Sum(s => (s.Amount));
					decimal fee = moneyTransfer.Where(m => m.Status == status && m.TransferType == (int)TransferType.SendMoney && string.IsNullOrEmpty(m.TransactionSubType)).Sum(s => (s.Fee));
					_items.Add(new ShoppingCartSummaryItem
					{
						Product = "Send Money",
						Status = status,
						TxnType = "d",
						TxnCount = moneyTransfer.Count(m => m.Status == status && m.TransferType == (int)TransferType.SendMoney),
						Amount = amount,
						Fee = fee,
						Total = amount + fee,
					});
				}
				if (moneyTransfer.Count(m => m.Status == status && m.TransferType == (int)TransferType.RecieveMoney) > 0)
				{
					_items.Add(new ShoppingCartSummaryItem
					{
						Product = "Receive Money",
						Status = status,
						TxnType = "c",
						TxnCount = moneyTransfer.Count(m => m.Status == status && m.TransferType == (int)TransferType.RecieveMoney),
						Amount = moneyTransfer.Where(m => m.Status == status && m.TransferType == (int)TransferType.RecieveMoney).Sum(s => (s.Amount)),
						Fee = moneyTransfer.Where(c => c.Status == status && c.TransferType == (int)TransferType.RecieveMoney).Sum(s => (s.Fee)),
						Total = moneyTransfer.Where(c => c.Status == status && c.TransferType == (int)TransferType.RecieveMoney).Sum(s => (s.Amount - s.Fee))
					});
				}
                if (moneyTransfer.Count(m => m.Status == status && m.TransferType == (int)TransferType.Refund) > 0)
                {
                    _items.Add(new ShoppingCartSummaryItem
                    {
                        Product = "Refund",
                        Status = status,
                        TxnType = "c",
                        TxnCount = moneyTransfer.Count(m => m.Status == status && m.TransferType == (int)TransferType.Refund),
                        Amount = moneyTransfer.Where(m => m.Status == status && m.TransferType == (int)TransferType.Refund).Sum(s => (s.Amount)),
                        Fee = moneyTransfer.Where(c => c.Status == status && c.TransferType == (int)TransferType.Refund).Sum(s => (s.Fee)),
                        Total = moneyTransfer.Where(c => c.Status == status && c.TransferType == (int)TransferType.Refund).Sum(s => (s.Amount - s.Fee))
                    });
                }
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="funds"></param>
		/// <param name="status"></param>
		public void FundsSummary(List<SharedData.GprCard> funds, string status)
		{
			if (funds != null && funds.Count(b => b.Status == status) > 0)
			{
				decimal amount = 0;

				amount += funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_LOAD)).Sum((d => d.LoadAmount));
				amount -= funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_WITHDRAW)).Sum((d => d.WithdrawAmount + d.WithdrawFee));
				amount += funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_ACTIVATE)).Sum((d => d.InitialLoadAmount));
				amount += funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_ACTIVATE)).Sum((d => d.ActivationFee));

				decimal fee = 0;
				fee += funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_LOAD)).Sum((d => d.LoadFee));
				fee -= funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_WITHDRAW)).Sum((d => d.WithdrawFee));
				fee += funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_ACTIVATE)).Sum((d => d.ActivationFee));

				decimal totalamt = 0;
				totalamt += funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_LOAD)).Sum((d => d.LoadAmount));
				totalamt -= funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_WITHDRAW)).Sum((d => d.WithdrawAmount));
				totalamt += funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_ACTIVATE)).Sum((d => d.ActivationFee));
				totalamt += funds.Where(s => s.Status == status && (s.ItemType == Constants.PREPAID_CARD_ACTIVATE)).Sum((d => d.InitialLoadAmount));

				if (amount < 0) // Withdraw is having -value
				{
					_items.Add(new ShoppingCartSummaryItem
					{
						Product = "Gpr Card",
						Status = status,
						TxnType = "c",
						TxnCount = funds.Count(m => m.Status == status),
						Amount = Math.Abs(amount),
						Fee = Math.Abs(fee),
						Total = Math.Abs(totalamt)
					});
				}
				else
				{
					_items.Add(new ShoppingCartSummaryItem
					{
						Product = "Gpr Card",
						Status = status,
						TxnType = "d",
						TxnCount = funds.Count(m => m.Status == status),
						Amount = Math.Abs(amount),
						Fee = Math.Abs(fee),
						Total = Math.Abs(totalamt)
					});
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="moneyOrders"></param>
		/// <param name="status"></param>
		public void MoneyOrderSummary(List<SharedData.MoneyOrder> moneyOrders, string status)
		{
			if (moneyOrders != null && moneyOrders.Count(c => c.Status == status) > 0)
			{
				_items.Add(new ShoppingCartSummaryItem
				{
					Product = "Money Order",
					Status = status,
					TxnType = "d",
					TxnCount = moneyOrders.Count(c => c.Status == status),
					Amount = moneyOrders.Where(c => c.Status == status).Sum(s => (s.Amount)),
					Fee = moneyOrders.Where(c => c.Status == status).Sum(s => (s.Fee)),
					Total = moneyOrders.Where(c => c.Status == status).Sum(s => (s.Amount + s.Fee))
				});
			}
		}
	}
}