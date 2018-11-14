-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <11/05/2015>
-- Description:	<As Alloy, I need a flag to differentiate between SSN and ITIN>
-- Jira ID:		<AL-1619>
-- ================================================================================
DROP TRIGGER [dbo].[tTSys_Account_Insert_Update]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, RevisionNo, AuditEvent, DTAudit,DTServerCreate,DTServerLastModified,IDCode)
	SELECT i.TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTTerminalCreate,DTTerminalLastModified, isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE(),DTServerCreate,DTServerLastModified,IDCode
	from inserted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO
