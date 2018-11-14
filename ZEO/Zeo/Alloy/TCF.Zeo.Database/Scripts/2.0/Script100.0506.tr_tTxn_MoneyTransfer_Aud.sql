--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-09-2016>
-- Description:	 Alter Trigger for tTxn_MoneyTransfer audit table
-- Jira ID:		<AL-8324>
-- ================================================================================


IF OBJECT_ID(N'tr_tTxn_MoneyTransfer_Aud', N'TR') IS NOT NULL
DROP TRIGGER tr_tTxn_MoneyTransfer_Aud
GO


CREATE TRIGGER [dbo].[tr_tTxn_MoneyTransfer_Aud] ON tTxn_MoneyTransfer
AFTER INSERT, UPDATE, DELETE
AS
		SET NOCOUNT ON;
		DECLARE @RevisionNo BIGINT
		DECLARE @AuditEvent SMALLINT	
	
		IF ((SELECT COUNT(1) FROM INSERTED) > 0 AND (SELECT COUNT(1) FROM DELETED) > 0)
		BEGIN
			-- UPDATE
			SET @AuditEvent = 2
		END
		ELSE IF ((SELECT COUNT(1) FROM INSERTED) > 0)
		BEGIN
			-- INSERT
			SET @AuditEvent = 1
		END	 
		ELSE IF ((SELECT COUNT(1) FROM DELETED) > 0)
		BEGIN
			-- DELETE
			SET @AuditEvent = 3
		END     

		IF(@AuditEvent != 3)
		BEGIN
			
			SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tTxn_MoneyTransfer_Aud ma
			  INNER JOIN INSERTED i ON ma.TransactionId = i.TransactionId

            INSERT INTO [dbo].[tTxn_MoneyTransfer_Aud]
			  (
			   [TransactionID]
			   ,[CustomerSessionId]
			   ,[CustomerRevisionNo]
			   ,[ProviderId]
			   ,[ProviderAccountId]
			   ,[Amount]
			   ,[Fee]
			   ,[Description]
			  -- ,[State]
			   ,[DTTerminalCreate]
			   ,[DTTerminalLastModified]
			   ,[ConfirmationNumber]
			   ,[RecipientId]
			   ,[ExchangeRate]
			   ,[DTServerCreate]
			   ,[DTServerLastModified]
			   ,[TransferType]
			   ,[Destination]
			   ,[TransactionSubType]
			   ,[OriginalTransactionID]
			   ,[DTAudit]
			   ,[AuditEvent]
			   ,[RevisionNo]
			  )
             SELECT 
				[TransactionID]
			   ,[CustomerSessionId]
			   ,[CustomerRevisionNo]
			   ,[ProviderId]
			   ,[ProviderAccountId]
			   ,[Amount]
			   ,[Fee]
			   ,[Description]
			  -- ,[State]
			   ,[DTTerminalCreate]
			   ,[DTTerminalLastModified]
			   ,[ConfirmationNumber]
			   ,[RecipientId]
			   ,[ExchangeRate]
			   ,[DTServerCreate]
			   ,[DTServerLastModified]
			   ,[TransferType]
			   ,[Destination]
			   ,[TransactionSubType]
			   ,[OriginalTransactionID]
			   ,GETDATE() 
			   ,@AuditEvent
			   ,@RevisionNo
			FROM INSERTED

       END
		ELSE
		BEGIN

			SELECT 
			  @RevisionNo = ISNULL(MAX(tma.RevisionNo), 0) + 1 
			FROM 
			  tTxn_MoneyTransfer_Aud tma
			INNER JOIN DELETED d
			ON 
			  d.TransactionID = tma.TransactionId

			 INSERT INTO [dbo].[tTxn_MoneyTransfer_Aud]
			  (
			   [TransactionID]
			   ,[CustomerSessionId]
			   ,[CustomerRevisionNo]
			   ,[ProviderId]
			   ,[ProviderAccountId]
			   ,[Amount]
			   ,[Fee]
			   ,[Description]
			   --,[State]
			   ,[DTTerminalCreate]
			   ,[DTTerminalLastModified]
			   ,[ConfirmationNumber]
			   ,[RecipientId]
			   ,[ExchangeRate]
			   ,[DTServerCreate]
			   ,[DTServerLastModified]
			   ,[TransferType]
			   ,[Destination]
			   ,[TransactionSubType]
			   ,[OriginalTransactionID]
			   ,[DTAudit]
			   ,[AuditEvent]
			   ,[RevisionNo]
			  )
            SELECT 
				[TransactionID]
			   ,[CustomerSessionId]
			   ,[CustomerRevisionNo]
			   ,[ProviderId]
			   ,[ProviderAccountId]
			   ,[Amount]
			   ,[Fee]
			   ,[Description]
			   --,[State]
			   ,[DTTerminalCreate]
			   ,[DTTerminalLastModified]
			   ,[ConfirmationNumber]
			   ,[RecipientId]
			   ,[ExchangeRate]
			   ,[DTServerCreate]
			   ,[DTServerLastModified]
			   ,[TransferType]
			   ,[Destination]
			   ,[TransactionSubType]
			   ,[OriginalTransactionID]
			   ,GETDATE() 
			   ,@AuditEvent
			   ,@RevisionNo
			FROM DELETED

       END
GO


