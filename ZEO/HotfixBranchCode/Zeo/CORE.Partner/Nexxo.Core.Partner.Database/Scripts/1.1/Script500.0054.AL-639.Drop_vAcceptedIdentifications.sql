--===========================================================================================
-- Author:		<Rogy Eapen>
-- Created date: <July 03 2015>
-- Description:	<Script to drop vAcceptedIdentifications view>           
-- Jira ID:	<AL-639>
--===========================================================================================

IF EXISTS (SELECT name FROM sys.views
   WHERE name = 'vAcceptedIdentifications')
BEGIN
   DROP VIEW vAcceptedIdentifications
END
GO