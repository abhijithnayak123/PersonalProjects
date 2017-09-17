--===========================================================================================
-- Auther:			<Bineesh Raghavan>
-- Date Created:	<01/23/2015>
-- Description:		<script to update Visa load fee to 0.00>
-- Rally ID:		<US2091, TA6153>		
--===========================================================================================

DECLARE @partnerPK UNIQUEIDENTIFIER
SELECT @partnerPK = rowguid FROM tChannelPartners WHERE Name = 'TCF'

UPDATE 
	tChannelPartnerFees_Funds 
SET 
	Fee = 0.00 
WHERE 
	ChannelPartnerPK = @partnerPK 
	AND FundsType = 2 -- Activation
GO
