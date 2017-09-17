-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<Store Proc to get terminal By ID>
-- Jira ID:		<AL-7583>
-- ================================================================================
IF EXISTS (	SELECT  1 FROM sys.objects WHERE NAME = 'usp_GetTerminalByTerminalId')
BEGIN DROP PROCEDURE usp_GetTerminalByTerminalId END
GO

CREATE PROCEDURE usp_GetTerminalByTerminalId
	@terminalId BIGINT
AS   
BEGIN TRY

    SET NOCOUNT ON; 
     
    SELECT
		TerminalID,
		 Name,
		 MacAddress, 
		 IpAddress,
		 LocationId,
		 NpsTerminalId
    FROM 
		tTerminals
	WHERE
		TerminalID = @terminalId

END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH


GO  