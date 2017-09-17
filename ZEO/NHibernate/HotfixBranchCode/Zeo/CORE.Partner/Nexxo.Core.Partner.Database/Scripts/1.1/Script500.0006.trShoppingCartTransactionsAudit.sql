--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger trShoppingCartTransactionsAudit>           
-- Jira ID:	<AL-242>
--===========================================================================================

/****** Object:  Trigger [dbo].[trShoppingCartTransactionsAudit]    Script Date: 3/27/2015 2:48:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[trShoppingCartTransactionsAudit] ON [dbo].[tShoppingCartTransactions] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tShoppingCartTransactions_Aud WHERE txnPK = (SELECT txnPK FROM inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tShoppingCartTransactions_Aud(
					 cartPK,
					 txnPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,txnPK,@RevisionNo,2 AS AuditEvent,GETDATE() FROM deleted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tShoppingCartTransactions_Aud(
					 cartPK,
					 txnPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,txnPK,@RevisionNo,1 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tShoppingCartTransactions_Aud(
					 cartPK,
					 txnPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              SELECT cartPK,txnPK,@RevisionNo,3 AS AuditEvent,GETDATE() FROM deleted
       END
GO


