--===========================================================================================
-- Author:		<Rogy Eapen>
-- Created date: <09/24/2015>
-- Description:	<Creating trigger trShoppingCartTransactionsAudit>           
-- Jira ID:	<AL-2018>
--===========================================================================================


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trShoppingCartTransactionsAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].trShoppingCartTransactionsAudit
GO

CREATE TRIGGER [dbo].[trShoppingCartTransactionsAudit] ON [dbo].[tShoppingCartTransactions] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tShoppingCartTransactions_Aud WHERE txnPK = (SELECT txnPK FROM inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tShoppingCartTransactions_Aud(
					 cartPK,
					 txnPK,
					 CartTxnPK,
					 CartItemStatus,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,txnPK,CartTxnPK,CartItemStatus,@RevisionNo,2 AS AuditEvent,GETDATE() FROM deleted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tShoppingCartTransactions_Aud(
					 cartPK,
					 txnPK,
					 CartTxnPK,
					 CartItemStatus,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,txnPK,CartTxnPK,CartItemStatus,@RevisionNo,1 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tShoppingCartTransactions_Aud(
					 cartPK,
					 txnPK,
					 CartTxnPK,
					 CartItemStatus,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,txnPK,CartTxnPK,CartItemStatus,@RevisionNo,3 AS AuditEvent,GETDATE() FROM deleted
       END