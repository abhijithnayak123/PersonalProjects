-- ================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <15/12/2015>
-- Description:	<The below query inserts exception messages for unsuccessful Visa DPS card registeration for exsisting Customer>
-- Jira ID:		<AL-1955>
-- ================================================================================

IF NOT EXISTS 
(
	SELECT 1 FROM tMessageStore WHERE MessageKey = '1003.2111'
)
BEGIN 
	INSERT INTO tMessageStore ([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [DTServerCreate], [Processor])
	VALUES (NEWID(), '1003.2111', 1, 0, 'Cardholder is not registered in Alloy. Register the customer and Associate Card to use VISA Prepaid Card', GETDATE(), 'Visa')
END
GO


IF NOT EXISTS 
(
	SELECT 1 FROM tMessageStore WHERE MessageKey = '1003.2112'
)
BEGIN
	INSERT INTO tMessageStore ([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [DTServerCreate], [Processor])
	VALUES (NEWID(), '1003.2112', 1, 0, 'SS# Mismatch,  please verify SS#', GETDATE(), 'Visa')
END
GO


