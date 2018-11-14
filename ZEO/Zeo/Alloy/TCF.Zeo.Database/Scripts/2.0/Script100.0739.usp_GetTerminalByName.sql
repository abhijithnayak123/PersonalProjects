	-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Modified By : Abhijith 
-- Create date: <01/21/2017>
-- Modified Date: 04/16/2018
-- Description:	<As an engineer, I want to implement ADO.Net for Terminal module>
-- Modified Description: If the Location is Inactive for the terminal give the error to the teller so that they
-- cannot do the transaction from that location.
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
				tnps.PeripheralServiceUrl,
				tl.IsActive AS IsLocationActive 
			FROM 
				tTerminals AS tt
				INNER JOIN	tNpsTerminals AS tnps ON tt.NpsTerminalId = tnps.NpsTerminalID
				INNER JOIN	tLocations AS tl ON tt.LocationId = tl.LocationId
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