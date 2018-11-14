--===========================================================================================
-- Author:		<RAJKUMAR M>
-- Create date: <Mar 26 2015>
-- Description:	<Script to update the content "MoneyGram" to "MGiAlloy" >
-- Jira ID:	    <AL-218>
--===========================================================================================

UPDATE 
	tMessageStore 
SET 
	Processor = 'MGiAlloy',
	Content = 'MGiAlloy Transaction Declined..'
WHERE 
	MessageKey = '1002.2001' 
	AND PartnerPK = 1
	
GO


UPDATE 
	tMessageStore 
SET 
	Processor = 'MGiAlloy'	
WHERE 
	MessageKey IN ('1008.6003', '1008.5001', '1008.5000', '1008.6012', '1008.6004', '1008.6002', '1008.5002', '1008.5003', '1008.5004')
	AND PartnerPK = 1

GO

UPDATE 
	tMessageStore 
SET 
	Processor = 'MGiAlloy',
	content ='Exceeded MGiAlloy Limit Check'
WHERE 
	MessageKey IN ('1008.6006','1008.6005','1008.6000','1008.6001')	
	AND PartnerPK = 1
	
GO

