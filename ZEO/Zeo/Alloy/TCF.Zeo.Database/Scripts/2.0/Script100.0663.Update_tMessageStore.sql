--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <03-13-2018>
-- Description:	 Updating the tMessageStore table for some of the message keys
-- Jira ID:		<B-09697>
-- ================================================================================


IF EXISTS(SELECT * FROM tMessageStore WHERE MessageKey = '1005.100.2077')
BEGIN
	UPDATE tMessageStore
	SET
	Content = N'This destination country is not authorized by TCF.'
   ,AddlDetails = N'Please contact Retail Banking Support(RBS) if further assistance is needed.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1005.100.2077'
END  


