--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <11/04/2015>
-- Description:	<As Alloy, I want to have an audit log for pricing cluster changes>
-- Jira ID:		<AL-2747>
-- ================================================================================

IF EXISTS( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'tChannelPartnerPricing_Audit'
)
BEGIN
	DROP TRIGGER [dbo].[tChannelPartnerPricing_Audit]
	END
GO


CREATE TRIGGER [dbo].[tChannelPartnerPricing_Audit] ON [dbo].[tChannelPartnerPricing] AFTER INSERT, UPDATE, DELETE
AS
       SET NOCOUNT ON
	   DECLARE @RevisionNo BIGINT
       SELECT @RevisionNo = ISNULL(MAX(RevisionNo),0) + 1 FROM tChannelPartnerPricing_Aud WHERE ChannelPartnerPricingPK = (SELECT ChannelPartnerPricingPK FROM inserted)
             
       IF ((SELECT COUNT(*) FROM inserted)<>0 AND (SELECT COUNT(*) FROM deleted)>0)
       BEGIN
              INSERT INTO tChannelPartnerPricing_Aud(
					ChannelPartnerPricingPK,
					Id,
					PricingGroupPK,
					ChannelPartnerPK,
					LocationPK,
					ProductPK,
					ProductType,
					RevisionNo,
					AuditEvent,
					DTAudit,
					DTServerCreate,
					DTServerLastModified,
					DTTerminalCreate,DTTerminalLastModified)
					
					SELECT 
						ChannelPartnerPricingPK,
						Inserted.Id,
						PricingGroupPK,
						ChannelPartnerPK,
						LocationPK,
						ProductPK,
						ProductType,
						@RevisionNo,
						2 as AuditEvent,
						GETDATE(),
						DTServerCreate,
						DTServerLastModified,
						DTTerminalCreate,DTTerminalLastModified 
					FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM inserted)>0 AND (SELECT COUNT(*) FROM deleted)=0
       BEGIN
              INSERT INTO tChannelPartnerPricing_Aud(
					ChannelPartnerPricingPK,
					Id,
					PricingGroupPK,
					ChannelPartnerPK,
					LocationPK,
					ProductPK,
					ProductType,
					RevisionNo,
					AuditEvent,
					DTAudit,
					DTServerCreate,
					DTServerLastModified,
					DTTerminalCreate,DTTerminalLastModified)
					
					SELECT ChannelPartnerPricingPK,
						Id,
						PricingGroupPK,
						ChannelPartnerPK,
						LocationPK,
						ProductPK,
						ProductType,
						@RevisionNo,
						1 AS AuditEvent,
						GETDATE(),
						DTServerCreate,
						DTServerLastModified,
						DTTerminalCreate,DTTerminalLastModified 
					FROM inserted
       END
       ELSE IF(SELECT COUNT(*) FROM deleted)>0
       BEGIN
              INSERT INTO tChannelPartnerPricing_Aud(
					ChannelPartnerPricingPK,
					Id,
					PricingGroupPK,
					ChannelPartnerPK,
					LocationPK,
					ProductPK,
					ProductType,
					RevisionNo,
					AuditEvent,
					DTAudit,
					DTServerCreate,
					DTServerLastModified,
					DTTerminalCreate,DTTerminalLastModified)
					
					SELECT 
						ChannelPartnerPricingPK,
						Id,
						PricingGroupPK,
						ChannelPartnerPK,
						LocationPK,
						ProductPK,
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
GO
