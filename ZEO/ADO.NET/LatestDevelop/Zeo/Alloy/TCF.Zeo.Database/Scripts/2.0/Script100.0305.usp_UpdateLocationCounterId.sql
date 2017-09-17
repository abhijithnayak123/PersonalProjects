--- ===============================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>
-- Description:	 Create stored procedure to update the counter ID
-- Jira ID:		<AL-7582>

--- EXEC usp_UpdateLocationCounterId 1000000007,1000000006,1,'02/02/2017','02/02/2017'
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateLocationCounterId]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[usp_UpdateLocationCounterId]
GO


CREATE PROCEDURE usp_UpdateLocationCounterId
(
	@customerSessionId BIGINT,
	@locationId INT,
	@isAvailable BIT,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME
)
AS
BEGIN
    
	BEGIN TRY       
		DECLARE @counterId NVARCHAR(100) =
		(
			SELECT CounterId 
			FROM tCustomerSessionCounterIdDetails
			WHERE CustomerSessionID = @customerSessionId
		)

		DECLARE @locationCounterIdDetailID BIGINT =
		(
			SELECT TOP 1 LocationCounterIdDetailID 
			FROM tLocationCounterIdDetails 
			WHERE LocationId = @locationId AND IsAvailable= 0 AND CounterId = @counterId 
		)
   	
		UPDATE tLocationCounterIdDetails  
		SET 
			IsAvailable = @isAvailable,
			DTServerLastModified =@dTServerLastModified,
			DTTerminalLastModified = @dTTerminalLastModified
		WHERE 
			LocationCounterIdDetailID = @locationCounterIdDetailID  

	END TRY
	BEGIN CATCH	        	
			EXECUTE usp_CreateErrorInfo; 			
    END CATCH
END
GO



	