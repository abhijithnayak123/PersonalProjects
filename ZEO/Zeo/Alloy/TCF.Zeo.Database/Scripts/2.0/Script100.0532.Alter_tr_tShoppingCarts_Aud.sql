--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <12-13-2016>
-- Description:	 Create the trigger to populate the tShoppingCarts_Aud table
-- Jira ID:		<AL-8952>
-- ================================================================================


IF OBJECT_ID(N'trShoppingCartsAudit', N'TR') IS NOT NULL
BEGIN
	DROP TRIGGER trShoppingCartsAudit      -- Drop the Old trigger.
END
GO


IF OBJECT_ID(N'tr_ShoppingCarts_Audit', N'TR') IS NOT NULL
BEGIN
	DROP TRIGGER tr_ShoppingCarts_Audit      -- Drop the existing trigger.
END
GO

CREATE TRIGGER [dbo].[tr_ShoppingCarts_Audit] ON [dbo].[tShoppingCarts] 
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
			  tShoppingCarts_Aud sca
			  INNER JOIN INSERTED i ON i.CartId = sca.CartId

           INSERT INTO tShoppingCarts_Aud(
                  cartId,
                  RevisionNo,
                  AuditEvent,
                  DTAudit,
				  IsReferral,
				  Status,
				  CustomerSessionId,
				  State,
				  DTTerminalCreate,
				  DTServerCreate,
                  DTTerminalLastModified,
                  DTServerLastModified)

           SELECT 
				cartId,
				@RevisionNo,
				@auditEvent,
				GETDATE(),
				IsReferral,
				Status,
				CustomerSessionId,
				State,
				DTTerminalCreate,
				DTServerCreate,
				DTTerminalLastModified,
				DTServerLastModified
			FROM INSERTED

		END
	ELSE
		BEGIN
		   SELECT 
			  @revisionNo = ISNULL(MAX(RevisionNo), 0) + 1 
			FROM 
			  tShoppingCarts_Aud sca
			  INNER JOIN DELETED d ON d.CartId = sca.CartId

           INSERT INTO tShoppingCarts_Aud(
                  cartId,
                  RevisionNo,
                  AuditEvent,
                  DTAudit,
				  IsReferral,
				  Status,
				  CustomerSessionId,
				  State,
				  DTTerminalCreate,
				  DTServerCreate,
                  DTTerminalLastModified,
                  DTServerLastModified)

           SELECT 
				cartId,
				@RevisionNo,
				@auditEvent,
				GETDATE(),
				IsReferral,
				Status,
				CustomerSessionId,
				State,
				DTTerminalCreate,
				DTServerCreate,
				DTTerminalLastModified,
				DTServerLastModified
			FROM DELETED

		END

END