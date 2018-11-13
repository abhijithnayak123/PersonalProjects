ALTER TABLE tTxn_Check_Commit ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

--ALTER TABLE tTxn_Check_Stage ADD
--DTServerCreate DateTime, DTServerLastMod DateTime
--GO

ALTER TABLE tTxn_BillPay_Commit ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_BillPay_Stage ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_Cash_Commit ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_Cash_Stage ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_Cash_Stage_Aud ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_Funds_Commit ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_Funds_Stage ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_Funds_Stage_Aud ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_MoneyTransfer_Commit ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_MoneyTransfer_Stage ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER trigger [dbo].[trTxn_Cash_StageAudit] on [dbo].[tTxn_Cash_Stage] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tTxn_Cash_Stage_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tTxn_Cash_Stage_Aud(
					 rowguid,
                     Id,
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
              select rowguid, Id,  Amount, Fee, AccountPK, CashTrxType, DTCreate, DTLastMod, @RevisionNo,2 as AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tTxn_Cash_Stage_Aud(
					 rowguid,
                     Id,
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
              select rowguid, Id,  Amount, Fee, AccountPK, CashTrxType, DTCreate, DTLastMod,@RevisionNo,1 as AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tTxn_Cash_Stage_Aud(
					 rowguid,
                     Id,
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
              select rowguid, Id,  Amount, Fee, AccountPK, CashTrxType, DTCreate, DTLastMod, @RevisionNo,3 as AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod from deleted
       end
GO

ALTER trigger [dbo].[trFundsStageAudit] on [dbo].[tTxn_Funds_Stage] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tTxn_Funds_Stage_Aud where Id = (select Id from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tTxn_Funds_Stage_Aud(rowguid,
                     Id,
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
              select rowguid,Id,Amount,Fee,AccountPK,[TYPE],[Status],DTCreate,DTLastMod,@RevisionNo,2 as AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tTxn_Funds_Stage_Aud(rowguid,
                     Id,
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
              select rowguid,Id,Amount,Fee,AccountPK,[TYPE],[Status],DTCreate,DTLastMod,@RevisionNo,1 as AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tTxn_Funds_Stage_Aud(rowguid,
                     Id,
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
              select rowguid,Id,Amount,Fee,AccountPK,[TYPE],[Status],DTCreate,DTLastMod,@RevisionNo,3 as AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod from deleted
       end
GO