--- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <03/27/2017>
-- Description:	 Insert script location Identifier exist message
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN ('1000.100.4210') 
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1000.100.4210',1,'0','Location with Location Identifier already exist','Please try again.  If problem persists, contact the IT Service Desk at 763-337-6600' ,GETDATE(),'Zeo',2)
