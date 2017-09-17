--- ===============================================================================
-- Author:		 Kaushik Sakala
-- Description:	 Script to populate the WU Country Translations
-- Jira ID:		<AL-8998>
-- ================================================================================

IF OBJECT_ID(N'usp_PopulateCountryTranslations', N'P') IS NOT NULL
DROP PROC usp_PopulateCountryTranslations
GO

CREATE PROCEDURE usp_PopulateCountryTranslations	
	@Translations XML
AS
BEGIN

 BEGIN TRY
        BEGIN TRANSACTION
        SET NOCOUNT ON;
		
		IF OBJECT_ID('#TempTranslations') IS NOT NULL
		 DROP TABLE #TempTranslations

		IF((SELECT COUNT(1) FROM tWunion_CountryTranslation)> 0)
		BEGIN
			DELETE FROM tWunion_CountryTranslation
		END
	
		SELECT
			[Table].[Column].value('CountryCode[1]', 'VARCHAR(20)') as 'CountryCode',
			[Table].[Column].value('Name[1]', 'VARCHAR(200)') as 'Name',
			[Table].[Column].value('Language[1]', 'VARCHAR(20)') as 'Language',
			[Table].[Column].value('DTServerCreate[1]', 'DATETIME') as 'DTServerCreate'
		INTO #TempTranslations
		FROM @Translations.nodes('/DocumentElement/Translations') as [Table]([Column])

		INSERT INTO 
			tWunion_CountryTranslation 
			([LanguageCode],[TranslationName],[ISOCountryCode],[DTServerCreate]) 
			(SELECT ts.Language,ts.Name,TS.CountryCode,ts.DTServerCreate 
			 FROM #TempTranslations ts)
		
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



