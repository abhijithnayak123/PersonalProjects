	--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <09-24-2018>
-- Description:	Get all the Features to be displayed in the screen.
-- ================================================================================

IF OBJECT_ID(N'usp_GetFeatures', N'P') IS NOT NULL
DROP PROC usp_GetFeatures
GO

CREATE PROCEDURE usp_GetFeatures	
AS
BEGIN		
		
	BEGIN TRY   
		
	SELECT 
		FeatureID,
		Name,
		IsEnable
	FROM 
		tFeatures

  END TRY

  BEGIN CATCH			
		EXECUTE usp_CreateErrorInfo;  
  END CATCH

END
GO

