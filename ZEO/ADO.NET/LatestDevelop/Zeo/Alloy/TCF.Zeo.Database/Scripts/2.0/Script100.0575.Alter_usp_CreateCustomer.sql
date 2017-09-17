-- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <26/06/2017>
-- Description:	<made changes to apply group promo(THREETHENFREE) automatically>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_CreateCustomer'
)
BEGIN
	DROP PROCEDURE usp_CreateCustomer
END
GO

CREATE PROCEDURE [dbo].[usp_CreateCustomer]
           @CustomerID bigint,
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
           @MailingZipCode nvarchar(255)= null,
           @ReceiptLanguage varchar(50),
           @ProfileStatus smallint,
           @DTServerCreate datetime,
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
           @DTTerminalCreate datetime,
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
		AND MC.Name = @IdIssueCountry AND n.Name = @GovtIdTypeId
		AND (ISNULL(S.Name,'') = ISNULL(@IdIssueState,'')  -- Abhijith - When Idtype is selected as - 'Passport', we do not be having StateId.
			OR ISNULL(S.Abbr,'') = ISNULL(@IdIssueState,''))
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

	INSERT INTO [tCustomers]
           ([CustomerID]
           ,[DTTerminalCreate]
           ,[FirstName]
           ,[MiddleName]
           ,[LastName]
           ,[LastName2]
           ,[MothersMaidenName]
           ,[DOB]
           ,[Address1]
           ,[Address2]
           ,[City]
           ,[State]
           ,[ZipCode]
           ,[Phone1]
           ,[Phone1Type]
           ,[Phone1Provider]
           ,[Phone2]
           ,[Phone2Type]
           ,[Phone2Provider]
           ,[SSN]
           ,[TaxpayerId]
           ,[DoNotCall]
           ,[SMSEnabled]
           ,[MarketingSMSEnabled]
           ,[ChannelPartnerId]
           ,[Gender]
           ,[Email]
           ,[PIN]
           ,[IsMailingAddressDifferent]
           ,[MailingAddress1]
           ,[MailingAddress2]
           ,[MailingCity]
           ,[MailingState]
           ,[MailingZipCode]
           ,[ReceiptLanguage]
           ,[ProfileStatus]
           ,[DTServerCreate]
           ,[CountryOfBirth]
           ,[Notes]
           ,[ClientID]
           ,[LegalCode]
           ,[PrimaryCountryCitizenShip]
           ,[SecondaryCountryCitizenShip]
           ,[IDCode]
           ,[Occupation]
           ,[Employer]
           ,[EmployerPhone]
           ,[OccupationDescription]
           ,[GovtIdTypeId]
           ,[GovtIdentification]
           ,[GovtIDExpirationDate]
           ,[GovtIdIssueDate]
           ,[Group1]
           ,[Group2]
           ,[IsPartnerAccountHolder]
           ,[ReferralCode]
           ,[AgentSessionID]
           ,[LastUpdatedAgentSessionID])
     VALUES
           (@CustomerID
           ,@DTTerminalCreate
           ,@FirstName
           ,@MiddleName
           ,@LastName
           ,@LastName2
           ,@MothersMaidenName
           ,@DOB
           ,@Address1
           ,@Address2
           ,@City
           ,@State
           ,@ZipCode
           ,@Phone1
           ,@Phone1Type
           ,@Phone1Provider
           ,@Phone2
           ,@Phone2Type
           ,@Phone2Provider
           ,@SSN
           ,@TaxpayerId
           ,@DoNotCall
           ,@SMSEnabled
           ,@MarketingSMSEnabled
           ,@ChannelPartnerId
           ,@Gender
           ,@Email
           ,@PIN
           ,@IsMailingAddressDifferent
           ,@MailingAddress1
           ,@MailingAddress2
           ,@MailingCity
           ,@MailingState
           ,@MailingZipCode
           ,@ReceiptLanguage
           ,@ProfileStatus
           ,@DTServerCreate
           ,@CountryOfBirth
           ,@Notes
           ,@ClientID
           ,@LegalCode
           ,@PrimaryCountryCitizenShip
           ,@SecondaryCountryCitizenShip
           ,@IDCode
           ,@Occupation
           ,@Employer
           ,@EmployerPhone
           ,@OccupationDescription
           ,@nexxoIdTypeID
           ,@GovtIdentification 
           ,@GovtIDExpirationDate
           ,@GovtIdIssueDate
           ,@GroupId1
           ,@GroupId2
           ,@IsPartnerAccountHolder
           ,@ReferralCode
           ,@AgentSessionID 
           ,@LastUpdatedAgentSessionID)
           

	-- Below code is only for the promotion 'THREETHENFREE', that should be removed once the promotion is completed 
    DECLARE @autoPromoId INT = NULL

	SELECT @autoPromoId = ChannelPartnerGroupId from tChannelPartnerGroups where Name = 'THREETHENFREE' AND @DTServerCreate BETWEEN DTStart AND DTEnd

	IF(@autoPromoId IS NOT NULL)
	BEGIN
	        IF (@GroupId1 IS NULL AND @GroupId2 != @autoPromoId )
			BEGIN
			    UPDATE tCustomers set Group1 = @autoPromoId where CustomerId = @CustomerID			    
			END
			ELSE IF(@GroupId2 IS NULL AND @GroupId1 != @autoPromoId)
			BEGIN
				UPDATE tCustomers set Group2 = @autoPromoId where CustomerId = @CustomerID
		    END

	END
	-- -------------------------------End---------------------------------------------------


	SELECT @CustomerID AS CustomerID
	
	END TRY
	BEGIN CATCH	        
	  -- Execute error retrieval routine.  
	 EXECUTE usp_CreateErrorInfo;  
	END CATCH
END



