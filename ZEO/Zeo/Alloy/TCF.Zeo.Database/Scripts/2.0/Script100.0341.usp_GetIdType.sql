-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <02/02/2017>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>

-- EXEC usp_GetIdType 34,'UNITED STATES','DRIVER''S LICENSE',''
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetIdType'
)
BEGIN
	DROP PROCEDURE usp_GetIdType
END
GO

CREATE PROCEDURE usp_GetIdType
	@channelPartnerId BIGINT,
	@country NVARCHAR(140),
	@idType NVARCHAR(200),
	@state NVARCHAR(100)
	
AS
BEGIN
	BEGIN TRY
		SELECT 
			n.Name,
			n.HasExpirationDate,
			n.Mask
		FROM 
			tChannelPartnerIDTypeMapping cp  WITH (NOLOCK)
			INNER JOIN tNexxoIdTypes n  WITH (NOLOCK) ON cp.NexxoIdTypeId = n.NexxoIdTypeId
			INNER JOIN tMasterCountries m  WITH (NOLOCK) ON n.MasterCountriesId = m.MasterCountriesId
			INNER JOIN tChannelPartners c  WITH (NOLOCK) ON cp.ChannelPartnerId = c.ChannelPartnerID
			LEFT JOIN tStates s WITH (NOLOCK) ON s.StateId = n.StateId
		WHERE 
		  c.ChannelPartnerId = @channelPartnerId 
		  AND n.Name = @idType  
		  AND m.Name = @country
		  AND ISNULL(S.Name,'') = ISNULL(@state,'')
		  AND cp.IsActive = 1

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END