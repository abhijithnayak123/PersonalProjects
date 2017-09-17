// -----------------------------------------------------------------------
// <copyright file="ChexarMapper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;

using MGI.Cxn.Check.Data;
using MGI.Cxn.Check.Contract;
using MGI.Cxn.Check.Chexar.Data;

using ChexarIO;

namespace MGI.Cxn.Check.Chexar.Impl
{
	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class ChexarMapper
	{
		public static ChexarAccount ConvertToCxn(CheckAccount checkAccount)
		{
			// covert the CheckAccount to ChexarAccount 
			var chxrAccount = new ChexarAccount 
			{				
				FirstName = checkAccount.FirstName,
				LastName = checkAccount.LastName,
				Address1 = checkAccount.Address1,
				Address2 = checkAccount.Address2,
				City = checkAccount.City,
				State = checkAccount.State,
				Zip = checkAccount.Zip,
				Phone = checkAccount.Phone,
				Occupation = checkAccount.Occupation,
				Employer = checkAccount.Employer,
				EmployerPhone = checkAccount.EmployerPhone,
				GovernmentId = checkAccount.GovernmentId,
				IDCardType = checkAccount.IDType,
				IDCardIssuedCountry = checkAccount.IDCountry,
				IDCardExpireDate = checkAccount.IDExpireDate,
				CardNumber = checkAccount.CardNumber,
				CustomerScore = checkAccount.CustomerScore,
				DateOfBirth = checkAccount.DateOfBirth == DateTime.MinValue ? null : checkAccount.DateOfBirth,
				
				SSN = checkAccount.SSN,
				IDCardImage = checkAccount.IDImage,
				DTServerCreate = DateTime.Now,
				IDCardIssuedDate = checkAccount.IDIssueDate,
				IDCode = checkAccount.IDCode 
			};

			if (chxrAccount.IDCode == "I" && string.IsNullOrWhiteSpace(chxrAccount.ITIN))
			{
				chxrAccount.ITIN = checkAccount.SSN;
				chxrAccount.SSN = string.Empty;
			}

			return chxrAccount;
			
		}

		public static CheckAccount ConvertToCheck(ChexarAccount chexarAccount)
		{
			var checkAccount = new CheckAccount
			{

				Id = chexarAccount.Id,
				FirstName = chexarAccount.FirstName,
				LastName = chexarAccount.LastName,
				Address1 = chexarAccount.Address1,
				Address2 = chexarAccount.Address2,
				City = chexarAccount.City,
				State = chexarAccount.State,
				Zip = chexarAccount.Zip,
				Phone = chexarAccount.Phone,
				Occupation = chexarAccount.Occupation,
				Employer = chexarAccount.Employer,
				EmployerPhone = chexarAccount.EmployerPhone,
				GovernmentId = chexarAccount.GovernmentId,
				IDType = chexarAccount.IDCardType,
				IDCountry = chexarAccount.IDCardIssuedCountry,
				IDExpireDate = chexarAccount.IDCardExpireDate,
				CardNumber = chexarAccount.CardNumber,
				CustomerScore = chexarAccount.CustomerScore,
				DateOfBirth = chexarAccount.DateOfBirth,
				SSN = chexarAccount.SSN,
				IDImage = chexarAccount.IDCardImage,
				IDIssueDate = chexarAccount.IDCardIssuedDate,
				IDCode = chexarAccount.IDCode
			};
			if(checkAccount.IDCode == "I" && string.IsNullOrWhiteSpace(checkAccount.ITIN))
			{
				checkAccount.ITIN = chexarAccount.SSN;
				checkAccount.SSN = string.Empty;
			}			
			return checkAccount;
		}

		public static ChexarCustomerIO ConvertToChexar(CheckAccount checkAccount)
		{
			var chexarCustomer = new ChexarCustomerIO
			{
				FName = checkAccount.FirstName ?? string.Empty,
				LName = checkAccount.LastName ?? string.Empty,
				Address1 = checkAccount.Address1 ?? string.Empty,
				Address2 = checkAccount.Address2 ?? string.Empty,
				City = checkAccount.City ?? string.Empty,
				State = checkAccount.State ?? string.Empty,
				Zip = checkAccount.Zip ?? string.Empty,
				Phone = checkAccount.Phone ?? string.Empty,
				Occupation = checkAccount.Occupation ?? string.Empty,
				Employer = checkAccount.Employer ?? string.Empty,
				EmployerPhone = checkAccount.EmployerPhone ?? string.Empty,
				GovernmentId = checkAccount.GovernmentId ?? string.Empty,
				IDType = ChexarIO.ChexarIDTypes.Unknown, // fix later
				IDCountry = checkAccount.IDCountry ?? string.Empty,
				CardNumber = checkAccount.CardNumber,
				CustomerScore = checkAccount.CustomerScore == 0 ? 100 : checkAccount.CustomerScore,
				DateOfBirth = checkAccount.DateOfBirth,
				SSN = checkAccount.SSN ?? string.Empty,
				IDImage = checkAccount.IDImage,
				IDCode = checkAccount.IDCode
			};

			if (checkAccount.IDExpireDate != null)
				chexarCustomer.IDExpDate = (DateTime)checkAccount.IDExpireDate;
			if (chexarCustomer.IDCode == "I" && string.IsNullOrWhiteSpace(chexarCustomer.ITIN))
			{
				chexarCustomer.ITIN = checkAccount.SSN;
				chexarCustomer.SSN = string.Empty;
			}
			return chexarCustomer;
		}

		public static ChexarTrx ConvertToCxn(CheckInfo check)
		{
			return new ChexarTrx
			 {
				 Amount = check.Amount,
				 Micr = check.Micr,
				 Latitude = check.Latitude,
				 Longitude = check.Longitude,
				 CheckDate = check.IssueDate,
			 };
		}

		public static CheckStatus ConvertToFacade(string status, bool onhold)
		{
			if (onhold) { return CheckStatus.Pending; }

			switch (status.ToLower())
			{
				case "approved":
					return CheckStatus.Approved;

				case "failed":
					return CheckStatus.Failed;

				default:
					return CheckStatus.Declined;
			}
		}
		
		public static CheckTrx ConvertToCheckTrx(ChexarTrx chexarTrx)
		{
			return new CheckTrx
			{
				Id = chexarTrx.Id,
				Amount = chexarTrx.Amount,
				ReturnAmount = chexarTrx.ChexarAmount,
				ReturnFee = chexarTrx.ChexarFee,
				Status = chexarTrx.Status,
				DeclineCode = chexarTrx.DeclineCode.ToString(),
				DeclineMessage = chexarTrx.Message,
				WaitTime = chexarTrx.WaitTime,
				ConfirmationNumber = chexarTrx.InvoiceId.ToString(),
				CheckNumber = chexarTrx.CheckNumber,
				TicketId = chexarTrx.TicketId,
				SubmitType = (CheckType)chexarTrx.SubmitType,
				ReturnType = (CheckType)chexarTrx.ReturnType
			};
		}
	}
}
