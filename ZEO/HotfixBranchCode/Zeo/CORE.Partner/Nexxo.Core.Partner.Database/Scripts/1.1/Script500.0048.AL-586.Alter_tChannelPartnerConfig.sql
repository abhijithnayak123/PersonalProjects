-- ================================================================
-- Author:		Kaushik Sakala
-- Create date: <07/07/2015>
-- Description:	<TCF: Added CanEnableProfileStatus column to determine if customer and change ProfileStatus 
--				even if the customer registration is failed from client side.>
-- JIRA ID:	<AL-586>
-- =================================================================
IF NOT EXISTS 
(
	SELECT 
		1 
	FROM   
		sys.columns 
	WHERE 
		name = N'CanEnableProfileStatus'
		AND object_id = OBJECT_ID(N'[dbo].[tChannelPartnerConfig]')      
)
BEGIN         
	ALTER TABLE tChannelPartnerConfig 
	ADD CanEnableProfileStatus BIT 
	NOT NULL DEFAULT((1))
END
GO