ALTER TABLE dbo.tChxr_Account_Aud
DROP COLUMN LogId

GO

ALTER TABLE dbo.tChxr_Account_Aud
ADD RevisionNo BIGINT NULL


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trChxr_AccountAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trChxr_AccountAudit]
GO
create trigger trChxr_AccountAudit on dbo.tChxr_Account AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tChxr_Account_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tChxr_Account_Aud(
					 rowguid,
                     Id,
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
              select rowguid, Id, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTCreate, DTLastMod,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tChxr_Account_Aud(
					rowguid,
                     Id,
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
              select rowguid, Id, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTCreate, DTLastMod,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tChxr_Account_Aud(
					 rowguid,
                     Id,
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
              select rowguid, Id, Badge, FirstName, LastName, ITIN, SSN, DateOfBirth, Address1, Address2, City, [State], Zip, Phone, Occupation, Employer, EmployerPhone, IDCardType, IDCardNumber, IDCardIssuedCountry, IDCardIssuedDate, IDCardImage, IDCardExpireDate, CardNumber, CustomerScore, DTCreate, DTLastMod,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 
GO


ALTER TABLE dbo.tChxr_Trx_Aud
DROP COLUMN LogId

GO

ALTER TABLE dbo.tChxr_Trx_Aud
ADD RevisionNo BIGINT NULL
GO
ALTER TABLE dbo.tChxr_Trx_Aud
ADD SubmitType int NULL
GO
ALTER TABLE dbo.tChxr_Trx_Aud
ADD ReturnType int NULL

GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trChxr_TrxAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trChxr_TrxAudit]
GO
create trigger trChxr_TrxAudit on dbo.tChxr_Trx AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tChxr_Trx_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tChxr_Trx_Aud(
					 rowguid,
                     Id,
                     Amount,
                     ChexarAmount,
                     ChexarFee,
                     CheckDate,
                     CheckNumber,
                     RoutingNumber,
                     AccountNumber,
                     Micr,
                     Latitude,
                     Longitude,
                     InvoiceId,
                     TicketId,
                     WaitTime,
                     [Status],
                     ChexarStatus,
                     DeclineCode,
                     [Message],
                     Location,
                     ChxrAccountPK,
                     DTCreate,
                     DTLastMod,
                     SubmitType,
                     ReturnType,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select rowguid, Id, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tChxr_Trx_Aud(
					rowguid,
                     Id,
                     Amount,
                     ChexarAmount,
                     ChexarFee,
                     CheckDate,
                     CheckNumber,
                     RoutingNumber,
                     AccountNumber,
                     Micr,
                     Latitude,
                     Longitude,
                     InvoiceId,
                     TicketId,
                     WaitTime,
                     [Status],
                     ChexarStatus,
                     DeclineCode,
                     [Message],
                     Location,
                     ChxrAccountPK,
                     DTCreate,
                     DTLastMod,
                     SubmitType,
                     ReturnType,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select rowguid, Id, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tChxr_Trx_Aud(
					 rowguid,
                     Id,
                     Amount,
                     ChexarAmount,
                     ChexarFee,
                     CheckDate,
                     CheckNumber,
                     RoutingNumber,
                     AccountNumber,
                     Micr,
                     Latitude,
                     Longitude,
                     InvoiceId,
                     TicketId,
                     WaitTime,
                     [Status],
                     ChexarStatus,
                     DeclineCode,
                     [Message],
                     Location,
                     ChxrAccountPK,
                     DTCreate,
                     DTLastMod,
                     SubmitType,
                     ReturnType,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select rowguid, Id, Amount, ChexarAmount, ChexarFee, CheckDate, CheckNumber, RoutingNumber, AccountNumber, Micr, Latitude, Longitude, InvoiceId, TicketId, WaitTime, [Status], ChexarStatus, DeclineCode, [Message], Location, ChxrAccountPK, DTCreate, DTLastMod, SubmitType, ReturnType,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 
GO
