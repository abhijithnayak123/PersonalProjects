-- ================================================================
-- Author:		Karun
-- Create date: <01/Oct/2015>
-- Description:	<As Carver, change the minimum price for check cashing to $2 instead of $3.>
-- JIRA ID:	<AL-2157>
-- =================================================================

DECLARE @ChannelPartnerPK UNIQUEIDENTIFIER

SELECT @ChannelPartnerPK = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'Carver'
UPDATE tChannelPartnerFees_Check SET FeeMinimum = 2.00 WHERE ChannelPartnerPK = @ChannelPartnerPK

GO