	--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <09-24-2018>
-- Description: Updating the Feature List with its status as selected in the UI.
-- ================================================================================

IF OBJECT_ID(N'usp_UpdateFeatures', N'P') IS NOT NULL
DROP PROC usp_UpdateFeatures
GO

CREATE PROCEDURE usp_UpdateFeatures	
(
	@featuresList XML
)
AS
BEGIN		
		
	BEGIN TRY   
		
			DECLARE @featuresListTable TABLE
			(	
				FeatureID BIGINT
				,IsEnable BIT
				,DTTerminalDate DATETIME
			)

			INSERT INTO @featuresListTable 
			(			
				FeatureID, 
				IsEnable,
				DTTerminalDate
			)	
			(
				SELECT 
					FeaturesList.value('FeatureID[1]', 'BIGINT') AS FeatureID,     
					FeaturesList.value('IsEnable[1]', 'BIT') AS IsEnable,
					FeaturesList.value('DTTerminalDate[1]', 'DATETIME') AS DTTerminalDate
				 FROM 
					@featuresList.nodes('/FeaturesList/Feature') AS Features(FeaturesList)
			)

			UPDATE tf
			SET tf.IsEnable = tft.IsEnable, DTTerminalLastModified = tft.DTTerminalDate, DTServerLastModified = GETDATE()
			FROM tFeatures tf
				INNER JOIN @featuresListTable tft ON tf.FeatureID = tft.FeatureID

  END TRY

  BEGIN CATCH			
		EXECUTE usp_CreateErrorInfo;  
  END CATCH

END
GO

