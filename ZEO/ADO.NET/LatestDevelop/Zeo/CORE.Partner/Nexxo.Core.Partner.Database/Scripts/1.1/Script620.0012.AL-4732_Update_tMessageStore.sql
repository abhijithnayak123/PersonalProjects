--- ================================================================================
-- Author:		<Divya Boddu>
-- Create date: <05/09/2016>
-- Description:	 Check cashing transaction did not appear on the Ingo settlement file.
--				 Had un-printed status at Ingo
-- Jira ID:		<AL-4732>
-- ================================================================================
UPDATE 
	tMessageStore
SET 
	Content = 'User is not assigned to the branch', 
	AddlDetails = 'The check was originally parked at a different branch. Please park the check and en-cash it at the original branch'
WHERE  
	MessageKey = '1002.2013'

GO



