-- ================================================================================
-- Author:		<Chinar Kulkarni>
-- Create date: <06/30/2015>
-- Description:	<Update the validation for Matricula Consular Id to accept 9 digits>
-- Jira ID:		<AL-616>
-- ================================================================================

UPDATE tNexxoIdTypes
SET Mask = '^\d{6,9}$'
WHERE Name = 'MATRICULA CONSULAR'
AND Country = 'MEXICO'