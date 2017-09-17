-- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <02/09/2017>
-- Description:	<Added condition to get the records between start date and ene date>
-- Jira ID:		<>
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
	@channelPartnerId BIGINT,
	@dTToday          DATETIME
AS
BEGIN
	BEGIN TRY
		SELECT
			cg.ChannelPartnerGroupId, cg.Name
		FROM 
			tChannelPartnerGroups cg WITH (NOLOCK)
			INNER JOIN tChannelPartners c WITH (NOLOCK) ON cg.ChannelPartnerID = c.ChannelPartnerId
		WHERE 
			c.ChannelPartnerId = @channelPartnerId AND @dTToday BETWEEN DTStart AND DTEnd
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END