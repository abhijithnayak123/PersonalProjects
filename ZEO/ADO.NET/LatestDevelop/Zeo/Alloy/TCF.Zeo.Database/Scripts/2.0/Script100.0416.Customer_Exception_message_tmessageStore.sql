--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03-19-2017>
-- Description:	Customer Realted error message
-- ================================================================================

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1001.100.8606',1,'0','Inactive Customer Cannot Transact.','Please try again. If problem persists, contact the IT Service Desk at 763-337-6600.',GETDATE(),'Zeo',2)


IF EXISTS (
	SELECT 1 
	FROM tMessageStore 
	WHERE MessageKey IN ('1001.602.1','1001.602.2','1001.602'))

BEGIN 
	UPDATE tMessageStore
	SET Content = 'Unknown error, Please try again.'
	WHERE MessageKey IN ('1001.602.1','1001.602.2','1001.602')
END 
GO

