--- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <06/22/2017>
-- Description: Adding exception messages for Pre/Post Flush flow.
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN 
	 ('1010.602.3700','1010.602.37002')

INSERT INTO [tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1010.602.3700', 1, '0', 'TimeOut:Error in Post Flush service call', 'RCIF Error. Please contact your technical support team', GETDATE(), 'Zeo', 2),
('1010.602.3702', 1, '0', 'TimeOut:Error in Pre Flush service call', 'RCIF Error. Please contact your technical support team', GETDATE(), 'Zeo', 2)
