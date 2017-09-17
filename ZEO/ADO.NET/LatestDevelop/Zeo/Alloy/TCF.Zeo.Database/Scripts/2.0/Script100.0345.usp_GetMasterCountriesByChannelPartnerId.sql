-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetMasterCountriesByChannelPartnerId'
)
BEGIN
	DROP PROCEDURE usp_GetMasterCountriesByChannelPartnerId
END
GO

CREATE PROCEDURE usp_GetMasterCountriesByChannelPartnerId
	@channelPartnerId BIGINT
AS
BEGIN
	BEGIN TRY
	
		DECLARE @countriesOrderList table(Name nvarchar(200),Abbr2 nvarchar(2),Abbr3 nvarchar(3))
		
		INSERT INTO 
			@countriesOrderList (Name, Abbr2, Abbr3) 
		VALUES 
			('UNITED STATES','US','USA'),('CANADA','CA','CAN'),('MEXICO','MX','MEX')
	
		INSERT INTO @countriesOrderList (Name, Abbr2, Abbr3)
		SELECT 
			MC.Name,MC.Abbr2,MC.Abbr3 
		FROM 
			tChannelPartnerMasterCountryMapping CM 
			INNER JOIN tMasterCountries MC ON CM.MasterCountriesId = MC.MasterCountriesId
			INNER JOIN tChannelPartners CP ON CP.ChannelPartnerId = CM.ChannelPartnerId
		WHERE 
			CP.ChannelPartnerId = @channelPartnerId 
			AND CM.IsActive = 1 
			AND MC.Name NOT IN(SELECT Name FROM @countriesOrderList)
		ORDER BY 
			MC.Name
		
		SELECT 
			Name, Abbr2, Abbr3
		FROM 
			@countriesOrderList
		 
	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END