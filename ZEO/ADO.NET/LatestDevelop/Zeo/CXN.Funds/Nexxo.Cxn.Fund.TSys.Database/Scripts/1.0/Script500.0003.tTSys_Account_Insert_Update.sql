--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger tTSys_Account_Insert_Update>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  Trigger [dbo].[tTSys_Account_Insert_Update]    Script Date: 4/7/2015 1:26:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[tTSys_Account_Insert_Update] ON [dbo].[tTSys_Account] AFTER INSERT, UPDATE
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
	FraudResolution,DTCreate,DTLastMod, RevisionNo, AuditEvent, DTAudit)
	SELECT i.TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTCreate,DTLastMod, isnull(a.MaxRev,0) + 1, @auditEvent, GETDATE()
	from inserted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO


