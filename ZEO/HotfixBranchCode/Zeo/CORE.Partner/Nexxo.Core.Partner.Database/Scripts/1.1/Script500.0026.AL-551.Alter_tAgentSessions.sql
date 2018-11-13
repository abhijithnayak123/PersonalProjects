
--===========================================================================================
-- Author:		<Prince Bajaj>
-- Create date: <May 28 2015>
-- Description:	<Script to add BusinessDate column in tAgentSession table>
-- Jira ID:	    <AL-551>
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'BusinessDate' AND Object_ID = Object_ID(N'tAgentSessions'))
	BEGIN 
		ALTER TABLE tAgentSessions
		ADD BusinessDate DATE NULL
	END
GO