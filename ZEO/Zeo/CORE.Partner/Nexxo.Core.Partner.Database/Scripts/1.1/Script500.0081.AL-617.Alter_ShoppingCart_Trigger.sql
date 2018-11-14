-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <08/04/2015>
-- Description:	<Alter Trigger date columns to DTTerminalCreate, DTTerminalLastModified,
--					 DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-617>
-- ================================================================================
IF EXISTS( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'trShoppingCartsAudit'
)
BEGIN
	DROP TRIGGER [dbo].[trShoppingCartsAudit]
	END
GO

CREATE TRIGGER [dbo].[trShoppingCartsAudit] ON [dbo].[tShoppingCarts] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tShoppingCarts_Aud WHERE cartPK = (SELECT cartPK FROM inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tShoppingCarts_Aud(
					 cartPK,
                     cartId,
                     Active,
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified)
              SELECT cartPK,cartId,Active,DTTerminalCreate,DTTerminalLastModified,CustomerPK,@RevisionNo,2 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tShoppingCarts_Aud(
					  cartPK,
                     cartId,
                     Active,
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified)
              SELECT cartPK,cartId,Active,DTTerminalCreate,DTTerminalLastModified,CustomerPK,@RevisionNo,1 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tShoppingCarts_Aud(
					  cartPK,
                     cartId,
                     Active,
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified)
              SELECT cartPK,cartId,Active,DTTerminalCreate,DTTerminalLastModified,CustomerPK,@RevisionNo,3 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified FROM deleted
       END
GO


