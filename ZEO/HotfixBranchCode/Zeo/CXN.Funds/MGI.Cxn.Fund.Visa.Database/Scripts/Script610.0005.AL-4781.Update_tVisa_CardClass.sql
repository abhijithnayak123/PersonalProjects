-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <05/02/2016>
-- Description:	<Synovus Visa Card - On Card Maintenance screen when tried to request for a replacement card 
--				 that was in 'Lost Card' status, Getting error popup.>
-- Jira ID:		<AL-4781>
-- ================================================================================

DECLARE @channelPartnerId INT = 34 -- TCF channel partner 

UPDATE 
	tVisa_CardClass 
SET 
	ChannelPartnerId = @channelPartnerId 
WHERE 
	StateCode IN ('MI', 'CO', 'AZ', 'WI', 'IN', 'MN', 'SD', 'IL')
GO
