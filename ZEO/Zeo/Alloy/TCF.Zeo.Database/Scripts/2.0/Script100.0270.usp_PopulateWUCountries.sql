--- ===============================================================================
-- Author:		 Kaushik Sakala
-- Description:	 Script to populate the WU Countries
-- Jira ID:		<AL-8998>
-- ================================================================================

IF OBJECT_ID(N'usp_PopulateWUCountries', N'P') IS NOT NULL
DROP PROC usp_PopulateWUCountries
GO

CREATE PROCEDURE usp_PopulateWUCountries	
	@Countries XML
AS
BEGIN

 BEGIN TRY
        BEGIN TRANSACTION
        SET NOCOUNT ON;
		
		IF OBJECT_ID('#TempCountries') IS NOT NULL
	    DROP TABLE #TempCountries

		IF ((SELECT COUNT(*) FROM tWUnion_Countries) > 0)
		BEGIN
			 DELETE FROM tWUnion_Countries
		END

		SELECT
		[Table].[Column].value('CountryCode[1]', 'VARCHAR(20)') as 'CountryCode',
		[Table].[Column].value('Name[1]', 'VARCHAR(200)') as 'Name',
		[Table].[Column].value('DTServerCreate[1]', 'DATETIME') as 'DTServerCreate'
		INTO #TempCountries
		FROM @Countries.nodes('/DocumentElement/Countries') as [Table]([Column])

		INSERT INTO 
			tWUnion_Countries ([ISOCountryCode],[Name],[DTServerCreate]) 
			(SELECT tc.CountryCode,tc.Name,tc.DTServerCreate FROM #TempCountries tc)
		
        COMMIT TRANSACTION;	          
  END TRY
  BEGIN CATCH

    IF @@TRANCOUNT > 0 
    BEGIN   	  
        ROLLBACK TRANSACTION 		
    END

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

  END CATCH
END
GO



