-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date:  <01/21/2017>
-- Description:	<Store proc to update terminal>
-- Jira ID:		<AL-7583>
-- ================================================================================

IF EXISTS (	SELECT  1 FROM sys.objects WHERE NAME = 'usp_UpdateTerminal')
BEGIN DROP PROCEDURE usp_UpdateTerminal END
GO

CREATE PROCEDURE usp_UpdateTerminal
	@terminalId BIGINT,
	@name VARCHAR(200),
	@macAddress VARCHAR(50),
	@ipAddress VARCHAR(50),
	@locationId BIGINT,
	@agentSessionId BIGINT,
	@dTServerCreate DATETIME,
	@serverDate  DATETIME,
	@terminalDate DATETIME
	
AS
BEGIN
	BEGIN TRY
		
		SET NOCOUNT ON; 
		UPDATE tTerminals
		   SET	
			  Name = @name,
			  MacAddress = @macAddress,
			  IpAddress = @ipAddress,
			  LocationId = @locationId,
			  DTServerLastModified = @serverDate,
			  DTTerminalLastModified = @terminalDate
	   WHERE
			TerminalID = @terminalId
		
	EXEC usp_UpdateAgentSession @agentSessionId,@terminalId,@name,@dTServerCreate

		
	END TRY
		 
	BEGIN CATCH
		
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH
END 
GO
		
	
		
		

