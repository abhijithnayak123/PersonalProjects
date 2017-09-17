--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <05-03-2017>
-- Description:	 Update the VISA error message for removal.
-- Jira ID:		<>
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey='1003.100.2126'
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES ('1003.100.2126',1,0,'Error occurred while removing the prepaid transaction','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2)
