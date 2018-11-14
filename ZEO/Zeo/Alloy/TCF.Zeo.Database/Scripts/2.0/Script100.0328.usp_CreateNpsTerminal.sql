-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<Store Proc to create Terminals>
-- Jira ID:		<AL-7583>
-- ================================================================================
--EXEC usp_CreateNpsTerminal 0,'OPT-LAP-0020','available',null,null,null,'set','2017-01-05','2017-01-05'
--Select * from terrorlog order by DTServerCreate desc

IF EXISTS (SELECT 1 FROM sys.objects WHERE NAME = 'usp_CreateNpsTerminal')
BEGIN DROP PROCEDURE usp_CreateNpsTerminal END
 GO

CREATE PROCEDURE usp_CreateNpsTerminal	
(
	@npsterminalId BIGINT OUTPUT,
	@name VARCHAR(50),
	@status VARCHAR(50),
	@description VARCHAR(500),
	@ipAddress VARCHAR(20),
	@port VARCHAR(10),
	@channelPartnerId BIGINT,
	@locationId BIGINT,
	@peripheralServiceUrl VARCHAR(100),
	@dTServerCreate  DATETIME,
	@dTTerminalCreate DATETIME
)
	
AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON; 
		
		INSERT INTO tNpsTerminals
           (
				 [Name]
				,[Status]
				,[Description]
				,[IpAddress]
				,[Port]
				,[ChannelPartnerId]
				,[LocationId]
				,[PeripheralServiceUrl]
				,[DTServerCreate]
				,[DTTerminalCreate]
			)
		 VALUES
           (
		      	 @name
				,@status
				,@description
				,@ipAddress
				,@port
				,@channelPartnerId
				,@locationId
				,@peripheralServiceUrl
				,@dTServerCreate
				,@dTTerminalCreate
		   )
		
	SET	@npsterminalId  = SCOPE_IDENTITY();
	END TRY
		 
	BEGIN CATCH
		Exec usp_CreateErrorInfo
	END CATCH
END 
GO


	
		

