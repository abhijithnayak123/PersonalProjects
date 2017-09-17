-- ============================================================
-- Author:		Sunil Shetty
-- Create date: <06/09/2015>
-- Description:	<Added columns to make MailingAddress configurable>
-- Rally ID:	<AL-533>
-- ============================================================

IF NOT EXISTS 
(
	SELECT 
		1 
	FROM   
		sys.columns 
	WHERE 
		name = N'IsMailingAddressEnable' 
		AND object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]')      
)
BEGIN         
	ALTER TABLE tChannelPartnerConfig 
	ADD IsMailingAddressEnable BIT NOT NULL CONSTRAINT [DF_tChannelPartnerConfig_MailingAddressEnable] DEFAULT((1))
END
GO