--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-26-2017>
-- Description:	 Update the additional details for VISA Fraud message.
-- Jira ID:		<>
-- ================================================================================

IF  EXISTS(SELECT * FROM tMessageStore WHERE MessageKey='1003.100.8101')
BEGIN
	UPDATE  tMessageStore SET AddlDetails='The customer’s previous account was closed due to fraud.' WHERE MessageKey='1003.100.8101'
END
