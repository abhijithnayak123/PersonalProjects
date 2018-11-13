--===========================================================================================
-- Author:		<Swarnalakshmi S>
-- Create date: <Feb 04 2016>
-- Description:	<Script to add ClientAgentIdentifier in tAgentSessions table>
-- Jira ID:	    <AL-5047>
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'ClientAgentIdentifier' AND Object_ID = Object_ID(N'tAgentSessions'))
	BEGIN 
		ALTER TABLE tAgentSessions
		ADD ClientAgentIdentifier NVARCHAR(20)
	END
GO