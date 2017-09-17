-- ============================================================
-- Author:		<Lokesh>
-- Create date: <12/16/2014>
-- Description:	<Script for updating the check franking data fro TCF>
-- Rally ID:	<US2332>
-- ============================================================

DECLARE @partnerPK UNIQUEIDENTIFIER
SELECT @partnerPK = rowguid FROM tChannelPartners WHERE Name = 'TCF'

UPDATE 
	tChannelPartnerConfig 
SET 
	IsCheckFrank = 1,
	FrankData = 'FormatedTransactionDate| SequenceNo|%26>291070001%26<TCF NATL Z'
WHERE 
	ChannelPartnerID = @partnerPK 

GO