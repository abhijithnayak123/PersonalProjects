-- ============================================================
-- Author:		Manikandan Govindraj
-- Create date: <09/24/2015>
-- Description:	<Script for Altering tCustomers_insert trigger>
-- Jira ID:		<AL-2018>
-- ============================================================

IF EXISTS
( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'tCustomers_insert'
)
BEGIN
	DROP TRIGGER [dbo].[tCustomers_insert]
END
GO

CREATE TRIGGER [dbo].[tCustomers_insert] ON [dbo].[tCustomers] AFTER INSERT
AS
	SET NOCOUNT ON           
	INSERT INTO tCustomers_Aud(CustomerPK, CustomerId, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, State, ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTTerminalCreate, DTTerminalLastModified, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit, DTServerCreate, DTServerLastModified, ReceiptLanguage, ProfileStatus, CountryOfBirth, Notes, ClientID, LegalCode, PrimaryCountryCitizenShip, SecondaryCountryCitizenShip)
	SELECT i.CustomerPK, i.CustomerId, i.FirstName, i.MiddleName, i.LastName, i.LastName2, i.MothersMaidenName, i.DOB, i.Address1, i.Address2, i.City, i.State, i.ZipCode, i.Phone1, i.Phone1Type, i.Phone1Provider, i.Phone2, i.Phone2Type, i.Phone2Provider, i.SSN, i.TaxpayerId, i.DoNotCall, i.SMSEnabled, i.MarketingSMSEnabled, i.ChannelPartnerId, i.DTTerminalCreate, i.DTTerminalLastModified, i.Gender, i.Email, i.PIN, i.IsMailingAddressDifferent, i.MailingAddress1, i.MailingAddress2, i.MailingCity, i.MailingState, i.MailingZipCode, ISNULL(a.MaxRev,0) + 1, 1, GETDATE(), i.DTServerCreate, i.DTServerLastModified, i.ReceiptLanguage, i.ProfileStatus, i.CountryOfBirth, i.Notes, i.ClientID, i.LegalCode, i.PrimaryCountryCitizenShip, i.SecondaryCountryCitizenShip
	FROM inserted i 
	LEFT OUTER JOIN
	(
		SELECT CustomerPK, MAX(RevisionNo) AS MaxRev
		FROM tCustomers_Aud
		GROUP BY CustomerPK
	) a ON i.CustomerPK = a.CustomerPK
GO


