--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <04-10-2017>
-- Description:	 Modified the SP to get 'PrimaryCountryCitizenShipName & SecondaryCountryCitizenShipName
-- ================================================================================


ALTER PROCEDURE [dbo].[usp_GetCustomerByCustomerId]
	@CustomerID BIGINT
AS
BEGIN
	BEGIN TRY
		SELECT 
            C.CustomerID
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
           ,C.Email
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
		   ,VA.CardNumber
           ,TA.TcfCustInd AS CustInd,
		   pmc.Name AS PrimaryCountryCitizenShipName,
		   smc.Name AS SecondaryCountryCitizenShipName
    FROM 
		tCustomers C 
		LEFT JOIN tNexxoIdTypes NI ON NI.NexxoIdTypeID = C.GovtIdTypeId
		LEFT JOIN tMasterCountries M ON M.MasterCountriesID = NI.MasterCountriesID
		LEFT JOIN tStates S ON S.StateId = NI.StateId
		LEFT JOIN tChannelPartnerGroups CPG ON CPG.ChannelPartnerGroupId = C.Group1
		LEFT JOIN tChannelPartnerGroups CP ON CP.ChannelPartnerGroupId = C.Group2
		LEFT JOIN tTCIS_Account TA ON TA.CustomerID = C.CustomerID
		LEFT JOIN tVisa_Account VA ON VA.CustomerId = C.CustomerID  AND VA.Activated = 1
		LEFT JOIN tMasterCountries pmc ON C.PrimaryCountryCitizenShip = pmc.Abbr2
		LEFT JOIN tMasterCountries smc ON C.SecondaryCountryCitizenShip = smc.Abbr2
	WHERE
		C.CustomerID = @CustomerID
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END


