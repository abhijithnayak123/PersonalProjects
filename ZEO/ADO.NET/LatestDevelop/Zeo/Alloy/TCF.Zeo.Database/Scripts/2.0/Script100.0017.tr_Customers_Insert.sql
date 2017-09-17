IF  EXISTS (
	SELECT * 
	FROM sys.triggers 
	WHERE object_id = OBJECT_ID(N'[dbo].[tr_Customers_Insert]')
)
DROP TRIGGER [dbo].[tr_Customers_Insert]
GO


CREATE TRIGGER [dbo].[tr_Customers_Insert] ON [dbo].[tCustomers] AFTER INSERT  --Audit event 3
		AS
			SET NOCOUNT ON	
			INSERT INTO tCustomers_Aud(
				CustomerID, FirstName, MiddleName, 
				LastName, LastName2, MothersMaidenName, DOB, 
				Address1, Address2, City, [State], ZipCode, 
				Phone1, Phone1Type, Phone1Provider, Phone2, 
				Phone2Type, Phone2Provider, SSN, TaxpayerId, 
				DoNotCall, SMSEnabled, MarketingSMSEnabled, 
				ChannelPartnerId, DTTerminalCreate, DTTerminalLastModified, 
				Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, 
				MailingAddress2, MailingCity, MailingState, MailingZipCode, 
				RevisionNo, AuditEvent, DTAudit, DTServerCreate, DTServerLastModified, 
				ReceiptLanguage, ProfileStatus, CountryOfBirth, Notes, ClientID, LegalCode, 
				PrimaryCountryCitizenShip, SecondaryCountryCitizenShip, IDCode ,
				Occupation,	Employer,EmployerPhone,	OccupationDescription,GovtIdTypeId,	
				GovtIdentification,	GovtIDExpirationDate,GovtIdIssueDate 
			)
			SELECT 
				d.CustomerID, d.FirstName, d.MiddleName, 
				d.LastName, d.LastName2, d.MothersMaidenName, d.DOB, 
				d.Address1, d.Address2, d.City, d.State, d.ZipCode, 
				d.Phone1, d.Phone1Type, d.Phone1Provider, d.Phone2, 
				d.Phone2Type, d.Phone2Provider, d.SSN,  d.TaxpayerId, 
				d.DoNotCall, d.SMSEnabled, d.MarketingSMSEnabled, 
				d.ChannelPartnerId, d.DTTerminalCreate, d.DTTerminalLastModified, 
				d.Gender, d.Email, d.PIN, d.IsMailingAddressDifferent, d.MailingAddress1, 
				d.MailingAddress2, d.MailingCity, d.MailingState, d.MailingZipCode, 
				ISNULL(a.MaxRev,0) + 1, 3, GETDATE(), d.DTServerCreate, d.DTServerLastModified, 
				d.ReceiptLanguage, d.ProfileStatus, d.CountryOfBirth, d.Notes, d.ClientID, 
				d.LegalCode, d.PrimaryCountryCitizenShip, d.SecondaryCountryCitizenShip, 
				d.IDCode,d.Occupation,d.Employer,d.EmployerPhone,d.OccupationDescription,d.GovtIdTypeId,
				d.GovtIdentification,d.GovtIDExpirationDate,d.GovtIdIssueDate 
			FROM 
				inserted d
			LEFT OUTER JOIN
			(
				SELECT 
					CustomerID, MAX(RevisionNo) AS MaxRev
				FROM 
					tCustomers_Aud
				GROUP BY 
					CustomerID
			) a ON d.CustomerID = a.CustomerID

GO


