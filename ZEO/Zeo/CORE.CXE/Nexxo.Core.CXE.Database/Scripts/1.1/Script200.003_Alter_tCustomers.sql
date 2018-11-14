ALTER TABLE tCustomers ADD
DTServerCreate DateTime,DTServerLastMod DateTime
GO

ALTER TABLE tCustomers_Aud ADD
DTServerCreate DateTime,DTServerLastMod DateTime
GO

ALTER TRIGGER [dbo].[tCustomers_delete] on [dbo].[tCustomers] AFTER DELETE
AS
	SET NOCOUNT ON	
	insert into tCustomers_Aud(rowguid, Id, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastMod)
	select d.rowguid, d.Id, d.FirstName, d.MiddleName, d.LastName, d.LastName2, d.MothersMaidenName, d.DOB, d.Address1, d.Address2, d.City, d.State, d.ZipCode, d.Phone1, d.Phone1Type, d.Phone1Provider, d.Phone2, d.Phone2Type, d.Phone2Provider, d.SSN, d.TaxpayerId, d.DoNotCall, d.SMSEnabled, d.MarketingSMSEnabled, d.ChannelPartnerId, d.DTCreate, d.DTLastMod, d.Gender, d.Email, d.PIN, d.IsMailingAddressDifferent, d.MailingAddress1, d.MailingAddress2, d.MailingCity, d.MailingState, d.MailingZipCode, isnull(a.MaxRev,0) + 1, 3, GETDATE(),d.DTServerCreate,d.DTServerLastMod
	from deleted d
	left outer join (
		select rowguid, MAX(RevisionNo) as MaxRev
		from tCustomers_Aud
		group by rowguid
	) a on d.rowguid = a.rowguid
GO

ALTER TRIGGER [dbo].[tCustomers_insert] on [dbo].[tCustomers] AFTER INSERT
AS
	SET NOCOUNT ON           
	insert into tCustomers_Aud(rowguid, Id, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, State, ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastMod)
	select i.rowguid, i.Id, i.FirstName, i.MiddleName, i.LastName, i.LastName2, i.MothersMaidenName, i.DOB, i.Address1, i.Address2, i.City, i.State, i.ZipCode, i.Phone1, i.Phone1Type, i.Phone1Provider, i.Phone2, i.Phone2Type, i.Phone2Provider, i.SSN, i.TaxpayerId, i.DoNotCall, i.SMSEnabled, i.MarketingSMSEnabled, i.ChannelPartnerId, i.DTCreate, i.DTLastMod, i.Gender, i.Email, i.PIN, i.IsMailingAddressDifferent, i.MailingAddress1, i.MailingAddress2, i.MailingCity, i.MailingState, i.MailingZipCode, isnull(a.MaxRev,0) + 1, 1, GETDATE(),i.DTServerCreate,i.DTServerLastMod
	from inserted i 
	left outer join (
		select rowguid, MAX(RevisionNo) as MaxRev
		from tCustomers_Aud
		group by rowguid
	) a on i.rowguid = a.rowguid
GO

ALTER TRIGGER [dbo].[tCustomers_update] on [dbo].[tCustomers] AFTER UPDATE
AS
	SET NOCOUNT ON	
	insert into tCustomers_Aud(rowguid, Id, FirstName, MiddleName, LastName, LastName2, MothersMaidenName, DOB, Address1, Address2, City, [State], ZipCode, Phone1, Phone1Type, Phone1Provider, Phone2, Phone2Type, Phone2Provider, SSN, TaxpayerId, DoNotCall, SMSEnabled, MarketingSMSEnabled, ChannelPartnerId, DTCreate, DTLastMod, Gender, Email, PIN, IsMailingAddressDifferent, MailingAddress1, MailingAddress2, MailingCity, MailingState, MailingZipCode, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastMod)
	select i.rowguid, i.Id, i.FirstName, i.MiddleName, i.LastName, i.LastName2, i.MothersMaidenName, i.DOB, i.Address1, i.Address2, i.City, i.State, i.ZipCode, i.Phone1, i.Phone1Type, i.Phone1Provider, i.Phone2, i.Phone2Type, i.Phone2Provider, i.SSN, i.TaxpayerId, i.DoNotCall, i.SMSEnabled, i.MarketingSMSEnabled, i.ChannelPartnerId, i.DTCreate, i.DTLastMod, i.Gender, i.Email, i.PIN, i.IsMailingAddressDifferent, i.MailingAddress1, i.MailingAddress2, i.MailingCity, i.MailingState, i.MailingZipCode, isnull(a.MaxRev,0) + 1, 2, GETDATE(),i.DTServerCreate,i.DTServerLastMod
	from inserted i
	left outer join (
		select rowguid, MAX(RevisionNo) as MaxRev
		from tCustomers_Aud
		group by rowguid
	) a on i.rowguid = a.rowguid