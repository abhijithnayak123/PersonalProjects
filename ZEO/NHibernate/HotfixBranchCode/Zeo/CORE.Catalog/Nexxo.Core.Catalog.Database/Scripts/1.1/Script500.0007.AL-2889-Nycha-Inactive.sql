--===========================================================================================
-- Auther:			Karun
-- Date Created:	11/13/2015
-- Description:		AL-2889 - NCYHA BILLER - Displays Object reference error. 
--===========================================================================================

UPDATE tMasterCatalog SET IsActive = 0 WHERE BillerName = 'NYCHA' AND ProviderId = 404
GO

DELETE FROM tPartnerCatalog WHERE BillerName = 'NYCHA' AND ProviderId = 404
GO
