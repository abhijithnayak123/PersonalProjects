--===========================================================================================
-- Auther:			Asha
-- Date Created:	1/15/2014
-- Description:		Script for Create NYCHA CXN Transaction Audit Tables
--===========================================================================================

CREATE TABLE [dbo].[tNYCHA_BillPay_Trx_Aud](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[TenantID] [nvarchar](20) NOT NULL,
	[AccountNumber] [nvarchar](20) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[BillerName] [varchar](255) NOT NULL,
	[ProviderId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ChannelParterId] [smallint] NOT NULL,	
	[LocationId] [uniqueidentifier] NULL,
	[NYCHAAccountPK] [uniqueidentifier] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastMod] [datetime] NULL,
	[AuditEvent] [smallint] NOT NULL,
	[DTAudit] [datetime] NOT NULL
) 
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[trTNYCHA_BillPay_TrxAudit]') AND type in (N'TR'))
DROP TRIGGER [dbo].[trTNYCHA_BillPay_TrxAudit]
GO
CREATE TRIGGER [dbo].[trTNYCHA_BillPay_TrxAudit] on [dbo].[tNYCHA_BillPay_Trx] AFTER Insert, Update, Delete
AS
    SET NOCOUNT ON
             
    IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM DELETED)>0)
    BEGIN
        INSERT INTO [dbo].[tNYCHA_BillPay_Trx_Aud]
			([rowguid],[Id],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],[AuditEvent],[DTAudit])
        SELECT 
			[rowguid],[Id],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod], 2 as AuditEvent,GETDATE() 
		FROM inserted
    END
    ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
    BEGIN
        INSERT INTO [dbo].[tNYCHA_BillPay_Trx_Aud]
			([rowguid],[Id],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],[AuditEvent],[DTAudit])
        SELECT  
			[rowguid],[Id],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod], 1 as AuditEvent,GETDATE() 
		FROM inserted
    END
    ELSE IF(SELECT COUNT(*) FROM deleted)>0
    BEGIN
        INSERT INTO [dbo].[tNYCHA_BillPay_Trx_Aud]
			([rowguid],[Id],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],[AuditEvent],[DTAudit])
        SELECT  
			[rowguid],[Id],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod], 3 as AuditEvent,GETDATE() 
		FROM deleted
    END
GO
--===========================================================================================




