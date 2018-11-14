-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<Store Proc to get terminal By ID>
-- Jira ID:		<AL-7583>
-- ================================================================================
IF EXISTS (	SELECT  1 FROM sys.objects WHERE NAME = 'usp_getNpsdiagnosticInfo')
BEGIN DROP PROCEDURE usp_getNpsdiagnosticInfo END
GO
CREATE PROCEDURE usp_getNpsdiagnosticInfo 
(
	@terminalId BIGINT
)
AS
BEGIN
	BEGIN TRY
	  
		SELECT 
			tt.Name,
			tl.LocationName,
			tnps.Name AS PeripheralServeName
	
		FROM
			 tNpsTerminals AS tnps 
		INNER JOIN  
			tLocations AS tl  ON tnps.LocationId = tl.LocationID
		INNER JOIN 
			tTerminals AS tt ON tt.NpsTerminalId = tnps.NpsTerminalID
		WHERE 
			tt.TerminalID = @terminalId
	END TRY
	BEGIn CATCH
		EXEC usp_CreateErrorInfo
	END CATCH
END
GO
