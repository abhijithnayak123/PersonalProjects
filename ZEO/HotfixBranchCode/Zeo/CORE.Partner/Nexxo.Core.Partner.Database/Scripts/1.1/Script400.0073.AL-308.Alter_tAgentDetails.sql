--===========================================================================================
-- Author:		<Ashok Kumar G>
-- Create date: <April 17 2015>
-- Description:	<Script to add Teller number column in tAgentDetails table>
-- Jira ID:	    <AL-308>
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'ClientAgentIdentifier' AND Object_ID = Object_ID(N'tAgentDetails'))
	BEGIN 
		ALTER TABLE tAgentDetails
		ADD ClientAgentIdentifier NVARCHAR(20)
	END
GO