--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger tTSys_Account_Delete>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  Trigger [dbo].[tTSys_Account_Delete]    Script Date: 4/7/2015 1:25:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[tTSys_Account_Delete] ON [dbo].[tTSys_Account] AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	insert tTSys_Account_Aud(TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTCreate,DTLastMod, RevisionNo, AuditEvent, DTAudit)
	SELECT i.TSysAccountPK, TSysAccountId, ProgramId,ExternalKey,CardNumber,UserId,AccountId,FirstName,MiddleName,LastName,
	DOB,SSN,Phone,PhoneType,Address1,Address2,City,State,ZipCode,Country,Activated,FraudScore,
	FraudResolution,DTCreate,DTLastMod, isnull(a.MaxRev,0) + 1, 3, GETDATE()
	from deleted i 
		left outer join (
			select TSysAccountPK, MAX(RevisionNo) as MaxRev
			from tTSys_Account_Aud
			group by TSysAccountPK
		) a on i.TSysAccountPK = a.TSysAccountPK
END
GO


