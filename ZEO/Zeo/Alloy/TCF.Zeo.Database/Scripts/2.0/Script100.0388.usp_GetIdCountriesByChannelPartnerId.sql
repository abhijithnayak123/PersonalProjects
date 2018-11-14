-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetIdCountriesByChannelPartnerId'
)
BEGIN
	DROP PROCEDURE usp_GetIdCountriesByChannelPartnerId
END
GO

CREATE PROCEDURE usp_GetIdCountriesByChannelPartnerId
	@channelPartnerId BIGINT
AS
BEGIN
	BEGIN TRY
	
	 DECLARE @countriesOrderList table(Name nvarchar(500))
		
		INSERT INTO 
			@countriesOrderList (Name) 
		VALUES 
			('UNITED STATES'),('CANADA'),('MEXICO')
		
		 
		INSERT INTO @countriesOrderList (Name) 
		SELECT 
			DISTINCT mc.Name
		FROM	
			tChannelPartnerIDTypeMapping cm WITH (NOLOCK)
			INNER JOIN tNexxoIdTypes nt WITH (NOLOCK) ON cm.NexxoIdTypeId = nt.NexxoIdTypeId
			INNER JOIN tMasterCountries mc WITH (NOLOCK) ON nt.MasterCountriesID = mc.MasterCountriesId
			INNER JOIN tChannelPartners cp WITH (NOLOCK) ON cp.ChannelPartnerId = cm.ChannelPartnerId
		WHERE
			cp.ChannelPartnerId = @channelPartnerId 
			AND cm.IsActive = 1
			AND mc.Name NOT IN(SELECT Name FROM @countriesOrderList)
		ORDER BY 
			MC.Name
	
		SELECT 
			Name 
		FROM 
			@countriesOrderList
			
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END