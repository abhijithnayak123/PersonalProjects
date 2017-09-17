--- ===============================================================================
-- Author:		<Nishad Varghese>
-- Create date: <01/Sep/2016>
-- Description:	Changes to triggers
-- Jira ID:		<AL-7706>
-- ================================================================================


IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE TYPE = 'TR' and NAME = 'tr_tTxn_MoneyOrder_Aud'
)
BEGIN
	DROP TRIGGER tr_tTxn_MoneyOrder_Aud
END
GO

CREATE TRIGGER tr_tTxn_MoneyOrder_Aud ON tTxn_MoneyOrder
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
	
	IF @AuditEvent != 3
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tTxn_MoneyOrder_Aud tma 
			  INNER JOIN 
			  INSERTED i ON i.TransactionID = tma.TransactionId
		END
	ELSE
		BEGIN
		   SELECT 
			  @RevisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tTxn_MoneyOrder_Aud tma
			  INNER JOIN 
			  DELETED d ON d.TransactionID = tma.TransactionId
		END
	
	IF @AuditEvent != 3
		BEGIN
			INSERT tTxn_MoneyOrder_Aud
			(
				TransactionId,
				CustomerSessionId,
				CustomerRevisionNo,
				MICR,
				PurchaseDate,
				Amount,
				Fee,
				Description,
				State,
				BaseFee,
				DiscountApplied,
				AdditionalFee,
				DiscountName,
				DiscountDescription,
				IsSystemApplied,
				CheckNumber,
				AccountNumber,
				RoutingNumber,
				DTAudit,
				RevisionNo,
				AuditEvent,
				DTTerminalCreate,
				DTTerminalLastModified,
				DTServerCreate,
				DTServerLastModified
			)
			SELECT
				TransactionId,
				CustomerSessionId,
				CustomerRevisionNo,
				MICR,
				PurchaseDate,
				Amount,
				Fee,
				Description,
				State,
				BaseFee,
				DiscountApplied,
				AdditionalFee,
				DiscountName,
				DiscountDescription,
				IsSystemApplied,
				CheckNumber,
				AccountNumber,
				RoutingNumber,
				GETDATE(),
				@RevisionNo,
				@AuditEvent,
				DTTerminalCreate,
				DTTerminalLastModified,
				DTServerCreate,
				DTServerLastModified
			FROM 
				INSERTED
		END
	ELSE
		BEGIN
			INSERT tTxn_MoneyOrder_Aud
			(
				TransactionId,
				CustomerSessionId,
				CustomerRevisionNo,
				MICR,
				PurchaseDate,
				Amount,
				Fee,
				Description,
				State,
				BaseFee,
				DiscountApplied,
				AdditionalFee,
				DiscountName,
				DiscountDescription,
				IsSystemApplied,
				CheckNumber,
				AccountNumber,
				RoutingNumber,
				DTAudit,
				RevisionNo,
				AuditEvent,
				DTTerminalCreate,
				DTTerminalLastModified,
				DTServerCreate,
				DTServerLastModified
			)
			SELECT
				TransactionId,
				CustomerSessionId,
				CustomerRevisionNo,
				MICR,
				PurchaseDate,
				Amount,
				Fee,
				Description,
				State,
				BaseFee,
				DiscountApplied,
				AdditionalFee,
				DiscountName,
				DiscountDescription,
				IsSystemApplied,
				CheckNumber,
				AccountNumber,
				RoutingNumber,
				GETDATE(),
				@RevisionNo,
				@AuditEvent,
				DTTerminalCreate,
				DTTerminalLastModified,
				DTServerCreate,
				DTServerLastModified
			FROM 
				DELETED
		END
GO


