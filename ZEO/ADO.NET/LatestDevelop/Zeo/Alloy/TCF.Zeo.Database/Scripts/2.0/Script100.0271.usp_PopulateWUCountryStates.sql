--- ===============================================================================
-- Author:		 Kaushik Sakala
-- Description:	 Script to populate the WU States
-- Jira ID:		<AL-8998>
-- ================================================================================

IF OBJECT_ID(N'usp_PopulateWUCountryStates', N'P') IS NOT NULL
DROP PROC usp_PopulateWUCountryStates
GO

CREATE PROCEDURE usp_PopulateWUCountryStates	
	@States XML
AS
BEGIN

 BEGIN TRY
        BEGIN TRANSACTION
        SET NOCOUNT ON;
		
		IF OBJECT_ID('#TempStates') IS NOT NULL
	    DROP TABLE #TempStates

		IF((SELECT COUNT(1) FROM tWUnion_states WHERE ISOCountryCode = 'CA') = 0)
		BEGIN
			INSERT INTO [tWUnion_States] 
				([StateCode],[Name],[ISOCountryCode],[DTServerCreate])
			 VALUES           
				('AB','Alberta','CA',GETDATE()),	
				('BC','British Columbia','CA',GETDATE()),		
				('MB','Manitoba','CA',GETDATE()),	
				('NB','New Brunswick','CA',GETDATE()),
				('NL','Newfoundland and Labrador','CA',GETDATE()),
				('NT','Northwest Territories','CA',GETDATE()),	
				('NS','Nova Scotia','CA',GETDATE()),	
				('NU','Nunavut','CA',GETDATE()),	
				('ON','Ontario','CA',GETDATE()),	
				('PE','Prince Edward Island','CA',GETDATE()),	
				('QC','Quebec','CA',GETDATE()),	
				('SK','Saskatchewan','CA',GETDATE()),	
				('YT','Yukon','CA',GETDATE())
		END
		
		IF((SELECT COUNT(*) FROM tWUnion_states WHERE ISOCountryCode! = 'CA')> 0)
		BEGIN
			DELETE FROM tWUnion_states
			WHERE ISOCountryCode != 'CA'
		END

		SELECT
			[Table].[Column].value('StateCode[1]', 'VARCHAR(20)') as 'StateCode',
			[Table].[Column].value('Name[1]', 'VARCHAR(200)') as 'Name',
			[Table].[Column].value('ISOCountryCode[1]', 'VARCHAR(20)') as 'CountryCode',
			[Table].[Column].value('DTServerCreate[1]', 'DATETIME') as 'DTServerCreate'
		INTO #TempStates
		FROM @States.nodes('/DocumentElement/States') as [Table]([Column])

		INSERT INTO 
			tWUnion_states ([ISOCountryCode],[Name],[StateCode],[DTServerCreate]) 
			(SELECT ts.CountryCode,ts.Name,TS.StateCode,ts.DTServerCreate FROM #TempStates ts)
		
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



