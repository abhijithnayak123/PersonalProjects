--- ===============================================================================
-- Author:		<Shwetha Mohan>
-- Create date:  <01/21/2017>
-- Description:	Create procedure for  SSO Agent Session
-- Jira ID:		<AL-7581>
-- ================================================================================


IF OBJECT_ID(N'usp_CreateAgentSession', N'P') IS NOT NULL
DROP PROCEDURE usp_CreateAgentSession   -- Drop the existing procedure.
GO

CREATE PROCEDURE [dbo].[usp_CreateAgentSession]
(
@agentSessionID         BIGINT OUTPUT,
@dtServerCreate         DATETIME,
@agentID                BIGINT,
@terminalName           NVARCHAR(100),
@businessDate           DATETIME,
@clientAgentIdentifier  NVARCHAR(20),
@channelPartnerId       BIGINT,
@locationName           NVARCHAR(100) OUTPUT,
@bankID                 VARCHAR(40) OUTPUT,
@branchID               VARCHAR(40) OUTPUT,
@channelPartnerName     NVARCHAR(50) OUTPUT,
@description            VARCHAR(500) OUTPUT,
@peripheralServiceUrl	VARCHAR(1000) OUTPUT,
@TerminalId				BIGINT OUTPUT
)
AS
BEGIN
	BEGIN TRY


	SELECT @ChannelPartnerName=tc.Name FROM tChannelPartners tc WHERE tc.ChannelPartnerId=@channelPartnerId
	IF (@TerminalName IS NOT NULL and @ChannelPartnerId IS NOT NULL) 
	BEGIN
          SELECT
              @TerminalId= tt.TerminalID,
              @locationName=tl.LocationName,
              @bankID=tl.BankID,
              @branchID=tl.BranchID,
			  @terminalName = tt.name,
			  @peripheralServiceUrl = tnps.PeripheralServiceUrl
              FROM tTerminals tt 
              INNER JOIN tLocations tl on tl.LocationID=tt.LocationId
			  INNER JOIN tNpsTerminals as tnps on tl.LocationID = tnps.LocationId
              WHERE tt.Name = @terminalName and tt.ChannelPartnerId=@channelPartnerId
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