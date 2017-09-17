  --===========================================================================================
-- Author:		<Rita Patel>
-- Created date: <June 8th 2015>
-- Description:	<Script to update the spelling of a mexican passport from PASAPORTE to PASSPORT>           
-- Jira ID:	<AL-547>
--=============================================================================================

UPDATE 
	[dbo].[tNexxoIdTypes] 
SET 
	Name = 'PASSPORT' 
WHERE NexxoIdTypeID = 88
GO
