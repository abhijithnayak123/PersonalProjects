--- ===============================================================================
-- Author:		 Kaushik Sakala
-- Description:	 Script to populate the WU States
-- Jira ID:		<AL-8998>
-- ================================================================================

IF OBJECT_ID(N'usp_PopulateWUMexxicoCities', N'P') IS NOT NULL
	DROP PROC usp_PopulateWUMexxicoCities
GO

CREATE PROCEDURE usp_PopulateWUMexxicoCities	
	@City XML
AS
BEGIN

 BEGIN TRY
        BEGIN TRANSACTION
        SET NOCOUNT ON;
		
		IF OBJECT_ID('#TempCities') IS NOT NULL
			DROP TABLE #TempCities

		IF((SELECT COUNT(1) FROM tWUnion_Cities)> 0)
		BEGIN
			DELETE FROM tWUnion_Cities
		END

		SELECT
			[Table].[Column].value('Name[1]', 'VARCHAR(200)') as 'Name',
			[Table].[Column].value('StateCode[1]', 'VARCHAR(20)') as 'StateCode',
			[Table].[Column].value('DTServerCreate[1]', 'DATETIME') as 'DTServerCreate'
		INTO #TempCities
		FROM @City.nodes('/DocumentElement/City') as [Table]([Column])

		INSERT INTO 
			tWUnion_Cities ([Name],[StateCode],[DTServerCreate]) 
			(SELECT ts.Name,TS.StateCode,ts.DTServerCreate FROM #TempCities ts)
		
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



