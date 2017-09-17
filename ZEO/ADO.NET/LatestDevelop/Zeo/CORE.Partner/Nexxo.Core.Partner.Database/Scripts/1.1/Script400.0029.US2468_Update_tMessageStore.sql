--===========================================================================================
-- Auther:			<Rita Patel>
-- Date Created:	<02/04/2015>
-- Description:		<script to update for tMessageStore for Chexar decline messages>
-- Rally ID:		<US2468>		
--===========================================================================================
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check has been denied for non-sufficient funds. Please return the check to the customer.' WHERE MessageKey = '1002.01'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check is stale dated and cannot be cashed. Please return the check to the customer.' WHERE MessageKey = '1002.06'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check is postdated and cannot be cashed today. Please return the check to the customer.' WHERE MessageKey = '1002.10'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check is not signed on the front. Please return the check to the customer.' WHERE MessageKey = '1002.11'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check is not endorsed by the payee, or is endorsed incorrectly.  Please return the check to the customer.' WHERE MessageKey = '1002.12'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The numeric and written amounts do not agree. Please return the check to the customer.' WHERE MessageKey = '1002.13'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The date on the check is missing or incorrect. Return the check to the maker to complete/correct the date.' WHERE MessageKey = '1002.14'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.18'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.19'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check is a starter check and cannot be approved.  Please return the check to the customer.' WHERE MessageKey = '1002.20'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Maker and payee have same address.  Please return the check to the customer' WHERE MessageKey = '1002.23'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check has been altered.  Please return the check to the customer.' WHERE MessageKey = '1002.24'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Maker limit exceeded for today. Please return the check to the customer.' WHERE MessageKey = '1002.25'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Check cannot be reviewed after hours.  Please return the check to the customer' WHERE MessageKey = '1002.26'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Please return the check to the customer. Customer should contact the maker.' WHERE MessageKey = '1002.27'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check is a credit card check and cannot be approved.  Please return the check to the customer.' WHERE MessageKey = '1002.29'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check has been denied for stop payment. Please return the check to the customer.' WHERE MessageKey = '1002.30'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.31'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.32'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.33'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.34'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check is from outside the USA and cannot be approved. Please return the check to the customer.' WHERE MessageKey = '1002.35'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Maker and payee are same person.  Please return the check to the customer' WHERE MessageKey = '1002.36'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.37'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Please try again' WHERE MessageKey = '1002.38'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.40'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed. Refer the customer to the issuing bank for questions.' WHERE MessageKey = '1002.41'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check is not endorsed by all parties. Please return the check to the customer.' WHERE MessageKey = '1002.42'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed. Please return the check to the customer. Customer should contact the maker.' WHERE MessageKey = '1002.43'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.This is not a check.' WHERE MessageKey = '1002.44'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The maker of the check could not be contacted. Please return the check to the customer.' WHERE MessageKey = '1002.45'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check is not completely filled out. Please return the check to the customer.' WHERE MessageKey = '1002.46'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Unable to verify information to cash check at this time. Attempt to resubmit or return check to the customer.' WHERE MessageKey = '1002.47'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.48'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.The maker cannot be contacted. Please return the check to the customer.' WHERE MessageKey = '1002.49'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Return check to the customer; check-cashing limit is exceeded for the business day.' WHERE MessageKey = '1002.50'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Obtain proper endorsement from customer before cashing check. Verify and resubmit.' WHERE MessageKey = '1002.51'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.Obtain valid identification from customer before cashing check. Verify and resubmit.' WHERE MessageKey = '1002.52'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED AT THIS TIME.This check is an on-us check and should be cashed through teller systems.' WHERE MessageKey = '1002.53'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.54'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.57'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.Incorrect decline code, please contact InGo and resubmit check' WHERE MessageKey = '1002.58'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check cannot be cashed.  Please return the check to the customer.' WHERE MessageKey = '1002.59'
UPDATE tMessageStore SET Content='THE CHECK CANNOT BE APPROVED.The check exceeded check cashing limit' WHERE MessageKey = '1002.60'
GO

IF NOT EXISTS(SELECT 1 FROM  tMessageStore WHERE MessageKey = '1002.02')
BEGIN
	INSERT INTO tMessageStore(rowguid, MessageKey, PartnerPK, [Language], Content, DTCreate, Processor) VALUES
	(NEWID(), '1002.02', 1, 0, 'THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed.  Please return the check to the customer.', GETDATE(), 'Chexar'),
	(NEWID(), '1002.03', 1, 0, 'THE CHECK CANNOT BE APPROVED AT THIS TIME.Please return the check to the customer.', GETDATE(), 'Chexar'),
	(NEWID(), '1002.04', 1, 0, 'THE CHECK CANNOT BE APPROVED AT THIS TIME.The image of the check could not be read, rescan the check.', GETDATE(), 'Chexar'),
	(NEWID(), '1002.05', 1, 0, 'THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed.  Please return the check to the customer.', GETDATE(), 'Chexar'),
	(NEWID(), '1002.07', 1, 0, 'THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed.  Please return the check to the customer.', GETDATE(), 'Chexar'),
	(NEWID(), '1002.22', 1, 0, 'THE CHECK CANNOT BE APPROVED.Check can only be cashed by intended payee. Please return the check to the customer.', GETDATE(), 'Chexar'),
	(NEWID(), '1002.56', 1, 0, 'THE CHECK CANNOT BE APPROVED.The check cannot be cashed.  Please return the check to the customer.', GETDATE(), 'Chexar')
END

