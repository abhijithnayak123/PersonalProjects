--- ===============================================================================
-- Author:		<Nitish Biradar>
-- Modified By: Abhijith
-- Modified By: Nitish Biradar
-- Create date: <01-19-2018>
-- Modified Date: 04-10-2018
-- Modified Date: 05-08-2018
-- Description:	 SP to get the promotions based on the filter
-- Modified Reason: Passing the "IsPromotionHidden" from the database to bind in Promotion screen.
-- Modified Reason: Make the Provider to be a non-mandatory field
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
		tp.Name AS ProductName, tp.ProductsID AS ProductId, tpr.Name AS ProviderName, tppm.Code AS ProviderId, Priority, StartDate, EndDate, Status, 
		IsNextCustomerSession, IsOverridable, IsPrintable, IsSystemApplied, FreeTransactionCount, Stackable, IsPromotionHidden
	FROM 
		tPromotions p WITH (NOLOCK)
		INNER JOIN tProducts tp WITH (NOLOCK) ON tp.ProductsID = p.ProductId
		LEFT JOIN tProductProcessorsMapping tppm WITH (NOLOCK) ON tppm.Code = p.ProviderId
		LEFT JOIN tProcessors tpr WITH (NOLOCK) ON tpr.ProcessorsID = tppm.ProcessorId
	WHERE 
		PromotionId = @promotionId

	SELECT
		pq.PromoQualifierId AS QualifierId, tp.ProductsID AS ProductId, tp.Name as ProductName, pq.EndDate, pq.Amount, pq.MinTransactionCount 
		,pq.TransactionStates, pq.IsPaidFee
	FROM
		tPromoQualifiers pq WITH(NOLOCK)
		INNER JOIN tProducts tp WITH(NOLOCK) ON pq.ProductId = tp.ProductsID
	WHERE 
		pq.PromotionId = @promotionId

	SELECT
		PromoProvisionId AS ProvisionId, locationIds as Locations, CheckTypeIds AS CheckTypes, Value, MinAmount, MaxAmount, DiscountType , Groups
	FROM
		tPromoProvisions  WITH(NOLOCK)
	WHERE 
		PromotionId = @promotionId
END