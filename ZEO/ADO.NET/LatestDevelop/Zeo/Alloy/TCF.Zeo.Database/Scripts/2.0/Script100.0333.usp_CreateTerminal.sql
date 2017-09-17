-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<Store Proc to create Terminals>
-- Jira ID:		<AL-7583>
-- ================================================================================
--EXEC usp_CreateTerminal 'OPT-LAP-0094',null,null,34,1000000003,10000009,'2017-01-17',0,'2017-01-17',1000000069,'2017-01-17'
IF EXISTS (SELECT 1 FROM sys.objects WHERE NAME = 'usp_CreateTerminal')
BEGIN DROP PROCEDURE usp_CreateTerminal END
 GO

CREATE PROCEDURE usp_CreateTerminal
	@name NVARCHAR(100),
	@ipAddress VARCHAR(20),
	@macAddress varchar(50), 
	@ChannelPartnerId SMALLINT, 
	@locationId BIGINT,
	@npsTerminalId BIGINT,
	@serverDate  DATETIME,
	@terminalId   BIGINT OUTPUT,
	@terminalDate DATETIME,
	@agentSessionId BIGINT,
	@dTServerCreate DATETIME,
	@agentId BIGINT,
	@agentFirstName VARCHAR(50),
	@agentLastName VARCHAR(50),
	@DtServerLastModified DATETIME
	
AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON; 
	
		INSERT INTO tTerminals
           ( [Name]
			,[MacAddress]
		    ,[ChannelPartnerId]
			,[LocationId]
			,[NpsTerminalId]
            ,[IpAddress]
            ,[DTServerCreate]
            ,[DTTerminalCreate])
		 VALUES
           (
				 @name
				,@macAddress
				,@ChannelPartnerId
				,@locationId
				,@npsTerminalId
				,@ipAddress
				,@serverDate
				,@terminalDate
			)

		 SET @terminalId = SCOPE_IDENTITY() 
		
	EXEC usp_UpdateAgentSession @agentSessionId,@terminalId,@name,@dTServerCreate

 UPDATE 
				dbo.tAgentDetails
			SET
				PrimaryLocationId = @locationId,
				DTServerLastModified = @DtServerLastModified
			WHERE 
				FirstName = @agentFirstName AND  LastName = @agentLastName AND channelPartnerId=@ChannelPartnerId
	END TRY
		 
	BEGIN CATCH
			Exec usp_CreateErrorInfo
	END CATCH
END 
GO


	
		
		

