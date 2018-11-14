-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <06/09/2015>
-- Description:	<To update the regular expression for GREEN CARD / PERMANENT RESIDENT CARD Id type>
-- Jira ID:		<AL-562>
-- ================================================================================

UPDATE
	tNexxoIdTypes 
SET
	Mask = '^([aA]0\d{8}$)|^\d{7,9}$'  
WHERE 
	NexxoIdTypeID = 158
GO 
