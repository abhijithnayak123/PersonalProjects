IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetIdTypesByChannelPartnerId'
)
BEGIN
	DROP PROCEDURE usp_GetIdTypesByChannelPartnerId
END
GO

CREATE PROCEDURE usp_GetIdTypesByChannelPartnerId
	@channelPartnerId BIGINT,
	@countryName NVARCHAR(200)
AS
BEGIN
	BEGIN TRY
		SELECT 
			DISTINCT n.Name 
		FROM 
			tChannelPartnerIDTypeMapping cp  WITH (NOLOCK)  
			INNER JOIN tNexxoIdTypes n  WITH (NOLOCK) ON cp.NexxoIdTypeId = n.NexxoIdTypeId
			INNER JOIN tMasterCountries m  WITH (NOLOCK) ON n.MasterCountriesID = m.MasterCountriesID
			INNER JOIN tChannelPartners c  WITH (NOLOCK) ON cp.ChannelPartnerId = c.ChannelPartnerId
		WHERE 
		  c.ChannelPartnerId = @channelPartnerId AND 
		  m.Name = @countryName AND
		  cp.IsActive = 1
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END