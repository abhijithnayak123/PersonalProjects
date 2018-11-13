ALTER TABLE [dbo].[tFView_Trx] ADD ChannelPartnerID bigint
GO

ALTER TABLE [dbo].tFView_Trx_Aud ADD  ChannelPartnerID bigint
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
                     ChannelPartnerID
                     )
              select  rowguid, Id, AccountPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,2 as AuditEvent,GETDATE(),
					ChannelPartnerID from inserted
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
					ChannelPartnerID
                     )
              select rowguid, Id, AccountPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,1 as AuditEvent,GETDATE(),
					ChannelPartnerID from inserted
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
					ChannelPartnerID
                     )
              select rowguid, Id, AccountPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,3 as AuditEvent,GETDATE(),
					ChannelPartnerID from deleted
       end
GO

DECLARE @partner_id bigint 
DECLARE @myCursor CURSOR
SET @myCursor = CURSOR FOR 
        SELECT ID FROM tFView_Trx
OPEN @myCursor
	FETCH NEXT FROM @myCursor 
	INTO @partner_id 
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		Update tFView_Trx set ChannelPartnerId=27
		 where current of @myCursor
		FETCH NEXT FROM @myCursor
		INTO @partner_id    
END 
CLOSE @myCursor
DEALLOCATE @myCursor
GO

ALTER TABLE [dbo].[tFView_Trx] ALTER COLUMN ChannelPartnerID bigint NOT NULL
GO

