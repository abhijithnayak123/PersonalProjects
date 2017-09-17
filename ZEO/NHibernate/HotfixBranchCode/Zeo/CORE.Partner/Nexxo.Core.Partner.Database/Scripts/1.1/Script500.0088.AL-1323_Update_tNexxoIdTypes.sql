-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <08/31/2015>
-- Description:	<To update the regular expression for DRIVER'S LICENSE and GEORGIA state>
-- Jira ID:		<AL-1323>
-- ================================================================================
 
UPDATE
	tNexxoIdTypes 
SET
	Mask = '^\d{7,9}$' 
WHERE 
	NexxoIdTypeID = 116 
GO 