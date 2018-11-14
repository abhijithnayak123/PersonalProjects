--- ===============================================================================
-- Author:		<Nishad Varghese>
-- Create date: <07-25-2016>
-- Description:	Changes to triggers
-- Jira ID:		<AL-7580>
-- ================================================================================

ALTER TRIGGER [dbo].[tChannelPartnerPricing_Audit] ON [dbo].[tChannelPartnerPricing] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
	   DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tChannelPartnerPricing_Aud WHERE ChannelPartnerPricingID = (SELECT ChannelPartnerPricingID FROM inserted)
             
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
					DTTerminalCreate,DTTerminalLastModified)
					
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
						DTTerminalCreate,DTTerminalLastModified 
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
					DTTerminalCreate,DTTerminalLastModified)
					
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
						DTTerminalCreate,DTTerminalLastModified 
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
					DTTerminalCreate,DTTerminalLastModified)
					
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
						DTTerminalLastModified 
					FROM deleted
       END


	   --================================================================================

