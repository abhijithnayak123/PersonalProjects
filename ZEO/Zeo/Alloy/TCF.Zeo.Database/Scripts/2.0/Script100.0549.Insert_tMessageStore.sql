--- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <06/22/2017>
-- Description: Adding exception messages for customer registration flow.
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN 
	 ('1001.602.2000','1001.100.3032','1001.100.6017')

INSERT INTO [tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1001.602.2000', 1, '0', 'TimeOut:Error in RCIF service call', 'RCIF Error. Please contact your technical support team', GETDATE(), 'Zeo', 2),
('1001.100.3032', 1, '0', 'Error Occured while updating the customer profile', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(), 'Zeo', 2),
('1001.100.6017', 1, '0', 'Error Occured while updating the customer profile', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(), 'Zeo', 2)