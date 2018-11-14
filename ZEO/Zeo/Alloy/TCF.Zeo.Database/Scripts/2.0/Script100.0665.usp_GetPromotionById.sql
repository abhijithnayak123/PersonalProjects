--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Create date: <01-19-2018>
-- Description:	 SP to get the promotions based on the filter
-- Jira ID:		<B-12321>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_GetPromotionById')
BEGIN
	DROP PROCEDURE usp_GetPromotionById
END 
GO

CREATE PROCEDURE usp_GetPromotionById
(
	@promotionId BIGINT,
	@channelpartnerId INT
)
AS
BEGIN

	SELECT 
		PromotionId, p.Name AS PromotionName, Description,
		tp.Name AS ProductName, tp.ProductsID AS ProductId, tpr.Name AS ProviderName, tppm.Code AS ProviderId, Priority, StartDate, EndDate, IsActive, 
		IsNextCustomerSession, IsOverridable, IsPrintable, IsSystemApplied 
	FROM 
		tPromotions p WITH (NOLOCK)
		INNER JOIN tProducts tp WITH (NOLOCK) ON tp.ProductsID = p.ProductId
		INNER JOIN tProductProcessorsMapping tppm WITH (NOLOCK) ON tppm.ProductId = tp.ProductsID
		INNER JOIN tProcessors tpr WITH (NOLOCK) ON tpr.ProcessorsID = tppm.ProcessorId
		INNER JOIN tChannelPartnerProductProcessorsMapping tcr WITH (NOLOCK) ON tcr.ProductProcessorId = tppm.ProductProcessorsMappingID
	WHERE 
		tcr.ChannelPartnerID = @channelpartnerId AND PromotionId = @promotionId

	SELECT
		pq.PromoQualifierId AS QualifierId, tp.ProductsID AS ProductId, tp.Name as ProductName, pq.EndDate, pq.Amount, pq.MinTransactionCount, pq.MaxTransactionCount 
		,pq.TransactionStates, pq.IsPaidFee
	FROM
		tPromoQualifiers pq WITH(NOLOCK)
		INNER JOIN tProducts tp WITH(NOLOCK) ON pq.ProductId = tp.ProductsID
	WHERE 
		pq.PromotionId = @promotionId

	SELECT
		PromoProvisionId AS ProvisionId, locationIds as Locations, CheckTypeIds AS CheckTypes, DiscountValue, MinAmount, MaxAmount, IsPercentage , Groups
	FROM
		tPromoProvisions  WITH(NOLOCK)
	WHERE 
		PromotionId = @promotionId
END