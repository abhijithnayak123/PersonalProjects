IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_InsertOrUpdateProvisions')
BEGIN
	DROP PROCEDURE usp_InsertOrUpdateProvisions
END 
GO

CREATE PROCEDURE usp_InsertOrUpdateProvisions
(
	 @promotionId BIGINT
	,@provisions XML
)
AS
BEGIN

	DECLARE @provisionsTable TABLE
	(
		  PromoProvisionId BIGINT
		 ,PromotionId BIGINT
		 ,[Value] DECIMAL(18,2)
		 ,MinAmount MONEY
		 ,MaxAmount MONEY
		 ,CheckTypeIds NVARCHAR(500)
		 ,Groups NVARCHAR(500)
		 ,DiscountType INT
		 ,locationIds NVARCHAR(500)
		 ,DTServerDate DATETIME
		 ,DTTerminalDate DATETIME
	)

	INSERT INTO @provisionsTable
	(		
		PromotionId,
		PromoProvisionId,
		[Value],
		MinAmount,
		MaxAmount, 
		CheckTypeIds, 
		DiscountType,
		locationIds,
		Groups,
		DTServerDate, 
		DTTerminalDate
	)
	(
		SELECT
			Provision.value('PromotionId[1]', 'BIGINT') AS PromotionId
		   ,Provision.value('PromoProvisionId[1]', 'DECIMAL(18,2)') AS PromoProvisionId
		   ,Provision.value('Value[1]', 'DECIMAL(18,2)') AS DiscountValue
		   ,Provision.value('MinAmount[1]', 'MONEY') AS MinAmount
		   ,Provision.value('MaxAmount[1]', 'MONEY') AS MaxAmount
		   ,Provision.value('CheckTypeIds[1]', 'NVARCHAR(500)') AS CheckTypeIds
		   ,Provision.value('DiscountType[1]', 'INT') AS DiscountType
		   ,Provision.value('locationIds[1]', 'NVARCHAR(500)') AS locationIds
		   ,Provision.value('Groups[1]', 'NVARCHAR(500)') AS Groups
		   ,Provision.value('DTServerDate[1]', 'DATETIME') AS DTServerDate
		   ,Provision.value('DTTerminalDate[1]', 'DATETIME') AS DTTerminalDate	
		 FROM 
			@provisions.nodes('/Provisions/Provision') AS Promotion(Provision)		
	)


	INSERT INTO tPromoProvisions
	(
	    PromotionId,
	    [Value],
	    MinAmount,
	    MaxAmount,
	    CheckTypeIds,
		DiscountType,
	    locationIds,
		Groups,
	    DTServerCreate,
	    DTTerminalCreate
	)
	(
	   SELECT 
		 PromotionId,
		 [Value],
		 MinAmount,
		 MaxAmount, 
		 CheckTypeIds, 
		 DiscountType,
		 LocationIds,
		 Groups,
		 DTServerDate, 
		 DTTerminalDate
	   FROM 
		 @provisionsTable
	   WHERE PromoProvisionId = 0
	)

	SELECT
		PromoProvisionId AS ProvisionId, PromotionId, locationIds as Locations, CheckTypeIds AS CheckTypes, Value, MinAmount, MaxAmount, DiscountType , Groups
	FROM
		tPromoProvisions  WITH(NOLOCK)
	WHERE 
		PromotionId = @promotionId

END