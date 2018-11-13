--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	29/05/2014
-- Description:		Script for insert TCF Fee for Money order,fund and check
-- Rally ID:		US1610-TCF Whitelabel
--===========================================================================================

DECLARE @PartnerPK UNIQUEIDENTIFIER,
		@FeeMin DECIMAL
SELECT @PartnerPK = rowguid FROM tChannelPartners WHERE id = 34

--MoneyOrder Fee		
INSERT tChannelPartnerFees_MoneyOrder
	(ChannelPartnerPK, Fee)
VALUES 
	(@PartnerPK, 5.00)

--Funds Fee
INSERT tChannelPartnerFees_Funds
	(ChannelPartnerPK, FundsType, Fee)
VALUES
	(@PartnerPK, 0, 0.00),
	(@PartnerPK, 1, 0.00),
	(@PartnerPK, 2, 0.00)

SET @FeeMin = 0.00

--Check Fee
INSERT tChannelPartnerFees_Check
	(ChannelPartnerPK, CheckType, FeeRate, FeeMinimum)
VALUES 
	(@PartnerPK, 1, 0.030, @FeeMin),
	(@PartnerPK, 2, 0.010, @FeeMin),
	(@PartnerPK, 3, 0.010, @FeeMin),
	(@PartnerPK, 4, 0.010, @FeeMin),
	(@PartnerPK, 5, 0.030, @FeeMin),
	(@PartnerPK, 6, 0.030, @FeeMin),
	(@PartnerPK, 7, 0.010, @FeeMin),
	(@PartnerPK, 8, 0.010, @FeeMin),
	(@PartnerPK, 9, 0.010, @FeeMin),
	(@PartnerPK, 10, 0.030, @FeeMin),
	(@PartnerPK, 11, 0.030, @FeeMin),
	(@PartnerPK, 12, 0.030, @FeeMin),
	(@PartnerPK, 13, 0.030, @FeeMin),
	(@PartnerPK, 14, 0.030, @FeeMin),
	(@PartnerPK, 15, 0.030, @FeeMin),
	(@PartnerPK, 16, 0.030, @FeeMin),
	(@PartnerPK, 17, 0.030, @FeeMin)
--===========================================================================================
