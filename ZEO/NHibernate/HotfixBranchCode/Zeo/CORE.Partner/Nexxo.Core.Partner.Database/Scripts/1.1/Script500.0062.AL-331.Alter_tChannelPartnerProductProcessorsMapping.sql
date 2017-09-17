-- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <07/07/2015>
-- Description:	<As Certegy, I need check capture information configurations.>
-- Jira ID:		<AL-587>
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = N'CheckEntryType' AND Object_ID = Object_ID(N'tChannelPartnerProductProcessorsMapping'))
BEGIN    		
	ALTER TABLE [dbo].[tChannelPartnerProductProcessorsMapping] ADD CheckEntryType SMALLINT NOT NULL DEFAULT(1)
END
GO	

IF EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = N'CheckEntryTypes' AND Object_ID = Object_ID(N'tChannelPartnerConfig'))
BEGIN 
      
	DECLARE @defaultConstraint varchar(100)

	SELECT 
		@defaultConstraint = OBJECT_NAME(default_object_id) 
	FROM 
		SYS.COLUMNS 
	WHERE 
		object_id = object_id('dbo.tChannelPartnerConfig')
		AND NAME = 'CheckEntryTypes'	  
	 
    EXEC ('ALTER TABLE [dbo].[tChannelPartnerConfig] DROP CONSTRAINT ' + @defaultConstraint)
    ALTER TABLE [dbo].[tChannelPartnerConfig] DROP COLUMN CheckEntryTypes
	  
END
GO