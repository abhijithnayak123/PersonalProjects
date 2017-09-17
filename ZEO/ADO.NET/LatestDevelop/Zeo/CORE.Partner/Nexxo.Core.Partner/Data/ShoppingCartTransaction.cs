using MGI.Core.Partner.Data.Transactions;

namespace MGI.Core.Partner.Data
{
	public class ShoppingCartTransaction
	{
		public ShoppingCartTransaction()
		{

		}

		public ShoppingCartTransaction(ShoppingCartTransaction shoppingcartTransaction)
		{
			Transaction = shoppingcartTransaction.Transaction;
		}

		public virtual System.Guid CartTxnRowguid { get; set; }
		public virtual ShoppingCart ShoppingCart { get; set; }
		public virtual ShoppingCartItemStatus CartItemStatus { get; set; }
		public virtual Transaction Transaction { get; set; }
	}
}
