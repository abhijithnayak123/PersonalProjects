-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<Store proc to get Nps terminal By ID>
-- Jira ID:		<AL-7583>
-- ================================================================================
--exec usp_getNpsTerminal N'B5BCFD8A-FAFC-4406-A53C-EF26C7B1E03B'

IF EXISTS (	SELECT  1 	FROM sys.objects WHERE NAME = 'usp_GetNpsTerminalById')
BEGIN DROP PROCEDURE usp_GetNpsTerminalById END
GO

CREATE PROCEDURE usp_GetNpsTerminalById
(
	@npsTerminalId BIGINT
)
AS
BEGIN

BEGIN TRY

	SELECT
		 [NpsTerminalID]
		,[Name]
		,[Status]
		,[Description]
		,[IpAddress]
		,[Port]
		,[PeripheralServiceUrl]
		,[LocationId]
	FROM
		 tNpsTerminals
	WHERE 
		NpsTerminalID = @npsTerminalId
END TRY
BEGIN CATCH
	Exec usp_CreateErrorInfo
END CATCH
END
GO
