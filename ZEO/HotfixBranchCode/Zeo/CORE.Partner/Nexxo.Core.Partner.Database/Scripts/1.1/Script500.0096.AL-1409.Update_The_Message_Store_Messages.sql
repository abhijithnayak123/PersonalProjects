-- ============================================================================
-- Author:		<Anisha Abraham>
-- Create date: <09/08/2015>
-- Description:	<Ingo decline codes are not matching for some of the decline codes>
-- Jira ID:	<AL-1409>
-- =============================================================================

UPDATE 
	tMessageStore 
SET 
	Content = 'THE CHECK CANNOT BE APPROVED AT THIS TIME.Please return the check to the customer.'
WHERE
	MessageKey = '1002.3' 
	AND PartnerPK = 1 
	AND Processor = 'Chexar'
GO

UPDATE 
	tMessageStore 
SET 
	Content = 'THE CHECK CANNOT BE APPROVED AT THIS TIME.The image of the check could not be read, rescan the check.'
WHERE 
	MessageKey = '1002.4' 
	AND PartnerPK = 1 
	AND Processor = 'Chexar'
GO

UPDATE 
	tMessageStore 
SET 
	Content = 'THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed. Please return the check to the customer.'
WHERE 
	MessageKey = '1002.5' 
	AND PartnerPK = 1 
	AND Processor = 'Chexar'
GO

UPDATE 
	tMessageStore 
SET 
	Content = 'THE CHECK CANNOT BE APPROVED AT THIS TIME.The check cannot be cashed. Please return the check to the customer.'
WHERE 
	MessageKey = '1002.7' 
	AND PartnerPK = 1 
	AND Processor = 'Chexar'
GO
