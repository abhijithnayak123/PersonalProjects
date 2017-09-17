--- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <07/07/2016>
-- Description:	 Check Processing exception messages
-- Jira ID:		<AL-6775>
-- ================================================================================

--Error Messaage for Biz and CXE

DELETE FROM tMessageStore WHERE MessageKey like '1002.%' or MessageKey in('1000.100.3404','1000.100.3403','1000.100.3402','1000.100.3401','1000.100.3400','1000.100.4400','1000.100.4401','1000.100.4402','1000.100.4403')
GO

INSERT INTO tMessageStore (MessageStorePK, MessageKey, PartnerPK, Language, Content, AddlDetails, DTServerCreate, Processor, Type)
VALUES   (NEWID(), '1002.100.1000', 1, 0, 'Check Get Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.1001', 1, 0, 'Check Create Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.1002', 1, 0, 'Check Update Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.1003', 1, 0, 'Check Commit Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		
		, (NEWID(), '1002.100.6000', 1, 0, 'Maximum amount allowed for this transaction is {0}','Maximum amount allowed for this transaction is {0} ', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6001', 1, 0, 'Minimum amount required for this transaction is {0}','Amount entered is less than the minimum limit. Please update the amount and retry', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6002', 1, 0, 'Check Print Template Not Found', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6003', 1, 0, 'Error occurred while submiting the transaction', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6004', 1, 0, 'Error occurred while canceling the transaction', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6005', 1, 0, 'Error occurred while updating', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6006', 1, 0, 'Error occurred while resubmiting transaction.', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6007', 1, 0, 'Error occurred while Commiting transaction', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6008', 1, 0, 'Error occurred while getting check types', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6009', 1, 0, 'Error occurred while fetching fee', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6010', 1, 0, 'Check Franking Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6011', 1, 0, 'Error occurred while featching check transaction', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6012', 1, 0, 'Error occurred while getting the check','Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6013', 1, 0, 'Error occurred while getting the status','Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.6015', 1, 0, 'Error occurred while doing Check Processing','Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		
		, (NEWID(), '1002.100.8250', 1, 0, 'Check Scanning Error', 'Please reinitiate the process by clicking on the Process Check button', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.8251', 1, 0, 'The printer is not accessible', 'There could be another scan or printing in progress or the device has been powered off, Try again later', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.8252', 1, 0, 'Check endorse data is not available', 'There could be another scan or printing in progress or the device has been powered off, Try again later', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1002.100.8253', 1, 0, 'The printer is not accessible', 'Please contact your technical support team for more information.', GETDATE(), 'MGiAlloy', 2)
		
		, (NEWID(), '1000.100.3400', 1, 0, 'Message Create Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.3401', 1, 0, 'Message Update Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.3402', 1, 0, 'Message Delete Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.3403', 1, 0, 'Message Look up Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.3404', 1, 0, 'Message Add Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		
		, (NEWID(), '1000.100.4400', 1, 0, 'Update Message  Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.4401', 1, 0, 'Get Message Failed', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'MGiAlloy', 2)
		


-- Error message for Ingo Decline Code 
		,(NEWID(),	'1002.200.36'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Maker and payee are same person Please return the check to the customer',NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.49'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The maker cannot be contacted Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.32'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed  Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.14'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The date on the check is missing or incorrect Return the check to the maker to complete/correct the date',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.3'	,35	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Invalid ID format Correct ID then resubmit check',NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.40'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed  Please return the check to the customer', NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.18'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed  Please return the check to the customer', NULL,GETDATE() , 'Chexar', 2)
		, (NEWID(),	'1002.200.47'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME.Unable to verify information to cash check at this time. Attempt to resubmit or return check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.45'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The maker of the check could not be contacted Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.6'	,35	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME ID and DOB do not match public records,  please update profile then resubmit check',NULL, GETDATE() , 'Certegy', 2)
		, (NEWID(),	'1002.200.05'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed  Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.53'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME This check is an on-us check and should be cashed through teller systems',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.7'	,35	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Invalid SSN Please update profile and resubmit check',NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.20'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check is a starter check and cannot be APPROVED  Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.46'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIMEThe check is not completely filled out Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.16'	,35	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer with copy of decline receipt',NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.6'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check is stale dated and cannot be cashed Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.57'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed  Please return the check to the customer', NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.38'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Please try again', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.25'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIMEMaker limit exceeded for today Please return the check to the customer', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.14'	,35	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed  Please return the check to the customer', NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.7'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed Please return the check to the customer', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.35'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check is from outside the USA and cannot be APPROVED Please return the check to the customer',  NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.12'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check is not endorsed by the payee, or is endorsed incorrectly Please return the check to the customer' , NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.44'	,1	,0	,'THE CHECK CANNOT BE APPROVED This is not a check' , NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.07'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed  Please return the check to the customer', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.11'	,35	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Amount entered does not match check Resubmit check with correct amount The check was successfully reversed Return uncashed check to customer', NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.27'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Please return the check to the customer Customer should contact the maker' , NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.51'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Obtain proper endorsement from customer before cashing check  Verify and resubmit', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.31'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.29'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check is a credit card check and cannot be APPROVED Please return the check to the customer', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.19'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed  Please return the check to the customer' , NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.23'	,35	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer with copy of decline receipt', NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.23'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Maker and payee have same address  Please return the check to the customer' , NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.56'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed  Please return the check to the customer' , NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.03'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Please return the check to the customer' , NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.04'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The image of the check could not be read, rescan the check', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.11'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check is not signed on the front Please return the check to the customer' , NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.30'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check has been denied for stop payment Please return the check to the customer', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.59'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed  Please return the check to the customer', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.55'	,1	,0	,'YOUR CHECK CANNOT BE APPROVED AT THIS TIME Please contact an Ingo Money customer service representative if you have further questions' , NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.-3'	,1	,0	,'YOUR CHECK CANNOT BE APPROVED AT THIS TIMEThe check is post dated Please return the check to the person who issued it to correct the date and resubmit' , NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.58'	,1	,0	,'THE CHECK CANNOT BE APPROVED Incorrect decline code, please contact InGo and resubmit check', NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.9'	,35	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME System error Please resubmit check or,  if still unsuccesful,  contact system administrator' , NULL ,GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.8'	,35	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Duplicate or mismatched name Please update name or SSN in profile and resubmit check', NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.5'	,35	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Location not set-up correctly,  please contact system administrator', NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.201'	,1	,0	,'Unknown error occurred while doing Process Check','Please contact the System Administrator', GETDATE(),'Chexar', 2)
		, (NEWID(),	'1002.200.43'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIMEThe check cannot be cashed Please return the check to the customerCustomer should contact the maker',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.5'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed Please return the check to the customer' ,NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.200'	,1	,0	,'Unknown error occurred while doing Process Check' ,NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.13'	,35	,0	,'THE CHECK CANNOT BE REVERSED Check is committed,  continue with processing of check' ,NULL ,GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.54'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.24'	,35	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer with copy of decline receipt' ,NULL ,GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.48'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed  Please return the check to the customer',NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.02'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed  Please return the check to the customer' ,NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.24'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check has been altered  Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.2'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed  Please return the check to the customer',NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.42'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check is not endorsed by all parties Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.34'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.33'	,1	,0	,'THE CHECK CANNOT BE APPROVED  The check cannot be cashed Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.3'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.60'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check exceeded check cashing limit',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.-2'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check is stale dated and cannot be cashed Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.26'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Check cannot be reviewed after hours  Please return the check to the customer' ,NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.22'	,35	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer with copy of decline receipt',NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.50'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Return check to the customer; check-cashing limit is exceeded for the business day',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.22'	,1	,0	,'THE CHECK CANNOT BE APPROVED Check can only be cashed by intended payee Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.52'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Obtain valid identification from customer before cashing check Verify and resubmit' ,NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.4'	,35	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer with copy of decline receipt',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.37'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed  Please return the check to the customer' ,NULL ,GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.41'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check cannot be cashed Refer the customer to the issuing bank for questions',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.13'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The numeric and written amounts do not agree Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.15'	,35	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer' ,NULL ,GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.4'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The image of the check could not be read, rescan the check',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.1'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check has been denied for non-sufficient funds Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.1'	,35	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer with copy of decline receipt',NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.-1'	,1	,0	,'THE CHECK CANNOT BE APPROVED The check cannot be cashed Please return the check to the customer',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.10'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME The check is postdated and cannot be cashed today Please return the check to the customer',NULL , GETDATE(), 'Certegy', 2)
		, (NEWID(),	'1002.200.10'	,35	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME Consumer not set-up Please update name,  address,  DOB SSN and ID information in profile and resubmit check',NULL , GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.100'	,1	,0	,'Unknown error occurred while doing Process Check' ,'Please contact the System Administrator',GETDATE(), 'Chexar', 2)
		, (NEWID(),	'1002.200.0'	,1	,0	,'THE CHECK CANNOT BE APPROVED AT THIS TIME. Please contact INGO for resolution' ,'Please contact the System Administrator',GETDATE(), 'Chexar', 2)

Go

--- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07/07/2016>
-- Description:	Add Error/Exception handling Framework for Check processing CXN layer.
-- Jira ID:		<AL-6775>
-- ================================================================================
-- select * from tMessageStore WHERE MessageKey like '1000.100.200%' or MessageKey like '1000.100.201%' or MessageKey like '1000.100.202%' 

-- Error message for Check processing CXN Layer.

DELETE FROM tMessageStore WHERE MessageKey like '1002.100.200%' or MessageKey like '1002.100.201%' or MessageKey like '1002.100.202%' 
GO

INSERT INTO [dbo].[tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
(NEWID(), '1002.100.2000', 1, 0, 'Location not set', 'This transaction could not be completed. Please set the location and submit the transaction', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2001', 1, 0, 'Invalid account number', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2002', 1, 0, 'Transaction not found', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2003', 1, 0, 'Chexar credentials not found', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2004', 1, 0, 'Chexar login failed', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2005', 1, 0, 'Chexar Check type not found',	'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2006', 1, 0, 'Error occurred while commit the transaction', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2007', 1, 0, 'Error occurred while submit the transaction', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2008', 1, 0, 'Error occurred while retrieving the transaction', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2009', 1, 0, 'Error occurred while retrieving the chexar status', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2010', 1, 0, 'Error occurred while canceling the transaction', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2011', 1, 0, 'Error occurred while retrieving the check processor information', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2012', 1, 0, 'Error occurred while updating the transaction', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2013', 1, 0, 'Error occurred while updating the pending check', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2014', 1, 0, 'Error occurred while registering the new customer account', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2015', 1, 0, 'Error occurred while retrieving the account', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2016', 1, 0, 'Error occurred while adding the chexar partner', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2017', 1, 0, 'Error occurred while retrieving the chexar session', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2018', 1, 0, 'Error occurred while retrieving Chexar wait time from INGO', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2019', 1, 0, 'Error occurred while closing the transaction in INGO', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2020', 1, 0, 'Error occurred while creating the transaction', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2021', 1, 0, 'Error occurred while retrieving the MICR details from INGO', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2022', 1, 0, 'Error occurred while employee authorizing in INGO', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2),
(NEWID(), '1002.100.2023', 1, 0, 'Error occurred while updating the Check account', 'This transaction could not be completed. Please contact technical support team for more information', GETDATE(), 'MGiAlloy', 2)
GO


--Default Additional details for INGO Provider.

DELETE FROM tMessageStore WHERE MessageKey = '1002.200'

INSERT INTO [dbo].[tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES
(NEWID(), '1002.200', 1, 0, '', 'Transaction could not be completed. Please contact your technical support team for more information', GETDATE(), 'Chexar', 2)

---Web messages

		
DELETE FROM tMessageStore WHERE MessageKey in ('1000.100.8021','1000.100.8022','1000.100.8023','1000.100.8024','1000.100.8025','1000.100.8026','1000.100.8027')
GO

INSERT INTO tMessageStore (MessageStorePK, MessageKey, PartnerPK, Language, Content, AddlDetails, DTServerCreate, Processor, Type)
VALUES (NEWID(), '1000.100.8021', 1, 0, 'Service connectivity could not be established', 'Please contact technical support team for further assistance.', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.8022', 1, 0, 'Error while retrieving check declined receipt template.', 'Please contact technical support team for further assistance.', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.8023', 1, 0, 'Error while retrieving dodfrank receipt template.', 'Please contact technical support team for further assistance.', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.8024', 1, 0, 'PS Service is not running.', 'Please contact technical support team for further assistance.', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.8025', 1, 0, 'Receipt Templates Not Found.', 'Please contact technical support team for further assistance.', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.8026', 1, 0, 'printer is not connected', 'Please contact technical support team for further assistance.', GETDATE(), 'MGiAlloy', 2)
		, (NEWID(), '1000.100.8027', 1, 0, 'The printer is not accessible', 'Please contact technical support team for further assistance.', GETDATE(), 'MGiAlloy', 2)

DELETE FROM tMessageStore WHERE MessageKey like '1002.200.500%'

INSERT INTO [dbo].[tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES

(NEWID(), '1002.200.5003', 1, 0, 'This operation has timed out', 'Transaction could not be completed due to a timeout error. Please retry. For more information contact your technical support team ', GETDATE(), 'MGiAlloy', 2)
, (NEWID(), '1002.200.5002', 1, 0, 'This operation could not be completed ', 'Transaction could not be completed due to a connectivity issue. Please contact your technical support team ', GETDATE(), 'MGiAlloy', 2)
, (NEWID(), '1002.200.5001', 1, 0, 'This operation could not be completed ', 'Transaction could not be completed due to a connectivity issue. Please contact your technical support team ', GETDATE(), 'MGiAlloy', 2)
, (NEWID(), '1002.200.5000', 1, 0, 'This operation could not be completed ', 'Transaction could not be completed due to a connectivity issue. Please contact your technical support team ', GETDATE(), 'MGiAlloy', 2)
