--- ===================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <11/23/2015>
-- Description:	<As Alloy, I need a minimum age for customers to register in the system for Synovus>
-- Jira ID:		<AL-1627>
-- =====================================================================================


IF EXISTS 
        (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerConfig' AND COLUMN_NAME = 'CustomerMinimumAge'		  
		)
BEGIN
     
    -- Update Minimum Age as '13' for Customer Registration.  

	DECLARE @synovusChannelPartnerPK UNIQUEIDENTIFIER
	DECLARE @customerMinimumAge INT

    select @SynovusChannelPartnerPK = ChannelPartnerPK from tChannelPartners WHERE NAME = 'Synovus'
	SET @CustomerMinimumAge = 13
	
	UPDATE tChannelPartnerConfig SET CustomerMinimumAge = @customerMinimumAge  
	WHERE ChannelPartnerPK = @synovusChannelPartnerPK
	

END
GO




