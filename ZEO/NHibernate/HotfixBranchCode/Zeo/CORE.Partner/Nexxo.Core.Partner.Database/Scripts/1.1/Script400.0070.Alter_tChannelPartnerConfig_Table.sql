--==========================================================================
-- Author: <Ashok Kumar G>
-- Date Created: <April 15 2015>
-- Description: < Remove the MGIAlloyLogo from upper right for TCF only.  >
-- User Story ID: <AL-291>
--===========================================================================

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IsMGIAlloyLogoEnable' AND Object_ID = Object_ID(N'tChannelPartnerConfig'))
BEGIN 
	ALTER TABLE [dbo].[tChannelPartnerConfig] 
	ADD IsMGIAlloyLogoEnable BIT NOT NULL DEFAULT 'TRUE'    
END

GO