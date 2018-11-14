--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger [tTxn_MoneyTransfer_StageAudit]>           
-- Jira ID:	<AL-243>
--===========================================================================================

/****** Object:  Trigger [dbo].[tTxn_MoneyTransfer_StageAudit]    Script Date: 3/30/2015 4:05:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[tTxn_MoneyTransfer_StageAudit] ON [dbo].[tTxn_MoneyTransfer_Stage] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
       DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tTxn_MoneyTransfer_Stage_Aud WHERE MoneyTransferId = (SELECT MoneyTransferId FROM inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tTxn_MoneyTransfer_Stage_Aud(
					 [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
                     RevisionNo, AuditEvent,DTAudit)
              SELECT  [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
					 @RevisionNo,2 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
               INSERT INTO tTxn_MoneyTransfer_Stage_Aud(
					 [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
                     RevisionNo, AuditEvent,DTAudit)
              SELECT [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
					 @RevisionNo,1 AS AuditEvent,GETDATE() FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tTxn_MoneyTransfer_Stage_Aud(
					 [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
                     RevisionNo, AuditEvent,DTAudit)
              SELECT [MoneyTransferPK], [MoneyTransferId], [Amount], [Fee],[AccountPK],[Status],
					 [ConfirmationNumber],[ReceiverName],[Destination],
					 [DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],
					 @RevisionNo,3 AS AuditEvent,GETDATE() FROM deleted
       END
GO


