--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <02-10-2016>
-- Description:	Create procedure for Create/update SSO Agent
-- Jira ID:		<AL-7581>
-- ================================================================================


IF OBJECT_ID(N'usp_CreateUpdateSSOAgent', N'P') IS NOT NULL
DROP PROCEDURE usp_CreateUpdateSSOAgent   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_CreateUpdateSSOAgent]
(
@UserName               NVARCHAR(225),
@ChannelPartnerId       BIGINT,
@FirstName              NVARCHAR(20),
@LastName               NVARCHAR(20),
@RoleId                 INT,
@TerminalName           NVARCHAR(100),
@ClientAgentIdentifier  NVARCHAR(20),
@DtServerLastModified   DATETIME
)
AS
BEGIN
	BEGIN TRY
	DECLARE @locationId  BIGINT=''
	IF (@TerminalName!='') 
	BEGIN
		SELECT
		@locationId= l.LocationID 
		FROM tTerminals t 
		LEFT JOIN tLocations l ON l.LocationID=t.LocationId 
		WHERE t.Name = @TerminalName and t.ChannelPartnerId=@ChannelPartnerId
	END     

    IF NOT EXISTS(SELECT 1 FROM tAgentDetails WHERE LOWER(UserName) = LOWER(@UserName) and ChannelPartnerId=@ChannelPartnerId)
	BEGIN
			INSERT INTO dbo.tAgentDetails
			(
			 ChannelPartnerId,
			 UserName,
			 FirstName,					  
			 LastName,
			 IsEnabled,
			 UserRoleId,
			 UserStatusId,
			 PrimaryLocationId,
			 ClientAgentIdentifier,
			 FullName
			)
			VALUES
			(
			 @ChannelPartnerId,
			 @UserName,
			 @FirstName,					  
			 @LastName,
			 1,
			 @RoleId,
			 1,
			 @locationId,
			 @ClientAgentIdentifier,
			 CONCAT(@FirstName,@LastName)
			 );
	END
	ELSE 
	BEGIN
			UPDATE 
				dbo.tAgentDetails
			SET
				ClientAgentIdentifier = @ClientAgentIdentifier,
				PrimaryLocationId = @locationId,
				DTServerLastModified=@DtServerLastModified
			WHERE 
				UserName = @UserName and channelPartnerId=@ChannelPartnerId
	END
    SELECT AgentID,
		   FirstName,
		   LastName,
		   UserName 
		   FROM tAgentDetails 
		   WHERE LOWER(UserName) = LOWER(@UserName) and ChannelPartnerId=@ChannelPartnerId 
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END