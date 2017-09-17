--===========================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <06/13/2014>
-- Description:	<System is incorrectly populating MO fees for Synovus>
-- Rally ID:	<DE2914>
--===========================================================================================
DECLARE @PartnerPK uniqueidentifier

-- Synovus 
SELECT @PartnerPK = rowguid from tChannelPartners where id = 33

UPDATE [dbo].[tChannelPartnerFees_MoneyOrder]
SET 
	[fee] = 1.00
WHERE 
	ChannelPartnerPK = @PartnerPK
GO



