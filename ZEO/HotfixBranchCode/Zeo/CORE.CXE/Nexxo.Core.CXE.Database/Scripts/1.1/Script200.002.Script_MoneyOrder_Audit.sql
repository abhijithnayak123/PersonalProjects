CREATE TABLE [dbo].[tTxn_MoneyOrder_Stage_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[PurchaseDate] [datetime] NOT NULL,
	[MoneyOrderCheckNumber] [nvarchar](50) NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) 
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trTxn_MoneyOrder_StageAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trTxn_MoneyOrder_StageAudit]
GO
CREATE TRIGGER [dbo].[trTxn_MoneyOrder_StageAudit] on [dbo].[tTxn_MoneyOrder_Stage] AFTER Insert, Update, Delete
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
                     DTAudit
                     )
              select rowguid, Id,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTCreate, DTLastMod, 2 as AuditEvent,GETDATE() from inserted
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
                     DTAudit
              )
              select rowguid, Id,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTCreate, DTLastMod, 1 as AuditEvent,GETDATE() from inserted
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
                     DTAudit
              )
              select rowguid, Id,  Amount, Fee, PurchaseDate, MoneyOrderCheckNumber, AccountPK, [Status], DTCreate, DTLastMod, 3 as AuditEvent,GETDATE() from deleted
       end

GO





