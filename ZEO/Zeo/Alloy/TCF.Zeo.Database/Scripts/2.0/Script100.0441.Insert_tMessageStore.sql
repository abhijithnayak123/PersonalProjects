--- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03/28/2017>
-- Description:	 Insert into tmessage store for card holder is not registered error.
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN ('1003.100.2111') 
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1003.100.2111',1,'0','Cardholder is not registered in Alloy','Register the customer and Associate Card to use VISA Prepaid Card.',GETDATE(),'Zeo',2)