--- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <06/29/2016>
-- Description:	 Money Order transaction exception messages
-- Jira ID:		<AL-6774>
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey like '1006.100.%'
GO

INSERT INTO tMessageStore (MessageStorePK, MessageKey, PartnerPK, Language, Content, AddlDetails, DTServerCreate, Processor, Type)
VALUES    (NEWID(), '1006.100.1000', 1, 0, 'MoneyOrder Create Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.1001', 1, 0, 'MoneyOrder Update Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.1002', 1, 0, 'MoneyOrder Commit Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.1003', 1, 0, 'MoneyOrder Not Found', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6000', 1, 0, 'Maximum amount allowed for this transaction is {0}','Maximum amount allowed for this transaction is {0} ', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6001', 1, 0, 'Minimum amount required for this transaction is {0}','Amount entered is less than the minimum limit. Please update the amount and retry', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6002', 1, 0, 'Check Print Template Not Found', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6003', 1, 0, 'Money Order has already been issued', 'Please use a different Money Order. Also validate previously printed Money Orders have matching check numbers on the MICR line and as printed', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6004', 1, 0, 'Error occurred while resubmiting the transaction', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6005', 1, 0, 'Error occurred while featching fee', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6006', 1, 0, 'Error occurred while Adding transaction', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6007', 1, 0, 'Error occurred while updaing transaction', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6008', 1, 0, 'Error occurred while commiting transaction', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6009', 1, 0, 'Error occurred while fetching transaction', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6010', 1, 0, 'Receipt Templates Not Found', 'Please contact technical support team with this message for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.6011', 1, 0, 'Error occurred while doing moneyorder diagonostic', 'Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.8500', 1, 0, 'Money Order Scanning Error', 'This operation could not be completed. Please retry the scan process', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.8501', 1, 0, 'The Money Order Check Number was not scanned correctly', 'This operation could not be completed. Please retry the scan process or try scanning a different Money Order', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.8502', 1, 0, 'Service connectivity could not be established,','Please contact your technical support team for more information ', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.8503', 1, 0, 'Printing Money Order check could not be completed and printer returned error message', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.8504', 1, 0, 'Printer could not be detected', 'Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1006.100.8505', 1, 0, 'The printer is not accessible. There could be another scan or printing in progress or the device has been powered off, Try again later', 'Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)