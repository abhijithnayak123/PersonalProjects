--- ===============================================================================
-- Author:		 Ashok Kumar
-- Description:	 update tLimitTypes for Billpay and Funds
-- Jira ID:		<>
-- ================================================================================


UPDATE tLimitTypes SET Name = 'BillPayment' WHERE Name = 'BillPay'
UPDATE tLimitTypes SET Name = 'Fund' WHERE Name = 'Funds'