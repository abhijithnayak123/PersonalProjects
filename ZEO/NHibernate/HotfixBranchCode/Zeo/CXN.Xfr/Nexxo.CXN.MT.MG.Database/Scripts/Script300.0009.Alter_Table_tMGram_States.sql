--===========================================================================================
-- Auther:			Pamila Jose
-- Date Created:	21/10/2014	
-- Description:		Alter Table tMgram_States
--===========================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tMgram_States]') AND type in (N'U'))
	ALTER TABLE tMgram_States ALTER COLUMN Name
            varchar(255) COLLATE Latin1_General_CI_AI NOT NULL;
 GO
