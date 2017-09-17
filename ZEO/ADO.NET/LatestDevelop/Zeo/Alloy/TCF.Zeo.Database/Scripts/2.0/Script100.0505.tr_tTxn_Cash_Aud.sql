--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	 pupulate the data to audit table
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('tr_tTxn_Cash_Aud') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tr_tTxn_Cash_Aud
	 END
GO

CREATE TRIGGER tr_tTxn_Cash_Aud ON tTxn_Cash
AFTER INSERT, UPDATE, DELETE
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @revisionNo BIGINT
	DECLARE @auditEvent SMALLINT

	IF((SELECT COUNT(1) FROM INSERTED) > 0 AND (SELECT COUNT(1) FROM DELETED) > 0)
	BEGIN
		-- UPDATE
		SET @auditEvent = 2
	END
	ELSE
	IF((SELECT COUNT(1) FROM INSERTED) > 0)
	BEGIN
		-- INSERT
		SET @auditEvent = 1
	END
	ELSE
	IF((SELECT COUNT(1) FROM DELETED) > 0)
	BEGIN
		-- DELETE
		SET @auditEvent = 3
	END

	IF @AuditEvent != 3
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tTxn_Cash_Aud tca
			  INNER JOIN INSERTED i ON i.TransactionId = tca.TransactionId
		END
	ELSE
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tTxn_Cash_Aud tca
			  INNER JOIN DELETED d ON d.TransactionId = tca.TransactionId
		END

	IF @AuditEvent != 3
		BEGIN
			
         INSERT INTO tTxn_Cash_Aud
           (
		    TransactionID
           ,Amount
           ,Fee
           ,Description
           ,DTTerminalCreate
           ,DTTerminalLastModified
           ,ConfirmationNumber
           ,CashType
           ,DTServerCreate
           ,DTServerLastModified
           ,CustomerSessionId
		   ,RevisionNo
		   ,AuditEvent
		   ,DTAudit
		   )

          SELECT 
		    TransactionId
		   ,Amount
           ,Fee
           ,Description
           ,DTTerminalCreate
           ,DTTerminalLastModified
           ,ConfirmationNumber
           ,CashType
           ,DTServerCreate
           ,DTServerLastModified
           ,CustomerSessionId
		   ,@RevisionNo
		   ,@auditEvent
		   ,GETDATE()
		  FROM 
		    INSERTED
		END
	ELSE
		BEGIN
		  INSERT INTO tTxn_Cash_Aud
           (
		    TransactionID
           ,Amount
           ,Fee
           ,Description
           ,DTTerminalCreate
           ,DTTerminalLastModified
           ,ConfirmationNumber
           ,CashType
           ,DTServerCreate
           ,DTServerLastModified
           ,CustomerSessionId
		   ,RevisionNo
		   ,AuditEvent
		   ,DTAudit
		   )

          SELECT 
		    TransactionId
		   ,Amount
           ,Fee
           ,Description
           ,DTTerminalCreate
           ,DTTerminalLastModified
           ,ConfirmationNumber
           ,CashType
           ,DTServerCreate
           ,DTServerLastModified
           ,CustomerSessionId
		   ,@RevisionNo
		   ,@auditEvent
		   ,GETDATE()
		  FROM 
		    DELETED
		END
END
GO
