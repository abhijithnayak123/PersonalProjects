--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <04-14-2017>
-- Description:	 Update the error type for warning message
-- Jira ID:		<89>
-- ================================================================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageStore')
BEGIN

	UPDATE tMessageStore 
	SET Type = 2
	WHERE
	  MessageKey 
		IN
		(
		 '1002.100.6000'
		,'1004.100.6000'
		,'1005.100.6000'
		,'1006.100.6000'
		,'1002.100.6001'
		,'1004.100.6001'
		,'1005.100.6001'
		,'1006.100.6001'
		)

END
GO


