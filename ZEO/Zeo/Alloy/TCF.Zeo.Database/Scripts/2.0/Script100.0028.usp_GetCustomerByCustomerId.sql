-- ================================================================================
-- Author:		Kaushik Sakal
-- Create date: 07/27/2015
-- Description:	As an engineer, I want to implement ADO.Net for Customer module
-- Jira ID:		AL-7630

--EXEC usp_GetCustomerByCustomerId 1000000000000010
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetCustomerByCustomerId'
)
BEGIN
	DROP PROCEDURE usp_GetCustomerByCustomerId
END
GO

CREATE PROCEDURE usp_GetCustomerByCustomerId
	@CustomerID BIGINT
AS
BEGIN
	BEGIN TRY
		SELECT 
            CustomerPK
           ,C.CustomerID
           ,C.FirstName
           ,C.MiddleName
           ,C.LastName
           ,C.LastName2
           ,C.MothersMaidenName
           ,C.DOB
           ,C.Address1
           ,C.Address2
           ,C.City
           ,C.State
           ,C.IDCode
           ,C.ZipCode
           ,C.Phone1
           ,Phone1Type
           ,Phone1Provider
           ,C.Phone2
           ,Phone2Type
           ,Phone2Provider
           ,C.SSN
           ,TaxpayerId
           ,DoNotCall
           ,SMSEnabled
           ,C.ChannelPartnerId
           ,C.Gender
           ,Email
           ,PIN
           ,IsMailingAddressDifferent
           ,MailingAddress1
           ,MailingAddress2
           ,MailingCity
           ,MailingState
           ,MailingZipCode
           ,ReceiptLanguage
           ,C.ProfileStatus
           ,c.DTServerCreate
           ,CountryOfBirth
           ,Notes
           ,ClientID
           ,LegalCode
           ,PrimaryCountryCitizenShip
           ,SecondaryCountryCitizenShip
           ,C.IDCode
           ,Occupation
           ,Employer
           ,EmployerPhone
           ,OccupationDescription
           ,NI.Name AS IdName
           ,S.Name AS IdState
		   ,S.Abbr AS IdStateAbbr
           ,m.Name AS IdCountry
           ,GovtIdentification
           ,GovtIDExpirationDate
           ,GovtIdIssueDate
           ,CPG.Name AS Group1
           ,CP.Name AS Group2
           ,IsPartnerAccountHolder
           ,ReferralCode
           ,AgentSessionID
           ,LastUpdatedAgentSessionID
           ,TA.ProfileStatus AS ClientProfileStatus
    FROM 
		tCustomers C 
		LEFT JOIN tNexxoIdTypes NI ON NI.NexxoIdTypeID = C.GovtIdTypeId
		LEFT JOIN tMasterCountries M ON M.MasterCountriesPK = NI.CountryPK
		LEFT JOIN tStates S ON S.StatePK = NI.StatePK
		LEFT JOIN tChannelPartnerGroups CPG ON CPG.ChannelPartnerGroupId = C.Group1
		LEFT JOIN tChannelPartnerGroups CP ON CP.ChannelPartnerGroupId = C.Group2
		LEFT JOIN tTCIS_Account TA ON TA.CustomerID = C.CustomerID
	WHERE
		C.CustomerID = @CustomerID 
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END