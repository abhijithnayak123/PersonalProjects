--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-17-2016>
-- Description:	Get the Validate Request. 
-- Jira ID:		<AL-8325>

--EXEC usp_GetValidateRequest 1000000001, 1000000000
-- ================================================================================

IF OBJECT_ID(N'usp_GetValidateRequest', N'P') IS NOT NULL
DROP PROC usp_GetValidateRequest
GO


CREATE PROCEDURE usp_GetValidateRequest
(
    @customerSessionId BIGINT
	,@wuTransactionId BIGINT
)
AS
BEGIN
	BEGIN TRY
	
		SELECT 
			c.FirstName,  
			c.LastName,
			LastName2,
			c.MothersMaidenName,
			c.DOB,
			c.Address1,
			c.Address2,
			c.City,
			c.Phone1 AS CustomerPhoneNumber,
			c.State,
			c.ZipCode,
			nt.Name AS PrimaryIdType,
			CASE
				WHEN c.Phone1 IS NOT NULL
					AND c.Phone1Type = 'Cell'
				THEN c.Phone1
				WHEN c.Phone2 IS NOT NULL
					AND c.Phone2Type = 'Cell'
				THEN c.Phone2
				ELSE ''
			END AS CustomerMobileNumber,
			ISNULL(mc.Name, '') AS PrimaryIdCountryOfIssue,
			ISNULL(nt.Country, '') AS PrimaryCountryOfIssue,
			ISNULL(ts.Name, '') AS PrimaryIdPlaceOfIssue,
			ISNULL(mc.Abbr2, '') AS PrimaryCountryCodeOfIssue,
			ISNULL(ts.Abbr, '') AS PrimaryIdPlaceOfIssueCode,
			'SSN' AS SecondIdType,
			'US' AS SecondIdCountryOfIssue,
			ISNULL(tmc.Abbr3, '') AS CountryOfBirthAbbr3,
			ISNULL(tmc.Abbr2, '') AS CountryOfBirthAbbr2,
			c.GovtIdentification AS PrimaryIdNumber,
			c.SSN AS SecondIdNumber,
			c.CountryOfBirth,
			oc.Name AS Occupation,
			wa.PreferredCustomerAccountNumber,
			wa.PreferredCustomerLevelCode,
			wa.SmsNotificationFlag,
			wt.GrossTotalAmount,
			wt.AmountToReceiver,
			wt.DestinationPrincipalAmount,
			wt.Charges,
			wt.ExpectedPayoutStateCode,
			wt.ExpectedPayoutCityName,
			wt.DestinationCountryCode,
			wt.DestinationCurrencyCode,
			wt.ExchangeRate,
			wt.originating_city,
			wt.PromotionSequenceNo,
			wt.PromoCodeDescription,
			wt.PromoName,
			wt.PromotionDiscount,
			wt.PromoMessage,
			wt.originating_state,
			wt.OriginalDestinationCountryCode,
			wt.OriginalDestinationCurrencyCode,
			wt.Sender_ComplianceDetails_ComplianceData_Buffer,
			wt.Sender_unv_Buffer,
			wt.Receiver_unv_Buffer,
			wt.DeliveryOption,
			wt.DeliveryServiceName,
			wt.municipal_tax ,
			wt.state_tax ,
			wt.county_tax ,
			wt.plus_charges_amount ,
			wt.PrincipalAmount ,
			wt.PaySideCharges ,
			wt.PaySideTax,
			wt.DeliveryServiceDesc ,
			wt.PdsRequiredFlag ,
			wt.DfTransactionFlag, 
			wt.AvailableForPickup, 
			wt.AvailableForPickupEST ,
			wt.message_charge,
			wt.total_discount,
			wt.total_discounted_charges,
			wt.total_undiscounted_charges,
			wt.instant_notification_addl_service_charges,
			wt.ReferenceNo,
			wt.TestQuestion,
			wt.TestAnswer,
			wt.OriginatorsPrincipalAmount,
			wt.MoneyTransferKey,
			wt.Mtcn,
			wt.TempMTCN,
			wt.RecieverFirstName AS ReceiverFirstName,
			wt.RecieverLastName AS ReceiverLastName,
			wt.RecieverSecondLastName AS ReceiverSecondLastName
		FROM 
			tCustomers c
			INNER JOIN tCustomerSessions cs ON c.CustomerId = cs.CustomerId
			LEFT JOIN tNexxoIdTypes nt ON nt.NexxoIdTypeID = c.GovtIdTypeId
			LEFT JOIN tMasterCountries mc ON nt.MasterCountriesID= mc.MasterCountriesID
			INNER JOIN tMasterCountries tmc  ON tmc.Abbr2 = c.CountryOfBirth
			LEFT JOIN tWUnion_Account wa ON wa.CustomerId = c.CustomerId
			LEFT JOIN tWUnion_Trx wt ON wt.WUAccountId = wa.WUAccountId 
			--INNER JOIN tTxn_MoneyTransfer mt ON mt.CXNId = wt.WUTrxId
			LEFT JOIN tStates ts  ON ts.StateId = nt.StateId
			LEFT JOIN tOccupations oc  ON oc.Code = c.Occupation
		WHERE 
			cs.CustomerSessionID = @customerSessionId AND wt.WUTrxId = @wuTransactionId
		
	END TRY
	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO


 