-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter views date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

-- trFView_CardAudit
DROP TRIGGER [dbo].[trFView_CardAudit]
GO

CREATE trigger [dbo].[trFView_CardAudit] on [dbo].[tFView_Card] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tFView_Card_Aud where FViewCardID = (select FViewCardID from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tFView_Card_Aud(
					 FViewCardPK,
                     FViewCardId,
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
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select  FViewCardPK, FViewCardId, CardNumber, AccountNumber, BSAccountNumber, NameAsOnCard, FirstName,  MiddleName, LastName, DateOfBirth, SSNNumber, GovernmentID, IDNumber, GovtIdExpirationDate, GovtIDIssueCountry, GovtIDIssueDate, GovtIDIssueState, AddressLine1, AddressLine2, City, [State], PostalCode, HomePhoneNumber, ShippingContactName, ShippingAddressLine1, ShippingAddressLine2, ShippingCity, ShippingState, ShippingZipCode, ExpiryDate, IsActive, DTActivated, ActivatedBy, DTDeactivated, DeactivatedBy, DeactivatedReason, DTTerminalCreate, DTTerminalLastModified,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tFView_Card_Aud(
					FViewCardPK,
                     FViewCardId,
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
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select FViewCardPK, FViewCardId, CardNumber, AccountNumber, BSAccountNumber, NameAsOnCard, FirstName,  MiddleName, LastName, DateOfBirth, SSNNumber, GovernmentID, IDNumber, GovtIdExpirationDate, GovtIDIssueCountry, GovtIDIssueDate, GovtIDIssueState, AddressLine1, AddressLine2, City, [State], PostalCode, HomePhoneNumber, ShippingContactName, ShippingAddressLine1, ShippingAddressLine2, ShippingCity, ShippingState, ShippingZipCode, ExpiryDate, IsActive, DTActivated, ActivatedBy, DTDeactivated, DeactivatedBy, DeactivatedReason, DTTerminalCreate, DTTerminalLastModified,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tFView_Card_Aud(
					 FViewCardPK,
                     FViewCardId,
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
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     RevisionNo,
                     AuditEvent,
                     DTAudit
                     )
              select FViewCardPK, FViewCardId, CardNumber, AccountNumber, BSAccountNumber, NameAsOnCard, FirstName,  MiddleName, LastName, DateOfBirth, SSNNumber, GovernmentID, IDNumber, GovtIdExpirationDate, GovtIDIssueCountry, GovtIDIssueDate, GovtIDIssueState, AddressLine1, AddressLine2, City, [State], PostalCode, HomePhoneNumber, ShippingContactName, ShippingAddressLine1, ShippingAddressLine2, ShippingCity, ShippingState, ShippingZipCode, ExpiryDate, IsActive, DTActivated, ActivatedBy, DTDeactivated, DeactivatedBy, DeactivatedReason, DTTerminalCreate, DTTerminalLastModified,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
GO

-- trFView_TrxAudit
DROP TRIGGER [dbo].[trFView_TrxAudit]
GO

CREATE trigger [dbo].[trFView_TrxAudit] on [dbo].[tFView_Trx] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tFView_Trx_Aud where FViewTrxId = (select FViewTrxId from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tFView_Trx_Aud(
					 FViewTrxPK,
                     FViewTrxId,
                     FViewCardPK,
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
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     PreviousCardBalance,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     ChannelPartnerID,
                     DTServerCreate,
                     DTServerLastModified
                     )
              select  FViewTrxPK, FViewTrxId, FViewCardPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTTerminalCreate, DTTerminalLastModified, PreviousCardBalance,@RevisionNo,2 as AuditEvent,GETDATE(),
					ChannelPartnerID,DTServerCreate,DTServerLastModified from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tFView_Trx_Aud(
					FViewTrxPK,
                     FViewTrxId,
                     FViewCardPK,
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
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     PreviousCardBalance,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
					 ChannelPartnerID,
					 DTServerCreate,
                     DTServerLastModified
                     )
              select FViewTrxPK, FViewTrxId, FViewCardPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTTerminalCreate, DTTerminalLastModified, PreviousCardBalance,@RevisionNo,1 as AuditEvent,GETDATE(),
					ChannelPartnerID,DTServerCreate,DTServerLastModified from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tFView_Trx_Aud(
					 FViewTrxPK,
                     FViewTrxId,
                     FViewCardPK,
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
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     PreviousCardBalance,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
					 ChannelPartnerID,
					 DTServerCreate,
					 DTServerLastModified
                     )
              select FViewTrxPK, FViewTrxId, FViewCardPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTTerminalCreate, DTTerminalLastModified, PreviousCardBalance,@RevisionNo,3 as AuditEvent,GETDATE(),
					ChannelPartnerID,DTServerCreate,DTServerLastModified from deleted
       end
GO


