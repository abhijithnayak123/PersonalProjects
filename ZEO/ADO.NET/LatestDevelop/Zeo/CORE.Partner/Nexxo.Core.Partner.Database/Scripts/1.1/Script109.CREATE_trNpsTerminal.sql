SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Bineesh Raghavan>
-- Create date: <11/18/2013>
-- Description:	<Temporary fix for tNpsTerminals table to update port and service url>
-- =============================================
CREATE TRIGGER trNpsTerminal
   ON  tNpsTerminals 
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @npsTerminalId UNIQUEIDENTIFIER
	
	SELECT @npsTerminalId = rowguid FROM inserted

    UPDATE 
		tNpsTerminals
	SET 
		PeripheralServiceUrl = 'https://nps.nexxofinancial.com:18732/Peripheral/',
		Port = 18732
	WHERE 
		rowguid = @npsTerminalId

END
GO
