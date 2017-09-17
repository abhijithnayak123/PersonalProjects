-- =============================================
-- Author:		<Shwetha Mohan>
-- Create date: <12/12/2016>
-- Description:Store Proc to Validate location
-- Jira ID:		<AL-7582>
-- =============================================
IF EXISTS (SELECT 1 FROM SYS.PROCEDURES WHERE object_id = OBJECT_ID(N'[dbo].[usp_ValidateLocation]') AND TYPE IN (N'P'))
DROP PROCEDURE [dbo].[usp_ValidateLocation]
GO

CREATE PROCEDURE usp_ValidateLocation
@bankId VARCHAR(40),
@branchId VARCHAR(40),
@locationIdentifier VARCHAR(50),
@locationName NVARCHAR(100),
@channelPartnerId SMALLINT
AS

BEGIN

	SET NOCOUNT ON;
	BEGIN TRY
		DECLARE @errorCode INT = 0
		
		IF EXISTS(SELECT 1 FROM tLocations WHERE LocationName = @locationName AND ChannelPartnerId = @channelPartnerId)
		BEGIN
			SET @errorCode = 1
		END	
		ELSE IF EXISTS(SELECT 1 FROM tLocations WHERE BankID = @bankId AND BranchID = @branchId AND ChannelPartnerId = @channelPartnerId)
		BEGIN
			SET @errorCode = 2
		END		
		ELSE IF EXISTS(SELECT 1 FROM tLocations WHERE LocationIdentifier = @locationIdentifier AND ChannelPartnerId = @channelPartnerId)
		BEGIN
			SET @errorCode = 3
		END
		SELECT @errorCode AS errorCode

	END TRY	
	BEGIN CATCH	        
	 
		EXECUTE usp_CreateErrorInfo;  
			
	END CATCH

END
GO
