using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.TSys.Data;

namespace MGI.Cxn.Fund.TSys.Impl
{
	public static class TSysMapper
	{
		public static FundTrx ToFundTrx(TSysTransaction tSysTrx)
		{
			return new FundTrx
			{
				TransactionAmount = tSysTrx.Amount,
				Fee = tSysTrx.Fee,
				TransactionID = tSysTrx.ConfirmationId,
				PreviousCardBalance = tSysTrx.Balance,
                Account = ToCardAccount(tSysTrx.Account),
                TransactionType = tSysTrx.TransactionType.ToString()
			};
		}

		public static CardAccount ToCardAccount(TSysAccount tSysAcct)
		{
			return new CardAccount
			{
				AccountNumber = tSysAcct.ExternalKey,
				FirstName = tSysAcct.FirstName,
				MiddleName = tSysAcct.MiddleName,
				LastName = tSysAcct.LastName,
				Address1 = tSysAcct.Address1,
				Address2 = tSysAcct.Address2,
				City = tSysAcct.City,
				State = tSysAcct.State,
				ZipCode = tSysAcct.ZipCode,
				MailingAddress1 = tSysAcct.Address1,
				MailingAddress2 = tSysAcct.Address2,
				MailingCity = tSysAcct.City,
				MailingState = tSysAcct.State,
				MailingZipCode = tSysAcct.ZipCode,
				CountryCode = tSysAcct.Country,
				DateOfBirth = tSysAcct.DateOfBirth,
				SSN = tSysAcct.SSN,
				Phone = tSysAcct.Phone,
				FraudScore = tSysAcct.FraudScore,
				Resolution = tSysAcct.FraudResolution,
                CardNumber = tSysAcct.CardNumber.Substring(tSysAcct.CardNumber.Length -4),
                Id = tSysAcct.Id,
				IsCardActive = tSysAcct.Activated
			};
		}

		public static TSysAccount ToTSysAccount(CardAccount cardAcct)
		{
			return new TSysAccount
			{
				ExternalKey = cardAcct.AccountNumber,
				FirstName = cardAcct.FirstName,
				MiddleName = cardAcct.MiddleName,
				LastName = cardAcct.LastName,
				Address1 = cardAcct.MailingAddress1,
				Address2 = cardAcct.MailingAddress2,
				City = cardAcct.MailingCity,
				State = cardAcct.MailingState,
				ZipCode = cardAcct.MailingZipCode,
				Country = cardAcct.CountryCode,
				DateOfBirth = cardAcct.DateOfBirth,
				SSN = cardAcct.SSN,
				Phone = cardAcct.Phone,
				FraudScore = cardAcct.FraudScore,
				FraudResolution = cardAcct.Resolution
			};
		}

		public static TSysIOProfile ToTSysIOProfile(TSysAccount account)
		{
			return new TSysIOProfile
			{
				ExternalKey = account.ExternalKey,
				AccountId = account.AccountId,
				UserId = account.UserId,
				FirstName = account.FirstName,
				MiddleName = account.MiddleName,
				LastName = account.LastName,
				Address1 = account.Address1,
				Address2 = account.Address2,
				City = account.City,
				State = account.State,
				ZipCode = account.ZipCode,
				Country = account.Country,
				DateOfBirth = account.DateOfBirth,
				SSN = account.SSN,
				Phone = account.Phone,
				PhoneType = account.PhoneType
			};
		}
	}
}
