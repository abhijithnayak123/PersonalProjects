--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger trFView_TrxAudit>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  Trigger [dbo].[trFView_TrxAudit]    Script Date: 4/7/2015 1:08:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER trigger [dbo].[trFView_TrxAudit] on [dbo].[tFView_Trx] AFTER Insert, Update, Delete
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
              select  FViewTrxPK, FViewTrxId, FViewCardPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,2 as AuditEvent,GETDATE(),
					ChannelPartnerID,DTServerCreate,DTServerLastMod from inserted
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
              select FViewTrxPK, FViewTrxId, FViewCardPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,1 as AuditEvent,GETDATE(),
					ChannelPartnerID,DTServerCreate,DTServerLastMod from inserted
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
              select FViewTrxPK, FViewTrxId, FViewCardPK, ProcessorId, PrimaryAccountNumber, TransactionType, TransactionAmount, CardAcceptorIdCode, CardAcceptorTerminalID, CardAcceptorBusinessCode, TransactionDescription, MessageTypeIdentifier, TransactionCurrencyCode, DTLocalTransaction, DTTransmission, CreditPlanMaster, AccountNumber, TransactionID, CardBalance, ErrorCode, ErrorMsg, CardStatus, ActivationRequired, DTCreate, DTLastMod, PreviousCardBalance,@RevisionNo,3 as AuditEvent,GETDATE(),
					ChannelPartnerID,DTServerCreate,DTServerLastMod from deleted
       end
GO


