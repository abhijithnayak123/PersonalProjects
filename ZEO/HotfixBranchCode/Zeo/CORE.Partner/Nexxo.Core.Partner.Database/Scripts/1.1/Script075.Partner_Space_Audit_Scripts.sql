IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tShoppingCarts_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tShoppingCarts_Aud]
GO
CREATE TABLE [dbo].[tShoppingCarts_Aud](
	[cartRowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Active] [bit] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[CustomerPK] [uniqueidentifier] NULL,
	RevisionNo BIGINT NOT NULL,
	AuditEvent SMALLINT NOT NULL,
	DTAudit DATETIME NOT NULL
)
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trShoppingCartsAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trShoppingCartsAudit]
GO
create trigger trShoppingCartsAudit on tShoppingCarts AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tShoppingCarts_Aud where cartRowguid = (select cartRowguid from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tShoppingCarts_Aud(
					 cartRowguid,
                     Id,
                     Active,
                     DTCreate,
                     DTLastMod,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select cartRowguid,Id,Active,DTCreate,DTLastMod,CustomerPK,@RevisionNo,2 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tShoppingCarts_Aud(
					  cartRowguid,
                     Id,
                     Active,
                     DTCreate,
                     DTLastMod,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select cartRowguid,Id,Active,DTCreate,DTLastMod,CustomerPK,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tShoppingCarts_Aud(
					  cartRowguid,
                     Id,
                     Active,
                     DTCreate,
                     DTLastMod,
                     CustomerPK,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select cartRowguid,Id,Active,DTCreate,DTLastMod,CustomerPK,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 
GO
--========================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tShoppingCartTransactions_Aud]') AND type in (N'U'))
DROP TABLE [dbo].[tShoppingCartTransactions_Aud]
GO
CREATE TABLE [dbo].[tShoppingCartTransactions_Aud](
	[cartRowguid] [uniqueidentifier] NOT NULL,
	[txnRowguid] [uniqueidentifier] NOT NULL,
	RevisionNo BIGINT NOT NULL,
	AuditEvent SMALLINT NOT NULL,
	DTAudit DATETIME NOT NULL
)

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trShoppingCartTransactionsAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trShoppingCartTransactionsAudit]
GO
create trigger trShoppingCartTransactionsAudit on tShoppingCartTransactions AFTER Insert, Update, Delete
AS
       SET NOCOUNT ON
       Declare @RevisionNo bigint
       select @RevisionNo = isnull(MAX(RevisionNo),0) + 1 from tShoppingCartTransactions_Aud where txnRowguid = (select txnRowguid from inserted)
             
       if ((select COUNT(*) from inserted)<>0 and (select COUNT(*) from deleted)>0)
       begin
              insert into tShoppingCartTransactions_Aud(
					 cartRowguid,
					 txnRowguid,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select cartRowguid,txnRowguid,@RevisionNo,2 as AuditEvent,GETDATE() from deleted
       end
       else if(select COUNT(*) from inserted)>0 and (select COUNT(*) from deleted)=0
       begin
              insert into tShoppingCartTransactions_Aud(
					 cartRowguid,
					 txnRowguid,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select cartRowguid,txnRowguid,@RevisionNo,1 as AuditEvent,GETDATE() from inserted
       end
       else if(select COUNT(*) from deleted)>0
       begin
              insert into tShoppingCartTransactions_Aud(
					 cartRowguid,
					 txnRowguid,
                     RevisionNo,
                     AuditEvent,
                     DTAudit)
              select cartRowguid,txnRowguid,@RevisionNo,3 as AuditEvent,GETDATE() from deleted
       end
 