using MGI.Core.CXE.Data;
namespace MGI.Core.CXE.Contract
{
	public interface IAccountService
	{

		/// <summary>
		/// This method is to add cash account to customer
		/// </summary>
		/// <param name="customer">This is customer details.</param>
		/// <returns>Customer cash account details</returns>
		Account AddCustomerCashAccount(Customer Customer);

		/// <summary>
		/// This method is to add fund account to customer 
		/// </summary>
		/// <param name="customer">This is customer details.</param>
		/// <returns>Customer fund account details</returns>
		Account AddCustomerFundsAccount(Customer Customer);

		/// <summary>
		/// This method is to add check account to customer
		/// </summary>
		/// <param name="customer">This is customer details.</param>
		/// <returns>Customer check account details</returns>
		Account AddCustomerCheckAccount(Customer Customer);

		/// <summary>
		/// This method is to add bill pay account to customer
		/// </summary>
		/// <param name="customer">This is customer details.</param>
		/// <returns>Customer bill pay account details</returns>
		Account AddCustomerBillPayAccount(Customer Customer);

		/// <summary>
		/// This method is to add money order account to customer 
		/// </summary>
		/// <param name="customer">This is customer details.</param>
		/// <returns>Customer money order account details</returns>
		Account AddCustomerMoneyOrderAccount(Customer Customer);

		/// <summary>
		/// This method is to add money transfer account to customer 
		/// </summary>
		/// <param name="customer">This is customer details.</param>
		/// <returns>Customer money transfer account details</returns>
		Account AddCustomerMoneyTransferAccount(Customer Customer);
	}
}
