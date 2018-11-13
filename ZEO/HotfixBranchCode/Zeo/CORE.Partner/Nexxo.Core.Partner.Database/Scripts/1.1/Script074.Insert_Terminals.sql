-- Insert yubikey master key record to tTerminals table
DECLARE @npsTerminalId UNIQUEIDENTIFIER
SELECT TOP 1 @npsTerminalId = rowguid FROM tNpsTerminals
INSERT tTerminals 
(
	rowguid, 
	Name, 
	IpAddress, 
	LocationPK, 
	NpsTerminalPK, 
	DTCreate, 
	DTLastMod
)
VALUES 
(
	'F7F17776-E886-4AE2-93BD-A339102D6720', 
	'CCCCCCBLDFHF', 
	'172.18.100.63', 
	'0AD068FB-88A6-48D4-9081-FC5A73F9A187', 
	@npsTerminalId, 
	getdate(), 
	getdate()
 )
GO
 
-- Update value for new column 
--UPDATE 
--	[dbo].[tNpsTerminals]
--SET 
--	[PeripheralServiceUrl] = 'http://172.18.100.29:8732/Peripheral/'
--GO

UPDATE 
	tChannelPartners 
SET 
	ComplianceProgramName = 'CentrisTest' 
WHERE 
	id = 27
