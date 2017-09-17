--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger>           
-- Jira ID:	<AL-243>
--===========================================================================================


/****** Object:  Trigger [dbo].[trTxn_MoneyOrder_StageAudit]    Script Date: 3/30/2015 3:58:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[trTxn_MoneyOrder_StageAudit] ON [dbo].[tTxn_MoneyOrder_Stage] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tTxn_MoneyOrder_Stage_Aud(
					 MoneyOrderPK,
                     MoneyOrderId,
                     Amount,
                     Fee,
                     PurchaseDate,
                     MoneyOrderCheckNumber,
                     AccountPK,
                     [Status],
                     DTCreate,
                     DTLastMod,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastMod
                     )
              SELECT MoneyOrderPK, MoneyOrderId,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTCreate, DTLastMod, 2 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tTxn_MoneyOrder_Stage_Aud(
					 MoneyOrderPK,
                     MoneyOrderId,
                     Amount,
                     Fee,
                     PurchaseDate,
                     MoneyOrderCheckNumber,
                     AccountPK,
                     [Status],
                     DTCreate,
                     DTLastMod,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastMod
              )
              SELECT MoneyOrderPK, MoneyOrderId,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTCreate, DTLastMod, 1 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tTxn_MoneyOrder_Stage_Aud(
					 MoneyOrderPK,
                     MoneyOrderId,
                     Amount,
                     Fee,
                     PurchaseDate,
                     MoneyOrderCheckNumber,
                     AccountPK,
                     [Status],
                     DTCreate,
                     DTLastMod,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastMod
              )
              SELECT MoneyOrderPK, MoneyOrderId,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTCreate, DTLastMod, 3 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod FROM deleted
       END
GO


