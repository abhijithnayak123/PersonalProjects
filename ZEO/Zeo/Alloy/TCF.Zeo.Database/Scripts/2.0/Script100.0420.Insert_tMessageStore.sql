--- ================================================================================
-- Author:		<Purna Pushkal>
-- Create date: <03/20/2017>
-- Description: Adding exception messages which are not added
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN 
	 ('1006.100.8500','1006.100.8501','1006.100.8502','1006.100.8503','1006.100.8504','1006.100.8505','1003.100.8100','1000.100.8091')

INSERT INTO [dbo].[tMessageStore]([MessageKey], [ChannelPartnerId], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES  
		  ('1006.100.8500', 1, '0', 'Money Order Scanning Error', 'This operation could not be completed. Please retry the scan process', GETDATE(), 'Zeo', 2)
		, ('1006.100.8501', 1, '0', 'The Money Order Check Number was not scanned correctly', 'This operation could not be completed. Please retry the scan process or try scanning a different Money Order', GETDATE(), 'Zeo', 2)
		, ('1006.100.8502', 1, '0', 'Service connectivity could not be established,','Please contact your technical support team for more information ', GETDATE(), 'Zeo', 2)
		, ('1006.100.8503', 1, '0', 'Printing Money Order check could not be completed and printer returned error message', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'Zeo', 2)
		, ('1006.100.8504', 1, '0', 'Printer could not be detected', 'Please contact your technical support team for more information', GETDATE(), 'Zeo', 2)
		, ('1006.100.8505', 1, '0', 'The printer is not accessible. There could be another scan or printing in progress or the device has been powered off, Try again later', 'Please contact your technical support team for more information', GETDATE(), 'Zeo', 2)
		, ('1003.100.8100', 1, '0', 'Please enter a valid card number', 'Use a valid or different card and retry', GETDATE(), 'Zeo', 2)
		, ('1000.100.8091', 1, '0', 'Terminal Not set up Properly', 'Terminal Not set up properly. Please set up terminal before you proceed further', GETDATE(), 'Zeo', 2)
