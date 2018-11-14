	-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <01/21/2017>
-- Description:	<As an engineer, I want to implement ADO.Net for Terminal module>
-- Jira ID:		<AL-7583>
-- ================================================================================
IF EXISTS (	SELECT  1 	FROM sys.objects WHERE NAME = 'usp_GetTerminalByName')
BEGIN DROP PROCEDURE usp_GetTerminalByName END
GO

CREATE PROCEDURE usp_GetTerminalByName
	@terminalName VARCHAR(100),
	@channelPartnerID BIGINT
AS   
BEGIN
	BEGIN TRY
			Select 
				tt.TerminalID,
				tt.Name,
				tt.MacAddress,
				tt.IpAddress,
				tt.LocationId,
				tt.NpsTerminalId,
				tt.ChannelPartnerId,
				tnps.PeripheralServiceUrl
				
			FROM 
							tTerminals AS tt
			INNER JOIN		tNpsTerminals AS tnps ON tt.NpsTerminalId = tnps.NpsTerminalID
			WHERE
				tt.ChannelPartnerId = @channelPartnerID
			AND 
				tt.Name = @terminalName
	END TRY

	BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
	END CATCH
END
GO  