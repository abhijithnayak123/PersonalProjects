--- ===============================================================================
-- Author:		<Rizwana Shaik>
-- Create date: <04-20-2017>
-- Description:	 Update the error type for warning message
-- Jira ID:		<>
-- ================================================================================



IF  EXISTS(SELECT * FROM tMessageStore WHERE MessageKey='1006.100.6003')
BEGIN
	update  tMessageStore set Content='Money Order check has already been issued' where MessageKey='1006.100.6003'
END