-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter table date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================


-- tTSys_Account_Delete
DROP TRIGGER [dbo].[tTSys_Account_Delete]
GO

CREATE TRIGGER [dbo].[tTSys_Account_Delete] ON [dbo].[tTSys_Account] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tTSys_Account_Aud(TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, RevisionNo, AuditEvent, DTAudit)
	SELECT i.TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, isnull(a.MaxRev,0) + 1, 3, GETDATE()
	from deleted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO


DROP TRIGGER [dbo].[tTSys_Account_Insert_Update]
GO

CREATE TRIGGER [dbo].[tTSys_Account_Insert_Update] ON [dbo].[tTSys_Account] AFTER INSERT, UPDATE
AS
BEGIN
	declare @auditEvent int

	if not exists (select * from deleted)
		set @auditEvent = 1
	else
		set @auditEvent = 2

	SET NOCOUNT ON

	insert tTSys_Account_Aud(TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, RevisionNo, AuditEvent, DTAudit)
	SELECT i.TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE()
	from inserted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO

-- tTSys_Trx_Delete
DROP TRIGGER [dbo].[tTSys_Trx_Delete]
GO

CREATE TRIGGER [dbo].[tTSys_Trx_Delete] ON [dbo].[tTSys_Trx] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tTSys_Trx_Aud(TSysTrxPK,TSysTrxId,TSysAccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTTerminalCreate,DTTerminalLastModified,ChannelPartnerID, DTServerCreate, DTServerLastModified,
	RevisionNo, AuditEvent, DTAudit)
	SELECT i.TSysTrxPK,TSysTrxId,i.TSysAccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTTerminalCreate,DTTerminalLastModified,ChannelPartnerID, DTServerCreate, DTServerLastModified,
	isnull(a.MaxRev,0) + 1, 3, GETDATE()
	from deleted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO

DROP TRIGGER [dbo].[tTSys_Trx_Insert_Update]
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

	insert tTSys_Trx_Aud(TSysTrxPK,TSysTrxId,TSysAccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTTerminalCreate,DTTerminalLastModified,ChannelPartnerID, DTServerCreate, DTServerLastModified,
	RevisionNo, AuditEvent, DTAudit)
	SELECT i.TSysTrxPK,TSysTrxId,i.TSysAccountPK,TransactionType,Amount,Fee,Description,DTLocalTransaction,DTTransmission,Status,
	ErrorCode,ErrorMsg,ConfirmationId,Balance,DTTerminalCreate,DTTerminalLastModified,ChannelPartnerID, DTServerCreate, DTServerLastModified,
	isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE()
	from inserted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO



