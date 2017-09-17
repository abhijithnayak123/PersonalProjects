--- ===============================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>	
-- Description:	 Create stored procedure to get the location Processor credentials by location id
-- Jira ID:		<AL-7582>
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetLocationProcessorCredentials]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[usp_GetLocationProcessorCredentials]
GO


Create Procedure usp_GetLocationProcessorCredentials
(
	@locationId BIGINT
)
AS 
BEGIN
	BEGIN TRY
		Select
			ProviderId,
			UserName,
			Password,
			Identifier
		FROM tLocationProcessorCredentials
		WHERE LocationId = @locationId

	END TRY

BEGIn CATCH
		EXECUTE usp_CreateErrorInfo;  
END CATCH

END 
GO

