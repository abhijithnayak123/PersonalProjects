--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <03-14-2017>
-- Description:	 pupulate the data to audit table
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('tr_Customers_Aud') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tr_Customers_Aud
	 END
GO

CREATE TRIGGER tr_Customers_Aud ON tCustomers
AFTER INSERT, UPDATE, DELETE
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @revisionNo BIGINT
	DECLARE @auditEvent SMALLINT

	IF((SELECT COUNT(1) FROM INSERTED) > 0 AND (SELECT COUNT(1) FROM DELETED) > 0)
	BEGIN
		-- UPDATE
		SET @auditEvent = 2
	END
	ELSE
	IF((SELECT COUNT(1) FROM INSERTED) > 0)
	BEGIN
		-- INSERT
		SET @auditEvent = 1
	END
	ELSE
	IF((SELECT COUNT(1) FROM DELETED) > 0)
	BEGIN
		-- DELETE
		SET @auditEvent = 3
	END

	IF @AuditEvent != 3
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tCustomers_Aud tba
			  INNER JOIN INSERTED i ON i.CustomerID = tba.CustomerID
		END
	ELSE
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tCustomers_Aud tba
			  INNER JOIN DELETED d ON d.CustomerID = tba.CustomerID
		END

	IF @AuditEvent != 3
		BEGIN
			INSERT INTO tCustomers_Aud(
				CustomerID,
				FirstName, 
				MiddleName, 
				LastName, 
				LastName2, 
				MothersMaidenName, 
				DOB, 
				Address1, 
				Address2, 
				City, 
				[State], 
				ZipCode, 
				Phone1, 
				Phone1Type, 
				Phone1Provider, 
				Phone2, 
				Phone2Type, 
				Phone2Provider, 
				SSN, 
				TaxpayerId, 
				DoNotCall, 
				SMSEnabled, 
				MarketingSMSEnabled, 
				ChannelPartnerId, 
				DTTerminalCreate, 
				DTTerminalLastModified, 
				Gender, 
				Email, 
				PIN, 
				IsMailingAddressDifferent, 
				MailingAddress1, 
				MailingAddress2, 
				MailingCity, 
				MailingState, 
				MailingZipCode, 
				RevisionNo, 
				AuditEvent, 
				DTAudit, 
				DTServerCreate, 
				DTServerLastModified, 
				ReceiptLanguage, 
				ProfileStatus, 
				CountryOfBirth, 
				Notes, 
				ClientID, 
				LegalCode, 
				PrimaryCountryCitizenShip, 
				SecondaryCountryCitizenShip, 
				IDCode ,
				Occupation,	
				Employer,
				EmployerPhone,	
				OccupationDescription,
				GovtIdTypeId,	
				GovtIdentification,	
				GovtIDExpirationDate,
				GovtIdIssueDate,
				Group1,
				Group2,
				IsPartnerAccountHolder,
				ReferralCode,
				AgentSessionID, 
				LastUpdatedAgentSessionID
			)
			SELECT 
				CustomerID,
				FirstName,
				MiddleName, 
				LastName,
				LastName2,
				MothersMaidenName,
				DOB, 
				Address1,
				Address2,
				City, 
				State,
				ZipCode, 
				Phone1,
				Phone1Type,
				Phone1Provider, 
				Phone2, 
				Phone2Type, 
				Phone2Provider, 
				SSN,  
				TaxpayerId, 
				DoNotCall,
				SMSEnabled,
				MarketingSMSEnabled, 
				ChannelPartnerId,
				DTTerminalCreate,
				DTTerminalLastModified, 
				Gender, 
				Email, 
				PIN, 
				IsMailingAddressDifferent, 
				MailingAddress1, 
				MailingAddress2, 
				MailingCity, MailingState, 
				MailingZipCode, 
				@revisionNo,
				@auditEvent, 
				GETDATE(),
				DTServerCreate, 
				DTServerLastModified, 
				ReceiptLanguage, 
				ProfileStatus, CountryOfBirth, 
				Notes, 
				ClientID, 
				LegalCode, 
				PrimaryCountryCitizenShip, 
				SecondaryCountryCitizenShip, 
				IDCode,
				Occupation,
				Employer,
				EmployerPhone,
				OccupationDescription,
				GovtIdTypeId,
				GovtIdentification,
				GovtIDExpirationDate,
				GovtIdIssueDate,
				Group1,
				Group2,
				IsPartnerAccountHolder,
				ReferralCode, 
				AgentSessionID, 
				LastUpdatedAgentSessionID 				
			FROM 
				INSERTED
		END
	ELSE
		BEGIN
			INSERT INTO tCustomers_Aud(
				CustomerID,
				FirstName, 
				MiddleName, 
				LastName, 
				LastName2, 
				MothersMaidenName, 
				DOB, 
				Address1, 
				Address2, 
				City, 
				[State], 
				ZipCode, 
				Phone1, 
				Phone1Type, 
				Phone1Provider, 
				Phone2, 
				Phone2Type, 
				Phone2Provider, 
				SSN, 
				TaxpayerId, 
				DoNotCall, 
				SMSEnabled, 
				MarketingSMSEnabled, 
				ChannelPartnerId, 
				DTTerminalCreate, 
				DTTerminalLastModified, 
				Gender, 
				Email, 
				PIN, 
				IsMailingAddressDifferent, 
				MailingAddress1, 
				MailingAddress2, 
				MailingCity, 
				MailingState, 
				MailingZipCode, 
				RevisionNo, 
				AuditEvent, 
				DTAudit, 
				DTServerCreate, 
				DTServerLastModified, 
				ReceiptLanguage, 
				ProfileStatus, 
				CountryOfBirth, 
				Notes, 
				ClientID, 
				LegalCode, 
				PrimaryCountryCitizenShip, 
				SecondaryCountryCitizenShip, 
				IDCode ,
				Occupation,	
				Employer,
				EmployerPhone,	
				OccupationDescription,
				GovtIdTypeId,	
				GovtIdentification,	
				GovtIDExpirationDate,
				GovtIdIssueDate,
				Group1,
				Group2,
				IsPartnerAccountHolder,
				ReferralCode,
				AgentSessionID, 
				LastUpdatedAgentSessionID
			)
			SELECT 
				CustomerID,
				FirstName,
				MiddleName, 
				LastName,
				LastName2,
				MothersMaidenName,
				DOB, 
				Address1,
				Address2,
				City, 
				State,
				ZipCode, 
				Phone1,
				Phone1Type,
				Phone1Provider, 
				Phone2, 
				Phone2Type, 
				Phone2Provider, 
				SSN,  
				TaxpayerId, 
				DoNotCall,
				SMSEnabled,
				MarketingSMSEnabled, 
				ChannelPartnerId,
				DTTerminalCreate,
				DTTerminalLastModified, 
				Gender, 
				Email, 
				PIN, 
				IsMailingAddressDifferent, 
				MailingAddress1, 
				MailingAddress2, 
				MailingCity, MailingState, 
				MailingZipCode, 
				@revisionNo,
				@auditEvent, 
				GETDATE(),
				DTServerCreate, 
				DTServerLastModified, 
				ReceiptLanguage, 
				ProfileStatus, CountryOfBirth, 
				Notes, 
				ClientID, 
				LegalCode, 
				PrimaryCountryCitizenShip, 
				SecondaryCountryCitizenShip, 
				IDCode,
				Occupation,
				Employer,
				EmployerPhone,
				OccupationDescription,
				GovtIdTypeId,
				GovtIdentification,
				GovtIDExpirationDate,
				GovtIdIssueDate,
				Group1,
				Group2,
				IsPartnerAccountHolder,
				ReferralCode, 
				AgentSessionID, 
				LastUpdatedAgentSessionID 	
			FROM 
				DELETED
		END
END
GO

-- Drop Old triggers
IF OBJECT_ID('tCustomers_delete') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tCustomers_delete
	 END
GO

IF OBJECT_ID('tCustomers_insert') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tCustomers_insert
	 END
GO

IF OBJECT_ID('tCustomers_update') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tCustomers_update
	 END
GO
IF OBJECT_ID('tr_Customers_Delete') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tr_Customers_Delete
	 END
GO

IF OBJECT_ID('tr_Customers_Insert') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tr_Customers_Insert
	 END
GO

IF OBJECT_ID('tr_Customers_Update') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tr_Customers_Update
	 END
GO
