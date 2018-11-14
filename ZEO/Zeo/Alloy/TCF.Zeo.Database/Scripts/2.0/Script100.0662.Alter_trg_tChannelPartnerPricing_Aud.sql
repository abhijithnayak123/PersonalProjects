--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <12-01-2017>
-- Description:	Modified the trigger.
-- Story No:	<B-08684>
-- ================================================================================
IF OBJECT_ID(N'tChannelPartnerPricing_Audit', N'TR') IS NOT NULL
BEGIN
	 DROP TRIGGER tChannelPartnerPricing_Audit      -- Drop the existing trigger.
END
GO

CREATE TRIGGER [dbo].[tChannelPartnerPricing_Audit] ON [dbo].[tChannelPartnerPricing] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
	   DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM 
	   tChannelPartnerPricing_Aud cppa INNER JOIN inserted i on cppa.ChannelPartnerPricingID = i.ChannelPartnerPricingID
	   
             
       IF ((SELECT COUNT(1) FROM inserted)<>0 AND (SELECT COUNT(1) FROM deleted)>0)
       BEGIN
              INSERT INTO tChannelPartnerPricing_Aud(
					ChannelPartnerPricingId,
					PricingGroupId,
					ChannelPartnerId,
					LocationId,
					ProductId,
					ProductType,
					RevisionNo,
					AuditEvent,
					DTAudit,
					DTServerCreate,
					DTServerLastModified,
					DTTerminalCreate,
					DTTerminalLastModified,
					ProductProviderCode)
					
					SELECT 
						Inserted.ChannelPartnerPricingId,
						PricingGroupId,
						ChannelPartnerId,
						LocationId,
						ProductId,
						ProductType,
						@RevisionNo,
						2 as AuditEvent,
						GETDATE(),
						DTServerCreate,
						DTServerLastModified,
						DTTerminalCreate,
						DTTerminalLastModified,
						ProductProviderCode 
					FROM inserted
       END
       ELSE IF(SELECT COUNT(1) FROM inserted)>0 AND (SELECT COUNT(1) FROM deleted)=0
       BEGIN
              INSERT INTO tChannelPartnerPricing_Aud(
					ChannelPartnerPricingId,
					PricingGroupId,
					ChannelPartnerId,
					LocationId,
					ProductId,
					ProductType,
					RevisionNo,
					AuditEvent,
					DTAudit,
					DTServerCreate,
					DTServerLastModified,
					DTTerminalCreate,
					DTTerminalLastModified,
					ProductProviderCode
					)
					
					SELECT 
						ChannelPartnerPricingId,
						PricingGroupId,
						ChannelPartnerId,
						LocationId,
						ProductId,
						ProductType,
						@RevisionNo,
						1 AS AuditEvent,
						GETDATE(),
						DTServerCreate,
						DTServerLastModified,
						DTTerminalCreate,
						DTTerminalLastModified,
						ProductProviderCode 
					FROM inserted
       END
       ELSE IF(SELECT COUNT(1) FROM deleted)>0
       BEGIN
              INSERT INTO tChannelPartnerPricing_Aud(
					ChannelPartnerPricingId,
					PricingGroupId,
					ChannelPartnerId,
					LocationId,
					ProductId,
					ProductType,
					RevisionNo,
					AuditEvent,
					DTAudit,
					DTServerCreate,
					DTServerLastModified,
					DTTerminalCreate,
					DTTerminalLastModified,
					ProductProviderCode)
					
					SELECT 
						ChannelPartnerPricingId,
						PricingGroupId,
						ChannelPartnerId,
						LocationId,
						ProductId,
						ProductType,
						@RevisionNo,
						3 AS AuditEvent,
						GETDATE(),
						DTServerCreate,
						DTServerLastModified,
						DTTerminalCreate,
						DTTerminalLastModified,
						ProductProviderCode 
					FROM deleted
       END


	   --================================================================================