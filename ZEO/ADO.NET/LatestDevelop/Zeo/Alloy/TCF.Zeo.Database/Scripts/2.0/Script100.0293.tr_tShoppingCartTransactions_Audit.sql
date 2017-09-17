--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <23-01-2016>
-- Description: Create the trigger to populate the tShoppingCartTransaction_Aud table
-- Jira ID:		<AL-8952>
-- ================================================================================


IF OBJECT_ID(N'tr_ShoppingCartTransaction_Audit', N'TR') IS NOT NULL
BEGIN
	 DROP TRIGGER tr_ShoppingCartTransaction_Audit      -- Drop the existing trigger.
END
GO

CREATE TRIGGER [dbo].[tr_ShoppingCartTransaction_Audit] ON [dbo].[tShoppingCartTransactions] 
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


	IF @auditEvent != 3
		BEGIN
		   SELECT 
			  @revisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tShoppingCartTransactions_Aud sta
			  INNER JOIN INSERTED i ON i.ShoppingCartTransactionId = sta.ShoppingCartTransactionId

           INSERT INTO tShoppingCartTransactions_Aud(
			      TransactionId,
				  ShoppingCartTransactionId,
                  cartId,
                  RevisionNo,
                  AuditEvent,
                  DTAudit,
                  ProductId,
				  CartItemStatus)

           SELECT 
			    TransactionId,
				ShoppingCartTransactionId,
				cartId,
				@RevisionNo,
				@auditEvent,
				GETDATE(),
				ProductId,
				CartItemStatus
				
			FROM INSERTED

		END
	ELSE
		BEGIN
		   SELECT 
			  @revisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tShoppingCartTransactions_Aud sta
			  INNER JOIN DELETED d ON d.ShoppingCartTransactionId = sta.ShoppingCartTransactionId

           INSERT INTO tShoppingCartTransactions_Aud(
			      TransactionId,
				  ShoppingCartTransactionId,
                  cartId,
                  RevisionNo,
                  AuditEvent,
                  DTAudit,
                  ProductId,
				  CartItemStatus)

           SELECT 
			    TransactionId,
				ShoppingCartTransactionId,
				cartId,
				@RevisionNo,
				@auditEvent,
				GETDATE(),
				ProductId,
				CartItemStatus
				
			FROM DELETED

		END

END