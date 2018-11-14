--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Modified By : <Manikandan Govindraj>
-- Modified date: <12-01-2017>
-- Create date: <09-15-2016>
-- Description:	 Stored procedure for pricing cluster
-- Modified description : Fetching MaximumFee for ON-US check cashing.
-- Jira ID:		<AL-7927>

--EXEC usp_PricingCluster 34,'',5,NULL
-- ================================================================================

IF EXISTS (SELECT  1 FROM sys.objects WHERE NAME = 'usp_PricingCluster')
BEGIN
	DROP PROCEDURE usp_PricingCluster
END
GO

CREATE PROCEDURE usp_PricingCluster
	@channelPartnerId INT,
	@locationId BIGINT,
	@productId INT,
	@productType VARCHAR(10),
	@productProviderCode INT
AS
BEGIN
	BEGIN TRY

		DECLARE @isExists BIT

		SET @isExists =
		(
			SELECT 
				 CASE 
					WHEN COUNT(1) > 0
					THEN 1 
				    ELSE 0 
				  END  
			FROM 
				tChannelPartnerPricing cpp
			WHERE
				cpp.ProductId = @productId AND cpp.ChannelPartnerId = @channelPartnerId AND (@productType IS NULL OR cpp.ProductType = CAST (@productType AS INT))
				AND cpp.LocationId = @locationId AND ProductProviderCode = @productProviderCode

		)

		IF @isExists = 1
		BEGIN

			SELECT 
				CompareTypeId,  MinimumAmount, MaximumAmount, MinimumFee, MaximumFee, Value, IsPercentage
			FROM 
				tChannelPartnerPricing cpp
			INNER JOIN tPricingGroups pg ON  cpp.PricingGroupId = pg.PricingGroupsID
			INNER JOIN tPricing p ON pg.PricingGroupsID = p.PricingGroupId
			WHERE
				 cpp.ProductId = @productId 
				 AND 
				 cpp.ChannelPartnerId = @channelPartnerId 
				 AND 
				 cpp.LocationId = @locationId 
				 AND 
				 (@productType IS NULL OR cpp.ProductType =  CAST(@productType AS INT))
				 AND 
				 (cpp.ProductProviderCode = @productProviderCode OR cpp.ProductProviderCode IS NULL)

		END
		ELSE
		BEGIN

		   SELECT 
				CompareTypeId,  MinimumAmount, MaximumAmount, MinimumFee, MaximumFee, Value, IsPercentage
			FROM 
				tChannelPartnerPricing cpp
			INNER JOIN tPricingGroups pg ON  cpp.PricingGroupId = pg.PricingGroupsID
			INNER JOIN tPricing p ON pg.PricingGroupsID = p.PricingGroupId
			WHERE
				cpp.ChannelPartnerId = @channelPartnerId  AND cpp.ProductId = @productId 
				AND 
				(@productType IS NULL OR cpp.ProductType =  CAST(@productType AS INT))	
				AND 
				(cpp.ProductProviderCode = @productProviderCode OR cpp.ProductProviderCode IS NULL)
				AND 
				cpp.LocationId IS NULL
		END

	END TRY
	BEGIN CATCH	        
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END

