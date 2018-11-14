--===========================================================================================
-- Auther:			<Ramakrishna>
-- Date Created:	<26/11/2014>
-- Description:		<Script for Update TCF Fee for Funds Fee>
-- Rally ID:		<US2091, TA6153>		
--===========================================================================================

DECLARE @partnerPK UNIQUEIDENTIFIER
SELECT @partnerPK = rowguid FROM tChannelPartners WHERE Name = 'TCF'

UPDATE 
	tChannelPartnerFees_Funds 
SET 
	Fee = 4.00 
WHERE 
	ChannelPartnerPK = @partnerPK 
	AND FundsType = 2 -- Activation
GO
