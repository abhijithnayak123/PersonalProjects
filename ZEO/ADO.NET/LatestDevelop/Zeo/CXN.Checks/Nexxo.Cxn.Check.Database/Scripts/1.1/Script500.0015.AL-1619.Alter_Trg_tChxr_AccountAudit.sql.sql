-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <11/05/2015>
-- Description:	<As Alloy, I need a flag to differentiate between SSN and ITIN>
-- Jira ID:		<AL-1619>
-- ================================================================================

DROP TRIGGER [dbo].[trChxr_AccountAudit]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE trigger [dbo].[trChxr_AccountAudit] on [dbo].[tChxr_Account] AFTER Insert, Update, Delete
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
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
					 DTTerminalCreate,
					 DTTerminalLastModified,
					 IDCode
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified,@RevisionNo,2 as AuditEvent,GETDATE(), DTTerminalCreate, DTTerminalLastModified,IDCode from inserted
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
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
					 DTTerminalCreate,
					 DTTerminalLastModified,
					 IDCode
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified,@RevisionNo,1 as AuditEvent,GETDATE(), 
					 DTTerminalCreate,
					 DTTerminalLastModified,IDCode from inserted
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
                     DTServerCreate,
                     DTServerLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
					 DTTerminalCreate,
					 DTTerminalLastModified,
					 IDCode
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified,@RevisionNo,3 as AuditEvent,GETDATE(), DTTerminalCreate, DTTerminalLastModified,IDCode from deleted
       end
GO


