using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Common.DataAccess.Data;

using MGI.Core.Partner.Data.Transactions;

namespace MGI.Core.Partner.Data
{
	public class ShoppingCart : NexxoModel
	{
		public virtual bool Active { get; set; }
		public virtual ShoppingCartStatus Status { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual ICollection<ShoppingCartTransaction> ShoppingCartTransactions { get; set; }

		//US1488 - Parking Transaction Changes
		public virtual bool IsParked { get; set; }
		//US1800 Referral promotions – Free check cashing to referrer and referee 
		public virtual bool IsReferral { get; set; }

		public ShoppingCart()
		{
			ShoppingCartTransactions = new List<ShoppingCartTransaction>();
		}

		/// <summary>
		/// Add a transaction to the ShoppingCart
		/// </summary>
		/// <param name="transaction">Transaction to add</param>
		public virtual void AddTransaction(Transaction transaction)
		{
			ShoppingCartTransaction trans = new ShoppingCartTransaction();
			trans.CartItemStatus = ShoppingCartItemStatus.Added;
			trans.ShoppingCart = this;
			trans.Transaction = transaction;

			if (Active)
				ShoppingCartTransactions.Add(trans);
			else
				throw new Exception("Can't add an item to a closed cart!");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		public virtual void RemoveTransaction(Transaction transaction)
		{
			ShoppingCartTransaction trans = ShoppingCartTransactions.Where(c => c.Transaction.rowguid == transaction.rowguid).First();
			trans.CartItemStatus = ShoppingCartItemStatus.Removed;
			if (Active)
				ShoppingCartTransactions.Add(trans);
			else
				throw new Exception("Can't remove an item from a closed cart!");
		}

		/// <summary>
		/// Close the ShoppingCart
		/// </summary>
		public virtual void CloseShoppingCart()
		{
			this.Active = false;
		}

		// US1488 Parking Transaction Changes
		/// <summary>
		/// Park the Transaction
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="parkingShoppingCart"></param>
		public virtual void ParkTransaction(Transaction transaction, ShoppingCart parkingShoppingCart)
		{
			try
			{
				ShoppingCartTransaction trans = ShoppingCartTransactions.Where(c => c.Transaction.rowguid == transaction.rowguid).First();
				trans.ShoppingCart = parkingShoppingCart;
				ShoppingCartTransactions.Add(trans);
			}
			catch (Exception Ex)
			{
				throw new Exception("Can't park an item", Ex);
			}
		}
	}
}
