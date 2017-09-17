-- ============================================================
-- Author:		<Abhijith>
-- Create date: <12/02/2014>
-- Description:	<Script for updating the check franking data fro TCF>
-- Rally ID:	<US1627>
-- ============================================================

DECLARE @partnerPK UNIQUEIDENTIFIER
SELECT @partnerPK = rowguid FROM tChannelPartners WHERE Name = 'TCF'

UPDATE 
	tChannelPartnerConfig 
SET 
	IsCheckFrank = 1,
	FrankData = 'CheckNumber|TransactionID'
WHERE 
	ChannelPartnerID = @partnerPK 

GO