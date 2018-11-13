using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.FIS.Data;

namespace MGI.Cxn.Fund.FIS.Impl
{
    internal static class Mapper
    {
        internal static FundTrx ToFundTrx(FISTrx FISTrx)
        {
            return new FundTrx
            {
                TransactionAmount = FISTrx.Amount,
                Fee = FISTrx.Fee,
                TransactionID = FISTrx.ConfirmationId,
                PreviousCardBalance = FISTrx.Balance,
                Account = ToCardAccount(FISTrx.Account),
                TransactionType = FISTrx.TransactionType.ToString()
            };
        }

        internal static FISAccount ToFISGPRAccount(CardAccount cardaccount)
        {
            FISAccount account = new FISAccount();
            account.AccountId = Int64.Parse(cardaccount.AccountNumber);
            account.FirstName = cardaccount.FirstName;
            account.LastName = cardaccount.LastName;
            account.Address1 = cardaccount.Address1;
            account.DOB = cardaccount.DateOfBirth;
            account.Country = cardaccount.CountryCode;
            account.SSN = cardaccount.SSN;
            account.State = cardaccount.State;
            account.City = cardaccount.City;
            account.ZipCode = cardaccount.ZipCode;
            account.DTCreate = DateTime.Now;
            account.DTServerCreate = DateTime.Now;
            return account;
        }

        internal static CardAccount ToCardAccount(FISAccount Acct)
        {
            return new CardAccount
            {
                AccountNumber = Acct.ExternalKey,
                FirstName = Acct.FirstName,
                MiddleName = Acct.MiddleName,
                LastName = Acct.LastName,
                Address1 = Acct.Address1,
                Address2 = Acct.Address2,
                City = Acct.City,
                State = Acct.State,
                ZipCode = Acct.ZipCode,
                MailingAddress1 = Acct.Address1,
                MailingAddress2 = Acct.Address2,
                MailingCity = Acct.City,
                MailingState = Acct.State,
                MailingZipCode = Acct.ZipCode,
                CountryCode = Acct.Country,
                DateOfBirth = Acct.DOB,
                SSN = Acct.SSN,
                Phone = Acct.Phone,
                FraudScore = Acct.FraudScore,
                Resolution = Acct.FraudResolution,
                CardNumber = Acct.CardNumber.Substring(Acct.CardNumber.Length - 4),
                Id = Acct.Id
            };
        }

        internal static FISIOProfile ToFISIOProfile(FISAccount account)
        {
            return new FISIOProfile
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
                DOB = account.DOB,
                SSN = account.SSN,
                Phone = account.Phone,
                PhoneType = account.PhoneType
            };
        }
    }
}
