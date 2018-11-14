-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <01-17-2017>
-- Description:	Get Eligibility Promotion for a customer
-- ===============================================================================


-- EXEC usp_GetPromotionEligibility '', 1000000016, 90, 2.70, 'MN', 0, 0, '2018-03-14', 5, 2808367073347145, 503

IF EXISTS (SELECT  1 FROM sys.objects WHERE NAME = 'usp_GetPromotionEligibility')
BEGIN
	DROP PROCEDURE usp_GetPromotionEligibility
END
GO

CREATE PROCEDURE usp_GetPromotionEligibility
(
    @promoCode         NVARCHAR(200),
	@customerSessionId BIGINT,
	@checkAmount       MONEY,
	@baseFee           MONEY,
	@locationStateCode NVARCHAR(20),
	@checkTypeId       INT,
    @transactionId     BIGINT,
	@currentDate       DATE,
	@productId         INT,
	@customerId        BIGINT,
	@providerId        INT	
)
AS
BEGIN
	
	BEGIN TRY

	
	 DECLARE @applicablePromotions TABLE (PromotionId BIGINT, IsPromoQualified BIT, Name NVARCHAR(100), Description NVARCHAR(1000), IsOverridable BIT, Priority INT)	

	 -- Get Applicable promotions for the Product and Provider based on Start date and end date, and Qualifiers should be vallidated in 'ufn_ValidateQualifiers' function.	  
	  
	 INSERT INTO @applicablePromotions
	 SELECT PromotionId, [dbo].ufn_ValidateQualifiers(@promoCode, PromotionId, @transactionId, IsNextCustomerSession, @customerSessionId, ProviderId, @customerId, FreeTransactionCount), Name, Description, IsOverridable, Priority
	 FROM   tPromotions
	 WHERE  @currentDate BETWEEN StartDate AND EndDate AND Status = 1 
   	        AND
			(
				Name = @promoCode
				OR 
				@promoCode = ''      -- AND IsSystemApplied = 1  -- In case of manual promotion, we will get promo code value in field - '@promoCode' or else we will check all the Promos.
			)														 
			AND ProductId = @productId AND ProviderId = @providerId 
	
      
     -- Get the Discount value based on CheckAmount, CheckType, location and discountType for the Qualified promotions.

	 SELECT
		p.PromotionId,
		p.Name,
		p.IsOverridable,
		p.Description,
		MAX(CASE 
		   pv.DiscountType                                           -- Discount type 1 => Percentage, 2 => flat rate, 3 => Fixed
		   WHEN 1 THEN (@baseFee * pv.Value/100 * -1)                -- Discount value will be calculated based on 'DiscountType' flag, and returned discount value should be nagative   
		   WHEN 2 THEN
				CASE 
					WHEN pv.Value < @baseFee THEN (pv.Value * -1) 
					ELSE (@baseFee * -1)
				END
		  ELSE
		        CASE 
				    WHEN pv.Value < @baseFee THEN ((@baseFee - pv.Value) * -1) 
					ELSE 0
				END 
		  END)
		AS DiscountValue,
		p.Priority,
		CONVERT(BIT,CASE ISNULL(pv.Groups,'') WHEN '' THEN 0 ELSE 1 END) AS IsGroupPromo 
		FROM 
		@applicablePromotions p 
		INNER JOIN tPromoProvisions pv ON pv.PromotionId = p.PromotionId and p.IsPromoQualified = 1       -- filtering only qualified promotions
	  WHERE
		(ISNULL(pv.CheckTypeIds, '') = '' OR ISNULL(@checkTypeId, 0) = 0 OR dbo.ufn_IsItemInList(@checkTypeId, pv.CheckTypeIds) = 1)      -- Check Type Ids is comma seperated, this should be moved to seperate function.
		AND
		(ISNULL(pv.locationIds, '') = '' OR dbo.ufn_IsItemInList(@locationStateCode, pv.locationIds) = 1)  -- Location Ids is comma seperated, this should be moved to seperate function.
		AND 
		  (
				(ISNULL(MinAmount, 0) = 0 OR MinAmount <= @checkAmount)
				AND
				(ISNULL(MaxAmount, 0) = 0 OR MaxAmount >= @checkAmount)
		  )
		AND
		(ISNULL(pv.Groups, '') = '' OR  dbo.ufn_IsCustomerBelongsToGroup(@customerId, pv.Groups) = 1)      -- Verify- Whether the customer is belongs to the group or not. It will return 1 if the groupId is null.

		GROUP BY p.PromotionId, p.Name, p.IsOverridable, p.Description, p.Priority, pv.Groups              -- Take max discount value, if we have two provisions for same promotions
	  
    END TRY

BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH

END