--- ===============================================================================
-- Author:		 Kaushik Sakala
-- Description:	 Script to populate the WU Country Currency
-- Jira ID:		<AL-8998>
-- ================================================================================

IF OBJECT_ID(N'usp_PopulateWUCountriesCurrency', N'P') IS NOT NULL
	DROP PROC usp_PopulateWUCountriesCurrency
GO

CREATE PROCEDURE usp_PopulateWUCountriesCurrency	
	@Currency XML
AS
BEGIN

 BEGIN TRY
        BEGIN TRANSACTION
        SET NOCOUNT ON;
		
		IF OBJECT_ID('#TempCurrency') IS NOT NULL
			DROP TABLE #TempCurrency
		
		IF((SELECT COUNT(1) FROM tWUnion_CountryCurrencies)> 0)
		BEGIN
			DELETE FROM tWUnion_CountryCurrencies
		END


		SELECT
			[Table].[Column].value('CountryName[1]', 'VARCHAR(20)') as 'CountryName',
			[Table].[Column].value('CountryCode[1]', 'VARCHAR(200)') as 'CountryCode',
			[Table].[Column].value('CountryNumCode[1]', 'VARCHAR(20)') as 'CountryNumCode',
			[Table].[Column].value('CurrencyCode[1]', 'VARCHAR(20)') as 'CurrencyCode',
			[Table].[Column].value('CurrencyNumCode[1]', 'VARCHAR(20)') as 'CurrencyNumCode',
			[Table].[Column].value('CurrencyName[1]', 'VARCHAR(20)') as 'CurrencyName',			
			[Table].[Column].value('DTServerCreate[1]', 'DATETIME') as 'DTServerCreate'
		INTO #TempCurrency
		FROM @Currency.nodes('/DocumentElement/Currency') as [Table]([Column])

		INSERT INTO 
			tWUnion_CountryCurrencies (CountryName,CountryCode,CountryNumCode,CurrencyCode,CurrencyNumCode,CurrencyName,DTServerCreate) 
			(SELECT ts.CountryName,ts.CountryCode,TS.CountryNumCode,ts.CurrencyCode,ts.CurrencyNumCode,ts.CurrencyName,DTServerCreate
			  FROM #TempCurrency ts)
		
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



