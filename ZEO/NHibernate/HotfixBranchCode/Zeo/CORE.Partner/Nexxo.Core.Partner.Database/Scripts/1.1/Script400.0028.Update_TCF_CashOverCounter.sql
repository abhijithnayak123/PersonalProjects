--===========================================================================================
-- Auther:			<Abhijith Nayak>
-- Date Created:	<01/31/2015>
-- Description:		<script to update cash over counter>
-- Rally ID:		<US1505>		
--===========================================================================================

DECLARE @partnerPK UNIQUEIDENTIFIER
SELECT @partnerPK = rowguid FROM tChannelPartners WHERE Name = 'TCF'

UPDATE 
	tChannelPartnerConfig
SET 
	DisableWithdrawCNP = 1
	,CashOverCounter = 1
WHERE ChannelPartnerID = @partnerPK

UPDATE 
	tChannelPartners
SET 
	CardPresenceVerificationConfig = 2
WHERE 
	rowguid = @partnerPK

GO