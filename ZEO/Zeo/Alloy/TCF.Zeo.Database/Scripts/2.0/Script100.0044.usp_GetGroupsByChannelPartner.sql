-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS(
	SELECT 1
	FROM SYS.objects
	WHERE NAME = 'usp_GetGroupsByChannelPartner'
)

BEGIN
	DROP PROCEDURE usp_GetGroupsByChannelPartner
END
GO

CREATE PROCEDURE usp_GetGroupsByChannelPartner
	@channelPartnerId BIGINT
AS
BEGIN
	BEGIN TRY
		SELECT
			cg.ChannelPartnerGroupId, cg.Name
		FROM 
			tChannelPartnerGroups cg WITH (NOLOCK)
			INNER JOIN tChannelPartners c WITH (NOLOCK) ON cg.ChannelPartnerPK = c.ChannelPartnerPK
		WHERE 
			c.ChannelPartnerId= @channelPartnerId
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END