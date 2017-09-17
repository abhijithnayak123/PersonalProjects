-- ============================================================
-- Author:		Swarnalakshmi
-- Create date: Feb 05 2015
-- Description:	Insert/Update Promotions Setup
-- Rally ID:	<US1799><US1800>
-- ============================================================

DECLARE @PartnerId uniqueidentifier

SELECT @PartnerId = rowguid FROM tChannelPartners WHERE id = 33

-- Update Existing Adjustments as SystemApplied
UPDATE tChannelPartnerFeeAdjustments SET SystemApplied=1 WHERE ChannelPartnerPK=@PartnerId

IF NOT Exists( select 1 from tFeeAdjustmentConditionTypes where Id = 8)
BEGIN
-- Insert FeeAdjustmentCondition Types
	insert tFeeAdjustmentConditionTypes (Id, Name) values (8,'Referral'),(9, 'Code')
END