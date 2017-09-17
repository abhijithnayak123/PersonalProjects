--==========================================================================
-- Author: <RITA PATEL>
-- Date Created: <JUNE 30 2015>
-- Description: < As Certegy,  I need to handle decline codes >
-- User Story ID: <AL-400>
--===========================================================================


IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE Processor = 'Certegy')
BEGIN
	INSERT INTO tMessageStore(MessageStorePK, MessageKey, PartnerPK, Language, Content, DTServerCreate, Processor) VALUES
	(NEWID(), '1002.1', '35', '0', 'THE CHECK CANNOT BE APPROVED. The check cannot be cashed. Please return the check to the customer with copy of decline receipt', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.2', '35', '0', 'THE CHECK CANNOT BE APPROVED AT THIS TIME. The check cannot be cashed. Please return the check to the customer with copy of decline receipt', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.3', '35', '0', 'THE CHECK CANNOT BE APPROVED AT THIS TIME. Invalid ID format. Correct ID then resubmit check', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.4', '35', '0', 'THE CHECK CANNOT BE APPROVED. The check cannot be cashed. Please return the check to the customer with copy of decline receipt', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.5', '35', '0', 'THE CHECK CANNOT BE APPROVED AT THIS TIME. Location not set-up correctly,  please contact system administrator', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.6', '35', '0', 'THE CHECK CANNOT BE APPROVED AT THIS TIME. ID and DOB do not match public records,  please update profile then resubmit check', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.7', '35', '0', 'THE CHECK CANNOT BE APPROVED AT THIS TIME. Invalid SSN. Please update profile and resubmit check', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.8', '35', '0', 'THE CHECK CANNOT BE APPROVED AT THIS TIME. Duplicate or mismatched name. Please update name or SSN in profile and resubmit check', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.9', '35', '0', 'THE CHECK CANNOT BE APPROVED AT THIS TIME. System error. Please resubmit check or,  if still unsuccesful,  contact system administrator', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.10', '35', '0', 'THE CHECK CANNOT BE APPROVED AT THIS TIME. Consumer not set-up. Please update name,  address,  DOB SSN and ID information in profile and resubmit check', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.11', '35', '0', 'THE CHECK CANNOT BE APPROVED AT THIS TIME.Amount entered does not match check. Resubmit check with correct amount The check was successfully reversed. Return uncashed check to customer.', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.13', '35', '0', 'THE CHECK CANNOT BE REVERSED. Check is committed,  continue with processing of check', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.14', '35', '0', 'THE CHECK CANNOT BE APPROVED. The check cannot be cashed.  Please return the check to the customer.', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.15', '35', '0', 'THE CHECK CANNOT BE APPROVED. The check cannot be cashed. Please return the check to the customer.', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.16', '35', '0', 'THE CHECK CANNOT BE APPROVED. The check cannot be cashed. Please return the check to the customer with copy of decline receipt', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.22', '35', '0', 'THE CHECK CANNOT BE APPROVED. The check cannot be cashed. Please return the check to the customer with copy of decline receipt', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.23', '35', '0', 'THE CHECK CANNOT BE APPROVED. The check cannot be cashed. Please return the check to the customer with copy of decline receipt', GETDATE(), 'Certegy'), 
	(NEWID(), '1002.24', '35', '0', 'THE CHECK CANNOT BE APPROVED. The check cannot be cashed. Please return the check to the customer with copy of decline receipt', GETDATE(), 'Certegy')
END
GO