-- ====================================================================
-- Author:		<Nitish Biradar>
-- Create date: <02/26/2018>
-- Description:	<Verify, the all provision fields are present>
-- =====================================================================

-- select  dbo.ufn_VerifyProvisions(100000001)

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'ufn_VerifyProvisions') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION ufn_VerifyProvisions
GO

CREATE FUNCTION [dbo].[ufn_VerifyProvisions]
(    
     @promotionId BIGINT
)
RETURNS BIT
AS
BEGIN
	
	DECLARE @TempTable TABLE (isValid BIT)
	DECLARE @isValidProvisions BIT = 1

	INSERT INTO 
		@TempTable
	SELECT 
		CASE 
			WHEN DiscountValue IS NOT NULL THEN 1
			ELSE 0
		END
	FROM 
		tPromoProvisions 
	WHERE 
		PromotionId = @promotionId
	
	SELECT @isValidProvisions = isValid FROM @TempTable WHERE isValid = 0

	RETURN @isValidProvisions

END
GO