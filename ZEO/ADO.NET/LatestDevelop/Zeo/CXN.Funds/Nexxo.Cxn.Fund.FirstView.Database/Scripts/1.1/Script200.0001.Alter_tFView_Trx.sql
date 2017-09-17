ALTER TABLE tFView_Trx ADD
DTServerCreate DateTime,DTServerLastMod DateTime
GO

ALTER TABLE tFView_Trx_Aud ADD
DTServerCreate DateTime,DTServerLastMod DateTime
GO

ALTER trigger [dbo].[trFView_TrxAudit] on [dbo].[tFView_Trx] AFTER Insert, Update, Delete
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
                     DTAudit,
                     ChannelPartnerID,
                     DTServerCreate,
                     DTServerLastMod
                     )
              select  rowguid, Id, AccountPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,2 as AuditEvent,GETDATE(),
					ChannelPartnerID,DTServerCreate,DTServerLastMod from inserted
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
                     DTAudit,
					 ChannelPartnerID,
					 DTServerCreate,
                     DTServerLastMod
                     )
              select rowguid, Id, AccountPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,1 as AuditEvent,GETDATE(),
					ChannelPartnerID,DTServerCreate,DTServerLastMod from inserted
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
                     DTAudit,
					 ChannelPartnerID,
					 DTServerCreate,
					 DTServerLastMod
                     )
              select rowguid, Id, AccountPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,3 as AuditEvent,GETDATE(),
					ChannelPartnerID,DTServerCreate,DTServerLastMod from deleted
       end
GO