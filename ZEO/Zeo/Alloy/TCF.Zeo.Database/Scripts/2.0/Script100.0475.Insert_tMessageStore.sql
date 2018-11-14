--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <04-06-2017>
-- Description:	Adding an entry in Message Store for fetching the statename by state code failed message.
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN ('1001.100.4034') 
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1001.100.4034',1,'0','Error while fetching the state name from state code','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600' ,GETDATE(),'Zeo',2)


DELETE FROM tMessageStore WHERE MessageKey IN ('1001.100.3031') 
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1001.100.3031',1,'0','Error while fetching the state name from state code','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600' ,GETDATE(),'Zeo',2)
