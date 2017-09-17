-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <05/02/2016>
-- Description:	<Synovus Visa Card - On Card Maintenance screen when tried to request for a replacement card 
--				 that was in 'Lost Card' status,Getting error popup.>
-- Jira ID:		<AL-4781>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tVisa_CardClass' AND COLUMN_NAME = 'ChannelPartnerId')
BEGIN
ALTER TABLE tVisa_CardClass ADD ChannelPartnerId [INT] 
END
GO
