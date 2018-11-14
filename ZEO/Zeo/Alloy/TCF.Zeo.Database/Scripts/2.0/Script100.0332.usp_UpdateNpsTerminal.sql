-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<Store proc to update terminal>
-- Jira ID:		<AL-7583>
-- ================================================================================

IF EXISTS (	SELECT  1 FROM sys.objects WHERE NAME = 'usp_UpdateNpsTerminal')
BEGIN DROP PROCEDURE usp_UpdateNpsTerminal END
GO

CREATE PROCEDURE usp_UpdateNpsTerminal
	@npsTerminalId BIGINT,
	@description VARCHAR(500),
	@ipAddress VARCHAR(20),
	@name VARCHAR(50),
	@peripheralServiceUrl VARCHAR(100),
	@port VARCHAR(10),
	@status VARCHAR(50),
	@locationId BIGINT,
	@dTTerminatalLastModified  DATETIME,
	@dTServerLastModified DATETIME
	
AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON
		UPDATE tNpsTerminals
		   SET
			Name = @name,
			Description = @description,
			IPAddress = @ipAddress,
			PeripheralServiceUrl = @peripheralServiceUrl,
			Port = @port,
			Status = @status,
			DTServerCreate =   @dTTerminatalLastModified,
			DTTerminalCreate = @dTServerLastModified,
			LocationId = @locationId
	   WHERE
			NpsTerminalID = @npsTerminalId
		

		
	END TRY
		 
	BEGIN CATCH
			Exec usp_CreateErrorInfo
	END CATCH
END 
GO
		
	
		
		

