ALTER TABLE tTxn_MoneyOrder_Stage ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_MoneyOrder_Commit ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TABLE tTxn_MoneyOrder_Stage_Aud ADD
DTServerCreate DateTime, DTServerLastMod DateTime
GO

ALTER TRIGGER [dbo].[trTxn_MoneyOrder_StageAudit] on [dbo].[tTxn_MoneyOrder_Stage] AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tTxn_MoneyOrder_Stage_Aud(
					 rowguid,
                     Id,
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
              select rowguid, Id,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTCreate, DTLastMod, 2 as AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tTxn_MoneyOrder_Stage_Aud(
					 rowguid,
                     Id,
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
              select rowguid, Id,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTCreate, DTLastMod, 1 as AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tTxn_MoneyOrder_Stage_Aud(
					 rowguid,
                     Id,
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
              select rowguid, Id,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTCreate, DTLastMod, 3 as AuditEvent,GETDATE(),DTServerCreate,DTServerLastMod from deleted
       end
GO


