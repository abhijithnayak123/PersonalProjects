--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger [tCustomers_delete]>           
-- Jira ID:	<AL-243>
--===========================================================================================

/****** Object:  Trigger [dbo].[tCustomers_delete]    Script Date: 3/30/2015 3:32:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[tCustomers_delete] ON [dbo].[tCustomers] AFTER DELETE
AS
	SET NOCOUNT ON	
	INSERT INTO tCustomers_Aud(CustomerPK,CustomerID, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastMod)
	SELECT d.CustomerPK, d.CustomerID, d.FirstName, d.MiddleName, d.LastName, d.LastName2, d.MothersMaidenName, d.DOB, d.Address1, d.Address2, d.City, d.State, d.ZipCode, d.Phone1, d.Phone1Type, d.Phone1Provider, d.Phone2, d.Phone2Type, d.Phone2Provider, d.SSN, d.TaxpayerId, d.DoNotCall, d.SMSEnabled, d.MarketingSMSEnabled, d.ChannelPartnerId, d.DTCreate, d.DTLastMod, d.Gender, d.Email, d.PIN, d.IsMailingAddressDifferent, d.MailingAddress1, d.MailingAddress2, d.MailingCity, d.MailingState, d.MailingZipCode, ISNULL(a.MaxRev,0) + 1, 3, GETDATE(),d.DTServerCreate,d.DTServerLastMod
	FROM deleted d
	LEFT OUTER JOIN (
		SELECT CustomerPK, MAX(RevisionNo) AS MaxRev
		FROM tCustomers_Aud
		GROUP BY CustomerPK
	) a ON d.CustomerPK = a.CustomerPK
GO


