using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.FirstView.Contract;
using MGI.Cxn.Fund.FirstView.Data;
using MGI.TimeStamp;
using MGI.Common.Util;

namespace MGI.Cxn.Fund.FirstView.Impl
{
    public class FVGatewayAssociate : FVGatewayActivate
    {
		public override long Register(CardAccount firstViewCard, MGIContext mgiContext, out ProcessorResult processorResult)
        {
            string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

            if (string.IsNullOrEmpty(timezone))
                throw new Exception("Time zone not provided in the context");

            processorResult = null;
            long cxnAccountId = 0;

            if (string.IsNullOrWhiteSpace(firstViewCard.CardNumber.Trim()))
            {
                throw new FundException(FundException.INVALID_CARD_NUMBER, "Account identifier should not be empty");
            }

			CardResponse response = GetCardInfo(firstViewCard.CardNumber, mgiContext, out processorResult, true);

            if (processorResult.Exception != null)
                throw new FundException(FundException.PROVIDER_ERROR, new FirstViewProviderException(Convert.ToString(FundProcessorDAL.GetExceptionMinorCode(response)), processorResult.ErrorMessage + processorResult.Exception.Message));

            if (response.ERR_NUMBER.Equals("0") && string.Compare(response.CARD_STATUS, "active", true) == 0
                && string.Compare(response.ACTIVATION_REQUIRED, "no", true) == 0) // can we refactor this to use the HandleProcessorResult...
            {
                processorResult = new ProcessorResult(int.Parse(response.ERR_NUMBER), true);

                string cardNumber = firstViewCard.CardNumber.Substring(firstViewCard.CardNumber.Length - 4);

                FirstViewCard gprCard = new FirstViewCard()
                {
                    rowguid = Guid.NewGuid(),
                    CardNumber = cardNumber,
                    AccountNumber = response.ACCOUNT_NO,
                    BSAccountNumber = response.BSAccountNumber,
                    NameAsOnCard = string.Empty,
                    FirstName = firstViewCard.FirstName,
                    MiddleName = firstViewCard.MiddleName,
                    LastName = firstViewCard.LastName,
                    DateOfBirth = firstViewCard.DateOfBirth,
                    SSN = string.IsNullOrWhiteSpace(firstViewCard.SSN)?null:(long?)long.Parse(firstViewCard.SSN),
					GovernmentId = firstViewCard.GovtIDType,
                    AddressLine1 = firstViewCard.Address1,
                    AddressLine2 = firstViewCard.Address2,
                    State = firstViewCard.State,
                    City = firstViewCard.City,
                    PostalCode = firstViewCard.ZipCode,
                    HomePhoneNumber = firstViewCard.Phone,
                    IsActive = true,
                    ExpiryDate = DateTime.Parse(response.ExpirationDate),
                    ActivatedBy = 0,
                    DeactivatedReason = string.Empty,
                    IDNumber = firstViewCard.GovernmentId,
                    GovtIdExpirationDate = firstViewCard.GovtIDExpiryDate,
                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
                    DTServerCreate = DateTime.Now //Added for Timestamp
                };

                cxnAccountId = FundProcessorDAL.AddGprCard(gprCard);
            }
            else
            {
                throw new FundException(FundException.PROVIDER_ERROR, new FirstViewProviderException(Convert.ToString(FundProcessorDAL.GetExceptionMinorCode(response)), response.ERRMSG));
            }

            return cxnAccountId;
        }
    }
}