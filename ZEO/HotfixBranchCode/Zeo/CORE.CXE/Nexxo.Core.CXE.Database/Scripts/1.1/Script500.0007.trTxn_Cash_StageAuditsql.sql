--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger>           
-- Jira ID:	<AL-243>
--===========================================================================================

/****** Object:  Trigger [dbo].[trTxn_Cash_StageAudit]    Script Date: 3/30/2015 3:51:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[trTxn_Cash_StageAudit] ON [dbo].[tTxn_Cash_Stage] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tTxn_Cash_Stage_Aud where CashId = (select CashId from inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tTxn_Cash_Stage_Aud(
					 CashPK,
                     CashId,
                     Amount,
                     Fee,
                     AccountPK,
                     CashTrxType,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastMod
                     )
              SELECT CashPK, CashId,  Amount, Fee, AccountPK, CashTrxType, DTCreate, DTLastMod, @RevisionNo,2 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tTxn_Cash_Stage_Aud(
					 CashPK,
                     CashId,
                     Amount,
                     Fee,
                     AccountPK,
                     CashTrxType,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastMod
              )
              SELECT CashPK, CashId,  Amount, Fee, AccountPK, CashTrxType, DTCreate, DTLastMod,@RevisionNo,1 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tTxn_Cash_Stage_Aud(
					 CashPK,
                     CashId,
                     Amount,
                     Fee,
                     AccountPK,
                     CashTrxType,
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastMod
              )
              SELECT CashPK, CashId,  Amount, Fee, AccountPK, CashTrxType, DTCreate, DTLastMod, @RevisionNo,3 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod FROM deleted
       END
GO


