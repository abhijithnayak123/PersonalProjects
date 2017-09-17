-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<Store proc to get Nps terminal By ID>
-- Jira ID:		<AL-7583>
-- ================================================================================
--exec usp_GetNpsTerminalBylocationId '1000000003'

IF EXISTS (	SELECT  1 	FROM sys.objects WHERE NAME = 'usp_GetNpsTerminalBylocationId')
BEGIN DROP PROCEDURE usp_GetNpsTerminalBylocationId END
GO

CREATE PROCEDURE usp_GetNpsTerminalBylocationId
(
	@locationId VARCHAR(20)
)
AS
BEGIN

BEGIN TRY

	SELECT
		NpsTerminalID,
		Name,
		[Status],
		[Description],
		Port,
		PeripheralServiceUrl
	FROM
		 tNpsTerminals
	WHERE 
		LocationId = @locationId

END TRY
BEGIN CATCH
	Exec usp_CreateErrorInfo
END CATCH
END
GO
