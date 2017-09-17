--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <10/28/2015>
-- Description:	<As Alloy, the pricing currently in effect for current clients will continue to exist and will be applied through the pricing engine>
-- Jira ID:		<AL-1760>
-- ================================================================================

	
	IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tChannelPartnerFees_Check')
	BEGIN
		DROP TABLE tChannelPartnerFees_Check
	END

	IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tChannelPartnerFees_Funds')
	BEGIN
		DROP TABLE tChannelPartnerFees_Funds
	END
	
    IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tChannelPartnerFees_MoneyOrder')
	BEGIN
		DROP TABLE tChannelPartnerFees_MoneyOrder
	END
