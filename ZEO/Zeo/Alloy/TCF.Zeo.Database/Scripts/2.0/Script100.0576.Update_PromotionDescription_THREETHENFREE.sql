--- ===============================================================================
-- Author:		<M.Pushkal>
-- Create date: <06-07-2017>
-- Description: Adding the description to the THREETHENFREE promo code
-- ================================================================================

IF EXISTS(SELECT 1 FROM tChannelPartnerFeeAdjustments WHERE Name = 'THREETHENFREE')
BEGIN 
	 UPDATE 
		  tChannelPartnerFeeAdjustments
	 SET 
		  Description = 'No check cashing fee on the fourth check, after three have been cashed'
	 WHERE 
		  Name = 'THREETHENFREE'
END 
