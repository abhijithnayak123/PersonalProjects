-- =============================================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <07/02/2015>
-- Description:	<Script for adding check processing fee for Redstone>
-- Jira ID:	<AL-438>
-- ==============================================================================================

DECLARE @PartnerPK UNIQUEIDENTIFIER,
		@FeeMin DECIMAL
SELECT @PartnerPK = ChannelPartnerPK FROM tChannelPartners WHERE ChannelPartnerId = 35
SET @FeeMin = 0.00

IF NOT EXISTS (SELECT 1 FROM [dbo].[tChannelPartnerFees_Check] WHERE [ChannelPartnerPK] = @PartnerPK)
BEGIN
INSERT tChannelPartnerFees_Check
	(ChannelPartnerPK, CheckTypePK, FeeRate, FeeMinimum)
VALUES 
	(@PartnerPK, 1, 0.030, @FeeMin),
	(@PartnerPK, 2, 0.010, @FeeMin),
	(@PartnerPK, 3, 0.010, @FeeMin),
	(@PartnerPK, 4, 0.010, @FeeMin),
	(@PartnerPK, 5, 0.030, @FeeMin),
	(@PartnerPK, 6, 0.030, @FeeMin),
	(@PartnerPK, 7, 0.010, @FeeMin),
	(@PartnerPK, 8, 0.010, @FeeMin),
	(@PartnerPK, 10, 0.010, @FeeMin),
	(@PartnerPK, 14, 0.010, @FeeMin)	
END
GO