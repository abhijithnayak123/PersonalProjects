--===========================================================================================
-- Author:		<Rogy Eapen>
-- Create date: <June 26 2015>
-- Description:	<Script to add new Tech user role>            
-- Jira ID:	<AL-546>
--===========================================================================================
IF NOT EXISTS(SELECT 1 FROM tUserRoles WHERE  Id = 5) 
BEGIN
	INSERT [dbo].tUserRoles( Id,Role) VALUES (5, 'Tech')
END
GO