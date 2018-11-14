--===========================================================================================
-- Auther:			<Chinar Kulkarni>
-- Date Created:	<29-July-2015>
-- Description:		<Script for updating Alloy to accept 9 or 10 digits for Military ID>
-- Jira ID:			<AL-903>
--===========================================================================================

UPDATE tNexxoIdTypes 
SET Mask = '^\d{9,10}$'
WHERE Name = 'MILITARY ID'

GO