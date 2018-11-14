--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <01-19-2018>
-- Description:	 SP to get the promotions based on the filter
-- Jira ID:		<B-12321>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_GetPromotions')
BEGIN
	DROP PROCEDURE usp_GetPromotions
END
GO

CREATE PROCEDURE usp_GetPromotions
(
	 @promotionName NVARCHAR(100) = NULL
	,@promotionStartDate DATE = NULL
	,@promotionEndDate DATE = NULL
	,@productId INT = 0
	,@providerId INT = 0
	,@channelpartnerId INT
	,@serverDate DATE
	,@showExpired BIT
)
AS
BEGIN
	SELECT 
		PromotionId,
		p.Name AS PromotionName,
		Description,
		CASE 
		  WHEN tp.Name = 'ProcessCheck' THEN 'Check Cashing'
		  WHEN tp.Name = 'BillPayment' THEN 'Bill Payment'
		  WHEN tp.Name = 'MoneyTransfer' THEN 'Send Money'
		  WHEN tp.Name = 'ProductCredential' THEN 'ZEO Card' 
		  WHEN tp.Name = 'MoneyOrder' THEN 'Money Order'
		  ELSE tp.Name 
		END AS ProductName,
		CASE 
		  WHEN tpr.Name = 'WesternUnion' THEN 'Western Union'
		  WHEN tpr.Name = 'INGO' THEN 'Ingo'
		  WHEN tpr.Name = 'VISA' THEN 'Visa'
		  ELSE tpr.Name 
		END AS ProviderName,
		Priority,
		StartDate,
		EndDate,
		p.Status AS PromotionStatus,
		FreeTransactionCount,
		Stackable
	FROM 
		tPromotions p WITH (NOLOCK)
		INNER JOIN tProducts tp WITH (NOLOCK) ON tp.ProductsID = p.ProductId
		INNER JOIN tProductProcessorsMapping tppm WITH (NOLOCK) ON tppm.ProductId = tp.ProductsID
		INNER JOIN tProcessors tpr WITH (NOLOCK) ON tpr.ProcessorsID = tppm.ProcessorId
		INNER JOIN tChannelPartnerProductProcessorsMapping tcr WITH (NOLOCK) ON tcr.ProductProcessorId = tppm.ProductProcessorsMappingID
	WHERE 
		tcr.ChannelPartnerID = @channelpartnerId
		AND
		(@promotionName IS NULL OR p.Name = @promotionName)
		AND 
		((@promotionStartDate IS NULL OR @promotionEndDate IS NULL)
		OR 
		(
			CAST(@promotionStartDate AS DATE) BETWEEN CAST(p.StartDate AS DATE) AND CAST(p.EndDate AS DATE)
			OR 
			CAST(@promotionEndDate AS DATE) BETWEEN CAST(p.StartDate AS DATE) AND CAST(p.EndDate AS DATE)
		))
		AND 
		(@productId IS NULL OR @productId = 0 OR p.ProductId = @productId)
		AND 
		(@providerId IS NULL OR @providerId = 0 OR p.ProviderId = @providerId)
		AND
		((@showExpired = 0 AND p.EndDate >= @serverDate) OR @showExpired = 1)
		AND
		(p.Status != 6)
	 ORDER BY 
		p.DTServerCreate DESC 
END 