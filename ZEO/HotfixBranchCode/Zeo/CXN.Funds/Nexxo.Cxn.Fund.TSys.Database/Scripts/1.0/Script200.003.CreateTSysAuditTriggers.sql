CREATE TRIGGER [dbo].[tTSys_Account_Insert_Update] ON [dbo].[tTSys_Account] AFTER INSERT, UPDATE
AS
BEGIN
	declare @auditEvent int

	if not exists (select * from deleted)
		set @auditEvent = 1
	else
		set @auditEvent = 2

	SET NOCOUNT ON

	insert tTSys_Account_Aud(rowguid, Id, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTCreate,DTLastMod, RevisionNo, AuditEvent, DTAudit)
	SELECT i.rowguid, Id, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTCreate,DTLastMod, isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE()
	from inserted i 
		left outer join (
			select rowguid, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by rowguid
		) a on i.rowguid = a.rowguid
END
GO

CREATE TRIGGER [dbo].[tTSys_Account_Delete] ON [dbo].[tTSys_Account] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tTSys_Account_Aud(rowguid, Id, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTCreate,DTLastMod, RevisionNo, AuditEvent, DTAudit)
	SELECT i.rowguid, Id, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTCreate,DTLastMod, isnull(a.MaxRev,0) + 1, 3, GETDATE()
	from deleted i 
		left outer join (
			select rowguid, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by rowguid
		) a on i.rowguid = a.rowguid
END
GO

CREATE TRIGGER [dbo].[tTSys_Trx_Insert_Update] ON [dbo].[tTSys_Trx] AFTER INSERT, UPDATE
AS
BEGIN
	declare @auditEvent int

	if not exists (select * from deleted)
		set @auditEvent = 1
	else
		set @auditEvent = 2

	SET NOCOUNT ON

	insert tTSys_Trx_Aud(rowguid,Id,AccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTCreate,DTLastMod,ChannelPartnerID, DTServerCreate, DTServerLastMod,
	RevisionNo, AuditEvent, DTAudit)
	SELECT i.rowguid,Id,AccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTCreate,DTLastMod,ChannelPartnerID, DTServerCreate, DTServerLastMod,
	isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE()
	from inserted i 
		left outer join (
			select rowguid, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by rowguid
		) a on i.rowguid = a.rowguid
END
GO

CREATE TRIGGER [dbo].[tTSys_Trx_Delete] ON [dbo].[tTSys_Trx] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tTSys_Trx_Aud(rowguid,Id,AccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTCreate,DTLastMod,ChannelPartnerID, DTServerCreate, DTServerLastMod,
	RevisionNo, AuditEvent, DTAudit)
	SELECT i.rowguid,Id,AccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTCreate,DTLastMod,ChannelPartnerID, DTServerCreate, DTServerLastMod,
	isnull(a.MaxRev,0) + 1, 3, GETDATE()
	from deleted i 
		left outer join (
			select rowguid, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by rowguid
		) a on i.rowguid = a.rowguid
END
GO
