-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetIdStatesByChannelPartnerId'
)
BEGIN
	DROP PROCEDURE usp_GetIdStatesByChannelPartnerId
END
GO

CREATE PROCEDURE usp_GetIdStatesByChannelPartnerId
	@channelPartnerId BIGINT,
	@country NVARCHAR(140),
	@idType NVARCHAR(200)
	
AS
BEGIN
	BEGIN TRY
		SELECT 
			DISTINCT s.Name 
		FROM 
			tChannelPartnerIDTypeMapping cp  WITH (NOLOCK)  
			INNER JOIN tNexxoIdTypes n  WITH (NOLOCK) ON cp.NexxoIdTypeId = n.NexxoIdTypeId
			INNER JOIN tMasterCountries m  WITH (NOLOCK) ON n.MasterCountriesID = m.MasterCountriesID 
			INNER JOIN tChannelPartners c  WITH (NOLOCK) ON cp.ChannelPartnerId = c.ChannelPartnerID
			INNER JOIN tStates s WITH (NOLOCK) ON s.StateId = n.StateId
			
		WHERE 
		  c.ChannelPartnerId = @channelPartnerId 
		  AND n.Name = @idType  
		  AND m.Name = @country
		  AND cp.IsActive = 1
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END