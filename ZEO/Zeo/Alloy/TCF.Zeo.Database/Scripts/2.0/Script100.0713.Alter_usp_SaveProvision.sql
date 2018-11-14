--- ===============================================================================
-- Author:		<M.Purna Pushkal>
-- Create date: <02-22-2018>
-- Description:	 SP to save the Provision details
-- Jira ID:		<B-13218>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'usp_SaveProvision')
BEGIN
	DROP PROCEDURE usp_SaveProvision
END
GO

CREATE PROCEDURE usp_SaveProvision
(
	 @promoProvisionId BIGINT
	,@promotionId BIGINT
	,@value DECIMAL(18,2)
	,@minAmount MONEY
	,@maxAmount MONEY
	,@checkTypeIds NVARCHAR(500)
	,@groups NVARCHAR(500)
	,@discountType INT
	,@locationIds NVARCHAR(500)
	,@dTServerDate DATETIME
	,@dTTerminalDate DATETIME 
	,@dbPromoProvisionId BIGINT OUTPUT
)
AS 
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION

	IF @promoProvisionId = 0
	BEGIN
		INSERT INTO tPromoProvisions
		(
			PromotionId,
			Value,
			MinAmount,
			MaxAmount,
			CheckTypeIds,
			locationIds,
			Groups,
			DiscountType,
			DTServerCreate,
			DTTerminalCreate
		)
		VALUES
		(
			@promotionId,
			@value, 
			@minAmount,
			@maxAmount,
			@checkTypeIds,
			@locationIds,
			@groups,
			@discountType,
			@dTServerDate,
			@dTTerminalDate
		) 

		SELECT @dbPromoProvisionId = CAST(SCOPE_IDENTITY() AS BIGINT)

	END 
	ELSE
	BEGIN
	 UPDATE tPromoProvisions
	 SET
		 Value = @value,
		 MinAmount = @minAmount,
		 MaxAmount = @maxAmount,
		 CheckTypeIds = @checkTypeIds,
		 locationIds = @locationIds,
		 Groups = @groups,
		 DiscountType = @discountType,
		 DTServerLastModified = @dTServerDate, 
		 DTTerminalLastModified = @dTTerminalDate
	 WHERE PromoProvisionId = @promoProvisionId 

	 SELECT @dbPromoProvisionId = @promoProvisionId

	END
	COMMIT
	END TRY

BEGIN CATCH
		ROLLBACK
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
END CATCH
END 