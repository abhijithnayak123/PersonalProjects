--- ===============================================================================
-- Author:		 Abhijith
-- Description:	 Updating the Error if the SSN is not provided.
-- Story : SSN / ITIN is required for US Citizens during registration - B-23417
-- ================================================================================


IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1001.100.8608%')
BEGIN
	
	UPDATE dbo.tMessageStore
	SET AddlDetails = N'Please complete the SSN on the "Personal" page to continue.'
	WHERE MessageKey = '1001.100.8608'

END
GO