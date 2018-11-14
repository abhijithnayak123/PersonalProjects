--- ===============================================================================
-- Author:		 Pushkal
-- Description:	 Changing the error message to display the country as blocked.
-- Story Id   :  B-20462- Blocked countries message
-- ================================================================================


IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE '%1005.100.2090%')
BEGIN
	
	UPDATE tMessageStore
	SET 
		Content = 'We are unable to process this transaction. Money was sent from a country that is blocked by TCF.',
		AddlDetails = 'Please have the customer contact Western Union Customer Service at 1-800-325-6000 for assistance in finding an agent location where they can receive money from this country.',
		DTServerLastModified = GETDATE()
	WHERE
		MessageKey = '1005.100.2090'
END
GO