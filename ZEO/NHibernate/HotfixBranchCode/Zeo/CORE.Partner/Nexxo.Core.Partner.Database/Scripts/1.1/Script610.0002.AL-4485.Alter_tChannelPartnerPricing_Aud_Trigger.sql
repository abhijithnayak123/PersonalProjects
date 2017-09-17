-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <18/01/2016>
-- Description:	<As Engineering, I need to have consistent naming convention for Id and PK columns>
-- Jira ID:		<AL-4485>
-- ================================================================================
IF EXISTS( 
	SELECT 1
	FROM SYS.TRIGGERS
	WHERE NAME = 'tChannelPartnerPricing_Audit'
)
BEGIN
	DROP trigger [dbo].[tChannelPartnerPricing_Audit]
	END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
					ChannelPartnerPricingId,
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
						Inserted.ChannelPartnerPricingId,
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
					ChannelPartnerPricingId,
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
						ChannelPartnerPricingId,
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
					ChannelPartnerPricingId,
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
						ChannelPartnerPricingId,
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


