--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger trChxr_AccountAudit>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  Trigger [dbo].[trChxr_AccountAudit]    Script Date: 4/7/2015 10:33:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER trigger [dbo].[trChxr_AccountAudit] on [dbo].[tChxr_Account] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tChxr_Account_Aud where ChxrAccountID = (select ChxrAccountID from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tChxr_Account_Aud(
					 ChxrAccountPK,
                     ChxrAccountID,
                     Badge,
                     FirstName,
                     LastName,
                     ITIN,
                     SSN,
                     DateOfBirth,
                     Address1,
                     Address2,
                     City,
                     [State],
                     Zip,
                     Phone,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     IDCardType,
                     IDCardNumber,
                     IDCardIssuedCountry,
                     IDCardIssuedDate,
                     IDCardImage,
                     IDCardExpireDate,
                     CardNumber,
                     CustomerScore,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTCreate, DTLastMod,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tChxr_Account_Aud(
					ChxrAccountPK,
                     ChxrAccountID,
                     Badge,
                     FirstName,
                     LastName,
                     ITIN,
                     SSN,
                     DateOfBirth,
                     Address1,
                     Address2,
                     City,
                     [State],
                     Zip,
                     Phone,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     IDCardType,
                     IDCardNumber,
                     IDCardIssuedCountry,
                     IDCardIssuedDate,
                     IDCardImage,
                     IDCardExpireDate,
                     CardNumber,
                     CustomerScore,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTCreate, DTLastMod,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tChxr_Account_Aud(
					 ChxrAccountPK,
                    ChxrAccountID,
                     Badge,
                     FirstName,
                     LastName,
                     ITIN,
                     SSN,
                     DateOfBirth,
                     Address1,
                     Address2,
                     City,
                     [State],
                     Zip,
                     Phone,
                     Occupation,
                     Employer,
                     EmployerPhone,
                     IDCardType,
                     IDCardNumber,
                     IDCardIssuedCountry,
                     IDCardIssuedDate,
                     IDCardImage,
                     IDCardExpireDate,
                     CardNumber,
                     CustomerScore,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTCreate, DTLastMod,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
GO


