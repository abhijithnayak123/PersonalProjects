--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <06-14-2018>
-- Description:	Updating Peripheral Service URL in tNPSterminal Table. 
-- ================================================================================

UPDATE tNpsTerminals
SET PeripheralServiceUrl = 'https://ps-zeo.tcfbank.com:18731/Peripheral/'
WHERE PeripheralServiceUrl = 'https://nps.nexxofinancial.com:18732/Peripheral/'
GO