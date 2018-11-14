--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-11-2016>
-- Description:	 pupulate the data to audit table
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('tr_PopulateBillPayTransaction_Aud') IS NOT NULL
	 BEGIN
		  DROP TRIGGER dbo.tr_PopulateBillPayTransaction_Aud
	 END
GO

CREATE TRIGGER tr_PopulateBillPayTransaction_Aud ON tTxn_BillPay
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
			  tTxn_BillPay_Aud tba
			  INNER JOIN INSERTED i ON i.TransactionId = tba.TransactionId
		END
	ELSE
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tTxn_BillPay_Aud tba
			  INNER JOIN DELETED d ON d.TransactionId = tba.TransactionId
		END

	IF @AuditEvent != 3
		BEGIN
			INSERT INTO dbo.tTxn_BillPay_Aud
			(
				TransactionId,
				CustomerSessionId,
				Amount,
				Fee,
				Description,
				State,
				DTTerminalCreate,
				DTTerminalLastModified,
				ConfirmationNumber,
				DTServerCreate,
				DTServerLastModified,
				CustomerRevisionNo,
				ProviderId,
				ProviderAccountId,
				DTAudit,
				AuditEvent,
				RevisionNo,
				AccountNumber
			)
			SELECT 
				TransactionID,
				CustomerSessionId,
				Amount,
				Fee,
				Description,
				State,
				DTTerminalCreate,
				DTTerminalLastModified,
				ConfirmationNumber,
				DTServerCreate,
				DTServerLastModified,
				CustomerRevisionNo,
				ProviderId,
				ProviderAccountId,
				GETDATE(),
				@auditEvent,
				@revisionNo,
				AccountNumber
			FROM 
				INSERTED
		END
	ELSE
		BEGIN
			INSERT INTO dbo.tTxn_BillPay_Aud
			(
				TransactionId,
				CustomerSessionId,
				Amount,
				Fee,
				Description,
				State,
				DTTerminalCreate,
				DTTerminalLastModified,
				ConfirmationNumber,
				DTServerCreate,
				DTServerLastModified,
				CustomerRevisionNo,
				ProviderId,
				ProviderAccountId,
				DTAudit,
				AuditEvent,
				RevisionNo,
				AccountNumber
			)
			SELECT 
				TransactionID,
				CustomerSessionId,
				Amount,
				Fee,
				Description,
				State,
				DTTerminalCreate,
				DTTerminalLastModified,
				ConfirmationNumber,
				DTServerCreate,
				DTServerLastModified,
				CustomerRevisionNo,
				ProviderId,
				ProviderAccountId,
				GETDATE(),
				@auditEvent,
				@revisionNo,
				AccountNumber
			FROM 
				DELETED
		END
END
GO
