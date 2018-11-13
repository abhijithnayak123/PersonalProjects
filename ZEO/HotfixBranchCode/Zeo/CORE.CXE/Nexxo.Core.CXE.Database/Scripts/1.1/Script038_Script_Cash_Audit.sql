CREATE TABLE [dbo].[tTxn_Cash_Stage_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[CashTrxType] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	RevisionNo [bigint] not null,
	AuditEvent [smallint] not null,
	DTAudit [datetime] not null
	)

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trTxn_Cash_StageAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trTxn_Cash_StageAudit]
GO
create trigger trTxn_Cash_StageAudit on tTxn_Cash_Stage AFTER Insert, Update, Delete
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
                     DTAudit
                     )
              select rowguid, Id,  Amount, Fee, AccountPK, CashTrxType, DTCreate, DTLastMod, @RevisionNo,2 as AuditEvent,GETDATE() from inserted
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
                     DTAudit
              )
              select rowguid, Id,  Amount, Fee, AccountPK, CashTrxType, DTCreate, DTLastMod,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
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
                     DTAudit
              )
              select rowguid, Id,  Amount, Fee, AccountPK, CashTrxType, DTCreate, DTLastMod, @RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 GO