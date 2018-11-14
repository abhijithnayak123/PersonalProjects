-- ================================================================================
-- Author:		Kaushik Sakala
-- Create date: 07/27/2015
-- Description:	As an engineer, I want to implement ADO.Net for Customer module
-- Jira ID:		AL-7630

/* 

EXEC usp_CreateCustomer 
1000000029,'John','','Martin','','Mom','10/10/1990', --DOB
'Addr1','Addr2','Acton','CA','53501','6504754017','Home','AT&T','','','', --PhoneProvider
'12452166','0',0,0,0,34,'MALE','','1234', --PIN
0,'ADDRESS','','LOSANGELES','CA','90009',NULL,1,'10/10/2020', --DTServerCreate
'US',NULL,'00000330162','U','US','US',NULL, --IDCode
'00024',NULL, NULL, NULL,'PASSPORT','434535', --GovtIdentification
'10/10/2020', '10/10/2010',NULL, NULL,0,NULL,'10/10/2016', --DTTerminalCreate
1000000000,0,'UNITED STATES',''

*/
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

CREATE PROCEDURE usp_CreateCustomer
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
		INNER JOIN tNexxoIdTypes n ON n.NexxoIdTypePK = cp.NexxoIdTypeId
		INNER JOIN tMasterCountries mc ON mc.MasterCountriesPK = n.CountryPK
		INNER JOIN tChannelPartners c ON c.ChannelPartnerPK = cp.ChannelPartnerId
		LEFT JOIN tStates s ON s.StatePK = n.StatePK
	WHERE 
		n.IsActive = 1 AND c.ChannelPartnerId = @ChannelPartnerId 
		AND MC.Name = @IdIssueCountry AND n.Name = @GovtIdTypeId
		AND ISNULL(S.Name,'') = ISNULL(@IdIssueState,'')  -- Abhijith - When Idtype is selected as - 'Passport', we do not be having StateId.
		
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
           ([CustomerPK]
           ,[CustomerID]
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
           (NEWID()
           ,@CustomerID
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
           
	SELECT @CustomerID AS CustomerID
	
	END TRY
	BEGIN CATCH	        
	  -- Execute error retrieval routine.  
	 EXECUTE usp_CreateErrorInfo;  
	END CATCH
END