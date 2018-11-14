--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <12-13-2016>
-- Description:	Create procedure for  SSO Agent Session
-- Jira ID:		<AL-7581>
-- ================================================================================


IF OBJECT_ID(N'usp_CreateAgentSession', N'P') IS NOT NULL
DROP PROCEDURE usp_CreateAgentSession   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_CreateAgentSession]
(
@AgentSessionID         BIGINT OUTPUT,
@DtServerCreate         DATETIME,
@AgentID                BIGINT,
@TerminalName           NVARCHAR(100),
@BusinessDate           DATETIME,
@ClientAgentIdentifier  NVARCHAR(20),
@ChannelPartnerId       BIGINT,
@LocationName           NVARCHAR(100) OUTPUT,
@BankID                 VARCHAR(40) OUTPUT,
@BranchID               VARCHAR(40) OUTPUT,
@ChannelPartnerName     NVARCHAR(50) OUTPUT,
@Description            VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
	DECLARE @TerminalId  BIGINT=null
	DECLARE @channelPartnerIdValue uniqueidentifier =
	(
		SELECT ChannelPartnerPK 
		FROM   tChannelPartners
		WHERE  ChannelPartnerId = @ChannelPartnerId
	)

	SELECT @ChannelPartnerName=tc.Name FROM tChannelPartners tc WHERE tc.ChannelPartnerId=@ChannelPartnerId
	IF (@TerminalName IS NOT NULL and @ChannelPartnerId IS NOT NULL) 
	BEGIN
		SELECT
		@TerminalId= t.TerminalID,
		@LocationName=l.LocationName,
		@BankID=l.BankID,
		@BranchID=l.BranchID,
		@Description=N.Description
		FROM tTerminals t 
		INNER JOIN tLocations l on l.LocationPK=t.LocationPK
		LEFT JOIN tNpsTerminals N on N.NpsTerminalPK=t.NpsTerminalPK
		WHERE t.Name = @TerminalName and t.ChannelPartnerPK=@channelPartnerIdValue
	END     
    	INSERT INTO dbo.tAgentSessions
			(
			 AgentId,
			 ClientAgentIdentifier,
			 TerminalId,
			 DTServerCreate,
			 BusinessDate
			)
			VALUES
			(
			 @AgentID,
			 @ClientAgentIdentifier,
			 @TerminalId,
			 @DtServerCreate,
			 @BusinessDate);
			set @AgentSessionID =scope_Identity();
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END