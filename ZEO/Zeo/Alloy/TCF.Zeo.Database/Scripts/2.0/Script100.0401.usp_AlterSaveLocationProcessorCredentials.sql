--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <07/03/2017>
-- Description:	Update location processor credential
-- Jira ID:		<2317: As Zeo, I want to share with Visa the dynamic location of the prepaid card transaction(s)>
-- ================================================================================


IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_SaveLocationProcessorCredentials]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].usp_SaveLocationProcessorCredentials
GO

CREATE PROCEDURE usp_SaveLocationProcessorCredentials
(
	@userName VARCHAR(510),
	@password VARCHAR(100),
	@identifier VARCHAR(100),
	@identifier2 VARCHAR(100),
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
				Identifier2 = @identifier2, 
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
					Identifier2,
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
					@identifier2,
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


