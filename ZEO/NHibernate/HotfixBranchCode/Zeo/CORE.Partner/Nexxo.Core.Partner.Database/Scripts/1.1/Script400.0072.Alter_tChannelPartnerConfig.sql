--===========================================================================================
-- Author:		<Rita Patel>
-- Create date: <April 15 2015>
-- Description:	<Script to add MasterSSN column in tChannelPartnerConfig table>
-- Jira ID:	    <AL-233>
--===========================================================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'MasterSSN' AND Object_ID = Object_ID(N'tChannelPartnerConfig'))
	BEGIN 
		ALTER TABLE tChannelPartnerConfig
		ADD MasterSSN NVARCHAR(15) NOT NULL CONSTRAINT DF_tChannelPartnerConfig_MasterSSN DEFAULT ('888888888')
	END
GO