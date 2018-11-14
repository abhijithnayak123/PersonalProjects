--- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <03/23/2017>
-- Description:	 Insert into tmessage store
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN ('1003.103.5000','1003.103.5001','1003.103.5002','1003.103.5003','1003.103') 
GO

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
('1003.103.5000',1,'0','This operation could not be completed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.',GETDATE(),'VisaDPS',2),
('1003.103.5001',1,'0','This operation could not be completed','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.',GETDATE(),'VisaDPS',2),
('1003.103.5002',1,'0','Visa DPS is down at this time','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.',GETDATE(),'VisaDPS',2),
('1003.103.5003',1,'0','This operation has timed out','Please try again.  If problem persists, contact IT Service Desk at 763-337-6600.',GETDATE(),'VisaDPS',2),
('1003.103',1,'0','','Please contact the IT Service Desk at 763-337-6600.',GETDATE(),'VisaDPS',2)
