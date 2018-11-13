-- Author:		<Anisha Abraham>
-- Create date: <11/05/2015>
-- Description:	<As Alloy, I need a flag to differentiate between SSN and ITIN>
-- Jira ID:		<AL-1619>
-- ================================================================================


DROP TRIGGER [dbo].[tCustomers_insert]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[tCustomers_insert] ON [dbo].[tCustomers] AFTER INSERT
AS
	SET NOCOUNT ON	
	INSERT INTO tCustomers_Aud(CustomerPK,CustomerID, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTTerminalCreate, DTTerminalLastModified, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit, DTServerCreate, DTServerLastModified, ReceiptLanguage, ProfileStatus, CountryOfBirth, Notes, ClientID, LegalCode, PrimaryCountryCitizenShip, SecondaryCountryCitizenShip, IDCode)
	SELECT d.CustomerPK, d.CustomerID, d.FirstName, d.MiddleName, d.LastName, d.LastName2, d.MothersMaidenName, d.DOB, d.Address1, d.Address2, d.City, d.State, d.ZipCode, d.Phone1, d.Phone1Type, d.Phone1Provider, d.Phone2, d.Phone2Type, d.Phone2Provider, d.SSN,  d.TaxpayerId, d.DoNotCall, d.SMSEnabled, d.MarketingSMSEnabled, d.ChannelPartnerId, d.DTTerminalCreate, d.DTTerminalLastModified, d.Gender, d.Email, d.PIN, d.IsMailingAddressDifferent, d.MailingAddress1, d.MailingAddress2, d.MailingCity, d.MailingState, d.MailingZipCode, ISNULL(a.MaxRev,0) + 1, 3, GETDATE(), d.DTServerCreate, d.DTServerLastModified, d.ReceiptLanguage, d.ProfileStatus, d.CountryOfBirth, d.Notes, d.ClientID, d.LegalCode, d.PrimaryCountryCitizenShip, d.SecondaryCountryCitizenShip, d.IDCode
	FROM inserted d
	LEFT OUTER JOIN
	(
		SELECT CustomerPK, MAX(RevisionNo) AS MaxRev
		FROM tCustomers_Aud
		GROUP BY CustomerPK
	) a ON d.CustomerPK = a.CustomerPK

GO