--- ================================================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <03/28/2016>
-- Description:	 Incorrect message appears when WU API service fails with error
-- Jira ID:		<AL-6125>
-- ================================================================================
 
DELETE 
	FROM tMessageStore 
WHERE 
	MessageKey = '1005.2003' 
AND 
	Content = 'Western Union counter Id is not available or has not been correctly setup'