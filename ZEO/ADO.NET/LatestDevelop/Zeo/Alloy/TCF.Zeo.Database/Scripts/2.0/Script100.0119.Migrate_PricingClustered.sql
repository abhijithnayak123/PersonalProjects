--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <09-15-2016>
-- Description:	 Migration script for pricing cluster
-- Jira ID:		<AL-7927>
-- ================================================================================

DISABLE TRIGGER tChannelPartnerPricing_Audit ON tChannelPartnerPricing
GO
  
BEGIN TRY  
  BEGIN TRANSACTION   
	
	UPDATE CPP SET CPP.PricingGroupId = PG.PricingGroupsID FROM tChannelPartnerPricing AS CPP
	   INNER JOIN tPricingGroups AS PG ON PG.PricingGroupPK = CPP.PricingGroupPK
	   
	UPDATE CPP SET CPP.ChannelPartnerId = CP.ChannelPartnerId FROM tChannelPartnerPricing AS CPP
	   INNER JOIN tChannelPartners AS CP ON CPP.ChannelPartnerPK = CP.ChannelPartnerPK

	UPDATE CPP SET CPP.LocationId = L.LocationID FROM tChannelPartnerPricing AS CPP
	   INNER JOIN tLocations AS L ON CPP.LocationId= L.LocationID

	UPDATE CPP SET CPP.ProductId = P.ProductsId FROM tChannelPartnerPricing AS CPP
	   INNER JOIN tProducts AS P ON CPP.ProductPK = P.ProductsPK

	UPDATE P  SET P.PricingGroupId = PG.PricingGroupsId FROM tPricing AS P
	   INNER JOIN tPricingGroups AS PG ON PG.PricingGroupPK = P.PricingGroupPK
	   
  COMMIT

END TRY
BEGIN CATCH
	 ROLLBACK
END CATCH
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME  = 'PricingGroupId')
BEGIN
	ALTER TABLE tChannelPartnerPricing 
	ALTER COLUMN PricingGroupId BIGINT NOT NULL 
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME  = 'ProductId')
BEGIN
	ALTER TABLE tChannelPartnerPricing 
	ALTER COLUMN ChannelPartnerId INT NOT NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tChannelPartnerPricing' AND COLUMN_NAME  = 'ChannelPartnerId')
BEGIN
	ALTER TABLE tChannelPartnerPricing 
	ALTER COLUMN ChannelPartnerId INT NOT NULL
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tPricing' AND COLUMN_NAME = 'PricingGroupId')
BEGIN
	ALTER TABLE tPricing 
	ALTER COLUMN PricingGroupId BIGINT NOT NULL 
END
GO

--ENABLE TRIGGER tChannelPartnerPricing_Audit ON tChannelPartnerPricing
--GO