--- ===============================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>
-- Description:	Update location processor credential
-- Jira ID:		<AL-7582>
-- ================================================================================
-- EXEC usp_SaveLocationProcessorCredentials '13139925','13139925',0, 0,5,'2017-01-31','2017-01-31','2017-01-31','2017-01-31'
SELECT GETDATE();
IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_SaveLocationProcessorCredentials]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].usp_SaveLocationProcessorCredentials
GO

CREATE PROCEDURE usp_SaveLocationProcessorCredentials
(
	@userName VARCHAR(255),
	@password VARCHAR(50),
	@identifier VARCHAR(50),
	@providerId INT,
	@locationId	BIGINT,
	@dTTerminalCreate DATETIME,
	@dTServerCreate DATETIME,
	@dtTerminalLastModified DATETIME,
	@dtServerLastModified DATETIME
)
AS
BEGIN

    SET NOCOUNT ON;

	BEGIN TRY   	  
	 IF EXISTS ( SELECT 1 FROM tLocationProcessorCredentials WHERE ProviderId = @providerId AND LocationId = @locationId)
		 UPDATE tLocationProcessorCredentials 
			SET 
				UserName = @userName, 
				[Password] = @password, 
				Identifier = @identifier, 
				DTServerLastModified = @dtServerLastModified, 
				DTTerminalLastModified = @dtTerminalLastModified
			 WHERE 
				LocationId = @locationId AND ProviderId = @providerId
		ELSE
		BEGIN
			INSERT INTO 
				tLocationProcessorCredentials
				(
					ProviderId,
					UserName,
					Password,
					Identifier,
					locationId,
					DTServerCreate,
					DTTerminalCreate
				) 		
				VALUES
				(
					@providerId,
					@userName,
					@password, 
					@identifier,
					@locationId, 
					@dTServerCreate,
					@dTTerminalCreate
				 )
			END
	END TRY
	BEGIN CATCH	        

		EXECUTE usp_CreateErrorInfo;  
			
	END CATCH
END
GO


