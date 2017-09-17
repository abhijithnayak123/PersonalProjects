using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.Consumer.Server.Contract
{

	public interface IShoppingCartService
	{
		/// <summary>
		/// This method is to remove the bill pay transaction from the shopping cart check out
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier of customer session</param>
		/// <param name="billPayId">This is bill pay transaction id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void RemoveBillPay(long customerSessionId, long billPayId, MGIContext mgiContext);

		/// <summary>
		/// This method is to remove the money transfer from the shopping cart check out
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier of customer session</param>
		/// <param name="moneyTransferId">This is send money transaction id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the shopping cart details
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier of customer session</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Shopping cart details</returns>
		ShoppingCart ShoppingCart(long customerSessionId, MGIContext mgiContext);
	

		/// <summary>
		/// This method is to get the shopping cart check out status
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier of customer session</param>
		/// <param name="cardNumber">This is card number is an optional parameter in the BIZ layer.</param>
		/// <param name="shoppingCartstatus">This is shopping cart check out status</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Shopping cart check out status</returns>
		ShoppingCartCheckoutStatus Checkout(long customerSessionId, string cardNumber, SharedData.ShoppingCartCheckoutStatus shoppingCartstatus, MGIContext mgiContext);

		/// <summary>
		/// This method is to generate the receipt for shopping cart
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier of customer session</param>
		/// <param name="shoppingCartId">This is unique identifier of shopping cart</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Shopping cart receipt details</returns>
		Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext);

		/// <summary>
		/// This method is to close the shopping cart
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier of customer session</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void CloseShoppingCart(long customerSessionId, MGIContext mgiContext);

	}
}
