  --- ================================================================================
-- Author:		<Abhijith>
-- Created date: <06/23/2016>
-- Description:	<Script to Updating State PK value for Id types for Florida state>           
-- Jira ID:	<AL-7496>
-- ================================================================================

BEGIN
	UPDATE tNexxoIdTypes 
	SET StatePK = 'E926F22B-CC6B-41E0-8A4B-FF0730596ECF'
	WHERE NexxoIdTypeId IN (115, 172)
END

