-- =============================================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <07/02/2015>
-- Description:	<Script for insert RedStone check franking data>
-- Jira ID:	<AL-438>
-- ==============================================================================================
DECLARE @PartnerPK UNIQUEIDENTIFIER		
SELECT @PartnerPK = ChannelPartnerPK FROM tChannelPartners WHERE ChannelPartnerId = 35

IF EXISTS (SELECT 1 FROM [dbo].[tChannelPartnerConfig] WHERE [ChannelPartnerPK] = @PartnerPK)
BEGIN
UPDATE [dbo].[tChannelPartnerConfig]
SET [IsCheckFrank] = 1,
[FrankData] = 'FRANKED| BranchID'
WHERE ChannelPartnerPK = @PartnerPK
END
GO