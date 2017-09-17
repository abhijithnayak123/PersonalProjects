-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <22/03/2016>
-- Description:	<As TCF, I should be able to do VISA GPR transactions without 
--					tlocationprocessorcredentials record>
-- Jira ID:		<AL-6019>
-- ================================================================================

BEGIN

	DELETE 
		tlp 
	FROM 
		tLocationProcessorCredentials tlp 
	INNER JOIN 
		tLocations tl ON tlp.LocationId = tl.LocationPK
	 WHERE 
		tlp.ProviderId = 103 AND tl.ChannelPartnerId = 34
	END

GO
