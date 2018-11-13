--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trShoppingCartsAudit>           
-- Jira ID:	<AL-242>
--===========================================================================================

/****** Object:  Trigger [dbo].[trShoppingCartsAudit]    Script Date: 3/27/2015 2:43:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[trShoppingCartsAudit] ON [dbo].[tShoppingCarts] AFTER INSERT, UPDATE, DELETE
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
                     DTCreate,
                     DTLastMod,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,cartId,Active,DTCreate,DTLastMod,CustomerPK,@RevisionNo,2 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tShoppingCarts_Aud(
					  cartPK,
                     cartId,
                     Active,
                     DTCreate,
                     DTLastMod,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,cartId,Active,DTCreate,DTLastMod,CustomerPK,@RevisionNo,1 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tShoppingCarts_Aud(
					  cartPK,
                     cartId,
                     Active,
                     DTCreate,
                     DTLastMod,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,cartId,Active,DTCreate,DTLastMod,CustomerPK,@RevisionNo,3 AS AuditEvent,GETDATE() FROM deleted
       END
GO


