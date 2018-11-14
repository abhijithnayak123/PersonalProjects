--- ===============================================================================
-- Author:		<M.Pushkal>
-- Create date: <12-01-2017>
-- Description:	Changing the check type names for the OnUs provider
-- Jira ID:		<B-10860>
-- ================================================================================

IF EXISTS(SELECT 1 FROM tCheckTypes WHERE Name = 'OnUsOCMO')
BEGIN
	UPDATE tCheckTypes 
	SET Name = 'Official Check or Money Order',
	    ProductProviderCode = 202
	WHERE Name = 'OnUsOCMO' 
END 

IF EXISTS(SELECT 1 FROM tCheckTypes WHERE Name = 'OnUsTRUE')
BEGIN
	UPDATE tCheckTypes 
	SET Name = 'TCF Check',
	    ProductProviderCode = 202
	WHERE Name = 'OnUsTRUE' 
END 

IF EXISTS(SELECT 1 FROM tCheckTypes WHERE Name = 'OnUsOTHER')
BEGIN
	UPDATE tCheckTypes 
	SET Name = 'OnUs Other',
	    ProductProviderCode = 202
	WHERE Name = 'OnUsOTHER' 
END 