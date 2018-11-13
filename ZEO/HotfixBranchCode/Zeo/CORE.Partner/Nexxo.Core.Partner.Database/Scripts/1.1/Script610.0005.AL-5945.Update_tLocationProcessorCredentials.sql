--==================================================================================================
-- Author:		<KAUSHIK SAKALA>
-- Created date: <18/03/2016>
-- Description:	<As Synovus, the Visa location identifier should be passed in the API for card activation, load and unload actions>           
-- Jira ID:	<AL-5945>
--===================================================================================================

UPDATE 
	tLocationProcessorCredentials
SET 
	Identifier = '-1'
FROM 
	tlocations l 
JOIN 
	tLocationProcessorCredentials lp 
ON 
	l.LocationPK = lp.LocationId 
WHERE 
	l.ChannelPartnerId = 34 AND l.LocationName = 'TCF Service Desk' AND lp.ProviderId = 103