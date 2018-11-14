-- ================================================================================
-- Author:		Kaushik Sakala
-- Create date: 07/02/2017
-- Description:	As an engineer, I want to implement ADO.Net for Customer module
-- Jira ID:		AL-7630
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_UpdateCustomer'
)
BEGIN
	DROP PROCEDURE usp_UpdateCustomer
END
GO

CREATE PROCEDURE usp_UpdateCustomer
           @CustomerID bigint,
           @DTTerminalLastModified datetime,
           @FirstName nvarchar(255),
           @MiddleName nvarchar(255),
           @LastName nvarchar(255),
           @LastName2 nvarchar(255),
           @MothersMaidenName nvarchar(255) = null,
           @DOB datetime,
           @Address1 nvarchar(255) = null,
           @Address2 nvarchar(255) = null,
           @City nvarchar(255) = null,
           @State nvarchar(255) = null,
           @ZipCode nvarchar(255) = null,
           @Phone1 nvarchar(255) = null,
           @Phone1Type nvarchar(255) = null,
           @Phone1Provider nvarchar(255) = null,
           @Phone2 nvarchar(255) = null,
           @Phone2Type nvarchar(255) = null,
           @Phone2Provider nvarchar(255) = null,
           @SSN nvarchar(255),
           @TaxpayerId nvarchar(255) = null,
           @DoNotCall bit,
           @SMSEnabled bit,
           @MarketingSMSEnabled bit = 0,
           @ChannelPartnerId bigint,
           @Gender nvarchar(6),
           @Email nvarchar(320),
           @PIN nvarchar(4),
           @IsMailingAddressDifferent bit,
           @MailingAddress1 nvarchar(255) = null,
           @MailingAddress2 nvarchar(255) = null,
           @MailingCity nvarchar(255) = null,
           @MailingState nvarchar(255) = null,
           @MailingZipCode nvarchar(255) = null,
           @ReceiptLanguage varchar(50),
           @ProfileStatus smallint,
           @DTServerLastModified datetime,
           @CountryOfBirth varchar(5),
           @Notes varchar(250),
           @ClientID varchar(15),
           @LegalCode char(1),
           @PrimaryCountryCitizenShip varchar(5),
           @SecondaryCountryCitizenShip varchar(5),
           @IDCode varchar(2),
           @Occupation nvarchar(255),
           @Employer nvarchar(255),
           @EmployerPhone nvarchar(255),
           @OccupationDescription nvarchar(255),
           @GovtIdTypeId VARCHAR(200),
           @GovtIdentification nvarchar(255),
           @GovtIDExpirationDate date,
           @GovtIdIssueDate date,
           @Group1 NVARCHAR(100),
           @Group2 NVARCHAR(100),
           @IsPartnerAccountHolder bit,
           @ReferralCode nvarchar(16),
           @AgentSessionID bigint,
           @LastUpdatedAgentSessionID bigint,
           @IdIssueCountry VARCHAR(200),
           @IdIssueState VARCHAR(100) = NULL	
AS
BEGIN
	BEGIN TRY
	
	DECLARE @nexxoIdTypeID BIGINT,
			@GroupId1 int,
			@GroupId2 int
	
	SELECT 
		@nexxoIdTypeID = n.NexxoIdTypeID
	FROM 
		tChannelPartnerIDTypeMapping cp 
		INNER JOIN tNexxoIdTypes n ON n.NexxoIdTypeID = cp.NexxoIdTypeId
		INNER JOIN tMasterCountries mc ON mc.MasterCountriesID = n.MasterCountriesID
		INNER JOIN tChannelPartners c ON c.ChannelPartnerId = cp.ChannelPartnerId
		LEFT JOIN tStates s ON s.StateId = n.StateId
	WHERE 
		n.IsActive = 1 AND c.ChannelPartnerId = @ChannelPartnerId 
		AND MC.Name = @IdIssueCountry 
		AND (ISNULL(S.Name,'') = ISNULL(@IdIssueState,'')  -- Abhijith - When Idtype is selected as - 'Passport', we do not be having StateId.
			OR ISNULL(S.Abbr,'') = ISNULL(@IdIssueState,'')) -- Abhijith - This OR condition added because on Cust. Reg. In "@IdIssueState" we will get State Name and on
																-- On SyncIn we will get State Abbr. So added a a OR condition to filter the IdType for both the conditions.
		AND n.Name = @GovtIdTypeId
		
	SELECT 
		@GroupId1 = ChannelPartnerGroupId
	FROM 
		tChannelPartnerGroups
	WHERE 
		NAME = @Group1
		
	SELECT 
		@GroupId2 = ChannelPartnerGroupId
	FROM 
		tChannelPartnerGroups
	WHERE 
		NAME = @Group2
	
	UPDATE 
		tCustomers
	SET
			DTTerminalLastModified = @DTTerminalLastModified
           ,FirstName = @FirstName
           ,MiddleName = @MiddleName
           ,LastName = @LastName
           ,LastName2 = @LastName2
           ,MothersMaidenName = @MothersMaidenName
           ,DOB = @DOB
           ,Address1 = @Address1
           ,Address2 = @Address2
           ,City = @City
           ,State = @State
           ,ZipCode = @ZipCode
           ,Phone1 = @Phone1
           ,Phone1Type = @Phone1Type
           ,Phone1Provider = @Phone1Provider
           ,Phone2 = @Phone2
           ,Phone2Type = @Phone2Type
           ,Phone2Provider = @Phone2Provider
           ,SSN = @SSN
           ,TaxpayerId = @TaxpayerId
           ,DoNotCall = @DoNotCall
           ,SMSEnabled = @SMSEnabled
           ,MarketingSMSEnabled = @MarketingSMSEnabled
           ,ChannelPartnerId = @ChannelPartnerId
           ,Gender = @Gender
           ,Email = @Email
           ,PIN = @PIN
           ,IsMailingAddressDifferent = @IsMailingAddressDifferent
           ,MailingAddress1 = @MailingAddress1
           ,MailingAddress2 = @MailingAddress2
           ,MailingCity = @MailingCity
           ,MailingState = @MailingState
           ,MailingZipCode = @MailingZipCode
           ,ReceiptLanguage = @ReceiptLanguage
           ,ProfileStatus = @ProfileStatus
           ,DTServerLastModified = @DTServerLastModified
           ,CountryOfBirth = @CountryOfBirth
           ,Notes = @Notes
           ,ClientID = @ClientID
           ,LegalCode = @LegalCode
           ,PrimaryCountryCitizenShip = @PrimaryCountryCitizenShip
           ,SecondaryCountryCitizenShip = @SecondaryCountryCitizenShip
           ,IDCode = @IDCode
           ,Occupation = @Occupation
           ,Employer = @Employer
           ,EmployerPhone = @EmployerPhone
           ,OccupationDescription = @OccupationDescription
           ,GovtIdTypeId = @nexxoIdTypeID
           ,GovtIdentification = @GovtIdentification
           ,GovtIDExpirationDate = @GovtIDExpirationDate
           ,GovtIdIssueDate = @GovtIdIssueDate
           ,Group1 = @GroupId1
           ,Group2 = @GroupId2
           ,IsPartnerAccountHolder = @IsPartnerAccountHolder
           ,ReferralCode = @ReferralCode
           ,LastUpdatedAgentSessionID = @LastUpdatedAgentSessionID
         WHERE
           CustomerID = @CustomerID

	 DECLARE @autoPromoId int

	 DECLARE @isRCIFSuccess bit 

	 SELECT @isRCIFSuccess = IsRCIFSuccess FROM tCustomers WHERE CustomerID = @CustomerID 

	 SELECT @autoPromoId = ChannelPartnerGroupId from tChannelPartnerGroups where Name = 'THREETHENFREE' AND @DTServerLastModified BETWEEN DTStart AND DTEnd


	 IF(@autoPromoId IS NOT NULL AND @isRCIFSuccess = 0)
		 BEGIN
				IF (@GroupId1 IS NULL AND @GroupId2 IS NOT NULL AND @GroupId2 != @autoPromoId )
				BEGIN
					 UPDATE tCustomers set Group1 = @autoPromoId where CustomerId = @CustomerID			    
				END
				ELSE IF(@GroupId2 IS NULL AND @GroupId1 IS NOT NULL AND @GroupId1 != @autoPromoId )
				BEGIN
					 UPDATE tCustomers set Group2 = @autoPromoId where CustomerId = @CustomerID
				END
				ELSE IF (@GroupId1 IS NULL AND @GroupId2 IS NULL)
				BEGIN
					 UPDATE tCustomers set Group1 = @autoPromoId where CustomerId = @CustomerID 
				END
		 END

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END