--===========================================================================================
-- Author:		<Bineesh Raghavan>
-- Created date: <02/26/2016>
-- Description:	<--Updating the table [dbo].[tMessageStore] for the clearer error message, when the customer is in InActive state.>
-- Jira ID:	<AL-3229>
--===========================================================================================

UPDATE 
	[dbo].[tMessageStore] 
SET 
	Content = 'Customer is not active and cannot transact',
	AddlDetails = 'If the customer should be active, then change the status on Profile tab, correct or update any missing or dated information and click done on summary page to update status.'
WHERE 
	MessageKey = '1001.1007'
GO

