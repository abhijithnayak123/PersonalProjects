--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger [trFundsStageAudit]>           
-- Jira ID:	<AL-243>
--===========================================================================================

/****** Object:  Trigger [dbo].[trFundsStageAudit]    Script Date: 3/30/2015 3:55:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[trFundsStageAudit] ON [dbo].[tTxn_Funds_Stage] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tTxn_Funds_Stage_Aud where FundsId = (select FundsId from inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tTxn_Funds_Stage_Aud(FundsPK,
                     FundsId,
                     Amount,
                     Fee,
                     AccountPK,
                     [Type],
                     [Status],
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastMod
                     )
              SELECT FundsPK,FundsId,Amount,Fee,AccountPK,[TYPE],[Status],DTCreate,DTLastMod,@RevisionNo,2 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tTxn_Funds_Stage_Aud(FundsPK,
                     FundsId,
                     Amount,
                     Fee,
                     AccountPK,
                     [Type],
                     [Status],
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastMod
                     )
              SELECT FundsPK,FundsId,Amount,Fee,AccountPK,[TYPE],[Status],DTCreate,DTLastMod,@RevisionNo,1 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tTxn_Funds_Stage_Aud(FundsPK,
                     FundsId,
                     Amount,
                     Fee,
                     AccountPK,
                     [Type],
                     [Status],
                     DTCreate,
                     DTLastMod,
                     RevisionNo,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastMod
                     )
              SELECT FundsPK,FundsId,Amount,Fee,AccountPK,[TYPE],[Status],DTCreate,DTLastMod,@RevisionNo,3 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod FROM deleted
       END
GO


