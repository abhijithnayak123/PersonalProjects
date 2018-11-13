--===========================================================================================
-- Author:		<Rita Patel>
-- Create date: <April 24 2015>
-- Description:	<Script to insert MasterCountries list in the table>
-- Jira ID:	    <AL-419>
--===========================================================================================

UPDATE 
	tMasterCountries 
SET 
	Name = 'CROATIA' 
WHERE 
	Name = 'CROATIA (local name: Hrvatska)' 
GO
