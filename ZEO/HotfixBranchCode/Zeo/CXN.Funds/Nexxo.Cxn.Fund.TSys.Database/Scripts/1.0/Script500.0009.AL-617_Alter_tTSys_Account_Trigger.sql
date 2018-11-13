-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <08/04/2015>
-- Description:	<Alter Trigger date columns to DTTerminalCreate, DTTerminalLastModified>
-- Jira ID:		<AL-617>
-- ================================================================================

IF EXISTS( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'tTSys_Account_Insert_Update'
)
BEGIN
	DROP TRIGGER [dbo].[tTSys_Account_Insert_Update]
	END
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
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastModified)
	SELECT i.TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE(),DTServerCreate,DTServerLastModified
	from inserted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO

IF EXISTS( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'tTSys_Account_Delete'
)
BEGIN
	DROP TRIGGER [dbo].[tTSys_Account_Delete]
	END
GO

CREATE TRIGGER [dbo].[tTSys_Account_Delete] ON [dbo].[tTSys_Account] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tTSys_Account_Aud(TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastModified)
	SELECT i.TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, isnull(a.MaxRev,0) + 1, 3, GETDATE(),DTServerCreate,DTServerLastModified
	from deleted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO

