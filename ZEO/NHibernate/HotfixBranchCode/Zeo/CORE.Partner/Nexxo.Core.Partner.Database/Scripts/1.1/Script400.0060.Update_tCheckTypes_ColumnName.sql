--===========================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <Mar 25 2015>
-- Description:	<Script to update column Name from RAC/Loan  to Loan/RAL >
-- Jira ID:	    <AL-111>
--===========================================================================================

UPDATE 
	tCheckTypes 
SET 
	name = 'Loan/RAL' 
WHERE 
	id = 14 
	AND name = 'RAC/Loan'

GO
	