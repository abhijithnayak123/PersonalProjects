--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter trigger trTNYCHA_BillPay_TrxAudit>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  Trigger [dbo].[trTNYCHA_BillPay_TrxAudit]    Script Date: 4/7/2015 1:17:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[trTNYCHA_BillPay_TrxAudit] on [dbo].[tNYCHA_BillPay_Trx] AFTER Insert, Update, Delete
AS
    SET NOCOUNT ON
             
    IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM DELETED)>0)
    BEGIN
        INSERT INTO [dbo].[tNYCHA_BillPay_Trx_Aud]
			([NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],[AuditEvent],[DTAudit])
        SELECT 
			[NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod], 2 as AuditEvent,GETDATE() 
		FROM inserted
    END
    ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
    BEGIN
        INSERT INTO [dbo].[tNYCHA_BillPay_Trx_Aud]
			([NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],[AuditEvent],[DTAudit])
        SELECT  
			[NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod], 1 as AuditEvent,GETDATE() 
		FROM inserted
    END
    ELSE IF(SELECT COUNT(*) FROM deleted)>0
    BEGIN
        INSERT INTO [dbo].[tNYCHA_BillPay_Trx_Aud]
			([NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod],[AuditEvent],[DTAudit])
        SELECT  
			[NYCHATrxPK],[NYCHATrxID],[Amount],[Fee],[TenantID],[AccountNumber],[ProductId],[BillerName],[ProviderId],[Status],[ChannelParterId],[LocationId],
			[NYCHAAccountPK],[DTCreate],[DTLastMod],[DTServerCreate],[DTServerLastMod], 3 as AuditEvent,GETDATE() 
		FROM deleted
    END
GO


