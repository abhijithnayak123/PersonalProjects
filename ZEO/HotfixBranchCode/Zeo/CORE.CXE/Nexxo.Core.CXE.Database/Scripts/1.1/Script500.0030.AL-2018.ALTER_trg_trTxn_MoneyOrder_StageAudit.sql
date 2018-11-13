-- ============================================================
-- Author:		Manikandan Govindraj
-- Create date: <09/24/2015>
-- Description:	<Script for Altering trTxn_MoneyOrder_StageAudit trigger>
-- Jira ID:		<AL-2018>
-- ============================================================

IF EXISTS
( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'trTxn_MoneyOrder_StageAudit'
)
BEGIN
	DROP TRIGGER [dbo].[trTxn_MoneyOrder_StageAudit]
END
GO


CREATE TRIGGER [dbo].[trTxn_MoneyOrder_StageAudit] ON [dbo].[tTxn_MoneyOrder_Stage] AFTER INSERT, UPDATE, DELETE
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
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified,
					 AccountNumber,
					 RoutingNumber,
					 MICR
                     )
              SELECT MoneyOrderPK, MoneyOrderId,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTTerminalCreate, DTTerminalLastModified, 2 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified,AccountNumber, RoutingNumber,MICR FROM inserted
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
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified,
					 AccountNumber,
					 RoutingNumber,
					 MICR
              )
              SELECT MoneyOrderPK, MoneyOrderId,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTTerminalCreate, DTTerminalLastModified, 1 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified,AccountNumber, RoutingNumber,MICR FROM inserted
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
                     DTTerminalCreate,
                     DTTerminalLastModified,
                     AuditEvent,
                     DTAudit,
                     DTServerCreate,
                     DTServerLastModified,
					 AccountNumber,
					 RoutingNumber,
					 MICR
              )
              SELECT MoneyOrderPK, MoneyOrderId,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTTerminalCreate, DTTerminalLastModified, 3 AS AuditEvent,GETDATE(),DTServerCreate,DTServerLastModified,AccountNumber, RoutingNumber,MICR FROM deleted
       END
GO


