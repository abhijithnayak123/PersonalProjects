using System.Collections.Generic;
using Spring.Transaction.Interceptor;
using MGI.Channel.Consumer.Server.Contract;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.Consumer.Server.Impl
{
	public partial class ConsumerEngine : IShoppingCartService
	{
		#region Injected Services

		#endregion

		#region ShoppingCart Engine Data Mapper

		/// <summary>
		/// 
		/// </summary>
		internal static void ShoppingCartConverter()
		{

		}

		#endregion

		#region IShoppingCartService Impl

		[Transaction()]
		public void RemoveBillPay(long customerSessionId, long billPayId, MGIContext mgiContext)
		{
			SharedEngine.RemoveBillPay(customerSessionId, billPayId, false, mgiContext);
		}

		[Transaction()]
		public void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext)
		{
			SharedEngine.RemoveMoneyTransfer(customerSessionId, moneyTransferId, false, mgiContext);
		}

		[Transaction()]
		public SharedData.ShoppingCart ShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
			return SharedEngine.ShoppingCart(customerSessionId, mgiContext);
		}

		[Transaction()]
		public SharedData.ShoppingCartCheckoutStatus Checkout(long customerSessionId, string cardNumber, SharedData.ShoppingCartCheckoutStatus shoppingCartstatus, MGIContext mgiContext)
		{
			return SharedEngine.Checkout(customerSessionId, 0m, cardNumber, shoppingCartstatus, mgiContext);
		}

		//TODO receipt Implementation for MVA yet to be desided
		[Transaction()]
		public SharedData.Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext)
		{
			return new SharedData.Receipts();
		}

		[Transaction()]
		public void CloseShoppingCart(long customerSessionId, MGIContext mgiContext)
		{
			SharedEngine.CloseShoppingCart(customerSessionId, mgiContext);
		}

		#endregion



	}
}
