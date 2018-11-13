-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <06/22/2015>
-- Description:	<To update the regular expression for EMPLOYMENT AUTHORIZATION CARD (EAD) Id type>
-- Jira ID:		<AL-562>
-- ================================================================================
 
UPDATE
	tNexxoIdTypes 
SET
	Mask = '^([aA]0\d{8}$)|^\d{7,9}$'  
WHERE 
	NexxoIdTypeID = 160
GO 