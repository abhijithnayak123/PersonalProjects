--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-24-2017>
-- Description:	 Insert the message for Preflush and Postflush provider error.
-- Jira ID:		<>
-- ================================================================================


INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1010.602',1,'0','','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.',GETDATE(),'RCIF',2)
,('1010.602.1',1,'0','','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.',GETDATE(),'RCIF',2)
,('1010.602.2',1,'0','','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.',GETDATE(),'RCIF',2)
GO