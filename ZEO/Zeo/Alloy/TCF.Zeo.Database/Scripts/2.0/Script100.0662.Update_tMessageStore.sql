--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <03-13-2018>
-- Description:	 Updating the tMessageStore table for some of the message keys
-- Jira ID:		<B-09697>
-- ================================================================================

--WU
IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1005.301.U1503')
BEGIN
	UPDATE tMessageStore
	SET
	AddlDetails = N'Transaction could not be completed. Pickup location specified a city/state/country that is different than this location. Customer must contact the sender to verify the correct location.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1005.301.U1503'
END 


IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1005.301.E9387')
BEGIN
	UPDATE tMessageStore
	SET
	AddlDetails = N'Unable to provide status on this transaction. Please contact Western Union Customer Service for assistance at 1-800-325-6000.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1005.301.E9387'
END 


IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1005.301.T6380')
BEGIN
	UPDATE tMessageStore
	SET
	AddlDetails = N'Western Union has declined this transaction. Please contact Western Union Customer Service for assistance at 1-800-325-6000.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1005.301.T6380'
END
ELSE
BEGIN
	INSERT INTO tMessageStore
	(
		MessageKey,
		ChannelPartnerId,
		Language,
		Content,
		DTServerCreate,
		AddlDetails,
		Processor,
		Type,
		DisplayMessage
	)
	VALUES
	(
		N'1005.301.T6380',
		1,
		N'0',
		N'',
		GETDATE(), 
		N'Western Union has declined this transaction. Please contact Western Union Customer Service for assistance at 1-800-325-6000.',
		N'Western Union', 
		2,
		N''
	) 
END 

--RCIF
IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1010.602.2')
BEGIN
	UPDATE tMessageStore
	SET
	AddlDetails = N'Please ensure Additional Information questionnaire is completed and try again. If problem persists, contact BSA at 763-337-7881 to check for recipient OFAC screening results.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1010.602.2'
END 

IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1001.602.2')
BEGIN
	UPDATE tMessageStore
	SET
	AddlDetails = N'Customer did not pass new account screening. Please provide the customer the “Unable to Provide Bank Products or Services” form. Please contact Retail Banking Support(RBS) if further assistance is needed.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1001.602.2'
END 

--VISA
IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1003.103.31133')
BEGIN
	UPDATE tMessageStore
	SET
	AddlDetails = N'Customer exceeded one or more transaction limits. Customer may contact Visa at 855-477-1135 for more information.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1003.103.31133'
END 

IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1003.103.26')
BEGIN
	UPDATE tMessageStore
	SET
	AddlDetails = N'Confirm that the promotion code entered is correct(ALL CAPS with no spaces). Please contact Retail Banking Support(RBS) if further assistance is needed.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1003.103.26'
END
ELSE
BEGIN
	INSERT INTO tMessageStore
	(
	    MessageKey,
	    ChannelPartnerId,
	    Language,
	    Content,
	    DTServerCreate,
	    AddlDetails,
	    Processor,
	    Type,
	    DisplayMessage
	)
	VALUES
	(

	    N'1003.103.26',
	    1,
	    N'0',
	    N'',
	    GETDATE(), 
	    N'Confirm that the promotion code entered is correct (ALL CAPS with no spaces). Please contact Retail Banking Support(RBS) if further assistance is needed.',
	    N'Visa', 
	    2,
	    N''
	) 
END 

--ZEO
IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1005.100.2010')
BEGIN
	UPDATE tMessageStore
	SET
    AddlDetails = N'My WU Rewards cannot be applied. Customer would need to contact Western Union at 1-800-325-6000 to correct their name so it matches with ZEO.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1005.100.2010'
END 

--Not there in DB
--IF EXISTS(SELECT * FROM tMessageStore WHERE MessageKey like '%1010.3700%')
--BEGIN
--	UPDATE tMessageStore
--	SET
--	Content = N'Questions located in Additional Information are required to be answered in order to continue.'
--   ,AddlDetails = N'Please click Additional Information button in upper right corner of screen.'
--   ,DTServerLastModified = GETDATE()
--	WHERE MessageKey  = '1010.3700'
--END

IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey = '1004.100.2103')
BEGIN
	UPDATE tMessageStore
	SET
	Content = N'Unable to process biller locations from Western Union.'
   ,AddlDetails = N'If further assistance is needed, please contact IT Service Desk at 1-800-TCF-DESK(823-3375).'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1004.100.2103'
END  

IF EXISTS(SELECT * FROM tMessageStore WHERE MessageKey = '1005.100.2077')
BEGIN
	UPDATE tMessageStore
	SET
	Content = N'This destination country is not authorized by TCF.'
   ,AddlDetails = N'The customer may contact Western Union at 1-800-325-6000 for further assistance.'
   ,DTServerLastModified = GETDATE()
	WHERE MessageKey  = '1005.100.2077'
END  


