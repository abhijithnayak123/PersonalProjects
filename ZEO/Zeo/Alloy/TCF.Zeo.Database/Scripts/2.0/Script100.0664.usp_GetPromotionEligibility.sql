-- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <01-17-2017>
-- Description:	Get Eligibility Promotion for a customer
-- ===============================================================================


-- EXEC usp_GetPromotionEligibility 'BOGO', 1000000002, 90, 2.70, 1000000003, 10, 0, '2018-02-08', 1, 2642468260740277, 200

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
	 SELECT PromotionId, [dbo].ufn_ValidateQualifiers(@promoCode, PromotionId, @transactionId, IsNextCustomerSession, @customerSessionId, ProviderId, @customerId), Name, Description, IsOverridable, Priority
	 FROM   tPromotions
	 WHERE  @currentDate BETWEEN StartDate AND EndDate --AND Status = 1 
   	        AND
			(
				Name = @promoCode
				OR 
				@promoCode = ''      -- AND IsSystemApplied = 1  -- In case of manual promotion, we will get promo code value in field - '@promoCode' or else we will check all the Promos.
			)														 
			AND ProductId = @productId AND ProviderId = @providerId 
	
   
   
   
     -- Get the Discount value based on CheckAmount, CheckType, location, Priority for the Qualified promotions.

	 SELECT
		p.PromotionId,
		p.Name,
		p.IsOverridable,
		p.Description,
		MIN(CASE 
		   pv.IsPercentage 
		   WHEN 1 THEN (@baseFee * pv.DiscountValue/100 * -1)                -- Discount value will be calculated based on IsPercentage flag, and returned discount value should be nagative   
		   ELSE 
				CASE 
					WHEN pv.DiscountValue < @baseFee THEN (pv.DiscountValue * -1) 
					ELSE (@baseFee * -1)
				END
		   END)
		AS DiscountValue,
		p.Priority,
		CONVERT(BIT,CASE ISNULL(pv.Groups,'') WHEN '' THEN 0 ELSE 1 END) AS IsGroupPromo 
		FROM 
		@applicablePromotions p 
		INNER JOIN tPromoProvisions pv ON pv.PromotionId = p.PromotionId and p.IsPromoQualified = 1  --  filtering only qualified promotions
	  WHERE
		(ISNULL(pv.CheckTypeIds,'') = '' OR dbo.ufn_IsItemInList(@checkTypeId, pv.CheckTypeIds) = 1)  -- Check Type Ids is comma seperated, this should be moved to seperate function.
		AND
		(ISNULL(pv.locationIds,'') = '' OR dbo.ufn_IsItemInList(@locationStateCode, pv.locationIds) = 1)    -- Location Ids is comma seperated, this should be moved to seperate function.
		AND 
		  (
				(ISNULL(MinAmount,0) = 0 OR MinAmount <= @checkAmount)
				AND
				(ISNULL(MaxAmount,0) = 0 OR MaxAmount >= @checkAmount)
		  )
		AND
		(ISNULL(pv.Groups, '') = '' OR  dbo.ufn_IsCustomerBelongsToGroup(@customerId, pv.Groups) = 1)      -- Verify- Whether the customer is belongs to the group or not. It will return 1 if the groupId is null.

		GROUP BY p.PromotionId, p.Name, p.IsOverridable, p.Description, p.Priority, pv.Groups                      -- Take max discount value, if we have two provisions for same promotions
	  
    END TRY

BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH

END