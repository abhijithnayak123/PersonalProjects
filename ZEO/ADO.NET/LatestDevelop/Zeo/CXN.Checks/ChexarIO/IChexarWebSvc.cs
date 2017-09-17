using System;
using System.Collections.Generic;

namespace ChexarIO
{
	public interface IChexarWebSvc
	{
		ChexarLogin GetChexarLogin(string baseURL, string companyId, string companyUname, string companyPwd, string employeeUname, string employeePwd);

		ChexarNewInvoiceResult CreateTransaction(ChexarLogin login, int badge, decimal amount, DateTime checkDate, int checkType, string checkNum, string routingNum, string accountNum, string micr, byte[] checkImgFront, byte[] checkImgBack, string checkImageFormat, byte[] checkImgFrontTIF, byte[] checkImgBackTIF, double[] geocodeLatLong, out int errorCode, out string errorMessage);
		bool CancelTransaction(ChexarLogin login, int invoiceNum, out string errorMessage);
		bool CloseTransaction(ChexarLogin login, int invoiceNo, out string errorMessage);
		ChexarMICRDetails GetMICRDetails(ChexarLogin login, int invoiceNum, out string errorMessage);
		List<ChexarInvoiceCheck> GetTransactionDetails(ChexarLogin login, int invoiceNo);
		ChexarInvoiceCheck GetTransactionStatus(ChexarLogin login, int invoiceNo, out string errorMessage);
		ChexarTicketStatus GetWaitTime(ChexarLogin login, int ticketNo);

		int RegisterNewCustomer(ChexarLogin login, ChexarCustomerIO customerInfo, out string errorMessage);
		bool UpdateCustomer(ChexarLogin login, ChexarCustomerIO customerInfo, out string errorMessage);
		int GetCustomerIdByBadge(ChexarLogin login, int badgeNum);
		ChexarCustomerIO FindCustomerById(ChexarLogin login, int customerId);

		ChexarCustomerIO FindCustomerByBDay(ChexarLogin login, DateTime dateOfBirth);
		ChexarCustomerIO FindCustomerByName(ChexarLogin login, string firstName, string lastName);
		ChexarCustomerIO FindCustomerByPhone(ChexarLogin login, string phoneNumber);
		ChexarCustomerIO FindCustomerBySSN(ChexarLogin login, string socialSecurityNumber);
	}
}
