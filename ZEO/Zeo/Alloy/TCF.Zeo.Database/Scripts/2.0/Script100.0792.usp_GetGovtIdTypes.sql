--- ===============================================================================
-- Author:		 M.Purna Pushkal
-- Description: Creating the table to store the Government Id mappings
-- Version-One: B-20520: Tech Debt - Moving the Customer Id Mapping to Database from Code
-- ================================================================================

IF OBJECT_ID(N'usp_GetGovtIdTypes', N'P') IS NOT NULL
DROP PROC usp_GetGovtIdTypes
GO

CREATE PROCEDURE usp_GetGovtIdTypes
(	
	@providerId INT
)
AS
BEGIN			
BEGIN TRY

	SELECT 
		IdType,
		IdTypeValue
	FROM 
		tGovtIdMapping tg
	WHERE
		tg.ProviderId = @providerId

END TRY

BEGIN CATCH
    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError		
END CATCH

END
GO