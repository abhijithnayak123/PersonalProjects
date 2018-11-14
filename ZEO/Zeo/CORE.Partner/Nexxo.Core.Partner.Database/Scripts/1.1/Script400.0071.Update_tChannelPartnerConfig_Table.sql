--==========================================================================
-- Author: <Ashok Kumar G>
-- Date Created: <April 15 2015>
-- Description: < Remove the MGIAlloyLogo from upper right for TCF only.  >
-- User Story ID: <AL-291>
--===========================================================================
DECLARE @PartnerPK UNIQUEIDENTIFIER

SELECT @PartnerPK = rowguid FROM tChannelPartners WHERE Name = 'TCF'

IF EXISTS (SELECT * FROM sys.columns WHERE name = N'IsMGIAlloyLogoEnable' AND object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]'))
BEGIN         
	UPDATE tChannelPartnerConfig
	SET IsMGIAlloyLogoEnable = 'FALSE'
	WHERE ChannelPartnerID = @PartnerPK
END

GO