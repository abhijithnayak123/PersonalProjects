--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <06-25-2018>
-- Description:	 Inserting the list of Blocked Countries.
-- ================================================================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_InsertBlockedCountries'
)
BEGIN
	DROP PROCEDURE usp_InsertBlockedCountries
END
GO

CREATE PROCEDURE [dbo].[usp_InsertBlockedCountries]
(
	@blockedCountries XML
)
AS
BEGIN
	BEGIN TRY

	 		DECLARE @blockedCountriesTable TABLE
			(	
				 ISOCountryCode [varchar](20)
				,DTServerDate DATETIME
				,DTTerminalDate DATETIME
			)


			INSERT INTO @blockedCountriesTable 
			(			
				ISOCountryCode, 
				DTServerDate, 
				DTTerminalDate
			)	
			(
				SELECT 
					BlockedCountries.value('ISOCountryCode[1]', 'VARCHAR(20)') AS ISOCountryCode,     
					BlockedCountries.value('DTServerDate[1]', 'DATETIME') AS DTServerDate,
					BlockedCountries.value('DTTerminalDate[1]', 'DATETIME') AS DTTerminalDate
				 FROM 
					@blockedCountries.nodes('/BlockedCountries/BlockedCountry') AS Countries(BlockedCountries)
			)

			DELETE FROM tBlockedCountries

			INSERT INTO tBlockedCountries
			(
				ISOCountryCode,
				DTServerCreate,
				DTTerminalCreate
			)
			( 
			   SELECT 
				 ISOCountryCode,
				 DTServerDate, 
				 DTTerminalDate
			   FROM 
				 @blockedCountriesTable
			)

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END