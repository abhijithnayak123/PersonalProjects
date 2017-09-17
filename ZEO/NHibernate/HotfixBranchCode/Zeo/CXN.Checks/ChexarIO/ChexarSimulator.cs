using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using System.Security.Cryptography;

namespace ChexarIO
{
	public class ChexarSimulator : IChexarWebSvc
	{

		private IRepository<SimEntities.ChexarSimAccount> _accountRepo;

		public IRepository<SimEntities.ChexarSimAccount> AccountRepo
		{
			get { return _accountRepo; }
			set { _accountRepo = value; }
		}

		private IRepository<SimEntities.ChexarSimInvoice> _invoiceRepo;

		public IRepository<SimEntities.ChexarSimInvoice> InvoiceRepo
		{
			get { return _invoiceRepo; }
			set { _invoiceRepo = value; }
		}

		private const Int64 MinRange = 100000;

		private const Int64 MaxRange = 999999;

		private byte[] _uint32Buffer = new byte[4];

		#region IChexarWebSvc Members

		public ChexarLogin GetChexarLogin(string baseURL, string companyId, string companyUname, string companyPwd, string employeeUname, string employeePwd)
		{
			return new ChexarLogin
			{
				URL = string.Empty,
				CompanyToken = "simulator",
				IngoBranchId = 0,
				EmployeeId = 0
			};
		}

		public ChexarNewInvoiceResult CreateTransaction(ChexarLogin login, int badge, decimal amount, DateTime checkDate, int checkType, string checkNum, string routingNum, string accountNum, string micr, byte[] checkImgFront, byte[] checkImgBack, string checkImageFormat, byte[] checkImgFrontTIF, byte[] checkImgBackTIF, double[] geocodeLatLong, out int errorCode, out string errorMessage)
		{
			errorCode = 0;
			errorMessage = string.Empty;  

			if (amount == 55.55m)
			{
				errorCode = 10;
				errorMessage = "Duplicate Check Violation";
				return new ChexarNewInvoiceResult();
			}
			else if (amount == 66.66m)
			{
				errorCode = -1;
				errorMessage = "Customer on hold";
				return new ChexarNewInvoiceResult();
			}

			SimEntities.ChexarSimAccount account = _accountRepo.FindBy(a => a.Badge == badge);

			SimEntities.ChexarSimInvoice invoice = new SimEntities.ChexarSimInvoice
			{
				Amount = amount,
				TicketId = 0,
				WaitTime = string.Empty,
				DeclineReason = string.Empty,
				DeclineId = 0,
				Fee = decimal.Zero,
				Account = account,
				DTServerCreate = DateTime.Now
			};
            //Divya@10/27/2015, AL-2013 
            if ((DateTime.Now.Date - checkDate).Days > 181)//morethan 6 months old check
            {
                errorCode = -2;
                invoice.Status = "Declined";
                _invoiceRepo.AddWithFlush(invoice);
                ChexarNewInvoiceResult resultNew = new ChexarNewInvoiceResult
                {
                    InvoiceNo = invoice.Id,
                    Status = invoice.Status,
                    OnHold = invoice.Status == "Approved" || invoice.Status == "Declined" ? false : true,
                    TicketNo = invoice.TicketId
                };

                return resultNew;
            }
            else if ((checkDate - DateTime.Now.Date).Days > 3)//more than 72 hours post dated check
            {
                errorCode = -3;
                invoice.Status = "Declined";
                _invoiceRepo.AddWithFlush(invoice);
                ChexarNewInvoiceResult resultNew = new ChexarNewInvoiceResult
                {
                    InvoiceNo = invoice.Id,
                    Status = invoice.Status,
                    OnHold = invoice.Status == "Approved" || invoice.Status == "Declined" ? false : true,
                    TicketNo = invoice.TicketId
                };

                return resultNew;
            }
            

			int endsWith = (int)(amount * 100) % 10;

			invoice.Status = endsWith == 0 ? "Approved" : amount.ToString().EndsWith("1") ? "Declined" : "Pending";

			if (invoice.Status == "Approved")
				invoice.CheckType = 23; // Printed Payroll
			else if (invoice.Status == "Declined")
			{
				DeclineReason d = getRandomDeclineReason();
				invoice.DeclineId = d.Code;
				invoice.DeclineReason = d.Reason;
			}
			else
			{
				invoice.TicketId = generateTicketId();
				invoice.WaitTime = endsWith.ToString();

				Console.WriteLine("ticketId: " + invoice.TicketId);

				if (endsWith == 2)
					invoice.CheckType = 19; //Two Party
				else if (endsWith == 3)
					invoice.CheckType = 21; //Handwritten Payroll
				else if (endsWith == 4)
					invoice.CheckType = 22; //Money Order
				else if (endsWith == 5)
					invoice.CheckType = 18; //Insurance/Attorney/Cashiers
				else if (endsWith == 6)
					invoice.CheckType = 26; //US Treasury
				else if (endsWith == 7)
					invoice.CheckType = 424; //Govt
				else if (endsWith == 8)
					invoice.CheckType = 421; //RAC/Loan
			}

			Console.WriteLine("status: {0}", invoice.Status);

			_invoiceRepo.AddWithFlush(invoice);

			ChexarNewInvoiceResult result = new ChexarNewInvoiceResult
			{
				InvoiceNo = invoice.Id,
				Status = invoice.Status,
				OnHold = invoice.Status == "Approved" || invoice.Status == "Declined" ? false : true,
				TicketNo = invoice.TicketId
			};

			return result;
		}

		public bool CancelTransaction(ChexarLogin login, int invoiceNum, out string errorMessage)
		{
			errorMessage = string.Empty;

			SimEntities.ChexarSimInvoice invoice = _invoiceRepo.FindBy(i => i.Id == invoiceNum);

			invoice.Status = "Voided";
			_invoiceRepo.UpdateWithFlush(invoice);

			return true;
		}

		public bool CloseTransaction(ChexarLogin login, int invoiceNo, out string errorMessage)
		{
			errorMessage = string.Empty;

			SimEntities.ChexarSimInvoice invoice = _invoiceRepo.FindBy(i => i.Id == invoiceNo);

			invoice.Status = "Closed";
			_invoiceRepo.UpdateWithFlush(invoice);

			return true;
		}

		public ChexarMICRDetails GetMICRDetails(ChexarLogin login, int invoiceNum, out string errorMessage)
		{
			errorMessage = string.Empty;

			SimEntities.ChexarSimInvoice invoice = _invoiceRepo.FindBy(i => i.Id == invoiceNum);

			return new ChexarMICRDetails
			{
				CheckTypeId = invoice.CheckType,
				ABARoutingNumber = "12345678",
				CheckNumber = "1245678",
				CheckAmount = invoice.Amount,
				AccountNumber = "11111111",
				FeeAmount = 1m
			};
		}

		public List<ChexarInvoiceCheck> GetTransactionDetails(ChexarLogin login, int invoiceNo)
		{
			SimEntities.ChexarSimInvoice invoice = _invoiceRepo.FindBy(i => i.Id == invoiceNo);

			List<ChexarInvoiceCheck> l = new List<ChexarInvoiceCheck>();
			l.Add(
				new ChexarInvoiceCheck
				{
					Badge = invoice.Account.Badge,
					Approved = invoice.Status == "Approved",
					CheckAmount = invoice.Amount,
					CheckTypeId = invoice.CheckType,
					CheckNumber = 12345,
					DeclineId = invoice.DeclineId,
					DeclineReason = invoice.DeclineReason,
					DetailId = 123,
					DTCheck = DateTime.Today,
					FeeAmount = invoice.Fee,
					FeeRate = .01m
				}
			);
			return l;
		}

		public ChexarInvoiceCheck GetTransactionStatus(ChexarLogin login, int invoiceNo, out string errorMessage)
		{
			errorMessage = string.Empty;
			SimEntities.ChexarSimInvoice invoice = _invoiceRepo.FindBy(i => i.Id == invoiceNo);

			if (invoice.Status == "Pending")
			{
				int waitTime = int.Parse(invoice.WaitTime);
				if (waitTime <= 1)
				{
					int endsWith = (int)(invoice.Amount * 100) % 10;

					invoice.WaitTime = string.Empty;

					if (endsWith == 9)
					{
						invoice.Status = "Declined";

						DeclineReason d = getRandomDeclineReason();
						invoice.DeclineId = d.Code;
						invoice.DeclineReason = d.Reason;
					}
					else
					{
						invoice.Status = "Approved";
					}
				}
				else
					return new ChexarInvoiceCheck();

				_invoiceRepo.UpdateWithFlush(invoice);
			}

			return new ChexarInvoiceCheck
			{
				Badge = invoice.Account.Badge,
				Approved = invoice.Status == "Approved",
				CheckAmount = invoice.Amount,
				CheckTypeId = invoice.CheckType,
				CheckNumber = 12345,
				DeclineId = invoice.DeclineId,
				DeclineReason = invoice.DeclineReason,
				DetailId = 123,
				DTCheck = DateTime.Today,
				FeeAmount = invoice.Fee,
				FeeRate = .01m
			};
		}

		public ChexarTicketStatus GetWaitTime(ChexarLogin login, int ticketNo)
		{
			SimEntities.ChexarSimInvoice invoice = _invoiceRepo.FindBy(i => i.TicketId == ticketNo);

			int waitTime = int.Parse(invoice.WaitTime);
			waitTime--;
			invoice.WaitTime = waitTime.ToString();

			Console.WriteLine("wait time: {0}", invoice.WaitTime);

			_invoiceRepo.UpdateWithFlush(invoice);

			ChexarTicketStatus ticketStatus = new ChexarTicketStatus
			{
				Status = invoice.Status,
				TicketNo = ticketNo,
				WaitTime = string.Format("{0} minutes", waitTime)
			};

			return ticketStatus;
		}

		public int RegisterNewCustomer(ChexarLogin login, ChexarCustomerIO customerInfo, out string errorMessage)
		{
			errorMessage = string.Empty;

			SimEntities.ChexarSimAccount account = new SimEntities.ChexarSimAccount
			{
				FName = customerInfo.FName,
				LName = customerInfo.LName,
				DateOfBirth = customerInfo.DateOfBirth.Equals(DateTime.MinValue) ? null : customerInfo.DateOfBirth,
				Address1 = customerInfo.Address1,
				Address2 = customerInfo.Address2,
				City = customerInfo.City,
				State = customerInfo.State,
				Zip = customerInfo.Zip,
				Phone = customerInfo.Phone,
				Occupation = customerInfo.Occupation,
				Employer = customerInfo.Employer,
				EmployerPhone = customerInfo.EmployerPhone,
				GovernmentId = customerInfo.GovernmentId,
				IDType = customerInfo.IDType,
				IDCountry = customerInfo.IDCountry,
				IDExpDate = DateTime.Today,//customerInfo.IDExpDate,
				CardNumber = customerInfo.CardNumber.ToString(),
				CustomerScore = customerInfo.CustomerScore,
				IDImage = customerInfo.IDImage,
				ITIN = customerInfo.ITIN,
				SSN = customerInfo.SSN,
				DTServerCreate = DateTime.Now,
				DTServerLastModified = DateTime.Now,
				IDCode = customerInfo.IDCode
			};

			_accountRepo.AddWithFlush(account);

			return account.Badge;
		}

		public bool UpdateCustomer(ChexarLogin login, ChexarCustomerIO customerInfo, out string errorMessage)
		{
			throw new NotImplementedException();
		}

		public int GetCustomerIdByBadge(ChexarLogin login, int badgeNum)
		{
			throw new NotImplementedException();
		}

		public ChexarCustomerIO FindCustomerById(ChexarLogin login, int customerId)
		{
			throw new NotImplementedException();
		}

		public ChexarCustomerIO FindCustomerByBDay(ChexarLogin login, DateTime dateOfBirth)
		{
			throw new NotImplementedException();
		}

		public ChexarCustomerIO FindCustomerByName(ChexarLogin login, string firstName, string lastName)
		{
			throw new NotImplementedException();
		}

		public ChexarCustomerIO FindCustomerByPhone(ChexarLogin login, string phoneNumber)
		{
			throw new NotImplementedException();
		}

		public ChexarCustomerIO FindCustomerBySSN(ChexarLogin login, string socialSecurityNumber)
		{
			throw new NotImplementedException();
		}
		#endregion

		private int generateTicketId()
		{
			/****************************Begin TA-50 Changes************************************************/
			//     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
			//      Purpose: On Vera Code Scan, the below methods were found having Insufficient Entropy which caused due to usage random.next, we are now using RNGCryptoServiceProvider to get random number
			Int32 ticketId = 100000;
			RNGCryptoServiceProvider _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
			Int64 diff = MaxRange - MinRange;
			while (true)
			{
				_rngCryptoServiceProvider.GetBytes(_uint32Buffer);
				UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);

				Int64 max = (1 + (Int64)UInt32.MaxValue);
				Int64 remainder = max % diff;
				if (rand < max - remainder)
				{
					ticketId = (Int32)(MinRange + (rand % diff));
					break;
				}
			}
			return ticketId;
			/****************************End TA-50 Changes************************************************/
		}

		private class DeclineReason
		{
			public int Code;
			public string Reason;
		}

		private DeclineReason getRandomDeclineReason()
		{
			var declineList = new List<DeclineReason>();
			declineList.Add(new DeclineReason { Code = 38, Reason = "Please resubmit-Teller used the wrong customer membership" });
			declineList.Add(new DeclineReason { Code = 40, Reason = "Resubmit if you have more info-Need info from customer and teller is not responding" });
			declineList.Add(new DeclineReason { Code = 41, Reason = "Return check to customer-Bank needs to cancel their endorsement" });
			declineList.Add(new DeclineReason { Code = 42, Reason = "Return check to customer-All payees are not present" });
			declineList.Add(new DeclineReason { Code = 43, Reason = "Return check to customer-Maker will not verify check" });
			declineList.Add(new DeclineReason { Code = 44, Reason = "Do not cash-This is not a check" });
			declineList.Add(new DeclineReason { Code = 45, Reason = "Return check to customer-Maker temporarily unavailable.  Can try again in an hour." });
			declineList.Add(new DeclineReason { Code = 46, Reason = "Resubmit if you have more info-check is not filled out" });
			declineList.Add(new DeclineReason { Code = 47, Reason = "Resubmit if you have more info-We cannot verify makers business or identity" });
			declineList.Add(new DeclineReason { Code = 48, Reason = "Return check to customer-Per Teller $ not available in store to cash" });
			declineList.Add(new DeclineReason { Code = 49, Reason = "LATESHIFT-cannot approve until normal business hours when we can contact maker" });
			declineList.Add(new DeclineReason { Code = 50, Reason = "Return check to customer-Check cashing limit exceeded for today" });
			declineList.Add(new DeclineReason { Code = 51, Reason = "Please resubmit-Your check will be approved after customer endorses and you rescan" });
			declineList.Add(new DeclineReason { Code = 52, Reason = "Resubmit if you have more info-Customers ID is not acceptable for check cashing" });
			declineList.Add(new DeclineReason { Code = 53, Reason = "Return check to customer-This check should be cashed via the Regions banking system" });
			declineList.Add(new DeclineReason { Code = 54, Reason = "Resubmit in you have more info-Duplicate Check for UNIDOS.  Call 1-800-317-6055" });
			declineList.Add(new DeclineReason { Code = 55, Reason = "Do not cash – Government check or money order is suspicious" });
			declineList.Add(new DeclineReason { Code = 57, Reason = "Do not cash – payee verification failed" });

			/****************************Begin TA-50 Changes************************************************/
			//      User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
			//      Purpose: On Vera Code Scan, the below methods were found having Insufficient Entropy which caused due to usage random.next, we are now using RNGCryptoServiceProvider to get random number
			int r = 1;
			RNGCryptoServiceProvider _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
			Int64 diff = declineList.Count - 1;
			while (true)
			{
				_rngCryptoServiceProvider.GetBytes(_uint32Buffer);
				UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);

				Int64 max = (1 + (Int64)UInt32.MaxValue);
				Int64 remainder = max % diff;
				if (rand < max - remainder)
				{
					r = (Int32)(1 + (rand % diff));
					break;
				}
			}
			/****************************End TA-50 Changes************************************************/
			return declineList[r];
		}

	}
}
