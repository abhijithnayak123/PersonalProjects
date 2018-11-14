-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>

-- EXEC usp_GetCountries 34
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_GetCountries'
)
BEGIN
	DROP PROCEDURE usp_GetCountries
END
GO

CREATE PROCEDURE usp_GetCountries
(
	 @channelPartnerId SMALLINT
)
AS
BEGIN
	BEGIN TRY

		DECLARE @countriesOrderList table(Name nvarchar(250))
		
		INSERT INTO 
			@countriesOrderList (Name) 
		VALUES 
			('UNITED STATES'),('CANADA'),('MEXICO')
		
		 
		INSERT INTO @countriesOrderList (Name) 
			SELECT 
				DISTINCT MC.Name
			FROM 
				 tChannelPartnerIDTypeMapping tcpim
				 INNER JOIN tNexxoIdTypes tnit ON tnit.NexxoIdTypeID = tcpim.NexxoIdTypeID
				 INNER JOIN tMasterCountries MC ON tnit.MasterCountriesId = MC.MasterCountriesId
				 INNER JOIN tChannelPartners CP ON CP.ChannelPartnerId = tcpim.ChannelPartnerId
			 WHERE 
				 CP.ChannelPartnerId = @channelPartnerId 
				 AND tcpim.IsActive = 1 
				 AND MC.Name NOT IN(SELECT Name FROM @countriesOrderList)
				 ORDER BY MC.Name

		SELECT 
			Name 
		FROM 
			@countriesOrderList
		
	END TRY
	BEGIN CATCH	        

      EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END