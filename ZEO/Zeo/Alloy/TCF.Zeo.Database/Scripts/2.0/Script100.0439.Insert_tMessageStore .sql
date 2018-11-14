--- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <03/27/2017>
-- Description:	 Insert script for visa limit voilation message
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN ('1003.103.31133') 
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1003.103.31133',1,'0','','Visa Limit Violation Detected.Please contact the IT Service Desk at 763-337-6600.',GETDATE(),'VisaDPS',2)
