--===========================================================================================
-- Auther:			Rita Patel
-- Date Created:	02/06/2015
-- Description:		US2485_Update_tPartnerCatalog
--===========================================================================================

UPDATE 
	tPartnerCatalog 
SET 
	BillerName = 'DO NOT USE' 
WHERE 
	BillerName = 'NYCHA'
	AND ChannelPartnerId = 28
GO
