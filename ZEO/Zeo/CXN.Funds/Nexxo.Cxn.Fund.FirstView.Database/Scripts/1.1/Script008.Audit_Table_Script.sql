ALTER TABLE dbo.tFView_Trx_Aud
DROP COLUMN LogId

GO

ALTER TABLE dbo.tFView_Trx_Aud
ADD RevisionNo BIGINT NULL
Go
ALTER TABLE dbo.tFView_Trx_Aud
ADD PreviousCardBalance money NULL
Go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trFView_TrxAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trFView_TrxAudit]
GO
create trigger trFView_TrxAudit on dbo.tFView_Trx AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tFView_Trx_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tFView_Trx_Aud(
					 rowguid,
                     Id,
                     AccountPK,
                     ProcessorId,
                     PrimaryAccountNumber,
                     TransactionType,
                     TransactionAmount,
                     CardAcceptorIdCode,
                     CardAcceptorTerminalID,
                     CardAcceptorBusinessCode,
                     TransactionDescription,
                     MessageTypeIdentifier,
                     TransactionCurrencyCode,
                     DTLocalTransaction,
                     DTTransmission,
                     CreditPlanMaster,
                     AccountNumber,
                     TransactionID,
                     CardBalance,
                     ErrorCode,
                     ErrorMsg,
                     CardStatus,
                     ActivationRequired,
                     DTCreate,
                     DTLastMod,
                     PreviousCardBalance,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select  rowguid, Id, AccountPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tFView_Trx_Aud(
					rowguid,
                     Id,
                     AccountPK,
                     ProcessorId,
                     PrimaryAccountNumber,
                     TransactionType,
                     TransactionAmount,
                     CardAcceptorIdCode,
                     CardAcceptorTerminalID,
                     CardAcceptorBusinessCode,
                     TransactionDescription,
                     MessageTypeIdentifier,
                     TransactionCurrencyCode,
                     DTLocalTransaction,
                     DTTransmission,
                     CreditPlanMaster,
                     AccountNumber,
                     TransactionID,
                     CardBalance,
                     ErrorCode,
                     ErrorMsg,
                     CardStatus,
                     ActivationRequired,
                     DTCreate,
                     DTLastMod,
                     PreviousCardBalance,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select rowguid, Id, AccountPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tFView_Trx_Aud(
					 rowguid,
                     Id,
                     AccountPK,
                     ProcessorId,
                     PrimaryAccountNumber,
                     TransactionType,
                     TransactionAmount,
                     CardAcceptorIdCode,
                     CardAcceptorTerminalID,
                     CardAcceptorBusinessCode,
                     TransactionDescription,
                     MessageTypeIdentifier,
                     TransactionCurrencyCode,
                     DTLocalTransaction,
                     DTTransmission,
                     CreditPlanMaster,
                     AccountNumber,
                     TransactionID,
                     CardBalance,
                     ErrorCode,
                     ErrorMsg,
                     CardStatus,
                     ActivationRequired,
                     DTCreate,
                     DTLastMod,
                     PreviousCardBalance,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select rowguid, Id, AccountPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 
GO


ALTER TABLE dbo.tFView_Card_Aud
DROP COLUMN LogId

GO

ALTER TABLE dbo.tFView_Card_Aud
ADD RevisionNo BIGINT NULL

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trFView_CardAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trFView_CardAudit]
GO
create trigger trFView_CardAudit on dbo.tFView_Card AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tFView_Card_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tFView_Card_Aud(
					 rowguid,
                     Id,
                     CardNumber,
                     AccountNumber,
                     BSAccountNumber,
                     NameAsOnCard,
                     FirstName,
                     MiddleName,
                     LastName,
                     DateOfBirth,
                     SSNNumber,
                     GovernmentID,
                     IDNumber,
                     GovtIdExpirationDate,
                     GovtIDIssueCountry,
                     GovtIDIssueDate,
                     GovtIDIssueState,
                     AddressLine1,
                     AddressLine2,
                     City,
                     [State],
                     PostalCode,
                     HomePhoneNumber,
                     ShippingContactName,
                     ShippingAddressLine1,
                     ShippingAddressLine2,
                     ShippingCity,
                     ShippingState,
                     ShippingZipCode,
                     ExpiryDate,
                     IsActive,
                     DTActivated,
                     ActivatedBy,
                     DTDeactivated,
                     DeactivatedBy,
                     DeactivatedReason,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select  rowguid, Id, CardNumber, AccountNumber, BSAccountNumber, NameAsOnCard, FirstName,  MiddleName, LastName, DateOfBirth, SSNNumber, GovernmentID, IDNumber, GovtIdExpirationDate, GovtIDIssueCountry, GovtIDIssueDate, GovtIDIssueState, AddressLine1, AddressLine2, City, [State], PostalCode, HomePhoneNumber, ShippingContactName, ShippingAddressLine1, ShippingAddressLine2, ShippingCity, ShippingState, ShippingZipCode, ExpiryDate, IsActive, DTActivated, ActivatedBy, DTDeactivated, DeactivatedBy, DeactivatedReason, DTCreate, DTLastMod,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tFView_Card_Aud(
					rowguid,
                     Id,
                     CardNumber,
                     AccountNumber,
                     BSAccountNumber,
                     NameAsOnCard,
                     FirstName,
                     MiddleName,
                     LastName,
                     DateOfBirth,
                     SSNNumber,
                     GovernmentID,
                     IDNumber,
                     GovtIdExpirationDate,
                     GovtIDIssueCountry,
                     GovtIDIssueDate,
                     GovtIDIssueState,
                     AddressLine1,
                     AddressLine2,
                     City,
                     [State],
                     PostalCode,
                     HomePhoneNumber,
                     ShippingContactName,
                     ShippingAddressLine1,
                     ShippingAddressLine2,
                     ShippingCity,
                     ShippingState,
                     ShippingZipCode,
                     ExpiryDate,
                     IsActive,
                     DTActivated,
                     ActivatedBy,
                     DTDeactivated,
                     DeactivatedBy,
                     DeactivatedReason,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select rowguid, Id, CardNumber, AccountNumber, BSAccountNumber, NameAsOnCard, FirstName,  MiddleName, LastName, DateOfBirth, SSNNumber, GovernmentID, IDNumber, GovtIdExpirationDate, GovtIDIssueCountry, GovtIDIssueDate, GovtIDIssueState, AddressLine1, AddressLine2, City, [State], PostalCode, HomePhoneNumber, ShippingContactName, ShippingAddressLine1, ShippingAddressLine2, ShippingCity, ShippingState, ShippingZipCode, ExpiryDate, IsActive, DTActivated, ActivatedBy, DTDeactivated, DeactivatedBy, DeactivatedReason, DTCreate, DTLastMod,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tFView_Card_Aud(
					 rowguid,
                     Id,
                     CardNumber,
                     AccountNumber,
                     BSAccountNumber,
                     NameAsOnCard,
                     FirstName,
                     MiddleName,
                     LastName,
                     DateOfBirth,
                     SSNNumber,
                     GovernmentID,
                     IDNumber,
                     GovtIdExpirationDate,
                     GovtIDIssueCountry,
                     GovtIDIssueDate,
                     GovtIDIssueState,
                     AddressLine1,
                     AddressLine2,
                     City,
                     [State],
                     PostalCode,
                     HomePhoneNumber,
                     ShippingContactName,
                     ShippingAddressLine1,
                     ShippingAddressLine2,
                     ShippingCity,
                     ShippingState,
                     ShippingZipCode,
                     ExpiryDate,
                     IsActive,
                     DTActivated,
                     ActivatedBy,
                     DTDeactivated,
                     DeactivatedBy,
                     DeactivatedReason,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select rowguid, Id, CardNumber, AccountNumber, BSAccountNumber, NameAsOnCard, FirstName,  MiddleName, LastName, DateOfBirth, SSNNumber, GovernmentID, IDNumber, GovtIdExpirationDate, GovtIDIssueCountry, GovtIDIssueDate, GovtIDIssueState, AddressLine1, AddressLine2, City, [State], PostalCode, HomePhoneNumber, ShippingContactName, ShippingAddressLine1, ShippingAddressLine2, ShippingCity, ShippingState, ShippingZipCode, ExpiryDate, IsActive, DTActivated, ActivatedBy, DTDeactivated, DeactivatedBy, DeactivatedReason, DTCreate, DTLastMod,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 
GO