--- ===============================================================================
-- Author:		<Abhijith Nayak>
-- Create date: <25-10-2017>
-- Description: Updating the Phone Numbers in Error Messages.
-- ================================================================================

UPDATE 
	tMessageStore
SET 
	AddlDetails = REPLACE(AddlDetails, '763-337-6600', '1-866-TCF-DESK (1-866-823-3375)') 
WHERE 
	AddlDetails LIKE '%763-337-6600%'