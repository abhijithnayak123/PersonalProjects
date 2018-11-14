--==========================================================================
-- Author: <KAUSHIK S>
-- Date Created: <MAY 26 2015>
-- Description: < As Certegy, I need check capture information configurations.  >
-- User Story ID: <AL-437>
--===========================================================================


IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CheckEntryTypes' AND Object_ID = Object_ID(N'tChannelPartnerConfig'))
BEGIN 
ALTER TABLE tChannelPartnerConfig
ADD CheckEntryTypes SMALLINT NOT NULL DEFAULT (1)
END

GO