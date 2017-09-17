-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <10/08/2015>
-- Description:	<As engineering team member, I want to have consistent format for date time values across all tables.>
-- Jira ID:		<AL-617>
-- ================================================================================


-- trChxr_AccountAudit


DROP TRIGGER [dbo].[trChxr_AccountAudit]
	
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
					 DTTerminalLastModified
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified,@RevisionNo,2 as AuditEvent,GETDATE(), DTTerminalCreate, DTTerminalLastModified from inserted
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
					 DTTerminalLastModified
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified,@RevisionNo,1 as AuditEvent,GETDATE(), 
					 DTTerminalCreate,
					 DTTerminalLastModified from inserted
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
					 DTTerminalLastModified
                     )
              select ChxrAccountPK, ChxrAccountID, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTServerCreate, DTServerLastModified,@RevisionNo,3 as AuditEvent,GETDATE(), DTTerminalCreate, DTTerminalLastModified from deleted
       end