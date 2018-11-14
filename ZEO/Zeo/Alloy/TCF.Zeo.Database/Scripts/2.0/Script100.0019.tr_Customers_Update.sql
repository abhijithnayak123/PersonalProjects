IF  EXISTS (
	SELECT * 
	FROM sys.triggers 
	WHERE object_id = OBJECT_ID(N'[dbo].[tr_Customers_Update]')
)
DROP TRIGGER [dbo].[tr_Customers_Update]
GO


CREATE TRIGGER [dbo].[tr_Customers_Update] ON [dbo].[tCustomers] AFTER UPDATE  --Audit event 2
		AS
			SET NOCOUNT ON	
		INSERT INTO tCustomers_Aud(
					CustomerId, FirstName, MiddleName, 
					LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, 
					City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, 
					Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled,
					MarketingSMSEnabled, ChannelPartnerId, DTTerminalCreate, DTTerminalLastModified, 
					Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, 
					MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit, 
					DTServerCreate, DTServerLastModified, ReceiptLanguage, ProfileStatus, 
					CountryOfBirth,Notes, ClientID, LegalCode, PrimaryCountryCitizenShip, 
					SecondaryCountryCitizenShip,IDCode,Occupation,Employer,EmployerPhone,		
					OccupationDescription,	GovtIdTypeId,GovtIdentification,GovtIDExpirationDate,	
					GovtIdIssueDate 
				)
		SELECT  i.CustomerId, i.FirstName, i.MiddleName, i.LastName, i.LastName2, 
				i.MothersMaidenName, i.DOB, i.Address1, i.Address2, i.City, i.State, i.ZipCode, 
				i.Phone1, i.Phone1Type, i.Phone1Provider, i.Phone2, i.Phone2Type, i.Phone2Provider, 
				i.SSN, i.TaxpayerId, i.DoNotCall, i.SMSEnabled, i.MarketingSMSEnabled, i.ChannelPartnerId, 
				i.DTTerminalCreate, i.DTTerminalLastModified, i.Gender, i.Email, i.PIN, 
				i.IsMailingAddressDifferent, i.MailingAddress1, i.MailingAddress2, i.MailingCity, 
				i.MailingState, i.MailingZipCode, ISNULL(a.MaxRev,0) + 1, 2, GETDATE(), i.DTServerCreate, 
				i.DTServerLastModified, i.ReceiptLanguage, i.ProfileStatus, i.CountryOfBirth, i.Notes, 
				i.ClientID, i.LegalCode, i.PrimaryCountryCitizenShip, i.SecondaryCountryCitizenShip, 
				i.IDCode,i.Occupation,	i.Employer,	i.EmployerPhone,		i.OccupationDescription,	
				i.GovtIdTypeId,	i.GovtIdentification,	i.GovtIDExpirationDate,	i.GovtIdIssueDate 
		FROM 
			inserted i
		LEFT OUTER JOIN
		(
			SELECT CustomerID, MAX(RevisionNo) AS MaxRev
			FROM tCustomers_Aud
			GROUP BY CustomerID
		) a ON i.CustomerID = a.CustomerID
GO


