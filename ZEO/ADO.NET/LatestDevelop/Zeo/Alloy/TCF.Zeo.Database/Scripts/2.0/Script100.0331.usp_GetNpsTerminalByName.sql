-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<Store proc to get Nps terminal By ID>
-- Jira ID:		<AL-7583>
-- ================================================================================
--exec usp_GetNpsTerminalByName 'OPT-LAP-0094',34

IF EXISTS (	SELECT  1 	FROM sys.objects WHERE NAME = 'usp_GetNpsTerminalByName')
BEGIN DROP PROCEDURE usp_GetNpsTerminalByName END
GO

CREATE PROCEDURE usp_GetNpsTerminalByName
(
	@name VARCHAR(50),
	@channelPartnerId BIGINT
)
AS
BEGIN
BEGIN TRY
	SELECT
		NpsTerminalID,
		Name,
		[Status],
		[Description],
		IpAddress,
		Port,
		PeripheralServiceUrl
	FROM
		 tNpsTerminals
	WHERE 
		Name = @name
	AND 
	ChannelPartnerId = @channelPartnerId

END TRY
BEGIN CATCH
	Exec usp_CreateErrorInfo
END CATCH

END
GO
