--=========================================================
-- Author: <Kaushik S>
-- Date Created: <Feb 23 2015>
-- Description:< Adding new coloum IsTCForcePrintRequired 
--				to tChannelPartnerProductProcessorsMapping Table to make 
--				WU T&C condition Pop-up configurable>
-- User Story ID: <AL-59>
--==========================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'IsTnCForcePrintRequired' AND Object_ID = Object_ID(N'tChannelPartnerProductProcessorsMapping'))
BEGIN 
	ALTER TABLE tChannelPartnerProductProcessorsMapping
	ADD IsTnCForcePrintRequired BIT NOT NULL CONSTRAINT DF_tChannelPartnerProductProcessorsMapping_IsTnCForcePrintRequired DEFAULT (0)
END
GO

