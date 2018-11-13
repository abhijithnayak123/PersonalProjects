--===========================================================================================
-- Auther:			<Nitish Biradar>
-- Date Created:	<27-July-2015>
-- Description:		<Script for Update two digit country code in tMasterCountries>
-- Jira ID:			<AL-769>
--===========================================================================================

UPDATE 
	tMasterCountries
SET 
	Abbr2 = 'GG'
WHERE 
	Name = 'GUERNSEY' 
	AND Id = '1000000088'
GO

UPDATE 
	tMasterCountries
SET 
    Abbr2 = 'GN'
WHERE 
	Name = 'GUINEA' 
	AND Id = '1000000089'

GO