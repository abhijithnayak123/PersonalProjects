--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger>           
-- Jira ID:	<AL-243>
--===========================================================================================

/****** Object:  Trigger [dbo].[tCustomers_update]    Script Date: 3/30/2015 3:37:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[tCustomers_update] ON [dbo].[tCustomers] AFTER UPDATE
AS
	SET NOCOUNT ON	
	INSERT INTO tCustomers_Aud(CustomerPK, CustomerId, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastMod)
	SELECT i.CustomerPK, i.CustomerId, i.FirstName, i.MiddleName, i.LastName, i.LastName2, i.MothersMaidenName, i.DOB, i.Address1, i.Address2, i.City, i.State, i.ZipCode, i.Phone1, i.Phone1Type, i.Phone1Provider, i.Phone2, i.Phone2Type, i.Phone2Provider, i.SSN, i.TaxpayerId, i.DoNotCall, i.SMSEnabled, i.MarketingSMSEnabled, i.ChannelPartnerId, i.DTCreate, i.DTLastMod, i.Gender, i.Email, i.PIN, i.IsMailingAddressDifferent, i.MailingAddress1, i.MailingAddress2, i.MailingCity, i.MailingState, i.MailingZipCode, ISNULL(a.MaxRev,0) + 1, 2, GETDATE(),i.DTServerCreate,i.DTServerLastMod
	FROM inserted i
	LEFT OUTER JOIN (
		SELECT CustomerPK, MAX(RevisionNo) AS MaxRev
		FROM tCustomers_Aud
		GROUP BY CustomerPK
	) a ON i.CustomerPK = a.CustomerPK
GO


