--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <11-22-2016>
-- Description:	 Trigger for tTxn_Funds audit table
-- Jira ID:		<AL-8323>
-- ================================================================================

IF OBJECT_ID(N'tr_tTxn_Funds_Aud', N'TR') IS NOT NULL
DROP TRIGGER tr_tTxn_Funds_Aud
GO


CREATE TRIGGER tr_tTxn_Funds_Aud ON tTxn_Funds   
AFTER INSERT, UPDATE, DELETE
AS
	SET NOCOUNT ON
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
		tTxn_Funds_Aud
	WHERE 
		TransactionId = (SELECT TransactionId FROM INSERTED)

			INSERT INTO [dbo].[tTxn_Funds_Aud]
					   ([TransactionID]
					   ,[CXNId]
					   ,[Amount]
					   ,[Fee]
					   ,[Description]
					   ,[State]
					   ,[DTTerminalCreate]
					   ,[DTTerminalLastModified]
					   ,[ConfirmationNumber]
					   ,[FundType]
					   ,[DTServerCreate]
					   ,[DTServerLastModified]
					   ,[BaseFee]
					   ,[DiscountApplied]
					   ,[AdditionalFee]
					   ,[DiscountName]
					   ,[DiscountDescription]
					   ,[IsSystemApplied]
					   ,[AddOnCustomerId]
					   ,[CustomerSessionId]
					   ,[ProviderId]
					   ,[CustomerRevisionNo]
					   ,[ProviderAccountId]
					   ,[RevisionNo] 
					   ,[AuditEvent] 
					   ,[DTAudit] 

					  )
				 SELECT  [TransactionID]
						,[CXNId]
						,[Amount]
						,[Fee]
						,[Description]
						,[State]
						,[DTTerminalCreate]
						,[DTTerminalLastModified]
						,[ConfirmationNumber]
						,[FundType]
						,[DTServerCreate]
						,[DTServerLastModified]
						,[BaseFee]
						,[DiscountApplied]
						,[AdditionalFee]
						,[DiscountName]
						,[DiscountDescription]
						,[IsSystemApplied]
						,[AddOnCustomerId]
						,[CustomerSessionId]
						,[ProviderId]
						,[CustomerRevisionNo]
						,[ProviderAccountId]
						,@RevisionNo
						,@AuditEvent
						,GETDATE()
				 FROM INSERTED
				 END
ELSE
	 BEGIN

	    SELECT 
		  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
		FROM 
		  tTxn_Funds_Aud
		WHERE 
		  TransactionId = (SELECT TransactionId FROM DELETED)
		   
			INSERT INTO [dbo].[tTxn_Funds_Aud]
					   ([TransactionID]
					   ,[CXNId]
					   ,[Amount]
					   ,[Fee]
					   ,[Description]
					   ,[State]
					   ,[DTTerminalCreate]
					   ,[DTTerminalLastModified]
					   ,[ConfirmationNumber]
					   ,[FundType]
					   ,[DTServerCreate]
					   ,[DTServerLastModified]
					   ,[BaseFee]
					   ,[DiscountApplied]
					   ,[AdditionalFee]
					   ,[DiscountName]
					   ,[DiscountDescription]
					   ,[IsSystemApplied]
					   ,[AddOnCustomerId]
					   ,[CustomerSessionId]
					   ,[ProviderId]
					   ,[CustomerRevisionNo]
					   ,[ProviderAccountId]
					   ,[RevisionNo] 
					   ,[AuditEvent] 
					   ,[DTAudit] 
					  )
				 SELECT  [TransactionID]
						,[CXNId]
						,[Amount]
						,[Fee]
						,[Description]
						,[State]
						,[DTTerminalCreate]
						,[DTTerminalLastModified]
						,[ConfirmationNumber]
						,[FundType]
						,[DTServerCreate]
						,[DTServerLastModified]
						,[BaseFee]
						,[DiscountApplied]
						,[AdditionalFee]
						,[DiscountName]
						,[DiscountDescription]
						,[IsSystemApplied]
						,[AddOnCustomerId]
						,[CustomerSessionId]
						,[ProviderId]
						,[CustomerRevisionNo]
						,[ProviderAccountId]
						,@RevisionNo
						,@AuditEvent
						,GETDATE()
				 FROM DELETED
				 END
GO


