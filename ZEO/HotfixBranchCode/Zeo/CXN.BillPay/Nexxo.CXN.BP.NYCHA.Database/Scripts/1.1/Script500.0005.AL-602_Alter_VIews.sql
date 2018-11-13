-- ================================================================================
-- Author:		<Ashok Kumar>
-- Create date: <06/24/2015>
-- Description:	<Alter views date columns to DTTerminalCreate, DTTerminalLastModified, DTServerCreate, DTServerLastModified. >
-- Jira ID:		<AL-602>
-- ================================================================================

--trTNYCHA_BillPay_TrxAudit
IF EXISTS (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TRTNYCHA_BILLPAY_TRXAUDIT')
BEGIN
	DROP TRIGGER [dbo].[trTNYCHA_BillPay_TrxAudit]
END
GO

CREATE TRIGGER [dbo].[trTNYCHA_BillPay_TrxAudit] on [dbo].[tNYCHA_BillPay_Trx] AFTER Insert, Update, Delete
AS
    SET NOCOUNT ON
             
    IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM DELETED)>0)
    BEGIN
        INSERT INTO [dbo].[tNYCHA_BillPay_Trx_Aud]
			([NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],[AuditEvent],[DTAudit])
        SELECT 
			[NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified], 2 as AuditEvent,GETDATE() 
		FROM inserted
    END
    ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
    BEGIN
        INSERT INTO [dbo].[tNYCHA_BillPay_Trx_Aud]
			([NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],[AuditEvent],[DTAudit])
        SELECT  
			[NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified], 1 as AuditEvent,GETDATE() 
		FROM inserted
    END
    ELSE IF(SELECT COUNT(*) FROM deleted)>0
    BEGIN
        INSERT INTO [dbo].[tNYCHA_BillPay_Trx_Aud]
			([NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified],[AuditEvent],[DTAudit])
        SELECT  
			[NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTTerminalCreate],[DTTerminalLastModified],[DTServerCreate],[DTServerLastModified], 3 as AuditEvent,GETDATE() 
		FROM deleted
    END
GO


