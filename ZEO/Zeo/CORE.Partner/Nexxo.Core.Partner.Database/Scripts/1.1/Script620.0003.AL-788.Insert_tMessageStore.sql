-- ================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <26/02/2016>
-- Description:	<Display helpful message when counterIds missing>
-- Jira ID:		<AL-788>
-- ================================================================================

IF NOT EXISTS 
(
	SELECT 1 FROM tMessageStore WHERE MessageKey = '1005.2003'
)
BEGIN 
	INSERT INTO tMessageStore ([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [DTServerCreate],[AddlDetails], [Processor])
	VALUES (NEWID(), '1005.2003', 1, 0, 'Western Union counter Id is not available or has not been correctly setup', GETDATE(),'Please contact the System Administrator', 'Western Union')
END
GO
